using System;
using System.Collections.Generic;

namespace Fengb3.EasyCodeBuilder;

/// <summary>
/// C# Code Builder - Inherits from CodeBuilder, provides C#-specific code generation methods
/// </summary>
public class CSharpCodeBuilder : CodeBuilder<CSharpCodeBuilder>
{
    #region Constructors & Static Factory Methods

    /// <summary>
    /// Initializes a new instance of CSharpCodeBuilder with default settings
    /// </summary>
    public CSharpCodeBuilder() : base()
    {
        Self = this;
    }

    /// <summary>
    /// Initializes a new instance of CSharpCodeBuilder
    /// </summary>
    /// <param name="indentChar">The character used for indentation</param>
    /// <param name="indentCount">The number of indent characters</param>
    /// <param name="initialCapacity">The initial capacity (default is 1024)</param>
    public CSharpCodeBuilder(string indentChar, int indentCount, int initialCapacity = 1024)
        : base(indentChar, indentCount, "{", "}", initialCapacity)
    {
        Self = this;
    }

    /// <summary>
    /// Creates a C# code builder that uses tab indentation
    /// </summary>
    /// <param name="tabCount">The number of tabs (default is 1)</param>
    /// <param name="initialCapacity">The initial capacity (default is 1024)</param>
    /// <returns>A new CSharpCodeBuilder instance</returns>
    public static CSharpCodeBuilder WithTabs(int tabCount = 1, int initialCapacity = 1024)
    {
        return new CSharpCodeBuilder("\t", tabCount, initialCapacity);
    }

    /// <summary>
    /// Creates a C# code builder that uses space indentation
    /// </summary>
    /// <param name="spaceCount">The number of spaces</param>
    /// <param name="initialCapacity">The initial capacity (default is 1024)</param>
    /// <returns>A new CSharpCodeBuilder instance</returns>
    public static CSharpCodeBuilder WithSpaces(int spaceCount, int initialCapacity = 1024)
    {
        return new CSharpCodeBuilder(" ", spaceCount, initialCapacity);
    }

    #endregion

    #region Basic Extension Methods

    /// <summary>
    /// Conditionally adds a line of code
    /// </summary>
    /// <param name="condition">The condition to check; code line is added only when true</param>
    /// <param name="line">The code line to add</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder AppendWhen(bool condition, string line)
    {
        if (condition)
            AppendLine(line);
        return this;
    }
    
    /// <summary>
    /// Adds code in batch by iterating through a collection
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection</typeparam>
    /// <param name="items">The collection to iterate through</param>
    /// <param name="action">The action to execute for each element, receiving the builder and current element as parameters</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder AppendBatch<T>(IEnumerable<T> items, Action<CSharpCodeBuilder, T> action)
    {
        foreach (var item in items)
            action(this, item);
        return this;
    }

    /// <summary>
    /// Adds code in batch by iterating through a collection using a function
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection</typeparam>
    /// <param name="items">The collection to iterate through</param>
    /// <param name="func">The function to execute for each element, receiving the builder and current element as parameters</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder AppendBatch<T>(IEnumerable<T> items, Func<CSharpCodeBuilder, T, CSharpCodeBuilder> func)
    {
        foreach (var item in items)
            func(this, item);
        return this;
    }

    #endregion

    #region Using and Namespace

    /// <summary>
    /// Adds using statements
    /// </summary>
    /// <param name="namespaces">Array of namespaces to import</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder Using(params string[] namespaces)
    {
        foreach (var ns in namespaces)
            AppendLine($"using {ns};");
        return (CSharpCodeBuilder)AppendLine();
    }

    /// <summary>
    /// Adds a namespace declaration
    /// </summary>
    /// <param name="name">The namespace name</param>
    /// <param name="action">The code building action to execute within the namespace</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder Namespace(string name, Action<CSharpCodeBuilder> action)
    {
        AppendLine($"namespace {name}");
        CodeBlock(action);
        return this;
    }


    /// <summary>
    /// Adds a namespace declaration using a function
    /// </summary>
    /// <param name="name">The namespace name</param>
    /// <param name="func">The code building function to execute within the namespace, receives and returns the builder instance</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder Namespace(string name, Func<CSharpCodeBuilder, CSharpCodeBuilder> func)
    {
        AppendLine($"namespace {name}");
        CodeBlock(func);
        return this;
    }

    /// <summary>
    /// Adds a namespace declaration (curried version)
    /// </summary>
    /// <param name="name">The namespace name</param>
    /// <returns>A function that receives a code building operation and returns the builder instance</returns>
    public Func<Func<CSharpCodeBuilder, CSharpCodeBuilder>, CSharpCodeBuilder> Namespace(string name)
        => func => Namespace(name, func);


    #endregion

    #region String Concatenation Optimization Helper Methods


