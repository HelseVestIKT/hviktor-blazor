using System.Text.RegularExpressions;

namespace Hviktor.Components.Code.Highlighters;

/// <summary>
/// Syntax highlighting patterns for YAML code.
/// </summary>
internal static partial class YamlHighlighter
{
    /// <summary>Highlights YAML source code.</summary>
    internal static string Highlight(string code) => HighlighterBase.ApplyPatterns(code, Patterns);

    /// <summary>Regex pattern and CSS class pairs for YAML highlighting.</summary>
    internal static readonly (Regex Pattern, string CssClass)[] Patterns =
    [
        (Comment(), "comment"),
        (StringLiteral(), "string"),
        (BlockScalarIndicator(), "keyword"),
        (Tag(), "type"),
        (Anchor(), "type"),
        (AliasRef(), "field"),
        (Key(), "attr"),
        (Keyword(), "keyword"),
        (Number(), "number"),
        (FlowValue(), "string"),
    ];

    /// <summary>Matches YAML comments.</summary>
    [GeneratedRegex(@"#.*$", RegexOptions.Multiline | RegexOptions.Compiled)]
    private static partial Regex Comment();

    /// <summary>Matches single- and double-quoted strings.</summary>
    [GeneratedRegex(@"(?:""[^""]*""|'[^']*')", RegexOptions.Compiled)]
    private static partial Regex StringLiteral();

    /// <summary>Matches block scalar indicators (<c>|</c> and <c>&gt;</c> with optional modifiers).</summary>
    [GeneratedRegex(@"(?<=:\s*)[|>][-+]?\s*$", RegexOptions.Multiline | RegexOptions.Compiled)]
    private static partial Regex BlockScalarIndicator();

    /// <summary>Matches YAML tags (e.g., <c>!!str</c>, <c>!custom</c>).</summary>
    [GeneratedRegex(@"!{1,2}[\w.-]+", RegexOptions.Compiled)]
    private static partial Regex Tag();

    /// <summary>Matches YAML anchors (e.g., <c>&amp;anchor</c>).</summary>
    [GeneratedRegex(@"&\w+", RegexOptions.Compiled)]
    private static partial Regex Anchor();

    /// <summary>Matches YAML alias references (e.g., <c>*anchor</c>).</summary>
    [GeneratedRegex(@"\*\w+", RegexOptions.Compiled)]
    private static partial Regex AliasRef();

    /// <summary>Matches YAML mapping keys (including nested and hyphen-containing keys).</summary>
    [GeneratedRegex(@"(?<=^[\s-]*)[\w][\w.-]*(?=\s*:(?:\s|$))", RegexOptions.Multiline | RegexOptions.Compiled)]
    private static partial Regex Key();

    /// <summary>Matches YAML boolean and null keywords.</summary>
    [GeneratedRegex(@"(?<=:\s+)\b(?:true|false|null|yes|no|on|off|TRUE|FALSE|NULL|YES|NO|ON|OFF|True|False|Null|Yes|No|On|Off)\b", RegexOptions.Compiled)]
    private static partial Regex Keyword();

    /// <summary>Matches numeric values after a colon (integers, floats, hex, octal, infinity, NaN).</summary>
    [GeneratedRegex(@"(?<=:\s+)-?(?:0x[0-9a-fA-F]+|0o[0-7]+|\d+(?:\.\d+)?(?:[eE][+-]?\d+)?|\.inf|\.nan)\s*$", RegexOptions.Multiline | RegexOptions.Compiled)]
    private static partial Regex Number();

    /// <summary>Matches unquoted values inside flow sequences (e.g., <c>main</c> in <c>[main]</c>).</summary>
    [GeneratedRegex(@"(?<=[\[,]\s*)[\w][\w./@-]*(?=\s*[,\]])", RegexOptions.Compiled)]
    private static partial Regex FlowValue();
}