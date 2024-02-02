using System;
using System.Collections.Generic;
using System.Text;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;
using Yaapii.Atoms;
using YamlDotNet.RepresentationModel;
using Yaapii.Atoms.List;
using gfs.YamlDotNet.YamlPath;
using System.Linq;
using System.IO;
using Yaapii.Atoms.Error;

namespace Yamly
{
    /// <summary>
    /// reads and navigates through the given yaml file
    /// </summary>
    public sealed class YamlOf : IYaml
    {
        private readonly IScalar<YamlNode> yamlNode;

        /// <summary>
        /// reads and navigates through the given yaml file
        /// </summary>
        public YamlOf(IInput yamlNode) : this(
            new TextOf(yamlNode)
        )
        { }

        /// <summary>
        /// reads and navigates through the given yaml file
        /// </summary>
        public YamlOf(IText yamlNode) : this(
            new ScalarOf<YamlNode>(() =>
            {
                var yaml = new YamlStream();
                yaml.Load(
                    new StringReader(yamlNode.AsString())
                );
                return yaml.Documents[0].RootNode;
            })
        )
        { }

        /// <summary>
        /// reads and navigates through the given yaml file
        /// </summary>
        public YamlOf(YamlNode yamlNode) : this(
            new ScalarOf<YamlNode>(yamlNode)
        )
        { }

        /// <summary>
        /// reads and navigates through the given yaml file
        /// </summary>
        public YamlOf(IScalar<YamlNode> yamlNode)
        {
            this.yamlNode = yamlNode;
        }

        public IList<IYaml> Nodes(string path)
        {
            return
                new Mapped<YamlNode, IYaml>(
                    node => new YamlOf(node),
                    this.yamlNode.Value().Query(path).ToList()
                );
        }

        public string Value(string path, string def = "")
        {
            return
                FirstOf.New(
                    Values(path),
                    def
                ).Value();
        }

        public IList<string> Values(string path)
        {
            return
                new Mapped<YamlNode, string>(node =>
                    {
                        new FailWhen(!(node is YamlScalarNode),
                            new InvalidOperationException($"Unable to read values from yaml with path '{path}', because the resulting nodes are yaml objects and not values.")
                        ).Go();
                        return node.ToString();
                    },
                    this.yamlNode.Value().Query(path).ToList()
                );
        }

        public YamlNode AsNode()
        {
            return this.yamlNode.Value();
        }
    }
}
