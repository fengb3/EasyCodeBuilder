using System.Reflection;
using CodeBuilder.Demo.Console;
using Fengb3.EasyCodeBuilder;

var command = args.Length > 0 ? args[0] : "performance";

// get all methods marked with CommandHandlerAttribute
var handler = typeof(Program).GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
    .FirstOrDefault(m => m.GetCustomAttribute<CommandHandlerAttribute>()?.Name == command);
if (handler == null)
{
    Console.WriteLine($"No handler found for command: {command}");
    Console.WriteLine("Available commands: csharp, python, performance, benchmark");
    return;
}

handler.Invoke(null, null);


#pragma warning disable CS8321 // Local function is declared but never used
[CommandHandler("csharp")]
static void CsharpCodeBuilderTest()
{
    Console.WriteLine("=== C# Code Generation Demo ===");
    
    var cb = new CSharpCodeBuilder();
    cb.Using("System", "System.Collections.Generic", "System.Threading.Tasks")
      .Namespace("Example.Generated", ns => ns
          .Class("MyClass", cls => cls
              .Property("string", "Name", "public", null, "{ get; set; }")
              .Property("int", "Id", "public", null, "{ get; set; }")
              .Method("TestMethod", method =>
                  method
                  .AppendLine("var sb = new StringBuilder();")
                  .AppendLine("sb.AppendLine(\"Hello from CodeBlock!\");")
                  .AppendLine("sb.AppendLine(\"Optimized performance!\");")
                  .AppendLine("Console.WriteLine(sb.ToString());")
                  .AppendLine("return sb.ToString();")
              , "string", "public")
          )
      );
    Console.WriteLine(cb.ToString());
    
    // Performance stats
    var stats = cb.GetPerformanceStats();
    Console.WriteLine($"\nPerformance Stats:");
    Console.WriteLine($"- Lines generated: {stats.LineCount}");
    Console.WriteLine($"- Estimated size: {stats.EstimatedSize} bytes");
    Console.WriteLine($"- Actual size: {stats.ActualSize} bytes");
    Console.WriteLine($"- Memory efficiency: {stats.Efficiency:P1}");
}

[CommandHandler("python")]
static void PythonCodeBuilderTest()
{
    Console.WriteLine("=== Python Code Generation Demo ===");
    
    var cb = new PythonCodeBuilder();
    cb.Import("datetime", "typing", "dataclasses")
      .AppendLine()
      .Class("MyClass", cls => cls
          .Method("__init__", init => init
              .AppendLine("self.name = name")
              .AppendLine("self.id = id_value")
              .AppendLine("self.created_at = datetime.datetime.now()")
          , "self, name: str, id_value: int")
          .AppendLine()
          .Method("test_method", method => method
              .AppendLine("print('Hello from CodeBlock!')")
              .AppendLine("print('Optimized performance!')")
              .AppendLine("return f'Name: {self.name}, ID: {self.id}'")
          , "self", "str")
      );
    Console.WriteLine(cb.ToString());
    
    // Performance stats
    var stats = cb.GetPerformanceStats();
    Console.WriteLine($"\nPerformance Stats:");
    Console.WriteLine($"- Lines generated: {stats.LineCount}");
    Console.WriteLine($"- Estimated size: {stats.EstimatedSize} bytes");
    Console.WriteLine($"- Actual size: {stats.ActualSize} bytes");
    Console.WriteLine($"- Memory efficiency: {stats.Efficiency:P1}");
}

[CommandHandler("performance")]
static void PerformanceAnalysisTest()
{
    Console.WriteLine("=== Performance Analysis ===");
    Console.WriteLine("Running comprehensive performance benchmarks...\n");
    
    var analysis = PerformanceBenchmark.RunFullPerformanceAnalysis();
    Console.WriteLine(analysis);
}

[CommandHandler("benchmark")]
static void BenchmarkTest()
{
    Console.WriteLine("=== Individual Benchmark Tests ===");
    
    // C# Benchmark
    Console.WriteLine("Running C# benchmark...");
    var csharpResult = PerformanceBenchmark.BenchmarkCSharpGeneration("C# Optimized Test", 5, 3, 100);
    Console.WriteLine($"C# Result: {csharpResult.LinesPerSecond:F0} lines/sec, {csharpResult.CharactersPerSecond:F0} chars/sec");
    Console.WriteLine($"Memory usage: {csharpResult.MemoryAllocated:N0} bytes, {csharpResult.MemoryPerLine:F2} bytes/line\n");
    
    // Python Benchmark
    Console.WriteLine("Running Python benchmark...");
    var pythonResult = PerformanceBenchmark.BenchmarkPythonGeneration("Python Optimized Test", 5, 3, 100);
    Console.WriteLine($"Python Result: {pythonResult.LinesPerSecond:F0} lines/sec, {pythonResult.CharactersPerSecond:F0} chars/sec");
    Console.WriteLine($"Memory usage: {pythonResult.MemoryAllocated:N0} bytes, {pythonResult.MemoryPerLine:F2} bytes/line\n");
    
    // Large scale test
    Console.WriteLine("Running large-scale benchmark...");
    var largeScaleResult = PerformanceBenchmark.BenchmarkCSharpGeneration("Large Scale Test", 20, 10, 50);
    Console.WriteLine($"Large Scale Result: {largeScaleResult.LinesPerSecond:F0} lines/sec");
    Console.WriteLine($"Generated {largeScaleResult.GeneratedLines:N0} lines in {largeScaleResult.ElapsedTime.TotalMilliseconds:F0} ms");
}


namespace CodeBuilder.Demo.Console
{
    public class CommandHandlerAttribute(string name) : Attribute
    {
        public string Name { get; } = name;
    }
}