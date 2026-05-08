using System.Text.Encodings.Web;
using Hviktor.Components.Code.Highlighters;

namespace Hviktor.Components.Code;

/// <summary>
/// Pure C# syntax highlighter that produces HTML spans with CSS class names.
/// Covers the most common languages used in Blazor documentation without
/// requiring any JavaScript runtime dependency.
/// </summary>
internal static class SyntaxHighlighter
{
    /// <summary>
    /// Highlights <paramref name="code"/> for the given <paramref name="language"/>
    /// and returns an HTML string with <c>&lt;span class="codeblock-*"&gt;</c> wrappers.
    /// </summary>
    internal static string Highlight(string code, string language)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return string.Empty;
        }

        return language.ToLowerInvariant() switch
        {
            "csharp" or "cs" or "c#" => CSharpHighlighter.Highlight(code),
            "razor" or "cshtml" => RazorHighlighter.Highlight(code),
            "xml" or "html" or "svg" => XmlHighlighter.Highlight(code),
            "css" or "scss" => CssHighlighter.Highlight(code),
            "json" => JsonHighlighter.Highlight(code),
            "javascript" or "js" or "typescript" or "ts" => JavaScriptHighlighter.Highlight(code),
            "bash" or "shell" or "sh" => BashHighlighter.Highlight(code),
            "powershell" or "ps1" or "ps" or "pwsh" => PowerShellHighlighter.Highlight(code),
            "sql" => SqlHighlighter.Highlight(code),
            "yaml" or "yml" => YamlHighlighter.Highlight(code),
            _ => HtmlEncoder.Default.Encode(code),
        };
    }

    /// <summary>Maps a language alias to a CSS class name for per-language styling.</summary>
    internal static string GetLanguageCssClass(string language)
    {
        return language.ToLowerInvariant() switch
        {
            "csharp" or "cs" or "c#" => "lang-csharp",
            "razor" or "cshtml" => "lang-razor",
            "xml" or "html" or "svg" => "lang-xml",
            "css" or "scss" => "lang-css",
            "json" => "lang-json",
            "javascript" or "js" or "typescript" or "ts" => "lang-js",
            "bash" or "shell" or "sh" => "lang-bash",
            "sql" => "lang-sql",
            "yaml" or "yml" => "lang-yaml",
            "powershell" or "ps1" or "ps" => "lang-powershell",
            "markdown" or "md" => "lang-markdown",
            _ => "",
        };
    }

    /// <summary>Maps a language alias to a human-readable display name.</summary>
    internal static string GetLanguageDisplayName(string language)
    {
        return language.ToLowerInvariant() switch
        {
            "csharp" or "cs" or "c#" => "C#",
            "razor" or "cshtml" => "Razor",
            "xml" => "XML",
            "html" => "HTML",
            "svg" => "SVG",
            "css" => "CSS",
            "scss" => "SCSS",
            "json" => "JSON",
            "javascript" or "js" => "JavaScript",
            "typescript" or "ts" => "TypeScript",
            "bash" or "shell" or "sh" => "Bash",
            "sql" => "SQL",
            "yaml" or "yml" => "YAML",
            "powershell" or "ps1" or "ps" => "PowerShell",
            "markdown" or "md" => "Markdown",
            "plaintext" or "text" or "" => "Text",
            _ => language.ToUpperInvariant(),
        };
    }
}