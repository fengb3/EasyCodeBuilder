using System;
using System.Linq;

namespace Fengb3.EasyCodeBuilder.Csharp;

/// <summary>
/// Provides the entry point and extension methods for building C# code using a configuration-based API.
/// </summary>
public static partial class Code
{
    /// <summary>
    /// Creates a new root <see cref="CodeOption"/> to begin building code.
    /// </summary>
    /// <returns>A new <see cref="CodeOption"/> instance.</returns>
    public static CodeOption Create()
    {
        return new CodeOption();
    }

    /// <summary>
    /// Backward-compatible API: add using directives by namespace names.
    /// </summary>
    public static CodeOption Using(this CodeOption option, params string[] usings)
    {
        foreach (var u in usings)
        {
            option.AddChild<CodeOption, UsingOption>(uo => { uo.Name = u; });
        }

        // keep existing behavior: add a blank line after usings
        option.OnChildren += cb => cb.AppendLine();
        return option;
    }

    /// <summary>
    /// Add a single using directive with full configuration.
    /// </summary>
    public static CodeOption Using(this CodeOption option, Action<UsingOption> configure)
        => option.AddChild(configure);

    /// <summary>
    /// using static {typeOrNamespace};
    /// </summary>
    public static CodeOption UsingStatic(this CodeOption option, string typeOrNamespace)
        => option.AddChild<CodeOption, UsingOption>(uo =>
        {
            uo.Name = typeOrNamespace;
            uo.IsStatic = true;
        });

    /// <summary>
    /// using {alias} = {typeOrNamespace};
    /// </summary>
    public static CodeOption UsingAlias(this CodeOption option, string alias, string typeOrNamespace)
        => option.AddChild<CodeOption, UsingOption>(uo =>
        {
            uo.Alias = alias;
            uo.Name = typeOrNamespace;
        });

    /// <summary>
    /// global using {typeOrNamespace};
    /// </summary>
    public static CodeOption GlobalUsing(this CodeOption option, string typeOrNamespace)
        => option.AddChild<CodeOption, UsingOption>(uo =>
        {
            uo.Keywords.Add("global");
            uo.Name = typeOrNamespace;
        });

    /// <summary>
    /// global using static {typeOrNamespace};
    /// </summary>
    public static CodeOption GlobalUsingStatic(this CodeOption option, string typeOrNamespace)
        => option.AddChild<CodeOption, UsingOption>(uo =>
        {
            uo.Keywords.Add("global");
            uo.Name = typeOrNamespace;
            uo.IsStatic = true;
        });

    /// <summary>
    /// Creates a new child option, configures it via <paramref name="configureChild"/>,
    /// and attaches its <see cref="CodeOption.Build"/> to the parent's <see cref="CodeOption.OnChildren"/>.
    /// </summary>
    /// <param name="parent">The parent code option.</param>
    /// <param name="configureChild">Action to configure the new child.</param>
    /// <typeparam name="TParent">Parent option type.</typeparam>
    /// <typeparam name="TChild">Child option type.</typeparam>
    /// <returns>The parent option, for fluent chaining.</returns>
    public static TParent AddChild<TParent, TChild>(this TParent parent, Action<TChild> configureChild)
        where TParent : CodeOption where TChild : CodeOption, new()
    {
        var child = new TChild();
        configureChild?.Invoke(child);
        return parent.AddChild(child);
    }

    /// <summary>
    /// Attaches an existing child option's <see cref="CodeOption.Build"/> to the parent's <see cref="CodeOption.OnChildren"/>.
    /// </summary>
    /// <param name="parent">The parent code option.</param>
    /// <param name="child">The child code option to attach.</param>
    /// <typeparam name="TParent">Parent option type.</typeparam>
    /// <typeparam name="TChild">Child option type.</typeparam>
    /// <returns>The parent option, for fluent chaining.</returns>
    public static TParent AddChild<TParent, TChild>(this TParent parent, TChild child)
        where TParent : CodeOption where TChild : CodeOption
    {
        parent.OnChildren += child.Build;
        return parent;
    }

    /// <summary>
    /// 添加一行或多行代码
    /// </summary>
    /// <param name="option">代码选项</param>
    /// <param name="lines">代码行</param>
    /// <returns>代码选项</returns>
    public static CodeOption AppendLine(this CodeOption option, params string[] lines)
    {
        option.OnChildren += cb => cb.AppendLines(lines);
        return option;
    }

    /// <summary>
    /// Add a namespace to the code.
    /// </summary>
    /// <param name="option">The root code option.</param>
    /// <param name="configure">Action to configure the namespace.</param>
    /// <returns>The root code option, for fluent chaining.</returns>
    public static CodeOption Namespace(this CodeOption option, Action<NamespaceOption> configure)
        => option.AddChild(configure);

    /// <summary>
    /// Add a class to the code.
    /// </summary>
    /// <param name="option">The root code option.</param>
    /// <param name="configure">Action to configure the class.</param>
    /// <returns>The root code option, for fluent chaining.</returns>
    public static CodeOption Class(this CodeOption option, Action<TypeOption> configure)
        => option.AddChild(configure);


    /// <summary>
    /// add attribute to an element
    /// </summary>
    /// <param name="option">The code option to add attributes to.</param>
    /// <param name="attributes">Attribute declarations (e.g. "Serializable", "Obsolete").</param>
    /// <typeparam name="T">The option type.</typeparam>
    /// <returns>The option, for fluent chaining.</returns>
    public static T WithAttributes<T>(this T option, params string[] attributes) where T : CodeOption
    {
        option.BeforeChildren += cb => cb.AppendLines(attributes.Select(attr => $"[{attr}]"));
        return option;
    }
    
    /// <summary>
    /// add XML doc to an element
    /// </summary>
    /// <param name="option">The code option to add XML documentation to.</param>
    /// <param name="configure">Action to configure the XML doc.</param>
    /// <typeparam name="T">The option type.</typeparam>
    /// <returns>The option, for fluent chaining.</returns>
    public static T WithXmlDoc<T>(this T option, Action<XmlDocOption> configure) where T : CodeOption
    {
        option.BeforeChildren += cb =>
        {
            var xmlDocOption = new XmlDocOption();
            configure(xmlDocOption);
            xmlDocOption.Build(cb);
            return cb;
        };
        return option;
    }


    /// <summary>
    /// Add single or multiple lines to a code block
    /// </summary>
    /// <param name="option">The code option.</param>
    /// <param name="lines">The lines of code to add.</param>
    /// <typeparam name="T">The option type.</typeparam>
    /// <returns>The option, for fluent chaining.</returns>
    public static T AppendLines<T>(this T option, params string[] lines) where T : CodeOption
    {
        foreach (var line in lines)
            option.AppendLine(line);

        return option;
    }

    /// <summary>
    /// 构建代码
    /// </summary>
    /// <param name="root">根选项</param>
    /// <param name="cb">代码构建器</param>
    /// <returns>生成的代码字符串</returns>
    public static string Build(this CodeOption root, CodeBuilder? cb = null)
    {
        cb ??= new CodeBuilder(' ', 2, "\n{", "}", 1024);
        root.Build(cb);
        return cb.ToString();
    }
}