    /// <summary>
    /// Efficiently builds a type declaration string
    /// </summary>
    /// <param name="modifiers">The access modifiers</param>
    /// <param name="typeKind">The type kind (class, interface, etc.)</param>
    /// <param name="name">The type name</param>
    /// <param name="baseTypes">The base types (optional)</param>
    /// <returns>The constructed type declaration string</returns>
    private static string BuildTypeDeclaration(string modifiers, string typeKind, string name, string? baseTypes = null)
    {
        // 使用 stackalloc 进行高效字符串拼接
        if (string.IsNullOrEmpty(baseTypes))
        {
            // 简单情况：modifiers typeKind name
            int totalLength = modifiers.Length + 1 + typeKind.Length + 1 + name.Length;

            return string.Create(totalLength, (modifiers, typeKind, name), static (span, state) =>
            {
                int pos = 0;
                state.modifiers.AsSpan().CopyTo(span[pos..]);
                pos += state.modifiers.Length;
                span[pos++] = ' ';

                state.typeKind.AsSpan().CopyTo(span[pos..]);
                pos += state.typeKind.Length;
                span[pos++] = ' ';

                state.name.AsSpan().CopyTo(span[pos..]);
            });
        }
        else
        {
            // 复杂情况：modifiers typeKind name : baseTypes
            int totalLength = modifiers.Length + 1 + typeKind.Length + 1 + name.Length + 3 + baseTypes.Length;

            return string.Create(totalLength, (modifiers, typeKind, name, baseTypes), static (span, state) =>
            {
                int pos = 0;
                state.modifiers.AsSpan().CopyTo(span[pos..]);
                pos += state.modifiers.Length;
                span[pos++] = ' ';

                state.typeKind.AsSpan().CopyTo(span[pos..]);
                pos += state.typeKind.Length;
                span[pos++] = ' ';

                state.name.AsSpan().CopyTo(span[pos..]);
                pos += state.name.Length;

                " : ".AsSpan().CopyTo(span[pos..]);
                pos += 3;

                state.baseTypes.AsSpan().CopyTo(span[pos..]);
            });
        }
    }

    /// <summary>
    /// Efficiently builds a property declaration string
    /// </summary>
    /// <param name="modifiers">The access modifiers</param>
    /// <param name="type">The property type</param>
    /// <param name="name">The property name</param>
    /// <param name="accessors">The property accessors</param>
    /// <param name="initialValue">The initial value (optional)</param>
    /// <returns>The constructed property declaration string</returns>
    private static string BuildPropertyDeclaration(string modifiers, string type, string name,
        string accessors, string? initialValue = null)
    {
        if (string.IsNullOrEmpty(initialValue))
        {
            // 简单情况：modifiers type name accessors;
            int totalLength = modifiers.Length + 1 + type.Length + 1 + name.Length + 1 + accessors.Length; //+ 1;

            return string.Create(totalLength, (modifiers, type, name, accessors), static (span, state) =>
            {
                int pos = 0;
                state.modifiers.AsSpan().CopyTo(span[pos..]);
                pos += state.modifiers.Length;
                span[pos++] = ' ';

                state.type.AsSpan().CopyTo(span[pos..]);
                pos += state.type.Length;
                span[pos++] = ' ';

                state.name.AsSpan().CopyTo(span[pos..]);
                pos += state.name.Length;
                span[pos++] = ' ';

                state.accessors.AsSpan().CopyTo(span[pos..]);
                pos += state.accessors.Length;
                // span[pos] = ';';
            });
        }
        else
        {
            // 复杂情况：modifiers type name accessors = initialValue;
            int totalLength = modifiers.Length + 1 + type.Length + 1 + name.Length + 1 + accessors.Length + 3 + initialValue.Length + 1;

            return string.Create(totalLength, (modifiers, type, name, accessors, initialValue), static (span, state) =>
            {
                int pos = 0;
                state.modifiers.AsSpan().CopyTo(span[pos..]);
                pos += state.modifiers.Length;
                span[pos++] = ' ';

                state.type.AsSpan().CopyTo(span[pos..]);
                pos += state.type.Length;
                span[pos++] = ' ';

                state.name.AsSpan().CopyTo(span[pos..]);
                pos += state.name.Length;
                span[pos++] = ' ';

                state.accessors.AsSpan().CopyTo(span[pos..]);
                pos += state.accessors.Length;

                " = ".AsSpan().CopyTo(span[pos..]);
                pos += 3;

                state.initialValue.AsSpan().CopyTo(span[pos..]);
                pos += state.initialValue.Length;
                span[pos] = ';';
            });
        }
    }

    /// <summary>
    /// Efficiently builds a field declaration string
    /// </summary>
    /// <param name="modifiers">The access modifiers</param>
    /// <param name="type">The field type</param>
    /// <param name="name">The field name</param>
    /// <param name="initialValue">The initial value (optional)</param>
    /// <returns>The constructed field declaration string</returns>
    private static string BuildFieldDeclaration(string modifiers, string type, string name, string? initialValue = null)
    {
        if (string.IsNullOrEmpty(initialValue))
        {
            // 简单情况：modifiers type name;
            int totalLength = modifiers.Length + 1 + type.Length + 1 + name.Length + 1;

            return string.Create(totalLength, (modifiers, type, name), static (span, state) =>
            {
                int pos = 0;
                state.modifiers.AsSpan().CopyTo(span[pos..]);
                pos += state.modifiers.Length;
                span[pos++] = ' ';

                state.type.AsSpan().CopyTo(span[pos..]);
                pos += state.type.Length;
                span[pos++] = ' ';

                state.name.AsSpan().CopyTo(span[pos..]);
                pos += state.name.Length;
                span[pos] = ';';
            });
        }
        else
        {
            // 复杂情况：modifiers type name = initialValue;
            int totalLength = modifiers.Length + 1 + type.Length + 1 + name.Length + 3 + initialValue.Length + 1;

            return string.Create(totalLength, (modifiers, type, name, initialValue), static (span, state) =>
            {
                int pos = 0;
                state.modifiers.AsSpan().CopyTo(span[pos..]);
                pos += state.modifiers.Length;
                span[pos++] = ' ';

                state.type.AsSpan().CopyTo(span[pos..]);
                pos += state.type.Length;
                span[pos++] = ' ';

                state.name.AsSpan().CopyTo(span[pos..]);
                pos += state.name.Length;

                " = ".AsSpan().CopyTo(span[pos..]);
                pos += 3;

                state.initialValue.AsSpan().CopyTo(span[pos..]);
                pos += state.initialValue.Length;
                span[pos] = ';';
            });
        }
    }

