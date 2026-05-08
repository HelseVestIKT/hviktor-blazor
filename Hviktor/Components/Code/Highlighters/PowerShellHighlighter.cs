using System.Text;
using System.Text.RegularExpressions;

namespace Hviktor.Components.Code.Highlighters;

/// <summary>
/// Syntax highlighting patterns for PowerShell scripts.
/// </summary>
internal static partial class PowerShellHighlighter
{
    /// <summary>Highlights PowerShell source code.</summary>
    internal static string Highlight(string code) => HighlighterBase.ApplyPatternsWithPostProcess(code, Patterns, RenderToken);

    /// <summary>Regex pattern and CSS class pairs for PowerShell highlighting.</summary>
    internal static readonly (Regex Pattern, string CssClass)[] Patterns =
    [
        (BlockComment(), "comment"),
        (LineComment(), "comment"),
        (SingleQuotedString(), "string"),
        (DoubleQuotedString(), "interpolated"),
        (Variable(), "field"),
        (Parameter(), "attr"),
        (Number(), "number"),
        (Keywords(), "keyword"),
        (Cmdlet(), "function"),
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
    /// Highlights a PowerShell double-quoted string, rendering <c>$var</c>
    /// interpolation as field tokens.
    /// </summary>
    private static void HighlightInterpolatedString(StringBuilder sb, string text)
    {
        HighlighterBase.AppendSpan(sb, "string", "\"");
        var inner = text[1..^1];
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

    /// <summary>Matches PowerShell block comments (<c>&lt;# ... #&gt;</c>).</summary>
    [GeneratedRegex(@"<#[\s\S]*?#>", RegexOptions.Compiled)]
    private static partial Regex BlockComment();

    /// <summary>Matches PowerShell line comments.</summary>
    [GeneratedRegex(@"#.*$", RegexOptions.Multiline | RegexOptions.Compiled)]
    private static partial Regex LineComment();

    /// <summary>Matches single-quoted strings (no interpolation).</summary>
    [GeneratedRegex(@"'[^']*'", RegexOptions.Compiled)]
    private static partial Regex SingleQuotedString();

    /// <summary>Matches double-quoted strings (may contain interpolation).</summary>
    [GeneratedRegex(@"""(?:[^""\\`]|`.|\\.|"""")*""", RegexOptions.Compiled)]
    private static partial Regex DoubleQuotedString();

    /// <summary>Matches PowerShell variables (e.g., <c>$Server</c>, <c>$env:PATH</c>).</summary>
    [GeneratedRegex(@"\$[\w:]+", RegexOptions.Compiled)]
    private static partial Regex Variable();

    /// <summary>Matches PowerShell command parameters (e.g., <c>-ComputerName</c>).</summary>
    [GeneratedRegex(@"-[A-Za-z][\w]*", RegexOptions.Compiled)]
    private static partial Regex Parameter();

    /// <summary>Matches numeric literals.</summary>
    [GeneratedRegex(@"\b\d+(?:\.\d+)?\b", RegexOptions.Compiled)]
    private static partial Regex Number();

    /// <summary>Matches PowerShell keywords.</summary>
    [GeneratedRegex(@"\b(?:if|else|elseif|foreach|for|while|do|until|switch|break|continue|return|function|filter|param|begin|process|end|try|catch|finally|throw|trap|in|exit|class|enum|using|workflow|parallel|sequence|inlinescript)\b", RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private static partial Regex Keywords();

    /// <summary>Matches PowerShell cmdlets (Verb-Noun pattern, e.g., <c>Test-Connection</c>, <c>Write-Output</c>).</summary>
    [GeneratedRegex(@"\b[A-Z][\w]+-[A-Z][\w]+\b", RegexOptions.Compiled)]
    private static partial Regex Cmdlet();
}

