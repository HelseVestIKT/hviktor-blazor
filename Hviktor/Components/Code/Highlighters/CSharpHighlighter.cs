using System.Text;
using System.Text.RegularExpressions;

namespace Hviktor.Components.Code.Highlighters;

/// <summary>
/// Syntax highlighting patterns for C# code.
/// </summary>
internal static partial class CSharpHighlighter
{
    /// <summary>Highlights C# source code.</summary>
    internal static string Highlight(string code, RazorContext? context = null)
    {
        return context is not null
            ? HighlighterBase.ApplyPatternsWithPostProcess(code, Patterns, RenderToken, context)
            : HighlighterBase.ApplyPatternsWithPostProcess(code, Patterns, RenderToken);
    }

    /// <summary>Regex pattern and CSS class pairs for C# highlighting.</summary>
    internal static readonly (Regex Pattern, string CssClass)[] Patterns =
    [
        (SharedPatterns.MultiLineComment(), "comment"),
        (SharedPatterns.SingleLineComment(), "comment"),
        (XmlDocComment(), "comment"),
        (RawStringLiteral(), "string"),
        (InterpolatedVerbatimString(), "interpolated"),
        (InterpolatedString(), "interpolated"),
        (VerbatimString(), "string"),
        (SharedPatterns.DoubleQuotedString(), "string"),
        (CharLiteral(), "string"),
        (SharedPatterns.NumberLiteral(), "number"),
        (Keywords(), "keyword"),
        (TypePattern(), "type"),
        (MemberAccess(), "field"),
        (Attribute(), "attr"),
        (SharedPatterns.MethodCall(), "function"),
    ];

    /// <summary>
    /// Custom token renderer that handles interpolated strings by splitting them
    /// into string literal parts and C# expression holes.
    /// </summary>
    private static void RenderToken(StringBuilder sb, string text, string cssClass, RazorContext? context)
    {
        if (cssClass != "interpolated")
        {
            HighlighterBase.AppendSpan(sb, cssClass, text);
            return;
        }

        HighlightInterpolatedString(sb, text, context);
    }

    /// <summary>
    /// Highlights a C# interpolated string, rendering the <c>$"</c> prefix and literal
    /// segments as string tokens, and <c>{...}</c> interpolation holes as C# code.
    /// </summary>
    private static void HighlightInterpolatedString(StringBuilder sb, string text, RazorContext? context)
    {
        // Determine the prefix ($", $@", @$")
        var prefixLen = text.StartsWith("$@") || text.StartsWith("@$") ? 3 : 2;
        HighlighterBase.AppendSpan(sb, "string", text[..prefixLen]);

        var inner = text[prefixLen..^1]; // content between the quotes
        ProcessInterpolatedStringContent(sb, inner, context);
        HighlighterBase.AppendSpan(sb, "string", "\"");
    }

    /// <summary>
    /// Processes the content of a C# interpolated string, handling literal segments
    /// and interpolation holes.
    /// </summary>
    private static void ProcessInterpolatedStringContent(StringBuilder sb, string inner, RazorContext? context)
    {
        var pos = 0;
        var depth = 0;
        var holeStart = -1;

        var index = 0;
        while (index < inner.Length)
        {
            var nextIncrement = GetCharacterIncrement(inner, index, depth, ref pos, ref depth, ref holeStart, sb, context);
            index += nextIncrement;
        }

        AppendRemainingStringContent(sb, inner, pos);
    }

    /// <summary>
    /// Determines how many characters to skip based on the current character and state.
    /// Updates mutable state (pos, depth, holeStart) and appends content to the StringBuilder.
    /// </summary>
    private static int GetCharacterIncrement(string inner, int index, int depth, ref int pos, ref int currentDepth,
        ref int holeStart, StringBuilder sb, RazorContext? context)
    {
        var ch = inner[index];
        var nextIncrement = 1;

        if (IsEscapedBrace(inner, index, depth))
        {
            return 2;
        }

        switch (ch)
        {
            case '{' when depth == 0:
                AppendStringBeforeInterpolation(sb, inner, pos, index);
                holeStart = index;
                currentDepth = 1;
                break;
            case '{' when depth > 0:
                currentDepth++;
                break;
            case '}' when depth > 0:
            {
                currentDepth--;
                if (currentDepth == 0)
                {
                    HighlightInterpolationHole(sb, inner, holeStart, index, context);
                    pos = index + 1;
                }

                break;
            }
            case '"' when depth > 0:
                nextIncrement = SkipNestedString(inner, index) - index + 1;
                break;
        }

        return nextIncrement;
    }

    /// <summary>
    /// Checks if the current position contains an escaped brace (<c>{{</c> or <c>}}</c>).
    /// </summary>
    private static bool IsEscapedBrace(string inner, int index, int depth)
    {
        var ch = inner[index];
        return depth == 0 &&
               index + 1 < inner.Length &&
               ((ch == '{' && inner[index + 1] == '{') || (ch == '}' && inner[index + 1] == '}'));
    }

