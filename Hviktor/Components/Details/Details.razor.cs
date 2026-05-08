using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Models;
using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

namespace Hviktor.Components.Details;

/// <summary>
/// <c>Details</c> is a collapsible component that allows the user to show or hide content.
/// </summary>
/// <use>
/// Use <c>Details</c> when:
/// <list type="bullet">
///   <item>You want to provide information that is only relevant to some users</item>
///   <item>You want to give examples or answer frequently asked questions</item>
///   <item>iIt should be up to the user whether they choose to read the content</item>
/// </list>
/// </use>
/// <avoid>
/// Avoid using <c>Details</c> when:
/// <list type="bullet">
///   <item>You need to show important content that everyone should see</item>
///   <item>You need to display an action button</item>
///   <item>You need to summarise error messages, use <c>ErrorSummary</c> instead</item>
/// </list>
/// </avoid>
/// <guidelines>
/// Large amounts of text can be demanding for many users.
/// The <c>Details</c> component allows users to decide for themselves what is relevant to read.
/// However, you should not use <c>Details</c> to hide content just to make the page look tidier.
/// When content is hidden, there is a risk that users will not notice it at all.
/// Carefully consider whether the content really needs to be hidden, and be clear about why you are doing so.
/// </guidelines>
/// <parameters>
/// <para>Additional attributes</para>
/// <list type="table">
///   <listheader>
///     <term>Attribute</term>
///     <description>Description</description>
///   </listheader>
///   <item>
///     <term>
///       <b>open</b>: <see cref="bool"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="false"/><br/>
///       <b>Allowed</b>: <see langword="true"/> | <see langword="false"/><br/>
///       <b>Description</b>: Controls open-state. Using this removes automatic control of open-state.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>defaultOpen</b>: <see cref="bool"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="false"/><br/>
///       <b>Allowed</b>: <see langword="true"/> | <see langword="false"/><br/>
///       <b>Description</b>: Defaults the details to open if not controlled.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>variant</b>: <see cref="Variant"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see cref="Variant.Default"/><br/>
///       <b>Allowed</b>: <see cref="Variant.Default"/> | <see cref="Variant.Tinted"/><br/>
///       <b>Description</b>: Change the background color of the details.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>color</b>: <see cref="Color"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: Change the color of the details. Only applies to certain variants.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>size</b>: <see cref="Size"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: Change the size of the details. Only applies to certain variants.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>@ontoggle</b>: <see cref="EventCallback"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: Event callback for when the open state changes. Using this removes automatic control of open-state. The callback receives the new open state as a parameter.
///     </description>
///   </item>
/// </list>
/// </parameters>
public partial class Details : CascadingComponentBase
{
    [Inject] private IColorService ColorService { get; set; } = null!;
    [Inject] private ISizeService SizeService { get; set; } = null!;
    [Inject] private IVariantService VariantService { get; set; } = null!;

    /// <summary>
    /// Specifies the child content to be rendered inside the component.
    /// </summary>
    /// <remarks>
    /// This property allows you to define a fragment of UI content that can be provided
    /// by a parent component and displayed within this component.
    /// </remarks>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private bool internalOpen;

    /// <summary>
    /// Represents an event callback that is triggered when the open state changes.
    /// </summary>
    /// <remarks>
    /// This property allows consumers of the component to bind to or handle changes
    /// to the open state, enabling them to perform custom logic or update state based on
    /// the toggling of the component's visibility.
    /// </remarks>
    [Parameter]
    public EventCallback<bool> OpenChanged { get; set; }

    /// <summary>
    /// Computes the attributes for the component by integrating base attributes, adding CSS classes,
    /// data attributes, and event handlers relevant to the component's configuration.
    /// </summary>
    /// <returns>
    /// A read-only dictionary containing the computed attributes for the component.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if an invalid operation occurs during the attribute computation process.
    /// </exception>
    protected override Dictionary<string, object?> ComputeAttributes()
    {
        var builder = HtmlAttributeBuilder.ToDictionary(base.ComputeAttributes())
            .AddClasses("ds-details")
            .AddAttribute("role", "group");

        EnumValue<Variant> variant = builder.ConsumeAttribute("variant") ?? builder.ConsumeAttribute("data-variant");
        builder.AddDataAttribute("variant", VariantService.GetDataAttribute(variant, Variant.Default));

        EnumValue<Color> color = builder.ConsumeAttribute("color") ?? builder.ConsumeAttribute("data-color");
        if (!color.IsEmpty)
        {
            builder.AddDataAttribute("color", ColorService.GetDataAttribute(color));
        }

        EnumValue<Size> size = builder.ConsumeAttribute("size") ?? builder.ConsumeAttribute("data-size");
        if (!size.IsEmpty)
        {
            builder.AddDataAttribute("size", SizeService.GetDataAttribute(size));
        }

        if (OpenChanged.HasDelegate)
        {
            if (builder.ContainsKey("ontoggle"))
            {
                throw new InvalidOperationException("The 'ontoggle' event is already defined. Please remove it to use the OpenChanged callback.");
            }

            builder.AddAttribute("ontoggle", EventCallback.Factory.Create<EventArgs>(this, OnOpenChangedAsync));
        }
        else if (!builder.ContainsKey("ontoggle"))
        {
            // If no ontoggle handler is defined, we still want to update the isChecked state when the input changes.
            builder.AddAttribute("ontoggle", EventCallback.Factory.Create<EventArgs>(this, OnOpenChangedAsync));
        }

        return builder;
    }

    private async Task OnOpenChangedAsync(EventArgs e)
    {
        internalOpen = !internalOpen;
        await OpenChanged.InvokeAsync(internalOpen);
    }
}