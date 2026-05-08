using System.Diagnostics;
using Hviktor.Security;

namespace Tests.Unit.Security;

/// <summary>
/// Performance tests for <see cref="Cryptography.GenerateId()"/> overloads.
/// Runs in the <see cref="TestCollections.Performance"/> collection to ensure
/// isolation from other CPU-intensive work.
/// </summary>
[Collection(TestCollections.Performance)]
[Trait(TestCollections.Traits.Collection, TestCollections.Performance)]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Identity)]
public class CryptographyPerformanceTests
{
    /// <summary>
    /// Number of warmup iterations to stabilize JIT and CPU caches
    /// before timing begins.
    /// </summary>
    private const int WarmupIterations = 1_000;

    private const int Iterations = 100_000;
    private const int MaxMilliseconds = 2_000;

    /// <summary>
    /// Warms up both <see cref="Cryptography.GenerateId()"/> overloads
    /// to eliminate JIT compilation and CPU cache cold-start noise.
    /// </summary>
    private static void WarmUp()
    {
        for (var i = 0; i < WarmupIterations; i++)
        {
            _ = Cryptography.GenerateId();
            _ = Cryptography.GenerateId(8);
        }
    }

    /// <summary>
    /// The default <see cref="Cryptography.GenerateId()"/> must complete
    /// 100 000 calls in under 2 000 ms on any CI/development machine.
    /// </summary>
    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Performance)]
    public void GenerateId_Default_PerformanceIsAcceptable()
    {
        WarmUp();

        var sw = Stopwatch.StartNew();
        for (var i = 0; i < Iterations; i++)
        {
            _ = Cryptography.GenerateId();
        }

        sw.Stop();

        Assert.True(
            sw.ElapsedMilliseconds < MaxMilliseconds,
            $"GenerateId() took {sw.ElapsedMilliseconds} ms for {Iterations:N0} calls (limit: {MaxMilliseconds} ms).");
    }

    /// <summary>
    /// The variable-length <see cref="Cryptography.GenerateId(int)"/> must complete
    /// 100 000 calls (length = 8) in under 2 000 ms on any CI/development machine.
    /// </summary>
    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Performance)]
    public void GenerateId_WithLength_PerformanceIsAcceptable()
    {
        WarmUp();

        var sw = Stopwatch.StartNew();
        for (var i = 0; i < Iterations; i++)
        {
            _ = Cryptography.GenerateId(8);
        }

        sw.Stop();

        Assert.True(
            sw.ElapsedMilliseconds < MaxMilliseconds,
            $"GenerateId(8) took {sw.ElapsedMilliseconds} ms for {Iterations:N0} calls (limit: {MaxMilliseconds} ms).");
    }

    /// <summary>
    /// The default overload must be faster than or comparable to the variable-length
    /// overload for the same 8-character output.
    /// </summary>
    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Performance)]
    public void GenerateId_DefaultOverload_IsFasterThanOrComparableToVariableLength()
    {
        WarmUp();

        var swDefault = Stopwatch.StartNew();
        for (var i = 0; i < Iterations; i++)
        {
            _ = Cryptography.GenerateId();
        }

        swDefault.Stop();

        var swVariable = Stopwatch.StartNew();
        for (var i = 0; i < Iterations; i++)
        {
            _ = Cryptography.GenerateId(8);
        }

        swVariable.Stop();

        Assert.True(swDefault.ElapsedTicks <= swVariable.ElapsedTicks * 2,
            $"Default GenerateId() ({swDefault.ElapsedMilliseconds} ms) was unexpectedly much slower than "
            + $"GenerateId(8) ({swVariable.ElapsedMilliseconds} ms)."
        );
    }
}

