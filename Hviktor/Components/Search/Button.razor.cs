using Hviktor.Models;
using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace Search;

/// <summary>
/// The Button component represents a submit button within a search form.
/// </summary>
public partial class Button : AsyncNestedComponentBase<Hviktor.Components.Search.Search>
{
    /// <summary>
    /// Represents the content to be rendered inside the Button component.
    /// This property is typically assigned a render fragment, which defines a section of UI to be rendered.
    /// The content can be arbitrary markup or other components, providing flexibility for customization.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Computes the attributes for the button component, including default attributes
    /// such as "type" and "aria-label". It also appends an auto-generated identity
    /// to ensure unique identification for the component.
    /// </summary>
    /// <returns>
    /// A dictionary containing the computed HTML attributes to be rendered for the component.
    /// </returns>
    protected override Dictionary<string, object?> ComputeAttributes()
        => HtmlAttributeBuilder.ToDictionary(base.ComputeAttributes())
            .AddIdentity(() => Parent is not null, $"{Parent?.Id}-submit")
            .AddAttribute("type", "submit")
            .AddAttribute("aria-label", "Søk");
}