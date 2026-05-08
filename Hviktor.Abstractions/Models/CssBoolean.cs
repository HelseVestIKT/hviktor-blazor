using System.Globalization;

namespace Hviktor.Abstractions.Models;

/// <summary>
/// Represents a CSS boolean value that can be specified as either a numeric 0|1 value or a "true"|"false" CSS string
/// (e.g. <c>0</c> → <c>false</c>, <c>"true"</c> → <c>true</c>).
/// </summary>
/// <remarks>
/// Accepted forms:
/// <list type="bullet">
///   <item><description><c>0</c> (<see cref="int"/>) → <c>false</c></description></item>
///   <item><description><c>1</c> (<see cref="int"/>) → <c>true</c></description></item>
///   <item><description><c>"true"</c> (<see cref="string"/>) → <c>true</c></description></item>
///   <item><description><c>"false"</c> (<see cref="string"/>) → <c>false</c></description></item>
///   <item><description>Any other non-empty string (e.g. <c>"yes"</c>, <c>"on"</c>, <c>"enabled"</c>) → <c>true</c></description></item>
/// </list>
/// </remarks>
public readonly struct CssBoolean : IEquatable<CssBoolean>
{
    private readonly bool value;

    private CssBoolean(string? value) => this.value = value is not null && (string.Equals(value, "true", StringComparison.OrdinalIgnoreCase) || value == "1");

    /// <summary>
    /// Returns the CSS string representation, or <see langword="null"/> when no value is set.
    /// </summary>
    public string? ToCssString() => value ? "true" : "false";

    /// <summary>
    /// Returns <see langword="true"/> when no value has been set (i.e. value is zero or <see langword="null"/>).
    /// </summary>
    public bool IsTruthy => value is true;

    /// <summary>Implicitly converts a <see cref="bool"/> to a <see cref="CssBoolean"/>.</summary>
    public static implicit operator CssBoolean(bool flag) => new(flag ? "true" : "false");

    /// <summary>Implicitly converts an <see cref="int"/> to a <see cref="CssBoolean"/>. Non-zero is <c>true</c>.</summary>
    public static implicit operator CssBoolean(int numeric) => new(numeric != 0 ? "true" : "false");

    /// <summary>Implicitly converts a <see cref="double"/> to a <see cref="CssBoolean"/>. Non-zero is <c>true</c>.</summary>
    public static implicit operator CssBoolean(double numeric) => new(Math.Abs(numeric) > double.Epsilon ? "true" : "false");

    /// <summary>
    /// Implicitly converts a <see cref="string"/> to a <see cref="CssBoolean"/>.
    /// Numeric strings use zero/non-zero semantics; all other non-empty strings are treated as <c>true</c>.
    /// </summary>
    /// <param name="value">The string to convert. Null or whitespace-only values produce an empty instance.</param>
    public static implicit operator CssBoolean(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return default;
        }

        var trimmed = value.Trim();

        if (int.TryParse(trimmed, out var numeric))
        {
            return new CssBoolean(numeric != 0 ? "true" : "false");
        }

        if (double.TryParse(trimmed, NumberStyles.Float, CultureInfo.InvariantCulture, out var numericDouble))
        {
            return new CssBoolean(Math.Abs(numericDouble) > double.Epsilon ? "true" : "false");
        }

        return new CssBoolean(trimmed);
    }

    /// <inheritdoc/>
    public override string? ToString() => value ? "true" : "false";

    /// <inheritdoc/>
    public bool Equals(CssBoolean other) => string.Equals(value.ToString(), other.value.ToString(), StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is CssBoolean other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => value.ToString().ToLowerInvariant().GetHashCode();

    /// <summary>Equality operator.</summary>
    public static bool operator ==(CssBoolean left, CssBoolean right) => left.Equals(right);

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(CssBoolean left, CssBoolean right) => !left.Equals(right);
}