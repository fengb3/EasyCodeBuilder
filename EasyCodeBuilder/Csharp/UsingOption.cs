using System.Collections.Generic;

namespace Fengb3.EasyCodeBuilder.Csharp;

/// <summary>
/// Option for building using directives.
/// Supports: normal using, using static, alias using, global using.
/// </summary>
public class UsingOption : CodeOption
{
    /// <summary>
    /// using keyword modifiers (e.g. "global")
    /// </summary>
    public ICollection<string> Keywords { get; set; } = new HashSet<string>();

    /// <summary>
    /// Namespace/type to import, e.g. "System", "System.Text", "System.Math".
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// If true, generates: using static {Name};
    /// </summary>
    public bool IsStatic { get; set; }

    /// <summary>
    /// Alias name. If set, generates: using {Alias} = {Name};
    /// </summary>
    public string? Alias { get; set; }

    /// <summary>
    /// Append a blank line after this using directive.
    /// </summary>
    public bool AppendBlankLine { get; set; } = false;

    /// <inheritdoc />
    public override CodeBuilder Build(CodeBuilder cb)
    {
        var keywords = string.Join(" ", CsharpKeywordOrdering.OrderForUsingPrefix(Keywords));
        var prefix = string.IsNullOrWhiteSpace(keywords) ? "using" : $"{keywords} using";

        if (string.IsNullOrWhiteSpace(Name))
        {
            // Do nothing for invalid input, keep it safe for fluent composition.
            return cb;
        }

        if (!string.IsNullOrWhiteSpace(Alias))
        {
            cb.AppendLine($"{prefix} {Alias} = {Name};");
        }
        else if (IsStatic)
        {
            cb.AppendLine($"{prefix} static {Name};");
        }
        else
        {
            cb.AppendLine($"{prefix} {Name};");
        }

        if (AppendBlankLine)
        {
            cb.AppendLine();
        }

        return cb;
    }
}

/// <summary>
/// Fluent helpers for <see cref="UsingOption"/>.
/// </summary>
public static class UsingOptionExtensions
{
    /// <summary>
    /// Add a keyword prefix to the using directive (e.g. "global").
    /// </summary>
    /// <param name="option">The using option.</param>
    /// <param name="keyword">The keyword to add.</param>
    /// <returns>The using option, for fluent chaining.</returns>
    public static UsingOption WithKeyword(this UsingOption option, string keyword)
    {
        option.Keywords.Add(keyword);
        return option;
    }

    /// <summary>
    /// Set the namespace or type to import.
    /// </summary>
    /// <param name="option">The using option.</param>
    /// <param name="name">The namespace or type name.</param>
    /// <returns>The using option, for fluent chaining.</returns>
    public static UsingOption WithName(this UsingOption option, string name)
    {
        option.Name = name;
        return option;
    }

    /// <summary>
    /// Mark the using directive as static (generates: using static {Name}).
    /// </summary>
    /// <param name="option">The using option.</param>
    /// <returns>The using option, for fluent chaining.</returns>
    public static UsingOption Static(this UsingOption option)
    {
        option.IsStatic = true;
        return option;
    }

    /// <summary>
    /// Mark the using directive as global (generates: global using {Name}).
    /// </summary>
    /// <param name="option">The using option.</param>
    /// <returns>The using option, for fluent chaining.</returns>
    public static UsingOption Global(this UsingOption option)
    {
        option.Keywords.Add("global");
        return option;
    }

    /// <summary>
    /// Set an alias for the using directive (generates: using {Alias} = {Name}).
    /// </summary>
    /// <param name="option">The using option.</param>
    /// <param name="alias">The alias name.</param>
    /// <returns>The using option, for fluent chaining.</returns>
    public static UsingOption WithAlias(this UsingOption option, string alias)
    {
        option.Alias = alias;
        return option;
    }

    /// <summary>
    /// Whether to append a blank line after this using directive.
    /// </summary>
    /// <param name="option">The using option.</param>
    /// <param name="appendBlankLine">True to append a blank line (default).</param>
    /// <returns>The using option, for fluent chaining.</returns>
    public static UsingOption WithBlankLine(this UsingOption option, bool appendBlankLine = true)
    {
        option.AppendBlankLine = appendBlankLine;
        return option;
    }
}
