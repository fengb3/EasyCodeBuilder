using System;

namespace Fengb3.EasyCodeBuilder.Csharp;

/// <summary>
/// switch 语句选项
/// </summary>
public class SwitchOption : CodeOption
{
    /// <summary>
    /// 表达式
    /// </summary>
    public string Expression { get; set; } = "";

    /// <summary>
    /// 构建代码
    /// </summary>
    /// <param name="cb">代码构建器</param>
    /// <returns>代码构建器</returns>
    public override CodeBuilder Build(CodeBuilder cb)
        => cb.CodeBlock(OnChildren, $"switch ({Expression})");
}

/// <summary>
/// case 语句选项
/// </summary>
public class CaesOption : CodeOption
{
    /// <summary>
    /// 值
    /// </summary>
    public string Value { get; set; } = "";

    /// <summary>
    /// 构建代码
    /// </summary>
    /// <param name="cb">代码构建器</param>
    /// <returns>代码构建器</returns>
    public override CodeBuilder Build(CodeBuilder cb)
        => cb.CodeBlock(OnChildren, $"case {Value}:");
}

/// <summary>
/// default 语句选项
/// </summary>
public class DefaultCaseOption : CodeOption
{
    /// <summary>
    /// 构建代码
    /// </summary>
    /// <param name="cb">代码构建器</param>
    /// <returns>代码构建器</returns>
    public override CodeBuilder Build(CodeBuilder cb)
        => cb.CodeBlock(OnChildren, "default:");
}

/// <summary>
/// switch 选项扩展方法
/// </summary>
public static class SwitchOptionExtensions
{
    /// <summary>
    /// set expression of switch
    /// </summary>
    /// <param name="switchOption"></param>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static SwitchOption WithExpression(this SwitchOption switchOption, string expression)
    {
        switchOption.Expression = expression;
        return switchOption;
    }

    /// <summary>
    /// add case into switch
    /// </summary>
    /// <param name="switchOption"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static SwitchOption Case(this SwitchOption switchOption, Action<CaesOption> configure)
        => switchOption.AddChild(configure);

    /// <summary>
    /// add default case into switch
    /// </summary>
    /// <param name="switchOption"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static SwitchOption Default(this SwitchOption switchOption, Action<DefaultCaseOption> configure)
        => switchOption.AddChild(configure);
}