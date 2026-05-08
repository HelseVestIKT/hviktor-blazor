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
    /// Hviktor services and logging.
    /// </summary>
    protected HviktorBunitContext()
    {
        Services.AddHviktor();
        Services.AddLogging();
    }
}

