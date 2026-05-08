using Hviktor.Models;
using Hviktor.Rendering;
using Hviktor.Security;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace Field;

/// <summary>
/// The Description component is used to provide additional information or context about a form field.
/// </summary>
public partial class Description : NestedComponentBase<Hviktor.Components.Field.Field>
{
    private static int Counter { get; set; }

    /// <summary>
    /// The ChildContent to render inside the Description.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <inheritdoc/>
    protected override Dictionary<string, object?> ComputeAttributes() => HtmlAttributeBuilder.ToDictionary(base.ComputeAttributes())
        .AddIdentity((Parent?.Id ?? Cryptography.GenerateId()) + "-description:" + ++Counter)
        .AddDataAttribute("field", "description");
}