using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

namespace Hviktor.Components.RequiredTag;

/// <summary>
/// <c>RequiredTag</c> is a tag that indicates that a field is required.
/// </summary>
/// <guidelines>
/// <h3>Required and optional form fields</h3>
/// To help users understand which fields must be completed, required fields must be clearly and consistently labelled.<br/>
/// There are several ways to mark required fields that <see href="https://www.uutilsynet.no/veiledning/skjema/38#ledetekster_og_instruksjoner">meet the requirements for labelling (uutilsynet.no)</see>.<br/>
/// Different contexts may require different approaches, but by keeping labelling as consistent as possible across our services, it becomes easier for users to understand and recognise the pattern.
/// <br/><br/>
/// A general guideline is to avoid asking for information we do not need and, as a result, to avoid optional fields where possible.<br/>
/// Also make sure there are not too many questions on the same page. Content grouped on the same page should share a common theme or include questions that are clearly related.
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
///       <b>required</b>: <see cref="bool"/><br/>
///       <i>(required)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="false"/><br/>
///       <b>Allowed</b>: <see langword="true"/> | <see langword="false"/><br/>
///       <b>Description</b>: Whether the field is required or optional.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>mode</b>: <see cref="string"/><br/>
///       <i>(required)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="default"/><br/>
///       <b>Allowed</b>: <see langword="default"/> | <see langword="all"/><br/>
///       <b>Description</b>: The mode of the tag. Leave empty for a standard required tag, or set to "all" to indicate that all fields are required.
///     </description>
///   </item>
/// </list>
/// </parameters>
public partial class RequiredTag : ComponentBase
{
    private bool required;
    private string mode = "default";

    private Dictionary<string, object?>? preComputedAttributes;

    /// <summary>
    /// A collection of additional HTML attributes that are passed to the rendered element.
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
            .AddClasses("ds-tag");

        var req = builder.GetValue("required");
        if (req is not null)
        {
            // fallback to true if set to any value (except "false")
            required = req.ToLower() switch
            {
                "false" => false,
                _ => true
            };
        }

        mode = builder.ConsumeAttribute("mode") ?? "default";

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