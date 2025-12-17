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

public static class AutoPropertyOptionExtensions
{
    public static AutoPropertyOption WithKeyword(this AutoPropertyOption prop, string keyword)
    {
        prop.Keywords.Add(keyword);
        return prop;
    }

    public static AutoPropertyOption WithType(this AutoPropertyOption prop, string type)
    {
        prop.Type = type;
        return prop;
    }

    public static AutoPropertyOption WithName(this AutoPropertyOption prop, string name)
    {
        prop.Name = name;
        return prop;
    }
}