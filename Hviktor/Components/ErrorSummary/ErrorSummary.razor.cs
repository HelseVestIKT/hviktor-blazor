using Hviktor.Models;
using Hviktor.Rendering;
using Hviktor.Security;
using Microsoft.AspNetCore.Components;

namespace Hviktor.Components.ErrorSummary;

/// <summary>
/// <c>ErrorSummary</c> is a summary of errors.<br/>
/// It provides the user with an overview of errors or issues that need to be addressed on a page or step in order to proceed.
/// </summary>
/// <use>
/// Use <c>ErrorSummary</c> when:
/// <list type="bullet">
///   <item>You need to present a clear overview of the errors that must be corrected before a form can be submitted.</item>
///   <item>A form contains many fields, making it difficult for users to understand where the errors are located.</item>
/// </list>
/// </use>
/// <avoid>
/// Avoid <c>ErrorSummary</c> when:
/// <list type="bullet">
///   <item>The feedback does not prevent submission, such as warnings or recommendations</item>
///   <item>You need to display system-level messages; use Alert instead</item>
/// </list>
/// </avoid>
/// <guidelines>
/// <c>ErrorSummary</c> is used to display a summary when there are errors or omissions in something the user has done. A summary may include one or several errors.<br/>
/// <c>ErrorSummary</c> should contain all error messages present on the page, so users can navigate directly to them by clicking the links in the summary.
/// </guidelines>
public partial class ErrorSummary : CascadingComponentBase
{
    /// <summary>
    /// Specifies the content to be rendered within the <c>ErrorSummary</c> component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <inheritdoc/>
    protected override Dictionary<string, object?> ComputeAttributes()
    {
        var builder = HtmlAttributeBuilder.ToDictionary(base.ComputeAttributes())
            .AddClasses("ds-error-summary")
            .AddAttribute("aria-labelledby", LabelledbyId)
            .RemoveFromTabOrder();

        return builder;
    }

    private string LabelledbyId { get; set; } = Cryptography.GenerateId();

    internal void SetLabelledbyId(string value)
    {
        if (LabelledbyId == value)
        {
            return;
        }

        LabelledbyId = value;
        StateHasChanged();
    }
}