using System;

namespace Fengb3.EasyCodeBuilder.Csharp;

/// <summary>
/// represents a for loop
/// </summary>
public class ForOption : CodeOption
{
    /// <summary>
    /// 初始化语句
    /// </summary>
    public string Initializer { get; set; } = "";
    
    /// <summary>
    /// 条件表达式
    /// </summary>
    public string Condition   { get; set; } = "";
    
    /// <summary>
    /// 迭代语句
    /// </summary>
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
        => cb.CodeBlock(OnChildren, $"while ({Condition})");
}

/// <summary>
/// represents a do-while loop
/// </summary>
public class DoWhileOption : CodeOption
{
    /// <summary>
    /// 条件表达式
    /// </summary>
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
    /// <summary>
    /// 变量类型
    /// </summary>
    public string VariableType { get; set; } = "";
    
    /// <summary>
    /// 变量名称
    /// </summary>
    public string VariableName { get; set; } = "";
    
    /// <summary>
    /// 集合表达式
    /// </summary>
    public string Collection   { get; set; } = "";

    /// <summary>
    /// 构建代码
    /// </summary>
    /// <param name="cb">代码构建器</param>
    /// <returns>代码构建器</returns>
    public override CodeBuilder Build(CodeBuilder cb)
        => cb.CodeBlock(OnChildren, $"foreach ({VariableType} {VariableName} in {Collection})");
}

/// <summary>
/// 循环选项扩展方法
/// </summary>
public static class LoopOptionExtensions
{
    /// <summary>
    /// set initializer of for loop
    /// </summary>
    /// <param name="forOption">The for-loop option.</param>
    /// <param name="initializer">The initializer expression (e.g. "int i = 0").</param>
    /// <returns>The for-loop option, for fluent chaining.</returns>
    public static ForOption WithInitializer(this ForOption forOption, string initializer)
    {
        forOption.Initializer = initializer;
        return forOption;
    }

    /// <summary>
    /// set condition of for or while loop
    /// </summary>
    /// <param name="loopOption">The for-loop option.</param>
    /// <param name="condition">The condition expression (e.g. "i &lt; 10").</param>
    /// <returns>The for-loop option, for fluent chaining.</returns>
    public static ForOption WithCondition(this ForOption loopOption, string condition)
    {
        loopOption.Condition = condition;
        return loopOption;
    }

    /// <summary>
    /// set iterator of for loop
    /// </summary>
    /// <param name="forOption">The for-loop option.</param>
    /// <param name="iterator">The iterator expression (e.g. "i++").</param>
    /// <returns>The for-loop option, for fluent chaining.</returns>
    public static ForOption WithIterator(this ForOption forOption, string iterator)
    {
        forOption.Iterator = iterator;
        return forOption;
    }

    /// <summary>
    /// set condition of while loop
    /// </summary>
    /// <param name="whileOption">The while-loop option.</param>
    /// <param name="condition">The condition expression.</param>
    /// <returns>The while-loop option, for fluent chaining.</returns>
    public static WhileOption WithCondition(this WhileOption whileOption, string condition)
    {
        whileOption.Condition = condition;
        return whileOption;
    }


    /// <summary>
    /// set condition of do-while loop
    /// </summary>
    /// <param name="doWhileOption">The do-while loop option.</param>
    /// <param name="condition">The condition expression.</param>
    /// <returns>The do-while loop option, for fluent chaining.</returns>
    public static DoWhileOption WithCondition(this DoWhileOption doWhileOption, string condition)
    {
        doWhileOption.Condition = condition;
        return doWhileOption;
    }

    /// <summary>
    /// set variable type, name and collection of foreach loop
    /// </summary>
    /// <param name="foreachOption">The foreach-loop option.</param>
    /// <param name="variableType">The variable type (e.g. "var").</param>
    /// <param name="variableName">The variable name.</param>
    /// <param name="collection">The collection expression to iterate over.</param>
    /// <returns>The foreach-loop option, for fluent chaining.</returns>
    public static ForeachOption WithVariable(this ForeachOption foreachOption, string variableType, string variableName, string collection)
    {
        foreachOption.VariableType = variableType;
        foreachOption.VariableName = variableName;
        foreachOption.Collection   = collection;
        return foreachOption;
    }
}