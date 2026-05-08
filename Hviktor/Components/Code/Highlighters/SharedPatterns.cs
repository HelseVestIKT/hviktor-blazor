using System.Text.RegularExpressions;

namespace Hviktor.Components.Code.Highlighters;

/// <summary>
/// Regex patterns shared across multiple language highlighters.
/// </summary>
internal static partial class SharedPatterns
{
    /// <summary>Matches number literals with optional decimal, exponent, and type suffix.</summary>
    [GeneratedRegex(@"\b\d+(?:\.\d+)?(?:[eE][+-]?\d+)?[fFdDmMlLuU]?\b", RegexOptions.Compiled)]
    internal static partial Regex NumberLiteral();

    /// <summary>Matches a method or function call (identifier followed by parenthesis).</summary>
    [GeneratedRegex(@"\b\w+(?=\s*\()", RegexOptions.Compiled)]
    internal static partial Regex MethodCall();

    /// <summary>Matches C-style multi-line comments.</summary>
    [GeneratedRegex(@"/\*[\s\S]*?\*/", RegexOptions.Compiled)]
    internal static partial Regex MultiLineComment();

    /// <summary>Matches C-style single-line comments.</summary>
    [GeneratedRegex(@"//.*$", RegexOptions.Multiline | RegexOptions.Compiled)]
    internal static partial Regex SingleLineComment();

    /// <summary>Matches double-quoted strings with escape sequences.</summary>
    [GeneratedRegex(@"""(?:\\.|[^""\\])*""", RegexOptions.Compiled)]
    internal static partial Regex DoubleQuotedString();
}

