using System.Globalization;
using Bunit;
using Hviktor.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Unit;

/// <summary>
/// Base context for all Hviktor bUnit tests.
/// Registers shared services so individual test classes don't repeat boilerplate.
/// </summary>
public abstract class HviktorBunitContext : BunitContext
{
    /// <summary>
    /// Initializes a new instance of <see cref="HviktorBunitContext"/> and registers
    /// localization, Hviktor services and logging.
    /// </summary>
    protected HviktorBunitContext()
    {
        var culture = CultureInfo.GetCultureInfo("en");
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
        Services.AddLocalization();

        Services.AddHviktor();
        Services.AddLogging();
    }
}