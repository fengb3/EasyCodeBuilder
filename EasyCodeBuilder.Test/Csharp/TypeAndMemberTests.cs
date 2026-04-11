using Fengb3.EasyCodeBuilder.Csharp;
using Fengb3.EasyCodeBuilder.Csharp.OptionConfigurations;
using Xunit.Abstractions;

namespace EasyCodeBuilder.Test.Csharp;

public class TypeAndMemberTests(ITestOutputHelper output)
{
    private static string Norm(string text) => text.Replace("\r\n", "\n").Trim();

    [Fact]
    public void StructType()
    {
        var code = new TypeOption()
            .WithTypeKind(TypeOption.Type.Struct)
            .WithName("Point")
            .WithKeywords("public")
            .Public.AutoProperty(p => p.WithType("int").WithName("X"))
            .Public.AutoProperty(p => p.WithType("int").WithName("Y"))
            .Build();

        var expected = """
            public struct Point
            {
              public int X { get; set; }
              public int Y { get; set; }
            }
            """;

        Assert.Equal(Norm(expected), Norm(code));
        output.WriteLine(code);
    }

    [Fact]
    public void EnumType()
    {
        var code = new NamespaceOption()
            .WithName("MyApp")
            .Public.Enum(e =>
            {
                e.WithName("Color");
                e.AppendLine("Red,");
                e.AppendLine("Green,");
                e.AppendLine("Blue");
            })
            .Build();

        var expected = """
            namespace MyApp
            {
              public enum Color
              {
                Red,
                Green,
                Blue
              }
            }
            """;

        Assert.Equal(Norm(expected), Norm(code));
        output.WriteLine(code);
    }

    [Fact]
    public void InterfaceType()
    {
        var code = new TypeOption()
            .WithTypeKind(TypeOption.Type.Interface)
            .WithName("IRepository")
            .WithKeywords("public")
            .Public.Method(m =>
            {
                m.WithName("GetById");
                m.WithReturnType("object");
                m.WithParameters("int id");
            })
            .Build();

        var expected = """
            public interface IRepository
            {
              public object GetById(int id)
              {
              }
            }
            """;

        Assert.Equal(Norm(expected), Norm(code));
        output.WriteLine(code);
    }

    [Fact]
    public void ClassWithBaseClass()
    {
        var code = new TypeOption()
            .WithTypeKind(TypeOption.Type.Class)
            .WithName("DerivedClass")
            .WithKeywords("public")
            .Inherit("BaseClass")
            .Build();

        var expected = """
            public class DerivedClass : BaseClass
            {
            }
            """;

        Assert.Equal(Norm(expected), Norm(code));
        output.WriteLine(code);
    }

    [Fact]
    public void ClassWithInterface()
    {
        var code = new TypeOption()
            .WithTypeKind(TypeOption.Type.Class)
            .WithName("MyService")
            .WithKeywords("public")
            .Inherit("IService", "IDisposable")
            .Build();

        var expected = """
            public class MyService : IService, IDisposable
            {
            }
            """;

        Assert.Equal(Norm(expected), Norm(code));
        output.WriteLine(code);
    }

    [Fact]
    public void ClassWithBaseAndInterface()
    {
        var code = new TypeOption()
            .WithTypeKind(TypeOption.Type.Class)
            .WithName("MyClass")
            .WithKeywords("public")
            .Inherit("BaseClass")
            .Inherit("IInterface")
            .Build();

        Assert.Contains(": BaseClass, IInterface", code);
        output.WriteLine(code);
    }

    [Fact]
    public void AbstractPartialClass()
    {
        var code = new TypeOption()
            .WithTypeKind(TypeOption.Type.Class)
            .WithName("MyClass")
            .WithKeywords("public", "abstract", "partial")
            .Build();

        var expected = """
            public abstract partial class MyClass
            {
            }
            """;

        Assert.Equal(Norm(expected), Norm(code));
        output.WriteLine(code);
    }

    [Fact]
    public void SealedClass()
    {
        var code = new TypeOption()
            .WithTypeKind(TypeOption.Type.Class)
            .WithName("MyClass")
            .WithKeywords("public", "sealed")
            .Build();

        var expected = """
            public sealed class MyClass
            {
            }
            """;

        Assert.Equal(Norm(expected), Norm(code));
        output.WriteLine(code);
    }

    [Fact]
    public void StaticClass()
    {
        var code = new TypeOption()
            .WithTypeKind(TypeOption.Type.Class)
            .WithName("Helper")
            .WithKeywords("public", "static")
            .Build();

        var expected = """
            public static class Helper
            {
            }
            """;

        Assert.Equal(Norm(expected), Norm(code));
        output.WriteLine(code);
    }

