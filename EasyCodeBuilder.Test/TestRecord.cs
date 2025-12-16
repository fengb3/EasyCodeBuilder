using System;
using System.Security.Cryptography.X509Certificates;
using Fengb3.EasyCodeBuilder.Csharp;
using static Fengb3.EasyCodeBuilder.Csharp.Code;
using Xunit.Abstractions;

namespace EasyCodeBuilder.Test;

public class TestRecord(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public void TestNamespace()
    {
        var op = Create()
            .Using("System")
            .Using("System.Linq", "System.Collections.Generic")
            .Namespace(@namespace => {
                @namespace.Name = "MyNamespace";
            })
            .Build();

        testOutputHelper.WriteLine(op);
    }

    [Fact]
    public void TestType()
    {
        var op = Create()
            .Namespace(@namespace => {
                @namespace.Name = "MyNamespace";
                @namespace.Type(type => {
                    type.TypeKind = TypeOption.Type.Record;
                    type.Keywords.Add("public");
                    type.Name = "MyRecord";
                });
                @namespace.Class(cls => {
                    cls.Name = "MyClass";
                    cls.Keywords.Add("public");

                    cls.BaseTypes.Add("MyRecord");
                    cls.Property(prop => {
                        prop.Name = "MyProperty";
                        prop.Type = "string";
                        prop.Keywords.Add("public");
                    });

                    cls.Method(method => {
                        method
                            .WithKeyword("public")
                            .WithName("MyMethod")
                            .WithReturnType("void")
                            .WithParameters("int x", "string y")
                            .If(@if => {
                                @if.Condition = "x != null";
                                @if.AppendLine(
                                    "Console.WriteLine(x);",
                                    $"""
                                    var x = 10; // {"this is comment in string interpolation"}
                                    var y = x + 20;
                                    Console.WriteLine(y);
                                    """);
                            }).Else(@else => {
                                @else.AppendLine("Console.WriteLine(x);");
                            });
                    });
                });
            })
            .Build();

        testOutputHelper.WriteLine(op);
    }
}