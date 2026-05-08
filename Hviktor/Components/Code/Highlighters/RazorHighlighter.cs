using System.Text;
using System.Text.RegularExpressions;

namespace Hviktor.Components.Code.Highlighters;

/// <summary>
/// Syntax highlighting for Razor code, combining C# and markup highlighting
/// with context-aware method and field recognition.
/// </summary>
internal static partial class RazorHighlighter
{
    /// <summary>Highlights Razor source code.</summary>
    internal static string Highlight(string code)
    {
        var context = BuildContext(code);
        var sb = new StringBuilder(code.Length * 2);
        var pos = 0;

        foreach (Match match in CodeBlock().Matches(code))
        {
            if (match.Index > pos)
            {
                sb.Append(HighlightMarkup(code[pos..match.Index], context));
            }

            sb.Append(HighlighterBase.Encode("@"));
            sb.Append("<span class=\"codeblock-keyword\">code</span>");
            sb.Append(" {");
            sb.Append(CSharpHighlighter.Highlight(match.Groups["body"].Value, context));
            sb.Append('}');

            pos = match.Index + match.Length;
        }

        if (pos < code.Length)
        {
            sb.Append(HighlightMarkup(code[pos..], context));
        }

        return sb.ToString();
    }

    /// <summary>Builds a Razor context by extracting methods, fields, and variables from code blocks.</summary>
    private static RazorContext BuildContext(string code)
    {
        var methods = new HashSet<string>();
        var fields = new HashSet<string>();

        foreach (Match m in CodeBlock().Matches(code))
        {
            var body = m.Groups["body"].Value;
            foreach (Match method in MethodDefinition().Matches(body))
            {
                methods.Add(method.Groups["name"].Value);
            }

            foreach (Match field in FieldDefinition().Matches(body))
            {
                fields.Add(field.Groups["name"].Value);
            }

            foreach (Match prop in PropertyDefinition().Matches(body))
            {
                fields.Add(prop.Groups["name"].Value);
            }
        }

        foreach (Match m in RefBinding().Matches(code))
        {
            fields.Add(m.Groups["name"].Value);
        }

        foreach (Match m in MarkupForeachVariable().Matches(code))
        {
            fields.Add(m.Groups["name"].Value);
        }

        return new RazorContext(methods, fields);
    }

    /// <summary>Regex pattern and CSS class pairs for Razor markup highlighting.</summary>
    internal static readonly (Regex Pattern, string CssClass)[] MarkupPatterns =
    [
        (XmlHighlighter.Comment(), "comment"),
        (XmlHighlighter.CData(), "string"),
        (AttributeValue(), "string"),
        (ComponentTagName(), "type"),
        (XmlHighlighter.TagName(), "tag"),
        (AttributeDirective(), "attr"),
        (XmlHighlighter.AttributeName(), "attr"),
        (UsingDirective(), "razor-using"),
        (ControlFlowStatement(), "razor-control"),
        (StructuralDirective(), "keyword"),
        (ImplicitExpression(), "razor-implicit"),
    ];

    /// <summary>
    /// Highlights the markup portion of Razor code, handling inline <c>@(...)</c>
    /// expressions and <c>@</c>-prefixed attribute values.
    /// </summary>
    private static string HighlightMarkup(string code, RazorContext context)
    {
        var tokens = HighlighterBase.CollectTokens(code, MarkupPatterns);

        foreach (Match match in InlineExpression().Matches(code))
        {
            if (match.Length > 0)
            {
                tokens.Add((match.Index, match.Length, "razor-expr"));
            }
        }

        var filtered = HighlighterBase.SortAndFilter(tokens);

        var sb = new StringBuilder(code.Length * 2);
        var pos = 0;
        foreach ((var start, var length, var cssClass) in filtered)
        {
            if (start > pos)
            {
                sb.Append(HighlighterBase.Encode(code[pos..start]));
            }

            var text = code.Substring(start, length);

            switch (cssClass)
            {
                case "string" when text.Contains('@'):
                    HighlightAttributeValue(sb, text, context);
                    break;
                case "razor-expr":
                    sb.Append(HighlighterBase.Encode("@("));
                    sb.Append(CSharpHighlighter.Highlight(text[2..^1], context));
                    sb.Append(HighlighterBase.Encode(")"));
                    break;
                case "razor-using":
                    sb.Append(HighlighterBase.Encode("@"));
                    HighlighterBase.AppendSpan(sb, "keyword", "using");
                    sb.Append(HighlighterBase.Encode(" "));
                    HighlighterBase.AppendSpan(sb, "type", text[7..]); // skip "@using "
                    break;
                case "razor-control":
                    HighlightControlFlow(sb, text, context);
                    break;
                case "keyword" when text.StartsWith('@'):
                case "attr" when text.StartsWith('@'):
                    sb.Append(HighlighterBase.Encode("@"));
                    HighlighterBase.AppendSpan(sb, cssClass, text[1..]);
                    break;
                case "razor-implicit":
                    sb.Append(HighlighterBase.Encode("@"));
                    HighlightImplicitExpression(sb, text[1..], context);
                    break;
                default:
                    HighlighterBase.AppendSpan(sb, cssClass, text);
                    break;
            }

            pos = start + length;
        }

        if (pos < code.Length)
        {
            sb.Append(HighlighterBase.Encode(code[pos..]));
        }

        return sb.ToString();
    }

