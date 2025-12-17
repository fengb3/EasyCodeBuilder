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

    public ICollection<string> Keywords { get; set; } = new HashSet<string>();
    
    public ICollection<string> BaseTypes { get; set; } = new HashSet<string>();

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

/// <summary>
/// extensions for TypeOption
/// </summary>
public static class TypeOptionExtensions
{
    /// <summary>
    /// set name of type
    /// </summary>
    /// <param name="type"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static TypeOption WithName(this TypeOption type, string name)
    {
        type.Name = name;
        return type;
    }
    
    /// <summary>
    /// add keywords into type, this can be public, private, abstract, sealed, partial, etc. duplicates are ignored
    /// </summary>
    /// <param name="type"></param>
    /// <param name="keywords"></param>
    /// <returns></returns>
    public static TypeOption WithKeyword(this TypeOption type, params string[] keywords)
    {
        foreach (var keyword in keywords)
            type.Keywords.Add(keyword);
        return type;
    }
    
    /// <summary>
    /// add base types or interface to inherit or implement, duplicates are ignored
    /// </summary>
    /// <param name="type"></param>
    /// <param name="baseType"></param>
    /// <returns></returns>
    public static TypeOption Inherit(this TypeOption type, params string[] baseTypes)
    {
        foreach (var baseType in baseTypes)
            type.BaseTypes.Add(baseType);
        return type;
    }
    
    /// <summary>
    ///  add method into type
    /// </summary>
    /// <param name="type"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static TypeOption Method(this TypeOption type, Action<MethodOption> configure) 
        => type.AddConfiguredChild(configure);
    
    /// <summary>
    /// add auto property into type
    /// </summary>
    /// <param name="type"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static TypeOption AutoProperty(this TypeOption type, Action<AutoPropertyOption> configure)
        => type.AddConfiguredChild(configure);
}