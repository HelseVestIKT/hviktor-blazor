using System.Text.RegularExpressions;

namespace Hviktor.Components.Code.Highlighters;

/// <summary>
/// Syntax highlighting patterns for CSS and SCSS code.
/// </summary>
internal static partial class CssHighlighter
{
    /// <summary>Highlights CSS/SCSS source code.</summary>
    internal static string Highlight(string code) => HighlighterBase.ApplyPatterns(code, Patterns);

    /// <summary>Regex pattern and CSS class pairs for CSS/SCSS highlighting.</summary>
    internal static readonly (Regex Pattern, string CssClass)[] Patterns =
    [
        (Comment(), "comment"),
        (StringLiteral(), "string"),
        (AtRule(), "keyword"),
        (CssFunction(), "function"),
        (CssVariable(), "field"),
        (MediaFeature(), "attr"),
        (AttributeSelectorName(), "attr"),
        (PropertyName(), "attr"),
        (Number(), "number"),
        (Pseudo(), "keyword"),
        (HtmlElement(), "tag"),
        (ClassIdSelector(), "tag"),
    ];

    /// <summary>Matches CSS class selectors (e.g., <c>.button</c>) and ID selectors (e.g., <c>#main</c>).</summary>
    [GeneratedRegex(@"[.#][\w-]+", RegexOptions.Compiled)]
    private static partial Regex ClassIdSelector();

    /// <summary>Matches the attribute name inside a CSS attribute selector (e.g., <c>data-command</c> in <c>[data-command='close']</c>).</summary>
    [GeneratedRegex(@"(?<=\[)[\w-]+(?=[~|^$*]?=|])", RegexOptions.Compiled)]
    private static partial Regex AttributeSelectorName();

    /// <summary>Matches HTML element type selectors (e.g., <c>button</c>, <c>div</c>, <c>section</c>).</summary>
    [GeneratedRegex(@"(?<=^|\s|,|>|~|\+)(?:a|abbr|address|article|aside|audio|b|blockquote|body|br|button|canvas|caption|cite|code|col|colgroup|data|datalist|dd|del|details|dfn|dialog|div|dl|dt|em|embed|fieldset|figcaption|figure|footer|form|h[1-6]|head|header|hgroup|hr|html|i|iframe|img|input|ins|kbd|label|legend|li|link|main|map|mark|menu|meta|meter|nav|noscript|object|ol|optgroup|option|output|p|param|picture|pre|progress|q|rp|rt|ruby|s|samp|script|search|section|select|slot|small|source|span|strong|style|sub|summary|sup|table|tbody|td|template|textarea|tfoot|th|thead|time|title|tr|track|u|ul|var|video|wbr)(?=\s*[{:.,\[>~+\s#]|$)", RegexOptions.Multiline | RegexOptions.Compiled)]
    private static partial Regex HtmlElement();

    [GeneratedRegex(@"/\*[\s\S]*?\*/", RegexOptions.Compiled)]
    private static partial Regex Comment();

    [GeneratedRegex("""(?:"[^"]*"|'[^']*')""", RegexOptions.Compiled)]
    private static partial Regex StringLiteral();

    [GeneratedRegex(@"@\w[\w-]*", RegexOptions.Compiled)]
    private static partial Regex AtRule();

    /// <summary>Matches CSS function names like <c>var</c>, <c>calc</c>, <c>rgb</c>, etc. (before the opening paren).</summary>
    [GeneratedRegex(@"\b(?:var|calc|min|max|clamp|rgb|rgba|hsl|hsla|oklch|lch|lab|color|color-mix|linear-gradient|radial-gradient|conic-gradient|url|env|attr|counter|counters|minmax|repeat|fit-content|cubic-bezier|steps)(?=\s*\()", RegexOptions.Compiled)]
    private static partial Regex CssFunction();

    /// <summary>Matches CSS custom properties (e.g., <c>--ds-border-radius-md</c>).</summary>
    [GeneratedRegex(@"--[\w-]+", RegexOptions.Compiled)]
    private static partial Regex CssVariable();

    /// <summary>Matches media query feature names (e.g., <c>prefers-reduced-motion</c>, <c>min-width</c>).</summary>
    [GeneratedRegex(@"(?<=\()[\w-]+(?=\s*:)", RegexOptions.Compiled)]
    private static partial Regex MediaFeature();

    [GeneratedRegex(@"(?<=\{[^}]*?)[\w-]+(?=\s*:)", RegexOptions.Compiled | RegexOptions.Singleline)]
    private static partial Regex PropertyName();

    /// <summary>Matches numeric values without the unit suffix (e.g., <c>0.5</c> in <c>0.5rem</c>).</summary>
    [GeneratedRegex(@"\b\d+(?:\.\d+)?(?=%|(?:px|em|rem|vh|vw|ch|ex|s|ms|fr|deg|rad|turn|dpi|dpcm|dppx)\b)?", RegexOptions.Compiled)]
    private static partial Regex Number();

    [GeneratedRegex(@"::?\w[\w-]*", RegexOptions.Compiled)]
    private static partial Regex Pseudo();
}