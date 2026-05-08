using Microsoft.AspNetCore.Components;

namespace Tests.Components.Shared;

/// <summary>
/// A wrapper component for individual test cases.
/// Each section represents a single testable scenario with a unique identifier.
/// </summary>
public partial class ComponentSection
{
    /// <summary>
    /// Unique identifier for this test case. Used as the section's id attribute.
    /// This should match the test method or fact name for easy correlation.
    /// </summary>
    [Parameter, EditorRequired]
    public required string TestId { get; set; }

    /// <summary>
    /// Human-readable name for the test case.
    /// </summary>
    [Parameter, EditorRequired]
    public required string TestName { get; set; }

    /// <summary>
    /// Optional description of what this test case validates.
    /// </summary>
    [Parameter]
    public string? Description { get; set; }

    /// <summary>
    /// Category for grouping related tests (e.g., "variant", "state", "interaction").
    /// </summary>
    [Parameter]
    public string Category { get; set; } = "default";

    /// <summary>
    /// Optional description of expected behavior for documentation.
    /// </summary>
    [Parameter]
    public string? ExpectedBehavior { get; set; }

    /// <summary>
    /// Current status of the test section for visual feedback.
    /// </summary>
    [Parameter]
    public TestStatus Status { get; set; } = TestStatus.Ready;

    /// <summary>
    /// The test case content to render.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}

public enum TestStatus
{
    Ready,
    Running,
    Passed,
    Failed
}