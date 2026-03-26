using System;
using System.Collections.Generic;
using System.Linq;

namespace Fengb3.EasyCodeBuilder.Csharp;

/// <summary>
/// 
/// </summary>
public class XmlDocOption : CodeOption
{
    /// <summary>
    /// summary
    /// </summary>
    public string Summary { get; set; } = "";
    
    /// <summary>
    /// param
    /// </summary>
    public ICollection<(string Name, string Description)> Params { get; set; } = [];

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

public static class XmlDocOptionExtensions
{
    /// <summary>
    /// Set the summary for XmlDoc
    /// </summary>
    /// <param name="option"></param>
    /// <param name="summary"></param>
    /// <returns></returns>
    public static XmlDocOption WithSummary(this XmlDocOption option, string summary)
    {
        option.Summary = summary;
        return option;
    }

    /// <summary>
    /// add a set of param for XmlDoc
    /// </summary>
    /// <param name="option"></param>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <returns></returns>
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