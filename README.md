[![EO principles respected here](http://www.elegantobjects.org/badge.svg)](http://www.elegantobjects.org)

Responsible: MSE

# Yamly
> Simple encapsulation of [YamlDotNet](https://github.com/aaubry/YamlDotNet) and the querying extension [YamlPathForYamlDotNet](https://github.com/gfs/YamlPathForYamlDotNet) to read and navigate yaml documents

## Usage
The IYAML interface provides method to read values from a yaml or naviagte to deeper nodes:
```csharp
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
```

To use it just call the implementation class ```new YamlOf(yourYamlContentText)``` and navigate with yaml path, which is documented [here](https://github.com/wwkimball/yamlpath/wiki/Segments-of-a-YAML-Path)

## Examples
yamlContent:
```yaml
root:
  object:
    name: test
    type: text
    value: hello
  objectlist:
    - name: test1
      type: text
      value: hello
    - name: test2
      type: number
      value: 2
  list:
    - test1
    - test2
    - test3

```

```csharp
[Fact]
public void ReadsValues()
{
    var yaml = new YamlOf(yamlContent);

    Assert.Equal(
        new ManyOf("test1", "test2", "test3"),
        yaml.Values("/root/list/*")
    );
}
```
