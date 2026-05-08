using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace Dialog;

/// <summary>
/// Use multiple <c>Dialog.Block</c> if you want to divide the dialog with dividers into, for example, a top and bottom area.<br/><br/>
/// <b>Note</b> that content cannot be placed directly in <c>Dialog</c> if you use <c>Dialog.Block</c>; then all content should be inside one of the dialog's <c>Dialog.Block</c> sections.
/// </summary>
public sealed partial class Block : ComponentBase
{
    private Dictionary<string, object?>? preComputedAttributes;

    /// <summary>
    /// The content to be displayed inside the dialog block.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Captures all unmatched attributes and applies them to the root element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    private Dictionary<string, object?> ComputeAttributes()
    {
        if (preComputedAttributes is not null)
        {
            return preComputedAttributes;
        }

        var builder = HtmlAttributeBuilder.ToDictionary(AdditionalAttributes)
            .AddClasses("ds-dialog__block");

        preComputedAttributes = builder;
        return preComputedAttributes;
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        preComputedAttributes = null;
        ComputeAttributes();
    }
}