using Documentation.Components.Services;
using Hviktor.Abstractions.Interfaces.Localization;
using Hviktor.Icons.Abstractions.Types;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Documentation.Components.Layout;

public partial class Navbar : ComponentBase, IAsyncDisposable
{
    [Inject] private NavigationManager Navigation { get; set; } = null!;
    [Inject] private IThemeService ThemeService { get; set; } = null!;
    [Inject] private ComponentSearchService SearchService { get; set; } = null!;
    [Inject] private IJsRuntimeService JsRuntimeService { get; set; } = null!;

    private Hviktor.Components.Dialog.Dialog searchDialogRef = null!;
    private CancellationTokenSource? searchCts;
    private bool isSearching;
    private List<SearchResult>? cachedResults;
    private DotNetObjectReference<Navbar>? dotNetRef;
    private IJSObjectReference? navbarModule;

    private const string Light = "light";

    private string ColorScheme => ThemeService.CurrentScheme;

    private async Task ToggleColorScheme() => await ThemeService.ToggleAsync();

    private IconDefinition GetIcon() => ColorScheme == Light ? DocumentationIconSet.Sun : DocumentationIconSet.Moon;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await ThemeService.InitializeAsync();
            dotNetRef = DotNetObjectReference.Create(this);
            navbarModule = await JsRuntimeService.ImportAsync<Navbar>();
            await navbarModule.InvokeVoidAsync("registerSearchShortcut", dotNetRef);
            StateHasChanged();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    /// <summary>Called from JavaScript when Ctrl+K is pressed.</summary>
    [JSInvokable]
    public async Task OpenSearchDialog()
    {
        await searchDialogRef.ShowModal();
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        try
        {
            if (navbarModule is not null)
            {
                await navbarModule.InvokeVoidAsync("unregisterSearchShortcut");
                await navbarModule.DisposeAsync();
            }
        }
        catch (JSDisconnectedException)
        {
            // Circuit disconnected
        }

        dotNetRef?.Dispose();
        GC.SuppressFinalize(this);
    }

    private string SearchFilter => SearchService.Filter;

    /// <summary>Handles search input changes and updates the shared search service with debouncing.</summary>
    private async Task OnSearchInput(ChangeEventArgs e)
    {
        SearchService.Filter = e.Value?.ToString() ?? string.Empty;

        if (searchCts is not null)
        {
            await searchCts.CancelAsync();
            searchCts.Dispose();
        }

        searchCts = new CancellationTokenSource();
        var token = searchCts.Token;

        if (string.IsNullOrWhiteSpace(SearchService.Filter))
        {
            isSearching = false;
            cachedResults = null;
            return;
        }

        isSearching = true;
        cachedResults = null;
        StateHasChanged();

        try
        {
            await Task.Delay(250, token);
            if (token.IsCancellationRequested) return;

            cachedResults = ComputeFilteredItems().ToList();
            isSearching = false;
            StateHasChanged();
        }
        catch (TaskCanceledException)
        {
            // Debounce
        }
    }

    /// <summary>Clears the current search filter.</summary>
    private void ClearSearch()
    {
        searchCts?.Cancel();
        isSearching = false;
        cachedResults = null;
        SearchService.Clear();
    }

    private void SetSearchFilter(string filter)
    {
        searchCts?.Cancel();
        SearchService.Filter = filter;
        cachedResults = ComputeFilteredItems().ToList();
        isSearching = false;
    }

    [Inject] private ComponentRegistry Registry { get; set; } = null!;

    /// <summary>Returns components filtered by the current search term, expanded into per-page results.</summary>
    private IEnumerable<SearchResult> FilteredItems => cachedResults ?? ComputeFilteredItems();

