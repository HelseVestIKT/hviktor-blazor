using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

namespace Hviktor.Components.Code.Highlighters;

/// <summary>
/// Shared utilities for syntax highlighters: token collection, sorting,
/// filtering, and HTML rendering.
/// </summary>
internal static partial class HighlighterBase
{
    /// <summary>
    /// Core highlighting engine. Collects regex matches as tokens, resolves overlaps,
    /// and renders HTML spans.
    /// </summary>
    internal static string ApplyPatterns(string code, (Regex Pattern, string CssClass)[] patterns)
    {
        var tokens = CollectTokens(code, patterns);
        var filtered = SortAndFilter(tokens);
        return RenderTokens(code, filtered);
    }

    /// <summary>
    /// Core highlighting engine with Razor context awareness for method and field recognition.
    /// </summary>
    internal static string ApplyPatterns(
        string code,
        (Regex Pattern, string CssClass)[] patterns,
        RazorContext context)
    {
        var tokens = CollectTokens(code, patterns);

        foreach (Match match in RazorIdentifierInCode().Matches(code))
        {
            var name = match.Value;
            if (context.Fields.Contains(name))
            {
                tokens.Add((match.Index, match.Length, "field"));
            }
            else if (context.Methods.Contains(name) && !SharedPatterns.MethodCall().IsMatch(code[match.Index..]))
            {
                tokens.Add((match.Index, match.Length, "function"));
            }
        }

        var filtered = SortAndFilter(tokens, useContextPriority: true);
        return RenderTokens(code, filtered);
    }

    /// <summary>
    /// Highlighting engine with a custom token renderer for special token types (e.g., interpolated strings).
    /// </summary>
    internal static string ApplyPatternsWithPostProcess(
        string code,
        (Regex Pattern, string CssClass)[] patterns,
        Action<StringBuilder, string, string, RazorContext?> tokenRenderer,
        RazorContext? context = null)
    {
        var tokens = CollectTokens(code, patterns);

        if (context is not null)
        {
            foreach (Match match in RazorIdentifierInCode().Matches(code))
            {
                var name = match.Value;
                if (context.Fields.Contains(name))
                {
                    tokens.Add((match.Index, match.Length, "field"));
                }
                else if (context.Methods.Contains(name) && !SharedPatterns.MethodCall().IsMatch(code[match.Index..]))
                {
                    tokens.Add((match.Index, match.Length, "function"));
                }
            }
        }

        var filtered = SortAndFilter(tokens, useContextPriority: context is not null);
        return RenderTokensWithPostProcess(code, filtered, tokenRenderer, context);
    }

    /// <summary>Collects all non-empty regex matches from the given patterns.</summary>
    internal static List<(int Start, int Length, string CssClass)> CollectTokens(
        string code,
        (Regex Pattern, string CssClass)[] patterns)
    {
        var tokens = new List<(int Start, int Length, string CssClass)>();

        foreach ((var pattern, var cssClass) in patterns)
        {
            foreach (Match match in pattern.Matches(code))
            {
                if (match.Length > 0)
                {
                    tokens.Add((match.Index, match.Length, cssClass));
                }
            }
        }

        return tokens;
    }

    /// <summary>Sorts tokens by position (longer first at ties), removes overlaps.</summary>
    internal static List<(int Start, int Length, string CssClass)> SortAndFilter(
        List<(int Start, int Length, string CssClass)> tokens,
        bool useContextPriority = false)
    {
        tokens.Sort((a, b) =>
        {
            if (a.Start != b.Start) return a.Start.CompareTo(b.Start);
            if (a.Length != b.Length) return b.Length.CompareTo(a.Length);
            var priority = TokenPriority(a.CssClass).CompareTo(TokenPriority(b.CssClass));
            if (priority != 0) return priority;
            return useContextPriority
                ? ContextPriority(a.CssClass).CompareTo(ContextPriority(b.CssClass))
                : 0;
        });

        var filtered = new List<(int Start, int Length, string CssClass)>();
        var lastEnd = 0;
        foreach (var token in tokens)
        {
            if (token.Start >= lastEnd)
            {
                filtered.Add(token);
                lastEnd = token.Start + token.Length;
            }
        }

        return filtered;
    }

    /// <summary>Renders sorted, non-overlapping tokens into an HTML string.</summary>
    internal static string RenderTokens(
        string code,
        List<(int Start, int Length, string CssClass)> tokens)
    {
        var sb = new StringBuilder(code.Length * 2);
        var pos = 0;

        foreach ((var start, var length, var cssClass) in tokens)
        {
            if (start > pos)
            {
                sb.Append(Encode(code[pos..start]));
            }

            AppendSpan(sb, cssClass, code.Substring(start, length));
            pos = start + length;
        }

        if (pos < code.Length)
        {
            sb.Append(Encode(code[pos..]));
        }

        return sb.ToString();
    }

    /// <summary>Renders tokens with a custom renderer for special token types.</summary>
    internal static string RenderTokensWithPostProcess(
        string code,
        List<(int Start, int Length, string CssClass)> tokens,
        Action<StringBuilder, string, string, RazorContext?> tokenRenderer,
        RazorContext? context)
    {
        var sb = new StringBuilder(code.Length * 2);
        var pos = 0;

        foreach ((var start, var length, var cssClass) in tokens)
        {
            if (start > pos)
            {
                sb.Append(Encode(code[pos..start]));
            }

            tokenRenderer(sb, code.Substring(start, length), cssClass, context);
            pos = start + length;
        }

        if (pos < code.Length)
        {
            sb.Append(Encode(code[pos..]));
        }

        return sb.ToString();
    }

    /// <summary>Appends an HTML-encoded span with the given CSS class.</summary>
    internal static void AppendSpan(StringBuilder sb, string cssClass, string text)
    {
        sb.Append($"<span class=\"codeblock-{cssClass}\">");
        sb.Append(Encode(text));
        sb.Append("</span>");
    }

    /// <summary>HTML-encodes the given text.</summary>
    internal static string Encode(string text) => HtmlEncoder.Default.Encode(text);

    /// <summary>Returns sort priority for context-aware CSS classes (lower = higher priority).</summary>
    private static int ContextPriority(string cssClass) => cssClass switch
    {
        "field" or "function" => 0,
        _ => 1,
    };

    /// <summary>Returns sort priority for overlapping token classes (lower = higher priority).</summary>
    private static int TokenPriority(string cssClass) => cssClass switch
    {
        "comment" => 0,
        "string" => 1,
        "keyword" => 2,
        "type" => 3,
        "tag" => 4,
        _ => 3,
    };

    [GeneratedRegex(@"\b[a-zA-Z_]\w*\b", RegexOptions.Compiled)]
    private static partial Regex RazorIdentifierInCode();
}

/// <summary>Holds method and field names extracted from Razor <c>@code</c> blocks.</summary>
internal sealed record RazorContext(HashSet<string> Methods, HashSet<string> Fields);

