using System.Reflection;
using System.Resources;
using Bunit;
using Hviktor.Extensions.Localization;

namespace Tests.Unit.Extensions.Localization;

[Collection(TestCollections.ResourceOverrides)]
[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
public class ResourceOverrideManagerTests : BunitContext
{
    private readonly ResourceOverrideManager sut = new();

    public ResourceOverrideManagerTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        ClearOverrides();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        ClearOverrides();
    }

    private static void ClearOverrides()
    {
        var field = typeof(ResourceExtensions).GetField("OverrideManagers", BindingFlags.NonPublic | BindingFlags.Static);
        if (field?.GetValue(null) is IList<ResourceManager> list)
        {
            list.Clear();
        }
    }

    #region RegisterOverride (baseName, assembly)

    [Fact]
    public void RegisterOverride_WithBaseNameAndAssembly_ReturnsSameInstance()
    {
        var result = sut.RegisterOverride("SomeBaseName", Assembly.GetExecutingAssembly());
        Assert.Same(sut, result);
    }

    [Fact]
    public void RegisterOverride_WithBaseNameAndAssembly_RegistersOverride()
    {
        sut.RegisterOverride("SomeBaseName", Assembly.GetExecutingAssembly());
        Assert.True(ResourceExtensions.HasOverrides);
    }

    #endregion

    #region RegisterOverride (Type)

    [Fact]
    public void RegisterOverride_WithType_ReturnsSameInstance()
    {
        var result = sut.RegisterOverride(typeof(ResourceOverrideManagerTests));
        Assert.Same(sut, result);
    }

    [Fact]
    public void RegisterOverride_WithType_RegistersOverride()
    {
        sut.RegisterOverride(typeof(ResourceOverrideManagerTests));
        Assert.True(ResourceExtensions.HasOverrides);
    }

    #endregion

    #region RegisterOverride<T>

    [Fact]
    public void RegisterOverrideGeneric_ReturnsSameInstance()
    {
        var result = sut.RegisterOverride<ResourceOverrideManagerTests>();
        Assert.Same(sut, result);
    }

    [Fact]
    public void RegisterOverrideGeneric_RegistersOverride()
    {
        sut.RegisterOverride<ResourceOverrideManagerTests>();
        Assert.True(ResourceExtensions.HasOverrides);
    }

    #endregion

    #region Fluent Chaining

    [Fact]
    public void RegisterOverride_SupportsFluentChaining()
    {
        var result = sut
            .RegisterOverride("First", Assembly.GetExecutingAssembly())
            .RegisterOverride(typeof(ResourceOverrideManagerTests))
            .RegisterOverride<ResourceOverrideManagerTests>();

        Assert.Same(sut, result);
        Assert.True(ResourceExtensions.HasOverrides);
    }

    #endregion
}