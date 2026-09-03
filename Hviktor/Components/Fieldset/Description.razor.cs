using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Models;
using Hviktor.Rendering;
using Hviktor.Security;
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

    private string id = null!;

    /// <summary>
    /// The ChildContent to render inside the Description.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        // Prefer an id supplied by the caller so consumers can predict the value.
        id = AdditionalAttributes is not null
             && AdditionalAttributes.TryGetValue("id", out var supplied)
             && supplied is string suppliedStr
             && !string.IsNullOrWhiteSpace(suppliedStr)
            ? suppliedStr
            : Cryptography.GenerateId();

        Parent!.RegisterDescription(id);
    }

    /// <inheritdoc/>
    protected override Dictionary<string, object?> ComputeAttributes()
    {
        var builder = HtmlAttributeBuilder.ToDictionary(base.ComputeAttributes());

        // Ensure the id is present on the rendered element so aria-describedby resolves correctly.
        builder.AddIdentity(id);

        EnumValue<Variant> variant = builder.ConsumeAttribute("variant") ?? builder.ConsumeAttribute("data-variant");
        builder.AddDataAttribute("variant", VariantService.GetDataAttribute(variant, Variant.Default));

        return builder;
    }
}