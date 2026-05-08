namespace Tests.Components.Shared;

/// <summary>
/// Defines test metadata for a ComponentSection.
/// Used by Playwright tests to locate and validate test cases.
/// </summary>
public sealed record ComponentTestCase
{
    /// <summary>
    /// The unique test ID (matches the section's id attribute).
    /// </summary>
    public required string TestId { get; init; }

    /// <summary>
    /// Human-readable test name.
    /// </summary>
    public required string TestName { get; init; }

    /// <summary>
    /// Test category for grouping.
    /// </summary>
    public string Category { get; init; } = "default";

    /// <summary>
    /// Description of the test case.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// CSS selector to locate this test section.
    /// </summary>
    public string Selector => $"#{TestId}";

    /// <summary>
    /// CSS selector for the content area of this test section.
    /// </summary>
    public string ContentSelector => $"#{TestId}-content";
}

/// <summary>
/// Registry of all component test cases for a given component.
/// </summary>
public static class ButtonTestCases
{
    public static readonly ComponentTestCase VariantPrimary = new()
    {
        TestId = "button-variant-primary",
        TestName = "Button renders with primary variant",
        Category = "variant",
        Description = "Primary variant should have data-variant='primary' attribute"
    };

    public static readonly ComponentTestCase VariantSecondary = new()
    {
        TestId = "button-variant-secondary",
        TestName = "Button renders with secondary variant",
        Category = "variant"
    };

    public static readonly ComponentTestCase VariantTertiary = new()
    {
        TestId = "button-variant-tertiary",
        TestName = "Button renders with tertiary variant",
        Category = "variant"
    };

    public static readonly ComponentTestCase IconTrue = new()
    {
        TestId = "button-icon-true",
        TestName = "Button renders as icon-only",
        Category = "icon"
    };

    public static readonly ComponentTestCase IconFalse = new()
    {
        TestId = "button-icon-false",
        TestName = "Button renders without icon-only styling by default",
        Category = "icon"
    };

    public static readonly ComponentTestCase LoadingTrue = new()
    {
        TestId = "button-loading-true",
        TestName = "Button renders in loading state",
        Category = "loading"
    };

    public static readonly ComponentTestCase LoadingFalse = new()
    {
        TestId = "button-loading-false",
        TestName = "Button renders without loading state by default",
        Category = "loading"
    };

    public static readonly ComponentTestCase LoadingWithIcon = new()
    {
        TestId = "button-loading-with-icon",
        TestName = "Icon button shows spinner when loading",
        Category = "loading"
    };

    public static readonly ComponentTestCase TypeButton = new()
    {
        TestId = "button-type-button",
        TestName = "Button has type='button' by default",
        Category = "type"
    };

    public static readonly ComponentTestCase TypeSubmit = new()
    {
        TestId = "button-type-submit",
        TestName = "Button renders with type='submit'",
        Category = "type"
    };

    public static readonly ComponentTestCase TypeReset = new()
    {
        TestId = "button-type-reset",
        TestName = "Button renders with type='reset'",
        Category = "type"
    };

    public static readonly ComponentTestCase DisabledTrue = new()
    {
        TestId = "button-disabled-true",
        TestName = "Button renders in disabled state",
        Category = "state"
    };

    public static readonly ComponentTestCase AriaDisabled = new()
    {
        TestId = "button-aria-disabled",
        TestName = "Button renders with aria-disabled",
        Category = "state"
    };

    public static readonly ComponentTestCase ClickHandler = new()
    {
        TestId = "button-click-handler",
        TestName = "Button triggers onclick event",
        Category = "interaction"
    };

    public static readonly ComponentTestCase KeyboardEnter = new()
    {
        TestId = "button-keyboard-enter",
        TestName = "Button activates with Enter key",
        Category = "interaction"
    };

    public static readonly ComponentTestCase KeyboardSpace = new()
    {
        TestId = "button-keyboard-space",
        TestName = "Button activates with Space key",
        Category = "interaction"
    };

    public static readonly ComponentTestCase FocusVisible = new()
    {
        TestId = "button-focus-visible",
        TestName = "Button shows focus indicator",
        Category = "accessibility"
    };

    public static readonly ComponentTestCase FormSubmit = new()
    {
        TestId = "button-form-submit",
        TestName = "Submit button submits form",
        Category = "form"
    };

    public static readonly ComponentTestCase DynamicLoading = new()
    {
        TestId = "button-dynamic-loading",
        TestName = "Button toggles loading state dynamically",
        Category = "state"
    };

    /// <summary>
    /// All test cases for the Button component.
    /// </summary>
    public static IReadOnlyList<ComponentTestCase> All =>
    [
        VariantPrimary,
        VariantSecondary,
        VariantTertiary,
        IconTrue,
        IconFalse,
        LoadingTrue,
        LoadingFalse,
        LoadingWithIcon,
        TypeButton,
        TypeSubmit,
        TypeReset,
        DisabledTrue,
        AriaDisabled,
        ClickHandler,
        KeyboardEnter,
        KeyboardSpace,
        FocusVisible,
        FormSubmit,
        DynamicLoading
    ];
}