    /// <summary>Computes the filtered and scored search results.</summary>
    private IEnumerable<SearchResult> ComputeFilteredItems()
    {
        var filter = SearchService.Filter;
        var items = Registry.Groups.SelectMany(g => g.Items);

        if (!string.IsNullOrWhiteSpace(filter))
        {
            return items
                .Select(c => (Component: c, Score: GetMatchScore(c, filter)))
                .Where(x => x.Score > 0)
                .OrderByDescending(x => x.Score)
                .ThenBy(x => x.Component.Title, StringComparer.OrdinalIgnoreCase)
                .SelectMany(x => ExpandToSearchResults(x.Component));
        }

        return items.SelectMany(ExpandToSearchResults);
    }

    /// <summary>Expands a component into search results for overview, code, and optionally accessibility.</summary>
    private static IEnumerable<SearchResult> ExpandToSearchResults(ComponentInfo component)
    {
        yield return new SearchResult(component, "overview");
        yield return new SearchResult(component, "code");

        if (component.AccessibilityLink is not null)
        {
            yield return new SearchResult(component, "accessibility");
        }
    }

    /// <summary>Represents a single search result entry for a component page.</summary>
    /// <param name="Component">The source component.</param>
    /// <param name="Page">The page type (overview, code, or accessibility).</param>
    private sealed record SearchResult(ComponentInfo Component, string Page)
    {
        /// <summary>Display title combining the component name and page type.</summary>
        public string Title => $"{Component.Title} - {Page}";

        /// <summary>Route path for this result.</summary>
        public string Href => $"components/{Component.Slug}/{Page}";
    }

    /// <summary>Returns a relevance score for a component against the search filter. Zero means no match.</summary>
    private static int GetMatchScore(ComponentInfo component, string filter)
    {
        const StringComparison c = StringComparison.OrdinalIgnoreCase;

        var score = GetTextMatchScore(component.Title, filter, c, exactBonus: 100, startsWithBonus: 80, containsBonus: 60)
                    + GetTextMatchScore(component.Slug, filter, c, exactBonus: 90, startsWithBonus: 70, containsBonus: 50)
                    + GetContainsScore(component.Description, filter, c, 30)
                    + GetKeywordScore(component, filter, c)
                    + GetDocumentationScore(component.Documentation, filter, c);

        return score;
    }

    /// <summary>Scores a text field using exact, starts-with, and contains matching.</summary>
    private static int GetTextMatchScore(string text, string filter, StringComparison c, int exactBonus, int startsWithBonus, int containsBonus)
    {
        if (text.Equals(filter, c))
        {
            return exactBonus;
        }

        if (text.StartsWith(filter, c))
        {
            return startsWithBonus;
        }

        return text.Contains(filter, c) ? containsBonus : 0;
    }

    /// <summary>Returns the bonus if the text contains the filter.</summary>
    private static int GetContainsScore(string? text, string filter, StringComparison c, int bonus)
        => text?.Contains(filter, c) == true ? bonus : 0;

    /// <summary>Scores status keyword matches.</summary>
    private static int GetKeywordScore(ComponentInfo component, string filter, StringComparison c)
    {
        const int bonus = 20;
        var score = 0;

        if (component.IsExperimental && "experimental".Contains(filter, c))
        {
            score += bonus;
        }

        if (component.IsDeprecated && "deprecated".Contains(filter, c))
        {
            score += bonus;
        }

        if (component.IsValidated && "validated".Contains(filter, c))
        {
            score += bonus;
        }

        if (component.AccessibilityLink is not null && "accessibility".Contains(filter, c))
        {
            score += bonus;
        }

        return score;
    }

    /// <summary>Scores matches within documentation content fields.</summary>
    private static int GetDocumentationScore(ClassDocumentation? docs, string filter, StringComparison c)
    {
        if (docs is null)
        {
            return 0;
        }

        const int bonus = 10;
        var score = 0;

        if (docs.Remarks?.Contains(filter, c) == true)
        {
            score += bonus;
        }

        if (docs.Use?.Contains(filter, c) == true)
        {
            score += bonus;
        }

        if (docs.Avoid?.Contains(filter, c) == true)
        {
            score += bonus;
        }

        if (docs.Guidelines?.Contains(filter, c) == true)
        {
            score += bonus;
        }

        return score;
    }
}