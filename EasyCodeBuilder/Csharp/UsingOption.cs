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
    public static UsingOption WithKeyword(this UsingOption option, string keyword)
    {
        option.Keywords.Add(keyword);
        return option;
    }

    public static UsingOption WithName(this UsingOption option, string name)
    {
        option.Name = name;
        return option;
    }

    public static UsingOption Static(this UsingOption option)
    {
        option.IsStatic = true;
        return option;
    }

    public static UsingOption Global(this UsingOption option)
    {
        option.Keywords.Add("global");
        return option;
    }

    public static UsingOption WithAlias(this UsingOption option, string alias)
    {
        option.Alias = alias;
        return option;
    }

    public static UsingOption WithBlankLine(this UsingOption option, bool appendBlankLine = true)
    {
        option.AppendBlankLine = appendBlankLine;
        return option;
    }
}