    /// <summary>
    /// Efficiently builds a method signature string
    /// </summary>
    /// <param name="modifiers">The access modifiers</param>
    /// <param name="returnType">The return type</param>
    /// <param name="name">The method name</param>
    /// <param name="parameters">The parameter list</param>
    /// <returns>The constructed method signature string</returns>
    private static string BuildMethodSignature(string modifiers, string returnType, string name, string parameters)
    {
        // modifiers returnType name(parameters)
        int totalLength = modifiers.Length + 1 + returnType.Length + 1 + name.Length + 1 + parameters.Length + 1;

        return string.Create(totalLength, (modifiers, returnType, name, parameters), static (span, state) =>
        {
            int pos = 0;

            state.modifiers.AsSpan().CopyTo(span[pos..]);
            pos += state.modifiers.Length;
            span[pos++] = ' ';

            state.returnType.AsSpan().CopyTo(span[pos..]);
            pos += state.returnType.Length;
            span[pos++] = ' ';

            state.name.AsSpan().CopyTo(span[pos..]);
            pos += state.name.Length;
            span[pos++] = '(';

            state.parameters.AsSpan().CopyTo(span[pos..]);
            pos += state.parameters.Length;
            span[pos] = ')';
        });
    }

    /// <summary>
    /// Uses StringBuilder for complex string concatenation (suitable for template generation scenarios)
    /// </summary>
    /// <param name="parts">Array of string parts</param>
    /// <returns>The concatenated string</returns>
    private static string BuildComplexString(params string[] parts)
    {
        if (parts.Length == 0) return string.Empty;
        if (parts.Length == 1) return parts[0];

        // 对于少量字符串，直接使用 string.Concat 更高效
        if (parts.Length <= 4)
        {
            return string.Concat(parts);
        }

        // 对于大量字符串，使用 StringBuilder
        var totalLength = 0;
        foreach (var part in parts)
            totalLength += part?.Length ?? 0;

        var sb = new System.Text.StringBuilder(totalLength);
        foreach (var part in parts)
        {
            if (!string.IsNullOrEmpty(part))
                sb.Append(part);
        }
        return sb.ToString();
    }

    /// <summary>
    /// Efficiently builds parameterized statements (such as if, while, etc.)
    /// </summary>
    /// <param name="keyword">The keyword</param>
    /// <param name="condition">The condition or parameter</param>
    /// <returns>The constructed statement</returns>
    private static string BuildParameterizedStatement(string keyword, string condition)
    {
        // keyword (condition)
        int totalLength = keyword.Length + 2 + condition.Length;

        return string.Create(totalLength, (keyword, condition), static (span, state) =>
        {
            int pos = 0;
            state.keyword.AsSpan().CopyTo(span[pos..]);
            pos += state.keyword.Length;
            span[pos++] = ' ';
            span[pos++] = '(';
            state.condition.AsSpan().CopyTo(span[pos..]);
            pos += state.condition.Length;
            span[pos] = ')';
        });
    }

    /// <summary>
    /// Efficiently builds a for loop statement
    /// </summary>
    /// <param name="init">The initialization expression</param>
    /// <param name="condition">The condition expression</param>
    /// <param name="increment">The increment expression</param>
    /// <returns>The constructed for statement</returns>
    private static string BuildForStatement(string init, string condition, string increment)
    {
        // for (init; condition; increment)
        int totalLength = 5 + init.Length + 2 + condition.Length + 2 + increment.Length + 1;

        return string.Create(totalLength, (init, condition, increment), static (span, state) =>
        {
            int pos = 0;
            "for (".AsSpan().CopyTo(span[pos..]);
            pos += 5;

            state.init.AsSpan().CopyTo(span[pos..]);
            pos += state.init.Length;

            "; ".AsSpan().CopyTo(span[pos..]);
            pos += 2;

            state.condition.AsSpan().CopyTo(span[pos..]);
            pos += state.condition.Length;

            "; ".AsSpan().CopyTo(span[pos..]);
            pos += 2;

            state.increment.AsSpan().CopyTo(span[pos..]);
            pos += state.increment.Length;

            span[pos] = ')';
        });
    }

