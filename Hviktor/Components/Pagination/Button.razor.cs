using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Models;
using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace Pagination;

/// <summary>
/// Represents a <see cref="Button"/> component that is used within a pagination context.
/// </summary>
/// <parameters>
/// <para><c>Pagination.Button</c> attributes</para>
/// <list type="table">
///   <listheader>
///     <term>Attribute</term>
///     <description>Description</description>
///   </listheader>
///   <item>
///     <term>
///       <b>variant</b>: <see cref="Variant"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see cref="Variant.Tertiary"/><br/>
///       <b>Allowed</b>: <see cref="Variant.Primary"/> | <see cref="Variant.Secondary"/> | <see cref="Variant.Tertiary"/><br/>
///       <b>Description</b>: Visual variant of the <c>Button</c>.
///                     <b>Note:</b> If <c>aria-current</c> is set to <see langword="true"/>, the variant will be set to <see cref="Variant.Primary"/>.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>aria-current</b>: <see cref="bool"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="false"/><br/>
///       <b>Allowed</b>: <see langword="true"/> | <see langword="false"/><br/>
///       <b>Description</b>: Indicates that the button represents the current page.
///     </description>
///   </item>
/// </list>
/// </parameters>
/// <inheritdoc cref="Hviktor.Components.Button.Button"/>
public partial class Button : NestedComponentBase<Hviktor.Components.Pagination.Pagination>
{
    [Inject] private IVariantService VariantService { get; set; } = null!;

    /// <summary>
    /// The ChildContent to render inside the Button.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <inheritdoc/>
    protected override Dictionary<string, object?> ComputeAttributes()
        => HtmlAttributeBuilder.ToDictionary(base.ComputeAttributes())
            .AddClasses("ds-button")
            .AddAttribute("type", "button")
            .Transform(attributes =>
            {
                EnumValue<Variant> variant = attributes.ConsumeAttribute("variant") ?? attributes.ConsumeAttribute("data-variant");
                var ariaCurrent = attributes.GetValue("aria-current");

                var fallbackVariant = ariaCurrent is not null ? Variant.Primary : Variant.Tertiary;
                attributes.AddDataAttribute("variant", VariantService.GetDataAttribute(variant, fallbackVariant));

                return attributes;
            });
}