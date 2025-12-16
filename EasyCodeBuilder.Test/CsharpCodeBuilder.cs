using System;
using System.Collections.Generic;
using System.Linq;
using Fengb3.EasyCodeBuilder;
using Xunit;
using Xunit.Abstractions;

namespace EasyCodeBuilder.Test
{

    public class CSharpCodeBuilderTests(ITestOutputHelper testOutputHelper)
    {
        [Fact]
        public void Test()
        {
            var cb = new CodeBuilder();

            // cb.Namespace(opt => {
            //     opt.Name = "Hello";
            // });

            testOutputHelper.WriteLine(cb.ToString());
        }
    }
}
//     #region Constructor Tests
//
//     [Fact]
//     public void Constructor_Default_CreatesInstance()
//     {
//         // Arrange & Act
//         var builder = new CSharpCodeBuilder();
//
//         // Assert
//         Assert.NotNull(builder);
//         Assert.Equal(string.Empty, builder.ToString());
//     }
//
//     [Fact]
//     public void Constructor_WithParameters_CreatesInstanceWithSettings()
//     {
//         // Arrange & Act
//         var builder = new CSharpCodeBuilder("  ", 2, 2048);
//
//         // Assert
//         Assert.NotNull(builder);
//         Assert.Equal(string.Empty, builder.ToString());
//     }
//
//     [Fact]
//     public void WithTabs_Default_CreatesInstanceWithTabIndentation()
//     {
//         // Arrange & Act
//         var builder = CSharpCodeBuilder.WithTabs();
//
//         // Assert
//         Assert.NotNull(builder);
//         // Test by adding a class (which uses CodeBlock internally)
//         builder.Class("TestClass", cb => cb.AppendLine("// test"));
//         var result = builder.ToString();
//         Assert.Contains("\t// test", result);
//     }
//
//     [Fact]
//     public void WithTabs_CustomCount_CreatesInstanceWithMultipleTabIndentation()
//     {
//         // Arrange & Act
//         var builder = CSharpCodeBuilder.WithTabs(2);
//
//         // Assert
//         Assert.NotNull(builder);
//         builder.Class("TestClass", cb => cb.AppendLine("// test"));
//         var result = builder.ToString();
//         Assert.Contains("\t\t// test", result);
//     }
//
//     [Fact]
//     public void WithSpaces_CreatesInstanceWithSpaceIndentation()
//     {
//         // Arrange & Act
//         var builder = CSharpCodeBuilder.WithSpaces(4);
//
//         // Assert
//         Assert.NotNull(builder);
//         builder.Class("TestClass", cb => cb.AppendLine("// test"));
//         var result = builder.ToString();
//         Assert.Contains("    // test", result);
//     }
//
//     #endregion
//
//     #region Basic Code Methods Tests
//
//     [Fact]
//     public void AppendWhen_ConditionTrue_AppendsLine()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.AppendWhen(true, "test line");
//
//         // Assert
//         Assert.Contains("test line", builder.ToString());
//     }
//
//     [Fact]
//     public void AppendWhen_ConditionFalse_DoesNotAppendLine()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.AppendWhen(false, "test line");
//
//         // Assert
//         Assert.Equal(string.Empty, builder.ToString());
//     }
//
//     [Fact]
//     public void AppendBatch_WithAction_ProcessesAllItems()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//         var items = new[] { "item1", "item2", "item3" };
//
//         // Act
//         builder.AppendBatch(items, (cb, item) => cb.AppendLine(item));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("item1", result);
//         Assert.Contains("item2", result);
//         Assert.Contains("item3", result);
//     }
//
//     [Fact]
//     public void AppendBatch_WithFunction_ProcessesAllItems()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//         var items = new[] { "item1", "item2", "item3" };
//
//         // Act
//         builder.AppendBatch(items, (cb, item) => cb.AppendLine(item));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("item1", result);
//         Assert.Contains("item2", result);
//         Assert.Contains("item3", result);
//     }
//
//     [Fact]
//     public void AppendBatch_WithActionAndIndex_ProcessesAllItemsWithIndex()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//         var items = new[] { "item1", "item2" };
//
//         // Act
//         builder.AppendBatch(items, (cb, item, index) => cb.AppendLine($"{index}: {item}"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("0: item1", result);
//         Assert.Contains("1: item2", result);
//     }
//
//     [Fact]
//     public void AppendBatch_WithFunctionAndIndex_ProcessesAllItemsWithIndex()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//         var items = new[] { "item1", "item2" };
//
//         // Act
//         builder.AppendBatch(items, (cb, item, index) => cb.AppendLine($"{index}: {item}"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("0: item1", result);
//         Assert.Contains("1: item2", result);
//     }
//
//     #endregion
//
//     #region Using and Namespace Tests
//
//     [Fact]
//     public void Using_SingleNamespace_AddsUsingStatement()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Using("System");
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("using System;", result);
//     }
//
//     [Fact]
//     public void Using_MultipleNamespaces_AddsAllUsingStatements()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Using("System", "System.Collections.Generic", "System.Linq");
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("using System;", result);
//         Assert.Contains("using System.Collections.Generic;", result);
//         Assert.Contains("using System.Linq;", result);
//     }
//
//     [Fact]
//     public void Namespace_WithAction_CreatesNamespaceBlock()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Namespace("MyNamespace", cb => cb.AppendLine("// content"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("namespace MyNamespace", result);
//         Assert.Contains("// content", result);
//         Assert.Contains("{", result);
//         Assert.Contains("}", result);
//     }
//
//     [Fact]
//     public void Namespace_WithFunction_CreatesNamespaceBlock()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Namespace("MyNamespace", cb => cb.AppendLine("// content"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("namespace MyNamespace", result);
//         Assert.Contains("// content", result);
//     }
//
//     [Fact]
//     public void Namespace_CurriedVersion_CreatesNamespaceBlock()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Namespace("MyNamespace")(cb => cb.AppendLine("// content"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("namespace MyNamespace", result);
//         Assert.Contains("// content", result);
//     }
//
//     #endregion
//
//     #region Type Definition Tests
//
//     [Fact]
//     public void Type_WithAction_CreatesTypeDefinition()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Type("class", "MyClass", cb => cb.AppendLine("// content"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("public class MyClass", result);
//         Assert.Contains("// content", result);
//     }
//
//     [Fact]
//     public void Type_WithBaseTypes_CreatesTypeDefinitionWithInheritance()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Type("class", "MyClass", cb => cb.AppendLine("// content"), "public", "BaseClass");
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("public class MyClass : BaseClass", result);
//     }
//
//     [Fact]
//     public void Class_WithAction_CreatesClassDefinition()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Class("MyClass", cb => cb.AppendLine("// content"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("public class MyClass", result);
//         Assert.Contains("// content", result);
//     }
//
//     [Fact]
//     public void Class_WithBaseClass_CreatesClassDefinitionWithInheritance()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Class("MyClass", cb => cb.AppendLine("// content"), "public", "BaseClass");
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("public class MyClass : BaseClass", result);
//     }
//
//     [Fact]
//     public void Class_CurriedVersion_CreatesClassDefinition()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Class("MyClass")(cb => cb.AppendLine("// content"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("public class MyClass", result);
//     }
//
//     [Fact]
//     public void Interface_WithAction_CreatesInterfaceDefinition()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Interface("IMyInterface", cb => cb.AppendLine("// content"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("public interface IMyInterface", result);
//         Assert.Contains("// content", result);
//     }
//
//     [Fact]
//     public void Interface_CurriedVersion_CreatesInterfaceDefinition()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Interface("IMyInterface", "public")(cb => cb.AppendLine("// content"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("public interface IMyInterface", result);
//     }
//
//     [Fact]
//     public void Enum_WithAction_CreatesEnumDefinition()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Enum("MyEnum", cb => cb.AppendLine("Value1,").AppendLine("Value2"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("public enum MyEnum", result);
//         Assert.Contains("Value1,", result);
//         Assert.Contains("Value2", result);
//     }
//
//     [Fact]
//     public void Enum_CurriedVersion_CreatesEnumDefinition()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Enum("MyEnum")(cb => cb.AppendLine("Value1"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("public enum MyEnum", result);
//         Assert.Contains("Value1", result);
//     }
//
//     [Fact]
//     public void Struct_WithAction_CreatesStructDefinition()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Struct("MyStruct", cb => cb.AppendLine("// content"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("public struct MyStruct", result);
//         Assert.Contains("// content", result);
//     }
//
//     [Fact]
//     public void Struct_CurriedVersion_CreatesStructDefinition()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Struct("MyStruct")(cb => cb.AppendLine("// content"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("public struct MyStruct", result);
//     }
//
//     #endregion
//
//     #region Member Definition Tests
//
//     [Fact]
//     public void Method_WithAction_CreatesMethodDefinition()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Method("MyMethod", cb => cb.AppendLine("// method body"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("public void MyMethod()", result);
//         Assert.Contains("// method body", result);
//     }
//
//     [Fact]
//     public void Method_WithParameters_CreatesMethodDefinitionWithParameters()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Method("MyMethod", cb => cb.AppendLine("// method body"), "string", "public", "string param1, int param2");
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("public string MyMethod(string param1, int param2)", result);
//         Assert.Contains("// method body", result);
//     }
//
//     [Fact]
//     public void Method_CurriedVersion_CreatesMethodDefinition()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Method("MyMethod")(cb => cb.AppendLine("// method body"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("public void MyMethod()", result);
//     }
//
//     [Fact]
//     public void Constructor_WithAction_CreatesConstructorDefinition()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Constructor("MyClass", cb => cb.AppendLine("// constructor body"), "string param");
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("public MyClass(string param)", result);
//         Assert.Contains("// constructor body", result);
//     }
//
//     [Fact]
//     public void Constructor_WithBaseClass_CreatesConstructorWithBaseCall()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Constructor("MyClass", cb => cb.AppendLine("// constructor body"), "string param", "public", "param");
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("public MyClass(string param) : base(param)", result);
//     }
//
//     [Fact]
//     public void Constructor_CurriedVersion_CreatesConstructorDefinition()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Constructor("MyClass", "string param")(cb => cb.AppendLine("// constructor body"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("public MyClass(string param)", result);
//     }
//
//     [Fact]
//     public void Deconstructor_WithAction_CreatesDeconstructorDefinition()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Deconstructor(cb => cb.AppendLine("// deconstructor body"), "out string param1, out int param2");
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("public void Deconstruct(out string param1, out int param2)", result);
//         Assert.Contains("// deconstructor body", result);
//     }
//
//     [Fact]
//     public void Deconstructor_CurriedVersion_CreatesDeconstructorDefinition()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Deconstructor("out string param1")(cb => cb.AppendLine("// deconstructor body"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("public void Deconstruct(out string param1)", result);
//     }
//
//     [Fact]
//     public void Destructor_WithAction_CreatesDestructorDefinition()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Destructor("MyClass", cb => cb.AppendLine("// destructor body"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("~MyClass()", result);
//         Assert.Contains("// destructor body", result);
//     }
//
//     [Fact]
//     public void Destructor_CurriedVersion_CreatesDestructorDefinition()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Destructor("MyClass")(cb => cb.AppendLine("// destructor body"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("~MyClass()", result);
//     }
//
//     [Fact]
//     public void Property_Simple_CreatesPropertyDefinition()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Property("string", "MyProperty");
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("public string MyProperty { get; set; }", result);
//     }
//
//     [Fact]
//     public void Property_WithInitialValue_CreatesPropertyDefinitionWithInitialValue()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Property("string", "MyProperty", "public", "\"default\"");
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("public string MyProperty { get; set; } = \"default\";", result);
//     }
//
//     [Fact]
//     public void Property_WithCustomAccessors_CreatesPropertyDefinitionWithCustomAccessors()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Property("string", "MyProperty", "public", null, "{ get; private set; }");
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("public string MyProperty { get; private set; }", result);
//     }
//
//     [Fact]
//     public void Property_WithFunction_CreatesPropertyDefinitionWithCustomBlock()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Property("string", "MyProperty", cb => cb.AppendLine("get => _field;").AppendLine("set => _field = value;"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("public string MyProperty { get; set; }", result);
//         Assert.Contains("get => _field;", result);
//         Assert.Contains("set => _field = value;", result);
//     }
//
//     [Fact]
//     public void PropertyBuilder_CurriedVersion_CreatesPropertyDefinition()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.PropertyBuilder("string", "MyProperty")(cb => cb.AppendLine("// custom logic"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("public string MyProperty { get; set; }", result);
//     }
//
//     [Fact]
//     public void Field_Simple_CreatesFieldDefinition()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Field("string", "_myField");
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("private string _myField;", result);
//     }
//
//     [Fact]
//     public void Field_WithInitialValue_CreatesFieldDefinitionWithInitialValue()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Field("string", "_myField", "private", "\"default\"");
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("private string _myField = \"default\";", result);
//     }
//
//     [Fact]
//     public void Field_WithModifiers_CreatesFieldDefinitionWithModifiers()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Field("string", "MyField", "public readonly");
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("public readonly string MyField;", result);
//     }
//
//     #endregion
//
//     #region Control Structure Tests
//
//     [Fact]
//     public void If_WithAction_CreatesIfStatement()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.If("condition", cb => cb.AppendLine("DoSomething();"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("if (condition)", result);
//         Assert.Contains("DoSomething();", result);
//     }
//
//     [Fact]
//     public void If_CurriedVersion_CreatesIfStatement()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         const string variableName = "a";
//         const int variableValue = 20;
//
//         // Act
//         builder.If($"{variableName} == \"{variableValue}\"")(cb => cb.AppendLine("DoSomething();"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains($"{variableName} == \"{variableValue}\"", result);
//         Assert.Contains("DoSomething();", result);
//     }
//
//     [Fact]
//     public void ElseIf_WithAction_CreatesElseIfStatement()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.ElseIf("condition", cb => cb.AppendLine("DoSomething();"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("else if (condition)", result);
//         Assert.Contains("DoSomething();", result);
//     }
//
//     [Fact]
//     public void ElseIf_CurriedVersion_CreatesElseIfStatement()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.ElseIf("condition")(cb => cb.AppendLine("DoSomething();"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("else if (condition)", result);
//     }
//
//     [Fact]
//     public void Else_WithAction_CreatesElseStatement()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Else(cb => cb.AppendLine("DoSomething();"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("else", result);
//         Assert.Contains("DoSomething();", result);
//     }
//
//     [Fact]
//     public void Else_CurriedVersion_CreatesElseStatement()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Else()(cb => cb.AppendLine("DoSomething();"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("else", result);
//     }
//
//     [Fact]
//     public void For_WithAction_CreatesForLoop()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.For("int i = 0", "i < 10", "i++", cb => cb.AppendLine("DoSomething();"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("for (int i = 0; i < 10; i++)", result);
//         Assert.Contains("DoSomething();", result);
//     }
//
//     [Fact]
//     public void For_CurriedVersion_CreatesForLoop()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.For("int i = 0", "i < 10", "i++")(cb => cb.AppendLine("DoSomething();"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("for (int i = 0; i < 10; i++)", result);
//     }
//
//     [Fact]
//     public void ForEach_WithAction_CreatesForEachLoop()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.ForEach("var", "item", "items", cb => cb.AppendLine("DoSomething(item);"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("foreach (var item in items)", result);
//         Assert.Contains("DoSomething(item);", result);
//     }
//
//     [Fact]
//     public void ForEach_CurriedVersion_CreatesForEachLoop()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.ForEach("var", "item", "items")(cb => cb.AppendLine("DoSomething(item);"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("foreach (var item in items)", result);
//     }
//
//     [Fact]
//     public void While_WithAction_CreatesWhileLoop()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.While("condition", cb => cb.AppendLine("DoSomething();"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("while (condition)", result);
//         Assert.Contains("DoSomething();", result);
//     }
//
//     [Fact]
//     public void While_CurriedVersion_CreatesWhileLoop()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.While("condition")(cb => cb.AppendLine("DoSomething();"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("while (condition)", result);
//     }
//
//     [Fact]
//     public void Try_WithAction_CreatesTryBlock()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Try(cb => cb.AppendLine("DoSomething();"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("try", result);
//         Assert.Contains("DoSomething();", result);
//     }
//
//     [Fact]
//     public void Try_CurriedVersion_CreatesTryBlock()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Try()(cb => cb.AppendLine("DoSomething();"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("try", result);
//     }
//
//     [Fact]
//     public void Catch_WithAction_CreatesCatchBlock()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Catch(cb => cb.AppendLine("HandleException();"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("catch (Exception ex)", result);
//         Assert.Contains("HandleException();", result);
//     }
//
//     [Fact]
//     public void Catch_WithCustomException_CreatesCatchBlockWithCustomException()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Catch(cb => cb.AppendLine("HandleException();"), "ArgumentException", "argEx");
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("catch (ArgumentException argEx)", result);
//         Assert.Contains("HandleException();", result);
//     }
//
//     [Fact]
//     public void Catch_CurriedVersion_CreatesCatchBlock()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Catch()(cb => cb.AppendLine("HandleException();"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("catch (Exception ex)", result);
//     }
//
//     [Fact]
//     public void CatchAll_WithAction_CreatesGenericCatchBlock()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.CatchAll(cb => cb.AppendLine("HandleException();"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("catch", result);
//         Assert.DoesNotContain("Exception", result.Split('\n').First(l => l.Contains("catch")));
//         Assert.Contains("HandleException();", result);
//     }
//
//     [Fact]
//     public void CatchAll_CurriedVersion_CreatesGenericCatchBlock()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.CatchAll()(cb => cb.AppendLine("HandleException();"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("catch", result);
//     }
//
//     [Fact]
//     public void Finally_WithAction_CreatesFinallyBlock()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Finally(cb => cb.AppendLine("Cleanup();"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("finally", result);
//         Assert.Contains("Cleanup();", result);
//     }
//
//     [Fact]
//     public void Finally_CurriedVersion_CreatesFinallyBlock()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder.Finally()(cb => cb.AppendLine("Cleanup();"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("finally", result);
//     }
//
//     #endregion
//
//     #region Integration Tests
//
//     [Fact]
//     public void CompleteClass_WithAllFeatures_GeneratesCorrectCode()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder
//             .Using("System", "System.Collections.Generic")
//             .Namespace("MyNamespace", ns => ns
//                 .Class("MyClass", cls => cls
//                     .Field("string", "_name", "private")
//                     .Property("string", "Name", "public", null, "{ get; private set; }")
//                     .Constructor("MyClass", ctor => ctor
//                         .AppendLine("_name = string.Empty;"), parameters: "string name")
//                     .Method("DoWork", method => method
//                         .If("_name != null", ifBlock => ifBlock
//                             .AppendLine("Console.WriteLine(_name);"))
//                         .Else(elseBlock => elseBlock
//                             .AppendLine("Console.WriteLine(\"No name\");")), "void", "public")));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("using System;", result);
//         Assert.Contains("using System.Collections.Generic;", result);
//         Assert.Contains("namespace MyNamespace", result);
//         Assert.Contains("public class MyClass", result);
//         Assert.Contains("private string _name;", result);
//         Assert.Contains("public string Name { get; private set; }", result);
//         Assert.Contains("public MyClass(string name)", result);
//         Assert.Contains("public void DoWork()", result);
//         Assert.Contains("if (_name != null)", result);
//         Assert.Contains("else", result);
//     }
//
//     [Fact]
//     public void TryCatchFinally_CompleteBlock_GeneratesCorrectCode()
//     {
//         // Arrange
//         var builder = new CSharpCodeBuilder();
//
//         // Act
//         builder
//             .Try(tryBlock => tryBlock
//                 .AppendLine("DoRiskyOperation();"))
//             .Catch(catchBlock => catchBlock
//                 .AppendLine("LogError(ex);"), "InvalidOperationException", "ex")
//             .CatchAll(catchAllBlock => catchAllBlock
//                 .AppendLine("LogGenericError();"))
//             .Finally(finallyBlock => finallyBlock
//                 .AppendLine("Cleanup();"));
//
//         // Assert
//         var result = builder.ToString();
//         Assert.Contains("try", result);
//         Assert.Contains("DoRiskyOperation();", result);
//         Assert.Contains("catch (InvalidOperationException ex)", result);
//         Assert.Contains("LogError(ex);", result);
//         Assert.Contains("catch", result);
//         Assert.Contains("LogGenericError();", result);
//         Assert.Contains("finally", result);
//         Assert.Contains("Cleanup();", result);
//          }
//  
//      #endregion
//  }
// }