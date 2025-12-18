using System.Collections.Generic;
using System.Linq;

namespace Fengb3.EasyCodeBuilder.Csharp;

/// <summary>
/// Option for building a field declaration.
/// Examples:
/// - private readonly int _x;
/// - public const string Name = "x";
/// - public static readonly List&lt;int&gt; Values = new();
/// </summary>
public class FieldOption : CodeOption
{
    /// <summary>
    /// Keywords for the field (e.g. public, private, static, readonly, const)
    /// </summary>
    public ICollection<string> Keywords { get; set; } = new HashSet<string>();

    /// <summary>
    /// Field type (ignored when IsVar=true)
    /// </summary>
    public string Type { get; set; } = "";

    /// <summary>
    /// Field name
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// Optional initializer, without trailing semicolon. Example: "new()" or "1"
    /// </summary>
    public string? Initializer { get; set; }

    /// <summary>
    /// If true, generates "var" instead of explicit type.
    /// Useful for local declarations, but kept here for convenience.
    /// </summary>
    public bool IsVar { get; set; } = false;

    public override CodeBuilder Build(CodeBuilder cb)
    {
        var keywords = string.Join(" ", CsharpKeywordOrdering.OrderForMember(Keywords));
        var typePart = IsVar ? "var" : Type;

        var head = string.Join(" ", new[] { keywords, typePart, Name }.Where(s => !string.IsNullOrWhiteSpace(s))).Trim();

        if (string.IsNullOrWhiteSpace(head))
            return cb;

        if (!string.IsNullOrWhiteSpace(Initializer))
        {
            cb.AppendLine($"{head} = {Initializer};");
        }
        else
        {
            cb.AppendLine($"{head};");
        }

        return cb;
    }
}

public static class FieldOptionExtensions
{
    public static FieldOption WithKeyword(this FieldOption option, string keyword)
    {
        option.Keywords.Add(keyword);
        return option;
    }

    public static FieldOption WithType(this FieldOption option, string type)
    {
        option.Type = type;
        return option;
    }

    public static FieldOption WithName(this FieldOption option, string name)
    {
        option.Name = name;
        return option;
    }

    public static FieldOption WithInitializer(this FieldOption option, string initializer)
    {
        option.Initializer = initializer;
        return option;
    }

    public static FieldOption Var(this FieldOption option)
    {
        option.IsVar = true;
        return option;
    }
}
