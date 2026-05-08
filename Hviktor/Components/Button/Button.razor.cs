using System.Diagnostics.CodeAnalysis;
using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Rendering;
using Hviktor.Security;
using Microsoft.AspNetCore.Components;

namespace Hviktor.Components.Button;

/// <summary>
/// The <c>Button</c> component is a clickable button element that can be used to trigger actions or events.
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
///       <b>Default</b>: <see cref="Variant.Primary"/><br/>
///       <b>Allowed</b>: <see cref="Variant.Primary"/> | <see cref="Variant.Secondary"/> | <see cref="Variant.Tertiary"/><br/>
///       <b>Description</b>: Visual variant of the <c>Button</c> component.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>color</b>: <see cref="Color"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Allowed</b>: <see cref="Color.Accent"/> | <see cref="Color.Neutral"/> | <see cref="Color.Brand1"/> | <see cref="Color.Brand2"/> | <see cref="Color.Brand3"/> | <see cref="Color.Info"/> | <see cref="Color.Success"/> | <see cref="Color.Warning"/> | <see cref="Color.Danger"/><br/>
///       <b>Description</b>: Color theme of the <c>Button</c> component.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>size</b>: <see cref="Size"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Allowed</b>: <see cref="Size.Small"/> | <see cref="Size.Medium"/> | <see cref="Size.Large"/><br/>
///       <b>Description</b>: <c>Button</c> size.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>icon</b>: <see cref="bool"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Allowed</b>: <see langword="true"/> | <see langword="false"/><br/>
///       <b>Description</b>: <c>Button</c> with icon-only styling. The icon should be passed as the child content of the button.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>command</b>: <see cref="string"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: Command to be executed when the button is clicked.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>commandfor</b>: <see cref="string"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: Specifies the id of the element that the command should be executed for.
///                     This allows the button to trigger commands on other elements, such as a form submission when the button is outside of the form element.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>type</b>: <see cref="string"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="button"/><br/>
///       <b>Allowed</b>: <see langword="button"/> | <see langword="submit"/> | <see langword="reset"/><br/>
///       <b>Description</b>: <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Reference/Elements/button#type">HTML button type (MDN)</see>.
///     </description>
///   </item>
/// </list>
/// </parameters>
public partial class Button : ComponentBase
{
    [Inject] private IColorService ColorService { get; set; } = null!;
    [Inject] private IVariantService VariantService { get; set; } = null!;
    [Inject] private ISizeService SizeService { get; set; } = null!;

    /// <summary>
    /// The ChildContent to render inside the Button.
    /// </summary>
    /// <example>
    /// <code>
    /// &lt;Button&gt;
    ///     &lt;span&gt;Some content here&lt;/span&gt;
    /// &lt;/Button&gt;
    /// </code>
    /// </example>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Toggle loading state.
    /// </summary>
    [Parameter]
    public bool Loading { get; set; }

    /// <summary>
    /// Reference to the button element in the DOM.
    /// </summary>
    [DisallowNull]
    public ElementReference? ElementRef { get; private set; }

    /// <summary>
    /// Captures all unmatched attributes and applies them to the root element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    private string LabelledById { get; set; } = string.Empty;

    private Dictionary<string, object?> ComputeAttributes()
    {
        var builder = HtmlAttributeBuilder.ToDictionary(AdditionalAttributes)
            .AddIdentity(Cryptography.GenerateId())
            .AddClasses("ds-button");

        if (builder.TryGetValue("id", out var idObj) && idObj is not null)
        {
            LabelledById = idObj.ToString() ?? string.Empty;
        }

        if (Loading)
        {
            builder.AddAttribute("aria-disabled", "true");
            builder.AddAttribute("aria-busy", "true");
        }

        EnumValue<Variant> variant = builder.ConsumeAttribute("variant") ?? builder.ConsumeAttribute("data-variant");
        builder.AddDataAttribute("variant", VariantService.GetDataAttribute(variant, Variant.Primary));

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