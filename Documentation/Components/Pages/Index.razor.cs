using Documentation.Components.Services;
using Documentation.Resources;
using Hviktor.Abstractions.Interfaces.Localization;
using Microsoft.AspNetCore.Components;

namespace Documentation.Components.Pages;

public sealed partial class Index : ComponentBase
{
    [Inject] private IStringLocalizerService<DocumentationResources> Localizer { get; set; } = null!;
    [Inject] private ComponentRegistry Registry { get; set; } = null!;
    [Inject] private IComponentMetadataService MetadataService { get; set; } = null!;

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        // Pre-warm metadata caches in background so component page navigations are instant
        await Task.Yield();
        foreach (var group in Registry.Groups)
        {
            foreach (var component in group.Items)
            {
                if (component.ComponentType is not { } t) continue;
                MetadataService.GetParameters(t);
                MetadataService.GetPublicMethods(t);

                if (component.SubComponents is not { Count: > 0 } subs) continue;
                foreach (var sub in subs)
                {
                    MetadataService.GetParameters(sub.ComponentType);
                    MetadataService.GetPublicMethods(sub.ComponentType);
                }
            }
        }
    }
}