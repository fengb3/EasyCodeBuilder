using System;

namespace Fengb3.EasyCodeBuilder.Csharp;

/// <summary>
/// try 语句选项
/// </summary>
public class TryOption : CodeOption
{
    private CodeRenderFragment? _clauses;

    /// <summary>
    /// 构建代码
    /// </summary>
    /// <param name="cb">代码构建器</param>
    /// <returns>代码构建器</returns>
    public override CodeBuilder Build(CodeBuilder cb)
    {
        BeforeChildren?.Invoke(cb);
        cb.CodeBlock(OnChildren, "try");
        _clauses?.Invoke(cb);
        return cb;
    }

    internal void AddClause(CodeRenderFragment clause)
    {
        _clauses += clause;
    }
}

/// <summary>
/// catch 语句选项
/// </summary>
public class CatchOption : CodeOption
{
    /// <summary>
    /// 捕获的异常类型（为空时表示裸 catch）
    /// </summary>
    public string ExceptionType { get; set; } = "";

    /// <summary>
    /// 异常变量名（可为空）
    /// </summary>
    public string VariableName { get; set; } = "";

    /// <summary>
    /// when 过滤条件（可为空）
    /// </summary>
    public string WhenFilter { get; set; } = "";

    /// <summary>
    /// 构建代码
    /// </summary>
    /// <param name="cb">代码构建器</param>
    /// <returns>代码构建器</returns>
    public override CodeBuilder Build(CodeBuilder cb)
    {
        string header;
        if (string.IsNullOrEmpty(ExceptionType))
        {
            header = "catch";
        }
        else if (string.IsNullOrEmpty(VariableName))
        {
            header = $"catch ({ExceptionType})";
        }
        else
        {
            header = $"catch ({ExceptionType} {VariableName})";
        }

        if (!string.IsNullOrEmpty(WhenFilter))
        {
            header += $" when ({WhenFilter})";
        }

        return cb.CodeBlock(OnChildren, header);
    }
}

/// <summary>
/// finally 语句选项
/// </summary>
public class FinallyOption : CodeOption
{
    /// <summary>
    /// 构建代码
    /// </summary>
    /// <param name="cb">代码构建器</param>
    /// <returns>代码构建器</returns>
    public override CodeBuilder Build(CodeBuilder cb)
        => cb.CodeBlock(OnChildren, "finally");
}

/// <summary>
/// try-catch-finally 选项扩展方法
/// </summary>
public static class TryCatchFinallyOptionExtensions
{
    /// <summary>
    /// add a catch clause to the try block
    /// </summary>
    /// <param name="tryOption">try 语句选项</param>
    /// <param name="configure">catch 语句配置委托</param>
    /// <returns>try 语句选项</returns>
    public static TryOption Catch(this TryOption tryOption, Action<CatchOption> configure)
    {
        var catchOption = new CatchOption();
        configure(catchOption);
        tryOption.AddClause(catchOption.Build);
        return tryOption;
    }

    /// <summary>
    /// add a finally clause to the try block
    /// </summary>
    /// <param name="tryOption">try 语句选项</param>
    /// <param name="configure">finally 语句配置委托</param>
    /// <returns>try 语句选项</returns>
    public static TryOption Finally(this TryOption tryOption, Action<FinallyOption> configure)
    {
        var finallyOption = new FinallyOption();
        configure(finallyOption);
        tryOption.AddClause(finallyOption.Build);
        return tryOption;
    }

    /// <summary>
    /// set exception type of catch clause
    /// </summary>
    /// <param name="catchOption">catch 语句选项</param>
    /// <param name="exceptionType">异常类型</param>
    /// <returns>catch 语句选项</returns>
    public static CatchOption WithExceptionType(this CatchOption catchOption, string exceptionType)
    {
        catchOption.ExceptionType = exceptionType;
        return catchOption;
    }

    /// <summary>
    /// set variable name of catch clause
    /// </summary>
    /// <param name="catchOption">catch 语句选项</param>
    /// <param name="variableName">异常变量名</param>
    /// <returns>catch 语句选项</returns>
    public static CatchOption WithVariableName(this CatchOption catchOption, string variableName)
    {
        catchOption.VariableName = variableName;
        return catchOption;
    }

    /// <summary>
    /// set when filter of catch clause
    /// </summary>
    /// <param name="catchOption">catch 语句选项</param>
    /// <param name="whenFilter">过滤条件表达式</param>
    /// <returns>catch 语句选项</returns>
    public static CatchOption WithWhenFilter(this CatchOption catchOption, string whenFilter)
    {
        catchOption.WhenFilter = whenFilter;
        return catchOption;
    }
}
