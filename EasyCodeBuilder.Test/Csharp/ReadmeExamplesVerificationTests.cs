using System;
using Fengb3.EasyCodeBuilder.Csharp;
using Fengb3.EasyCodeBuilder.Csharp.OptionConfigurations;
using Xunit.Abstractions;

namespace EasyCodeBuilder.Test.Csharp;

/// <summary>
/// Tests to verify that the README examples work correctly
/// </summary>
public class ReadmeExamplesVerificationTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    public ReadmeExamplesVerificationTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void TestBasicUsageExample()
    {
        // This verifies the "Basic Usage" example in README
        var code = Code.Create()
            .Using("System")
            .Namespace(ns => {
                ns.Name = "MyProject";
                ns.Public.Class(cls => {
                    cls.WithName("Person");
                    cls.Public.AutoProperty(p => p
                        .WithType("string")
                        .WithName("Name")
                    );
                    cls.Public.AutoProperty(p => p
                        .WithType("int")
                        .WithName("Age")
                    );
                });
            })
            .Build();

        const string expected =
            """
            using System;

            namespace MyProject
            {
              public class Person
              {
                public string Name { get; set; }
                public int Age { get; set; }
              }
            }
            """;

        Assert.Equal(expected.Trim(), code.Trim());
        _testOutputHelper.WriteLine(code);
    }

    [Fact]
    public void TestGenerateClassWithMethodsExample()
    {
        // This verifies the "Generate a Class with Methods" example in README
        var code = Code.Create()
            .Using("System")
            .Namespace(ns => {
                ns.Name = "MyApp";
                ns.Public.Class(cls => {
                    cls.WithName("Calculator");
                    cls.Public.Method(method => {
                        method.WithName("Add")
                              .WithReturnType("int")
                              .WithParameters("int a", "int b")
                              .AppendLine("return a + b;");
                    });
                });
            })
            .Build();

        const string expected =
            """
            using System;

            namespace MyApp
            {
              public class Calculator
              {
                public int Add(int a, int b)
                {
                  return a + b;
                }
              }
            }
            """;

        Assert.Equal(expected.Trim(), code.Trim());
        _testOutputHelper.WriteLine(code);
    }

    [Fact]
    public void TestKeywordConfiguratorPublicClassExample()
    {
        // This verifies the first "Using Keyword Configurator" example in README
        var @namespace = new NamespaceOption()
            .WithName("MyNamespace")
            .Public.Class(cls => {
                cls.WithName("MyClass");
            });

        var code = @namespace.Build();

        const string expected =
            """
            namespace MyNamespace
            {
              public class MyClass
              {
              }
            }
            """;

        Assert.Equal(expected.Trim(), code.Trim());
        _testOutputHelper.WriteLine(code);
    }

    [Fact]
    public void TestKeywordConfiguratorAutoPropertyExample()
    {
        // This verifies the second "Using Keyword Configurator" example in README
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
        _testOutputHelper.WriteLine(classCode);
    }
}
