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
    /// <param name="cb">代码构建器</param>
    /// <returns>代码构建器</returns>
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
    /// <param name="method">方法选项</param>
    /// <param name="keyword">关键字</param>
    /// <returns>方法选项</returns>
    public static MethodOption WithKeyword(this MethodOption method, string keyword)
    {
        method.Keywords.Add(keyword);
        return method;
    }
    
    /// <summary>
    /// set name of method
    /// </summary>
    /// <param name="method">方法选项</param>
    /// <param name="name">方法名称</param>
    /// <returns>方法选项</returns>
    public static MethodOption WithName(this MethodOption method, string name)
    {
        method.Name = name;
        return method;
    }
    
    /// <summary>
    /// set return type of method
    /// </summary>
    /// <param name="method">方法选项</param>
    /// <param name="returnType">返回类型</param>
    /// <returns>方法选项</returns>
    public static MethodOption WithReturnType(this MethodOption method, string returnType)
    {
        method.ReturnType = returnType;
        return method;
    }
    
    /// <summary>
    /// set parameters of method
    /// </summary>
    /// <param name="method">方法选项</param>
    /// <param name="parameter">参数列表</param>
    /// <returns>方法选项</returns>
    public static MethodOption WithParameters(this MethodOption method, params string[] parameter)
    {
        method.Parameters = parameter;
        return method;
    }
    
    /// <summary>
    /// add if statement
    /// </summary>
    /// <param name="method">方法选项</param>
    /// <param name="configure">if 语句配置委托</param>
    /// <returns>方法选项</returns>
    public static MethodOption If(this MethodOption method, Action<IfOption> configure)
        => method.AddChild(configure);
    
    /// <summary>
    /// add else statement
    /// </summary>
    /// <param name="method">方法选项</param>
    /// <param name="configure">else 语句配置委托</param>
    /// <returns>方法选项</returns>
    public static MethodOption Else(this MethodOption method, Action<ElseOption> configure)
        => method.AddChild(configure);
    
    /// <summary>
    /// add else if statement
    /// </summary>
    /// <param name="method">方法选项</param>
    /// <param name="configure">else if 语句配置委托</param>
    /// <returns>方法选项</returns>
    public static MethodOption ElseIf(this MethodOption method, Action<ElseIfOption> configure)
        => method.AddChild(configure);
    
    /// <summary>
    /// add for loop
    /// </summary>
    /// <param name="method">方法选项</param>
    /// <param name="configure">for 循环配置委托</param>
    /// <returns>方法选项</returns>
    public static MethodOption For(this MethodOption method, Action<ForOption> configure)
        => method.AddChild(configure);
    
    /// <summary>
    /// add while loop
    /// </summary>
    /// <param name="method">方法选项</param>
    /// <param name="configure">while 循环配置委托</param>
    /// <returns>方法选项</returns>
    public static MethodOption While(this MethodOption method, Action<WhileOption> configure)
        => method.AddChild(configure);
    
    /// <summary>
    /// add do while loop
    /// </summary>
    /// <param name="method">方法选项</param>
    /// <param name="configure">do while 循环配置委托</param>
    /// <returns>方法选项</returns>
    public static MethodOption DoWhile(this MethodOption method, Action<DoWhileOption> configure)
        => method.AddChild(configure);
    
    /// <summary>
    /// add foreach loop
    /// </summary>
    /// <param name="method">方法选项</param>
    /// <param name="configure">foreach 循环配置委托</param>
    /// <returns>方法选项</returns>
    public static MethodOption Foreach(this MethodOption method, Action<ForeachOption> configure)
        => method.AddChild(configure);
    
    /// <summary>
    /// add switch statement
    /// </summary>
    /// <param name="method">方法选项</param>
    /// <param name="configure">switch 语句配置委托</param>
    /// <returns>方法选项</returns>
    public static MethodOption Switch(this MethodOption method, Action<SwitchOption> configure)
        => method.AddChild(configure);
}