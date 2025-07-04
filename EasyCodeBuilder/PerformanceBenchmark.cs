using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Fengb3.EasyCodeBuilder;

/// <summary>
/// 性能基准测试工具 - 用于测量和分析代码生成性能
/// </summary>
public static class PerformanceBenchmark
{
    /// <summary>
    /// 性能测试结果
    /// </summary>
    public readonly struct BenchmarkResult
    {
        public string TestName { get; }
        public TimeSpan ElapsedTime { get; }
        public long MemoryAllocated { get; }
        public int GeneratedLines { get; }
        public int GeneratedCharacters { get; }
        public double LinesPerSecond => GeneratedLines / ElapsedTime.TotalSeconds;
        public double CharactersPerSecond => GeneratedCharacters / ElapsedTime.TotalSeconds;
        public double MemoryPerLine => GeneratedLines > 0 ? (double)MemoryAllocated / GeneratedLines : 0;

        public BenchmarkResult(string testName, TimeSpan elapsedTime, long memoryAllocated, 
                              int generatedLines, int generatedCharacters)
        {
            TestName = testName;
            ElapsedTime = elapsedTime;
            MemoryAllocated = memoryAllocated;
            GeneratedLines = generatedLines;
            GeneratedCharacters = generatedCharacters;
        }
    }

    /// <summary>
    /// 运行性能基准测试
    /// </summary>
    /// <typeparam name="T">代码构建器类型</typeparam>
    /// <param name="testName">测试名称</param>
    /// <param name="builderFactory">构建器工厂函数</param>
    /// <param name="testAction">测试动作</param>
    /// <param name="iterations">迭代次数</param>
    /// <returns>基准测试结果</returns>
    public static BenchmarkResult RunBenchmark<T>(
        string testName,
        Func<T> builderFactory,
        Action<T> testAction,
        int iterations = 1000) where T : CodeBuilder<T>
    {
        // 预热 JIT
        for (int i = 0; i < 10; i++)
        {
            var warmupBuilder = builderFactory();
            testAction(warmupBuilder);
        }

        // 强制垃圾回收以获得准确的内存测量
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        var initialMemory = GC.GetTotalMemory(false);
        var stopwatch = Stopwatch.StartNew();

        T? lastBuilder = default;
        
        // 执行实际测试
        for (int i = 0; i < iterations; i++)
        {
            lastBuilder = builderFactory();
            testAction(lastBuilder);
        }

        stopwatch.Stop();
        var finalMemory = GC.GetTotalMemory(false);

        // 计算结果统计
        var generatedCode = lastBuilder?.ToString() ?? "";
        var lines = CountLines(generatedCode);
        var characters = generatedCode.Length;

        return new BenchmarkResult(
            testName,
            stopwatch.Elapsed,
            Math.Max(0, finalMemory - initialMemory),
            lines * iterations,
            characters * iterations
        );
    }

    /// <summary>
    /// 比较两个基准测试结果
    /// </summary>
    public static string CompareResults(BenchmarkResult baseline, BenchmarkResult optimized)
    {
        var timeImprovement = baseline.ElapsedTime.TotalMilliseconds / optimized.ElapsedTime.TotalMilliseconds;
        var memoryImprovement = (double)baseline.MemoryAllocated / optimized.MemoryAllocated;
        var throughputImprovement = optimized.LinesPerSecond / baseline.LinesPerSecond;

        return $"""
            Performance Comparison: {baseline.TestName} vs {optimized.TestName}
            ================================================================
            
            Time Performance:
            - Baseline: {baseline.ElapsedTime.TotalMilliseconds:F2} ms
            - Optimized: {optimized.ElapsedTime.TotalMilliseconds:F2} ms
            - Improvement: {timeImprovement:F2}x faster ({((timeImprovement - 1) * 100):F1}% improvement)
            
            Memory Usage:
            - Baseline: {baseline.MemoryAllocated:N0} bytes
            - Optimized: {optimized.MemoryAllocated:N0} bytes
            - Improvement: {memoryImprovement:F2}x less memory ({((1 - 1/memoryImprovement) * 100):F1}% reduction)
            
            Throughput:
            - Baseline: {baseline.LinesPerSecond:F0} lines/sec, {baseline.CharactersPerSecond:F0} chars/sec
            - Optimized: {optimized.LinesPerSecond:F0} lines/sec, {optimized.CharactersPerSecond:F0} chars/sec
            - Improvement: {throughputImprovement:F2}x throughput
            
            Memory Efficiency:
            - Baseline: {baseline.MemoryPerLine:F2} bytes/line
            - Optimized: {optimized.MemoryPerLine:F2} bytes/line
            """;
    }

    /// <summary>
    /// 创建标准的C#代码生成基准测试
    /// </summary>
    public static BenchmarkResult BenchmarkCSharpGeneration(string testName, int classCount = 10, int methodsPerClass = 5, int iterations = 100)
    {
        return RunBenchmark(
            testName,
            () => new CSharpCodeBuilder(),
            builder => GenerateComplexCSharpCode(builder, classCount, methodsPerClass),
            iterations
        );
    }

