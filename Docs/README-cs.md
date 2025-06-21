# CSharpCodeBuilder 使用指南

`CSharpCodeBuilder` 是一个强大的 C# 代码生成器，继承自 `CodeBuilder`，专门用于生成符合 C# 语法规范的代码。它提供了丰富的 API 来构建各种 C# 代码结构。

## 特性

- 🔷 **C# 语法支持**: 自动处理 C# 的大括号语法和缩进
- 📦 **命名空间**: 支持 `using` 语句和 `namespace` 定义
- 🏗️ **类型定义**: 支持类、接口、枚举定义
- 🔧 **成员定义**: 支持方法、属性、字段定义
- 🔀 **控制结构**: 支持 if/else、for、foreach、while、try/catch 语句
- 🚀 **代码模板**: 内置代码模板（当前重构中）
- 🎨 **链式调用**: 支持流畅的链式调用语法
- ⚙️ **可配置**: 支持自定义缩进字符和缩进数量
- 🚄 **性能优化**: 使用 `string.Create` 和 `StringBuilder` 进行高效字符串拼接
- 🔄 **函数式编程**: 支持 curried 版本的方法，提供更灵活的代码构建方式

## 目录

- [构造函数](#构造函数)
- [Using 和 Namespace](#using-和-namespace)
- [类型定义](#类型定义)
- [成员定义](#成员定义)
- [控制结构](#控制结构)
- [基础扩展方法](#基础扩展方法)
- [代码模板（重构中）](#代码模板)
- [完整示例](#完整示例)
- [函数式编程支持](#函数式编程支持)
- [最佳实践](#最佳实践)

## 构造函数

### 默认构造函数

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API 调用**:
```csharp
var builder = new CSharpCodeBuilder();
```

**生成配置**: 使用 4 个空格缩进，大括号作为块开始和结束符号


### 自定义缩进

**API 调用**:
```csharp
// 使用 Tab 缩进
var builder = CSharpCodeBuilder.WithTabs(1);

// 使用 2 个空格缩进
var builder = CSharpCodeBuilder.WithSpaces(2);

// 完全自定义
var builder = new CSharpCodeBuilder("\t", 1);
```

**说明**: 支持多种缩进方式自定义，满足不同编码风格需求


## Using 和 Namespace

### Using - 导入命名空间

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API 调用**:
```csharp
var builder = new CSharpCodeBuilder();
builder.Using("System", "System.Collections.Generic", "System.Linq");
```

</div>
<div style="flex: 1;">

**生成的代码**:
```csharp
using System;
using System.Collections.Generic;
using System.Linq;

```

</div>
</div>

### Namespace - 命名空间定义

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API 调用**:
```csharp
var builder = new CSharpCodeBuilder();
builder.Namespace("MyApp.Services", ns =>
{
    ns.AppendLine("// 命名空间内容");
});
```

</div>
<div style="flex: 1;">

**生成的代码**:
```csharp
namespace MyApp.Services
{
    // 命名空间内容
}
```

</div>
</div>

## 类型定义

### Class - 基础类

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API 调用**:
```csharp
var builder = new CSharpCodeBuilder();
builder.Class("MyClass", cls =>
{
    cls.AppendLine("// 类内容");
});
```

</div>
<div style="flex: 1;">

**生成的代码**:
```csharp
public class MyClass
{
    // 类内容
}
```

</div>
</div>

### Class - 带继承和修饰符

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API 调用**:
```csharp
var builder = new CSharpCodeBuilder();
builder.Class("Child", cls =>
{
    cls.AppendLine("// 子类内容");
}, "public sealed", "Parent");
```

</div>
<div style="flex: 1;">

**生成的代码**:
```csharp
public sealed class Child : Parent
{
    // 子类内容
}
```

</div>
</div>

### Interface - 接口定义

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API 调用**:
```csharp
var builder = new CSharpCodeBuilder();
builder.Interface("IService", iface =>
{
    iface.AppendLine("void DoSomething();");
});
```

</div>
<div style="flex: 1;">

**生成的代码**:
```csharp
public interface IService
{
    void DoSomething();
}
```

</div>
</div>

### Enum - 枚举定义

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API 调用**:
```csharp
var builder = new CSharpCodeBuilder();
builder.Enum("Status", enm =>
{
    enm.AppendLine("Active,");
    enm.AppendLine("Inactive,");
    enm.AppendLine("Pending");
});
```

</div>
<div style="flex: 1;">

**生成的代码**:
```csharp
public enum Status
{
    Active,
    Inactive,
    Pending
}
```

</div>
</div>

## 成员定义

### Method - 方法定义

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API 调用**:
```csharp
var builder = new CSharpCodeBuilder();
builder.Method("Calculate", method =>
{
    method.AppendLine("var result = a + b;");
    method.AppendLine("return result;");
}, "int", "public", "int a, int b");
```

</div>
<div style="flex: 1;">

**生成的代码**:
```csharp
public int Calculate(int a, int b)
{
    var result = a + b;
    return result;
}
```

</div>
</div>

### Property - 属性定义

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API 调用**:
```csharp
var builder = new CSharpCodeBuilder();
builder.Property("string", "Name")
       .Property("int", "Age", "public", "18")
       .Property("bool", "IsActive", "public", null, "{ get; private set; }");
```

</div>
<div style="flex: 1;">

**生成的代码**:
```csharp
public string Name { get; set; };
public int Age { get; set; } = 18;
public bool IsActive { get; private set; };
```

</div>
</div>

### Field - 字段定义

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API 调用**:
```csharp
var builder = new CSharpCodeBuilder();
builder.Field("string", "_name", "private")
       .Field("int", "MaxSize", "public const", "100");
```

</div>
<div style="flex: 1;">

**生成的代码**:
```csharp
private string _name;
public const int MaxSize = 100;
```

</div>
</div>

## 控制结构

### If/ElseIf/Else - 条件判断

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API 调用**:
```csharp
var builder = new CSharpCodeBuilder();
builder.Method("CheckNumber", method =>
{
    method.If("x > 0", ifBody =>
    {
        ifBody.AppendLine("Console.WriteLine(\"Positive\");");
    });
    method.ElseIf("x < 0", elseIfBody =>
    {
        elseIfBody.AppendLine("Console.WriteLine(\"Negative\");");
    });
    method.Else(elseBody =>
    {
        elseBody.AppendLine("Console.WriteLine(\"Zero\");");
    });
}, "void", "public", "int x");
```

</div>
<div style="flex: 1;">

**生成的代码**:
```csharp
public void CheckNumber(int x)
{
    if (x > 0)
    {
        Console.WriteLine("Positive");
    }
    else if (x < 0)
    {
        Console.WriteLine("Negative");
    }
    else
    {
        Console.WriteLine("Zero");
    }
}
```

</div>
</div>

### For - 循环

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API 调用**:
```csharp
var builder = new CSharpCodeBuilder();
builder.Method("PrintNumbers", method =>
{
    method.For("int i = 0", "i < 5", "i++", forBody =>
    {
        forBody.AppendLine("Console.WriteLine($\"Number: {i}\");");
    });
});
```

</div>
<div style="flex: 1;">

**生成的代码**:
```csharp
public void PrintNumbers()
{
    for (int i = 0; i < 5; i++)
    {
        Console.WriteLine($"Number: {i}");
    }
}
```

</div>
</div>

### ForEach - foreach 循环

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API 调用**:
```csharp
var builder = new CSharpCodeBuilder();
builder.Method("ProcessItems", method =>
{
    method.ForEach("var", "item", "items", foreachBody =>
    {
        foreachBody.AppendLine("Console.WriteLine(item);");
    });
}, "void", "public", "IEnumerable<string> items");
```

</div>
<div style="flex: 1;">

**生成的代码**:
```csharp
public void ProcessItems(IEnumerable<string> items)
{
    foreach (var item in items)
    {
        Console.WriteLine(item);
    }
}
```

</div>
</div>

### While - 循环

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API 调用**:
```csharp
var builder = new CSharpCodeBuilder();
builder.Method("Countdown", method =>
{
    method.AppendLine("int counter = 5;");
    method.While("counter > 0", whileBody =>
    {
        whileBody.AppendLine("Console.WriteLine(counter);");
        whileBody.AppendLine("counter--;");
    });
});
```

</div>
<div style="flex: 1;">

**生成的代码**:
```csharp
public void Countdown()
{
    int counter = 5;
    while (counter > 0)
    {
        Console.WriteLine(counter);
        counter--;
    }
}
```

</div>
</div>

### Try/Catch/Finally - 异常处理

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API 调用**:
```csharp
var builder = new CSharpCodeBuilder();
builder.Method("SafeDivide", method =>
{
    method.Try(tryBody =>
    {
        tryBody.AppendLine("var result = a / b;");
        tryBody.AppendLine("return result;");
    });
    method.Catch(catchBody =>
    {
        catchBody.AppendLine("Console.WriteLine($\"Error: {ex.Message}\");");
        catchBody.AppendLine("return 0;");
    }, "DivideByZeroException", "ex");
    method.Finally(finallyBody =>
    {
        finallyBody.AppendLine("Console.WriteLine(\"Division operation completed\");");
    });
}, "double", "public", "double a, double b");
```

</div>
<div style="flex: 1;">

**生成的代码**:
```csharp
public double SafeDivide(double a, double b)
{
    try
    {
        var result = a / b;
        return result;
    }
    catch (DivideByZeroException ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
        return 0;
    }
    finally
    {
        Console.WriteLine("Division operation completed");
    }
}
```

</div>
</div>

### CatchAll - 通用异常捕获

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API 调用**:
```csharp
var builder = new CSharpCodeBuilder();
builder.Method("SafeOperation", method =>
{
    method.Try(tryBody =>
    {
        tryBody.AppendLine("// 执行可能抛出异常的代码");
        tryBody.AppendLine("return true;");
    });
    method.CatchAll(catchBody =>
    {
        catchBody.AppendLine("Console.WriteLine(\"发生了未知异常\");");
        catchBody.AppendLine("return false;");
    });
}, "bool", "public");
```

</div>
<div style="flex: 1;">

**生成的代码**:
```csharp
public bool SafeOperation()
{
    try
    {
        // 执行可能抛出异常的代码
        return true;
    }
    catch
    {
        Console.WriteLine("发生了未知异常");
        return false;
    }
}
```

</div>
</div>

## 基础扩展方法

### AppendMultiLine - 多行文本添加

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API 调用**:
```csharp
var builder = new CSharpCodeBuilder();
builder.AppendMultiLine(@"// 这是一个多行注释
// 第二行注释
// 第三行注释");
```

</div>
<div style="flex: 1;">

**生成的代码**:
```csharp
// 这是一个多行注释
// 第二行注释
// 第三行注释
```

</div>
</div>

### AppendWhen - 条件添加

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API 调用**:
```csharp
var builder = new CSharpCodeBuilder();
var debug = true;
builder.Method("Main", method =>
{
    method.AppendWhen(debug, "Console.WriteLine(\"Debug mode enabled\");");
    method.AppendLine("Console.WriteLine(\"Program started\");");
});
```

</div>
<div style="flex: 1;">

**生成的代码**:
```csharp
public void Main()
{
    Console.WriteLine("Debug mode enabled");
    Console.WriteLine("Program started");
}
```

</div>
</div>

### AppendBatch - 批量添加

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API 调用**:
```csharp
var builder = new CSharpCodeBuilder();
var properties = new[] { ("Name", "string"), ("Age", "int"), ("Email", "string") };
builder.Class("Person", cls =>
{
    cls.AppendBatch(properties, (b, prop) =>
    {
        b.Property(prop.Item2, prop.Item1);
    });
});
```

</div>
<div style="flex: 1;">

**生成的代码**:
```csharp
public class Person
{
    public string Name { get; set; };
    public int Age { get; set; };
    public string Email { get; set; };
}
```

</div>
</div>

## 代码模板

> **注意**: 代码模板功能正在重构中，当前版本暂时不可用。预计将在未来版本中重新启用并提供更强大的模板生成功能。

<!-- 
代码模板功能已临时禁用，正在进行重构以提供更好的用户体验和更强大的功能。
重构完成后将支持：
- 更灵活的模板配置
- 自定义模板支持
- 更丰富的内置模板
- 更好的错误处理和验证
-->

## 完整示例

### 综合示例 - Web API 服务

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API 调用**:
```csharp
var builder = new CSharpCodeBuilder();

// 添加 using 语句
builder.Using("Microsoft.AspNetCore.Mvc", "System.Collections.Generic", "System.Linq");
builder.AppendLine();

// 定义命名空间
builder.Namespace("MyApp.Controllers", ns =>
{
    // 定义控制器类
    ns.Class("ProductController", cls =>
    {
        // 私有字段
        cls.Field("List<Product>", "_products", "private readonly", "new()");
        cls.AppendLine();
        
        // Get 方法
        cls.Method("Get", method =>
        {
            method.If("_products.Any()", ifBody =>
            {
                ifBody.AppendLine("return Ok(_products);");
            });
            method.Else(elseBody =>
            {
                elseBody.AppendLine("return NotFound();");
            });
        }, "IActionResult", "public");
        
        // Post 方法
        cls.Method("Post", method =>
        {
            method.Try(tryBody =>
            {
                tryBody.AppendLine("_products.Add(product);");
                tryBody.AppendLine("return CreatedAtAction(nameof(Get), product);");
            });
            method.Catch(catchBody =>
            {
                catchBody.AppendLine("return BadRequest();");
            });
        }, "IActionResult", "public", "Product product");
        
    }, "public", "ControllerBase");
});

Console.WriteLine(builder.ToString());
```

</div>
<div style="flex: 1;">

**生成的代码**:
```csharp
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace MyApp.Controllers
{
    public class ProductController : ControllerBase
    {
        private readonly List<Product> _products = new();

        public IActionResult Get()
        {
            if (_products.Any())
            {
                return Ok(_products);
            }
            else
            {
                return NotFound();
            }
        }
        public IActionResult Post(Product product)
        {
            try
            {
                _products.Add(product);
                return CreatedAtAction(nameof(Get), product);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
```

</div>
</div>

## 函数式编程支持

CSharpCodeBuilder 支持函数式编程风格，提供了curried版本的方法，使代码更加简洁和可复用。

### Curried 方法示例

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API 调用**:
```csharp
var builder = new CSharpCodeBuilder();

// 创建可重用的类构建器
var createClass = builder.Class("MyClass");

// 使用函数式方法构建类
createClass(cls => 
    cls.Property("string", "Name")
       .Property("int", "Age")
       .Method("ToString"), ( m => 
           m.AppendLine("return $\"{Name}, {Age}\";"), "string", "public override")
);
```

</div>
<div style="flex: 1;">

**生成的代码**:
```csharp
public class MyClass
{
    public string Name { get; set; };
    public int Age { get; set; };
    public override string ToString()
    {
        return $"{Name}, {Age}";
    }
}
```

</div>
</div>
</div>
</div>

### 支持 Curried 版本的方法

- `Namespace(string name)` - 命名空间定义
- `Class(string name, string modifiers, string baseClass)` - 类定义
- `Interface(string name, string modifiers, string baseInterface)` - 接口定义
- `Enum(string name, string modifiers)` - 枚举定义
- `Struct(string name, string modifiers, string baseInterfaces)` - 结构体定义
- `Method(string name, string returnType, string modifiers, string parameters)` - 方法定义
- `PropertyBuilder(string type, string name, ...)` - 属性构建器
- `If(string condition)` - 条件语句
- `ElseIf(string condition)` - Else If 语句
- `Else()` - Else 语句
- `For(string init, string condition, string increment)` - For 循环
- `ForEach(string type, string variable, string collection)` - ForEach 循环
- `While(string condition)` - While 循环
- `Try()` - Try 语句
- `Catch(string exceptionType, string exceptionVar)` - Catch 语句
- `CatchAll()` - 通用 Catch 语句
- `Finally()` - Finally 语句

## 最佳实践

1. **链式调用**: 利用流畅的 API 设计，可以将多个操作链接在一起
2. **缩进一致性**: CSharpCodeBuilder 自动处理 C# 的缩进和大括号规则
3. **命名规范**: 遵循 C# 命名约定（PascalCase 用于类型和公共成员，camelCase 用于私有字段）
4. **命名空间组织**: 将相关的 using 语句放在文件开头，合理组织命名空间结构
5. **代码结构**: 遵循 C# 的代码组织约定（using、namespace、类型定义、成员定义）
6. **函数式编程**: 利用 curried 版本的方法创建可重用的代码构建器，提高代码复用性

## Roadmap - 未来发展路线图

正在持续改进和扩展 CSharpCodeBuilder 的功能。以下是按版本规划的功能路线图：

### 🎯 核心语法增强 (计划中)

**高优先级特性 - 日常开发必需**
- [ ] **构造函数和析构函数支持**
  - `Constructor(string name, string parameters, string modifiers)`
  - `Destructor(string name)`
- [ ] **特性/属性标注 (Attributes)**
  - `Attribute(string attributeName, params string[] parameters)`
  - 支持常用特性如 `[HttpGet]`、`[Required]` 等
- [ ] **异步方法支持**
  - `AsyncMethod(string name, string returnType = "Task")`
  - 自动处理 async/await 关键字
- [ ] **泛型支持**
  - `GenericClass<T>(string name, string typeConstraints)`
  - `GenericMethod<T>(string name, string typeConstraints)`

**属性访问器增强**
- [ ] **高级属性控制**
  - `Property()` 方法支持自定义 getter/setter 实现
  - 支持只读属性、初始化器属性
- [ ] **自动属性优化**
  - 支持 `{ get; init; }` 语法
  - 支持属性表达式体语法

### 🚀 现代 C# 特性 (规划中)

**现代 C# 语法支持**
- [ ] **记录类型 (Records) - C# 9+**
  - `Record(string name, string parameters)`
  - `RecordStruct(string name, string parameters)`
- [ ] **Switch 表达式 - C# 8+**
  - `SwitchExpression(string expression, params (string pattern, string result)[] cases)`
- [ ] **模式匹配**
  - `PatternMatch(string expression)` 支持各种模式
- [ ] **文件范围命名空间 - C# 10+**
  - `FileScopedNamespace(string name)`
- [ ] **全局 using**
  - `GlobalUsing(params string[] namespaces)`

**表达式和 LINQ 支持**
- [ ] **Lambda 表达式**
  - `Lambda(string parameters, string body)`
  - `AnonymousMethod(string parameters)`
- [ ] **LINQ 查询表达式**
  - `LinqQuery(string expression)`
  - 支持 from、where、select 等关键字

### 🏗️ 高级特性 (远期规划)

**面向对象高级特性**
- [ ] **继承和多态**
  - `AbstractClass(string name)` 抽象类支持
  - `VirtualMethod()` 和 `OverrideMethod()` 虚方法支持
  - `OperatorOverload()` 运算符重载
- [ ] **委托和事件**
  - `Delegate(string name, string returnType, string parameters)`
  - `Event(string delegateType, string name)`
- [ ] **索引器和迭代器**
  - `Indexer(string type, string parameters)`
  - `YieldReturn()` 和 `YieldBreak()` 支持

**资源管理和并发**
- [ ] **资源管理语句**
  - `UsingStatement(string resource)` using 语句
  - `UsingDeclaration(string type, string variable)` using 声明
  - `Lock(string lockObject)` lock 语句
- [ ] **本地函数和嵌套类型**
  - `LocalFunction(string name, string returnType)`
  - `NestedClass(string name)` 嵌套类型

### 🔧 代码模板扩展 (远期规划)

**丰富的代码模板**
- [ ] **微服务模板**
  - `MicroserviceTemplate()` 完整微服务架构
  - `DockerfileTemplate()` Docker 配置生成
- [ ] **测试代码模板**
  - `UnitTestTemplate()` 单元测试类生成
  - `MockTemplate()` Mock 对象生成
- [ ] **ORM 模板**
  - `EntityFrameworkTemplate()` EF Core 实体和上下文
  - `DapperTemplate()` Dapper 查询方法

**代码质量工具**
- [ ] **代码分析器集成**
  - 自动应用代码规范和最佳实践
  - 生成符合 StyleCop 规则的代码
- [ ] **智能代码优化**
  - 自动检测和应用性能优化模式
  - 内存分配优化建议

### 🎨 持续改进

**性能优化**
- [ ] **内存使用优化**
  - 更高效的字符串构建算法
  - 减少内存分配和 GC 压力
- [ ] **生成速度优化**
  - 并行代码生成支持
  - 缓存和增量生成

**用户体验**
- [ ] **更好的错误处理**
  - 详细的错误信息和建议
  - 代码验证和语法检查
- [ ] **文档和示例**
  - 交互式文档和教程
  - 更多实际项目示例


## 注意事项

- CSharpCodeBuilder 使用大括号 `{}` 作为代码块开始和结束符号
- 默认使用 4 个空格缩进，符合 .NET 编码规范
- 所有方法都支持链式调用，返回 CSharpCodeBuilder 实例
- 代码模板方法会自动添加必要的 using 语句和命名空间（当前重构中）
- 生成的代码遵循 C# 语法规范，可直接编译执行
- 内部使用 `string.Create` 和高效的字符串拼接算法，提供优异的性能表现
- 支持函数式编程风格，curried 方法可以创建可重用的代码构建器