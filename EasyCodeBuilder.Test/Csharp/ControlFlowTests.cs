using Fengb3.EasyCodeBuilder.Csharp;
using Xunit.Abstractions;

namespace EasyCodeBuilder.Test.Csharp;

public class ControlFlowTests(ITestOutputHelper output)
{
    private static string Norm(string text) => text.Replace("\r\n", "\n").Trim();

    [Fact]
    public void IfStatement()
    {
        var code = new MethodOption()
            .WithKeyword("public")
            .WithName("Check")
            .WithReturnType("void")
            .If(@if =>
            {
                @if.Condition = "x > 0";
                @if.AppendLine("Console.WriteLine(x);");
            })
            .Build();

        var expected = """
            public void Check()
            {
              if (x > 0)
              {
                Console.WriteLine(x);
              }
            }
            """;

        Assert.Equal(Norm(expected), Norm(code));
        output.WriteLine(code);
    }

    [Fact]
    public void IfElseStatement()
    {
        var code = new MethodOption()
            .WithKeyword("public")
            .WithName("Check")
            .WithReturnType("void")
            .If(@if =>
            {
                @if.Condition = "x > 0";
                @if.AppendLine("Console.WriteLine(\"positive\");");
            })
            .Else(@else =>
            {
                @else.AppendLine("Console.WriteLine(\"non-positive\");");
            })
            .Build();

        var expected = """
            public void Check()
            {
              if (x > 0)
              {
                Console.WriteLine("positive");
              }
              else
              {
                Console.WriteLine("non-positive");
              }
            }
            """;

        Assert.Equal(Norm(expected), Norm(code));
        output.WriteLine(code);
    }

    [Fact]
    public void IfElseIfElseStatement()
    {
        var code = new MethodOption()
            .WithKeyword("public")
            .WithName("Classify")
            .WithReturnType("void")
            .If(@if =>
            {
                @if.Condition = "x > 0";
                @if.AppendLine("Console.WriteLine(\"positive\");");
            })
            .ElseIf(@elseIf =>
            {
                @elseIf.Condition = "x < 0";
                @elseIf.AppendLine("Console.WriteLine(\"negative\");");
            })
            .Else(@else =>
            {
                @else.AppendLine("Console.WriteLine(\"zero\");");
            })
            .Build();

        // Verify structural output contains all branches
        Assert.Contains("if (x > 0)", code);
        Assert.Contains("else if (x < 0)", code);
        Assert.Contains("else", code);
        Assert.Contains("Console.WriteLine(\"positive\");", code);
        Assert.Contains("Console.WriteLine(\"negative\");", code);
        Assert.Contains("Console.WriteLine(\"zero\");", code);
        output.WriteLine(code);
    }

    [Fact]
    public void NestedIfStatement()
    {
        var code = new MethodOption()
            .WithKeyword("public")
            .WithName("Check")
            .WithReturnType("void")
            .If(outer =>
            {
                outer.Condition = "x > 0";
                outer.AddChild<IfOption, IfOption>(inner =>
                {
                    inner.Condition = "x > 100";
                    inner.AppendLine("Console.WriteLine(\"large\");");
                });
            })
            .Build();

        Assert.Contains("if (x > 0)", code);
        Assert.Contains("if (x > 100)", code);
        Assert.Contains("Console.WriteLine(\"large\");", code);
        // Verify nesting: inner if should be indented more
        Assert.Contains("    if (x > 100)", code.Replace("\r\n", "\n"));
        output.WriteLine(code);
    }

    [Fact]
    public void WhileLoop()
    {
        var code = new MethodOption()
            .WithKeyword("public")
            .WithName("Loop")
            .WithReturnType("void")
            .While(@while =>
            {
                @while.Condition = "running";
                @while.AppendLine("DoWork();");
            })
            .Build();

        var expected = """
            public void Loop()
            {
              while (running)
              {
                DoWork();
              }
            }
            """;

        Assert.Equal(Norm(expected), Norm(code));
        output.WriteLine(code);
    }

