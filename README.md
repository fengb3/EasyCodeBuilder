# Easy Code Builder

[![NuGet Version](https://img.shields.io/nuget/v/Fengb3.EasyCodeBuilder)](https://www.nuget.org/packages/Fengb3.EasyCodeBuilder/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
![CI](https://github.com/fengb3/EasyCodeBuilder/workflows/🔄%20Continuous%20Integration/badge.svg)
![NuGet](https://github.com/fengb3/EasyCodeBuilder/workflows/🚀%20NuGet%20Package%20CI/CD/badge.svg)

A powerful and flexible library for dynamic code generation, supporting multiple programming languages including C#, Python, and Lisp.

## ✨ Features

- **🚀 Fluent API**: Easy-to-use method chaining for intuitive code building
- **🎯 Multi-Language Support**: Generate C#, Python, and Lisp code
- **🔧 Configurable Indentation**: Support for spaces, tabs, and custom indentation
- **📚 Rich Code Constructs**: Classes, methods, properties, control structures, and more
- **⚡ High Performance**: Optimized string building with caching mechanisms
- **🛡️ Type Safety**: Generic-based design ensures compile-time safety
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

var builder = new CSharpCodeBuilder();

var code = builder
    .Using("System", "System.Collections.Generic")
    .Namespace("MyProject", ns => ns
        .Class("Person", cls => cls
            .Property("string", "Name", accessors: "{ get; set; }")
            .Property("int", "Age", accessors: "{ get; set; }")
            .Method("GetInfo", method => method
                .AppendLine("return $\"Name: {Name}, Age: {Age}\";")
            , returnType: "string")
        )
    )
    .ToString();
```

</div>
<div style="flex: 1;">

**Generated Code**:
```csharp
using System;
using System.Collections.Generic;

namespace MyProject
{
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        
        public string GetInfo()
        {
            return $"Name: {Name}, Age: {Age}";
        }
    }
}
```

</div>
</div>

## 📚 Documentation

For detailed documentation and examples, please refer to:

- **[C# Code Builder Guide](Docs/README-cs.md)** - Complete guide for C# code generation
- **[Python Code Builder Guide](Docs/README-py.md)** - Complete guide for Python code generation

## 🏗️ Supported Languages

| Language | Status | Builder Class |
|----------|--------|---------------|
| **C#** | ✅ Full Support | `CSharpCodeBuilder` |
| **Python** | ✅ Full Support | `PythonCodeBuilder` |
| **Lisp** | ✅ Basic Support | `LispCodeBuilder` |

## 🎯 Key Components

### Core Classes
- **`CodeBuilder<T>`** - Abstract base class with core functionality
- **`CSharpCodeBuilder`** - C# specific code generation
- **`PythonCodeBuilder`** - Python specific code generation  
- **`LispCodeBuilder`** - Lisp specific code generation

### Features
- **Fluent API** - Method chaining for readable code
- **Indentation Management** - Automatic and configurable indentation
- **Code Blocks** - Structured code generation with proper nesting
- **Performance Optimized** - Efficient string building and caching

## 📊 Examples

### Generate a Complete C# Class

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API Calling**:
```csharp
var code = new CSharpCodeBuilder()
    .Using("System")
    .Namespace("MyApp.Models", ns => ns
        .Class("User", cls => cls
            .Field("string", "_name", "private")
            .Property("string", "Name", 
                modifiers: "public",
                accessors: "{ get => _name; set => _name = value ?? throw new ArgumentNullException(); }")
            .Method("ToString", method => method
                .AppendLine("return $\"User: {Name}\";")
            , returnType: "string", modifiers: "public override")
        )
    );
```

</div>
<div style="flex: 1;">

**Generated Code**:
```csharp
using System;

namespace MyApp.Models
{
    public class User
    {
        private string _name;
        
        public string Name 
        { 
            get => _name; 
            set => _name = value ?? throw new ArgumentNullException(); 
        }
        
        public override string ToString()
        {
            return $"User: {Name}";
        }
    }
}
```

</div>
</div>

### Generate Python Code

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API Calling**:
```csharp
var pythonCode = new PythonCodeBuilder()
    .Import("json", "datetime")
    .AppendLine()
    .Class("User", cls => cls
        .Function("__init__", init => init
            .AppendLine("self.name = name")
            .AppendLine("self.created_at = datetime.datetime.now()")
        , "self, name: str")
        .AppendLine()
        .Function("to_dict", func => func
            .AppendLine("return {")
            .AppendLine("    'name': self.name,")
            .AppendLine("    'created_at': self.created_at.isoformat()")
            .AppendLine("}")
        , "self", "dict")
    );
```

</div>
<div style="flex: 1;">

**Generated Code**:
```python
import json
import datetime

class User:
    def __init__(self, name: str):
        self.name = name
        self.created_at = datetime.datetime.now()
    
    def to_dict(self) -> dict:
        return {
            'name': self.name,
            'created_at': self.created_at.isoformat()
        }
```

</div>
</div>

### Control Structures Example

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API Calling**:
```csharp
var code = new CSharpCodeBuilder()
    .Namespace("Examples", ns => ns
        .Class("Calculator", cls => cls
            .Method("Divide", method => method
                .If("b == 0", ifBlock => ifBlock
                    .AppendLine("throw new DivideByZeroException(\"Cannot divide by zero\");")
                )
                .AppendLine("return a / b;")
            , returnType: "double", parameters: "double a, double b")
        )
    );
```

</div>
<div style="flex: 1;">

**Generated Code**:
```csharp
namespace Examples
{
    public class Calculator
    {
        public double Divide(double a, double b)
        {
            if (b == 0)
            {
                throw new DivideByZeroException("Cannot divide by zero");
            }
            return a / b;
        }
    }
}
```

</div>
</div>

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
