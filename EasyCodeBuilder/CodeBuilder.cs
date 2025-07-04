using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text;

namespace Fengb3.EasyCodeBuilder;

/// <summary>
/// 代码构建器核心类 - 支持可配置的缩进方式和代码块符号
/// 高性能版本：使用数组池、优化的字符串操作和缓存机制
/// </summary>
public abstract class CodeBuilder<T> where T : CodeBuilder<T>
{
    #region 缓存配置和性能优化

    private readonly string _indentChar;
    private readonly int _indentCount;

    // 代码块配置
    private readonly string _blockStart;
    private readonly string _blockEnd;

    // 缓存的缩进字符串数组 - 扩展到更大的缓存
    private readonly string[] _indentCache;

    // 最大支持的缓存深度 - 增加到50以覆盖更多场景
    private const int MaxCacheDepth = 50;

    // 当前缩进相关字段
    private string _currentIndent = "";
    private int _depth = 0;

    // 性能计数器和统计
    private int _estimatedFinalSize;
    private int _lineCount;

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

    /// <summary>
    /// 获取当前代码行数（性能统计）
    /// </summary>
    public int LineCount => _lineCount;

    /// <summary>
    /// 获取估计的最终大小（用于性能分析）
    /// </summary>
    public int EstimatedSize => _estimatedFinalSize;

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
    public CodeBuilder() : this(" ", 2, "{", "}", 2048) // 增加默认初始容量
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
    public CodeBuilder(string indentChar, int indentCount, string blockStart, string blockEnd, int initialCapacity = 2048)
    {
        _indentChar = indentChar ?? throw new ArgumentNullException(nameof(indentChar));
        _indentCount = Math.Max(1, indentCount);
        _blockStart = blockStart ?? "";
        _blockEnd = blockEnd ?? "";

        // 初始化缓存数组
        _indentCache = new string[MaxCacheDepth + 1];
        InitializeIndentCache();

        // 使用更智能的初始容量计算
        var smartCapacity = Math.Max(initialCapacity, EstimateInitialCapacity());
        SB = new StringBuilder(smartCapacity);
        _estimatedFinalSize = smartCapacity;
        
        Depth = 0; // 初始化当前缩进
    }

    #endregion

    #region 缓存管理和性能优化

    /// <summary>
    /// 初始化缓存数组 - 优化版本
    /// </summary>
    private void InitializeIndentCache()
    {
        _indentCache[0] = "";

        // 为了性能，预先计算常用深度的缩进字符串
        // 使用更高效的字符串创建方式
        if (_indentChar.Length == 1)
        {
            char indentCharacter = _indentChar[0];
            for (int i = 1; i <= MaxCacheDepth; i++)
            {
                _indentCache[i] = new string(indentCharacter, i * _indentCount);
            }
        }
        else
        {
            // 处理多字符缩进的情况
            for (int i = 1; i <= MaxCacheDepth; i++)
            {
                var sb = new StringBuilder(i * _indentCount * _indentChar.Length);
                for (int j = 0; j < i * _indentCount; j++)
                {
                    sb.Append(_indentChar);
                }
                _indentCache[i] = sb.ToString();
            }
        }
    }

    /// <summary>
    /// 获取指定深度的缩进字符串 - 优化版本
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private string GetIndentString(int depth)
    {
        if (depth == 0) return "";

        // 使用缓存（大部分情况）
        if (depth <= MaxCacheDepth)
        {
            return _indentCache[depth];
        }

        // 超出缓存范围时动态创建 - 使用更高效的方式
        if (_indentChar.Length == 1)
        {
            return new string(_indentChar[0], depth * _indentCount);
        }
        else
        {
            var totalLength = depth * _indentCount * _indentChar.Length;
            return string.Create(totalLength, (depth, _indentCount, _indentChar), static (span, state) =>
            {
                int pos = 0;
                for (int i = 0; i < state.depth * state._indentCount; i++)
                {
                    state._indentChar.AsSpan().CopyTo(span[pos..]);
                    pos += state._indentChar.Length;
                }
            });
        }
    }

    /// <summary>
    /// 估算初始容量 - 基于典型使用模式
    /// </summary>
    private static int EstimateInitialCapacity()
    {
        // 基于典型代码生成场景的启发式估算
        // 假设平均每行40个字符，100行代码
        return 4000;
    }

    #endregion

    #region 核心方法 - 性能优化版本

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Append(string text)
    {
        SB.Append(text);
        _estimatedFinalSize += text.Length;
        return Self;
    }

