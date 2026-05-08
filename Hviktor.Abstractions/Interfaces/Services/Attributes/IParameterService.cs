using Hviktor.Abstractions.Models;

namespace Hviktor.Abstractions.Interfaces.Services.Attributes;

/// <summary>
/// Service for working with parameter values.
/// </summary>
/// <typeparam name="T">
/// The type of parameter values.
/// </typeparam>
public interface IParameterService<T> where T : struct, Enum
{
    /// <summary>
    /// Gets the data attribute value for the specified parameter value.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    string GetDataAttribute(EnumValue<T> value);

    /// <summary>
    /// Gets the data attribute value for the specified parameter value.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    string GetDataAttribute(EnumValue<T> value, T defaultValue);

    /// <summary>
    /// Gets the parameter value from the specified string.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    T GetFromString(string value);

    /// <summary>
    /// Gets the parameter value from the specified string.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    T GetFromString(string value, T defaultValue);
}