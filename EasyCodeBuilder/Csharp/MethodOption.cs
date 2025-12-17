using System;
using System.Collections.Generic;

namespace Fengb3.EasyCodeBuilder.Csharp;

/// <summary>
/// Option for building a method
/// </summary>
public class MethodOption : CodeOption
{
    /// <summary>
    /// Keywords for the method (e.g. public, static, async)
    /// </summary>
    public ICollection<string> Keywords { get; set; } = new HashSet<string>();

    /// <summary>
    /// Return type of the method
    /// </summary>
    public string ReturnType { get; set; } = "void";

    /// <summary>
    /// Name of the method
    /// </summary>
    public string Name { get; set; } = "";
    
    /// <summary>
    /// Parameters of the method
    /// </summary>
    public ICollection<string> Parameters { get; set; } = new HashSet<string>();

    /// <summary>
    /// Build the method code
    /// </summary>
    /// <param name="cb"></param>
    /// <returns></returns>
    public override CodeBuilder Build(CodeBuilder cb)
    {
        var keywords   = string.Join(" ", Keywords);
        var parameters = string.Join(", ", Parameters);
        
        cb.CodeBlock(OnChildren, $"{keywords} {ReturnType} {Name}({parameters})");
        
        return cb;
    }
}

/// <summary>
/// Extension methods for MethodOption
/// </summary>
public static class MethodOptionExtensions
{
    /// <summary>
    /// add keyword into method, this can be public, private, protected, internal, static, virtual, override, async, etc. duplicates are ignored
    /// </summary>
    /// <param name="method"></param>
    /// <param name="keyword"></param>
    /// <returns></returns>
    public static MethodOption WithKeyword(this MethodOption method, string keyword)
    {
        method.Keywords.Add(keyword);
        return method;
    }
    
    /// <summary>
    /// set name of method
    /// </summary>
    /// <param name="method"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static MethodOption WithName(this MethodOption method, string name)
    {
        method.Name = name;
        return method;
    }
    
    /// <summary>
    /// set return type of method
    /// </summary>
    /// <param name="method"></param>
    /// <param name="returnType"></param>
    /// <returns></returns>
    public static MethodOption WithReturnType(this MethodOption method, string returnType)
    {
        method.ReturnType = returnType;
        return method;
    }
    
    /// <summary>
    /// set parameters of method
    /// </summary>
    /// <param name="method"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public static MethodOption WithParameters(this MethodOption method, params string[] parameter)
    {
        method.Parameters = parameter;
        return method;
    }
    
    /// <summary>
    /// add if statement
    /// </summary>
    /// <param name="method"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static MethodOption If(this MethodOption method, Action<IfOption> configure)
        => method.AddConfiguredChild(configure);
    
    /// <summary>
    /// add else statement
    /// </summary>
    /// <param name="method"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static MethodOption Else(this MethodOption method, Action<ElseOption> configure)
        => method.AddConfiguredChild(configure);
    
    /// <summary>
    /// add else if statement
    /// </summary>
    /// <param name="method"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static MethodOption ElseIf(this MethodOption method, Action<ElseIfOption> configure)
        => method.AddConfiguredChild(configure);
    
    /// <summary>
    /// add for loop
    /// </summary>
    /// <param name="method"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static MethodOption For(this MethodOption method, Action<ForOption> configure)
        => method.AddConfiguredChild(configure);
    
    /// <summary>
    /// add while loop
    /// </summary>
    /// <param name="method"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static MethodOption While(this MethodOption method, Action<WhileOption> configure)
        => method.AddConfiguredChild(configure);
    
    /// <summary>
    /// add do while loop
    /// </summary>
    /// <param name="method"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static MethodOption DoWhile(this MethodOption method, Action<DoWhileOption> configure)
        => method.AddConfiguredChild(configure);
    
    /// <summary>
    /// add foreach loop
    /// </summary>
    /// <param name="method"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static MethodOption Foreach(this MethodOption method, Action<ForeachOption> configure)
        => method.AddConfiguredChild(configure);
}