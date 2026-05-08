using Hviktor.Rendering;

// ReSharper disable once CheckNamespace
namespace Chip;

/// <summary>
/// The <c>Chip.Removable</c> class represents a specialized button component
/// that adds functionality for marking the button as "removable".
/// It extends the <see cref="Chip.Button"/> class and overrides specific behavior for attribute computation.
/// </summary>
// ReSharper disable once RedundantNameQualifier
public partial class Removable : Chip.Button
{
    /// <summary>
    /// Adds the data-removable attribute to the button.
    /// </summary>
    /// <returns>A dictionary representing the attributes to be applied to the button element, including the data-removable attribute.</returns>
    protected override Dictionary<string, object?> ComputeAttributes()
        => HtmlAttributeBuilder.ToDictionary(base.ComputeAttributes())
            .AddDataAttribute("removable", "true");
}