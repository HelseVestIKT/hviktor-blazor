using Documentation.Resources;
using Hviktor.Abstractions.Interfaces.Localization;
using Hviktor.Security;
using Microsoft.AspNetCore.Components;

namespace Documentation.Components.Demos;

public partial class DropdownDemo : ComponentBase
{
    [Inject] protected IStringLocalizerService<DocumentationResources> Localizer { get; set; } = null!;

    private bool? dropdownOpen = false;

    private readonly string dropdownId = Cryptography.GenerateId();

    /// <summary>Column name key → whether the column is currently visible.</summary>
    private Dictionary<string, bool> Columns { get; } = new()
    {
        ["Name"] = true,
        ["Email"] = true,
        ["Role"] = true,
        ["Status"] = false,
        ["Department"] = false,
    };

    private int SelectedColumnCount => Columns.Count(c => c.Value);
}