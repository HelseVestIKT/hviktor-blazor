using System.Globalization;

namespace Hviktor.Abstractions.Models;

/// <summary>
/// Represents a CSS length value that can be specified as either a numeric pixel value or a full CSS string
/// (e.g. <c>10</c> → <c>"10px"</c>, <c>"2rem"</c> → <c>"2rem"</c>).
/// </summary>
/// <remarks>
/// Accepted forms:
/// <list type="bullet">
///   <item><description><c>100</c> (<see cref="int"/>) → <c>"100px"</c></description></item>
///   <item><description><c>1.5</c> (<see cref="double"/>) → <c>"1.5px"</c></description></item>
///   <item><description><c>"20rem"</c>, <c>"1em"</c>, <c>"100%"</c>, <c>"50vw"</c>, <c>"10vh"</c></description></item>
///   <item><description>Any valid CSS length expression, e.g. <c>"clamp(1rem, 5vw, 3rem)"</c></description></item>
/// </list>
/// Bare numbers default to <c>px</c>. Strings are passed through as-is.<br/>
/// A value of <c>0</c> or <see langword="null"/> produces an empty instance — no style attribute is emitted.
/// </remarks>
public readonly struct CssLength : IEquatable<CssLength>
{
    private readonly string? value;

    private CssLength(string? value) => this.value = value;

    /// <summary>
    /// Returns the CSS string representation, or <see langword="null"/> when no value is set.
    /// </summary>
    public string? ToCssString() => value;

    /// <summary>
    /// Gets the numeric pixel count when the value was constructed from an integer, otherwise <see langword="null"/>.
    /// </summary>
    public int? PixelValue
    {
        get
        {
            if (value is null || !value.EndsWith("px", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            return int.TryParse(value[..^2], out var px) ? px : null;
        }
    }

    /// <summary>
    /// Returns <see langword="true"/> when no value has been set (i.e. value is zero or <see langword="null"/>).
    /// </summary>
    public bool IsEmpty => value is null;

    /// <summary>Implicitly converts an <see cref="int"/> to a pixel-based <see cref="CssLength"/>.</summary>
    public static implicit operator CssLength(int pixels)
        => pixels == 0
            ? default
            : new CssLength($"{pixels}px");

    /// <summary>Implicitly converts an <see cref="double"/> to a pixel-based <see cref="CssLength"/>.</summary>
    public static implicit operator CssLength(double pixels)
        => Math.Abs(pixels) < double.Epsilon
            ? default
            : new CssLength(string.Create(CultureInfo.InvariantCulture, $"{pixels}px"));

    /// <summary>
    /// Implicitly converts a <see cref="string"/> to a <see cref="CssLength"/>, passing the value through as-is.
    /// Any valid CSS length expression is accepted, e.g.:
    /// <c>"20rem"</c>, <c>"1em"</c>, <c>"100%"</c>, <c>"50vw"</c>, <c>"clamp(1rem, 5vw, 3rem)"</c>.
    /// Bare numeric strings (e.g. <c>"200"</c>) are treated as pixels, matching the <see cref="int"/> and
    /// <see cref="double"/> conversion behaviour (e.g. <c>"200"</c> → <c>"200px"</c>).
    /// Whitespace-only or <see langword="null"/> strings produce an empty instance.
    /// </summary>
    public static implicit operator CssLength(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return default;
        }

        var trimmed = value.Trim();

        // Bare integer (e.g. "200")
        if (int.TryParse(trimmed, out var intPixels))
        {
            return intPixels == 0
                ? default
                : new CssLength($"{intPixels}px");
        }

        // Bare decimal (e.g. "1.5") — use invariant culture to avoid comma separators
        if (double.TryParse(trimmed, NumberStyles.Float, CultureInfo.InvariantCulture, out var doublePixels))
        {
            return Math.Abs(doublePixels) < double.Epsilon
                ? default
                : new CssLength(string.Create(CultureInfo.InvariantCulture, $"{doublePixels}px"));
        }

        return new CssLength(trimmed);
    }

    /// <inheritdoc/>
    public override string? ToString() => value;

    /// <inheritdoc/>
    public bool Equals(CssLength other) => string.Equals(value, other.value, StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is CssLength other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => value?.ToLowerInvariant().GetHashCode() ?? 0;

    /// <summary>Equality operator.</summary>
    public static bool operator ==(CssLength left, CssLength right) => left.Equals(right);

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(CssLength left, CssLength right) => !left.Equals(right);
}