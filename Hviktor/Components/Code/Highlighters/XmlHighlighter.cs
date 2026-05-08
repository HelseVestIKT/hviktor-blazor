using System.Text.RegularExpressions;

namespace Hviktor.Components.Code.Highlighters;

/// <summary>
/// Syntax highlighting patterns for XML, HTML, and SVG markup.
/// </summary>
internal static partial class XmlHighlighter
{
    /// <summary>Highlights XML/HTML/SVG source code.</summary>
    internal static string Highlight(string code) => HighlighterBase.ApplyPatterns(code, Patterns);

    /// <summary>Regex pattern and CSS class pairs for XML highlighting.</summary>
    internal static readonly (Regex Pattern, string CssClass)[] Patterns =
    [
        (Comment(), "comment"),
        (CData(), "string"),
        (AttributeValue(), "string"),
        (TagName(), "tag"),
        (AttributeName(), "attr"),
        (Directive(), "keyword"),
    ];

    [GeneratedRegex(@"<!--[\s\S]*?-->", RegexOptions.Compiled)]
    internal static partial Regex Comment();

    [GeneratedRegex(@"<!\[CDATA\[[\s\S]*?\]\]>", RegexOptions.Compiled)]
    internal static partial Regex CData();

    [GeneratedRegex(@"=""[^""]*""", RegexOptions.Compiled)]
    internal static partial Regex AttributeValue();

    [GeneratedRegex(@"<[/?]?[\w:.-]+|\??>|/?>", RegexOptions.Compiled)]
    internal static partial Regex TagName();

    [GeneratedRegex(@"(?<=\s)[\w:.-]+(?==)", RegexOptions.Compiled)]
    internal static partial Regex AttributeName();

    [GeneratedRegex(@"@\w+", RegexOptions.Compiled)]
    private static partial Regex Directive();
}