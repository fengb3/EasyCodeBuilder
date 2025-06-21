using System;
using System.Text;

namespace Fengb3.EasyCodeBuilder;

/// <summary>
/// 代码构建器核心类 - 支持可配置的缩进方式和代码块符号
/// </summary>
public abstract class CodeBuilder<T> where T : CodeBuilder<T>
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
    private const int MaxCacheDepth = 20;

    // 当前缩进相关字段
    private string _currentIndent = "";
    private int _depth = 0;

    #endregion

    #region 属性和嵌套类

    public T Self {get; set;} = null!;

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
    public CodeBuilder() : this(" ", 2, "{", "}", 1024)
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
    public CodeBuilder(string indentChar, int indentCount, string blockStart, string blockEnd, int initialCapacity = 1024)
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
        if (depth == 0) return "";

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

    public T Append(string text)
    {
        SB.Append(text);
        return Self;
    }

    /// <summary>
    /// 添加一行或多行代码（自动处理缩进）
    /// </summary>
    public T AppendLine(string text = "")
    {
        // 特殊处理空行，避免不必要的字符串操作
        if (string.IsNullOrEmpty(text))
        {
            SB.AppendLine();
            return Self;
        }

        // 使用 Span 高效处理多行字符串
        ReadOnlySpan<char> span = text.AsSpan();


        int start = 0;
        
        for (int i = 0; i < span.Length; i++)
        {
            if (span[i] == '\n')
            {
                // 找到换行符，处理当前行
                ReadOnlySpan<char> line = span.Slice(start, i - start);
                
                // 移除行末的 \r（如果存在）
                if (line.Length > 0 && line[line.Length - 1] == '\r')
                {
                    line = line.Slice(0, line.Length - 1);
                }
                
                AppendSingleLine(line);
                start = i + 1;
            }
        }
        
        // 处理最后一行（如果没有以换行符结尾）
        if (start < span.Length)
        {
            ReadOnlySpan<char> lastLine = span.Slice(start);
            AppendSingleLine(lastLine);
        }
        else if (start == span.Length && span.Length > 0 && span[span.Length - 1] == '\n')
        {
            // 如果字符串以换行符结尾，添加一个空行
            AppendSingleLine(ReadOnlySpan<char>.Empty);
        }

        return Self;
    }

    /// <summary>
    /// 添加单行代码（内部辅助方法）
    /// </summary>
    private void AppendSingleLine(ReadOnlySpan<char> line)
    {
        // 特殊处理空行
        if (line.IsEmpty)
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

    /// <summary>
    /// 添加代码块（使用配置的代码块符号）
    /// </summary>
    protected T CodeBlock(Func<T, T> func)
    {
        // 将调用重定向到带前缀的版本，prefix 传递 null
        return CodeBlock(null, func);
    }

    protected T CodeBlock(Action<T> action)
    {
        return CodeBlock(null, (cb) =>
        {
            action(cb);
            return cb;
        });
    }

    // /// <summary>
    // /// 添加代码块（泛型版本，支持子类直接调用）
    // /// </summary>
    // /// <typeparam name="T">构建器类型</typeparam>
    // /// <param name="action">在代码块内执行的操作</param>
    // /// <returns>当前构建器实例</returns>
    // protected T CodeBlock<T>(Action<T> action) where T : EasyCodeBuilder
    // {
    //     return CodeBlock<T>(null, action);
    // }

    /// <summary>
    /// 添加带前缀的代码块（如 if 语句等）
    /// </summary>
    protected T CodeBlock(string? prefix, Func<T, T> action)
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

    protected T CodeBlock(string? prefix, Action<T> action)	
    {
        return CodeBlock(prefix, (cb) =>
        {
            action(cb);
            return cb;
        });
    }

    // /// <summary>
    // /// 添加带前缀的代码块（泛型版本，支持子类直接调用）
    // /// </summary>
    // /// <typeparam name="T">构建器类型</typeparam>
    // /// <param name="prefix">代码块前缀</param>
    // /// <param name="action">在代码块内执行的操作</param>
    // /// <returns>当前构建器实例</returns>
    // protected T CodeBlock<T>(string? prefix, Action<T> action) where T : EasyCodeBuilder
    // {
    //     string header;
    //     if (prefix is null)
    //     {
    //         // 无前缀版本：仅使用块开始符号
    //         header = _blockStart;
    //     }
    //     else
    //     {
    //         // 有前缀版本：组合前缀和块开始符号
    //         header = prefix + _blockStart;
    //     }

    //     if (!string.IsNullOrEmpty(header))
    //     {
    //         AppendLine(header);
    //     }

    //     using (Indent)
    //     {
    //         action((T)this);
    //     }

    //     if (!string.IsNullOrEmpty(_blockEnd))
    //     {
    //         AppendLine(_blockEnd);
    //     }

    //     return (T)this;
    // }

    public override string ToString() => SB.ToString();

    #endregion

    #region 重载运算符

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