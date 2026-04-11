using System;
using Fengb3.EasyCodeBuilder.Csharp;
using Fengb3.EasyCodeBuilder.Csharp.OptionConfigurations;
using Xunit.Abstractions;
using static Fengb3.EasyCodeBuilder.Csharp.Code;

namespace EasyCodeBuilder.Test.Csharp;

public partial class CsharpCodeOptionTests(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public void TestNamespace()
    {
        var code = Create()
            .Using("System", "System.Collections.Generic")
            .Namespace(ns => { ns.Name = "MyNamespace"; })
            .Build();

        const string expected = """
                                using System;
                                using System.Collections.Generic;
                                namespace MyNamespace
                                {
                                }                          
                                """;

        Assert.Equal(expected.Trim(), code.Trim());

        testOutputHelper.WriteLine(code);
    }

    [Fact]
    public void TestClassInNamespace()
    {
        var code = Create()
            .AddChild<CodeOption, NamespaceOption>(ns =>
            {
                ns.Name = "MyNamespace";
                ns.Class(to => { to.Name = "MyClass"; });
            })
            .Build();

        var expected = """
                       namespace MyNamespace
                       {
                         class MyClass
                         {
                         }
                       }                          
                       """;

        Assert.Equal(expected.Trim(), code.Trim());

        testOutputHelper.WriteLine(code);
    }

    [Fact]
    public void TestPropertyInClass()
    {
        var code = new TypeOption()
            .WithTypeKind(TypeOption.Type.Class)
            .WithName("MyClass")
            .WithKeywords("public")
            .AutoProperty(po => { po.WithType("string").WithName("MyProperty").WithKeyword("public"); })
            .Build();

        var expected = """
                       public class MyClass
                       {
                         public string MyProperty { get; set; }
                       }
                       """;

        Assert.Equal(expected.Trim(), code.Trim());

        testOutputHelper.WriteLine(code);
    }

    [Fact]
    public void TestConstructorInClass()
    {
        // var constructor = new ConstructorOption()
        //     .WithKeywords("public")
        //     .WithParameter("string name")
        //     .WithParameter("int age");

        var classOption = new TypeOption()
            .WithTypeKind(TypeOption.Type.Class)
            .WithName("MyClass")
            .WithKeywords("public")
            .Constructor(ctor => { ctor.WithKeyword("public").WithParameter("string name").WithParameter("int age"); });

        var code = classOption.Build();

        var expected = """
                       public class MyClass
                       {
                         public MyClass(string name, int age)
                         {
                         }
                       }
                       """;

        Assert.Equal(expected.Trim(), code.Trim());
    }

    [Fact]
    public void TestForLoopInMethod()
    {
        var method = new MethodOption()
            .WithName("MyMethod")
            .WithReturnType("void")
            .WithKeyword("public")
            .For(@for =>
            {
                @for.WithInitializer("int i = 0")
                    .WithCondition("i < 10")
                    .WithIterator("i++")
                    .AppendLine("Console.WriteLine(i);");
            })
            .Build();

        var expected = """
                       public void MyMethod()
                       {
                         for (int i = 0; i < 10; i++)
                         {
                           Console.WriteLine(i);
                         }
                       }
                       """;

        Assert.Equal(expected.Trim(), method.Trim());
    }

    [Fact]
    public void TestSwitchCaseInMethod()
    {
        var method = new MethodOption()
            .WithName("CheckValue")
            .WithReturnType("string")
            .WithKeyword("public");

        method.AppendLine("var value = GetValue();");

        method.Switch(@switch =>
        {
            @switch.Expression = "value";
            @switch.Case(@case =>
            {
                @case.Value = "1";
                @case.AppendLine("""return "One";""");
            });
            @switch.Case(@case =>
            {
                @case.Value = "2";
                @case.AppendLine("""return "Two";""");
            });
            @switch.Default(@default => @default.AppendLine("""return "Other";"""));
        });

        var methodCode = method.Build();

        var expected = """
                       public string CheckValue()
                       {
                         var value = GetValue();
                         switch (value)
                         {
                           case 1:
                           {
                             return "One";
                           }
                           case 2:
                           {
                             return "Two";
                           }
                           default:
                           {
                             return "Other";
                           }
                         }
                       }
                       """;

        Assert.Equal(expected.Trim(), methodCode.Trim());
    }
}

public partial class CsharpCodeOptionTests
{
    [Fact]
    public void TestAttributeInClass()
    {
        var code = Code.Create().Namespace(ns =>
            ns.WithName("Gg")
                .Public.Class(cls =>
                    cls.WithName("MyClass")
                        .WithAttributes("Serializable", "DataContract"))
                .Public.Class(cls =>
                    cls.WithName("MyClass2")
                        .WithKeyword("partial")
                        .WithAttributes("Serializable")
                )
        ).Build();

        testOutputHelper.WriteLine(code);
    }

    [Fact]
    public void TestXmlDocInClassAndInMethod()
    {
        var code = Code.Create().Namespace(ns =>
            ns.WithName("Gg")
                .Public.Class(cls =>
                    cls.WithName("MyClass")
                        .WithXmlDoc(xml => xml.WithSummary("This is MyClass"))
                        .Public.Method(@method =>
                            method.WithName("MyMethod").WithReturnType("void")
                                .WithXmlDoc(xml =>
                                    xml.WithSummary("This is MyMethod")
                                        .WithParam("param1", "This is param1"))
                                .AppendLine("Console.WriteLine(param1);")
                        )
                )
                .Public.Class(cls =>
                    cls.WithName("MyClass2")
                        .WithKeyword("partial")
                        .WithAttributes("Serializable")
                        .WithXmlDoc(xml => xml.WithSummary("This is MyClass2\nwith multiple lines in summary"))
                )
        ).Build();

        testOutputHelper.WriteLine(code);
    }
}