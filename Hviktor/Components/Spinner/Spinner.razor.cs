using System.Diagnostics.CodeAnalysis;
using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Rendering;
using Hviktor.Security;
using Microsoft.AspNetCore.Components;

namespace Hviktor.Components.Spinner;

/// <summary>
/// The <c>Spinner</c> component is used to indicate that content or an action is loading, and that the user must wait before they can continue.
/// </summary>
/// <use>
/// Use <c>Spinner</c> when:
/// <list type="bullet">
/// <item>You need to indicate that the system is working on a task the user must wait for</item>
/// <item>A local action is being performed, such as submitting a form or updating a table</item>
/// <item>Loading will take more than one second</item>
/// </list>
/// </use>
/// <avoid>
/// Avoid <c>Spinner</c> when:
/// <list type="bullet">
/// <item>Only specific parts of the page are loading — consider using Skeleton instead</item>
/// <item>Loading takes less than one second</item>
/// </list>
/// </avoid>
/// <guidelines>
/// The purpose of <c>Spinner</c> is to reassure the user that the system is working and has not stopped.
/// It can be used in a button, a field, or another defined area where the user is waiting for something to happen, such as saving or updating content.
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
///       <b>size</b>: <see cref="Size"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Allowed</b>: <see cref="Size.Small"/> | <see cref="Size.Medium"/> | <see cref="Size.Large"/><br/>
///       <b>Description</b>: <c>Spinner</c> size.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>color</b>: <see cref="Color"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Allowed</b>: <see cref="Color.Accent"/> | <see cref="Color.Neutral"/> | <see cref="Color.Info"/> | <see cref="Color.Success"/> | <see cref="Color.Warning"/> | <see cref="Color.Danger"/><br/>
///       <b>Description</b>: Color theme.
///     </description>
///   </item>
/// </list>
/// </parameters>
public partial class Spinner : ComponentBase
{
    [Inject] private ISizeService SizeService { get; set; } = null!;
    [Inject] private IColorService ColorService { get; set; } = null!;

    /// <summary>
    /// Accessible title for the spinner SVG. Provides alternative text for screen readers.
    /// </summary>
    [Parameter]
    public string Title { get; set; } = "Laster...";

    /// <summary>
    /// Captures all unmatched attributes and applies them to the root element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    private string TitleId { get; } = Cryptography.GenerateId();

    private bool ShowTitle { get; set; }

    [SuppressMessage("Performance", "CA1859:Use concrete types when possible for improved performance")]
    private IReadOnlyDictionary<string, object?> ComputeAttributes()
    {
        var builder = HtmlAttributeBuilder.ToDictionary(AdditionalAttributes)
            .AddClasses("ds-spinner")
            .AddAttribute("role", "img");

        var hasExternalLabel = builder.ContainsKey("aria-label")
                               || builder.ContainsKey("aria-labelledby")
                               || builder.ContainsKey("aria-hidden");

        // Only add the default <title> + aria-labelledby when no external labelling is provided
        ShowTitle = !hasExternalLabel;
        if (ShowTitle)
        {
            builder.AddAttribute("aria-labelledby", TitleId);
        }

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

        return builder;
    }
}