using System;
using System.Collections.Generic;
using System.Xml;
using YamlDotNet.RepresentationModel;

namespace Yamly
{
    /// <summary>
    /// methods for reading a yaml file
    /// </summary>
    public interface IYaml
    {
        /// <summary>
        /// lists all nodes at the given path
        /// </summary>
        IList<IYaml> Nodes(string path);

        /// <summary>
        /// the value at the given path
        /// </summary>
        string Value(string path, string def = "");

        /// <summary>
        /// the values at the given path
        /// </summary>
        IList<string> Values(string path);

        YamlNode AsNode();
    }
}