    [Fact]
    public void DoWhileLoop()
    {
        var code = new MethodOption()
            .WithKeyword("public")
            .WithName("Loop")
            .WithReturnType("void")
            .DoWhile(@do =>
            {
                @do.Condition = "running";
                @do.AppendLine("DoWork();");
            })
            .Build();

        var expected = """
            public void Loop()
            {
              do
              {
                DoWork();
              }while (running);
            }
            """;

        Assert.Equal(Norm(expected), Norm(code));
        output.WriteLine(code);
    }

    [Fact]
    public void ForeachLoop()
    {
        var code = new MethodOption()
            .WithKeyword("public")
            .WithName("Iterate")
            .WithReturnType("void")
            .Foreach(@foreach =>
            {
                @foreach.WithVariable("var", "item", "items");
                @foreach.AppendLine("Console.WriteLine(item);");
            })
            .Build();

        var expected = """
            public void Iterate()
            {
              foreach (var item in items)
              {
                Console.WriteLine(item);
              }
            }
            """;

        Assert.Equal(Norm(expected), Norm(code));
        output.WriteLine(code);
    }

    [Fact]
    public void ForLoopStandalone()
    {
        var code = new MethodOption()
            .WithKeyword("private")
            .WithName("Count")
            .WithReturnType("void")
            .For(@for =>
            {
                @for.WithInitializer("int i = 0");
                @for.WithCondition("i < 5");
                @for.WithIterator("i++");
                @for.AppendLine("sum += i;");
            })
            .Build();

        var expected = """
            private void Count()
            {
              for (int i = 0; i < 5; i++)
              {
                sum += i;
              }
            }
            """;

        Assert.Equal(Norm(expected), Norm(code));
        output.WriteLine(code);
    }

    [Fact]
    public void SwitchWithExpressionBody()
    {
        var method = new MethodOption()
            .WithKeyword("public")
            .WithName("GetLabel")
            .WithReturnType("string")
            .WithParameters("int value");

        method.Switch(@switch =>
        {
            @switch.Expression = "value";
            @switch.Case(@case =>
            {
                @case.Value = "1";
                @case.AppendLine("return \"One\";");
            });
            @switch.Case(@case =>
            {
                @case.Value = "2";
                @case.AppendLine("return \"Two\";");
            });
            @switch.Default(@default => @default.AppendLine("return \"Unknown\";"));
        });

        var code = method.Build();

        Assert.Contains("switch (value)", code);
        Assert.Contains("case 1:", code);
        Assert.Contains("case 2:", code);
        Assert.Contains("default:", code);
        Assert.Contains("return \"One\";", code);
        Assert.Contains("return \"Unknown\";", code);
        output.WriteLine(code);
    }

    [Fact]
    public void ForContainingSwitch_Issue11()
    {
        var code = new MethodOption()
            .WithKeyword("public")
            .WithName("Parse")
            .WithReturnType("void")
            .WithParameters("string[] args")
            .For(f =>
            {
                f.WithInitializer("int i = 0")
                 .WithCondition("i < args.Length")
                 .WithIterator("i++");

                f.Switch(s =>
                {
                    s.WithExpression("args[i]");
                    s.Case(c =>
                    {
                        c.Value = "\"--x\"";
                        c.AppendLine("x = int.Parse(args[++i]);");
                        c.AppendLine("break;");
                    });
                    s.Default(d =>
                    {
                        d.AppendLine("break;");
                    });
                });
            })
            .Build();

        var expected = """
            public void Parse(string[] args)
            {
              for (int i = 0; i < args.Length; i++)
              {
                switch (args[i])
                {
                  case "--x":
                  {
                    x = int.Parse(args[++i]);
                    break;
                  }
                  default:
                  {
                    break;
                  }
                }
              }
            }
            """;

        Assert.Equal(Norm(expected), Norm(code));
        output.WriteLine(code);
    }

