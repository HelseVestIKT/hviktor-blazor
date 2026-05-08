using Documentation.Resources;
using Hviktor.Abstractions.Interfaces.Localization;
using Microsoft.AspNetCore.Components;

namespace Documentation.Components.Demos.Input;

public partial class InputStatesDemo : ComponentBase
{
    [Inject] protected IStringLocalizerService<DocumentationResources> Localizer { get; set; } = null!;
}