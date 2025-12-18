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

    [Fact]
    public void TestUsingConstructorsExample()
    {
        // This verifies the "Using Constructors" example in README
        var classOption = new TypeOption()
            .WithTypeKind(TypeOption.Type.Class)
            .WithName("Person")
            .WithKeyword("public");

        classOption.Constructor(ctor => {
            ctor.WithKeyword("public")
                .WithParameter("string name")
                .WithParameter("int age")
                .AppendLine("Name = name;")
                .AppendLine("Age = age;");
        });

        classOption.Public.AutoProperty(p => p
            .WithType("string")
            .WithName("Name"));

        classOption.Public.AutoProperty(p => p
            .WithType("int")
            .WithName("Age"));

        var code = classOption.Build();

        const string expected =
            """
            public class Person
            {
              public Person(string name, int age)
              {
                Name = name;
                Age = age;
              }
              public string Name { get; set; }
              public int Age { get; set; }
            }
            """;

        Assert.Equal(expected.Trim(), code.Trim());
        _testOutputHelper.WriteLine(code);
    }

    [Fact]
    public void TestControlStructuresForLoopExample()
    {
        // This verifies the "Control Structures - For Loop" example in README
        var classOption = new TypeOption()
            .WithTypeKind(TypeOption.Type.Class)
            .WithName("NumberPrinter");

        classOption.Public.Method(method => {
            method.WithName("PrintNumbers")
                  .WithReturnType("void")
                  .For(@for => {
                      @for.WithInitializer("int i = 0")
                          .WithCondition("i < 10")
                          .WithIterator("i++")
                          .AppendLine("Console.WriteLine(i);");
                  });
        });

        var code = classOption.Build();

        const string expected =
            """
            class NumberPrinter
            {
              public void PrintNumbers()
              {
                for (int i = 0; i < 10; i++)
                {
                  Console.WriteLine(i);
                }
              }
            }
            """;

        Assert.Equal(expected.Trim(), code.Trim());
        _testOutputHelper.WriteLine(code);
    }

    [Fact]
    public void TestControlStructuresSwitchStatementExample()
    {
        // This verifies the "Control Structures - Switch Statement" example in README
        var classOption = new TypeOption()
            .WithTypeKind(TypeOption.Type.Class)
            .WithName("DayHelper");

        classOption.Public.Method(method => {
            method.WithName("GetDayName")
                  .WithReturnType("string")
                  .WithParameters("int day");

            method.Switch(@switch => {
                @switch.Expression = "day";
                @switch.Case(@case => {
                    @case.Value = "1";
                    @case.AppendLine("return \"Monday\";");
                });
                @switch.Case(@case => {
                    @case.Value = "2";
                    @case.AppendLine("return \"Tuesday\";");
                });
                @switch.Default(@default =>
                    @default.AppendLine("return \"Unknown\";")
                );
            });
        });

        var code = classOption.Build();

        const string expected =
            """
            class DayHelper
            {
              public string GetDayName(int day)
              {
                switch (day)
                {
                  case 1:
                  {
                    return "Monday";
                  }
                  case 2:
                  {
                    return "Tuesday";
                  }
                  default:
                  {
                    return "Unknown";
                  }
                }
              }
            }
            """;

        Assert.Equal(expected.Trim(), code.Trim());
        _testOutputHelper.WriteLine(code);
    }
}
