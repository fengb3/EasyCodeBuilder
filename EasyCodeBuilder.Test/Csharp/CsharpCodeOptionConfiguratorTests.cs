using System;
using Fengb3.EasyCodeBuilder.Csharp;
using Fengb3.EasyCodeBuilder.Csharp.OptionConfigurations;
using Xunit.Abstractions;

namespace EasyCodeBuilder.Test.Csharp;

public class CsharpCodeOptionConfiguratorTests(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public void TestKeywordOptionConfigurator_PublicClassInNamespace()
    {
        var @namespace = new NamespaceOption()
            .WithName("MyNamespace")
            .Public.Class(cls => {
                cls.WithName("MyClass");
            });

        var namespaceCode = @namespace.Build();

        const string expected =
            """
            namespace MyNamespace
            {
              public class MyClass
              {
              }
            }
            """;

        Assert.Equal(expected.Trim(), namespaceCode.Trim());

        testOutputHelper.WriteLine(namespaceCode);
    }

    [Fact]
    public void TestKeywordOptionConfigurator_PrivateStaticMethodInClass()
    {
        var @namespace = new NamespaceOption()
            .WithName("MyNamespace")
            .Class(cls => {
                cls.WithName("MyClass");
                cls.Static.Private.Method(mth => {
                    mth.WithName("MyMethod");
                    mth.AppendLine("Console.WriteLine(\"Hello, World!\");");
                });
            });

        var namespaceCode = @namespace.Build();
        const string expected =
            """
            namespace MyNamespace
            {
              class MyClass
              {
                private static void MyMethod()
                {
                  Console.WriteLine("Hello, World!");
                }
              }
            }
            """;

        Assert.Equal(expected.Trim(), namespaceCode.Trim());
        // _testOutputHelper.WriteLine(namespaceCode);
    }

    [Fact]
    public void TestKeywordOptionConfigurator_PublicStaticPropertyInClass()
    {
        var @class = Code.Create()
            .Class(cls => cls
                .WithName("MyClass")
                .Public.AutoProperty(prop => prop
                    .WithName("MyProperty")
                    .WithType(typeof(int).FullName ?? "int")
                )
            );

        var classCode = @class.Build();
        const string expected =
            """
            class MyClass
            {
              public System.Int32 MyProperty { get; set; }
            }
            """;
        
        Assert.Equal(expected.Trim(), classCode.Trim());
    }

}