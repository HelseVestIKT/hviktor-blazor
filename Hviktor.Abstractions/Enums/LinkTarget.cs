using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Hviktor.Abstractions.Enums;

/// <summary>
/// Represents the target frame for a link or navigation action.<br/>
/// It is also used to determine the CSS class for styling purposes.
/// </summary>
public enum LinkTarget
{
    /// <summary>
    /// Opens the link in the same tab or window.
    /// </summary>
    Self,

    /// <inheritdoc cref="LinkTarget.Self"/>
    SameTab,

    /// <summary>
    /// Opens the link in a new tab or window.
    /// </summary>
    Blank,

    /// <inheritdoc cref="LinkTarget.Blank"/>
    NewTab,

    /// <summary>
    /// Opens the link in the parent frame.
    /// </summary>
    Parent,

    /// <inheritdoc cref="LinkTarget.Parent"/>
    ParentFrame,

    /// <summary>
    /// Opens the link in the topmost frame.
    /// </summary>
    Top,

    /// <inheritdoc cref="LinkTarget.Top"/>
    TopFrame,

    /// <summary>
    /// Opens the link in the topmost frame, but only if it is not already loaded.
    /// </summary>
    UnfencedTop
}

/// <summary>
/// Extension methods for <see cref="LinkTarget"/>.
/// </summary>
public static class LinkTargetExtensions
{
    /// <param name="target">The target to convert to a data attribute value.</param>
    extension(LinkTarget target)
    {
        /// <summary>
        /// Gets the data attribute value for the specified <see cref="LinkTarget"/>.<br/>
        /// If the provided target is not allowed, the default value is used instead.
        /// </summary>
        /// <returns>The data attribute value for the specified <see cref="LinkTarget"/>.</returns>
        public string GetDataAttribute()
        {
            return target switch
            {
                LinkTarget.NewTab or LinkTarget.Blank => "_blank",
                LinkTarget.ParentFrame or LinkTarget.Parent => "_parent",
                LinkTarget.TopFrame or LinkTarget.Top => "_top",
                LinkTarget.UnfencedTop => "_unfencedTop",
                _ => "_self" // LinkTarget.SameTab
            };
        }
    }
}