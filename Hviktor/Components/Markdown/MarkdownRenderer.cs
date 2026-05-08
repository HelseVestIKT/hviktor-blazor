using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Hviktor.Components.Markdown;

/// <summary>
/// Minimal Markdown-to-HTML renderer for trusted, developer-authored strings.
/// </summary>
/// <remarks>
/// <para>Supported block-level syntax:</para>
/// <list type="bullet">
///   <item><c>\n\n</c> - paragraph breaks</item>
///   <item><c>---</c> / <c>***</c> / <c>___</c> - horizontal rule</item>
///   <item><c>&gt; text</c> - blockquote (including nested <c>&gt;&gt;</c>)</item>
///   <item><c>- item</c> / <c>* item</c> - unordered list (with indentation nesting)</item>
///   <item><c>1. item</c> - ordered list (with indentation nesting)</item>
///   <item>GFM pipe tables (<c>| col | col |</c> with a separator row)</item>
///   <item>Fenced code blocks (<c>```</c>)</item>
///   <item>Raw HTML blocks (lines starting with <c>&lt;</c>) - passed through verbatim</item>
/// </list>
/// <para>Supported inline syntax:</para>
/// <list type="bullet">
///   <item><c>**bold**</c> / <c>__bold__</c></item>
///   <item><c>*italic*</c> / <c>_italic_</c></item>
///   <item><c>`inline code`</c></item>
///   <item><c>~~strikethrough~~</c></item>
///   <item><c>[text](url)</c> - link</item>
///   <item><c>![alt](url "title")</c> - image</item>
/// </list>
/// <para>
/// All text nodes are HTML-encoded before inline patterns are applied, so the output
/// is safe as long as the input comes from controlled registry literals - never from
/// user-supplied content.
/// </para>
/// <para>
/// Raw HTML blocks are passed through without encoding. Only use with trusted,
/// developer-authored strings - never with user-supplied content.
/// </para>
/// </remarks>
internal static partial class MarkdownRenderer
{
    /// <summary>
    /// Represents a segment of parsed Markdown output.
    /// </summary>
    /// <param name="Content">The HTML content or raw code text.</param>
    /// <param name="IsCodeBlock">Whether this segment is a fenced code block.</param>
    /// <param name="Language">The language identifier for code blocks.</param>
    internal sealed record MarkdownSegment(string Content, bool IsCodeBlock = false, string Language = "plaintext");

    // Inline regexes (order matters - bold before italic)
    [GeneratedRegex(@"\*\*(.+?)\*\*")]
    private static partial Regex BoldAsterisksRegex();

    [GeneratedRegex(@"__(.+?)__")]
    private static partial Regex BoldUnderscoreRegex();

    [GeneratedRegex(@"(?<!\*)\*(?!\*)(.+?)(?<!\*)\*(?!\*)")]
    private static partial Regex ItalicAsterisksRegex();

    [GeneratedRegex(@"(?<!_)_(?!_)(.+?)(?<!_)_(?!_)")]
    private static partial Regex ItalicUnderscoreRegex();

    [GeneratedRegex(@"`([^`]+)`")]
    private static partial Regex InlineCodeRegex();

    [GeneratedRegex(@"~~(.+?)~~")]
    private static partial Regex StrikethroughRegex();

    [GeneratedRegex(@"!\[([^\]]*)\]\(([^\s)]+)(?:\s+&quot;([^&]*)&quot;)?\)")]
    private static partial Regex ImageRegex();

    [GeneratedRegex(@"\[([^\]]+)\]\(([^)]+)\)")]
    private static partial Regex LinkRegex();

    // Block-level regexes

    [GeneratedRegex(@"^#{1,6}\s+(.+)$")]
    private static partial Regex HeadingLineRegex();

    [GeneratedRegex(@"^---+$|^\*\*\*+$|^___+$")]
    private static partial Regex HorizontalRuleRegex();

    [GeneratedRegex(@"^>+\s?(.*)$")]
    private static partial Regex BlockquoteLineRegex();

    [GeneratedRegex(@"^(\s*)[-*]\s+(.+)$")]
    private static partial Regex UnorderedListItemRegex();

    [GeneratedRegex(@"^(\s*)\d+\.\s+(.+)$")]
    private static partial Regex OrderedListItemRegex();

