using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Localization;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Hviktor.Components.Code;

/// <summary>
/// <c>CodeBlock</c> displays syntax-highlighted code blocks.
/// </summary>
public partial class CodeBlock : ComponentBase
{
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;
    [Inject] private ILogger<CodeBlock> Logger { get; set; } = null!;
    [Inject] private IStringLocalizerService<Resources.Resources> Localizer { get; set; } = null!;
    [Inject] private IColorService ColorService { get; set; } = null!;
    [Inject] private ISizeService SizeService { get; set; } = null!;

    private bool isCopied;
    private bool jsInteropAllowed;
    private System.Timers.Timer? copiedResetTimer;

    /// <summary>
    /// The programming language of the code block.
    /// Supported: csharp, razor, html, xml, css, scss, json, javascript, typescript, bash, sql, yaml, and more.
    /// Defaults to "plaintext".
    /// </summary>
    [Parameter]
    [DefaultValue("plaintext")]
    public string Language { get; set; } = "plaintext";

    /// <summary>
    /// The verbatim source code to display.
    /// </summary>
    [Parameter]
    public string? Code { get; set; }

    /// <summary>
    /// Optional filename to display in the header (e.g. "Button.razor.cs").
    /// </summary>
    [Parameter]
    public string? Filename { get; set; }

    /// <summary>
    /// If true, shows line numbers alongside the code.
    /// </summary>
    [Parameter]
    public bool LineNumbers { get; set; }

    /// <summary>
    /// If true, shows a copy-to-clipboard button in the header. Defaults to true.
    /// </summary>
    [Parameter]
    [DefaultValue(true)]
    public bool Copyable { get; set; } = true;

    /// <summary>
    /// If true, disables syntax highlighting and renders plain text.
    /// </summary>
    [Parameter]
    public bool NoHighlight { get; set; }

    /// <summary>
    /// Controls how code overflow is handled: "scroll" (default), "wrap", or "auto".
    /// "auto" wraps on mobile and scrolls on desktop.
    /// </summary>
    [Parameter]
    [DefaultValue("scroll")]
    public string Overflow { get; set; } = "scroll";

    /// <summary>
    /// The color scheme for the code block container.
    /// </summary>
    [Parameter]
    public Color Color { get; set; } = Color.Neutral;

    /// <summary>
    /// The size of the code block text.
    /// </summary>
    [Parameter]
    public Size? Size { get; set; }

    /// <summary>
    /// Captures all unmatched attributes and applies them to the root element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { protected get; set; }

    private string NormalizedCode => NormalizeIndentation(Code);

    private string HighlightedCode =>
        NoHighlight || string.IsNullOrWhiteSpace(Code)
            ? HtmlEncoder.Default.Encode(NormalizedCode)
            : SyntaxHighlighter.Highlight(NormalizedCode, Language);

    private string[] Lines => NormalizedCode.Split('\n');

    private string LanguageDisplay => SyntaxHighlighter.GetLanguageDisplayName(Language);

    private string LanguageCssClass => $"codeblock-code {SyntaxHighlighter.GetLanguageCssClass(Language)}";

    private bool ShowHeader => !string.IsNullOrEmpty(Language) || !string.IsNullOrEmpty(Filename) || Copyable;

    private string CopyButtonLabel => isCopied
        ? Localizer.GetValue("Hviktor.Components.CodeBlock.Copied.Button")
        : Localizer.GetValue("Hviktor.Components.CodeBlock.Copy.Button");

    private string GetFigureClass()
    {
        var cls = $"codeblock codeblock-overflow-{Overflow}";
        if (!Copyable)
        {
            cls += " codeblock-not-copyable";
        }

        return cls;
    }

    [SuppressMessage("Performance", "CA1859:Use concrete types when possible for improved performance")]
    private IReadOnlyDictionary<string, object?> ComputeAttributes() =>
        HtmlAttributeBuilder.ToDictionary(AdditionalAttributes)
            .AddAttribute("role", "figure")
            .AddClasses(GetFigureClass())
            .AddAttribute("aria-label", Localizer.GetValue("Hviktor.Components.CodeBlock.AriaLabel"))
            .AddDataAttribute("color", ColorService.GetDataAttribute(Color))
            .AddDataAttribute("size", Size.HasValue ? SizeService.GetDataAttribute(Size.Value) : null);

    /// <inheritdoc/>
    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            jsInteropAllowed = true;
        }
    }

    /// <summary>
    /// Copies the raw code to the clipboard.
    /// </summary>
    private async Task CopyToClipboardAsync()
    {
        if (string.IsNullOrWhiteSpace(Code) || !jsInteropAllowed)
        {
            return;
        }

        try
        {
            await JsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", NormalizedCode);
            isCopied = true;
            StateHasChanged();

            // Reset after 2 seconds
            copiedResetTimer?.Dispose();
            copiedResetTimer = new System.Timers.Timer(2000) { AutoReset = false };
            copiedResetTimer.Elapsed += async (_, _) =>
            {
                isCopied = false;
                await InvokeAsync(StateHasChanged);
            };
            copiedResetTimer.Start();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to copy to clipboard.");
        }
    }

    private static string NormalizeIndentation(string? code)
    {
        if (string.IsNullOrEmpty(code))
        {
            return string.Empty;
        }

        var lines = code.Replace("\r\n", "\n").Split('\n');
        var firstNonEmpty = lines.FirstOrDefault(l => !string.IsNullOrWhiteSpace(l));
        if (firstNonEmpty is null)
        {
            return code;
        }

        var baseIndent = firstNonEmpty.TakeWhile(char.IsWhiteSpace).Count();
        var normalized = lines
            .Select(line => line.Length >= baseIndent ? line[baseIndent..] : line.TrimStart())
            .ToList();

        // Trim leading/trailing empty lines
        while (normalized.Count > 0 && string.IsNullOrWhiteSpace(normalized[0]))
        {
            normalized.RemoveAt(0);
        }

        while (normalized.Count > 0 && string.IsNullOrWhiteSpace(normalized[^1]))
        {
            normalized.RemoveAt(normalized.Count - 1);
        }

        return string.Join("\n", normalized);
    }
}