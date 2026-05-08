namespace Hviktor.Abstractions.Models;

/// <summary>
/// Represents a parameter value that can be supplied either as a strongly-typed <typeparamref name="TEnum"/>
/// or as a raw <see cref="string"/> (e.g. for dynamic / data-driven scenarios).
/// </summary>
/// <typeparam name="TEnum">The enum type this value wraps.</typeparam>
/// <remarks>
/// Implicit conversion from <typeparamref name="TEnum"/> stores the enum value.<br/>
/// Implicit conversion from <see cref="string"/> stores the raw string.<br/>
/// <see cref="IsRaw"/> distinguishes the two cases when consuming the value.
/// </remarks>
public readonly struct EnumValue<TEnum> : IEquatable<EnumValue<TEnum>> where TEnum : struct, Enum
{
    private EnumValue(TEnum value)
    {
        EnumValueOrNull = value;
        RawValue = null;
    }

    private EnumValue(string value)
    {
        RawValue = value;
        EnumValueOrNull = null;
    }

    /// <summary>
    /// Returns <see langword="true"/> when the value was supplied as a raw string
    /// rather than a typed <typeparamref name="TEnum"/>.
    /// </summary>
    public bool IsRaw => RawValue is not null;

    /// <summary>
    /// Returns <see langword="true"/> when no value has been set.
    /// </summary>
    public bool IsEmpty => EnumValueOrNull is null && RawValue is null;

    /// <summary>
    /// Gets the typed enum value, or <see langword="null"/> when the value was supplied as a string.
    /// </summary>
    public TEnum? EnumValueOrNull { get; }

    /// <summary>
    /// Gets the raw string value, or <see langword="null"/> when the value was supplied as an enum.
    /// </summary>
    public string? RawValue { get; }

    /// <summary>Implicitly converts a <typeparamref name="TEnum"/> to an <see cref="EnumValue{TEnum}"/>.</summary>
    public static implicit operator EnumValue<TEnum>(TEnum value) => new(value);

    /// <summary>Implicitly converts a nullable <typeparamref name="TEnum"/> to an <see cref="EnumValue{TEnum}"/>.</summary>
    public static implicit operator EnumValue<TEnum>(TEnum? value)
        => value.HasValue ? new EnumValue<TEnum>(value.Value) : default;

    /// <summary>Implicitly converts a <see cref="string"/> to an <see cref="EnumValue{TEnum}"/>.</summary>
    public static implicit operator EnumValue<TEnum>(string? value)
        => string.IsNullOrWhiteSpace(value) ? default : new EnumValue<TEnum>(value.Trim());

    /// <inheritdoc/>
    public override string ToString()
        => EnumValueOrNull?.ToString() ?? RawValue ?? string.Empty;

    /// <summary>
    /// Resolves the effective <typeparamref name="TEnum"/> value, parsing <see cref="RawValue"/> when needed.
    /// Returns <see langword="null"/> if no value is set or the raw string cannot be parsed.
    /// </summary>
    private TEnum? ResolvedEnumValue
        => EnumValueOrNull
           ?? (RawValue is not null && Enum.TryParse<TEnum>(RawValue, ignoreCase: true, out var parsed)
               ? parsed
               : null);

    /// <inheritdoc/>
    public bool Equals(EnumValue<TEnum> other)
    {
        var thisResolved = ResolvedEnumValue;
        var otherResolved = other.ResolvedEnumValue;

        // Both resolved to a typed enum value, compare by enum.
        if (thisResolved is not null && otherResolved is not null)
        {
            return EqualityComparer<TEnum?>.Default.Equals(thisResolved, otherResolved);
        }

        // Neither could be resolved, compare raw strings (case-insensitive).
        if (thisResolved is null && otherResolved is null)
        {
            return string.Equals(RawValue, other.RawValue, StringComparison.OrdinalIgnoreCase);
        }

        // One resolved, one didn't, not equal.
        return false;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is EnumValue<TEnum> other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        var resolved = ResolvedEnumValue;
        return resolved is not null
            ? HashCode.Combine(resolved)
            : HashCode.Combine(RawValue?.ToLowerInvariant());
    }

    /// <summary>Equality operator.</summary>
    public static bool operator ==(EnumValue<TEnum> left, EnumValue<TEnum> right) => left.Equals(right);

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(EnumValue<TEnum> left, EnumValue<TEnum> right) => !left.Equals(right);
}