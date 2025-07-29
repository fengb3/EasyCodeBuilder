using System.Collections.Generic;
using Fengb3.EasyCodeBuilder;
using Microsoft.CodeAnalysis;

namespace EasyCodeBuilder.SourceGenerator;

[Generator]
public class EasyCodeBuilderDemoGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var cb = new CSharpCodeBuilder();

        cb.Using("System.Text.Json");

        List<(string Name, string Value)> variables = [
            (Name: "a", Value: "233"),
            (Name: "b", Value: "445"),
            (Name: "c", Value: "667")
        ];
        

        cb.Namespace("EasyCodeBuilderDemo")
        (
            ns => ns.Class("SampleClass")
            (
                cls => cls.Method("SampleMethod")
                (
                    method =>
                        method.AppendBatch(variables, (mth, variable) =>
                        {
                            mth.AppendLine($"var {variable.Name} = {variable.Value};");
                            mth.If($" {variable.Name} == {variable.Name[0]}")(
                                @if => @if
                                    .AppendLine("System.Console.WriteLine(" + variable.Name[0] + ");")
                                    .AppendLine("System.Console.WriteLine(a);")
                                    .AppendLine("System.Console.WriteLine(\"Wow, a and " + variable.Name[0] + " are same !!!\");")
                            );
                            return mth;
                        })
                )
            )
        );
        
        context.RegisterPostInitializationOutput(ctx =>
        {
            ctx.AddSource("EasyCodeBuilderDemo.cs", cb.ToString());
        });
        
    }
}