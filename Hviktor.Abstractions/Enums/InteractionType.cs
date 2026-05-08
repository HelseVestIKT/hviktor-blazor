namespace Hviktor.Abstractions.Enums;

/// <summary>
/// Represents the type of interaction that can occur with a loader.
/// </summary>
public enum InteractionType
{
    /// <summary>
    /// No interaction type specified. This is the default value and indicates that no specific interaction is allowed.
    /// </summary>
    None,

    /// <summary>
    /// Indicates that the loader can be closed. This interaction type is typically used to stop or hide the loader.
    /// </summary>
    Close,

    /// <summary>
    /// Indicates that the loader can be reloaded. This interaction type is typically used to refresh the loader's content or state.
    /// </summary>
    Reload,

    /// <summary>
    /// Indicates that the loader can be both closed and reloaded. This interaction type allows for both stopping and refreshing the loader.
    /// </summary>
    All,
}