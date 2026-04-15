using System;
using Fengb3.EasyCodeBuilder;
using Fengb3.EasyCodeBuilder.Csharp;
using Xunit.Abstractions;

namespace EasyCodeBuilder.Test.Csharp;

public class TryCatchFinallyTests(ITestOutputHelper output)
{
    private static string Norm(string text) => text.Replace("\r\n", "\n").Trim();

    [Fact]
    public void TryWithoutCatchOrFinallyThrows()
    {
        var tryOption = new TryOption();
        tryOption.AppendLine("DoWork();");

        Assert.Throws<InvalidOperationException>(() =>
        {
            var cb = new CodeBuilder(' ', 2, "\n{", "}", 1024);
            tryOption.Build(cb);
        });
    }

    [Fact]
    public void CatchAfterFinallyThrows()
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            var tryOption = new TryOption();
            tryOption.Finally(f => f.AppendLine("Cleanup();"));
            tryOption.Catch(c => c.AppendLine("Handle();"));
        });
    }

    [Fact]
    public void MultipleFinallyThrows()
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            var tryOption = new TryOption();
            tryOption.Finally(f => f.AppendLine("Cleanup1();"));
            tryOption.Finally(f => f.AppendLine("Cleanup2();"));
        });
    }

    [Fact]
    public void NullCatchConfigureThrows()
    {
        var tryOption = new TryOption();
        Assert.Throws<ArgumentNullException>(() => tryOption.Catch(null!));
    }

    [Fact]
    public void NullFinallyConfigureThrows()
    {
        var tryOption = new TryOption();
        Assert.Throws<ArgumentNullException>(() => tryOption.Finally(null!));
    }

    [Fact]
    public void TryCatch()
    {
        var code = new MethodOption()
            .WithKeyword("public")
            .WithName("Run")
            .WithReturnType("void")
            .Try(@try =>
            {
                @try.AppendLine("DoWork();");
                @try.Catch(c =>
                {
                    c.ExceptionType = "Exception";
                    c.VariableName  = "ex";
                    c.AppendLine("Console.WriteLine(ex.Message);");
                });
            })
            .Build();

        var expected = """
            public void Run()
            {
              try
              {
                DoWork();
              }
              catch (Exception ex)
              {
                Console.WriteLine(ex.Message);
              }
            }
            """;

        Assert.Equal(Norm(expected), Norm(code));
        output.WriteLine(code);
    }

    [Fact]
    public void TryCatchFinally()
    {
        var code = new MethodOption()
            .WithKeyword("public")
            .WithName("Run")
            .WithReturnType("void")
            .Try(@try =>
            {
                @try.AppendLine("DoWork();");
                @try.Catch(c =>
                {
                    c.ExceptionType = "Exception";
                    c.VariableName  = "ex";
                    c.AppendLine("Log(ex);");
                });
                @try.Finally(f =>
                {
                    f.AppendLine("Cleanup();");
                });
            })
            .Build();

        var expected = """
            public void Run()
            {
              try
              {
                DoWork();
              }
              catch (Exception ex)
              {
                Log(ex);
              }
              finally
              {
                Cleanup();
              }
            }
            """;

        Assert.Equal(Norm(expected), Norm(code));
        output.WriteLine(code);
    }

    [Fact]
    public void TryMultipleCatch()
    {
        var code = new MethodOption()
            .WithKeyword("public")
            .WithName("Run")
            .WithReturnType("void")
            .Try(@try =>
            {
                @try.AppendLine("DoWork();");
                @try.Catch(c =>
                {
                    c.ExceptionType = "ArgumentNullException";
                    c.VariableName  = "ex";
                    c.AppendLine("HandleNull(ex);");
                });
                @try.Catch(c =>
                {
                    c.ExceptionType = "InvalidOperationException";
                    c.VariableName  = "ex";
                    c.AppendLine("HandleInvalid(ex);");
                });
                @try.Catch(c =>
                {
                    c.ExceptionType = "Exception";
                    c.VariableName  = "ex";
                    c.AppendLine("HandleGeneral(ex);");
                });
            })
            .Build();

        Assert.Contains("catch (ArgumentNullException ex)", code);
        Assert.Contains("catch (InvalidOperationException ex)", code);
        Assert.Contains("catch (Exception ex)", code);
        Assert.Contains("HandleNull(ex);", code);
        Assert.Contains("HandleInvalid(ex);", code);
        Assert.Contains("HandleGeneral(ex);", code);
        output.WriteLine(code);
    }

    [Fact]
    public void TryCatchWithoutVariableName()
    {
        var code = new MethodOption()
            .WithKeyword("public")
            .WithName("Run")
            .WithReturnType("void")
            .Try(@try =>
            {
                @try.AppendLine("DoWork();");
                @try.Catch(c =>
                {
                    c.ExceptionType = "Exception";
                    c.AppendLine("HandleError();");
                });
            })
            .Build();

        Assert.Contains("catch (Exception)", code);
        Assert.Contains("HandleError();", code);
        output.WriteLine(code);
    }

    [Fact]
    public void BareCatch()
    {
        var code = new MethodOption()
            .WithKeyword("public")
            .WithName("Run")
            .WithReturnType("void")
            .Try(@try =>
            {
                @try.AppendLine("DoWork();");
                @try.Catch(c =>
                {
                    c.AppendLine("HandleError();");
                });
            })
            .Build();

        Assert.Contains("catch", code);
        Assert.DoesNotContain("catch (", code);
        Assert.Contains("HandleError();", code);
        output.WriteLine(code);
    }

    [Fact]
    public void TryCatchWithWhenFilter()
    {
        var code = new MethodOption()
            .WithKeyword("public")
            .WithName("Run")
            .WithReturnType("void")
            .Try(@try =>
            {
                @try.AppendLine("DoWork();");
                @try.Catch(c =>
                {
                    c.WithExceptionType("Exception")
                     .WithVariableName("ex")
                     .WithWhenFilter("ex.Message.Contains(\"timeout\")")
                     .AppendLine("HandleTimeout(ex);");
                });
            })
            .Build();

        Assert.Contains("catch (Exception ex) when (ex.Message.Contains(\"timeout\"))", code);
        Assert.Contains("HandleTimeout(ex);", code);
        output.WriteLine(code);
    }

    [Fact]
    public void TryFinallyWithoutCatch()
    {
        var code = new MethodOption()
            .WithKeyword("public")
            .WithName("Run")
            .WithReturnType("void")
            .Try(@try =>
            {
                @try.AppendLine("DoWork();");
                @try.Finally(f =>
                {
                    f.AppendLine("Cleanup();");
                });
            })
            .Build();

        var expected = """
            public void Run()
            {
              try
              {
                DoWork();
              }
              finally
              {
                Cleanup();
              }
            }
            """;

        Assert.Equal(Norm(expected), Norm(code));
        output.WriteLine(code);
    }
}
