using Bunit;
using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Localization;
using Hviktor.Abstractions.Interfaces.Services;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Tests.Unit.Extensions;

/// <summary>
/// Unit tests for <see cref="ServiceCollectionExtensions.AddHviktor"/>.
/// </summary>
[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
public class ServiceCollectionExtensionsTests : HviktorBunitContext
{
    public ServiceCollectionExtensionsTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    #region Service registration

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
    public void AddHviktor_RegistersColorService()
    {
        var provider = Services.BuildServiceProvider();
        Assert.NotNull(provider.GetService<IColorService>());
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
    public void AddHviktor_RegistersSizeService()
    {
        var provider = Services.BuildServiceProvider();
        Assert.NotNull(provider.GetService<ISizeService>());
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
    public void AddHviktor_RegistersVariantService()
    {
        var provider = Services.BuildServiceProvider();
        Assert.NotNull(provider.GetService<IVariantService>());
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
    public void AddHviktor_RegistersWeightService()
    {
        var provider = Services.BuildServiceProvider();
        Assert.NotNull(provider.GetService<IWeightService>());
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
    public void AddHviktor_RegistersWidthService()
    {
        var provider = Services.BuildServiceProvider();
        Assert.NotNull(provider.GetService<IWidthService>());
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
    public void AddHviktor_RegistersPositionService()
    {
        var provider = Services.BuildServiceProvider();
        Assert.NotNull(provider.GetService<IPositionService>());
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
    public void AddHviktor_RegistersPlacementService()
    {
        var provider = Services.BuildServiceProvider();
        Assert.NotNull(provider.GetService<IPlacementService>());
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
    public void AddHviktor_RegistersInputTypeService()
    {
        var provider = Services.BuildServiceProvider();
        Assert.NotNull(provider.GetService<IInputTypeService>());
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
    public void AddHviktor_RegistersComparisonService()
    {
        var provider = Services.BuildServiceProvider();
        Assert.NotNull(provider.GetService<IComparisonService>());
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
    public void AddHviktor_RegistersHtmlRenderer()
    {
        var provider = Services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        Assert.NotNull(scope.ServiceProvider.GetService<HtmlRenderer>());
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
    public void AddHviktor_RegistersLocalizationService()
    {
        var provider = Services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        Assert.NotNull(scope.ServiceProvider.GetService<ILocalizationService>());
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
    public void AddHviktor_RegistersStringLocalizerService()
    {
        var serviceType = typeof(IStringLocalizerService<>);
        Assert.Contains(Services, sd => sd.ServiceType == serviceType);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
    public void AddHviktor_RegistersJsRuntimeService()
    {
        var provider = Services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        Assert.NotNull(scope.ServiceProvider.GetService<IJsRuntimeService>());
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
    public void AddHviktor_RegistersJsObjectReferenceService()
    {
        var provider = Services.BuildServiceProvider();
        Assert.NotNull(provider.GetService<IJsObjectReferenceService>());
    }

    #endregion

    #region TryAdd semantics (does not override existing registrations)

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
    public void AddHviktor_DoesNotOverrideExistingColorService()
    {
        var services = new ServiceCollection();
        var expected = new TestColorService();
        services.AddSingleton<IColorService>(expected);
        services.AddHviktor();
        var provider = services.BuildServiceProvider();

        Assert.Same(expected, provider.GetService<IColorService>());
    }

    #endregion

    #region Return value

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
    public void AddHviktor_ReturnsServiceCollection()
    {
        var services = new ServiceCollection();
        var result = services.AddHviktor();

        Assert.Same(services, result);
    }

    #endregion

    #region Configure callback

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
    public void AddHviktor_InvokesConfigureCallback()
    {
        var services = new ServiceCollection();
        var invoked = false;

        services.AddHviktor(configure: _ => invoked = true);

        Assert.True(invoked);
    }

    #endregion

    #region RequestLocalization callback

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
    public void AddHviktor_ConfiguresRequestLocalizationOptions()
    {
        var services = new ServiceCollection();
        services.AddHviktor(requestLocalization: options => { options.SetDefaultCulture("nb-NO"); });
        var provider = services.BuildServiceProvider();

        var options = provider.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;

        Assert.Equal("nb-NO", options.DefaultRequestCulture.Culture.Name);
    }

    #endregion

    #region Resources callback

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
    public void AddHviktor_InvokesResourcesCallback()
    {
        var services = new ServiceCollection();
        var invoked = false;

        services.AddHviktor(resources: _ => invoked = true);

        Assert.True(invoked);
    }

    #endregion

    #region Singleton semantics

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
    public void AddHviktor_SingletonServices_ReturnSameInstance()
    {
        var provider = Services.BuildServiceProvider();

        var first = provider.GetService<IColorService>();
        var second = provider.GetService<IColorService>();

        Assert.Same(first, second);
    }

    #endregion

    #region Test doubles

    private sealed class TestColorService : IColorService
    {
        public string GetDataAttribute(EnumValue<Color> value) => "test";
        public string GetDataAttribute(EnumValue<Color> value, Color defaultValue) => "test";
        public Color GetFromString(string value) => default;
        public Color GetFromString(string value, Color defaultValue) => default;
    }

    #endregion
}