# **Tests.Unit**

Unit tests for Hviktor components, models, and services using [xUnit v3 (xunit.net)](https://xunit.net/)
and [bUnit (bunit.dev)](https://bunit.dev/).

## Table of Contents

- [Running Tests](#running-tests)
- [Writing Tests](#writing-tests)
- [Traits](#traits)
- [Conventions](#conventions)

## Running Tests

```bash
# Run all unit tests
dotnet test Tests.Unit

# Filter by component
dotnet test Tests.Unit --filter "Component=Button"

# Filter by category
dotnet test Tests.Unit --filter "Category=rendering"
```

## Writing Tests

Extend `HviktorBunitContext` as the base class. It registers `AddHviktor()` and `AddLogging()` automatically:

```csharp
public class ButtonTests : HviktorBunitContext
{
    [Fact]
    [Trait(TestCollections.Traits.Component, "Button")]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Button_RendersWithDefaultValues()
    {
        var component = Render<Button>();
        Assert.NotNull(component.Instance);
    }
}
```

## Traits

Use constants from `TestCollections` for filtering:

- **Component**: `[Trait(Traits.Component, "Button")]`
- **Category**: `[Trait(Traits.Category, Categories.Rendering)]`

Available categories: `LifeCycle`, `Rendering`, `Attributes`, `Callbacks`, `Disposal`, `Integrations`, `Cascading`,
`Nested`, `Identity`, `Popover`, `Trigger`, `Context`.

## Conventions

- One test class per component, mirroring the component folder structure.
- Test naming: `Component_Scenario_ExpectedBehavior`.
- Use `[Fact]` for single cases, `[Theory]` with `[InlineData]` for parameterized tests.
- Test all enum values for constrained parameters (variant, size, color).
