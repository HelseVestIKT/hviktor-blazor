using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

namespace Hviktor.Components.Typography;

/// <summary>
/// <c>Heading</c> is used to structure content and create hierarchy on a page.
/// </summary>
/// <guidelines>
/// <c>Heading</c> is used to establish structure and hierarchy in content. Use headings to divide information into clear, logical sections.<br/>
/// Make sure to use <see href="https://designsystemet.no/en/components/docs/heading/accessibility#riktige-overskriftsniv%C3%A5er">correct heading levels</see> (h1-h6) to ensure proper semantic structure.
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
///       <b>level</b>: <see cref="int"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///         <b>Default</b>: <see langword="2"/><br/>
///         <b>Allowed</b>: <see langword="1"/> | <see langword="2"/> | <see langword="3"/> | <see langword="4"/> | <see langword="5"/> | <see langword="6"/><br/>
///         <b>Description</b>: Heading level. This will translate into any h1-6 level.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>size</b>: <see cref="Size"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///         <b>Allowed</b>: <see cref="Size.ExtraExtraSmall"/> | <see cref="Size.ExtraSmall"/> | <see cref="Size.Small"/> | <see cref="Size.Medium"/> | <see cref="Size.Large"/> | <see cref="Size.ExtraLarge"/> | <see cref="Size.ExtraExtraLarge"/><br/>
///         <b>Description</b>: Heading size.
///     </description>
///   </item>
/// </list>
/// </parameters>
public partial class Heading : ComponentBase
{
    [Inject] private ISizeService SizeService { get; set; } = null!;

    /// <summary>
    /// Specifies the content to be rendered inside the heading component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Represents the parent component's unique identifier, used to link or associate the heading with its parent context
    /// for semantic or structural purposes.
    /// </summary>
    [CascadingParameter]
    public string? ParentId { get; set; }

    private int InternalLevel { get; set; } = 2;

    /// <summary>
    /// Captures all unmatched attributes and applies them to the root element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object?>? AdditionalAttributes { get; set; }

    private Dictionary<string, object?>? preComputedAttributes;

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        preComputedAttributes = null;
        ComputeAttributes();
    }

    private Dictionary<string, object?> ComputeAttributes()
    {
        if (preComputedAttributes is not null)
        {
            return preComputedAttributes;
        }

        var builder = HtmlAttributeBuilder.ToDictionary(AdditionalAttributes)
            .AddClasses("ds-heading");

        InternalLevel = builder.ConsumeAttribute("level") switch
        {
            "1" => 1,
            "2" => 2,
            "3" => 3,
            "4" => 4,
            "5" => 5,
            "6" => 6,
            _ => InternalLevel
        };

        var id = builder.GetValueOrDefault("id");
        if (id is null)
        {
            builder.AddAttribute("id", ParentId);
        }

        EnumValue<Size> size = builder.ConsumeAttribute("size") ?? builder.ConsumeAttribute("data-size");
        if (!size.IsEmpty)
        {
            builder.AddDataAttribute("size", SizeService.GetDataAttribute(size));
        }

        preComputedAttributes = builder;
        return preComputedAttributes;
    }

    private RenderFragment BuildContent() => builder =>
    {
        var seq = 0;
        builder.OpenElement(seq++, $"h{InternalLevel}");
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