    // A GFM table separator row: e.g. | --- | :---: | ---: |
    [GeneratedRegex(@"^\|[\s\-:|]+\|[\s\-:|]*$")]
    private static partial Regex TableSeparatorRegex();

    // Any pipe-delimited row: starts and ends with optional whitespace + pipe
    [GeneratedRegex(@"^\|(.+)\|$")]
    private static partial Regex TableRowRegex();

    // An HTML block opener: trimmed line starts with '<'
    [GeneratedRegex(@"^<")]
    private static partial Regex HtmlBlockStartRegex();

    // Fenced code block opener/closer
    [GeneratedRegex(@"^```")]
    private static partial Regex FencedCodeRegex();

    /// <summary>
    /// Converts a Markdown string to an HTML fragment suitable for rendering as a
    /// <see cref="Microsoft.AspNetCore.Components.MarkupString"/>.
    /// </summary>
    /// <param name="markdown">Developer-authored Markdown text.</param>
    /// <returns>HTML string with block and inline formatting applied.</returns>
    internal static string Render(string markdown)
    {
        if (string.IsNullOrWhiteSpace(markdown))
        {
            return string.Empty;
        }

        var lines = markdown.Replace("\r\n", "\n").Split('\n');
        var sb = new StringBuilder();
        var i = 0;

        while (i < lines.Length)
        {
            var line = lines[i];
            var trimmed = line.Trim();

            // Blank line
            if (string.IsNullOrWhiteSpace(trimmed))
            {
                i++;
                continue;
            }

            // Fenced code block
            if (FencedCodeRegex().IsMatch(trimmed))
            {
                i++;
                sb.Append("<div class=\"codeblock-container\"><pre class=\"codeblock-pre\" tabindex=\"0\"><code class=\"codeblock-code\">");
                while (i < lines.Length && !FencedCodeRegex().IsMatch(lines[i].Trim()))
                {
                    sb.Append(WebUtility.HtmlEncode(lines[i]));
                    sb.Append('\n');
                    i++;
                }

                // Skip closing fence
                if (i < lines.Length)
                {
                    i++;
                }

                sb.Append("</code></pre></div>");
                continue;
            }

            // Raw HTML block - pass through verbatim until a blank line or end of input.
            if (HtmlBlockStartRegex().IsMatch(trimmed))
            {
                while (i < lines.Length && !string.IsNullOrWhiteSpace(lines[i]))
                {
                    sb.Append(lines[i]);
                    i++;
                }

                continue;
            }

            // Horizontal rule
            if (HorizontalRuleRegex().IsMatch(trimmed))
            {
                sb.Append("<hr class=\"ds-divider\" />");
                i++;
                continue;
            }

            // Heading (# ... ######)
            var headingMatch = HeadingLineRegex().Match(trimmed);
            if (headingMatch.Success)
            {
                var level = trimmed.TakeWhile(c => c == '#').Count();
                if (level is >= 1 and <= 6)
                {
                    sb.Append($"<h{level} class=\"ds-heading\">");
                    sb.Append(RenderInline(headingMatch.Groups[1].Value.Trim()));
                    sb.Append($"</h{level}>");
                    i++;
                    continue;
                }
            }

            // Blockquote
            if (BlockquoteLineRegex().IsMatch(trimmed))
            {
                var blockquoteLines = new List<string>();
                while (i < lines.Length)
                {
                    var bqMatch = BlockquoteLineRegex().Match(lines[i].Trim());
                    if (!bqMatch.Success)
                    {
                        break;
                    }

                    blockquoteLines.Add(bqMatch.Groups[1].Value);
                    i++;
                }

                sb.Append("<blockquote class=\"markdown-blockquote\">");
                sb.Append(Render(string.Join("\n", blockquoteLines)));
                sb.Append("</blockquote>");
                continue;
            }

            // GFM pipe table
            if (TableRowRegex().IsMatch(trimmed)
                && i + 1 < lines.Length
                && TableSeparatorRegex().IsMatch(lines[i + 1].Trim()))
            {
                var headerCells = SplitTableRow(trimmed);
                sb.Append("<table class=\"ds-table\"><thead><tr>");
                foreach (var cell in headerCells)
                {
                    sb.Append("<th>");
                    sb.Append(RenderInline(cell));
                    sb.Append("</th>");
                }

                sb.Append("</tr></thead><tbody>");

                i += 2;

                while (i < lines.Length)
                {
                    var rowTrimmed = lines[i].Trim();
                    var rowMatch = TableRowRegex().Match(rowTrimmed);
                    if (!rowMatch.Success)
                    {
                        break;
                    }

                    sb.Append("<tr>");
                    foreach (var cell in SplitTableRow(rowTrimmed))
                    {
                        sb.Append("<td>");
                        sb.Append(RenderInline(cell));
                        sb.Append("</td>");
                    }

                    sb.Append("</tr>");
                    i++;
                }

                sb.Append("</tbody></table>");
                continue;
            }

            // Unordered list
            if (UnorderedListItemRegex().IsMatch(line))
            {
                i = RenderList(lines, i, sb, isOrdered: false);
                continue;
            }

            // Ordered list
            if (OrderedListItemRegex().IsMatch(line))
            {
                i = RenderList(lines, i, sb, isOrdered: true);
                continue;
            }

            // Paragraph (collect consecutive non-block lines)
            var paragraphLines = new List<string>();
            while (i < lines.Length)
            {
                var pLine = lines[i].Trim();
                if (string.IsNullOrWhiteSpace(pLine)
                    || HtmlBlockStartRegex().IsMatch(pLine)
                    || HorizontalRuleRegex().IsMatch(pLine)
                    || HeadingLineRegex().IsMatch(pLine)
                    || BlockquoteLineRegex().IsMatch(pLine)
                    || UnorderedListItemRegex().IsMatch(lines[i])
                    || OrderedListItemRegex().IsMatch(lines[i])
                    || FencedCodeRegex().IsMatch(pLine)
                    || (TableRowRegex().IsMatch(pLine) && i + 1 < lines.Length && TableSeparatorRegex().IsMatch(lines[i + 1].Trim())))
                {
                    break;
                }

                paragraphLines.Add(pLine);
                i++;
            }

            if (paragraphLines.Count > 0)
            {
                sb.Append("<p class=\"ds-paragraph\">");
                sb.Append(string.Join("<br/>", paragraphLines.Select(RenderInline)));
                sb.Append("</p>");
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// Parses a Markdown string into a sequence of segments, separating fenced code blocks
    /// from rendered HTML so that code blocks can be rendered with dedicated components.
    /// </summary>
    /// <param name="markdown">Developer-authored Markdown text.</param>
    /// <returns>An enumerable of <see cref="MarkdownSegment"/> instances.</returns>
    internal static IReadOnlyList<MarkdownSegment> Parse(string markdown)
    {
        if (string.IsNullOrWhiteSpace(markdown))
        {
            return [];
        }

        var lines = markdown.Replace("\r\n", "\n").Split('\n');
        var segments = new List<MarkdownSegment>();
        var htmlLines = new List<string>();
        var i = 0;

        while (i < lines.Length)
        {
            var trimmed = lines[i].Trim();

            // Fenced code block - extract as a separate segment
            if (FencedCodeRegex().IsMatch(trimmed))
            {
                // Flush accumulated HTML lines
                if (htmlLines.Count > 0)
                {
                    var html = Render(string.Join("\n", htmlLines));
                    if (!string.IsNullOrEmpty(html))
                    {
                        segments.Add(new MarkdownSegment(html));
                    }

                    htmlLines.Clear();
                }

                // Extract language from opening fence (e.g. ```csharp)
                var lang = trimmed.Length > 3 ? trimmed[3..].Trim() : "plaintext";
                if (string.IsNullOrEmpty(lang))
                {
                    lang = "plaintext";
                }

                i++;
                var codeLines = new List<string>();
                while (i < lines.Length && !FencedCodeRegex().IsMatch(lines[i].Trim()))
                {
                    codeLines.Add(lines[i]);
                    i++;
                }

                // Skip closing fence
                if (i < lines.Length)
                {
                    i++;
                }

                segments.Add(new MarkdownSegment(string.Join("\n", codeLines), IsCodeBlock: true, Language: lang));
                continue;
            }

            htmlLines.Add(lines[i]);
            i++;
        }

        // Flush remaining HTML
        if (htmlLines.Count > 0)
        {
            var html = Render(string.Join("\n", htmlLines));
            if (!string.IsNullOrEmpty(html))
            {
                segments.Add(new MarkdownSegment(html));
            }
        }

        return segments;
    }

    /// <summary>
    /// Renders a list (ordered or unordered) with support for nested lists via indentation.
    /// </summary>
    private static int RenderList(string[] lines, int i, StringBuilder sb, bool isOrdered)
    {
        var tag = isOrdered ? "ol" : "ul";
        var regex = isOrdered ? OrderedListItemRegex() : UnorderedListItemRegex();

        sb.Append($"<{tag} class=\"ds-list\">");

        // Determine the base indentation from the first item
        var baseIndent = lines[i].Length - lines[i].TrimStart().Length;

        while (i < lines.Length)
        {
            var currentLine = lines[i];
            var currentTrimmed = currentLine.Trim();

            if (string.IsNullOrWhiteSpace(currentTrimmed))
            {
                break;
            }

            var currentIndent = currentLine.Length - currentLine.TrimStart().Length;

            // If indentation is less than or equal to base, check if it's a list item at this level
            if (currentIndent <= baseIndent)
            {
                var match = regex.Match(currentLine);
                if (!match.Success)
                {
                    break;
                }

                sb.Append("<li>");
                sb.Append(RenderInline(match.Groups[2].Value));
                sb.Append("</li>");
                i++;
            }
            else
            {
                // Nested list - determine its type and recurse
                if (UnorderedListItemRegex().IsMatch(currentLine))
                {
                    i = RenderList(lines, i, sb, isOrdered: false);
                }
                else if (OrderedListItemRegex().IsMatch(currentLine))
                {
                    i = RenderList(lines, i, sb, isOrdered: true);
                }
                else
                {
                    break;
                }
            }
        }

        sb.Append($"</{tag}>");
        return i;
    }

    /// <summary>
    /// Splits a GFM table row by pipe delimiters, trimming surrounding pipes and cell whitespace.
    /// </summary>
    /// <param name="row">A trimmed table row string such as <c>| A | B | C |</c>.</param>
    /// <returns>An enumerable of trimmed cell strings.</returns>
    private static IEnumerable<string> SplitTableRow(string row)
    {
        var inner = row.Trim('|');
        return inner.Split('|').Select(c => c.Trim());
    }

    /// <summary>
    /// Applies inline Markdown patterns to a single run of text.
    /// Text is HTML-encoded first; then safe replacement tags are injected.
    /// </summary>
    private static string RenderInline(string text)
    {
        var encoded = WebUtility.HtmlEncode(text);

        // Bold (** and __)
        encoded = BoldAsterisksRegex()
            .Replace(encoded, m => $"<strong>{m.Groups[1].Value}</strong>");

        encoded = BoldUnderscoreRegex()
            .Replace(encoded, m => $"<strong>{m.Groups[1].Value}</strong>");

        // Strikethrough
        encoded = StrikethroughRegex()
            .Replace(encoded, m => $"<del>{m.Groups[1].Value}</del>");

        // Italic (* and _)
        encoded = ItalicAsterisksRegex()
            .Replace(encoded, m => $"<em>{m.Groups[1].Value}</em>");

        encoded = ItalicUnderscoreRegex()
            .Replace(encoded, m => $"<em>{m.Groups[1].Value}</em>");

        // Inline code
        encoded = InlineCodeRegex()
            .Replace(encoded, m => $"<code class=\"inline\">{m.Groups[1].Value}</code>");

        // Images (before links to avoid conflict)
        encoded = ImageRegex().Replace(encoded, m =>
        {
            var alt = m.Groups[1].Value;
            var src = m.Groups[2].Value;
            var title = m.Groups[3].Success ? $" title=\"{m.Groups[3].Value}\"" : "";
            return $"<img src=\"{src}\" alt=\"{alt}\"{title} />";
        });

        // Links
        encoded = LinkRegex().Replace(encoded, m =>
        {
            var label = m.Groups[1].Value;
            var href = WebUtility.HtmlEncode(m.Groups[2].Value);
            return $"<a href=\"{href}\" target=\"_blank\" rel=\"noopener noreferrer\">{label}</a>";
        });

        return encoded;
    }
}