    [Fact]
    public void DeepNesting_ForSwitchCaseIf()
    {
        var code = new MethodOption()
            .WithKeyword("public")
            .WithName("Deep")
            .WithReturnType("void")
            .For(f =>
            {
                f.WithInitializer("int i = 0")
                 .WithCondition("i < items.Count")
                 .WithIterator("i++");

                f.Switch(s =>
                {
                    s.WithExpression("items[i].Type");
                    s.Case(c =>
                    {
                        c.Value = "\"A\"";
                        c.If(@if =>
                        {
                            @if.Condition = "items[i].Value > 0";
                            @if.AppendLine("Process(items[i]);");
                        });
                    });
                });
            })
            .Build();

        Assert.Contains("for (int i = 0; i < items.Count; i++)", code);
        Assert.Contains("switch (items[i].Type)", code);
        Assert.Contains("case \"A\":", code);
        Assert.Contains("if (items[i].Value > 0)", code);
        Assert.Contains("Process(items[i]);", code);
        output.WriteLine(code);
    }

    [Fact]
    public void WhileContainingFor()
    {
        var code = new MethodOption()
            .WithKeyword("public")
            .WithName("Retry")
            .WithReturnType("void")
            .While(w =>
            {
                w.Condition = "retry";
                w.For(f =>
                {
                    f.WithInitializer("int i = 0")
                     .WithCondition("i < retries")
                     .WithIterator("i++");
                    f.AppendLine("Attempt(i);");
                });
            })
            .Build();

        Assert.Contains("while (retry)", code);
        Assert.Contains("for (int i = 0; i < retries; i++)", code);
        Assert.Contains("Attempt(i);", code);
        output.WriteLine(code);
    }

    [Fact]
    public void TryInsideFor()
    {
        var code = new MethodOption()
            .WithKeyword("public")
            .WithName("SafeIterate")
            .WithReturnType("void")
            .WithParameters("List<string> items")
            .For(f =>
            {
                f.WithInitializer("int i = 0")
                 .WithCondition("i < items.Count")
                 .WithIterator("i++");

                f.Try(t =>
                {
                    t.AppendLine("Process(items[i]);");
                    t.Catch(c =>
                    {
                        c.WithExceptionType("Exception")
                         .WithVariableName("ex");
                        c.AppendLine("Log(ex);");
                    });
                });
            })
            .Build();

        Assert.Contains("for (int i = 0; i < items.Count; i++)", code);
        Assert.Contains("try", code);
        Assert.Contains("Process(items[i]);", code);
        Assert.Contains("catch (Exception ex)", code);
        Assert.Contains("Log(ex);", code);
        output.WriteLine(code);
    }

    [Fact]
    public void IfElseIfElseInsideFor()
    {
        var code = new MethodOption()
            .WithKeyword("public")
            .WithName("Classify")
            .WithReturnType("void")
            .For(f =>
            {
                f.WithInitializer("int i = 0")
                 .WithCondition("i < values.Length")
                 .WithIterator("i++");

                f.If(@if =>
                {
                    @if.Condition = "values[i] > 0";
                    @if.AppendLine("Console.WriteLine(\"positive\");");
                })
                .ElseIf(elseIf =>
                {
                    elseIf.Condition = "values[i] < 0";
                    elseIf.AppendLine("Console.WriteLine(\"negative\");");
                })
                .Else(@else =>
                {
                    @else.AppendLine("Console.WriteLine(\"zero\");");
                });
            })
            .Build();

        Assert.Contains("for (int i = 0; i < values.Length; i++)", code);
        Assert.Contains("if (values[i] > 0)", code);
        Assert.Contains("else if (values[i] < 0)", code);
        Assert.Contains("else", code);
        Assert.Contains("Console.WriteLine(\"positive\");", code);
        Assert.Contains("Console.WriteLine(\"negative\");", code);
        Assert.Contains("Console.WriteLine(\"zero\");", code);
        output.WriteLine(code);
    }
}
