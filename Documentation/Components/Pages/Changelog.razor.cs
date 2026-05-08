using Documentation.Components.Services;
using Microsoft.AspNetCore.Components;

namespace Documentation.Components.Pages;

/// <summary>
/// Changelog page.
/// </summary>
public sealed partial class Changelog : ComponentBase
{
    [Inject] private HttpClient Http { get; set; } = null!;

    private List<ChangelogRelease> releases = [];
    private bool isLoading = true;
    private string? errorMessage;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        try
        {
            var markdown = await Http.GetStringAsync("changelog.md");
            if (string.IsNullOrWhiteSpace(markdown))
            {
                errorMessage = "Changelog file was empty.";
                return;
            }

            // Strip BOM if present
            markdown = markdown.TrimStart('\uFEFF');
            releases = ChangelogParser.ParseReleases(markdown);
        }
        catch (HttpRequestException ex)
        {
            errorMessage = $"Could not load changelog: {ex.StatusCode} - {ex.Message}";
        }
        finally
        {
            isLoading = false;
        }
    }
}