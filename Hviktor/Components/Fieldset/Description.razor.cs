using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Components.Checkbox;
using Hviktor.Components.Radio;
using Hviktor.Models;
using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace Fieldset;

/// <summary>
/// The description of the fieldset.
/// </summary>
/// <parameters>
/// <para>Additional attributes</para>
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
///         <b>Default</b>: <see cref="Variant.Default"/><br/>
///         <b>Allowed</b>: <see cref="Variant.Long"/> | <see cref="Variant.Default"/> | <see cref="Variant.Short"/><br/>
///         <b>Description</b>: Adjusts styling for paragraph length
///     </description>
///   </item>
/// </list>
/// </parameters>
public partial class Description : NestedComponentBase<Hviktor.Components.Fieldset.Fieldset>
{
    [Inject] private IVariantService VariantService { get; set; } = null!;

    /// <summary>
    /// The ChildContent to render inside the Description.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <inheritdoc/>
    protected override Dictionary<string, object?> ComputeAttributes()
    {
        var builder = HtmlAttributeBuilder.ToDictionary(base.ComputeAttributes());

        EnumValue<Variant> variant = builder.ConsumeAttribute("variant") ?? builder.ConsumeAttribute("data-variant");
        builder.AddDataAttribute("variant", VariantService.GetDataAttribute(variant, Variant.Default));

        return builder;
    }
}