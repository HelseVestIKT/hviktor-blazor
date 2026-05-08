using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Hviktor.Models;
using Hviktor.Rendering;
using Hviktor.Security;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace ErrorSummary;

/// <summary>
/// Represents a configurable heading component that renders an HTML heading element (h1, h2, etc.)
/// with customizable content and attributes. This component integrates with a parent
/// <see cref="Hviktor.Components.ErrorSummary.ErrorSummary"/> component, allowing for
/// accessibility enhancements and dynamic identification.
/// </summary>
/// <inheritdoc cref="Hviktor.Components.Typography.Heading" />
public partial class Heading : NestedComponentBase<Hviktor.Components.ErrorSummary.ErrorSummary>
{
    /// <summary>
    /// The unique identifier for the heading.<br/>
    /// This is used to associate the heading with its label.
    /// </summary>
    /// <remarks>If not provided, a unique identifier will be generated.</remarks>
    [Parameter, Required]
    public required string Id { get; set; } = Cryptography.GenerateId();

    /// <summary>
    /// The heading level. Valid values are 1 through 6, corresponding to HTML heading elements <c>&lt;h1&gt;</c> through <c>&lt;h6&gt;</c>.
    /// </summary>
    [Parameter]
    [AllowedValues(1, 2, 3, 4, 5, 6)]
    [DefaultValue(2)]
    public int Level { get; set; } = 2;

    /// <summary>
    /// The HTML Content to render inside the <see cref="Heading"/> component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <inheritdoc/>
    protected override Dictionary<string, object?> ComputeAttributes() => HtmlAttributeBuilder.ToDictionary(base.ComputeAttributes())
        .AddIdentity(Id)
        .AddClasses("ds-heading");

    /// <inheritdoc/>
    protected override void OnParametersSet()
    {
        Parent?.SetLabelledbyId(Id);
    }

    private RenderFragment BuildContent() => builder =>
    {
        var seq = 0;
        builder.OpenElement(seq++, $"h{Level}");
        foreach (var attribute in ComputeAttributes())
        {
            builder.AddAttribute(seq++, attribute.Key, attribute.Value);
        }

        if (ChildContent is not null)
        {
            builder.AddContent(seq, ChildContent);
        }

        builder.CloseElement();
    };
}