    [Fact]
    public void ConstructorWithMultipleParameters()
    {
        var code = new TypeOption()
            .WithTypeKind(TypeOption.Type.Class)
            .WithName("Person")
            .WithKeywords("public")
            .Constructor(ctor =>
            {
                ctor.WithKeyword("public");
                ctor.WithParameter("string firstName");
                ctor.WithParameter("string lastName");
                ctor.WithParameter("int age");
                ctor.AppendLine("FirstName = firstName;");
                ctor.AppendLine("LastName = lastName;");
                ctor.AppendLine("Age = age;");
            })
            .Public.AutoProperty(p => p.WithType("string").WithName("FirstName"))
            .Build();

        Assert.Contains("public Person(string firstName, string lastName, int age)", code);
        Assert.Contains("FirstName = firstName;", code);
        Assert.Contains("LastName = lastName;", code);
        Assert.Contains("Age = age;", code);
        output.WriteLine(code);
    }

    [Fact]
    public void MethodWithAsyncKeyword()
    {
        var code = new TypeOption()
            .WithTypeKind(TypeOption.Type.Class)
            .WithName("MyClass")
            .WithKeywords("public")
            .Public.Method(m =>
            {
                m.WithName("DoWorkAsync");
                m.WithReturnType("Task");
                m.WithKeyword("async");
                m.AppendLine("await Task.Delay(100);");
            })
            .Build();

        Assert.Contains("public async Task DoWorkAsync()", code);
        Assert.Contains("await Task.Delay(100);", code);
        output.WriteLine(code);
    }

    [Fact]
    public void MethodWithVirtualOverride()
    {
        var code = new TypeOption()
            .WithTypeKind(TypeOption.Type.Class)
            .WithName("MyClass")
            .WithKeywords("public")
            .Public.Method(m =>
            {
                m.WithName("ToString");
                m.WithReturnType("string");
                m.WithKeyword("override");
                m.AppendLine("return \"MyClass\";");
            })
            .Build();

        Assert.Contains("public override string ToString()", code);
        output.WriteLine(code);
    }

    [Fact]
    public void AttributesOnClass()
    {
        var code = Code.Create()
            .Namespace(ns =>
                ns.WithName("MyApp")
                    .Public.Class(cls =>
                        cls.WithName("MyClass")
                            .WithAttributes("Serializable")
                            .WithAttributes("Obsolete(\"Use NewClass\")")
                    )
            )
            .Build();

        Assert.Contains("[Serializable]", code);
        Assert.Contains("[Obsolete(\"Use NewClass\")]", code);
        Assert.Contains("public class MyClass", code);
        output.WriteLine(code);
    }

    [Fact]
    public void XmlDocSummaryOnClass()
    {
        var code = new TypeOption()
            .WithTypeKind(TypeOption.Type.Class)
            .WithName("MyClass")
            .WithKeywords("public")
            .WithXmlDoc(xml => xml.WithSummary("Represents a sample class."))
            .Build();

        Assert.Contains("/// <summary>Represents a sample class.</summary>", code);
        output.WriteLine(code);
    }

    [Fact]
    public void XmlDocSummaryAndParamsOnMethod()
    {
        var code = new TypeOption()
            .WithTypeKind(TypeOption.Type.Class)
            .WithName("MyClass")
            .WithKeywords("public")
            .Public.Method(m =>
            {
                m.WithName("Calculate");
                m.WithReturnType("int");
                m.WithParameters("int x", "int y");
                m.WithXmlDoc(xml =>
                {
                    xml.WithSummary("Calculates the sum of two numbers.");
                    xml.WithParam("x", "The first number.");
                    xml.WithParam("y", "The second number.");
                });
                m.AppendLine("return x + y;");
            })
            .Build();

        Assert.Contains("/// <summary>Calculates the sum of two numbers.</summary>", code);
        Assert.Contains("/// <param name=\"x\">The first number.</param>", code);
        Assert.Contains("/// <param name=\"y\">The second number.</param>", code);
        Assert.Contains("public int Calculate(int x, int y)", code);
        output.WriteLine(code);
    }

    [Fact]
    public void XmlDocMultiLineSummary()
    {
        var code = new TypeOption()
            .WithTypeKind(TypeOption.Type.Class)
            .WithName("MyClass")
            .WithKeywords("public")
            .WithXmlDoc(xml => xml.WithSummary("Line one\nLine two\nLine three"))
            .Build();

        Assert.Contains("/// <summary>", code);
        Assert.Contains("/// Line one", code);
        Assert.Contains("/// Line two", code);
        Assert.Contains("/// Line three", code);
        Assert.Contains("/// </summary>", code);
        output.WriteLine(code);
    }
}
