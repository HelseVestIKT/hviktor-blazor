using System.Text;
using System.Text.RegularExpressions;

namespace Hviktor.Components.Code.Highlighters;

/// <summary>
/// Syntax highlighting patterns for JavaScript and TypeScript code.
/// </summary>
internal static partial class JavaScriptHighlighter
{
    /// <summary>Highlights JavaScript/TypeScript source code.</summary>
    internal static string Highlight(string code)
    {
        var context = BuildContext(code);
        return HighlighterBase.ApplyPatternsWithPostProcess(code, Patterns, RenderToken, context);
    }

    /// <summary>Regex pattern and CSS class pairs for JavaScript/TypeScript highlighting.</summary>
    internal static readonly (Regex Pattern, string CssClass)[] Patterns =
    [
        (SharedPatterns.MultiLineComment(), "comment"),
        (SharedPatterns.SingleLineComment(), "comment"),
        (TemplateLiteral(), "interpolated"),
        (SharedPatterns.DoubleQuotedString(), "string"),
        (SingleQuotedString(), "string"),
        (SharedPatterns.NumberLiteral(), "number"),
        (Keywords(), "keyword"),
        (MemberAccess(), "field"),
        (SharedPatterns.MethodCall(), "function"),
    ];

    /// <summary>Builds a context with declared functions, variables, and parameters.</summary>
    private static RazorContext BuildContext(string code)
    {
        var methods = new HashSet<string>();
        var fields = new HashSet<string>();

        foreach (Match m in FunctionDeclaration().Matches(code))
        {
            methods.Add(m.Groups["name"].Value);
        }

        foreach (Match m in VariableDeclaration().Matches(code))
        {
            fields.Add(m.Groups["name"].Value);
        }

        foreach (Match m in ParameterName().Matches(code))
        {
            fields.Add(m.Groups["name"].Value);
        }

        return new RazorContext(methods, fields);
    }

    /// <summary>Custom token renderer that handles template literals and JSDoc comments.</summary>
    private static void RenderToken(StringBuilder sb, string text, string cssClass, RazorContext? context)
    {
        switch (cssClass)
        {
            case "interpolated":
                HighlightTemplateLiteral(sb, text);
                break;
            case "comment" when text.StartsWith("/**") || JsDocTag().IsMatch(text):
                HighlightJsDoc(sb, text);
                break;
            default:
                HighlighterBase.AppendSpan(sb, cssClass, text);
                break;
        }
    }

    /// <summary>
    /// Highlights a JSDoc comment, rendering tags as keywords, type expressions
    /// as types, and parameter names as fields.
    /// </summary>
    private static void HighlightJsDoc(StringBuilder sb, string text)
    {
        var pos = 0;
        foreach (Match m in JsDocToken().Matches(text))
        {
            if (m.Index > pos)
            {
                HighlighterBase.AppendSpan(sb, "comment", text[pos..m.Index]);
            }

            RenderJsDocMatch(sb, m);
            pos = m.Index + m.Length;
        }

        if (pos < text.Length)
        {
            HighlighterBase.AppendSpan(sb, "comment", text[pos..]);
        }
    }

    /// <summary>Renders a single JSDoc token match (tag, type expression, or parameter name).</summary>
    private static void RenderJsDocMatch(StringBuilder sb, Match m)
    {
        if (m.Groups["tag"].Success)
        {
            HighlighterBase.AppendSpan(sb, "keyword", m.Groups["tag"].Value);
        }
        else if (m.Groups["type"].Success)
        {
            HighlighterBase.AppendSpan(sb, "comment", "{");
            HighlightJsDocType(sb, m.Groups["type"].Value);
            HighlighterBase.AppendSpan(sb, "comment", "}");

            if (m.Groups["param"].Success)
            {
                sb.Append(' ');
                HighlighterBase.AppendSpan(sb, "field", m.Groups["param"].Value);
            }
        }
        else if (m.Groups["param"].Success)
        {
            HighlighterBase.AppendSpan(sb, "field", m.Groups["param"].Value);
        }
    }

    /// <summary>Highlights type expressions inside JSDoc curly braces.</summary>
    private static void HighlightJsDocType(StringBuilder sb, string typeContent)
    {
        var pos = 0;
        foreach (Match m in JsDocTypeToken().Matches(typeContent))
        {
            if (m.Index > pos)
            {
                HighlighterBase.AppendSpan(sb, "comment", typeContent[pos..m.Index]);
            }

            if (m.Groups["import"].Success)
            {
                HighlighterBase.AppendSpan(sb, "keyword", "import");
                HighlighterBase.AppendSpan(sb, "comment", "(");
                HighlighterBase.AppendSpan(sb, "string", m.Groups["path"].Value);
                HighlighterBase.AppendSpan(sb, "comment", ")");
            }
            else if (m.Groups["name"].Success)
            {
                HighlighterBase.AppendSpan(sb, "type", m.Groups["name"].Value);
            }

            pos = m.Index + m.Length;
        }

        if (pos < typeContent.Length)
        {
            HighlighterBase.AppendSpan(sb, "comment", typeContent[pos..]);
        }
    }