    /// <summary>
    /// Appends any string content before an interpolation hole begins.
    /// </summary>
    private static void AppendStringBeforeInterpolation(StringBuilder sb, string inner, int pos, int index)
    {
        if (index > pos)
        {
            HighlighterBase.AppendSpan(sb, "string", inner[pos..index]);
        }
    }

    /// <summary>
    /// Highlights an interpolation hole, rendering its content as C# code.
    /// </summary>
    private static void HighlightInterpolationHole(StringBuilder sb, string inner, int holeStart, int holeEnd, RazorContext? context)
    {
        sb.Append(HighlighterBase.Encode("{"));
        var holeContent = inner[(holeStart + 1)..holeEnd];
        sb.Append(context is not null
            ? HighlighterBase.ApplyPatterns(holeContent, Patterns, context)
            : HighlighterBase.ApplyPatterns(holeContent, Patterns));
        sb.Append(HighlighterBase.Encode("}"));
    }

    /// <summary>
    /// Appends any remaining string content after the last interpolation hole.
    /// </summary>
    private static void AppendRemainingStringContent(StringBuilder sb, string inner, int pos)
    {
        if (pos < inner.Length)
        {
            HighlighterBase.AppendSpan(sb, "string", inner[pos..]);
        }
    }

    /// <summary>Skips over a nested string literal inside an interpolation hole.</summary>
    private static int SkipNestedString(string text, int start)
    {
        var nestIndex = start + 1;
        while (nestIndex < text.Length)
        {
            switch (text[nestIndex])
            {
                case '\\':
                    nestIndex += 2;
                    break;
                case '"':
                    return nestIndex;
                default:
                    nestIndex++;
                    break;
            }
        }

        return text.Length - 1;
    }

    [GeneratedRegex(@"///.*$", RegexOptions.Multiline | RegexOptions.Compiled)]
    private static partial Regex XmlDocComment();

    [GeneratedRegex(@"@""(?:""""|[^""])*""", RegexOptions.Compiled)]
    private static partial Regex VerbatimString();

    /// <summary>Matches C# 11+ raw string literals delimited by three or more double quotes.</summary>
    [GeneratedRegex(@"""{3,}[\s\S]*?""{3,}", RegexOptions.Compiled)]
    private static partial Regex RawStringLiteral();

    /// <summary>Matches C# interpolated strings like <c>$"text {expr}"</c>.</summary>
    [GeneratedRegex(@"\$""(?:[^""\\{]|\\.|(?:\{\{|\}\})|(?:\{(?:[^{}""]*(?:""(?:\\.|[^""\\])*""[^{}""]*)*|\{(?:[^{}""]*(?:""(?:\\.|[^""\\])*""[^{}""]*)*)*\})*\}))*""", RegexOptions.Compiled)]
    private static partial Regex InterpolatedString();

    /// <summary>Matches C# interpolated verbatim strings like <c>$@"text {expr}"</c> or <c>@$"..."</c>.</summary>
    [GeneratedRegex(@"(?:\$@|@\$)""(?:[^""{]|""""|(?:\{\{|\}\})|(?:\{(?:[^{}""]*(?:""(?:\\.|[^""\\])*""[^{}""]*)*|\{(?:[^{}""]*(?:""(?:\\.|[^""\\])*""[^{}""]*)*)*\})*\}))*""", RegexOptions.Compiled)]
    private static partial Regex InterpolatedVerbatimString();

    [GeneratedRegex(@"'(?:\\.|[^'\\])'", RegexOptions.Compiled)]
    private static partial Regex CharLiteral();

    [GeneratedRegex(@"\b(?:abstract|as|async|await|base|bool|break|byte|case|catch|char|checked|class|const|continue|decimal|default|delegate|do|double|else|enum|event|explicit|extern|false|finally|fixed|float|for|foreach|goto|if|implicit|in|int|interface|internal|is|lock|long|namespace|new|not|null|object|operator|or|and|out|override|params|partial|private|protected|public|readonly|record|ref|required|return|sbyte|sealed|short|sizeof|stackalloc|static|string|struct|switch|this|throw|true|try|typeof|uint|ulong|unchecked|unsafe|ushort|using|var|virtual|void|volatile|when|while|where|yield|get|set|init|value|global|nameof|nint|nuint|with|scoped|file|required)\b", RegexOptions.Compiled)]
    private static partial Regex Keywords();

    [GeneratedRegex(@"\b[A-Z][a-zA-Z0-9]*(?=\s|<|\?|\[|\.|,|\)|$)", RegexOptions.Compiled | RegexOptions.Multiline)]
    private static partial Regex TypePattern();

    [GeneratedRegex(@"(?<=\.)[a-zA-Z_]\w*(?!\s*\()", RegexOptions.Compiled)]
    private static partial Regex MemberAccess();

    [GeneratedRegex(@"\[[\w.]+(?:\(.*?\))?\]", RegexOptions.Compiled)]
    private static partial Regex Attribute();
}