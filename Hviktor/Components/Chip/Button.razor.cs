using Hviktor.Rendering;

// ReSharper disable once CheckNamespace
namespace Chip;

/// <summary>
/// <c>Chip.Button</c> is a variant of the <see cref="Hviktor.Components.Chip.Chip">Chip</see> component that behaves like a button element.<br/>
/// It can be used to trigger actions or events when clicked, while still maintaining the visual style of a chip.
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
/// </list>
/// </parameters>
public partial class Button : Hviktor.Components.Chip.Chip
{
    private Dictionary<string, object?>? preComputedAttributes;

    /// <inheritdoc/>
    protected override Dictionary<string, object?> ComputeAttributes()
    {
        if (preComputedAttributes is not null)
        {
            return preComputedAttributes;
        }

        var builder = HtmlAttributeBuilder.ToDictionary(base.ComputeAttributes())
            .AddAttribute("type", "button");

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