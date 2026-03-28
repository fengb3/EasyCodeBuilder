using Fengb3.EasyCodeBuilder.Csharp;
using Xunit.Abstractions;

namespace EasyCodeBuilder.Test.Csharp;

public class NestedClassAndPropertyTests(ITestOutputHelper testOutputHelper)
{
    private static string Normalize(string text) => text.Replace("\r\n", "\n").Trim();

    [Fact]
    public void TestNestedClass()
    {
        var code = new TypeOption()
            .WithTypeKind(TypeOption.Type.Class)
            .WithName("OuterClass")
            .WithKeywords("public")
            .NestedClass(nc => nc
                .WithKeywords("public")
                .WithName("InnerClass")
                .AutoProperty(p => p.WithType("string").WithName("Name").WithKeyword("public"))
            )
            .Build();

        var expected = """
                       public class OuterClass
                       {
                         public class InnerClass
                         {
                           public string Name { get; set; }
                         }
                       }
                       """;

        Assert.Equal(Normalize(expected), Normalize(code));
        testOutputHelper.WriteLine(code);
    }

    [Fact]
    public void TestMultipleNestedClasses()
    {
        var code = new TypeOption()
            .WithTypeKind(TypeOption.Type.Class)
            .WithName("Container")
            .WithKeywords("public")
            .NestedClass(nc => nc.WithKeywords("public").WithName("Item1"))
            .NestedClass(nc => nc.WithKeywords("private").WithName("Item2"))
            .Build();

        var expected = """
                       public class Container
                       {
                         public class Item1
                         {
                         }
                         private class Item2
                         {
                         }
                       }
                       """;

        Assert.Equal(Normalize(expected), Normalize(code));
        testOutputHelper.WriteLine(code);
    }

    [Fact]
    public void TestPropertyWithCustomGetterAndSetter()
    {
        var code = new TypeOption()
            .WithTypeKind(TypeOption.Type.Class)
            .WithName("MyClass")
            .WithKeywords("public")
            .Field(f => f.WithType("string").WithName("_name").WithKeyword("private"))
            .Property(p => p
                .WithType("string")
                .WithName("Name")
                .WithKeyword("public")
                .Getter(g => g.AppendLines("return _name;"))
                .Setter(s => s.AppendLines("_name = value;"))
            )
            .Build();

        var expected = """
                       public class MyClass
                       {
                         private string _name;
                         public string Name
                         {
                           get
                           {
                             return _name;
                           }
                           set
                           {
                             _name = value;
                           }
                         }
                       }
                       """;

        Assert.Equal(Normalize(expected), Normalize(code));
        testOutputHelper.WriteLine(code);
    }

    [Fact]
    public void TestPropertyWithExpressionBodyGetter()
    {
        var code = new TypeOption()
            .WithTypeKind(TypeOption.Type.Class)
            .WithName("MyClass")
            .WithKeywords("public")
            .Field(f => f.WithType("string").WithName("_name").WithKeyword("private"))
            .Property(p => p
                .WithType("string")
                .WithName("Name")
                .WithKeyword("public")
                .Getter(g => g.AsExpressionBody().AppendLines("_name"))
                .Setter(s => s.AppendLines("_name = value;"))
            )
            .Build();

        var expected = """
                       public class MyClass
                       {
                         private string _name;
                         public string Name
                         {
                           get => _name;
                           set
                           {
                             _name = value;
                           }
                         }
                       }
                       """;

        Assert.Equal(Normalize(expected), Normalize(code));
        testOutputHelper.WriteLine(code);
    }

    [Fact]
    public void TestPropertyWithPrivateSetter()
    {
        var code = new TypeOption()
            .WithTypeKind(TypeOption.Type.Class)
            .WithName("MyClass")
            .WithKeywords("public")
            .Property(p => p
                .WithType("string")
                .WithName("Name")
                .WithKeyword("public")
                .AutoGetter()
                .Setter(s => s.WithModifier("private"))
            )
            .Build();

        var expected = """
                       public class MyClass
                       {
                         public string Name
                         {
                           get;
                           private set;
                         }
                       }
                       """;

        Assert.Equal(Normalize(expected), Normalize(code));
        testOutputHelper.WriteLine(code);
    }

    [Fact]
    public void TestPropertyWithInit()
    {
        var code = new TypeOption()
            .WithTypeKind(TypeOption.Type.Class)
            .WithName("MyClass")
            .WithKeywords("public")
            .Property(p => p
                .WithType("string")
                .WithName("Name")
                .WithKeyword("public")
                .AutoGetter()
                .AutoInit()
            )
            .Build();

        var expected = """
                       public class MyClass
                       {
                         public string Name
                         {
                           get;
                           init;
                         }
                       }
                       """;

        Assert.Equal(Normalize(expected), Normalize(code));
        testOutputHelper.WriteLine(code);
    }

    [Fact]
    public void TestExpressionBodyProperty()
    {
        var code = new TypeOption()
            .WithTypeKind(TypeOption.Type.Class)
            .WithName("MyClass")
            .WithKeywords("public")
            .Field(f => f.WithType("string").WithName("_name").WithKeyword("private"))
            .Property(p => p
                .WithType("string")
                .WithName("Name")
                .WithKeyword("public")
                .AsExpressionBody()
                .AppendLines("_name")
            )
            .Build();

        var expected = """
                       public class MyClass
                       {
                         private string _name;
                         public string Name => _name;
                       }
                       """;

        Assert.Equal(Normalize(expected), Normalize(code));
        testOutputHelper.WriteLine(code);
    }

