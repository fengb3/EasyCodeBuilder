using System;

namespace Fengb3.EasyCodeBuilder.Csharp;

/// <summary>
/// represents a for loop
/// </summary>
public class ForOption : CodeOption
{
    public string Initializer { get; set; } = "";
    public string Condition   { get; set; } = "";
    public string Iterator    { get; set; } = "";

    /// <inheritdoc />
    public override CodeBuilder Build(CodeBuilder cb)
        => cb.CodeBlock(OnChildren, $"for ({Initializer}; {Condition}; {Iterator})");
}

/// <summary>
/// represents a while loop
/// </summary>
public class WhileOption : CodeOption
{
    public string Condition { get; set; } = "";

    public override CodeBuilder Build(CodeBuilder cb)
        => cb.CodeBlock(OnChildren, $"while ({Condition})");
}

/// <summary>
/// represents a do-while loop
/// </summary>
public class DoWhileOption : CodeOption
{
    public string Condition { get; set; } = "";

    /// <inheritdoc />
    public override CodeBuilder Build(CodeBuilder cb)
        => cb.CodeBlock(OnChildren, "do", $"while ({Condition});");
}

/// <summary>
/// represents a foreach loop
/// </summary>
public class ForeachOption : CodeOption
{
    public string VariableType { get; set; } = "";
    public string VariableName { get; set; } = "";
    public string Collection   { get; set; } = "";

    public override CodeBuilder Build(CodeBuilder cb)
        => cb.CodeBlock(OnChildren, $"foreach ({VariableType} {VariableName} in {Collection})");
}

public static class LoopOptionExtensions
{
    /// <summary>
    /// set initializer of for loop
    /// </summary>
    /// <param name="forOption"></param>
    /// <param name="initializer"></param>
    /// <returns></returns>
    public static ForOption WithInitializer(this ForOption forOption, string initializer)
    {
        forOption.Initializer = initializer;
        return forOption;
    }

    /// <summary>
    /// set condition of for or while loop
    /// </summary>
    /// <param name="loopOption"></param>
    /// <param name="condition"></param>
    /// <returns></returns>
    public static ForOption WithCondition(this ForOption loopOption, string condition)
    {
        loopOption.Condition = condition;
        return loopOption;
    }

    /// <summary>
    /// set iterator of for loop
    /// </summary>
    /// <param name="forOption"></param>
    /// <param name="iterator"></param>
    /// <returns></returns>
    public static ForOption WithIterator(this ForOption forOption, string iterator)
    {
        forOption.Iterator = iterator;
        return forOption;
    }

    /// <summary>
    /// set condition of while loop
    /// </summary>
    /// <param name="whileOption"></param>
    /// <param name="condition"></param>
    /// <returns></returns>
    public static WhileOption WithCondition(this WhileOption whileOption, string condition)
    {
        whileOption.Condition = condition;
        return whileOption;
    }


    /// <summary>
    /// set condition of do-while loop
    /// </summary>
    /// <param name="doWhileOption"></param>
    /// <param name="condition"></param>
    /// <returns></returns>
    public static DoWhileOption WithCondition(this DoWhileOption doWhileOption, string condition)
    {
        doWhileOption.Condition = condition;
        return doWhileOption;
    }

    /// <summary>
    /// set variable type, name and collection of foreach loop
    /// </summary>
    /// <param name="foreachOption"></param>
    /// <param name="variableType"></param>
    /// <param name="variableName"></param>
    /// <param name="collection"></param>
    /// <returns></returns>
    public static ForeachOption WithVariable(this ForeachOption foreachOption, string variableType, string variableName, string collection)
    {
        foreachOption.VariableType = variableType;
        foreachOption.VariableName = variableName;
        foreachOption.Collection   = collection;
        return foreachOption;
    }
}