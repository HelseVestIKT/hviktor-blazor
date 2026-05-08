namespace Documentation.Components.Services;

/// <summary>
/// Holds the full XML documentation extracted from a component class, including
/// custom tags such as <c>&lt;use&gt;</c>, <c>&lt;avoid&gt;</c>, and <c>&lt;guidelines&gt;</c>.
/// </summary>
/// <param name="Summary">The <c>&lt;summary&gt;</c> section, or <see langword="null"/>.</param>
/// <param name="Remarks">The <c>&lt;remarks&gt;</c> section, or <see langword="null"/>.</param>
/// <param name="Parameters">The <c>&lt;parameters&gt;</c> section describing the component parameters, or <see langword="null"/>.</param>
/// <param name="Use">The <c>&lt;use&gt;</c> section describing when to use the component, or <see langword="null"/>.</param>
/// <param name="Avoid">The <c>&lt;avoid&gt;</c> section describing when not to use the component, or <see langword="null"/>.</param>
/// <param name="Guidelines">The <c>&lt;guidelines&gt;</c> section with usage guidelines, or <see langword="null"/>.</param>
public sealed record ClassDocumentation(
    string? Summary,
    string? Parameters,
    string? Remarks,
    string? Use,
    string? Avoid,
    string? Guidelines,
    string? SummaryHtml = null,
    string? RemarksHtml = null,
    string? UseHtml = null,
    string? AvoidHtml = null,
    string? GuidelinesHtml = null);