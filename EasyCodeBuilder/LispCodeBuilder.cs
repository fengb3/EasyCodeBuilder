using System;

namespace Fengb3.EasyCodeBuilder;

/// <summary>
/// Lisp 代码生成器 - 继承自 EasyCodeBuilder，提供 Lisp 特定的代码生成方法
/// </summary>
public class LispCodeBuilder : CodeBuilder<LispCodeBuilder>
{
    #region 构造函数

    public LispCodeBuilder() : base(" ", 2, "(", ")", 1024)
    {
        Self = this;
    }
    
    public LispCodeBuilder(string indentChar, int indentCount, int initialCapacity = 1024) 
        : base(indentChar, indentCount, "(", ")", initialCapacity)
    {
        Self = this;
    }
    
    /// <summary>
    /// 创建使用Tab缩进的Lisp代码构建器
    /// </summary>
    public static LispCodeBuilder WithTabs(int tabCount = 1, int initialCapacity = 1024)
    {
        return new LispCodeBuilder("\t", tabCount, initialCapacity);
    }
    
    /// <summary>
    /// 创建使用指定空格数缩进的Lisp代码构建器
    /// </summary>
    public static LispCodeBuilder WithSpaces(int spaceCount = 2, int initialCapacity = 1024)
    {
        return new LispCodeBuilder(" ", spaceCount, initialCapacity);
    }

    #endregion

    #region Lisp 表达式

    /// <summary>
    /// 添加S表达式
    /// </summary>
    public LispCodeBuilder SExpression(string expression, Action<LispCodeBuilder>? action = null)
    {
        if (action == null)
        {
            AppendLine($"({expression})");
        }
        else
        {
            CodeBlock(cb => action(cb), expression);
        }
        return this;
    }
    
    /// <summary>
    /// 添加函数调用
    /// </summary>
    public LispCodeBuilder FunctionCall(string functionName, params string[] arguments)
    {
        if (arguments.Length == 0)
        {
            AppendLine($"({functionName})");
        }
        else
        {
            AppendLine($"({functionName} {string.Join(" ", arguments)})");
        }
        return this;
    }
    
    /// <summary>
    /// 添加变量定义
    /// </summary>
    public LispCodeBuilder DefVar(string name, string value)
    {
        AppendLine($"(defvar {name} {value})");
        return this;
    }

    #endregion

    #region 函数定义

    /// <summary>
    /// 添加函数定义
    /// </summary>
    public LispCodeBuilder DefFunction(string name, Action<LispCodeBuilder> action, 
                                     params string[] parameters)
    {
        var paramList = parameters.Length > 0 ? $"({string.Join(" ", parameters)})" : "()";
        CodeBlock(action, $"defun {name} {paramList}");
        return this;
    }
    
    /// <summary>
    /// 添加Lambda表达式
    /// </summary>
    public LispCodeBuilder Lambda(Action<LispCodeBuilder> action, params string[] parameters)
    {
        var paramList = parameters.Length > 0 ? $"({string.Join(" ", parameters)})" : "()";
        CodeBlock(action, $"lambda {paramList}");
        return this;
    }

    #endregion

    #region 控制结构

    /// <summary>
    /// 添加 if 表达式
    /// </summary>
    public LispCodeBuilder If(string condition, string thenExpr, string? elseExpr = null)
    {
        AppendLine(
            elseExpr == null ? 
                $"(if {condition} {thenExpr})" : 
                $"(if {condition} {thenExpr} {elseExpr})");
        return this;
    }
    
    /// <summary>
    /// 添加 cond 表达式
    /// </summary>
    public LispCodeBuilder Cond(Action<LispCodeBuilder> action)
    {
        CodeBlock(action, "cond");
        return this;
    }

    #endregion
} 