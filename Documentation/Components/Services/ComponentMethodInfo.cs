namespace Documentation.Components.Services;

/// <summary>
/// Describes a public method on a Hviktor component, typically used via <c>@ref</c>.
/// </summary>
/// <param name="Name">The method name.</param>
/// <param name="ReturnType">Human-readable return type name.</param>
/// <param name="ParameterSignature">Formatted parameter list (e.g. <c>string id, bool force</c>).</param>
/// <param name="XmlDocSummary">Summary from XML doc comments as Markdown, or <see langword="null"/> if unavailable.</param>
/// <param name="XmlDocSummaryHtml">Summary from XML doc comments rendered as HTML, or <see langword="null"/> if unavailable.</param>
/// <param name="ReturnTypeHtml">The return type rendered as HTML with popovers for known types, or <see langword="null"/> to fall back to plain <c>ReturnType</c>.</param>
/// <param name="ParameterSignatureHtml">The parameter signature rendered as HTML with popovers for known types, or <see langword="null"/> to fall back to plain <c>ParameterSignature</c>.</param>
public sealed record ComponentMethodInfo(
    string Name,
    string ReturnType,
    string ParameterSignature,
    string? XmlDocSummary,
    string? XmlDocSummaryHtml = null,
    string? ReturnTypeHtml = null,
    string? ParameterSignatureHtml = null);
