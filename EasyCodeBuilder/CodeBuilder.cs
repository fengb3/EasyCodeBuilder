using System;
using System.Text;

namespace Fengb3.EasyCodeBuilder;

/// <summary>
/// 代码构建器核心类 - 支持可配置的缩进方式和代码块符号
/// </summary>
public abstract class CodeBuilder<T>
    where T : CodeBuilder<T>
{
    #region 缩进配置和缓存

    private readonly string _indentChar;
    private readonly int _indentCount;

    // 代码块配置
    private readonly string _blockStart;
    private readonly string _blockEnd;

    // 缓存的缩进字符串数组
    private readonly string[] _indentCache;

    // 最大支持的缓存深度
    private const int MaxCacheDepth = 10;

    // 当前缩进相关字段
    private string _currentIndent = "";
    private int _depth = 0;

    #endregion

    #region 属性和嵌套类

    public T Self { get; set; } = null!;

    public int Depth
    {
        get => _depth;
        private set
        {
            _depth = Math.Max(0, value);
            _currentIndent = GetIndentString(_depth);
        }
    }

    public struct Indenter : IDisposable
    {
        private readonly CodeBuilder<T> _builder;

        public Indenter(CodeBuilder<T> builder)
        {
            _builder = builder;
            _builder.Depth++;
        }

        public void Dispose()
        {
            _builder.Depth--;
        }
    }

    public IDisposable Indent => new Indenter(this);

    #endregion

    #region 构造函数

    private readonly StringBuilder SB;

    /// <summary>
    /// 默认构造函数 - 使用2个空格缩进和C#风格大括号
    /// </summary>
    public CodeBuilder()
        : this(" ", 2, "{", "}", 1024)
    {
    }

    /// <summary>
    /// 完全配置的构造函数
    /// </summary>
    /// <param name="indentChar">缩进字符（" " 表示空格，"\t" 表示Tab）</param>
    /// <param name="indentCount">每级缩进的字符个数</param>
    /// <param name="blockStart">代码块开始符号</param>
    /// <param name="blockEnd">代码块结束符号</param>
    /// <param name="initialCapacity">初始容量</param>
    public CodeBuilder(
        string indentChar,
        int indentCount,
        string blockStart,
        string blockEnd,
        int initialCapacity = 1024
    )
    {
        _indentChar = indentChar ?? throw new ArgumentNullException(nameof(indentChar));
        _indentCount = Math.Max(1, indentCount);
        _blockStart = blockStart ?? "";
        _blockEnd = blockEnd ?? "";

        // 初始化缓存数组
        _indentCache = new string[MaxCacheDepth + 1];
        InitializeIndentCache();

        SB = new StringBuilder(initialCapacity);
        Depth = 0; // 初始化当前缩进
    }

    #endregion

    #region 缓存管理

    /// <summary>
    /// 初始化缓存数组
    /// </summary>
    private void InitializeIndentCache()
    {
        _indentCache[0] = "";

        // 为了性能，预先计算常用深度的缩进字符串
        for (int i = 1; i <= MaxCacheDepth; i++)
        {
            _indentCache[i] = new string(_indentChar[0], i * _indentCount);
        }
    }

    /// <summary>
    /// 获取指定深度的缩进字符串
    /// </summary>
    private string GetIndentString(int depth)
    {
        if (depth == 0)
            return "";

        // 使用缓存（大部分情况）
        if (depth <= MaxCacheDepth)
        {
            return _indentCache[depth];
        }

        // 超出缓存范围时动态创建
        return new string(_indentChar[0], depth * _indentCount);
    }

    #endregion

    #region 核心方法

    /// <summary>
    /// 添加文本
    /// </summary>
    /// <param name="text">文本</param>
    /// <returns>当前构建器实例</returns>
    public T Append(string text)
    {
        SB.Append(text);
        return Self;
    }

    /// <summary>
    /// 添加一行或多行代码（自动处理缩进）
    /// </summary>
    /// <param name="text">文本 (支持以 \n 为分隔符的多行文本)</param>
    /// <param name="lineSpliter"></param>
    /// <returns>当前构建器实例</returns>
    public T AppendLine(string text = "", char lineSpliter = '\n')
    {
        // 特殊处理空行，避免不必要的字符串操作
        if (string.IsNullOrEmpty(text))
        {
            SB.AppendLine();
            return Self;
        }

        // 使用传统字符串处理多行字符串
        int start = 0;

        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] != '\n') continue;

            // 找到换行符，处理当前行
            string line = text.Substring(start, i - start);

            // 移除行末的 \r（如果存在）
            if (line.Length > 0 && line[line.Length - 1] == '\r')
            {
                line = line.Substring(0, line.Length - 1);
            }

            AppendSingleLine(line);
            start = i + 1;
        }

        // 处理最后一行（如果没有以换行符结尾）
        if (start < text.Length)
        {
            string lastLine = text.Substring(start);
            AppendSingleLine(lastLine);
        }
        else if (start == text.Length && text.Length > 0 && text[text.Length - 1] == '\n')
        {
            // 如果字符串以换行符结尾，添加一个空行
            AppendSingleLine(string.Empty);
        }

        return Self;
    }

    /// <summary>
    /// 添加多行代码
    /// </summary>
    /// <param name="lines">多行代码</param>
    /// <returns>当前构建器实例</returns>
    public T AppendLines(params string[] lines)
    {
        foreach (var line in lines)
        {
            AppendLine(line);
        }

        return Self;
    }


    /// <summary>
    /// 添加单行代码（内部辅助方法）
    /// </summary>
    private void AppendSingleLine(string line)
    {
        // 特殊处理空行
        if (string.IsNullOrEmpty(line))
        {
            SB.AppendLine();
            return;
        }

        // 使用缓存的缩进字符串
        if (_currentIndent.Length == 0)
        {
            SB.Append(line).AppendLine();
        }
        else
        {
            SB.Append(_currentIndent).Append(line).AppendLine();
        }
    }

    // /// <summary>
    // /// 添加代码块（使用配置的代码块符号）
    // /// </summary>
    // public T CodeBlock(Func<T, T> func)
    // {
    //     // 将调用重定向到带前缀的版本，prefix 传递 null
    //     return CodeBlock(func, null);
    // }
    //
    // /// <summary>
    // /// 
    // /// </summary>
    // /// <param name="action"></param>
    // /// <returns></returns>
    // public T CodeBlock(Action<T> action)
    // {
    //     return CodeBlock((cb) =>
    //         {
    //             action(cb);
    //             return cb;
    //         }, null);
    // }

    /// <summary>
    /// 添加代码块
    /// </summary>
    protected T CodeBlock(Func<T, T> action, string? prefix = null)
    {
        string header;
        if (prefix is null)
        {
            // 无前缀版本：仅使用块开始符号
            header = _blockStart;
        }
        else
        {
            // 有前缀版本：组合前缀和块开始符号
            header = prefix + _blockStart;
        }

        if (!string.IsNullOrEmpty(header))
        {
            AppendLine(header);
        }

        using (Indent)
        {
            action(Self);
        }

        if (!string.IsNullOrEmpty(_blockEnd))
        {
            AppendLine(_blockEnd);
        }

        return Self;
    }

    /// <summary>
    /// 添加代码块
    /// </summary>
    protected T CodeBlock(Action<T> action, string? prefix = null)
    {
        return CodeBlock(
            cb =>
            {
                action(cb);
                return cb;
            }, prefix);
    }

    /// <summary>
    /// 字符串啦
    /// </summary>
    /// <returns></returns>
    public override string ToString() => SB.ToString();

    #endregion

    #region 重载运算符

    /// <summary>
    /// 重载运算符 +, 调用 AppendLine 方法
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    public static T operator +(CodeBuilder<T> builder, string text)
    {
        return builder.AppendLine(text);
    }

    /// <summary>
    /// 重载 运算符，调用 AppendLine 方法
    /// </summary>
    public static T operator <<(CodeBuilder<T> builder, string text)
    {
        return builder.AppendLine(text);
    }

    /// <summary>
    /// 重载 运算符，调用 Append 方法
    /// </summary>
    public static T operator <(CodeBuilder<T> builder, string text)
    {
        return builder.Append(text);
    }

    /// <summary>
    /// 重载 > 运算符（为了满足 C# 要求成对重载比较运算符）
    /// 这里简单返回 builder 本身，不执行任何操作
    /// </summary>
    public static T operator >(CodeBuilder<T> builder, string text)
    {
        return builder.Self;
    }

    #endregion
}