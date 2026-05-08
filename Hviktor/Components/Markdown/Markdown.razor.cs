using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

namespace Hviktor.Components.Markdown;

/// <summary>
/// Renders a trusted Markdown string (from registry literals) as sanitized HTML.
/// </summary>
/// <remarks>
/// <para>Supported syntax:</para>
/// <list type="bullet">
///   <item><c>**bold**</c>, <c>*italic*</c>, <c>`code`</c>, <c>~~strikethrough~~</c>, <c>__underline__</c></item>
///   <item><c>[text](url)</c> - link (opens in new tab)</item>
///   <item><c># Heading</c> through <c>###### Heading</c></item>
///   <item><c>- item</c> / <c>* item</c> - unordered list</item>
///   <item><c>1. item</c> - ordered list</item>
///   <item><c>&gt; text</c> - blockquote</item>
///   <item><c>---</c> - horizontal rule</item>
///   <item>GFM pipe tables (<c>| col | col |</c> with a separator row)</item>
///   <item>Raw HTML blocks - lines starting with <c>&lt;</c> are passed through verbatim</item>
///   <item>Blank lines - paragraph breaks</item>
/// </list>
/// <para>Only use with controlled, developer-authored strings - never with user-supplied content.</para>
/// </remarks>
public sealed partial class Markdown : ComponentBase
{
    /// <summary>
    /// The Markdown string to render. Must be a trusted, developer-authored value.
    /// </summary>
    [Parameter, EditorRequired]
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Captures all unmatched attributes and applies them to the wrapper element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object?>? AdditionalAttributes { get; set; }

    /// <summary>
    /// Computes the merged HTML attributes, always including the <c>markdown</c> CSS class.
    /// </summary>
    private Dictionary<string, object?> ComputeAttributes()
        => HtmlAttributeBuilder.ToDictionary(AdditionalAttributes)
            .AddClasses("markdown");
}