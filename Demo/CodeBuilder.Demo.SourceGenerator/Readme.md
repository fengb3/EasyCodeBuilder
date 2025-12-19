# Roslyn Source Generators Sample with Fengb3.EasyCodeBuilder

this project is modified from Roslyn Source Generator Example project, it is mean to demonstrate how to integrate `Fengb3.EasyCodeBuilder` into `Roslyn Source Generators`. 

## Content
### CodeBuilder.Demo.SourceGenerator
following source generators are implemented with `Fengb3.EasyCodeBuilder`.
**You must build this project to see the result (generated code) in the IDE.**

- [SampleSourceGenerator.cs](SampleSourceGenerator.cs): A source generator that creates C# classes based on a text file (in this case, Domain Driven Design ubiquitous language registry).
- [SampleIncrementalSourceGenerator.cs](SampleIncrementalSourceGenerator.cs): A source generator that creates a custom report based on class properties. The target class should be annotated with the `Generators.ReportAttribute` attribute.
- [BuilderSourceGenerator.cs](BuilderSourceGenerator.cs): A source generator that creates builder classes for annotated classes. The target class should be annotated with the `Generators.BuilderAttribute` attribute.

### CodeBuilder.Demo.SourceGenerator.Sample
A project that references source generators. Note the parameters of `ProjectReference` in [CodeBuilder.Demo.SourceGenerator.Sample.csproj](../CodeBuilder.Demo.SourceGenerator.Sample/CodeBuilder.Demo.SourceGenerator.Sample.csproj), they make sure that the project is referenced as a set of source generators. 

## How To?
### How to debug?
- Use the [launchSettings.json](Properties/launchSettings.json) profile.
- Debug tests.

### How to Use Fengb3.EasyCodeBuilder in your own source generator project? 
Refer to [CodeBuilder.Demo.SourceGenerator.csproj](CodeBuilder.Demo.SourceGenerator.csproj) to see how to reference `Fengb3.EasyCodeBuilder` nuget package in your source generator project.
Focus on comments with `👇` symbol.

### How can I determine which syntax nodes I should expect?
Consider using the Roslyn Visualizer tool window, which allows you to observe the syntax tree.

### How to learn more about wiring source generators?
Watch the walkthrough video: [Let’s Build an Incremental Source Generator With Roslyn, by Stefan Pölz](https://youtu.be/azJm_Y2nbAI)
The complete set of information is available in [Source Generators Cookbook](https://github.com/dotnet/roslyn/blob/main/docs/features/source-generators.cookbook.md).