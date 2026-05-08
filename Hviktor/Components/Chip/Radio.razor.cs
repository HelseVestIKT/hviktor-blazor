using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace Chip;

/// <summary>
/// <c>Chip.Radio</c> are small, interactive components that allow users to control how they want to see content.<br/>
/// For example, they can be used to filter categories in a search result and show which filters are active.
/// </summary>
/// <remarks>
/// The Radio component is used to render a styled radio button that can handle user interactions
/// and manage its state.
/// </remarks>
/// <example>
/// This component allows setting parameters to manage its default state, handle changes, and provide custom attributes.
/// </example>
/// <parameters>
/// <para>Additional attributes</para>
/// <list type="table">
///   <listheader>
///     <term>Attribute</term>
///     <description>Description</description>
///   </listheader>
///   <item>
///     <term>
///       <b>checked</b>: <see cref="bool"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="false"/><br/>
///       <b>Allowed</b>: <see langword="true"/> | <see langword="false"/><br/>
///       <b>Description</b>: Whether the <see cref="Chip"/> is checked or not.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>name</b>: <see cref="string"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: Used to identify the <see cref="Hviktor.Components.Chip.Chip">Chip</see>.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>value</b>: <see cref="string"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: Represent the data associated with the <see cref="Hviktor.Components.Chip.Chip">Chip</see>.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>@onchange</b>: <see cref="EventCallback"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: Event callback that is triggered when the checked state of the <see cref="Hviktor.Components.Chip.Chip">Chip</see> changes.
///     </description>
///   </item>
/// </list>
/// </parameters>
public partial class Radio : Hviktor.Components.Chip.Chip
{
    private Dictionary<string, object?>? preComputedAttributes;

    /// <summary>
    /// Gets or sets a callback that updates the bound checked value.
    /// </summary>
    [Parameter]
    public EventCallback<bool> CheckedChanged { get; set; }

    private bool? isChecked;

    private string? internalName;
    private string? internalValue;

    /// <inheritdoc/>
    protected override Dictionary<string, object?> ComputeAttributes()
    {
        if (preComputedAttributes is not null)
        {
            return preComputedAttributes;
        }

        var builder = HtmlAttributeBuilder.ToDictionary(base.ComputeAttributes());

        internalName = builder.ConsumeAttribute("name");
        internalValue = builder.ConsumeAttribute("value");

        // Consume the checked attribute and set the isChecked property so we can apply it to the input element.
        var checkedValue = builder.ConsumeAttribute("checked");
        isChecked ??= checkedValue switch
        {
            not null when bool.TryParse(checkedValue, out var parsed) => parsed,
            _ => null
        };

        preComputedAttributes = builder;
        return preComputedAttributes;
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        preComputedAttributes = null;
        ComputeAttributes();
    }

    /// <summary>
    /// Computes and returns a collection of attributes for the input element, including classes, type, name, value, and event bindings.
    /// </summary>
    /// <returns>
    /// A dictionary representing the attributes to be applied to the input element.
    /// </returns>
    protected Dictionary<string, object?> ComputeInputAttributes()
    {
        var builder = HtmlAttributeBuilder.ToDictionary()
            .AddClasses("ds-input")
            .AddAttribute("name", internalName)
            .AddAttribute("value", internalValue)
            .AddAttribute("checked", isChecked is true ? true : null); // The checked attribute we consumed in ComputeAttributes()

        if (CheckedChanged.HasDelegate)
        {
            if (builder.ContainsKey("onchange"))
            {
                throw new InvalidOperationException("The 'onchange' event is already defined. Please remove it to use the CheckedChanged callback.");
            }

            builder.AddAttribute("onchange", EventCallback.Factory.Create<ChangeEventArgs>(this, OnCheckedChangeAsync));
        }
        else if (!builder.ContainsKey("onchange"))
        {
            // If no onchange handler is defined, we still want to update the isChecked state when the input changes.
            builder.AddAttribute("onchange", EventCallback.Factory.Create<ChangeEventArgs>(this, OnCheckedChangeAsync));
        }

        return builder;
    }

    private async Task OnCheckedChangeAsync(ChangeEventArgs args)
    {
        isChecked = args.Value is true or "true" or "True";
        await CheckedChanged.InvokeAsync(isChecked.Value);
    }
}