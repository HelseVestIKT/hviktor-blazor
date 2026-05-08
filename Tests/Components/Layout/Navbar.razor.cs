using Hviktor.Icons.Abstractions.Types;
using Hviktor.Icons.Types;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Tests.Components.Layout;

public partial class Navbar : ComponentBase
{
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;

    private const string Light = "light";
    private const string Dark = "dark";

    private string ColorScheme { get; set; } = Light;

    private async Task<string> GetColorScheme()
    {
        try
        {
            var scheme = "";
            var cookie = await JsRuntime.InvokeAsync<string>("eval", "document.cookie");
            if (!string.IsNullOrWhiteSpace(cookie) && cookie.Contains("color_scheme"))
            {
                // Parse cookie
                var parts = cookie.Split(';').Select(c => c.Trim()).ToArray();
                foreach (var part in parts)
                {
                    if (part.StartsWith("color_scheme"))
                    {
                        var kv = part.Split('=');
                        if (kv.Length == 2)
                        {
                            scheme = kv[1].Trim('\'', '"');
                        }
                    }
                }
            }

            return scheme;
        }
        catch (Exception)
        {
            // No-op
        }

        return Light;
    }

    private async Task SetColorScheme(string scheme)
    {
        try
        {
            await JsRuntime.InvokeVoidAsync("eval", $"document.body.setAttribute('data-color-scheme', '{scheme}');");
            await JsRuntime.InvokeVoidAsync("eval", $"document.cookie='color_scheme={scheme};'");
            ColorScheme = scheme;
            await InvokeAsync(StateHasChanged);
        }
        catch (Exception)
        {
            // No-op
        }
    }

    private async Task ToggleColorScheme()
    {
        try
        {
            var scheme = await GetColorScheme() == Dark ? Light : Dark;
            await SetColorScheme(scheme);
        }
        catch (Exception)
        {
            // No-op
        }
    }

    private IconDefinition GetIcon() => ColorScheme == Light ? IconSet.Sun : IconSet.Moon;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var scheme = await GetColorScheme();
            await SetColorScheme(scheme);
        }

        await base.OnAfterRenderAsync(firstRender);
    }
}