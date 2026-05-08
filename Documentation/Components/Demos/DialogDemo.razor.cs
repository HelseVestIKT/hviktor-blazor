using Documentation.Resources;
using Hviktor.Abstractions.Interfaces.Localization;
using Hviktor.Security;
using Microsoft.AspNetCore.Components;

namespace Documentation.Components.Demos;

public partial class DialogDemo : ComponentBase
{
    [Inject] protected IStringLocalizerService<DocumentationResources> Localizer { get; set; } = null!;
    private Hviktor.Components.Dialog.Dialog DialogRef { get; set; } = null!;
    private readonly string dialogId = $"dialog-{Cryptography.GenerateId()}";
}