using System;
using System.Collections.Generic;

namespace Fengb3.EasyCodeBuilder.Csharp;

public class TypeOption : CodeOption
{
    public enum Type
    {
        Class,
        Struct,
        Interface,
        Enum,
        Record
    }

    public Type TypeKind { get; set; } = Type.Class;

    public ICollection<string> Keywords { get; set; } = [];
    
    public ICollection<string> BaseTypes { get; set; } = [];

    public string Name { get; set; }
    
    public override CodeBuilder Build(CodeBuilder cb)
    {
        var keywords    = string.Join(" ", Keywords);
        var typeKeyword = TypeKind.ToString().ToLower();
        if (BaseTypes.Count > 0)
        {
            Name += " : " + string.Join(", ", BaseTypes);
        }
            
        // cb.AppendLines($"{keywords} {typeKeyword} {Name}", "{");;
        
        cb.CodeBlock(OnChildren, $"{keywords} {typeKeyword} {Name}");
        return cb;
    }
}