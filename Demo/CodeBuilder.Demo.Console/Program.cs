using System.Reflection;
using CodeBuilder.Demo.Console;
using Fengb3.EasyCodeBuilder.Csharp;
using Fengb3.EasyCodeBuilder.Csharp.OptionConfigurations;

var command = args[0];

// get all methods marked with CommandHandlerAttribute
var handler = typeof(Program).GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
    .FirstOrDefault(m => m.GetCustomAttribute<CommandHandlerAttribute>()?.Name == command);
if (handler == null)
{
    Console.WriteLine($"No handler found for command: {command}");
    return;
}

handler.Invoke(null, null);


#pragma warning disable CS8321// Local function is declared but never used
[CommandHandler("csharp")]
static void CsharpCodeBuilderTest()
{
    var root = Code.Create()
        .Namespace(ns => {
            ns.Public.Class(clt => {
                clt.Public.Static.Method(mtd => {
                    mtd.AppendLine("var sb = new StringBuilder();")
                        .AppendLine("sb.AppendLine(\"Hello from CodeBlock!\")")
                        .AppendLine(".AppendLine(\"Hello from CodeBlock!\")")
                        .AppendLine(".AppendLine(\"Hello from CodeBlock!\");")
                        .AppendLine("Console.WriteLine(sb.ToString());");
                });
            });
        });

    Console.WriteLine(root.Build());
    
}

[CommandHandler("python")]
static void PythonCodeBuilderTest()
{
    // var cb = new PythonCodeBuilder();
    // cb.Function("test_function", func => func
    //                                      << "print('Hello from CodeBlock!')"
    //                                      << "print('Hello from CodeBlock!')"
    //                                      << "print('Hello from CodeBlock!')"
    //     )
    //     .AppendLine()
    //     .Function("another_function", func => func
    //                                           << "print('This is another function')"
    //     );
    // Console.WriteLine(cb.ToString());
}


namespace CodeBuilder.Demo.Console
{

    public class CommandHandlerAttribute(string name) : Attribute
    {
        public string Name { get; } = name;
    }

}