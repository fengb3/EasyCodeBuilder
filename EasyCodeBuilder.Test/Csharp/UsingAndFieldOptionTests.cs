using Fengb3.EasyCodeBuilder.Csharp;
using Fengb3.EasyCodeBuilder.Csharp.OptionConfigurations;
using Xunit;

namespace EasyCodeBuilder.Test.Csharp;

public class UsingAndFieldOptionTests
{
    [Fact]
    public void UsingOption_CanBuildStaticAliasAndGlobal()
    {
        var code = Code.Create()
            .Using(u => u.WithName("System").WithBlankLine())
            .UsingStatic("System.Math")
            .UsingAlias("IO", "System.IO")
            .GlobalUsing("System.Text")
            .Build();

        var expected =
            """
            using System;

            using static System.Math;
            using IO = System.IO;
            global using System.Text;
            """;

        Assert.Equal(expected.Trim(), code.Trim());
    }

    [Fact]
    public void FieldOption_CanBuildFieldWithInitializer()
    {
        var code = new TypeOption()
            .WithTypeKind(TypeOption.Type.Class)
            .WithName("MyClass")
            .WithKeywords("public")
            .Private.Readonly.Field(f => f
                .WithType("int")
                .WithName("_x")
                .WithInitializer("1")
            )
            .Build();

        var expected =
            """
            public class MyClass
            {
              private readonly int _x = 1;
            }
            """;

        Assert.Equal(expected.Trim(), code.Trim());
    }
}

