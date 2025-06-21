# PythonCodeBuilder 使用指南

`PythonCodeBuilder` 是一个强大的 Python 代码生成器，继承自 `CodeBuilder`，专门用于生成符合 Python 语法规范的代码。它提供了丰富的 API 来构建各种 Python 代码结构。

## 特性

- 🐍 **Python 语法支持**: 自动处理 Python 的冒号语法和缩进
- 📦 **模块导入**: 支持 `import` 和 `from...import` 语句
- 🏗️ **类和函数**: 支持类、方法、静态方法、类方法定义
- 🔀 **控制结构**: 支持 if/elif/else、for、while、try/except、with 语句
- 🚀 **框架模板**: 内置 FastAPI 和 Django 代码模板
- 🎨 **链式调用**: 支持流畅的链式调用语法
- ⚙️ **可配置**: 支持自定义缩进字符和缩进数量

## 目录

- [构造函数](#构造函数)
- [导入模块](#导入模块)
- [函数定义](#函数定义)
- [类定义](#类定义)
- [控制结构](#控制结构)
- [扩展功能](#扩展功能)
- [框架模板](#框架模板)
- [完整示例](#完整示例)

## 构造函数

### 默认构造函数

```csharp
var builder = new PythonCodeBuilder();
```

**生成配置**: 使用 4 个空格缩进，冒号作为块开始符号，无块结束符号

### 自定义缩进

```csharp
// 使用 Tab 缩进
var builder = PythonCodeBuilder.WithTabs(1);

// 使用 2 个空格缩进
var builder = PythonCodeBuilder.WithSpaces(2);

// 完全自定义
var builder = new PythonCodeBuilder("\t", 1);
```

## 导入模块

### Import - 导入模块

<table>
<tr>
<td width="50%">

**API 调用**:
```csharp
var builder = new PythonCodeBuilder();
builder.Import("os", "sys", "json");
```

</td>
<td width="50%">

**生成的代码**:
```python
import os
import sys
import json
```

</td>
</tr>
</table>

### FromImport - 从模块导入

<table>
<tr>
<td width="50%">

**API 调用**:
```csharp
var builder = new PythonCodeBuilder();
builder.FromImport("datetime", "datetime", "timedelta")
       .FromImport("pathlib", "Path");
```

</td>
<td width="50%">

**生成的代码**:
```python
from datetime import datetime, timedelta
from pathlib import Path
```

</td>
</tr>
</table>

## 函数定义

### Function - 基础函数

<table>
<tr>
<td width="50%">

**API 调用**:
```csharp
var builder = new PythonCodeBuilder();
builder.Function("hello_world", func =>
{
    func.AppendLine("print('Hello, World!')");
    func.AppendLine("return 'success'");
});
```

</td>
<td width="50%">

**生成的代码**:
```python
def hello_world():
    print('Hello, World!')
    return 'success'
```

</td>
</tr>
</table>

### Function - 带参数和返回类型

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API 调用**:
```csharp
var builder = new PythonCodeBuilder();
builder.Function("add_numbers", func =>
{
    func.AppendLine("result = a + b");
    func.AppendLine("return result");
}, "a: int, b: int", "int");
```

</div>
<div style="flex: 1;">

**生成的代码**:
```python
def add_numbers(a: int, b: int) -> int:
    result = a + b
    return result
```

</div>
</div>

## 类定义

### Class - 基础类

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API 调用**:
```csharp
var builder = new PythonCodeBuilder();
builder.Class("MyClass", cls =>
{
    cls.AppendLine("pass");
});
```

</div>
<div style="flex: 1;">

**生成的代码**:
```python
class MyClass:
    pass
```

</div>
</div>

### Class - 带继承

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API 调用**:
```csharp
var builder = new PythonCodeBuilder();
builder.Class("Child", cls =>
{
    cls.AppendLine("pass");
}, "Parent");
```

</div>
<div style="flex: 1;">

**生成的代码**:
```python
class Child(Parent):
    pass
```

</div>
</div>

### Method - 实例方法

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API 调用**:
```csharp
var builder = new PythonCodeBuilder();
builder.Class("Calculator", cls =>
{
    cls.Method("__init__", init =>
    {
        init.AppendLine("self.value = 0");
    });
    
    cls.Method("add", method =>
    {
        method.AppendLine("self.value += amount");
        method.AppendLine("return self.value");
    }, "amount: int", "int");
});
```

</div>
<div style="flex: 1;">

**生成的代码**:
```python
class Calculator:
    def __init__(self):
        self.value = 0
    def add(self, amount: int) -> int:
        self.value += amount
        return self.value
```

</div>
</div>

### StaticMethod - 静态方法

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API 调用**:
```csharp
var builder = new PythonCodeBuilder();
builder.Class("MathUtils", cls =>
{
    cls.StaticMethod("multiply", method =>
    {
        method.AppendLine("return a * b");
    }, "a: float, b: float", "float");
});
```

</div>
<div style="flex: 1;">

**生成的代码**:
```python
class MathUtils:
    @staticmethod
    def multiply(a: float, b: float) -> float:
        return a * b
```

</div>
</div>

### ClassMethod - 类方法

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API 调用**:
```csharp
var builder = new PythonCodeBuilder();
builder.Class("Person", cls =>
{
    cls.ClassMethod("create_anonymous", method =>
    {
        method.AppendLine("return cls('Anonymous')");
    }, returnType: "'Person'");
});
```

</div>
<div style="flex: 1;">

**生成的代码**:
```python
class Person:
    @classmethod
    def create_anonymous(cls) -> 'Person':
        return cls('Anonymous')
```

</div>
</div>

## 控制结构

### If/Elif/Else - 条件判断

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API 调用**:
```csharp
var builder = new PythonCodeBuilder();
builder.Function("check_number", func =>
{
    func.If("x > 0", ifBody =>
    {
        ifBody.AppendLine("print('Positive')");
    });
    func.Elif("x < 0", elifBody =>
    {
        elifBody.AppendLine("print('Negative')");
    });
    func.Else(elseBody =>
    {
        elseBody.AppendLine("print('Zero')");
    });
}, "x: int");
```

</div>
<div style="flex: 1;">

**生成的代码**:
```python
def check_number(x: int):
    if x > 0:
        print('Positive')
    elif x < 0:
        print('Negative')
    else:
        print('Zero')
```

</div>
</div>

### For - 循环

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API 调用**:
```csharp
var builder = new PythonCodeBuilder();
builder.Function("print_range", func =>
{
    func.For("i", "range(5)", forBody =>
    {
        forBody.AppendLine("print(f'Number: {i}')");
    });
});
```

</div>
<div style="flex: 1;">

**生成的代码**:
```python
def print_range():
    for i in range(5):
        print(f'Number: {i}')
```

</div>
</div>

### While - 循环

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API 调用**:
```csharp
var builder = new PythonCodeBuilder();
builder.Function("countdown", func =>
{
    func.AppendLine("counter = 5");
    func.While("counter > 0", whileBody =>
    {
        whileBody.AppendLine("print(counter)");
        whileBody.AppendLine("counter -= 1");
    });
});
```

</div>
<div style="flex: 1;">

**生成的代码**:
```python
def countdown():
    counter = 5
    while counter > 0:
        print(counter)
        counter -= 1
```

</div>
</div>

### TryExcept - 异常处理

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API 调用**:
```csharp
var builder = new PythonCodeBuilder();
builder.Function("safe_divide", func =>
{
    func.TryExcept(
        tryfunc: tryBody =>
        {
            tryBody.AppendLine("result = a / b");
            tryBody.AppendLine("return result");
        },
        exceptionType: "ZeroDivisionError",
        exceptionVar: "e",
        exceptfunc: exceptBody =>
        {
            exceptBody.AppendLine("print(f'Error: {e}')");
            exceptBody.AppendLine("return 0");
        }
    );
}, "a: float, b: float", "float");
```

</div>
<div style="flex: 1;">

**生成的代码**:
```python
def safe_divide(a: float, b: float) -> float:
    try:
        result = a / b
        return result
    except ZeroDivisionError as e:
        print(f'Error: {e}')
        return 0
```

</div>
</div>

### With - 上下文管理器

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API 调用**:
```csharp
var builder = new PythonCodeBuilder();
builder.Function("write_file", func =>
{
    func.With("open('output.txt', 'w')", withBody =>
    {
        withBody.AppendLine("file.write('Hello, World!')");
        withBody.AppendLine("file.write('\\n')");
    }, "file");
});
```

</div>
<div style="flex: 1;">

**生成的代码**:
```python
def write_file():
    with open('output.txt', 'w') as file:
        file.write('Hello, World!')
        file.write('\n')
```

</div>
</div>

## 扩展功能

### AppendWhen - 条件添加

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API 调用**:
```csharp
var builder = new PythonCodeBuilder();
var debug = true;
builder.Function("main", func =>
{
    func.AppendWhen(debug, "print('Debug mode enabled')");
    func.AppendLine("print('Program started')");
});
```

</div>
<div style="flex: 1;">

**生成的代码**:
```python
def main():
    print('Debug mode enabled')
    print('Program started')
```

</div>
</div>

### AppendBatch - 批量添加

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API 调用**:
```csharp
var builder = new PythonCodeBuilder();
var modules = new[] { "os", "sys", "json" };
builder.AppendBatch(modules, (b, module) =>
{
    b.Import(module);
    return b;
});
```

</div>
<div style="flex: 1;">

**生成的代码**:
```python
import os
import sys
import json
```

</div>
</div>

### DataClass - 数据类

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API 调用**:
```csharp
var builder = new PythonCodeBuilder();
builder.DataClass("Person", new Dictionary<string, string>
{
    {"name", "str"},
    {"age", "int"},
    {"email", "Optional[str] = None"}
});
```

</div>
<div style="flex: 1;">

**生成的代码**:
```python
from dataclasses import dataclass
@dataclass
class Person:
    name: str
    age: int
    email: Optional[str] = None
```

</div>
</div>

## 完整示例

### 综合示例 - Web API 服务

<div style="display: flex; gap: 20px;">
<div style="flex: 1;">

**API 调用**:
```csharp
var builder = new PythonCodeBuilder();

// 导入依赖
builder.Import("os", "logging")
       .FromImport("fastapi", "FastAPI", "HTTPException")
       .FromImport("pydantic", "BaseModel")
       .AppendLine();

// 数据模型
builder.DataClass("User", new Dictionary<string, string>
{
    {"id", "int"},
    {"name", "str"},
    {"email", "str"}
}).AppendLine();

// 应用初始化
builder.AppendLine("app = FastAPI(title='User API')")
       .AppendLine("users_db = []")
       .AppendLine();

// 路由处理函数
builder.Function("get_user", func =>
{
    func.For("user", "users_db", forBody =>
    {
        forBody.If("user.id == user_id", ifBody =>
        {
            ifBody.AppendLine("return user");
        });
    });
    func.AppendLine("raise HTTPException(status_code=404, detail='User not found')");
}, "user_id: int", "User");

// FastAPI 路由
builder.AppendLine()
       .FastApiRoute("/users/{user_id}", "get", "get_user_endpoint", "User");

Console.WriteLine(builder.ToString());
```

</div>
<div style="flex: 1;">

**生成的代码**:
```python
import os
import logging
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel

from dataclasses import dataclass
@dataclass
class User:
    id: int
    name: str
    email: str

app = FastAPI(title='User API')
users_db = []

def get_user(user_id: int) -> User:
    for user in users_db:
        if user.id == user_id:
            return user
    raise HTTPException(status_code=404, detail='User not found')

@app.get("/users/{user_id}")
def get_user_endpoint() -> User:
    # TODO: Implement route logic
    return {"message": "Success"}
```

</div>
</div>

## 最佳实践

1. **链式调用**: 利用流畅的 API 设计，可以将多个操作链接在一起
2. **缩进一致性**: PythonCodeBuilder 自动处理 Python 的缩进规则
3. **类型注解**: 在函数和方法定义中使用类型注解提高代码质量
4. **模块组织**: 将相关的导入语句放在文件开头
5. **代码结构**: 遵循 Python 的代码组织约定（导入、常量、类、函数、主程序）

## 注意事项

- PythonCodeBuilder 使用冒号 `:` 作为代码块开始符号，无结束符号
- 默认使用 4 个空格缩进，符合 PEP 8 规范
- 所有方法都支持链式调用，返回 PythonCodeBuilder 实例
- 框架模板方法会自动添加必要的导入语句
- 生成的代码遵循 Python 语法规范，可直接执行 