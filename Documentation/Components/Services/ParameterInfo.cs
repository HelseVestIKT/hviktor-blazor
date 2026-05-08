namespace Documentation.Components.Services;

/// <summary>
/// Describes a single parameter on a Hviktor component.
/// </summary>
/// <param name="Name">The C# property name.</param>
/// <param name="TypeName">Human-readable type name (e.g. <c>EnumValue&lt;Color&gt;</c>).</param>
/// <param name="DefaultValue">Default value string, or <see langword="null"/> if none declared.</param>
/// <param name="AllowedValues">Allowed values when constrained by <see cref="System.ComponentModel.DataAnnotations.AllowedValuesAttribute"/>, or empty.</param>
/// <param name="IsRequired">Whether the parameter is marked <see cref="Microsoft.AspNetCore.Components.EditorRequiredAttribute"/>.</param>
/// <param name="XmlDocSummary">Summary from XML doc comments, or <see langword="null"/> if unavailable.</param>
/// <param name="XmlDocSummaryHtml">Summary from XML doc comments rendered as HTML, or <see langword="null"/> if unavailable.</param>
/// <param name="TypeHtml">The type name rendered as HTML with popovers for known types, or <see langword="null"/> to fall back to plain <c>TypeName</c>.</param>
public sealed record ParameterInfo(
    string Name,
    string TypeName,
    string? DefaultValue,
    IReadOnlyList<string> AllowedValues,
    bool IsRequired,
    string? XmlDocSummary,
    string? XmlDocSummaryHtml = null,
    string? TypeHtml = null
);