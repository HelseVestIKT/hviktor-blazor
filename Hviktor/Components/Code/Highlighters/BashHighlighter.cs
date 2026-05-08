using System.Text;
using System.Text.RegularExpressions;

namespace Hviktor.Components.Code.Highlighters;

/// <summary>
/// Syntax highlighting patterns for Bash and shell scripts.
/// </summary>
internal static partial class BashHighlighter
{
    /// <summary>Highlights Bash/Shell source code.</summary>
    internal static string Highlight(string code) =>
        HighlighterBase.ApplyPatternsWithPostProcess(code, Patterns, RenderToken);

    /// <summary>Regex pattern and CSS class pairs for Bash highlighting.</summary>
    internal static readonly (Regex Pattern, string CssClass)[] Patterns =
    [
        (Shebang(), "type"),
        (Comment(), "comment"),
        (SingleQuotedString(), "string"),
        (DoubleQuotedString(), "interpolated"),
        (VariableAssignment(), "field"),
        (Variable(), "field"),
        (Keywords(), "keyword"),
    ];

    /// <summary>Custom token renderer that handles string interpolation in double-quoted strings.</summary>
    private static void RenderToken(StringBuilder sb, string text, string cssClass, RazorContext? context)
    {
        if (cssClass != "interpolated")
        {
            HighlighterBase.AppendSpan(sb, cssClass, text);
            return;
        }

        HighlightInterpolatedString(sb, text);
    }

    /// <summary>
    /// Highlights a Bash double-quoted string, rendering <c>$var</c> and <c>${expr}</c>
    /// interpolation as field tokens.
    /// </summary>
    private static void HighlightInterpolatedString(StringBuilder sb, string text)
    {
        var inner = text[1..^1]; // strip quotes
        HighlighterBase.AppendSpan(sb, "string", "\"");

        var pos = 0;
        foreach (Match match in Variable().Matches(inner))
        {
            if (match.Index > pos)
            {
                HighlighterBase.AppendSpan(sb, "string", inner[pos..match.Index]);
            }

            HighlighterBase.AppendSpan(sb, "field", match.Value);
            pos = match.Index + match.Length;
        }

        if (pos < inner.Length)
        {
            HighlighterBase.AppendSpan(sb, "string", inner[pos..]);
        }

        HighlighterBase.AppendSpan(sb, "string", "\"");
    }

    /// <summary>Matches shebang lines (e.g., <c>#!/bin/bash</c>).</summary>
    [GeneratedRegex(@"^#!.*$", RegexOptions.Multiline | RegexOptions.Compiled)]
    private static partial Regex Shebang();

    /// <summary>Matches comments (excluding shebangs starting with <c>#!</c>).</summary>
    [GeneratedRegex(@"#(?!!).*$", RegexOptions.Multiline | RegexOptions.Compiled)]
    private static partial Regex Comment();

    /// <summary>Matches single-quoted strings (no interpolation).</summary>
    [GeneratedRegex(@"'[^']*'", RegexOptions.Compiled)]
    private static partial Regex SingleQuotedString();

    /// <summary>Matches double-quoted strings (may contain interpolation).</summary>
    [GeneratedRegex(@"""(?:\\.|[^""\\])*""", RegexOptions.Compiled)]
    private static partial Regex DoubleQuotedString();

    /// <summary>Matches variable assignments (e.g., <c>value</c> in <c>value="World"</c>).</summary>
    [GeneratedRegex(@"\b\w+(?==)", RegexOptions.Compiled)]
    private static partial Regex VariableAssignment();

    /// <summary>Matches variable references (e.g., <c>$value</c>, <c>${value}</c>).</summary>
    [GeneratedRegex(@"\$\w+|\$\{[^}]+\}", RegexOptions.Compiled)]
    private static partial Regex Variable();

    [GeneratedRegex(@"\b(?:if|then|else|elif|fi|for|while|do|done|case|esac|function|return|in|select|until|shift|export|source|alias|unalias|echo|printf|read|cd|pwd|ls|cat|grep|sed|awk|find|xargs|pipe|sudo|chmod|chown|mkdir|rm|cp|mv|ln|touch|head|tail|sort|uniq|wc|curl|wget|git|npm|pnpm|dotnet|docker)\b", RegexOptions.Compiled)]
    private static partial Regex Keywords();
}