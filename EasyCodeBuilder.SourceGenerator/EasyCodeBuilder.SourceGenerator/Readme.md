# Roslyn Source Generators Sample

A set of three projects that illustrates Roslyn source generators. Enjoy this template to learn from and modify source generators for your own needs.

## Content
### EasyCodeBuilder.SourceGenerator
A .NET Standard project with implementations of sample source generators.
**You must build this project to see the result (generated code) in the IDE.**

- [SampleSourceGenerator.cs](SampleSourceGenerator.cs): A source generator that creates C# classes based on a text file (in this case, Domain Driven Design ubiquitous language registry).
- [SampleIncrementalSourceGenerator.cs](SampleIncrementalSourceGenerator.cs): A source generator that creates a custom report based on class properties. The target class should be annotated with the `Generators.ReportAttribute` attribute.

### EasyCodeBuilder.SourceGenerator.Sample
A project that references source generators. Note the parameters of `ProjectReference` in [EasyCodeBuilder.SourceGenerator.Sample.csproj](../EasyCodeBuilder.SourceGenerator.Sample/EasyCodeBuilder.SourceGenerator.Sample.csproj), they make sure that the project is referenced as a set of source generators. 

### EasyCodeBuilder.SourceGenerator.Tests
Unit tests for source generators. The easiest way to develop language-related features is to start with unit tests.

## How To?
### How to debug?
- Use the [launchSettings.json](Properties/launchSettings.json) profile.
- Debug tests.

### How can I determine which syntax nodes I should expect?
Consider using the Roslyn Visualizer toolwindow, witch allow you to observe syntax tree.

### How to learn more about wiring source generators?
Watch the walkthrough video: [Let’s Build an Incremental Source Generator With Roslyn, by Stefan Pölz](https://youtu.be/azJm_Y2nbAI)
The complete set of information is available in [Source Generators Cookbook](https://github.com/dotnet/roslyn/blob/main/docs/features/source-generators.cookbook.md).