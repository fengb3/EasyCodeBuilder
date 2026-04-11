# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

EasyCodeBuilder is a .NET library providing a fluent, configuration-based API for dynamic C# code generation. NuGet package: `Fengb3.EasyCodeBuilder`. Targets .NET Standard 2.0 with zero external dependencies.

## Build & Test Commands

```bash
dotnet build EasyCodeBuilder/EasyCodeBuilder.csproj --configuration Release
dotnet test EasyCodeBuilder.Test/EasyCodeBuilder.Test.csproj --configuration Release
dotnet pack EasyCodeBuilder/EasyCodeBuilder.csproj --configuration Release
```

There is no separate lint command — XML doc warnings are checked during build via CI (`CS1591`).

## Architecture

The core rendering pipeline is: **Option objects** configure code structure, then `Build()` walks the tree via `CodeBuilder` (a `StringBuilder` wrapper with indentation tracking).

### Key classes and their roles

- **`CodeOption`** (`Csharp/CodeOption.cs`) — Base class for all code constructs. Has two delegate hooks: `BeforeChildren` and `OnChildren` (both `CodeRenderFragment`). Children are added by composing delegates onto `OnChildren`.
- **`CodeBuilder`** (`CodeBuilder.cs`) — Indentation-aware string builder. `CodeBlock()` opens a `{` block, increments depth, invokes content, then closes `}`. Supports configurable indent char/count and block delimiters.
- **`CodeRenderFragment`** (`CodeRenderFragment.cs`) — `delegate CodeBuilder CodeRenderFragment(CodeBuilder builder)` — the type for all render hooks.
- **`Code`** (`Csharp/Code.cs`) — Static entry point. `Create()` returns a `CodeOption`. Contains `AddChild<TParent,TChild>()` which creates a child option, configures it, and appends its `Build` to the parent's `OnChildren`.

### Option hierarchy

Each code construct (namespace, type, method, property, etc.) is a `CodeOption` subclass that overrides `Build()` to emit its header (e.g. `public class Foo`), then calls `cb.CodeBlock(OnChildren)` to render the body.

Extension methods (in the same file or in `TypeOptionExtensions`, `MethodOptionExtensions`, `NameSpaceOptionsExtensions`) add fluent `With*()` and child-adding methods.

### Keyword configurator pattern

`KeywordOptionConfigurator<TParent>` (`Csharp/OptionConfigurations/KeywordOptionConfigurator.cs`) is a fluent chain for access modifiers and keywords (`.Public`, `.Private`, `.Static`, etc.). It collects keywords into a `HashSet`, then `Configure()` applies them to the child option.

`KeywordConfiguratorExtensions` (`Csharp/OptionConfigurations/OptionConfigurations.cs`) bridges the configurator to concrete option types — e.g. `.Public.Class(...)` on `NamespaceOption`, `.Public.Method(...)` on `TypeOption`.

### Keyword ordering

`CsharpKeywordOrdering` (`Csharp/CsharpKeywordOrdering.cs`) provides rank-based sorting so keywords appear in correct C# order regardless of how they were added. Separate rank tables for type declarations and members.

## Naming conventions

- Option classes follow the `*Option` pattern: `TypeOption`, `MethodOption`, `NamespaceOption`, etc.
- Extension methods for each option type live in a `*Extensions` static class in the same file
- Fluent `With*()` methods return the option itself for chaining
- Add-child methods accept `Action<T>` or `Func<T, T>` delegates (both overloads provided)
- Comments and XML docs are a mix of English and Chinese

## Test structure

Tests are in `EasyCodeBuilder.Test/Csharp/`. They construct option trees and assert against expected generated code strings. Tests serve as the primary usage documentation — see `ReadmeExamplesVerificationTests.cs` for README examples and `CsharpCodeOptionConfiguratorTests.cs` for keyword configurator tests.

## CI

GitHub Actions (`ci.yml`): builds on Windows/Linux/macOS with .NET 8 SDK, runs tests, verifies package creation, checks for missing XML docs. Publishing (`nuget-publish.yml`) auto-detects version bumps on main.
