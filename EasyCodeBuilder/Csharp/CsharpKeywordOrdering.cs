using System;
using System.Collections.Generic;
using System.Linq;

namespace Fengb3.EasyCodeBuilder.Csharp;

/// <summary>
/// Provides stable and legal-ish keyword ordering for generated C# code.
/// 
/// Rationale: options store keywords in sets for de-duplication, but sets are unordered.
/// This helper normalizes output so adding keywords in any order yields the same result.
/// </summary>
public static class CsharpKeywordOrdering
{
    /// <summary>
    /// Ordering for type declarations (class/struct/interface/record/enum).
    /// </summary>
    public static IEnumerable<string> OrderForType(IEnumerable<string> keywords)
        => OrderByRank(keywords, TypeKeywordRanks);

    /// <summary>
    /// Ordering for members (methods/properties/fields/constructors).
    /// </summary>
    public static IEnumerable<string> OrderForMember(IEnumerable<string> keywords)
        => OrderByRank(keywords, MemberKeywordRanks);

    /// <summary>
    /// Ordering for using directive prefixes (e.g. global using ...).
    /// Note: "static" is not included here because it appears after the "using" token.
    /// </summary>
    public static IEnumerable<string> OrderForUsingPrefix(IEnumerable<string> keywords)
        => OrderByRank(keywords, UsingPrefixKeywordRanks);

    private static IEnumerable<string> OrderByRank(IEnumerable<string> keywords, IReadOnlyDictionary<string, int> ranks)
    {
        // normalize, dedupe, ignore blanks
        var list = (keywords ?? Array.Empty<string>())
            .Where(k => !string.IsNullOrWhiteSpace(k))
            .Select(k => k.Trim())
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        return list
            .OrderBy(k => ranks.TryGetValue(k, out var r) ? r : int.MaxValue)
            .ThenBy(k => k, StringComparer.Ordinal);
    }

    // Access then modifiers. Keep it intentionally small; unknown keywords go last but stable.
    private static readonly IReadOnlyDictionary<string, int> TypeKeywordRanks =
        new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["public"] = 0,
            ["internal"] = 1,
            ["protected"] = 2,
            ["private"] = 3,

            ["new"] = 10,
            ["static"] = 11,
            ["abstract"] = 12,
            ["sealed"] = 13,
            ["partial"] = 14,
        };

    private static readonly IReadOnlyDictionary<string, int> MemberKeywordRanks =
        new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["public"] = 0,
            ["internal"] = 1,
            ["protected"] = 2,
            ["private"] = 3,

            ["new"] = 10,
            ["static"] = 11,
            ["abstract"] = 12,
            ["virtual"] = 13,
            ["override"] = 14,
            ["sealed"] = 15,

            // field/property focused
            ["readonly"] = 20,
            ["const"] = 21,

            // method focused
            ["async"] = 30,

            // less common but still basic
            ["extern"] = 40,
            ["unsafe"] = 41,
        };

    private static readonly IReadOnlyDictionary<string, int> UsingPrefixKeywordRanks =
        new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["global"] = 0,
        };
}