    /// <summary>
    /// Efficiently builds a foreach loop statement
    /// </summary>
    /// <param name="type">The type of the iteration variable</param>
    /// <param name="variable">The iteration variable name</param>
    /// <param name="collection">The collection to iterate over</param>
    /// <returns>The constructed foreach statement</returns>
    private static string BuildForeachStatement(string type, string variable, string collection)
    {
        // foreach (type variable in collection)
        int totalLength = 9 + type.Length + 1 + variable.Length + 4 + collection.Length + 1;

        return string.Create(totalLength, (type, variable, collection), static (span, state) =>
        {
            int pos = 0;
            "foreach (".AsSpan().CopyTo(span[pos..]);
            pos += 9;

            state.type.AsSpan().CopyTo(span[pos..]);
            pos += state.type.Length;
            span[pos++] = ' ';

            state.variable.AsSpan().CopyTo(span[pos..]);
            pos += state.variable.Length;

            " in ".AsSpan().CopyTo(span[pos..]);
            pos += 4;

            state.collection.AsSpan().CopyTo(span[pos..]);
            pos += state.collection.Length;

            span[pos] = ')';
        });
    }

    #endregion

    #region Type Definitions

    /// <summary>
    /// Generic type definition method
    /// </summary>
    /// <param name="typeKind">The type kind (such as class, interface, enum, struct)</param>
    /// <param name="name">The type name</param>
    /// <param name="action">The code building action to execute within the type</param>
    /// <param name="modifiers">The access modifiers (default is public)</param>
    /// <param name="baseTypes">The base types or interfaces (optional)</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder Type(string typeKind, string name, Action<CSharpCodeBuilder> action,
        string modifiers = "public", string? baseTypes = null)
    {
        var typeDeclaration = BuildTypeDeclaration(modifiers, typeKind, name, baseTypes);
        AppendLine(typeDeclaration);
        CodeBlock(action);
        return this;
    }

    /// <summary>
    /// Generic type definition method using a function
    /// </summary>
    /// <param name="typeKind">The type kind (such as class, interface, enum, struct)</param>
    /// <param name="name">The type name</param>
    /// <param name="func">The code building function to execute within the type</param>
    /// <param name="modifiers">The access modifiers (default is public)</param>
    /// <param name="baseTypes">The base types or interfaces (optional)</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder Type(string typeKind, string name, Func<CSharpCodeBuilder, CSharpCodeBuilder> func,
        string modifiers = "public", string? baseTypes = null)
    {
        var typeDeclaration = BuildTypeDeclaration(modifiers, typeKind, name, baseTypes);
        AppendLine(typeDeclaration);
        CodeBlock(func);
        return this;
    }

    /// <summary>
    /// Adds a class definition
    /// </summary>
    /// <param name="name">The class name</param>
    /// <param name="action">The code building action to execute within the class</param>
    /// <param name="modifiers">The access modifiers (default is public)</param>
    /// <param name="baseClass">The base class (optional)</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder Class(string name, Action<CSharpCodeBuilder> action, string modifiers = "public", string? baseClass = null)
        => Type("class", name, action, modifiers, baseClass);

    /// <summary>
    /// Adds a class definition using a function
    /// </summary>
    /// <param name="name">The class name</param>
    /// <param name="func">The code building function to execute within the class</param>
    /// <param name="modifiers">The access modifiers (default is public)</param>
    /// <param name="baseClass">The base class (optional)</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder Class(string name, Func<CSharpCodeBuilder, CSharpCodeBuilder> func, string modifiers = "public", string? baseClass = null)
        => Type("class", name, func, modifiers, baseClass);

    /// <summary>
    /// Adds a class definition (curried version)
    /// </summary>
    /// <param name="name">The class name</param>
    /// <param name="modifiers">The access modifiers (default is public)</param>
    /// <param name="baseClass">The base class (optional)</param>
    /// <returns>A function that receives a code building operation and returns the builder instance</returns>
    public Func<Func<CSharpCodeBuilder, CSharpCodeBuilder>, CSharpCodeBuilder> Class(string name, string modifiers = "public", string? baseClass = null)
        => func => Class(name, func, modifiers, baseClass);

    /// <summary>
    /// Adds an interface definition
    /// </summary>
    /// <param name="name">The interface name</param>
    /// <param name="action">The code building action to execute within the interface</param>
    /// <param name="modifiers">The access modifiers (default is public)</param>
    /// <param name="baseInterfaces">The base interfaces (optional)</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder Interface(string name, Action<CSharpCodeBuilder> action, string modifiers = "public", string? baseInterfaces = null)
        => Type("interface", name, action, modifiers, baseInterfaces);

    /// <summary>
    /// Adds an interface definition using a function
    /// </summary>
    /// <param name="name">The interface name</param>
    /// <param name="func">The code building function to execute within the interface</param>
    /// <param name="modifiers">The access modifiers (default is public)</param>
    /// <param name="baseInterfaces">The base interfaces (optional)</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder Interface(string name, Func<CSharpCodeBuilder, CSharpCodeBuilder> func, string modifiers = "public", string? baseInterfaces = null)
        => Type("interface", name, func, modifiers, baseInterfaces);

    /// <summary>
    /// Adds an interface definition (curried version)
    /// </summary>
    /// <param name="name">The interface name</param>
    /// <param name="modifiers">The access modifiers (default is public)</param>
    /// <param name="baseInterface">The base interface (optional)</param>
    /// <returns>A function that receives a code building operation and returns the builder instance</returns>
    public Func<Func<CSharpCodeBuilder, CSharpCodeBuilder>, CSharpCodeBuilder> Interface(string name, string modifiers, string? baseInterface = null)
        => func => Interface(name, func, modifiers, baseInterface);


    /// <summary>
    /// Adds an enum definition
    /// </summary>
    /// <param name="name">The enum name</param>
    /// <param name="action">The code building action to execute within the enum</param>
    /// <param name="modifiers">The access modifiers (default is public)</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder Enum(string name, Action<CSharpCodeBuilder> action, string modifiers = "public")
        => Type("enum", name, action, modifiers);

    /// <summary>
    /// Adds an enum definition using a function
    /// </summary>
    /// <param name="name">The enum name</param>
    /// <param name="func">The code building function to execute within the enum</param>
    /// <param name="modifiers">The access modifiers (default is public)</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder Enum(string name, Func<CSharpCodeBuilder, CSharpCodeBuilder> func, string modifiers = "public")
        => Type("enum", name, func, modifiers);

    /// <summary>
    /// Adds an enum definition (curried version)
    /// </summary>
    /// <param name="name">The enum name</param>
    /// <param name="modifiers">The access modifiers (default is public)</param>
    /// <returns>A function that receives a code building operation and returns the builder instance</returns>
    public Func<Func<CSharpCodeBuilder, CSharpCodeBuilder>, CSharpCodeBuilder> Enum(string name, string modifiers = "public")
        => func => Enum(name, func, modifiers);


    /// <summary>
    /// Adds a struct definition
    /// </summary>
    /// <param name="name">The struct name</param>
    /// <param name="action">The code building action to execute within the struct</param>
    /// <param name="modifiers">The access modifiers (default is public)</param>
    /// <param name="baseInterfaces">The implemented interfaces (optional)</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder Struct(string name, Action<CSharpCodeBuilder> action, string modifiers = "public", string? baseInterfaces = null)
        => Type("struct", name, action, modifiers, baseInterfaces);

    /// <summary>
    /// Adds a struct definition using a function
    /// </summary>
    /// <param name="name">The struct name</param>
    /// <param name="func">The code building function to execute within the struct</param>
    /// <param name="modifiers">The access modifiers (default is public)</param>
    /// <param name="baseInterfaces">The implemented interfaces (optional)</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder Struct(string name, Func<CSharpCodeBuilder, CSharpCodeBuilder> func, string modifiers = "public", string? baseInterfaces = null)
        => Type("struct", name, func, modifiers, baseInterfaces);

    /// <summary>
    /// Adds a struct definition (curried version)
    /// </summary>
    /// <param name="name">The struct name</param>
    /// <param name="modifiers">The access modifiers (default is public)</param>
    /// <param name="baseInterfaces">The implemented interfaces (optional)</param>
    /// <returns>A function that receives a code building operation and returns the builder instance</returns>
    public Func<Func<CSharpCodeBuilder, CSharpCodeBuilder>, CSharpCodeBuilder> Struct(string name, string modifiers = "public", string? baseInterfaces = null)
        => func => Struct(name, func, modifiers, baseInterfaces);


    #endregion

    #region Member Definitions

    /// <summary>
    /// Adds a method definition
    /// </summary>
    /// <param name="name">The method name</param>
    /// <param name="action">The code building action to execute within the method</param>
    /// <param name="returnType">The return type (default is void)</param>
    /// <param name="modifiers">The access modifiers (default is public)</param>
    /// <param name="parameters">The parameter list (default is empty)</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder Method(string name, Action<CSharpCodeBuilder> action,
        string returnType = "void", string modifiers = "public",
        string parameters = "")
    {
        var methodSignature = BuildMethodSignature(modifiers, returnType, name, parameters);
        AppendLine(methodSignature);
        CodeBlock(action);
        return this;
    }

    /// <summary>
    /// Adds a method definition using a function
    /// </summary>
    /// <param name="name">The method name</param>
    /// <param name="func">The code building function to execute within the method</param>
    /// <param name="returnType">The return type (default is void)</param>
    /// <param name="modifiers">The access modifiers (default is public)</param>
    /// <param name="parameters">The parameter list (default is empty)</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder Method(string name, Func<CSharpCodeBuilder, CSharpCodeBuilder> func,
        string returnType = "void", string modifiers = "public",
        string parameters = "")
    {
        var methodSignature = BuildMethodSignature(modifiers, returnType, name, parameters);
        AppendLine(methodSignature);
        CodeBlock(func);
        return this;
    }

    /// <summary>
    /// Adds a method definition (curried version)
    /// </summary>
    /// <param name="name">The method name</param>
    /// <param name="returnType">The return type (default is void)</param>
    /// <param name="modifiers">The access modifiers (default is public)</param>
    /// <param name="parameters">The parameter list (default is empty)</param>
    /// <returns>A function that receives a code building operation and returns the builder instance</returns>
    public Func<Func<CSharpCodeBuilder, CSharpCodeBuilder>, CSharpCodeBuilder> Method(string name, string returnType = "void", string modifiers = "public", string parameters = "")
        => func => Method(name, func, returnType, modifiers, parameters);

    /// <summary>
    /// Adds a constructor definition
    /// </summary>
    /// <param name="name">The constructor name</param>
    /// <param name="action">The code building action to execute within the constructor</param>
    /// <param name="parameters">The parameter list (default is empty)</param>
    /// <param name="modifiers">The access modifiers (default is public)</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder Constructor(string name,Action<CSharpCodeBuilder> action, string parameters, string modifiers = "public", string? baseClass = null)
    {
        var constructorSignature = BuildMethodSignature(modifiers, "", name, parameters);
        AppendLine(constructorSignature + (baseClass != null ? $" : base({baseClass})" : ""));
        CodeBlock(action);
        return this;
    }

        /// <summary>
    /// Adds a constructor definition
    /// </summary>
    /// <param name="name">The constructor name</param>
    /// <param name="func">The code building action to execute within the constructor</param>
    /// <param name="parameters">The parameter list (default is empty)</param>
    /// <param name="modifiers">The access modifiers (default is public)</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder Constructor(string name,Func<CSharpCodeBuilder, CSharpCodeBuilder> func, string parameters, string modifiers = "public", string? baseClass = null)
    {
        var constructorSignature = BuildMethodSignature(modifiers, "", name, parameters);
        AppendLine(constructorSignature + (baseClass != null ? $" : base({baseClass})" : ""));
        CodeBlock(func);
        return this;
    }

    /// <summary>
    /// Adds a constructor definition (curried version)
    /// </summary>
    /// <param name="name">The constructor name</param>
    /// <param name="parameters">The parameter list (default is empty)</param>
    /// <param name="modifiers">The access modifiers (default is public)</param>
    /// <returns>A function that receives a code building operation and returns the builder instance</returns>
    public Func<Func<CSharpCodeBuilder, CSharpCodeBuilder>, CSharpCodeBuilder> Constructor(string name, string parameters, string modifiers = "public", string? baseClass = null)
        => func => Constructor(name, func, parameters, modifiers, baseClass);


    /// <summary>
    /// Adds a property definition
    /// </summary>
    /// <param name="type">The property type</param>
    /// <param name="name">The property name</param>
    /// <param name="modifiers">The access modifiers (default is public)</param>
    /// <param name="initialValue">The initial value (optional)</param>
    /// <param name="accessors">The property accessors (default is { get; set; })</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder Property(string type, string name,
        string modifiers = "public", string? initialValue = null,
        string accessors = "{ get; set; }")
    {
        var prop = BuildPropertyDeclaration(modifiers, type, name, accessors, initialValue);
        return AppendLine(prop);
    }

    /// <summary>
    /// Adds a property definition with a custom block
    /// </summary>
    /// <param name="type">The property type</param>
    /// <param name="name">The property name</param>
    /// <param name="func">The function to execute for custom property implementation</param>
    /// <param name="modifiers">The access modifiers (default is public)</param>
    /// <param name="initialValue">The initial value (optional)</param>
    /// <param name="accessors">The property accessors (default is { get; set; })</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder Property(string type, string name, Func<CSharpCodeBuilder, CSharpCodeBuilder> func,
        string modifiers = "public", string? initialValue = null,
        string accessors = "{ get; set; }")
    {
        var prop = BuildPropertyDeclaration(modifiers, type, name, accessors, initialValue);
        AppendLine(prop);
        CodeBlock(func);
        return this;
    }

    /// <summary>
    /// Adds a property definition (curried version)
    /// </summary>
    /// <param name="type">The property type</param>
    /// <param name="name">The property name</param>
    /// <param name="modifiers">The access modifiers (default is public)</param>
    /// <param name="initialValue">The initial value (optional)</param>
    /// <param name="accessors">The property accessors (default is { get; set; })</param>
    /// <returns>A function that receives a code building operation and returns the builder instance</returns>
    public Func<Func<CSharpCodeBuilder, CSharpCodeBuilder>, CSharpCodeBuilder> PropertyBuilder(string type, string name, string modifiers = "public", string? initialValue = null, string accessors = "{ get; set; }")
        => func => Property(type, name, func, modifiers, initialValue, accessors);

    /// <summary>
    /// Adds a field definition
    /// </summary>
    /// <param name="type">The field type</param>
    /// <param name="name">The field name</param>
    /// <param name="modifiers">The access modifiers (default is private)</param>
    /// <param name="initialValue">The initial value (optional)</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder Field(string type, string name,
        string modifiers = "private", string? initialValue = null)
    {
        var field = BuildFieldDeclaration(modifiers, type, name, initialValue);
        return AppendLine(field);
    }

    #endregion

    #region Control Structures

    /// <summary>
    /// Adds an if statement
    /// </summary>
    /// <param name="condition">The condition to evaluate</param>
    /// <param name="action">The code building action to execute when the condition is true</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder If(string condition, Action<CSharpCodeBuilder> action)
    {
        AppendLine(BuildParameterizedStatement("if", condition));
        CodeBlock(action);
        return this;
    }

    /// <summary>
    /// Adds an if statement using a function
    /// </summary>
    /// <param name="condition">The condition to evaluate</param>
    /// <param name="func">The code building function to execute when the condition is true</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder If(string condition, Func<CSharpCodeBuilder, CSharpCodeBuilder> func)
    {
        AppendLine(BuildParameterizedStatement("if", condition));
        CodeBlock(func);
        return this;
    }

    /// <summary>
    /// Adds an if statement (curried version)
    /// </summary>
    /// <param name="condition">The condition to evaluate</param>
    /// <returns>A function that receives a code building operation and returns the builder instance</returns>
    public Func<Func<CSharpCodeBuilder, CSharpCodeBuilder>, CSharpCodeBuilder> If(string condition)
        => func => If(condition, func);

    /// <summary>
    /// Adds an else if statement
    /// </summary>
    /// <param name="condition">The condition to evaluate</param>
    /// <param name="action">The code building action to execute when the condition is true</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder ElseIf(string condition, Action<CSharpCodeBuilder> action)
    {
        AppendLine(BuildParameterizedStatement("else if", condition));
        CodeBlock(action);
        return this;
    }

    /// <summary>
    /// Adds an else if statement using a function
    /// </summary>
    /// <param name="condition">The condition to evaluate</param>
    /// <param name="func">The code building function to execute when the condition is true</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder ElseIf(string condition, Func<CSharpCodeBuilder, CSharpCodeBuilder> func)
    {
        AppendLine(BuildParameterizedStatement("else if", condition));
        CodeBlock(func);
        return this;
    }

    /// <summary>
    /// Adds an else if statement (curried version)
    /// </summary>
    /// <param name="condition">The condition to evaluate</param>
    /// <returns>A function that receives a code building operation and returns the builder instance</returns>
    public Func<Func<CSharpCodeBuilder, CSharpCodeBuilder>, CSharpCodeBuilder> ElseIf(string condition)
        => func => ElseIf(condition, func);

    /// <summary>
    /// Adds an else statement
    /// </summary>
    /// <param name="action">The code building action to execute in the else branch</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder Else(Action<CSharpCodeBuilder> action)
    {
        AppendLine("else");
        CodeBlock(action);
        return this;
    }

    /// <summary>
    /// Adds an else statement using a function
    /// </summary>
    /// <param name="func">The code building function to execute in the else branch</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder Else(Func<CSharpCodeBuilder, CSharpCodeBuilder> func)
    {
        AppendLine("else");
        CodeBlock(func);
        return this;
    }

    /// <summary>
    /// Adds an else statement (curried version)
    /// </summary>
    /// <returns>A function that receives a code building operation and returns the builder instance</returns>
    public Func<Func<CSharpCodeBuilder, CSharpCodeBuilder>, CSharpCodeBuilder> Else()
        => func => Else(func);

    /// <summary>
    /// Adds a for loop
    /// </summary>
    /// <param name="init">The initialization expression</param>
    /// <param name="condition">The loop condition</param>
    /// <param name="increment">The increment expression</param>
    /// <param name="action">The code building action to execute within the loop body</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder For(string init, string condition, string increment, Action<CSharpCodeBuilder> action)
    {
        AppendLine(BuildForStatement(init, condition, increment));
        CodeBlock(action);
        return this;
    }

    /// <summary>
    /// Adds a for loop using a function
    /// </summary>
    /// <param name="init">The initialization expression</param>
    /// <param name="condition">The loop condition</param>
    /// <param name="increment">The increment expression</param>
    /// <param name="func">The code building function to execute within the loop body</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder For(string init, string condition, string increment, Func<CSharpCodeBuilder, CSharpCodeBuilder> func)
    {
        AppendLine(BuildForStatement(init, condition, increment));
        CodeBlock(func);
        return this;
    }

    /// <summary>
    /// Adds a for loop (curried version)
    /// </summary>
    /// <param name="init">The initialization expression</param>
    /// <param name="condition">The loop condition</param>
    /// <param name="increment">The increment expression</param>
    /// <returns>A function that receives a code building operation and returns the builder instance</returns>
    public Func<Func<CSharpCodeBuilder, CSharpCodeBuilder>, CSharpCodeBuilder> For(string init, string condition, string increment)
        => func => For(init, condition, increment, func);

    /// <summary>
    /// Adds a foreach loop
    /// </summary>
    /// <param name="type">The type of the iteration variable</param>
    /// <param name="variable">The iteration variable name</param>
    /// <param name="collection">The collection to iterate over</param>
    /// <param name="action">The code building action to execute within the loop body</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder ForEach(string type, string variable, string collection, Action<CSharpCodeBuilder> action)
    {
        AppendLine(BuildForeachStatement(type, variable, collection));
        CodeBlock(action);
        return this;
    }

    /// <summary>
    /// Adds a foreach loop using a function
    /// </summary>
    /// <param name="type">The type of the iteration variable</param>
    /// <param name="variable">The iteration variable name</param>
    /// <param name="collection">The collection to iterate over</param>
    /// <param name="func">The code building function to execute within the loop body</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder ForEach(string type, string variable, string collection, Func<CSharpCodeBuilder, CSharpCodeBuilder> func)
    {
        AppendLine(BuildForeachStatement(type, variable, collection));
        CodeBlock(func);
        return this;
    }

    /// <summary>
    /// Adds a foreach loop (curried version)
    /// </summary>
    /// <param name="type">The type of the iteration variable</param>
    /// <param name="variable">The iteration variable name</param>
    /// <param name="collection">The collection to iterate over</param>
    /// <returns>A function that receives a code building operation and returns the builder instance</returns>
    public Func<Func<CSharpCodeBuilder, CSharpCodeBuilder>, CSharpCodeBuilder> ForEach(string type, string variable, string collection)
        => func => ForEach(type, variable, collection, func);

    /// <summary>
    /// Adds a while loop
    /// </summary>
    /// <param name="condition">The loop condition</param>
    /// <param name="action">The code building action to execute within the loop body</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder While(string condition, Action<CSharpCodeBuilder> action)
    {
        AppendLine(BuildParameterizedStatement("while", condition));
        CodeBlock(action);
        return this;
    }

    /// <summary>
    /// Adds a while loop using a function
    /// </summary>
    /// <param name="condition">The loop condition</param>
    /// <param name="func">The code building function to execute within the loop body</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder While(string condition, Func<CSharpCodeBuilder, CSharpCodeBuilder> func)
    {
        AppendLine(BuildParameterizedStatement("while", condition));
        CodeBlock(func);
        return this;
    }

    /// <summary>
    /// Adds a while loop (curried version)
    /// </summary>
    /// <param name="condition">The loop condition</param>
    /// <returns>A function that receives a code building operation and returns the builder instance</returns>
    public Func<Func<CSharpCodeBuilder, CSharpCodeBuilder>, CSharpCodeBuilder> While(string condition)
        => func => While(condition, func);

    /// <summary>
    /// Adds a try statement block
    /// </summary>
    /// <param name="action">The code building action to execute within the try block</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder Try(Action<CSharpCodeBuilder> action)
    {
        AppendLine("try");
        CodeBlock(cb => action((CSharpCodeBuilder)cb));
        return this;
    }

    /// <summary>
    /// Adds a try statement block (function version)
    /// </summary>
    /// <param name="func">The code building function to execute within the try block</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder Try(Func<CSharpCodeBuilder, CSharpCodeBuilder> func)
    {
        AppendLine("try");
        CodeBlock(func);
        return this;
    }

    /// <summary>
    /// Adds a try statement block (curried version)
    /// </summary>
    /// <returns>A function that receives a code building operation and returns the builder instance</returns>
    public Func<Func<CSharpCodeBuilder, CSharpCodeBuilder>, CSharpCodeBuilder> Try()
        => func => Try(func);

    /// <summary>
    /// Adds a catch statement block
    /// </summary>
    /// <param name="action">The code building action to execute within the catch block</param>
    /// <param name="exceptionType">The exception type (default is Exception)</param>
    /// <param name="exceptionVar">The exception variable name (default is ex)</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder Catch(Action<CSharpCodeBuilder> action, 
        string exceptionType = "Exception", string exceptionVar = "ex")
    {
        AppendLine($"catch ({exceptionType} {exceptionVar})");
        CodeBlock(cb => action((CSharpCodeBuilder)cb));
        return this;
    }

    /// <summary>
    /// Adds a catch statement block (function version)
    /// </summary>
    /// <param name="func">The code building function to execute within the catch block</param>
    /// <param name="exceptionType">The exception type (default is Exception)</param>
    /// <param name="exceptionVar">The exception variable name (default is ex)</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder Catch(Func<CSharpCodeBuilder, CSharpCodeBuilder> func,
        string exceptionType = "Exception", string exceptionVar = "ex")
    {
        AppendLine($"catch ({exceptionType} {exceptionVar})");
        CodeBlock(func);
        return this;
    }

    /// <summary>
    /// Adds a catch statement block (curried version)
    /// </summary>
    /// <param name="exceptionType">The exception type (default is Exception)</param>
    /// <param name="exceptionVar">The exception variable name (default is ex)</param>
    /// <returns>A function that receives a code building operation and returns the builder instance</returns>
    public Func<Func<CSharpCodeBuilder, CSharpCodeBuilder>, CSharpCodeBuilder> Catch(
        string exceptionType = "Exception", string exceptionVar = "ex")
        => func => Catch(func, exceptionType, exceptionVar);

    /// <summary>
    /// Adds a generic catch statement block (without specifying exception type)
    /// </summary>
    /// <param name="action">The code building action to execute within the catch block</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder CatchAll(Action<CSharpCodeBuilder> action)
    {
        AppendLine("catch");
        CodeBlock(cb => action((CSharpCodeBuilder)cb));
        return this;
    }

    /// <summary>
    /// Adds a generic catch statement block (function version)
    /// </summary>
    /// <param name="func">The code building function to execute within the catch block</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder CatchAll(Func<CSharpCodeBuilder, CSharpCodeBuilder> func)
    {
        AppendLine("catch");
        CodeBlock(func);
        return this;
    }

    /// <summary>
    /// Adds a generic catch statement block (curried version)
    /// </summary>
    /// <returns>A function that receives a code building operation and returns the builder instance</returns>
    public Func<Func<CSharpCodeBuilder, CSharpCodeBuilder>, CSharpCodeBuilder> CatchAll()
        => func => CatchAll(func);

    /// <summary>
    /// Adds a finally statement block
    /// </summary>
    /// <param name="action">The code building action to execute within the finally block</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder Finally(Action<CSharpCodeBuilder> action)
    {
        AppendLine("finally");
        CodeBlock(cb => action((CSharpCodeBuilder)cb));
        return this;
    }

    /// <summary>
    /// Adds a finally statement block (function version)
    /// </summary>
    /// <param name="func">The code building function to execute within the finally block</param>
    /// <returns>The current builder instance for method chaining</returns>
    public CSharpCodeBuilder Finally(Func<CSharpCodeBuilder, CSharpCodeBuilder> func)
    {
        AppendLine("finally");
        CodeBlock(func);
        return this;
    }

    /// <summary>
    /// Adds a finally statement block (curried version)
    /// </summary>
    /// <returns>A function that receives a code building operation and returns the builder instance</returns>
    public Func<Func<CSharpCodeBuilder, CSharpCodeBuilder>, CSharpCodeBuilder> Finally()
        => func => Finally(func);

    #endregion
}