using Fengb3.EasyCodeBuilder.Csharp;
using Fengb3.EasyCodeBuilder.Csharp.OptionConfigurations;
using Xunit.Abstractions;

namespace EasyCodeBuilder.Test.Csharp;

public class PartialKeywordTests(ITestOutputHelper testOutputHelper)
{
    private static string Normalize(string text) => text.Replace("\r\n", "\n").Trim();

    [Fact]
    public void TestPartialClassInNamespace()
    {
        var @namespace = new NamespaceOption()
            .WithName("MyNamespace")
            .Partial.Class(cls =>
            {
                cls.WithName("MyClass");
            });

        var code = @namespace.Build();

        const string expected =
            """
            namespace MyNamespace
            {
              partial class MyClass
              {
              }
            }
            """;

        Assert.Equal(Normalize(expected), Normalize(code));
        testOutputHelper.WriteLine(code);
    }

    [Fact]
    public void TestPublicPartialClass()
    {
        var @namespace = new NamespaceOption()
            .WithName("MyNamespace")
            .Public.Partial.Class(cls =>
            {
                cls.WithName("MyClass");
            });

        var code = @namespace.Build();

        const string expected =
            """
            namespace MyNamespace
            {
              public partial class MyClass
              {
              }
            }
            """;

        Assert.Equal(Normalize(expected), Normalize(code));
        testOutputHelper.WriteLine(code);
    }

    [Fact]
    public void TestPublicStaticPartialClass()
    {
        var @namespace = new NamespaceOption()
            .WithName("MyNamespace")
            .Public.Static.Partial.Class(cls =>
            {
                cls.WithName("MyClass");
            });

        var code = @namespace.Build();

        const string expected =
            """
            namespace MyNamespace
            {
              public static partial class MyClass
              {
              }
            }
            """;

        Assert.Equal(Normalize(expected), Normalize(code));
        testOutputHelper.WriteLine(code);
    }

    [Fact]
    public void TestPartialNestedClass()
    {
        var code = new TypeOption()
            .WithTypeKind(TypeOption.Type.Class)
            .WithName("OuterClass")
            .WithKeywords("public")
            .Partial.NestedClass(nc => nc
                .WithName("InnerClass")
            )
            .Build();

        const string expected =
            """
            public class OuterClass
            {
              partial class InnerClass
              {
              }
            }
            """;

        Assert.Equal(Normalize(expected), Normalize(code));
        testOutputHelper.WriteLine(code);
    }

    [Fact]
    public void TestPublicPartialNestedClass()
    {
        var code = new TypeOption()
            .WithTypeKind(TypeOption.Type.Class)
            .WithName("OuterClass")
            .WithKeywords("public")
            .Public.Partial.NestedClass(nc => nc
                .WithName("InnerClass")
            )
            .Build();

        const string expected =
            """
            public class OuterClass
            {
              public partial class InnerClass
              {
              }
            }
            """;

        Assert.Equal(Normalize(expected), Normalize(code));
        testOutputHelper.WriteLine(code);
    }
}
