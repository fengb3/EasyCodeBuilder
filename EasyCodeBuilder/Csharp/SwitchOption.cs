using System;

namespace Fengb3.EasyCodeBuilder.Csharp;

public class SwitchOption : CodeOption
{
    public string Expression { get; set; } = "";

    public override CodeBuilder Build(CodeBuilder cb)
        => cb.CodeBlock(OnChildren, $"switch ({Expression})");
}

public class CaesOption : CodeOption
{
    public string Value { get; set; } = "";

    public override CodeBuilder Build(CodeBuilder cb)
        => cb.CodeBlock(OnChildren, $"case {Value}:");
}

public class DefaultCaseOption : CodeOption
{
    public override CodeBuilder Build(CodeBuilder cb)
        => cb.CodeBlock(OnChildren, "default:");
}

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
        => switchOption.AddChildByConfiguration(configure);

    /// <summary>
    /// add default case into switch
    /// </summary>
    /// <param name="switchOption"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static SwitchOption Default(this SwitchOption switchOption, Action<DefaultCaseOption> configure)
        => switchOption.AddChildByConfiguration(configure);
}