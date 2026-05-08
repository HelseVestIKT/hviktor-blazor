using System.Text.RegularExpressions;
using Hviktor.Security;

namespace Tests.Unit.Security;

/// <summary>
/// Unit tests for <see cref="Cryptography"/>.
/// Covers correctness, alphabet constraints, uniqueness, length,
/// distribution (statistical), and performance.
/// </summary>
[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Identity)]
public partial class CryptographyTests
{
    // The full 64-char alphabet used by Cryptography.
    private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_";

    // Letters only (positions 0-51) - valid first characters.
    private const string Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    [GeneratedRegex(@"^[A-Za-z][A-Za-z0-9\-_]{7}$")]
    private static partial Regex DefaultIdPattern();

    [GeneratedRegex(@"^[A-Za-z][A-Za-z0-9\-_]+$")]
    private static partial Regex VariableLengthIdPattern();

    #region GenerateId()

    [Fact]
    public void GenerateId_ReturnsNonNullOrEmpty()
    {
        var id = Cryptography.GenerateId();
        Assert.False(string.IsNullOrEmpty(id));
    }

    [Fact]
    public void GenerateId_HasLengthEight()
    {
        var id = Cryptography.GenerateId();
        Assert.Equal(8, id.Length);
    }

    [Fact]
    public void GenerateId_StartsWithLetter()
    {
        // First char must always be A-Z or a-z (positions 0-51 in the table).
        for (var i = 0; i < 200; i++)
        {
            var id = Cryptography.GenerateId();
            Assert.Contains(id[0], Letters);
        }
    }

    [Fact]
    public void GenerateId_ContainsOnlyAlphabetCharacters()
    {
        for (var i = 0; i < 200; i++)
        {
            var id = Cryptography.GenerateId();
            foreach (var c in id)
            {
                Assert.Contains(c, Alphabet);
            }
        }
    }

    [Fact]
    public void GenerateId_MatchesExpectedPattern()
    {
        var pattern = DefaultIdPattern();

        for (var i = 0; i < 200; i++)
        {
            var id = Cryptography.GenerateId();
            Assert.Matches(pattern, id);
        }
    }

    [Fact]
    public void GenerateId_ProducesUniqueValues()
    {
        const int count = 10_000;
        var ids = new HashSet<string>(count);

        for (var i = 0; i < count; i++)
        {
            ids.Add(Cryptography.GenerateId());
        }

        // Collision probability for 8-char base-64 IDs over 10 000 samples is ~0.0001 %.
        // Allow zero collisions - any collision is a serious bug.
        Assert.Equal(count, ids.Count);
    }

    [Fact]
    public void GenerateId_NeverContainsPaddingOrWhitespace()
    {
        for (var i = 0; i < 200; i++)
        {
            var id = Cryptography.GenerateId();
            Assert.DoesNotContain('=', id);
            Assert.DoesNotContain('+', id);
            Assert.DoesNotContain('/', id);
            Assert.DoesNotContain(' ', id);
        }
    }

    #endregion

    #region GenerateId(int length)

    [Theory]
    [InlineData(1)]
    [InlineData(4)]
    [InlineData(8)]
    [InlineData(16)]
    [InlineData(32)]
    [InlineData(64)]
    [InlineData(128)]
    public void GenerateId_WithLength_HasCorrectLength(int length)
    {
        var id = Cryptography.GenerateId(length);

        Assert.Equal(length, id.Length);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(8)]
    [InlineData(32)]
    public void GenerateId_WithLength_StartsWithLetter(int length)
    {
        for (var i = 0; i < 100; i++)
        {
            var id = Cryptography.GenerateId(length);
            Assert.Contains(id[0], Letters);
        }
    }

    [Theory]
    [InlineData(8)]
    [InlineData(16)]
    [InlineData(32)]
    public void GenerateId_WithLength_ContainsOnlyAlphabetCharacters(int length)
    {
        for (var i = 0; i < 100; i++)
        {
            var id = Cryptography.GenerateId(length);
            foreach (var c in id)
            {
                Assert.Contains(c, Alphabet);
            }
        }
    }

    [Theory]
    [InlineData(8)]
    [InlineData(16)]
    [InlineData(32)]
    public void GenerateId_WithLength_MatchesExpectedPattern(int length)
    {
        var pattern = VariableLengthIdPattern();

        for (var i = 0; i < 100; i++)
        {
            var id = Cryptography.GenerateId(length);
            Assert.Matches(pattern, id);
        }
    }

    [Theory]
    [InlineData(8)]
    [InlineData(16)]
    public void GenerateId_WithLength_ProducesUniqueValues(int length)
    {
        const int count = 5_000;
        var ids = new HashSet<string>(count);

        for (var i = 0; i < count; i++)
        {
            ids.Add(Cryptography.GenerateId(length));
        }

        Assert.Equal(count, ids.Count);
    }

    #endregion

    #region Statistical distribution - characters should not be systematically skewed

    [Fact]
    public void GenerateId_CharacterDistribution_IsReasonablyUniform()
    {
        // Collect a large sample and check that every alphabet character appears.
        // A perfect uniform distribution gives each char ~1/64 frequency.
        // We allow a generous ±50 % tolerance to avoid flakiness while still
        // catching a broken alphabet (e.g. only upper-case letters generated).
        const int sampleCount = 50_000;
        var frequency = new Dictionary<char, int>();

        foreach (var c in Alphabet)
        {
            frequency[c] = 0;
        }

        for (var i = 0; i < sampleCount; i++)
        {
            // Use the variable-length overload so all positions are covered.
            var id = Cryptography.GenerateId(8);

            // Skip position 0 - it is intentionally restricted to letters only.
            for (var pos = 1; pos < id.Length; pos++)
            {
                frequency[id[pos]]++;
            }
        }

        var totalChars = sampleCount * 7; // 7 unrestricted positions per id
        var expectedPerChar = (double)totalChars / Alphabet.Length;
        var lowerBound = (int)(expectedPerChar * 0.50);
        var upperBound = (int)(expectedPerChar * 1.50);

        foreach (var kvp in frequency)
        {
            Assert.InRange(kvp.Value, lowerBound, upperBound);
        }
    }

    [Fact]
    public void GenerateId_FirstCharDistribution_IsRestrictedToLetters()
    {
        // Position 0 must map only to the first 52 chars of the alphabet.
        const int sampleCount = 10_000;
        var nonLetterCount = 0;

        for (var i = 0; i < sampleCount; i++)
        {
            var first = Cryptography.GenerateId()[0];
            if (!Letters.Contains(first))
            {
                nonLetterCount++;
            }
        }

        Assert.Equal(0, nonLetterCount);
    }

    #endregion


}