    [Fact]
    public void TestReadOnlyProperty()
    {
        var code = new TypeOption()
            .WithTypeKind(TypeOption.Type.Class)
            .WithName("MyClass")
            .WithKeywords("public")
            .Field(f => f.WithType("int").WithName("_count").WithKeyword("private"))
            .Property(p => p
                .WithType("int")
                .WithName("Count")
                .WithKeyword("public")
                .Getter(g => g.AsExpressionBody().AppendLines("_count"))
            )
            .Build();

        var expected = """
                       public class MyClass
                       {
                         private int _count;
                         public int Count
                         {
                           get => _count;
                         }
                       }
                       """;

        Assert.Equal(Normalize(expected), Normalize(code));
        testOutputHelper.WriteLine(code);
    }

    [Fact]
    public void TestComplexNestedClass()
    {
        var code = new TypeOption()
            .WithTypeKind(TypeOption.Type.Class)
            .WithName("OuterClass")
            .WithKeywords("public")
            .AutoProperty(p => p.WithType("string").WithName("OuterProperty").WithKeyword("public"))
            .NestedClass(nc => nc
                .WithKeywords("public")
                .WithName("InnerClass")
                .WithKeyword("partial")
                .Property(p => p
                    .WithType("string")
                    .WithName("InnerProperty")
                    .WithKeyword("public")
                    .Getter(g => g.AppendLines("return _innerField;"))
                    .Setter(s => s.WithModifier("private").AppendLines("_innerField = value;"))
                )
                .Field(f => f.WithType("string").WithName("_innerField").WithKeyword("private"))
            )
            .Build();

        testOutputHelper.WriteLine(code);

        // Verify that nested class contains its own members
        Assert.Contains("public partial class InnerClass", code);
        Assert.Contains("public string InnerProperty", code);
        Assert.Contains("private string _innerField", code);
    }

    #region Primary Constructor Tests

    [Fact]
    public void TestPrimaryConstructor()
    {
        var code = new TypeOption()
            .WithTypeKind(TypeOption.Type.Class)
            .WithName("MyClass")
            .WithKeywords("public")
            .WithPrimaryConstructor("string name", "int age")
            .AutoProperty(p => p.WithType("string").WithName("Name").WithKeyword("public"))
            .Build();

        var expected = """
                       public class MyClass(string name, int age)
                       {
                         public string Name { get; set; }
                       }
                       """;

        Assert.Equal(Normalize(expected), Normalize(code));
        testOutputHelper.WriteLine(code);
    }

    [Fact]
    public void TestPrimaryConstructorWithInheritance()
    {
        var code = new TypeOption()
            .WithTypeKind(TypeOption.Type.Class)
            .WithName("DerivedClass")
            .WithKeywords("public")
            .WithPrimaryConstructor("string name", "int age")
            .Inherit("BaseClass")
            .AutoProperty(p => p.WithType("string").WithName("Name").WithKeyword("public"))
            .Build();

        var expected = """
                       public class DerivedClass(string name, int age) : BaseClass
                       {
                         public string Name { get; set; }
                       }
                       """;

        Assert.Equal(Normalize(expected), Normalize(code));
        testOutputHelper.WriteLine(code);
    }

    [Fact]
    public void TestPrimaryConstructorWithInterface()
    {
        var code = new TypeOption()
            .WithTypeKind(TypeOption.Type.Class)
            .WithName("MyClass")
            .WithKeywords("public")
            .WithPrimaryConstructor("string value")
            .Inherit("IInterface")
            .Property(p => p
                .WithType("string")
                .WithName("Value")
                .WithKeyword("public")
                .AsExpressionBody()
                .AppendLines("value")
            )
            .Build();

        var expected = """
                       public class MyClass(string value) : IInterface
                       {
                         public string Value => value;
                       }
                       """;

        Assert.Equal(Normalize(expected), Normalize(code));
        testOutputHelper.WriteLine(code);
    }

    [Fact]
    public void TestPrimaryConstructorWithStruct()
    {
        var code = new TypeOption()
            .WithTypeKind(TypeOption.Type.Struct)
            .WithName("Point")
            .WithKeywords("public readonly")
            .WithPrimaryConstructor("int x", "int y")
            .AutoProperty(p => p.WithType("int").WithName("X").WithKeyword("public"))
            .AutoProperty(p => p.WithType("int").WithName("Y").WithKeyword("public"))
            .Build();

        var expected = """
                       public readonly struct Point(int x, int y)
                       {
                         public int X { get; set; }
                         public int Y { get; set; }
                       }
                       """;

        Assert.Equal(Normalize(expected), Normalize(code));
        testOutputHelper.WriteLine(code);
    }

    [Fact]
    public void TestAddPrimaryConstructorParameter()
    {
        var code = new TypeOption()
            .WithTypeKind(TypeOption.Type.Class)
            .WithName("MyClass")
            .WithKeywords("public")
            .AddPrimaryConstructorParameter("string name")
            .AddPrimaryConstructorParameter("int age")
            .Build();

        var expected = """
                       public class MyClass(string name, int age)
                       {
                       }
                       """;

        Assert.Equal(Normalize(expected), Normalize(code));
        testOutputHelper.WriteLine(code);
    }

    #endregion
}
