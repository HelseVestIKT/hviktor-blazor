namespace Hviktor.Abstractions.Interfaces.Localization;

/// <summary>
/// Provides methods for managing localization settings in the application.
/// </summary>
public interface ILocalizationService
{
    /// <summary>
    /// Sets the language for the application.
    /// </summary>
    /// <param name="culture">The culture code to set as the current language, e.g., "en-US", "fr-FR".</param>
    /// <returns> A task that represents the asynchronous operation.</returns>
    public Task SetLanguageAsync(string culture);

    /// <summary>
    /// Gets the current language of the application.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result should contain the current language code, e.g., "en-US", "fr-FR".</returns>
    public Task<string> GetLanguageAsync();

    /// <summary>
    /// Gets the current language culutre of the application.
    /// </summary>
    /// <returns>The current language code, e.g., "en-US", "fr-FR".</returns>
    public string GetLanguage();
}