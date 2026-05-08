using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Models;
using Hviktor.Rendering;
using Hviktor.Security;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace ToggleGroup;

/// <summary>
/// Represents a toggle group item component that is a child of an asynchronous toggle group component.
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
///       <b>value</b>: <see cref="string"/>? | <see cref="string"/>[]?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///         <b>Description</b>: The value of the ToggleGroupItem. Generates a random value if not set.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>type</b>: <see cref="InputType"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///         <b>Default</b>: <see cref="InputType.Radio"/><br/>
///         <b>Allowed</b>: <see cref="InputType.Radio"/> | <see cref="InputType.Checkbox"/><br/>
///         <b>Description</b>: The type of the <c>ToggleGroup.Item</c>.<br/>
///                         <b>Note</b>: The type should not be set manually.
///     </description>
///   </item>
/// </list>
/// </parameters>
public partial class Item : AsyncNestedComponentBase<Hviktor.Components.ToggleGroup.ToggleGroup>
{
    [Inject] private IVariantService VariantService { get; set; } = null!;
    [Inject] private IInputTypeService InputTypeService { get; set; } = null!;

    private Dictionary<string, object?>? preComputedAttributes;

    /// <summary>
    /// The HTML Content to render inside the <see cref="Item"/> component.
    /// </summary>
    [Parameter]
    public string Id { get; set; } = Cryptography.GenerateId();

    /// <summary>
    /// The HTML Content to render inside the <see cref="Item"/> component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private string? innerValue;

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        preComputedAttributes = null;
        ComputeAttributes();
    }

    /// <inheritdoc/>
    protected override Dictionary<string, object?> ComputeAttributes()
    {
        if (preComputedAttributes is not null)
        {
            return preComputedAttributes;
        }

        var builder = HtmlAttributeBuilder.ToDictionary(base.ComputeAttributes())
            .AddIdentity(Id)
            .AddAttribute("role", "radio")
            .AddAttribute("name", Parent?.Name)
            .AddDataAttribute("roving-tabindex-item", true)
            .RemoveFromTabOrder();

        var classes = builder.ConsumeAttribute("class");
        if (!string.IsNullOrWhiteSpace(classes))
        {
            labelAttributes.AddClasses(classes);
        }

        innerValue = builder.GetValue("value");
        var isChecked = innerValue is not null && Parent?.InternalValue == innerValue;
        builder.AddAttribute("checked", isChecked);
        builder.AddAttribute("aria-checked", isChecked ? "true" : "false");

        EnumValue<InputType> inputType = builder.ConsumeAttribute("type") ?? builder.ConsumeAttribute("data-type");
        builder.AddAttribute("type", InputTypeService.GetDataAttribute(inputType, InputType.Radio));

        preComputedAttributes = builder;
        return preComputedAttributes;
    }

    private readonly Dictionary<string, object?> labelAttributes = new();

    private Dictionary<string, object?> ComputeLabelAttributes()
        => HtmlAttributeBuilder.ToDictionary(labelAttributes)
            .AddClasses("ds-button")
            .Transform((dict) =>
            {
                if (innerValue is not null && Parent?.InternalValue == innerValue)
                {
                    dict.AddDataAttribute("variant", VariantService.GetDataAttribute(Variant.Primary));
                    return dict;
                }

                dict.AddDataAttribute("variant", VariantService.GetDataAttribute(Variant.Tertiary));
                return dict;
            });

    private async Task OnChangeEventAsync()
    {
        if (Parent is null || string.IsNullOrWhiteSpace(innerValue))
        {
            return;
        }

        await Parent.OnClickEventAsync(innerValue);
    }
}