# Easy Code Builder

[![NuGet Version](https://img.shields.io/nuget/v/Fengb3.EasyCodeBuilder)](https://www.nuget.org/packages/Fengb3.EasyCodeBuilder/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
![CI](https://github.com/fengb3/EasyCodeBuilder/workflows/🔄%20Continuous%20Integration/badge.svg)
![NuGet](https://github.com/fengb3/EasyCodeBuilder/workflows/🚀%20NuGet%20Package%20CI/CD/badge.svg)

A powerful and flexible library for dynamic C# code generation using a configuration-based API.

## ✨ Features

- **⚙️ Configuration-Based API**: Use option objects to configure code structures
- **🏗️ Type-Safe Builder**: Strongly typed options for C# code generation
- **🔧 Flexible Configuration**: Declarative approach with extension methods
- **📚 Rich Code Constructs**: Namespaces, classes, methods, properties, control structures, and more
- **⚡ High Performance**: Optimized string building with efficient rendering
- **🛡️ Type Safety**: Compile-time safety with strongly typed options
- **📖 XML Documentation**: Complete IntelliSense support

## 🚀 Quick Start

### Installation

```bash
dotnet add package Fengb3.EasyCodeBuilder
```

### Basic Usage

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API Calling**:
```csharp
using Fengb3.EasyCodeBuilder.Csharp;
using static Fengb3.EasyCodeBuilder.Csharp.Code;

var code = Create()
    .Using("System")
    .Namespace(ns => {
        ns.Name = "MyProject";
        ns.Public.Class(cls => {
            cls.WithName("Person");
            cls.Public.AutoProperty(p => p
                .WithType("string")
                .WithName("Name")
            );
            cls.Public.AutoProperty(p => p
                .WithType("int")
                .WithName("Age")
            );
        });
    })
    .Build();
```

</div>
<div style="flex: 1;">

**Generated Code**:
```csharp
using System;

namespace MyProject
{
  public class Person
  {
    public string Name { get; set; }
    public int Age { get; set; }
  }
}
```

</div>
</div>

## 📚 Documentation

For detailed documentation and examples, see this README and the test project examples in the repository.

## 🏗️ Architecture

EasyCodeBuilder v0.1.0 uses a configuration-based approach with option objects:

| Component | Description |
|-----------|-------------|
| **`CodeOption`** | Base class for all code options |
| **`Code.Create()`** | Entry point for creating code |
| **`NamespaceOption`** | Configure namespaces |
| **`TypeOption`** | Configure classes, structs, interfaces |
| **`MethodOption`** | Configure methods |
| **`AutoPropertyOption`** | Configure auto-properties |
| **`IfOption/ElseIfOption/ElseOption`** | Configure conditional statements |
| **`ForOption/WhileOption/ForeachOption`** | Configure loops |
| **`SwitchOption`** | Configure switch statements |

## 📊 Examples

### Generate a Class with Methods

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API Calling**:
```csharp
using static Fengb3.EasyCodeBuilder.Csharp.Code;

var code = Create()
    .Using("System")
    .Namespace(ns => {
        ns.Name = "MyApp";
        ns.Public.Class(cls => {
            cls.WithName("Calculator");
            cls.Public.Method(method => {
                method.WithName("Add")
                      .WithReturnType("int")
                      .WithParameters("int a", "int b")
                      .AppendLine("return a + b;");
            });
        });
    })
    .Build();
```

</div>
<div style="flex: 1;">

**Generated Code**:
```csharp
using System;

namespace MyApp
{
  public class Calculator
  {
    public int Add(int a, int b)
    {
      return a + b;
    }
  }
}
```

</div>
</div>

### Using Keyword Configurator

The new `KeywordOptionConfigurator` API provides a fluent way to specify access modifiers and other keywords. Note: You need to import `Fengb3.EasyCodeBuilder.Csharp.OptionConfigurations` to use these extension methods.

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API Calling**:
```csharp
using Fengb3.EasyCodeBuilder.Csharp;
using Fengb3.EasyCodeBuilder.Csharp.OptionConfigurations;

var @namespace = new NamespaceOption()
    .WithName("MyNamespace")
    .Public.Class(cls => {
        cls.WithName("MyClass");
    });

var code = @namespace.Build();
```

</div>
<div style="flex: 1;">

**Generated Code**:
```csharp
namespace MyNamespace
{
  public class MyClass
  {
  }
}
```

</div>
</div>

You can also use the keyword configurator with auto-properties:

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API Calling**:
```csharp
using Fengb3.EasyCodeBuilder.Csharp;
using static Fengb3.EasyCodeBuilder.Csharp.Code;

var @class = Create()
    .Class(cls => cls
        .WithName("MyClass")
        .Public.AutoProperty(prop => prop
            .WithName("MyProperty")
            .WithType(typeof(int).FullName ?? "int")
        )
    );

var classCode = @class.Build();
```

</div>
<div style="flex: 1;">

**Generated Code**:
```csharp
class MyClass
{
  public System.Int32 MyProperty { get; set; }
}
```

</div>
</div>

### Using Constructors

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API Calling**:
```csharp
using static Fengb3.EasyCodeBuilder.Csharp.Code;

var classOption = new TypeOption()
    .WithTypeKind(TypeOption.Type.Class)
    .WithName("Person")
    .WithKeyword("public")
    .Constructor(ctor => {
        ctor.WithKeyword("public")
            .WithParameter("string name")  // WithParameter for constructors
            .WithParameter("int age")      // WithParameter for constructors
            .AppendLine("Name = name;")
            .AppendLine("Age = age;");
    })
    .AutoProperty(p => p
        .WithKeyword("public")
        .WithType("string")
        .WithName("Name"))
    .AutoProperty(p => p
        .WithKeyword("public")
        .WithType("int")
        .WithName("Age"));

var code = classOption.Build();
```

</div>
<div style="flex: 1;">

**Generated Code**:
```csharp
public class Person
{
  public Person(string name, int age)
  {
    Name = name;
    Age = age;
  }
  public string Name { get; set; }
  public int Age { get; set; }
}
```

</div>
</div>

### Control Structures - For Loop

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API Calling**:
```csharp
using static Fengb3.EasyCodeBuilder.Csharp.Code;

var method = new MethodOption()
    .WithName("PrintNumbers")
    .WithReturnType("void")
    .WithKeyword("public")
    .For(@for => {
        @for.WithInitializer("int i = 0")
            .WithCondition("i < 10")
            .WithIterator("i++")
            .AppendLine("Console.WriteLine(i);");
    });

var code = method.Build();
```

</div>
<div style="flex: 1;">

**Generated Code**:
```csharp
public void PrintNumbers()
{
  for (int i = 0; i < 10; i++)
  {
    Console.WriteLine(i);
  }
}
```

</div>
</div>

### Control Structures - Switch Statement

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API Calling**:
```csharp
using static Fengb3.EasyCodeBuilder.Csharp.Code;

var method = new MethodOption()
    .WithName("GetDayName")
    .WithReturnType("string")
    .WithKeyword("public")
    .WithParameters("int day");  // WithParameters for methods

method.Switch(@switch => {
    @switch.Expression = "day";
    @switch.Case(@case => {
        @case.Value = "1";
        @case.AppendLine("return \"Monday\";");
    });
    @switch.Case(@case => {
        @case.Value = "2";
        @case.AppendLine("return \"Tuesday\";");
    });
    @switch.Default(@default => 
        @default.AppendLine("return \"Unknown\";")
    );
});

var code = method.Build();
```

</div>
<div style="flex: 1;">

**Generated Code**:
```csharp
public string GetDayName(int day)
{
  switch (day)
  {
    case 1:
    {
      return "Monday";
    }
    case 2:
    {
      return "Tuesday";
    }
    default:
    {
      return "Unknown";
    }
  }
}
```

</div>
</div>

## 🎯 Key Concepts

### Option-Based Configuration

EasyCodeBuilder v0.1.0 uses a configuration-based approach where you:

1. **Create option objects** - Each code construct has a corresponding option class
2. **Configure properties** - Use `With*` methods or set properties directly
3. **Add children** - Nest options within options to build complex structures
4. **Build the code** - Call `.Build()` to generate the final code string

### Extension Methods

The library provides fluent extension methods for common operations:

- **`WithName()`**, **`WithKeyword()`**, **`WithType()`** - Configure basic properties
- **`Public`**, **`Private`**, **`Internal`**, **`Static`** - Keyword configurator properties for fluent keyword specification
- **`KeywordConfigurator`** - Access the keyword configurator for advanced chaining with `Parent` property
- **`Using()`** - Add using statements
- **`Namespace()`**, **`Class()`**, **`Method()`** - Add structural elements
- **`AutoProperty()`**, **`Constructor()`** - Add members
- **`For()`**, **`While()`**, **`If()`**, **`Switch()`** - Add control flow
- **`AppendLine()`** - Add raw code lines

### Building Code

There are two main ways to build code:

1. **Direct building from options**:
   ```csharp
   var option = new TypeOption().WithName("MyClass");
   var code = option.Build();
   ```

2. **Using the Code.Create() entry point**:
   ```csharp
   var code = Code.Create()
       .Using("System")
       .Namespace(ns => { ... })
       .Build();
   ```

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🔗 Links

- **NuGet Package**: [Fengb3.EasyCodeBuilder](https://www.nuget.org/packages/Fengb3.EasyCodeBuilder/)
- **GitHub Repository**: [https://github.com/fengb3/EasyCodeBuilder](https://github.com/fengb3/EasyCodeBuilder)
- **Issues**: [Report a bug or request a feature](https://github.com/fengb3/EasyCodeBuilder/issues)

---

**Made with ❤️ by Bohan Feng**
