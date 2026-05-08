using Hviktor.Models;
using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;
using FieldsetLegend = Fieldset.Legend;

namespace Hviktor.Components.Fieldset;

/// <summary>
/// <c>Fieldset</c> is used to group and label fields that naturally belong together, such as date fields or address fields.<br/>
/// The component helps organize information, make forms more manageable, and improve accessibility for screen readers.
/// </summary>
/// <remarks>
/// It is common to get confused between <c>Fieldset</c> and <c>Field</c>.<br/>
/// A good rule of thumb is that Fieldset is a set of Field
/// </remarks>
/// <use>
/// Use <c>Fieldset.Description</c> when:
/// <list type="bullet">
///   <item>You have a group or list of multiple <see cref="Radio"/> or <see cref="Checkbox"/> components that naturally belong together.</item>
///   <item>A form needs to be divided into sections with explanatory headings.</item>
///   <item>Several fields naturally belong together such as contact information.</item>
/// </list>
/// </use>
/// <avoid>
/// Avoid <c>Fieldset.Description</c> when:
/// <list type="bullet">
///   <item>You are grouping elements that are not part of a form.</item>
///   <item>The fields do not share a common purpose.</item>
/// </list>
/// </avoid>
/// <guidelines>
/// When using <see cref="Fieldset"/>, start with a <see cref="FieldsetLegend">Fieldset.Legend</see> that explains what the fields below relate to.
/// This may be a question such as “Where do you currently live?” or a descriptive phrase like “Personal details”.
/// This helps users understand the relationship between the fields and what they need to fill in.
/// </guidelines>
public partial class Fieldset : CascadingComponentBase
{
    /// <summary>
    /// Content rendered inside the fieldset.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <inheritdoc/>
    protected override Dictionary<string, object?> ComputeAttributes()
    {
        return HtmlAttributeBuilder.ToDictionary(base.ComputeAttributes())
            .AddClasses("ds-fieldset");
    }
}