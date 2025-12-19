using CodeBuilder.Demo.SourceGenerator.Attributes;

namespace CodeBuilder.Demo.SourceGenerator.Sample;

[Builder]
public class BuilderExample
{
    public string Name { get; set; }
    public int Age { get; set; }
    
    [BuilderIgnore]
    public string Gender { get; set; }
}