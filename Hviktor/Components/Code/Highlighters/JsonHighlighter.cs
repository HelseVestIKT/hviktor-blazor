using System.Text;
using System.Text.RegularExpressions;

namespace Hviktor.Components.Code.Highlighters;

/// <summary>
/// Syntax highlighting for JSON using a single combined regex with named groups.
/// </summary>
internal static partial class JsonHighlighter
{
    /// <summary>Highlights JSON source code.</summary>
    internal static string Highlight(string code)
    {
        var sb = new StringBuilder(code.Length * 2);
        var pos = 0;

        foreach (Match match in Combined().Matches(code))
        {
            if (match.Index > pos)
            {
                sb.Append(HighlighterBase.Encode(code[pos..match.Index]));
            }

            var cssClass = GetCssClass(match);

            HighlighterBase.AppendSpan(sb, cssClass, match.Value);
            pos = match.Index + match.Length;
        }

        if (pos < code.Length)
        {
            sb.Append(HighlighterBase.Encode(code[pos..]));
        }

        return sb.ToString();
    }

    /// <summary>Determines the CSS class for a JSON token match.</summary>
    private static string GetCssClass(Match match)
    {
        if (match.Groups["key"].Success) return "attr";
        if (match.Groups["str"].Success) return "string";
        if (match.Groups["num"].Success) return "number";
        if (match.Groups["kw"].Success) return "keyword";
        return "string";
    }

    [GeneratedRegex(@"(?<key>""(?:\\.|[^""\\])*"")(?=[ \t]*:)|(?<str>""(?:\\.|[^""\\])*"")|(?<num>-?\b\d+(?:\.\d+)?(?:[eE][+-]?\d+)?\b)|(?<kw>\b(?:true|false|null)\b)", RegexOptions.Compiled)]
    private static partial Regex Combined();
}