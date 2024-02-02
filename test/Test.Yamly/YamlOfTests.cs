using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.IO;
using Yamly;

namespace Yamly.Test
{
    public class YamlOfTests
    {
        [Fact]
        public void ReadsNodes()
        {
            var yaml =
                new YamlOf(
                    new ResourceOf(
                        "test1.yaml",
                        typeof(YamlOfTests)
                    )
                );

            Assert.Equal(
                2,
                yaml.Nodes("/root/objectlist/*").Count
            );
        }

        [Fact]
        public void ReadsContentFromNodes()
        {
            var yaml =
                new YamlOf(
                    new ResourceOf(
                        "test1.yaml",
                        typeof(YamlOfTests)
                    )
                );

            Assert.Equal(
                "hello",
                yaml.Nodes("/root/objectlist/*")[0].Value("/value")
            );
        }

        [Fact]
        public void ReadsValue()
        {
            var yaml =
                new YamlOf(
                    new ResourceOf(
                        "test1.yaml",
                        typeof(YamlOfTests)
                    )
                );

            Assert.Equal(
                "test",
                yaml.Value("/root/object/name")
            );
        }

        [Fact]
        public void ReadsValues()
        {
            var yaml =
                new YamlOf(
                    new ResourceOf(
                        "test1.yaml",
                        typeof(YamlOfTests)
                    )
                );

            Assert.Equal(
                new ManyOf("test1", "test2", "test3"),
                yaml.Values("/root/list/*")
            );
        }

        [Fact]
        public void RejectsObjectsAsNodes()
        {
            var yaml =
                new YamlOf(
                    new ResourceOf(
                        "test1.yaml",
                        typeof(YamlOfTests)
                    )
                );

            Assert.Throws<InvalidOperationException>(() =>
                yaml.Values("/root/objectlist/*").Count
            );
        }
    }
}
