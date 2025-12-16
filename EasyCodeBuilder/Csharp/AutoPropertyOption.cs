using System.Collections.Generic;

namespace Fengb3.EasyCodeBuilder.Csharp;

public class AutoPropertyOption : CodeOption
{
    public ICollection<string> Keywords { get; set; } = [];

    public string Type { get; set; } = "";

    public string Name { get; set; } = "";
    
    public override CodeBuilder Build(CodeBuilder cb)
    {
        var keywords = string.Join(" ", Keywords);
        cb.AppendLine($"{keywords} {Type} {Name} {{ get; set; }}");
        return cb;
    }
}