    /// <summary>
    /// 创建标准的Python代码生成基准测试
    /// </summary>
    public static BenchmarkResult BenchmarkPythonGeneration(string testName, int classCount = 10, int methodsPerClass = 5, int iterations = 100)
    {
        return RunBenchmark(
            testName,
            () => new PythonCodeBuilder(),
            builder => GenerateComplexPythonCode(builder, classCount, methodsPerClass),
            iterations
        );
    }

    /// <summary>
    /// 生成复杂的C#代码用于性能测试
    /// </summary>
    private static void GenerateComplexCSharpCode(CSharpCodeBuilder builder, int classCount, int methodsPerClass)
    {
        builder.Using("System", "System.Collections.Generic", "System.Linq", "System.Threading.Tasks");

        builder.Namespace("PerformanceTest.Generated", ns =>
        {
            for (int i = 0; i < classCount; i++)
            {
                ns.Class($"TestClass{i}", cls =>
                {
                    // 添加属性
                    cls.Property("string", "Name", "public", null, "{ get; set; }")
                       .Property("int", "Id", "public", null, "{ get; set; }")
                       .Property("DateTime", "CreatedAt", "public", "DateTime.Now", "{ get; set; }");

                    // 添加方法
                    for (int j = 0; j < methodsPerClass; j++)
                    {
                        cls.Method($"Method{j}", method =>
                        {
                            method.AppendLine("// Auto-generated method")
                                  .AppendLine($"var result = Id * {j};")
                                  .If("result > 0", ifBlock =>
                                      ifBlock.AppendLine("Console.WriteLine($\"Result: {result}\");")
                                  )
                                  .AppendLine("return result;");
                            return method;
                        }, "int", "public", $"int param{j}");
                    }

                    return cls;
                });
            }
        });
    }

    /// <summary>
    /// 生成复杂的Python代码用于性能测试
    /// </summary>
    private static void GenerateComplexPythonCode(PythonCodeBuilder builder, int classCount, int methodsPerClass)
    {
        builder.Import("datetime", "typing", "dataclasses");

        for (int i = 0; i < classCount; i++)
        {
            builder.Class($"TestClass{i}", cls =>
            {
                cls.Method("__init__", init =>
                {
                    init.AppendLine("self.name = name")
                        .AppendLine($"self.id = {i}")
                        .AppendLine("self.created_at = datetime.datetime.now()");
                    return init;
                }, "self, name: str");

                for (int j = 0; j < methodsPerClass; j++)
                {
                    cls.Method($"method_{j}", method =>
                    {
                        method.AppendLine("# Auto-generated method")
                              .AppendLine($"result = self.id * {j}")
                              .If("result > 0", ifBlock =>
                                  ifBlock.AppendLine("print(f'Result: {result}')")
                              )
                              .AppendLine("return result");
                        return method;
                    }, $"self, param_{j}: int", "int");
                }

                return cls;
            });
        }
    }

    /// <summary>
    /// 计算文本中的行数
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int CountLines(string text)
    {
        if (string.IsNullOrEmpty(text)) return 0;
        
        int count = 1;
        var span = text.AsSpan();
        for (int i = 0; i < span.Length; i++)
        {
            if (span[i] == '\n') count++;
        }
        return count;
    }

    /// <summary>
    /// 运行完整的性能分析套件
    /// </summary>
    public static string RunFullPerformanceAnalysis()
    {
        var results = new List<string>();
        
        results.Add("=== EasyCodeBuilder Performance Analysis ===");
        results.Add("");

        // C# 代码生成基准测试
        var csharpBaseline = BenchmarkCSharpGeneration("C# Baseline", 5, 3, 200);
        var csharpOptimized = BenchmarkCSharpGeneration("C# Optimized", 5, 3, 200);
        
        results.Add("C# Code Generation Benchmark:");
        results.Add($"- Lines/sec: {csharpOptimized.LinesPerSecond:F0}");
        results.Add($"- Chars/sec: {csharpOptimized.CharactersPerSecond:F0}");
        results.Add($"- Memory efficiency: {csharpOptimized.MemoryPerLine:F2} bytes/line");
        results.Add("");

        // Python 代码生成基准测试
        var pythonBaseline = BenchmarkPythonGeneration("Python Baseline", 5, 3, 200);
        var pythonOptimized = BenchmarkPythonGeneration("Python Optimized", 5, 3, 200);
        
        results.Add("Python Code Generation Benchmark:");
        results.Add($"- Lines/sec: {pythonOptimized.LinesPerSecond:F0}");
        results.Add($"- Chars/sec: {pythonOptimized.CharactersPerSecond:F0}");
        results.Add($"- Memory efficiency: {pythonOptimized.MemoryPerLine:F2} bytes/line");
        results.Add("");

        // 整体统计
        results.Add("Performance Summary:");
        results.Add($"- Total test time: {(csharpOptimized.ElapsedTime + pythonOptimized.ElapsedTime).TotalMilliseconds:F2} ms");
        results.Add($"- Total memory used: {(csharpOptimized.MemoryAllocated + pythonOptimized.MemoryAllocated):N0} bytes");
        results.Add($"- Average throughput: {((csharpOptimized.LinesPerSecond + pythonOptimized.LinesPerSecond) / 2):F0} lines/sec");

        return string.Join(Environment.NewLine, results);
    }
}