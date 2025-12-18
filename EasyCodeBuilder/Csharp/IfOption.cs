using System;

namespace Fengb3.EasyCodeBuilder.Csharp;

/// <summary>
/// if 语句选项
/// </summary>
public class IfOption : CodeOption
{
    /// <summary>
    /// 条件表达式
    /// </summary>
    public string Condition { get; set; } = "";

    /// <summary>
    /// 构建代码
    /// </summary>
    /// <param name="cb">代码构建器</param>
    /// <returns>代码构建器</returns>
    public override CodeBuilder Build(CodeBuilder cb)
    {
        return cb.CodeBlock(OnChildren, $"if ({Condition})");   
    }
}

/// <summary>
/// else if 语句选项
/// </summary>
public class ElseIfOption : CodeOption
{
    /// <summary>
    /// 条件表达式
    /// </summary>
    public string Condition { get; set; } = "";

    /// <summary>
    /// 构建代码
    /// </summary>
    /// <param name="cb">代码构建器</param>
    /// <returns>代码构建器</returns>
    public override CodeBuilder Build(CodeBuilder cb)
    {
        return cb.CodeBlock(OnChildren, $"else if ({Condition})");
    }
}

/// <summary>
/// else 语句选项
/// </summary>
public class ElseOption : CodeOption
{
    /// <summary>
    /// 构建代码
    /// </summary>
    /// <param name="cb">代码构建器</param>
    /// <returns>代码构建器</returns>
    public override CodeBuilder Build(CodeBuilder cb)
    {
        return cb.CodeBlock(OnChildren, "else");
    }
}