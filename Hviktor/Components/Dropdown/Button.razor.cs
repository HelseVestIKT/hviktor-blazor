using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Models;
using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace Dropdown;

/// <summary>
/// <c>Dropdown.Button</c> is used to trigger actions or events when clicked, while maintaining the visual style of a dropdown button.
/// </summary>
/// <inheritdoc cref="Hviktor.Components.Button.Button" />
public partial class Button : AsyncNestedComponentBase<Hviktor.Components.Dropdown.Dropdown>
{
    [Inject] private IColorService ColorService { get; set; } = null!;
    [Inject] private IVariantService VariantService { get; set; } = null!;
    [Inject] private ISizeService SizeService { get; set; } = null!;

    /// <summary>
    /// Content rendered inside the dropdown item.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <inheritdoc/>
    protected override Dictionary<string, object?> ComputeAttributes()
    {
        var builder = HtmlAttributeBuilder.ToDictionary(base.ComputeAttributes());

        EnumValue<Variant> variant = builder.ConsumeAttribute("variant") ?? builder.ConsumeAttribute("data-variant");
        builder.AddDataAttribute("variant", VariantService.GetDataAttribute(variant, Variant.Tertiary));

        EnumValue<Size> size = builder.ConsumeAttribute("size") ?? builder.ConsumeAttribute("data-size");
        if (!size.IsEmpty)
        {
            builder.AddDataAttribute("size", SizeService.GetDataAttribute(size));
        }

        EnumValue<Color> color = builder.ConsumeAttribute("color") ?? builder.ConsumeAttribute("data-color");
        if (!color.IsEmpty)
        {
            builder.AddDataAttribute("color", ColorService.GetDataAttribute(color));
        }

        var icon = builder.ConsumeAttribute("icon");
        if (icon is not null)
        {
            builder.AddDataAttribute("icon", true);
        }

        return builder;
    }
}