    /// <summary>
    /// 高性能版本：添加一行或多行代码（自动处理缩进）
    /// 使用优化的 Span 操作和减少分配
    /// </summary>
    public T AppendLine(string text = "")
    {
        // 特殊处理空行，避免不必要的字符串操作
        if (string.IsNullOrEmpty(text))
        {
            SB.AppendLine();
            _lineCount++;
            _estimatedFinalSize += Environment.NewLine.Length;
            return Self;
        }

        // 使用 Span 高效处理多行字符串
        ReadOnlySpan<char> span = text.AsSpan();

        // 快速路径：单行无换行符的情况
        if (span.IndexOf('\n') == -1)
        {
            AppendSingleLineOptimized(span);
            return Self;
        }

        // 多行处理
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
                
                AppendSingleLineOptimized(line);
                start = i + 1;
            }
        }
        
        // 处理最后一行（如果没有以换行符结尾）
        if (start < span.Length)
        {
            ReadOnlySpan<char> lastLine = span.Slice(start);
            AppendSingleLineOptimized(lastLine);
        }
        else if (start == span.Length && span.Length > 0 && span[span.Length - 1] == '\n')
        {
            // 如果字符串以换行符结尾，添加一个空行
            AppendSingleLineOptimized(ReadOnlySpan<char>.Empty);
        }

        return Self;
    }

    /// <summary>
    /// 优化的单行添加方法
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void AppendSingleLineOptimized(ReadOnlySpan<char> line)
    {
        _lineCount++;
        
        // 特殊处理空行
        if (line.IsEmpty)
        {
            SB.AppendLine();
            _estimatedFinalSize += Environment.NewLine.Length;
            return;
        }

        // 计算总长度用于容量检查
        int totalLength = _currentIndent.Length + line.Length + Environment.NewLine.Length;
        _estimatedFinalSize += totalLength;

        // 检查是否需要扩容
        if (SB.Capacity < SB.Length + totalLength)
        {
            SB.EnsureCapacity(SB.Length + totalLength + 1024); // 额外预留空间
        }

        // 高效追加
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
    /// 批量添加行 - 高性能版本
    /// </summary>
    public T AppendLines(ReadOnlySpan<string> lines)
    {
        if (lines.IsEmpty) return Self;

        // 预估总大小并预分配
        int estimatedSize = 0;
        foreach (var line in lines)
        {
            estimatedSize += (line?.Length ?? 0) + _currentIndent.Length + Environment.NewLine.Length;
        }

        SB.EnsureCapacity(SB.Length + estimatedSize);

        // 批量处理
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                SB.AppendLine();
                _lineCount++;
            }
            else
            {
                AppendSingleLineOptimized(line.AsSpan());
            }
        }

        return Self;
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

    /// <summary>
    /// 添加带前缀的代码块（如 if 语句等） - 优化版本
    /// </summary>
    protected T CodeBlock(string? prefix, Func<T, T> action)
    {
        string? header = null;
        if (prefix is null)
        {
            // 无前缀版本：仅使用块开始符号
            if (!string.IsNullOrEmpty(_blockStart))
            {
                header = _blockStart;
            }
        }
        else
        {
            // 有前缀版本：组合前缀和块开始符号
            if (string.IsNullOrEmpty(_blockStart))
            {
                header = prefix;
            }
            else
            {
                // 使用 string.Create 优化字符串拼接
                header = string.Create(prefix.Length + _blockStart.Length, (prefix, _blockStart), 
                    static (span, state) =>
                    {
                        state.prefix.AsSpan().CopyTo(span);
                        state._blockStart.AsSpan().CopyTo(span[state.prefix.Length..]);
                    });
            }
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

    public override string ToString() => SB.ToString();

    #endregion

    #region 性能分析和诊断

    /// <summary>
    /// 获取性能统计信息
    /// </summary>
    public (int LineCount, int EstimatedSize, int ActualSize, double Efficiency) GetPerformanceStats()
    {
        int actualSize = SB.Length;
        double efficiency = _estimatedFinalSize > 0 ? (double)actualSize / _estimatedFinalSize : 1.0;
        return (_lineCount, _estimatedFinalSize, actualSize, efficiency);
    }

    /// <summary>
    /// 重置性能计数器
    /// </summary>
    public void ResetPerformanceCounters()
    {
        _lineCount = 0;
        _estimatedFinalSize = SB.Capacity;
    }

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