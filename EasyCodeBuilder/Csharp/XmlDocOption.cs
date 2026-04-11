using System;
using System.Collections.Generic;
using System.Linq;

namespace Fengb3.EasyCodeBuilder.Csharp;

/// <summary>
/// Represents an XML documentation comment that can be attached to code elements.
/// </summary>
public class XmlDocOption : CodeOption
{
    /// <summary>
    /// The &lt;summary&gt; text for the XML documentation.
    /// </summary>
    public string Summary { get; set; } = "";

    /// <summary>
    /// The collection of &lt;param&gt; entries for the XML documentation.
    /// </summary>
    public ICollection<(string Name, string Description)> Params { get; set; } = [];

    /// <inheritdoc />
    public override CodeBuilder Build(CodeBuilder cb)
    {
        // summary
        var summaryLines = Summary.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

        var isMultiLine = summaryLines.Length > 1;
        if (isMultiLine)
        {
            cb.AppendLine("/// <summary>");
            cb.AppendLines(summaryLines.Select(line => $"/// {line}"));
            cb.AppendLine("/// </summary>");
        }
        else
        {
            cb.AppendLine($"/// <summary>{Summary}</summary>");
        }


        // params
        foreach (var param in Params)
        {
            var descLines = param.Description.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var isMultiLineDesc = descLines.Length > 1;
            if (isMultiLineDesc)
            {
                cb.AppendLine($"/// <param name=\"{param.Name}\">");
                cb.AppendLines(descLines.Select(line => $"/// {line}"));
                cb.AppendLine("/// </param>");
            }
            else
            {
                cb.AppendLine($"/// <param name=\"{param.Name}\">{param.Description}</param>");
            }
        }

        return cb;
    }
}

/// <summary>
/// Extension methods for <see cref="XmlDocOption"/>.
/// </summary>
public static class XmlDocOptionExtensions
{
    /// <summary>
    /// Set the summary for XmlDoc
    /// </summary>
    /// <param name="option">The XML doc option.</param>
    /// <param name="summary">The summary text.</param>
    /// <returns>The XML doc option, for fluent chaining.</returns>
    public static XmlDocOption WithSummary(this XmlDocOption option, string summary)
    {
        option.Summary = summary;
        return option;
    }

    /// <summary>
    /// add a set of param for XmlDoc
    /// </summary>
    /// <param name="option">The XML doc option.</param>
    /// <param name="name">The parameter name.</param>
    /// <param name="description">The parameter description.</param>
    /// <returns>The XML doc option, for fluent chaining.</returns>
    public static XmlDocOption WithParam(this XmlDocOption option, string name, string description)
    {
        if (option.Params == null)
        {
            option.Params = new List<(string Name, string Description)>();
        }

        option.Params.Add((name, description));
        return option;
    }
}