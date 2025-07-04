using System;
using System.Collections.Generic;

namespace Fengb3.EasyCodeBuilder;

/// <summary>
/// Python 代码生成器 - 继承自 EasyCodeBuilder，提供 Python 特定的代码生成方法
/// </summary>
public class PythonCodeBuilder : CodeBuilder<PythonCodeBuilder>
{
    #region 构造函数 & 静态工厂方法 

    public PythonCodeBuilder() : base(" ", 4, ":", "", 1024)
    {
    }

    public PythonCodeBuilder(string indentChar, int indentCount, int initialCapacity = 1024)
        : base(indentChar, indentCount, ":", "", initialCapacity)
    {
    }

    /// <summary>
    /// 创建使用Tab缩进的Python代码构建器
    /// </summary>
    public static PythonCodeBuilder WithTabs(int tabCount = 1, int initialCapacity = 1024)
    {
        return new PythonCodeBuilder("\t", tabCount, initialCapacity);
    }

    /// <summary>
    /// 创建使用指定空格数缩进的Python代码构建器
    /// </summary>
    public static PythonCodeBuilder WithSpaces(int spaceCount = 4, int initialCapacity = 1024)
    {
        return new PythonCodeBuilder(" ", spaceCount, initialCapacity);
    }

    #endregion

    #region 基础扩展方法

    /// <summary>
    /// 条件添加
    /// </summary>
    public PythonCodeBuilder AppendWhen(bool condition, string line)
    {
        if (condition)
            AppendLine(line);
        return this;
    }

    /// <summary>
    /// 循环添加
    /// </summary>
    public PythonCodeBuilder AppendBatch<T>(IEnumerable<T> items, Func<PythonCodeBuilder, T, PythonCodeBuilder> func)
    {
        foreach (var item in items)
            func(this, item);
        return this;
    }

    #endregion

    #region Import 和模块

    /// <summary>
    /// 添加 import 语句
    /// </summary>
    public PythonCodeBuilder Import(params string[] modules)
    {
        foreach (var module in modules)
            AppendLine($"import {module}");
        return this;
    }

    /// <summary>
    /// 添加 from...import 语句
    /// </summary>
    public PythonCodeBuilder FromImport(string module, params string[] items)
    {
        if (items.Length == 1)
            AppendLine($"from {module} import {items[0]}");
        else
            AppendLine($"from {module} import {string.Join(", ", items)}");
        return this;
    }

    #endregion

    #region 类和函数定义

    /// <summary>
    /// 添加函数定义
    /// </summary>
    public PythonCodeBuilder Function(string name, Func<PythonCodeBuilder, PythonCodeBuilder> func,
                                    string parameters = "", string returnType = "")
    {
        var funcDeclaration = $"def {name}({parameters})";
        if (!string.IsNullOrEmpty(returnType))
            funcDeclaration += $" -> {returnType}";

        CodeBlock(funcDeclaration, func);
        return this;
    }

    /// <summary>
    /// 添加类定义
    /// </summary>
    /// <param name="name">类名</param>
    /// <param name="func">在类内执行的代码构建操作</param>
    /// <param name="baseClass">基类（可选）</param>
    /// <returns>当前构建器实例，支持链式调用</returns>
    public PythonCodeBuilder Class(string name, Func<PythonCodeBuilder, PythonCodeBuilder> func,
                                 string? baseClass = null)
    {
        var classDeclaration = $"class {name}";
        if (!string.IsNullOrEmpty(baseClass))
            classDeclaration += $"({baseClass})";

        CodeBlock(classDeclaration, func);
        return this;
    }

    /// <summary>
    /// 添加方法定义（带self参数）
    /// </summary>
    public PythonCodeBuilder Method(string name, Func<PythonCodeBuilder, PythonCodeBuilder> func,
                                  string parameters = "", string returnType = "")
    {
        var fullParams = string.IsNullOrEmpty(parameters) ? "self" : $"self, {parameters}";
        return Function(name, func, fullParams, returnType);
    }

    /// <summary>
    /// 添加静态方法
    /// </summary>
    public PythonCodeBuilder StaticMethod(string name, Func<PythonCodeBuilder, PythonCodeBuilder> func,
                                        string parameters = "", string returnType = "")
    {
        AppendLine("@staticmethod");
        return Function(name, func, parameters, returnType);
    }

    /// <summary>
    /// 添加类方法
    /// </summary>
    public PythonCodeBuilder ClassMethod(string name, Func<PythonCodeBuilder, PythonCodeBuilder> func,
                                       string parameters = "", string returnType = "")
    {
        AppendLine("@classmethod");
        var fullParams = string.IsNullOrEmpty(parameters) ? "cls" : $"cls, {parameters}";
        return Function(name, func, fullParams, returnType);
    }

    #endregion

    #region 控制结构

    /// <summary>
    /// 添加 if 语句
    /// </summary>
    public PythonCodeBuilder If(string condition, Func<PythonCodeBuilder, PythonCodeBuilder> func)
    {
        CodeBlock($"if {condition}", func);
        return this;
    }

    /// <summary>
    /// 添加 elif 语句
    /// </summary>
    public PythonCodeBuilder Elif(string condition, Func<PythonCodeBuilder, PythonCodeBuilder> func)
    {
        CodeBlock($"elif {condition}", func);
        return this;
    }

    /// <summary>
    /// 添加 else 语句
    /// </summary>
    public PythonCodeBuilder Else(Func<PythonCodeBuilder, PythonCodeBuilder> func)
    {
        CodeBlock("else", func);
        return this;
    }

    /// <summary>
    /// 添加 for 循环
    /// </summary>
    public PythonCodeBuilder For(string variable, string iterable, Func<PythonCodeBuilder, PythonCodeBuilder> func)
    {
        CodeBlock($"for {variable} in {iterable}", func);
        return this;
    }

    /// <summary>
    /// 添加 while 循环
    /// </summary>
    public PythonCodeBuilder While(string condition, Func<PythonCodeBuilder, PythonCodeBuilder> func)
    {
        CodeBlock($"while {condition}", func);
        return this;
    }

    /// <summary>
    /// 添加 try-except 语句
    /// </summary>
    public PythonCodeBuilder TryExcept(Func<PythonCodeBuilder, PythonCodeBuilder> tryfunc,
                                     string exceptionType = "Exception", string exceptionVar = "e",
                                     Func<PythonCodeBuilder, PythonCodeBuilder>? exceptfunc = null)
    {
        CodeBlock("try", cb => tryfunc((PythonCodeBuilder)cb));

        if (exceptfunc != null)
            CodeBlock($"except {exceptionType} as {exceptionVar}", cb => exceptfunc((PythonCodeBuilder)cb));
        else
            CodeBlock($"except {exceptionType} as {exceptionVar}", cb => cb.AppendLine("pass"));

        return this;
    }

    /// <summary>
    /// 添加 with 语句
    /// </summary>
    public PythonCodeBuilder With(string expression, Func<PythonCodeBuilder, PythonCodeBuilder> func, string? asVar = null)
    {
        var withStatement = $"with {expression}";
        if (!string.IsNullOrEmpty(asVar))
            withStatement += $" as {asVar}";

        CodeBlock(withStatement, func);
        return this;
    }

    #endregion
}