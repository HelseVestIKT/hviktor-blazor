using Hviktor.Abstractions.Interfaces;
using Hviktor.Abstractions.Interfaces.Localization;
using Hviktor.Abstractions.Interfaces.Services;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Extensions.Localization;
using Hviktor.Extensions.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Hviktor.Extensions;

/// <summary>
/// Provides dependency injection extension methods for registering Hviktor services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Hviktor core services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configure">Optional action to configure additional services.</param>
    /// <param name="requestLocalization">Optional action to configure RequestLocalizationOptions.</param>
    /// <param name="resources">Optional action to register resource overrides using ResourceOverrideManager instance.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddHviktor(
        this IServiceCollection services,
        Action<IServiceCollection>? configure = null,
        Action<RequestLocalizationOptions>? requestLocalization = null,
        Action<ResourceOverrideManager>? resources = null)
    {
        // Register core and extension services
        services.TryAddSingleton<IColorService, ColorService>();
        services.TryAddSingleton<ISizeService, SizeService>();
        services.TryAddSingleton<IVariantService, VariantService>();
        services.TryAddSingleton<IWeightService, WeightService>();
        services.TryAddSingleton<IWidthService, WidthService>();
        services.TryAddSingleton<IPositionService, PositionService>();
        services.TryAddSingleton<IPlacementService, PlacementService>();
        services.TryAddSingleton<IInputTypeService, InputTypeService>();

        services.TryAddSingleton<IComparisonService, ComparisonService>();

        // Register resource overrides before configuring services
        resources?.Invoke(new ResourceOverrideManager());
        if (requestLocalization is not null)
        {
            services.Configure<RequestLocalizationOptions>(requestLocalization.Invoke);
        }

        // Register HTML renderer (used for rendering components to HTML strings)
        services.AddScoped<HtmlRenderer>();

        // Register the ILocalizationService implementation
        services.TryAddScoped<ILocalizationService, LocalizationExtensions>();
        services.TryAddSingleton(typeof(IStringLocalizerService<>), typeof(StringLocalizerService<>));

        // Register localization infrastructure
        services.AddLocalization();

        services.TryAddScoped<IJsRuntimeService, JsRuntimeService>();
        services.TryAddSingleton<IJsObjectReferenceService, JsObjectReferenceService>();

        // Allow user to register/override services if needed
        configure?.Invoke(services);

        return services;
    }
}