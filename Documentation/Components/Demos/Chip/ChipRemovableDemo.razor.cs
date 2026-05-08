using Documentation.Resources;
using Hviktor.Abstractions.Interfaces.Localization;
using Microsoft.AspNetCore.Components;

namespace Documentation.Components.Demos.Chip;

public partial class ChipRemovableDemo : ComponentBase
{
    [Inject] protected IStringLocalizerService<DocumentationResources> Localizer { get; set; } = null!;

    protected List<string> removableChipFilters = ["Filter 1", "Filter 2", "Filter 3"];

    protected void RemoveChipFilter(string filter)
    {
        removableChipFilters.Remove(filter);
    }

    protected void ResetChipFilters()
    {
        removableChipFilters = ["Filter 1", "Filter 2", "Filter 3"];
    }
}