    /// <summary>
    /// Highlights a Razor attribute value like <c>"@IconSet.Captions"</c>,
    /// rendering the C# expression within using the standard C# highlighter.
    /// </summary>
    private static void HighlightAttributeValue(StringBuilder sb, string value, RazorContext context)
    {
        var atIndex = value.IndexOf('@');
        var prefix = value[..atIndex];
        var closingQuote = value[^1] == '"' ? "\"" : "";
        var expression = closingQuote.Length > 0
            ? value[(atIndex + 1)..^1]
            : value[(atIndex + 1)..];

        HighlighterBase.AppendSpan(sb, "string", prefix);
        sb.Append(HighlighterBase.Encode("@"));
        sb.Append(HighlightCSharpExpression(expression, context));

        if (closingQuote.Length > 0)
        {
            HighlighterBase.AppendSpan(sb, "string", closingQuote);
        }
    }

    /// <summary>
    /// Highlights a C# expression with additional context from Razor <c>@code</c> blocks,
    /// recognizing known methods and fields.
    /// </summary>
    private static string HighlightCSharpExpression(string code, RazorContext context)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return string.Empty;
        }

        if (SimpleIdentifier().IsMatch(code))
        {
            if (context.Methods.Contains(code))
            {
                return $"<span class=\"codeblock-function\">{HighlighterBase.Encode(code)}</span>";
            }

            if (context.Fields.Contains(code))
            {
                return $"<span class=\"codeblock-field\">{HighlighterBase.Encode(code)}</span>";
            }
        }

        return CSharpHighlighter.Highlight(code, context);
    }

    /// <summary>
    /// Highlights an implicit Razor expression (after the <c>@</c>), detecting control flow
    /// keywords like <c>foreach</c>, <c>if</c>, etc. and rendering them as keywords.
    /// </summary>
    private static void HighlightImplicitExpression(StringBuilder sb, string expression, RazorContext context)
    {
        var match = ControlFlowKeyword().Match(expression);
        if (match is { Success: true, Index: 0 })
        {
            HighlighterBase.AppendSpan(sb, "keyword", match.Value);
            sb.Append(CSharpHighlighter.Highlight(expression[match.Length..], context));
        }
        else
        {
            sb.Append(CSharpHighlighter.Highlight(expression, context));
        }
    }

    /// <summary>
    /// Highlights a Razor control flow statement like <c>@foreach (var person in list)</c>,
    /// rendering the keyword and parenthesized C# expression separately.
    /// </summary>
    private static void HighlightControlFlow(StringBuilder sb, string text, RazorContext context)
    {
        var match = ControlFlowKeyword().Match(text[1..]); // skip @
        if (!match.Success)
        {
            sb.Append(HighlighterBase.Encode(text));
            return;
        }

        sb.Append(HighlighterBase.Encode("@"));
        HighlighterBase.AppendSpan(sb, "keyword", match.Value);
        var rest = text[(1 + match.Length)..];

        // Find the parenthesized expression
        var parenStart = rest.IndexOf('(');
        if (parenStart >= 0)
        {
            sb.Append(HighlighterBase.Encode(rest[..parenStart]));
            sb.Append(HighlighterBase.Encode("("));
            sb.Append(CSharpHighlighter.Highlight(rest[(parenStart + 1)..^1], context));
            sb.Append(HighlighterBase.Encode(")"));
        }
        else
        {
            sb.Append(HighlighterBase.Encode(rest));
        }
    }

    /// <summary>Matches Razor attribute values, handling nested @(...) expressions with inner string literals.</summary>
    [GeneratedRegex(@"""(?:[^""@]*(?:@\((?:[^()""]*(?:""(?:\\.|[^""\\])*""[^()""]*)*|\((?:[^()""]*(?:""(?:\\.|[^""\\])*""[^()""]*)*|\((?:[^()""]*(?:""(?:\\.|[^""\\])*""[^()""]*)*)*\))*\))*\)[^""@]*)*|[^""]*)""", RegexOptions.Compiled)]
    private static partial Regex AttributeValue();

    /// <summary>Matches Razor component tag names (PascalCase, with optional dot-notation like Table.Row).</summary>
    [GeneratedRegex(@"</?[A-Z][\w]*(?:\.[\w]+)*", RegexOptions.Compiled)]
    private static partial Regex ComponentTagName();

    /// <summary>Matches Razor attribute directives: event handlers, bind, ref, key (used as attributes).</summary>
    [GeneratedRegex(@"@(?:on\w+(?::[\w.]+)?|bind(?:[-:]\w+)?|ref|key)(?=\s*=|\b)", RegexOptions.Compiled)]
    private static partial Regex AttributeDirective();

    /// <summary>Matches Razor structural directives: page, using, inject, code, control flow, etc. (used as keywords).</summary>
    [GeneratedRegex(@"@(?:using|code|page|inject|implements|inherits|layout|namespace|attribute|typeparam|section|rendermode|preservewhitespace|foreach|for|while|do|if|else|switch|case|lock|try|catch|finally)\b", RegexOptions.Compiled)]
    private static partial Regex StructuralDirective();

    /// <summary>Matches a full Razor @using directive including the namespace path.</summary>
    [GeneratedRegex(@"@using\s+[\w.]+", RegexOptions.Compiled)]
    private static partial Regex UsingDirective();

    [GeneratedRegex(@"@[a-zA-Z_]\w*(?:\.\w+)*(?:\((?:[^()]*|\((?:[^()]*|\([^()]*\))*\))*\))?", RegexOptions.Compiled)]
    private static partial Regex ImplicitExpression();

    [GeneratedRegex(@"@code\s*\{(?<body>(?:[^{}]|\{(?:[^{}]|\{(?:[^{}]|\{(?:[^{}]|\{[^{}]*\})*\})*\})*\})*)\}", RegexOptions.Compiled)]
    private static partial Regex CodeBlock();

    [GeneratedRegex(@"@\((?:[^()""]*(?:""(?:\\.|[^""\\])*""[^()""]*)*|\((?:[^()""]*(?:""(?:\\.|[^""\\])*""[^()""]*)*|\((?:[^()""]*(?:""(?:\\.|[^""\\])*""[^()""]*)*|\([^()]*\))*\))*\))*\)", RegexOptions.Compiled)]
    private static partial Regex InlineExpression();

    [GeneratedRegex(@"(?:void|Task|bool|int|string|float|double|decimal|long|char|byte|short|object|var|\w+(?:<[^>]+>)?)\s+(?<name>\w+)\s*\(", RegexOptions.Compiled)]
    private static partial Regex MethodDefinition();

    [GeneratedRegex(@"(?:private|protected|internal|public)\s+(?:(?:static|const|readonly|volatile|new|required)\s+)*(?:bool|int|string|float|double|decimal|long|char|byte|short|object|var|\w+(?:<[^>]+>)?)\s+(?<name>\w+)\s*[;={]", RegexOptions.Compiled)]
    private static partial Regex FieldDefinition();

    [GeneratedRegex(@"@ref\s*=\s*""@?(?<name>\w+)""", RegexOptions.Compiled)]
    private static partial Regex RefBinding();

    [GeneratedRegex(@"^\w+$", RegexOptions.Compiled)]
    private static partial Regex SimpleIdentifier();

    /// <summary>Matches property definitions inside classes (e.g., "int Id { get; init; }").</summary>
    [GeneratedRegex(@"(?:(?:internal|public|protected|private)\s+)?(?:bool|int|string|float|double|decimal|long|char|byte|short|object|\w+(?:<[^>]+>)?)\??\s+(?<name>\w+)\s*\{(?:\s*get\s*;)", RegexOptions.Compiled)]
    private static partial Regex PropertyDefinition();

    /// <summary>Matches loop variables in Razor @foreach markup expressions (e.g., "var person in").</summary>
    [GeneratedRegex(@"@foreach\s*\(\s*(?:var|\w+)\s+(?<name>\w+)\s+in\b", RegexOptions.Compiled)]
    private static partial Regex MarkupForeachVariable();

    /// <summary>Matches Razor control flow keywords at the start of an implicit expression.</summary>
    [GeneratedRegex(@"^(?:foreach|for|while|do|if|else|switch|case|lock|try|catch|finally)\b", RegexOptions.Compiled)]
    private static partial Regex ControlFlowKeyword();

    /// <summary>Matches Razor control flow statements with parenthesized expressions (e.g., "@foreach (var x in list)").</summary>
    [GeneratedRegex(@"@(?:foreach|for|while|if|switch|lock|catch)\s*\((?:[^()]*|\((?:[^()]*|\([^()]*\))*\))*\)", RegexOptions.Compiled)]
    private static partial Regex ControlFlowStatement();
}