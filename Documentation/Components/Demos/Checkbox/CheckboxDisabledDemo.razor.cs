using Documentation.Resources;
using Hviktor.Abstractions.Interfaces.Localization;
using Microsoft.AspNetCore.Components;

namespace Documentation.Components.Demos.Checkbox;

public partial class CheckboxDisabledDemo : ComponentBase
{
    [Inject] protected IStringLocalizerService<DocumentationResources> Localizer { get; set; } = null!;
}
