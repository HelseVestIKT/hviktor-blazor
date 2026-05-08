using System.Diagnostics.CodeAnalysis;
using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace Hviktor.Components.Loader;

/// <summary>
/// The <b>Loader</b> component is used to display a loading indicator on the page.<br/>
/// It supports customizable styles, classes, and positions, and can block page interaction while loading.
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
///       <b>modal</b>: <see cref="bool"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="false"/><br/>
///       <b>Allowed</b>: <see langword="true"/> | <see langword="false"/><br/>
///       <b>Description</b>: Whether the loader should be displayed as a modal, blocking interaction with the rest of the page.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>position</b>: <see cref="Position"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see cref="Position.Center"/><br/>
///       <b>Allowed</b>: <see cref="Position.Center"/> | <see cref="Position.Top"/> | <see cref="Position.Right"/> | <see cref="Position.Bottom"/> | <see cref="Position.Left"/><br/>
///       <b>Description</b>: The position of the loader on the page. Only applicable if <b>modal</b> is <see langword="true"/>.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>size</b>: <see cref="Size"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see cref="Size.Medium"/><br/>
///       <b>Allowed</b>: <see cref="Size.Small"/> | <see cref="Size.Medium"/> | <see cref="Size.Large"/><br/>
///       <b>Description</b>: The size of the loader.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>color</b>: <see cref="Color"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see cref="Color.Neutral"/><br/>
///       <b>Allowed</b>: <see cref="Color.Accent"/> | <see cref="Color.Neutral"/> | <see cref="Color.Brand1"/> | <see cref="Color.Brand2"/> | <see cref="Color.Brand3"/> | <see cref="Color.Success"/> | <see cref="Color.Info"/> | <see cref="Color.Warning"/> | <see cref="Color.Danger"/><br/>
///       <b>Description</b>: The color theme of the loader.
///     </description>
///   </item>
/// </list>
/// </parameters>
/// <use>
/// Use <c>Loader</c> when:
/// <list type="bullet">
///   <item>You need to indicate that the system is working on a task the user must wait for</item>
///   <item>Loading will take more than one second</item>
///   <item>Use <see cref="Loader"/> when you need to block content from being interacted with</item>
/// </list>
/// </use>
/// <avoid>
/// Avoid <c>Loader</c> when:
/// <list type="bullet">
///   <item>Use <see cref="Spinner"/> when you do not need to block content from being interacted with</item>
///   <item>Use <see cref="Skeleton"/> when loading takes less than one second, or when loading predictable content</item>
/// </list>
/// </avoid>
/// <guidelines>
/// </guidelines>
public partial class Loader : ComponentBase
{
    [Inject] private IPositionService PositionService { get; set; } = null!;
    [Inject] private ISizeService SizeService { get; set; } = null!;
    [Inject] private IColorService ColorService { get; set; } = null!;

    [Inject] private ILogger<Loader> Logger { get; set; } = null!;

    /// <summary>
    /// The content of the loader, typically a spinner or progress indicator.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private bool isModal;
    private Dialog.Dialog? DialogRef { get; set; }

    private Dictionary<string, object?>? preComputedAttributes;

    /// <summary>
    /// Captures all unmatched attributes and applies them to the root element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    [SuppressMessage("Performance", "CA1859:Use concrete types when possible for improved performance")]
    private IReadOnlyDictionary<string, object?> ComputeAttributes()
    {
        if (preComputedAttributes is not null)
        {
            return preComputedAttributes;
        }

        var builder = HtmlAttributeBuilder.ToDictionary(AdditionalAttributes);
        EnumValue<Position> position = builder.ConsumeAttribute("position") ?? builder.ConsumeAttribute("data-position");

        isModal = builder.ConsumeAttribute("modal") is not null and not "false";
        if (!isModal)
        {
            builder.AddClasses(["flex!", "justify-center", "items-center"]);

            if (!position.IsEmpty)
            {
                Logger.LogWarning("Position attribute is not supported for non-modal loaders.");
            }
        }
        else
        {
            builder.AddDataAttribute("position", PositionService.GetDataAttribute(position, Position.Start));
        }

        EnumValue<Color> color = builder.ConsumeAttribute("color") ?? builder.ConsumeAttribute("data-color");
        builder.AddDataAttribute("color", ColorService.GetDataAttribute(color, Color.Neutral));

        EnumValue<Size> size = builder.ConsumeAttribute("size") ?? builder.ConsumeAttribute("data-size");
        if (!size.IsEmpty)
        {
            builder.AddDataAttribute("size", SizeService.GetDataAttribute(size));
        }

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
    /// Closes the loader dialog.
    /// </summary>
    public void Close() => DialogRef?.Close();

    /// <summary>
    /// Opens the loader dialog.
    /// </summary>
    public void Open() => DialogRef?.ShowModal();
}