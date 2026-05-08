using Hviktor.Models;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace ErrorSummary;

/// <summary>
/// Represents a custom Blazor component designed to be nested inside a parent
/// cascading component of type <see cref="Hviktor.Components.ErrorSummary.ErrorSummary"/>.
/// Inherits behavior from <see cref="NestedComponentBase{T}"/>.
/// </summary>
/// <inheritdoc cref="Hviktor.Components.Link.HyperLink" />
public partial class Link : NestedComponentBase<Hviktor.Components.ErrorSummary.ErrorSummary>
{
    /// <summary>
    /// Gets or sets the content to be rendered inside this component.
    /// This property allows you to define custom nested content using Razor syntax.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}