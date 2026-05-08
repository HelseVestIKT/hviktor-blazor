using System.Globalization;
using System.Reflection;
using System.Resources;
using Bunit;
using Hviktor.Extensions.Localization;

namespace Tests.Unit.Extensions.Localization;

[Collection(TestCollections.ResourceOverrides)]
[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
public class ResourceExtensionsTests : BunitContext
{
    private const string ResourcesPath = "Tests.Unit.Extensions.Localization.Resources";

    public ResourceExtensionsTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        // Clear overrides before each test by resetting via reflection
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

    #region HasOverrides

    [Fact]
    public void HasOverrides_WhenNoOverridesRegistered_ReturnsFalse()
    {
        Assert.False(ResourceExtensions.HasOverrides);
    }

    [Fact]
    public void HasOverrides_AfterRegister_ReturnsTrue()
    {
        ResourceExtensions.Register("SomeBaseName", Assembly.GetExecutingAssembly());
        Assert.True(ResourceExtensions.HasOverrides);
    }

    #endregion

    #region Register

    [Fact]
    public void Register_WithBaseNameAndAssembly_AddsOverride()
    {
        ResourceExtensions.Register("SomeBaseName", Assembly.GetExecutingAssembly());
        Assert.True(ResourceExtensions.HasOverrides);
    }

    [Fact]
    public void Register_MultipleTimes_AllAdded()
    {
        ResourceExtensions.Register("First", Assembly.GetExecutingAssembly());
        ResourceExtensions.Register("Second", Assembly.GetExecutingAssembly());
        Assert.True(ResourceExtensions.HasOverrides);
    }

    [Fact]
    public void Register_WithBaseNameOnly_UsesCallingAssembly()
    {
        ResourceExtensions.Register($"{ResourcesPath}.TestResources");
        var result = ResourceExtensions.GetString("TestKey", CultureInfo.InvariantCulture);
        Assert.Equal("TestValue", result);
    }

    [Fact]
    public void Register_InsertsAtFront_LatestRegistrationTakesPrecedence()
    {
        ResourceExtensions.Register($"{ResourcesPath}.TestResources", Assembly.GetExecutingAssembly());
        ResourceExtensions.Register($"{ResourcesPath}.TestResources2", Assembly.GetExecutingAssembly());

        var result = ResourceExtensions.GetString("SharedKey", CultureInfo.InvariantCulture);
        Assert.Equal("SecondValue", result);
    }

    [Fact]
    public void Register_EarlierRegistration_UsedAsFallback()
    {
        ResourceExtensions.Register($"{ResourcesPath}.TestResources", Assembly.GetExecutingAssembly());
        ResourceExtensions.Register($"{ResourcesPath}.TestResources2", Assembly.GetExecutingAssembly());

        var result = ResourceExtensions.GetString("TestKey", CultureInfo.InvariantCulture);
        Assert.Equal("TestValue", result);
    }

    [Fact]
    public void Register_WithAssembly_CreatesWorkingResourceManager()
    {
        ResourceExtensions.Register($"{ResourcesPath}.TestResources", Assembly.GetExecutingAssembly());
        var result = ResourceExtensions.GetString("TestKey", CultureInfo.InvariantCulture);
        Assert.Equal("TestValue", result);
    }

    #endregion

    #region GetString

    [Fact]
    public void GetString_WithNoOverrides_ReturnsNull()
    {
        var result = ResourceExtensions.GetString("NonExistentKey");
        Assert.Null(result);
    }

    [Fact]
    public void GetString_WithCulture_WithNoOverrides_ReturnsNull()
    {
        var result = ResourceExtensions.GetString("NonExistentKey", CultureInfo.InvariantCulture);
        Assert.Null(result);
    }

    [Fact]
    public void GetString_WithInvalidBaseName_ReturnsNull()
    {
        ResourceExtensions.Register("NonExistent.Resource.BaseName", Assembly.GetExecutingAssembly());
        var result = ResourceExtensions.GetString("SomeKey");
        Assert.Null(result);
    }

    [Fact]
    public void GetString_WithNonExistentKey_ReturnsNull()
    {
        ResourceExtensions.Register($"{ResourcesPath}.ResourceExtensionsTests", Assembly.GetExecutingAssembly());
        var result = ResourceExtensions.GetString("KeyThatDoesNotExist", CultureInfo.InvariantCulture);
        Assert.Null(result);
    }

    [Fact]
    public void GetString_CatchesArgumentNullException_ReturnsNull()
    {
        InjectManager(new ThrowingResourceManager<ArgumentNullException>());
        var result = ResourceExtensions.GetString("AnyKey", CultureInfo.InvariantCulture);
        Assert.Null(result);
    }

    [Fact]
    public void GetString_CatchesMissingManifestResourceException_ReturnsNull()
    {
        InjectManager(new ThrowingResourceManager<MissingManifestResourceException>());
        var result = ResourceExtensions.GetString("AnyKey", CultureInfo.InvariantCulture);
        Assert.Null(result);
    }

    [Fact]
    public void GetString_CatchesObjectDisposedException_ReturnsNull()
    {
        InjectManager(new ObjectDisposedResourceManager());
        var result = ResourceExtensions.GetString("AnyKey", CultureInfo.InvariantCulture);
        Assert.Null(result);
    }

    [Fact]
    public void GetString_SkipsFailingManager_ReturnsFallbackValue()
    {
        ResourceExtensions.Register($"{ResourcesPath}.TestResources", Assembly.GetExecutingAssembly());
        InjectManager(new ThrowingResourceManager<MissingManifestResourceException>());

        var result = ResourceExtensions.GetString("TestKey", CultureInfo.InvariantCulture);
        Assert.Equal("TestValue", result);
    }

    #endregion

    #region Helpers

    private static void InjectManager(ResourceManager manager)
    {
        var field = typeof(ResourceExtensions).GetField("OverrideManagers", BindingFlags.NonPublic | BindingFlags.Static);
        if (field?.GetValue(null) is IList<ResourceManager> list)
        {
            list.Insert(0, manager);
        }
    }

    /// <summary>
    /// A ResourceManager that throws a specified exception type from GetResourceSet.
    /// </summary>
    private sealed class ThrowingResourceManager<TException> : ResourceManager
        where TException : Exception, new()
    {
        public override ResourceSet GetResourceSet(CultureInfo culture, bool createIfNotExists, bool tryParents)
        {
            throw new TException();
        }
    }

    private sealed class ObjectDisposedResourceManager : ResourceManager
    {
        public override ResourceSet GetResourceSet(CultureInfo culture, bool createIfNotExists, bool tryParents)
        {
            throw new ObjectDisposedException("manager");
        }
    }

    #endregion

    #region Initial State

    [Fact]
    public void OverrideManagers_InitiallyEmpty()
    {
        Assert.False(ResourceExtensions.HasOverrides);
    }

    #endregion
}