    /// <summary>
    /// Highlights a JS/TS template literal, rendering <c>${...}</c> interpolation
    /// holes as JavaScript code.
    /// </summary>
    private static void HighlightTemplateLiteral(StringBuilder sb, string text)
    {
        HighlighterBase.AppendSpan(sb, "string", "`");
        var inner = text[1..^1];
        var pos = 0;

        while (pos < inner.Length)
        {
            if (IsInterpolationStart(inner, pos))
            {
                pos = ProcessInterpolation(sb, inner, pos, pos);
            }
            else if (inner[pos] == '\\')
            {
                pos += 2; // skip escaped char
            }
            else
            {
                pos++;
            }
        }

        HighlighterBase.AppendSpan(sb, "string", "`");
    }

    /// <summary>
    /// Checks if the current position starts an interpolation hole (<c>${</c>).
    /// </summary>
    private static bool IsInterpolationStart(string inner, int i)
    {
        return inner[i] == '$' && i + 1 < inner.Length && inner[i + 1] == '{';
    }

    /// <summary>
    /// Processes an interpolation hole, extracts its content, and highlights it.
    /// Returns the position after the closing brace.
    /// </summary>
    private static int ProcessInterpolation(StringBuilder sb, string inner, int startPos, int lastPos)
    {
        // Append any string content before the interpolation
        if (startPos > lastPos)
        {
            HighlighterBase.AppendSpan(sb, "string", inner[lastPos..startPos]);
        }

        var closingIndex = FindClosingBrace(inner, startPos + 2);
        sb.Append(HighlighterBase.Encode("${"));

        var holeContent = inner[(startPos + 2)..closingIndex];
        sb.Append(HighlighterBase.ApplyPatterns(holeContent, Patterns));
        sb.Append(HighlighterBase.Encode("}"));

        return closingIndex + 1;
    }

    /// <summary>
    /// Finds the index of the closing brace that matches the opening brace at the given start position.
    /// </summary>
    private static int FindClosingBrace(string inner, int start)
    {
        var depth = 1;
        var index = start;
        while (index < inner.Length)
        {
            var ch = inner[index];
            if (ch == '{')
            {
                depth++;
            }
            else if (ch == '}')
            {
                depth--;
                if (depth == 0)
                {
                    break;
                }
            }

            index++;
        }

        return index;
    }

    [GeneratedRegex(@"`(?:\\.|[^`])*`", RegexOptions.Compiled)]
    private static partial Regex TemplateLiteral();

    [GeneratedRegex(@"'(?:\\.|[^'\\])*'", RegexOptions.Compiled)]
    private static partial Regex SingleQuotedString();

    [GeneratedRegex(@"\b(?:abstract|as|async|await|break|case|catch|class|const|continue|debugger|default|delete|do|else|enum|export|extends|false|finally|for|from|function|get|if|implements|import|in|instanceof|interface|let|new|null|of|package|private|protected|public|return|set|static|super|switch|this|throw|true|try|typeof|undefined|var|void|while|with|yield|type|declare|module|namespace|require)\b", RegexOptions.Compiled)]
    private static partial Regex Keywords();

    /// <summary>Matches property access after a dot (not followed by a method call).</summary>
    [GeneratedRegex(@"(?<=[\.\?]\.?)[a-zA-Z_]\w*(?!\s*\()", RegexOptions.Compiled)]
    private static partial Regex MemberAccess();

    /// <summary>Matches named function declarations.</summary>
    [GeneratedRegex(@"\bfunction\s+(?<name>[a-zA-Z_]\w*)", RegexOptions.Compiled)]
    private static partial Regex FunctionDeclaration();

    /// <summary>Matches variable declarations (let, const, var).</summary>
    [GeneratedRegex(@"\b(?:let|const|var)\s+(?<name>[a-zA-Z_]\w*)", RegexOptions.Compiled)]
    private static partial Regex VariableDeclaration();

    /// <summary>Matches function parameter names.</summary>
    [GeneratedRegex(@"(?:function\s*\w*\s*\(|(?:=>|\()\s*)(?<name>[a-zA-Z_]\w*)", RegexOptions.Compiled)]
    private static partial Regex ParameterName();

    /// <summary>Matches a JSDoc tag (e.g., <c>@param</c>, <c>@type</c>).</summary>
    [GeneratedRegex(@"@\w+", RegexOptions.Compiled)]
    private static partial Regex JsDocTag();

    /// <summary>Matches JSDoc tokens: tags, type expressions in braces, and parameter names after types.</summary>
    [GeneratedRegex(@"(?<tag>@\w+)|\{(?<type>[^}]+)\}\s*(?<param>[a-zA-Z_]\w*)?", RegexOptions.Compiled)]
    private static partial Regex JsDocToken();

    /// <summary>Matches type identifiers and import expressions inside JSDoc type braces.</summary>
    [GeneratedRegex(@"(?<import>import\((?<path>'[^']*'|""[^""]*"")\))|(?<name>[A-Z][a-zA-Z0-9]*)", RegexOptions.Compiled)]
    private static partial Regex JsDocTypeToken();
}