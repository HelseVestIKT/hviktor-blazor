using Hviktor.Models;
using Hviktor.Rendering;

// ReSharper disable once CheckNamespace
namespace Search;

/// <summary>
/// The Input component represents the input field within a search form.
/// </summary>
public partial class Input : AsyncNestedComponentBase<Hviktor.Components.Search.Search>
{
    /// <inheritdoc/>
    protected override Dictionary<string, object?> ComputeAttributes() => HtmlAttributeBuilder.ToDictionary(base.ComputeAttributes())
        .AddIdentity(() => Parent is not null, $"{Parent?.Id}-input")
        .AddClasses("ds-input");
}