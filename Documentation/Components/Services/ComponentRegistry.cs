using Documentation.Components.Demos;
using Documentation.Components.Demos.Badge;
using Documentation.Components.Demos.Dropdown;
using Documentation.Components.Demos.Breadcrumbs;
using Documentation.Components.Demos.Button;
using Documentation.Components.Demos.Card;
using Documentation.Components.Demos.Checkbox;
using Documentation.Components.Demos.Chip;
using Documentation.Components.Demos.Code;
using Documentation.Components.Demos.Details;
using Documentation.Components.Demos.Dialog;
using Documentation.Components.Demos.Divider;
using Documentation.Components.Demos.ErrorSummary;
using Documentation.Components.Demos.Field;
using Documentation.Components.Demos.Fieldset;
using Documentation.Components.Demos.Icon;
using Documentation.Components.Demos.Input;
using Documentation.Components.Demos.Link;
using Documentation.Components.Demos.List;
using Documentation.Components.Demos.Logo;
using Documentation.Components.Demos.Markdown;
using Documentation.Components.Demos.Pagination;
using Documentation.Components.Demos.Popover;
using Documentation.Components.Demos.Radio;
using Documentation.Components.Demos.RequiredTag;
using Documentation.Components.Demos.Search;
using Documentation.Components.Demos.Select;
using Documentation.Components.Demos.Skeleton;
using Documentation.Components.Demos.SkipLink;
using Documentation.Components.Demos.Spinner;
using Documentation.Components.Demos.Suggestion;
using Documentation.Components.Demos.Switch;
using Documentation.Components.Demos.Table;
using Documentation.Components.Demos.Tabs;
using Documentation.Components.Demos.Tags;
using Documentation.Components.Demos.Textarea;
using Documentation.Components.Demos.Textfield;
using Documentation.Components.Demos.ToggleGroup;
using Documentation.Components.Demos.Tooltip;
using Documentation.Components.Demos.Typography.Heading;
using Documentation.Components.Demos.Typography.Label;
using Documentation.Components.Demos.Typography.Paragraph;
using Documentation.Components.Demos.Typography.ValidationMessage;
using Hviktor.Components.Alert;
using Hviktor.Components.Avatar;
using Hviktor.Components.Button;
using Hviktor.Components.Checkbox;
using Hviktor.Components.Divider;
using Hviktor.Components.Icon;
using Hviktor.Components.Input;
using Hviktor.Components.Link;
using Hviktor.Components.Logo;
using Hviktor.Components.Markdown;
using Hviktor.Components.Radio;
using Hviktor.Components.RequiredTag;
using Hviktor.Components.Switch;
using Hviktor.Components.Tag;
using Hviktor.Components.Textarea;
using Hviktor.Components.Textfield;
using Hviktor.Components.Tooltip;
using Hviktor.Components.Typography;
using Hviktor.Models;
using Hviktor.Models.List;
using System.Collections.Frozen;
using Documentation.Components.Demos.Avatar;
using Documentation.Components.Demos.AvatarStack;
using Documentation.Components.Demos.Loader;
using Hviktor.Components.AvatarStack;
using Hviktor.Components.Loader;
using Hviktor.Components.Skeleton;
using Hviktor.Components.Spinner;

namespace Documentation.Components.Services;

/// <summary>
/// Central registry mapping URL slugs to component documentation information.
/// Enrichment with XML docs and changelog dates is deferred until first access.
/// </summary>
public sealed class ComponentRegistry
{
    private readonly IComponentMetadataService metadataService;
    private readonly IChangelogDateProvider changelogDates;
    private FrozenDictionary<string, ComponentInfo>? bySlug;
    private IReadOnlyList<ComponentInfo>? all;

    /// <summary>Initializes the registry with dependencies. Actual build is deferred to first access.</summary>
    public ComponentRegistry(IComponentMetadataService metadataService, IChangelogDateProvider changelogDates)
    {
        this.metadataService = metadataService;
        this.changelogDates = changelogDates;
    }

    /// <summary>Gets the ordered list of component groups (e.g. "Components", "Typography", "Utilities").</summary>
    public IReadOnlyList<ComponentGroup> Groups => field ??= EnsureBuilt();

    /// <summary>Gets all registered components across all groups.</summary>
    private IReadOnlyList<ComponentInfo> All => all ??= Groups.SelectMany(g => g.Items).ToArray();

    /// <summary>Finds a component by its URL slug, or returns <see langword="null"/>.</summary>
    public ComponentInfo? Get(string slug)
    {
        bySlug ??= All.ToFrozenDictionary(c => c.Slug, StringComparer.OrdinalIgnoreCase);
        return bySlug.GetValueOrDefault(slug);
    }

    /// <summary>Builds and enriches all component groups on first access.</summary>
    private ComponentGroup[] EnsureBuilt()
    {
        var components = BuildComponents();
        var typography = BuildTypography();
        var visuals = BuildVisuals();

        var result = new[]
        {
            BuildGroup("Components", components, metadataService, changelogDates),
            BuildGroup("Typography", typography, metadataService, changelogDates),
            BuildGroup("Visuals", visuals, metadataService, changelogDates),
        };

        all = result.SelectMany(g => g.Items).ToArray();
        bySlug = all.ToFrozenDictionary(c => c.Slug, StringComparer.OrdinalIgnoreCase);

        return result;
    }

    /// <summary>Builds the main components list.</summary>
    private static List<ComponentInfo> BuildComponents()
    {
        return
        [
            new ComponentInfo("alert", "Alert",
                typeof(Alert),
                [
                    new DemoInfo(typeof(AlertDemo), Display: "inline-block")
                ],
                "https://designsystemet.no/en/components/docs/alert/overview",
                IsValidated: true),
            new ComponentInfo("avatar", "Avatar",
                typeof(Avatar),
                [
                    new DemoInfo(typeof(AvatarDemo)),
                    new DemoInfo(typeof(AvatarSizesDemo), "Sizes", "In addition to `sm`, `md` and `lg`, Avatar also supports the `xs` size."),
                    new DemoInfo(typeof(AvatarColorsDemo), "Colors", "<see cref=\"Avatar\"/> is available in all colours in your theme."),
                    new DemoInfo(typeof(AvatarVariantsDemo), "Variants", "<see cref=\"Avatar\"/> can be round or square. Use them consistently throughout your solution."),
                ],
                "https://designsystemet.no/en/components/docs/avatar/overview"),
            new ComponentInfo("avatar-stack", "AvatarStack",
                typeof(AvatarStack),
                [
                    new DemoInfo(typeof(AvatarStackDemo)),
                    new DemoInfo(typeof(AvatarStackCustomizationDemo), "Customization", "`AvatarStack` is designed to be flexible enough to cover many different needs and use cases.", Display: "block"),
                    new DemoInfo(typeof(AvatarStackSizesDemo), "Sizes", "The size of `avatar`s in an `AvatarStack` is controlled via avatarSize and can be specified in any valid CSS size unit for maximum flexibility."),
                    new DemoInfo(typeof(AvatarStackGapDemo), "Gap", "The visible space between `avatar`s can be customized with `gap`"),
                    new DemoInfo(typeof(AvatarStackExpandableDemo), "Expandable", "In some cases, it may be desirable to expand the stack on hover and focus. This functionality is off by default."),
                    new DemoInfo(typeof(AvatarStackVariantsDemo), "Variants", "`AvatarStack` supports both round and square variants, but only one variant per stack."),
                    new DemoInfo(typeof(AvatarStackIndicatingAvatarNotShownDemo), "Indicating avatars not shown", "When there are more avatars than can be displayed in the stack, there are two different ways to indicate this."),
                    new DemoInfo(typeof(AvatarStackTooltipsAndLinksDemo), "Tooltips and links", "`Avatar`s in `AvatarStack` can have tooltips and be links, but be aware of accessibility considerations.", IsNotSupported: true),
                ],
                "https://designsystemet.no/en/components/docs/avatar-stack/overview",
                IsExperimental: true),
            new ComponentInfo("badge", "Badge",
                typeof(Hviktor.Components.Badge.Badge),
                [
                    new DemoInfo(typeof(BadgeDemo)),
                    new DemoInfo(typeof(BadgeVariantsDemo), "Variants", "Badges have two variants, default and tinted. The default variant has a solid background color, while the tinted variant has a lighter background color and a colored border."),
                    new DemoInfo(typeof(BadgeFloatingDemo), "Floating", "Badges can be placed floating above the content they are associated with."),
                    new DemoInfo(typeof(BadgeCustomPlacementDemo), "Custom Placement", "Badges can be placed at custom positions relative to their associated content."),
                ],
                "https://designsystemet.no/en/components/docs/badge/overview",
                SubComponents:
                [
                    new SubComponentInfo("Badge.Position", typeof(Badge.Position))
                ]),
            new ComponentInfo("breadcrumbs", "Breadcrumbs",
                typeof(Hviktor.Components.Breadcrumbs.Breadcrumbs),
                [
                    new DemoInfo(typeof(BreadcrumbsDemo)),
                    new DemoInfo(typeof(BreadcrumbsBackButtonOnlyDemo), "Back button only", "If you place a Breadcrumbs.Link directly in Breadcrumbs, the link will be displayed as a back button. It is important that you are consistent in your solution, and use either the back button or the path, not both."),
                    new DemoInfo(typeof(BreadcrumbsPathOnlyDemo), "Path only", "If you only put Breadcrumbs.List directly in Breadcrumbs, it will always be displayed as a path. This means that you will get the same display on both mobile and desktop."),
                    new DemoInfo(typeof(BreadcrumbsPathOrBackButtonOnlyDemo), "Path or back button based on screen width", "If you put both a Breadcrumbs.Link and a Breadcrumbs.List directly in Breadcrumbs, the back button will be displayed on screens narrower than 650px, and the path will be displayed on wider screens.")
                ],
                "https://designsystemet.no/en/components/docs/breadcrumbs/overview"),
            new ComponentInfo("button", "Button",
                typeof(Button),
                [
                    new DemoInfo(typeof(ButtonDemo)),
                    new DemoInfo(typeof(ButtonColorDemo)),
                    new DemoInfo(typeof(ButtonIconDemo)),
                    new DemoInfo(typeof(ButtonSizeDemo)),
                    new DemoInfo(typeof(ButtonVariantDemo)),
                    new DemoInfo(typeof(ButtonLoadingDemo)),
                    new DemoInfo(typeof(ButtonDisabledDemo))
                ],
                "https://designsystemet.no/en/components/docs/button/overview"),
            new ComponentInfo("card", "Card",
                typeof(Hviktor.Components.Card.Card),
                [
                    new DemoInfo(typeof(CardDemo)),
                    new DemoInfo(typeof(CardColorDemo), "Colors", "Cards come in all theme colours and have two variants.", Display: "flex-row"),
                    new DemoInfo(typeof(CardWithSectionsDemo), "With sections", "Use multiple Card.Block elements if you want to divide the card with separators or add images or video that extend to the edge. Note that when you use Card.Block, all content must be placed inside a Card.Block and not directly in the Card."),
                    new DemoInfo(typeof(CardLinkDemo), "Card with a link in the title", "If the card should link to another page, you can place a link in the card’s title.\nThe entire card then becomes clickable, but screen reader users get a better experience than if the whole card were a link (adrianroselli.com).\n\nIn this example, Card renders an <a> inside the heading element.\nThis is useful when Card contains text or media and should navigate to another page."),
                    new DemoInfo(typeof(CardAsLinkDemo), "Card as a link", "The entire card can become a link by using asChild. This is useful when you want all text and content in the Card to be read by screen readers as one continuous link.\n\nIn this example, Card renders as <a>.", IsNotSupported: true),
                    new DemoInfo(typeof(CardHorizontalDemo), "Horizontal", "You can switch between `display: flex` and `display: grid` to place `Card.Block` next to each other.\n**Note** that if you're using tailwind CSS you should set `grid! (display: grid !important)` for the horizontal to take effect.")
                ],
                "https://designsystemet.no/en/components/docs/card/overview"),
            new ComponentInfo("checkbox", "Checkbox",
                typeof(Checkbox),
                [
                    new DemoInfo(typeof(CheckboxDemo)),
                    new DemoInfo(typeof(CheckboxGroupingDemo), "Grouping", "Use `Fieldset` for grouping multiple options."),
                    new DemoInfo(typeof(CheckboxIndeterminateDemo), "Indeterminate checkbox", "Add allowIndeterminate: true to getCheckboxProps to create a parent Checkbox that can select or clear all options. This activates an additional state, indeterminate, next to checked and unchecked. It is displayed with a horizontal line when one or more Checkbox are selected in the group. It will be displayed as a regular Checkbox if everyone in the group has the same state."),
                    new DemoInfo(typeof(CheckboxReadOnlyDemo), "Read only", "Use the `readOnly` prop to prevent users from checking/unchecking a `readOnly` Checkbox. This is useful for disabling a Checkbox that is already checked, but should not be unchecked.", Display: "block"),
                    new DemoInfo(typeof(CheckboxDisabledDemo), "Disabled", "Avoid using if possible for accessibility purposes", Display: "block"),
                ],
                "https://designsystemet.no/en/components/docs/checkbox/overview"),
            new ComponentInfo("chip", "Chip",
                typeof(Hviktor.Components.Chip.Chip),
                [
                    new DemoInfo(typeof(ChipDemo))
                ],
                "https://designsystemet.no/en/components/docs/chip/overview",
                SubComponents:
                [
                    new SubComponentInfo(
                        "Chip.Radio",
                        typeof(Chip.Radio),
                        [
                            new DemoInfo(typeof(ChipRadioDemo), "")
                        ]),
                    new SubComponentInfo("Chip.Button", typeof(Chip.Button),
                    [
                        new DemoInfo(typeof(ChipButtonDemo), "Basic usage", "The Chip.Button component can be used to trigger an action when clicked. In this demo, we show a basic example of how to use the Chip.Button component."),
                        new DemoInfo(typeof(ChipButtonActionDemo), "Click action", "The `<Chip.Button>` component can be used to trigger an action when clicked.\nIn this demo, we increment a counter each time the button is clicked to demonstrate the functionality.")
                    ]),
                    new SubComponentInfo("Chip.Checkbox", typeof(Chip.Checkbox),
                    [
                        new DemoInfo(typeof(ChipCheckboxDemo), "Chip.Checkbox")
                    ]),
                    new SubComponentInfo("Chip.Removable", typeof(Chip.Removable),
                    [
                        new DemoInfo(typeof(ChipRemovableDemo), "Chip.Removable")
                    ]),
                ]),
            new ComponentInfo("code-block", "CodeBlock",
                typeof(Hviktor.Components.Code.CodeBlock),
                [
                    new DemoInfo(typeof(CodeBlockDemo)),
                    new DemoInfo(typeof(CodeBlockLineNumbersDemo), "With line numbers", "When you want to display line numbers, you can use the `lineNumbers` prop."),
                    new DemoInfo(typeof(CodeBlockNoHighlightDemo), "With line wrap & no highlighting", "By default, `CodeBlock` does not wrap long lines and highlights syntax. You can disable both of these features if you prefer."),
                    new DemoInfo(typeof(CodeBlockCSharpDemo), "C#"),
                    new DemoInfo(typeof(CodeBlockRazorDemo), "Razor"),
                    new DemoInfo(typeof(CodeBlockJavascriptDemo), "Javascript"),
                    new DemoInfo(typeof(CodeBlockCssDemo), "CSS (Cascading Style Sheets)", IsAiGenerated: true),
                    new DemoInfo(typeof(CodeBlockBashDemo), "Bash", IsAiGenerated: true),
                    new DemoInfo(typeof(CodeBlockSqlDemo), "SQL", IsAiGenerated: true),
                    new DemoInfo(typeof(CodeBlockJsonDemo), "JSON", IsAiGenerated: true),
                    new DemoInfo(typeof(CodeBlockXmlDemo), "XML", IsAiGenerated: true),
                    new DemoInfo(typeof(CodeBlockPowershellDemo), "PowerShell", IsAiGenerated: true),
                    new DemoInfo(typeof(CodeBlockYamlDemo), "YAML", IsAiGenerated: true),
                    new DemoInfo(typeof(CodeBlockMarkdownDemo), "Markdown")
                ],
                IsExperimental: true),
            new ComponentInfo("details", "Details",
                typeof(Hviktor.Components.Details.Details),
                [
                    new DemoInfo(typeof(DetailsDemo), Display: "block"),
                    new DemoInfo(typeof(DetailsFrameAndVariantDemo), "Frame and variant", "If you put `Details` as the only child of `Card`, you get a frame around the whole Details.\nYou can also use `variant=\"tinted\"` on either `Details` or `Card` to get a lighter background.", Display: "block"),
                    new DemoInfo(typeof(DetailsControlledDemo), "Controlled", "`Details` keeps track of whether it is open or closed, but this can also be controlled externally.\n**Note** that listening for `@onclick` events should be done on the `<Summary>` component if you want to control the `Details` open/close state using click events.", Display: "block"),
                ],
                "https://designsystemet.no/en/components/docs/details/overview",
                IsValidated: true),
            new ComponentInfo("dialog", "Dialog",
                typeof(Hviktor.Components.Dialog.Dialog),
                [
                    new DemoInfo(typeof(DialogDemo)),
                    new DemoInfo(typeof(DialogCommandDemo), "With command and without context", "If you don't want to use `Dialog.TriggerContext`, you can use `command=\"show-modal\"` or `command=\"--show-non-modal\"` and `commandfor=\"DIALOG-ID\"` to open the dialog from an external trigger."),
                    new DemoInfo(typeof(DialogRefDemo), "With ref and without context", "If you don't want to use `Dialog.TriggerContext`, you can use ref to open the dialog from an external trigger.\nYou then use native methods on the `<dialog>` element, such as `showModal()` or `show()`.\n**Note** that you should add `aria-haspopup=\"dialog\"` to the button that opens the dialog for better accessibility."),
                ],
                "https://designsystemet.no/en/components/docs/dialog/overview",
                SubComponents:
                [
                    new SubComponentInfo("Dialog.Block", typeof(Dialog.Block)),
                ]),
            new ComponentInfo("divider", "Divider",
                typeof(Divider),
                [new DemoInfo(typeof(DividerDemo))],
                "https://designsystemet.no/en/components/docs/divider/overview"),
            new ComponentInfo("dropdown", "Dropdown",
                typeof(Hviktor.Components.Dropdown.Dropdown),
                [
                    new DemoInfo(typeof(Documentation.Components.Demos.Dropdown.DropdownDemo)),
                    new DemoInfo(typeof(DropdownWithIconsDemo), "With icons", "You can use icons together with text in the dropdown items:"),
                    new DemoInfo(typeof(DropdownControlledDemo), "Controlled", "If you submit `open`, then you use `Dropdown` controlled. You can use `onClose` to get notified when `Dropdown` wants to close.\n**Note** that we do not use `@onclick` on the trigger, the dropdown handles this internally and sends to `onOpen` and `onClose`."),
                    new DemoInfo(typeof(DropdownWithoutTriggerContextDemo), "Without `TriggerContext`", "`Dropdown` uses the popover API, so you can use `Dropdown` without `Dropdown.Trigger`.\nYou must then add `popovertarget={id}` to `Dropdown`, and id to `Dropdown`.")
                ],
                "https://designsystemet.no/en/components/docs/dropdown/overview",
                SubComponents:
                [
                    new SubComponentInfo("Dropdown.TriggerContext", typeof(Dropdown.TriggerContext)),
                    new SubComponentInfo("Dropdown.Button", typeof(Dropdown.Button)),
                    new SubComponentInfo("Dropdown.Heading", typeof(Dropdown.Heading)),
                    new SubComponentInfo("Dropdown.Trigger", typeof(Dropdown.Trigger)),
                    new SubComponentInfo("Dropdown.List", typeof(Dropdown.List)),
                    new SubComponentInfo("Dropdown.Item", typeof(Dropdown.Item)),
                ]
            ),
            new ComponentInfo("error-summary", "ErrorSummary",
                typeof(Hviktor.Components.ErrorSummary.ErrorSummary),
                [
                    new DemoInfo(typeof(ErrorSummaryDemo)),
                    new DemoInfo(typeof(ErrorSummaryWithTextfieldsDemo), "Use with text fields", "In order for `ErrorSummary.Link` to navigate to the fields with errors, the fields must have a unique `id` that matches the `href` in the link."),
                    new DemoInfo(typeof(ErrorSummaryMovingFocusDemo), "Moving focus", "Below is an example where we move focus to `ErrorSummary` when it becomes visible."),
                ],
                "https://designsystemet.no/en/components/docs/error-summary/overview",
                SubComponents:
                [
                    new SubComponentInfo("ErrorSummary.Heading", typeof(ErrorSummary.Heading)),
                    new SubComponentInfo("ErrorSummary.List", typeof(ErrorSummary.List)),
                    new SubComponentInfo("ErrorSummary.Item", typeof(ErrorSummary.Item)),
                    new SubComponentInfo("ErrorSummary.Link", typeof(ErrorSummary.Link)),
                ]),
            new ComponentInfo("field", "Field",
                typeof(Hviktor.Components.Field.Field),
                [
                    new DemoInfo(typeof(FieldDemo), null, "It is common to get confused between `Fieldset` and `Field`. A good rule of thumb is that `Fieldset` is a set of `Field`"),
                    new DemoInfo(typeof(FieldPrefixSuffixDemo), "Prefix/Suffix", "Prefixes and suffixes are useful for displaying units, currency or other types of information relevant to the field. You should not use these on their own, as screen readers do not read them out. It is important that the same information displayed in the prefix or suffix is also included in the prompt."),
                    new DemoInfo(typeof(FieldNumberOfCharsDemo), "Number of characters", "Use `Field.Counter` to inform about the number of characters users can type in the field."),
                    new DemoInfo(typeof(FieldPositionDemo), "Position", "Use `position=\"end\"` when you want to place, for example, a `Switch` to the right of `Label`.\nYou can change the text by submitting props, which you can see at the top of the page. The language is standard Norwegian Bokmål"),
                ],
                "https://designsystemet.no/en/components/docs/field/overview",
                SubComponents:
                [
                    new SubComponentInfo("Field.Description", typeof(Field.Description)),
                    new SubComponentInfo("Field.Counter", typeof(Field.Counter))
                ]
            ),
            new ComponentInfo("fieldset", "Fieldset",
                typeof(Hviktor.Components.Fieldset.Fieldset),
                [
                    new DemoInfo(typeof(FieldsetDemo), null, "It is common to get confused between `Fieldset` and `Field`. A good rule of thumb is that `Fieldset` is a set of `Field`"),
                    new DemoInfo(typeof(FieldsetWithCheckboxDemo), "With checkbox", "Both `Checkbox` and `Radio` should be grouped in a `Fieldset`, even if there is only one element in the group."),
                    new DemoInfo(typeof(FieldsetMoreFieldsDemo), "More fields", "You can use `Fieldset` to group multiple `Field` components."),
                    new DemoInfo(typeof(FieldsetLegendAsHeadingDemo), "Legend as heading", "If it is a page that only has form on it, and the title of the page (`<h1>`) is the same as what will be in `<legend>`, you can put `<h1>` inside `Fieldset.Legend`."),
                ],
                "https://designsystemet.no/en/components/docs/fieldset/overview",
                SubComponents:
                [
                    new SubComponentInfo("Fieldset.Description", typeof(Fieldset.Description)),
                    new SubComponentInfo("Fieldset.Legend", typeof(Fieldset.Legend))
                ]),
            new ComponentInfo("input", "Input",
                typeof(Input),
                [
                    new DemoInfo(typeof(InputDemo)),
                    new DemoInfo(typeof(InputWithLabelDemo), "With label", "Inputs should always have a label for accessibility reasons. If you don't want to show the label, you can use `aria-label` or `aria-labelledby` to provide an accessible name for the input."),
                    new DemoInfo(typeof(InputWithErrorDemo), "With error", "To display an error message, you can use the `aria-invalid` html attribute. This will change the border color of the input to indicate that there is an error."),
                    new DemoInfo(typeof(InputDisabledDemo), "Disabled", "To disable an input, you can use the `disabled` html attribute. This will prevent the user from interacting with the input and will change the appearance to indicate that it is disabled."),
                    new DemoInfo(typeof(InputReadOnlyDemo), "ReadOnly", "To make an input read-only, you can use the `readOnly` html attribute. This will prevent the user from editing the input but will still allow them to interact with it.")
                ],
                "https://designsystemet.no/en/components/docs/input/overview"),
            new ComponentInfo("list", "List",
                typeof(ListBase),
                [new DemoInfo(typeof(ListDemo))],
                "https://designsystemet.no/en/components/docs/list/overview",
                SubComponents:
                [
                    new SubComponentInfo("List.Item", typeof(List.Item)),
                    new SubComponentInfo("List.Unordered", typeof(List.Unordered),
                    [
                        new DemoInfo(typeof(UnorderedListDemo), ""),
                    ]),
                    new SubComponentInfo("List.Ordered", typeof(List.Ordered),
                    [
                        new DemoInfo(typeof(OrderedListDemo), ""),
                    ]),
                ]),
            new ComponentInfo("loader", "Loader",
                typeof(Loader),
                [
                    new DemoInfo(typeof(LoaderDemo)),
                    new DemoInfo(typeof(LoaderColorDemo), "Color", "You can change the color of the loader by using the `color` prop.", Display: "flex-row"),
                    new DemoInfo(typeof(LoaderModalDemo)),
                ], IsExperimental: true),
            new ComponentInfo("link", "Link",
#pragma warning disable CS0618 // Type or member is obsolete
                typeof(Link), [],
#pragma warning restore CS0618 // Type or member is obsolete
                "https://designsystemet.no/en/components/docs/link/overview",
                IsDeprecated: true),
            new ComponentInfo("links", "Links",
                typeof(HyperLink), [],
                "https://designsystemet.no/en/components/docs/link/overview",
                SubComponents:
                [
                    new SubComponentInfo("HyperLink", typeof(HyperLink),
                    [
                        new DemoInfo(typeof(HyperLinkDemo), ""),
                        new DemoInfo(typeof(HyperLinkInTextDemo), "In text"),
                        new DemoInfo(typeof(HyperLinkWithIconDemo), "With icon"),
                        new DemoInfo(typeof(HyperLinkNeutralColorDemo), "Neutral color"),
                    ]),
                    new SubComponentInfo("NavigationLink", typeof(NavigationLink),
                    [
                        new DemoInfo(typeof(NavigationLinkDemo), ""),
                    ]),
                    new SubComponentInfo("ActionLink", typeof(ActionLink),
                    [
                        new DemoInfo(typeof(ActionLinkDemo), ""),
                    ]),
                ]),
            new ComponentInfo("markdown", "Markdown",
                typeof(Markdown),
                [
                    new DemoInfo(typeof(MarkdownDemo)),
                    new DemoInfo(typeof(MarkdownBlockQuotesDemo), "Block quotes", "You can use Markdown to render block quotes, which are useful for highlighting important information or quotes from users.", Display: "block"),
                    new DemoInfo(typeof(MarkdownCodeDemo), "Code", "You can use Markdown to render code snippets.", Display: "block"),
                    new DemoInfo(typeof(MarkdownEmphasisDemo), "Emphasis", "You can use Markdown to render text with different emphasis, such as bold, italic, strikethrough, and underline.", Display: "block"),
                    new DemoInfo(typeof(MarkdownHeadersDemo), "Headers", "You can use Markdown to render headers.", Display: "block"),
                    new DemoInfo(typeof(MarkdownImagesDemo), "Images", "You can use Markdown to render images.", Display: "block"),
                    new DemoInfo(typeof(MarkdownLinksDemo), "Links", "You can use Markdown to render links.", Display: "block"),
                    new DemoInfo(typeof(MarkdownListsDemo), "Lists", "You can use Markdown to render lists.", Display: "block"),
                    new DemoInfo(typeof(MarkdownTablesDemo), "Tables", "You can use Markdown to render tables.", Display: "block"),
                ],
                IsExperimental: true,
                SubComponents:
                [
                    new SubComponentInfo("Markdown Renderer", typeof(MarkdownRenderer)),
                ]),
            new ComponentInfo("pagination", "Pagination",
                typeof(Hviktor.Components.Pagination.Pagination),
                [
                    new DemoInfo(typeof(PaginationDemo), "Default", "The example shows `Pagination' with buttons that are not linked to any action. When you press, you will not navigate to another page."),
                    new DemoInfo(typeof(PaginationWithLinksDemo), "With links", "Below you see `Pagination` used with links.\n\nYou can use `asChild` to change `Pagination.Button` to `<a>`.", IsNotSupported: true),
                    new DemoInfo(typeof(PaginationMobileDemo), "Mobile", "There is no built-in mobile support in `Pagination`, but you can show fewer pages and remove text for the previous/next buttons on mobile devices."),
                ],
                SubComponents:
                [
                    new SubComponentInfo("Pagination.Button", typeof(Pagination.Button)),
                    new SubComponentInfo("Pagination.List", typeof(Pagination.List)),
                    new SubComponentInfo("Pagination.Item", typeof(Pagination.Item)),
                ]),
            new ComponentInfo("popover", "Popover",
                typeof(PopoverBase),
                [
                    new DemoInfo(typeof(PopoverDemo), "Standard"),
                    new DemoInfo(typeof(PopoverInlineDemo), "Inline trigger"),
                    new DemoInfo(typeof(PopoverControlledDemo), "Controlled", "You can control when the popover is opened or closed.\n\nNote that we do not use `@onclick` on the trigger, the dropdown handles this internally and sends to `onOpen` and `onClose`."),
                    new DemoInfo(typeof(PopoverWithoutContextDemo), "Without TriggerContext", "You can also use Popover without TriggerContext by using `popovertarget`:"),
                ],
                "https://designsystemet.no/en/components/docs/popover/overview",
                SubComponents:
                [
                    new SubComponentInfo("Popover.TriggerContext", typeof(Popover.TriggerContext)),
                    new SubComponentInfo("Popover.Trigger", typeof(Popover.Trigger)),
                ]),
            new ComponentInfo("radio", "Radio",
                typeof(Radio),
                [
                    new DemoInfo(typeof(RadioDemo)),
                    new DemoInfo(typeof(RadioGroupingDemo), "Grouping", "Use `Fieldset` around a group of radio buttons to provide a common prompt (`<legend>`) and optionally a description (`description`)."),
                    new DemoInfo(typeof(RadioWithErrorDemo), "With Error", "Here we must use `Fieldset`, because it activates the correct style and ensures that the content has the correct connections for accessibility."),
                    new DemoInfo(typeof(RadioReadOnlyDemo), "ReadOnly", "Fields with the `readonly` attribute are included in the tab order. Users can copy the content but not edit it. Information is included when the form is submitted.\n\n`Readonly` fields can be confusing for some users. Not everyone will understand why they cannot change the content of the field. We therefore recommend avoiding `readonly` as much as possible."),
                    new DemoInfo(typeof(RadioInlineDemo), "Inline", "Radio can be positioned horizontally with `flex`, but should generally be [placed vertically](https://designsystemet.no/en/components/docs/radio/overview#placement)."),
                ],
                "https://designsystemet.no/en/components/docs/radio/overview"),
            new ComponentInfo("required-tag", "RequiredTag",
                typeof(RequiredTag),
                [
                    new DemoInfo(typeof(RequiredTagDemo)),
                    new DemoInfo(typeof(RequiredTagAllDemo),
                        "When you only have required fields",
                        "If all fields on a page must be completed, you can clearly inform users at the top that all fields are required. In this case, there is no need to label each individual field. If there is only one field, this can be communicated through the question or supporting text.\n\nFor example, you can use a yellow tag as shown in the example below, or make this clear in the introductory text."),
                    new DemoInfo(typeof(RequiredTagComboDemo),
                        "When you have a combination of required and optional fields",
                        "As a general rule, we should only ask users for information we absolutely need and avoid optional fields. However, there are situations where optional fields cannot be avoided. If you must include optional fields alongside required ones, all fields should be labelled individually. In this case, there is no need to provide additional information at the top of the page.\n<ul class=\"ds-list\"><li>Clearly label each field as \"Required\" or \"Optional\" after the question or field label.</li><li>For example, use a yellow tag for \"Required\" and a blue tag for \"Optional\".</li><li>Do not use an asterisk to indicate required fields.</li></ul>"),
                ],
                "https://designsystemet.no/en/patterns/required-and-optional-fields"),
            new ComponentInfo("search", "Search",
                typeof(Hviktor.Components.Search.Search),
                [
                    new DemoInfo(typeof(SearchDemo)),
                    new DemoInfo(typeof(SearchVariantsDemo), "Variants"),
                    new DemoInfo(typeof(SearchWithLabelDemo), "With label"),
                    new DemoInfo(typeof(SearchFormDemo), "<form>")
                ],
                "https://designsystemet.no/en/components/docs/search/overview",
                SubComponents:
                [
                    new SubComponentInfo("Search.Input", typeof(Search.Input)),
                    new SubComponentInfo("Search.Button", typeof(Search.Button)),
                    new SubComponentInfo("Search.Clear", typeof(Search.Clear))
                ]),
            new ComponentInfo("select", "Select",
                typeof(Hviktor.Components.Select.Select),
                [
                    new DemoInfo(typeof(SelectDemo)),
                    new DemoInfo(typeof(SelectGroupingDemo), "Grouping", "You can use `Select.Optgroup` to group options into categories, making long lists more manageable and easier to navigate."),
                    new DemoInfo(typeof(SelectDisabledDemo), "Disabled"),
                    new DemoInfo(typeof(SelectReadOnlyDemo), "ReadOnly", "`<select>` is always `:read-only`, and does not support this in the same way as input types you can type into. If you set `aria-readonly` on a `<select>`, we ensure that it behaves as if it has read-only access, by preventing `@onkeydown` and `@onmousedown`. The user will still be able to focus on the element."),
                ],
                "https://designsystemet.no/en/components/docs/select/overview",
                SubComponents:
                [
                    new SubComponentInfo("Select.Optgroup", typeof(Select.Optgroup)),
                    new SubComponentInfo("Select.Option", typeof(Select.Option)),
                ]),
            new ComponentInfo("skeleton", "Skeleton",
                typeof(Skeleton),
                [
                    new DemoInfo(typeof(SkeletonDemo)),
                    new DemoInfo(typeof(SkeletonVariantsDemo), "Variants"),
                    new DemoInfo(typeof(SkeletonUsageExampleDemo), "Usage Example"),
                    new DemoInfo(typeof(SkeletonTextVariantDemo), "Text Variant")
                ],
                "https://designsystemet.no/en/components/docs/skeleton/overview"),
            new ComponentInfo("skip-link", "SkipLink",
                typeof(SkipLink),
                [
                    new DemoInfo(typeof(SkipLinkDemo))
                ],
                "https://designsystemet.no/en/components/docs/skip-link/overview"),
            new ComponentInfo("spinner", "Spinner",
                typeof(Spinner),
                [
                    new DemoInfo(typeof(SpinnerDemo)),
                    new DemoInfo(typeof(SpinnerSizesDemo)),
                ],
                "https://designsystemet.no/en/components/docs/spinner/overview"),
            new ComponentInfo("suggestion", "Suggestion",
                typeof(Hviktor.Components.Suggestion.Suggestion),
                [
                    new DemoInfo(typeof(SuggestionDemo)),
                    new DemoInfo(typeof(SuggestionMultipleChoiceDemo), "Multiple choice"),
                    new DemoInfo(typeof(SuggestionCustomFilteringDemo), "Custom filtering", "By default, `Suggestion` filters options based on whether the input value is a substring of the option label.\nIf you want to use a different filtering logic, you can provide your own filter function."),
                    new DemoInfo(typeof(SuggestionControlledMultipleChoiceDemo), "Controlled multiple choice"),
                    new DemoInfo(typeof(SuggestionAddingNewOptionsDemo), "Adding new options"),
                ],
                "https://designsystemet.no/en/components/docs/suggestion/overview",
                IsExperimental: true,
                SubComponents:
                [
                    new SubComponentInfo("Suggestion.Clear", typeof(Suggestion.Clear)),
                    new SubComponentInfo("Suggestion.Empty", typeof(Suggestion.Empty)),
                    new SubComponentInfo("Suggestion.Input", typeof(Suggestion.Input)),
                    new SubComponentInfo("Suggestion.List", typeof(Suggestion.List)),
                    new SubComponentInfo("Suggestion.Option", typeof(Suggestion.Option)),
                ]
            ),
            new ComponentInfo("switch", "Switch",
                typeof(Switch),
                [
                    new DemoInfo(typeof(SwitchDemo)),
                    new DemoInfo(typeof(SwitchGroupingDemo), "Grouping", "Use [Fieldset](https://designsystemet.no/en/components/docs/fieldset/code) to group multiple `Switch` components together."),
                    new DemoInfo(typeof(SwitchRightAlignedDemo), "Right-aligned", "Use `position=\"end\"` to position Switch on the right side of the prompt if you need it."),
                ],
                "https://designsystemet.no/en/components/docs/switch/overview"),
            new ComponentInfo("table", "Table",
                typeof(Hviktor.Components.Table.Table),
                [
                    new DemoInfo(typeof(TableDemo)),
                    new DemoInfo(typeof(TableZebraDemo), "Zebra"),
                    new DemoInfo(typeof(TableBorderDemo), "With border"),
                    new DemoInfo(
                        typeof(TableSortingDemo),
                        "Sorting",
                        "You can use props on `Table.HeaderCell` to show that the table can be sorted by the contents of a column.\n\nUse the sort prop to indicate the sort direction:\n<ul class=\"ds-list\"><li>\"none\" - no sorting</li><li>\"ascending\" - ascending order</li><li>\"descending\" - descending order</li><li>Use @onclick to handle what happens when the user clicks on the column header.</li></ul>"
                    ),
                    new DemoInfo(
                        typeof(TableFixedColumnWidthDemo),
                        "Fixed column width"
                        , "You can use `table-layout: fixed` to prevent column widths from changing when the table content is updated. This provides a smoother layout, and helps the browser draw the table faster.\n\nThis is especially useful in tables with pagination or other dynamically updated content, where stable column widths provide a better user experience."),
                ],
                "https://designsystemet.no/en/components/docs/table/overview",
                SubComponents:
                [
                    new SubComponentInfo("Table.Head", typeof(Table.Head)),
                    new SubComponentInfo("Table.HeaderCell", typeof(Table.HeaderCell)),
                    new SubComponentInfo("Table.Body", typeof(Table.Body)),
                    new SubComponentInfo("Table.Row", typeof(Table.Row)),
                    new SubComponentInfo("Table.Cell", typeof(Table.Cell)),
                    new SubComponentInfo("Table.Foot", typeof(Table.Foot)),
                ]),
            new ComponentInfo("tabs", "Tabs",
                typeof(Hviktor.Components.Tabs.Tabs),
                [
                    new DemoInfo(typeof(TabsDemo)),
                    new DemoInfo(typeof(TabsIconsOnlyDemo), "Icons only", "In the example below, we have added `Tooltip` around `Tabs.Tab` to provide descriptive text for the icons."),
                    new DemoInfo(typeof(TabsControlledDemo), "Controlled"),
                ],
                "https://designsystemet.no/en/components/docs/tabs/overview",
                SubComponents:
                [
                    new SubComponentInfo("Tabs.List", typeof(Tabs.List)),
                    new SubComponentInfo("Tabs.Panel", typeof(Tabs.Panel)),
                    new SubComponentInfo("Tabs.Tab", typeof(Tabs.Tab)),
                ]),
            new ComponentInfo("tag", "Tag",
                typeof(Tag),
                [
                    new DemoInfo(typeof(TagDemo)),
                    new DemoInfo(typeof(TagSizesDemo), "Sizes"),
                    new DemoInfo(typeof(TagIconsDemo), "Icons", "You must determine the spacing between text and icon yourself. We recommend that the `padding` on the `Tag` towards the outer edge is equal to the `margin` on the icon."),
                    new DemoInfo(typeof(TagVariantsDemo), "Variants", "You can set `variant=\"outline\"` to make the tag more visible on some background colors."),
                ],
                "https://designsystemet.no/en/components/docs/tag/overview"),
            new ComponentInfo("textarea", "Textarea",
                typeof(Textarea),
                [
                    new DemoInfo(typeof(TextareaDemo)),
                    new DemoInfo(typeof(TextareaWithRowsDemo), "Rows", "Use the `rows` attribute to set the number of visible rows in the text area. This only affects the initial height of the field."),
                    new DemoInfo(typeof(TextareaDisabledDemo), "Disabled", "You can set `disabled=\"true\"` to make the textarea disabled."),
                    new DemoInfo(typeof(TextareaReadOnlyDemo), "Readonly", "You can set `readonly=\"true\"` to make the textarea readonly."),
                ],
                "https://designsystemet.no/en/components/docs/textarea/overview"),
            new ComponentInfo("textfield", "Textfield",
                typeof(Textfield),
                [
                    new DemoInfo(typeof(TextfieldDemo)),
                    new DemoInfo(typeof(TextfieldWithRowsDemo), "With rows"),
                    new DemoInfo(typeof(TextfieldWithCounterDemo), "With counter"),
                    new DemoInfo(typeof(TextfieldWithPrefixAndSuffixDemo), "With prefix and suffix"),
                ],
                "https://designsystemet.no/en/components/docs/textfield/overview"),
            new ComponentInfo("toggle-group", "ToggleGroup",
                typeof(Hviktor.Components.ToggleGroup.ToggleGroup),
                [
                    new DemoInfo(typeof(ToggleGroupDemo)),
                    new DemoInfo(typeof(ToggleGroupIconsOnlyDemo), "Icons only", "Use `Tooltip` to explain the actions when using only icons."),
                    new DemoInfo(typeof(ToggleGroupControlledDemo), "Controlled"),
                    new DemoInfo(typeof(ToggleGroupSecondaryVariantDemo), "Secondary variant"),
                ],
                "https://designsystemet.no/en/components/docs/togglegroup/overview"),
            new ComponentInfo("tooltip", "Tooltip",
                typeof(Tooltip),
                [
                    new DemoInfo(typeof(TooltipDemo)),
                    new DemoInfo(typeof(TooltipWithTextDemo), "With text"),
                    new DemoInfo(typeof(TooltipPlacementDemo), "Placement", "Use the `placement` prop to adjust the placement of the `Tooltip`."),
                ],
                "https://designsystemet.no/en/components/docs/tooltip/overview")
        ];
    }

    /// <summary>Builds the typography components list.</summary>
    private static List<ComponentInfo> BuildTypography()
    {
        return
        [
            new ComponentInfo("heading", "Heading",
                typeof(Heading),
                [
                    new DemoInfo(typeof(HeadingDemo)),
                    new DemoInfo(typeof(HeadingSizesDemo), "Sizes", Display: "block"),
                ],
                "https://designsystemet.no/en/components/docs/heading/overview"),
            new ComponentInfo("label", "Label",
                typeof(Label),
                [
                    new DemoInfo(typeof(LabelDemo)),
                    new DemoInfo(typeof(LabelWeightsDemo), "Weights", Display: "flex-col"),
                ],
                "https://designsystemet.no/en/components/docs/label/overview"),
            new ComponentInfo("paragraph", "Paragraph",
                typeof(Paragraph),
                [
                    new DemoInfo(typeof(ParagraphDemo)),
                    new DemoInfo(typeof(ParagraphSizesDemo), "Sizes", Display: "block"),
                ],
                "https://designsystemet.no/en/components/docs/paragraph/overview"),
            new ComponentInfo("validation-message", "ValidationMessage",
                typeof(ValidationMessage),
                [
                    new DemoInfo(typeof(ValidationMessageDemo)),
                    new DemoInfo(typeof(ValidationMessageAllColorsDemo), Display: "block"),
                ],
                "https://designsystemet.no/en/components/docs/validation-message/overview")
        ];
    }

    /// <summary>Builds the visual components list.</summary>
    private static List<ComponentInfo> BuildVisuals()
    {
        return
        [
            new ComponentInfo("icon", "Icon",
                typeof(Icon),
                [
                    new DemoInfo(typeof(IconStrokeDemo)),
                    new DemoInfo(typeof(IconFillDemo)),
                    new DemoInfo(typeof(IconSizesDemo)),
                ]),
            new ComponentInfo("logo", "Logo",
                typeof(Logo),
                [
                    new DemoInfo(typeof(LogoDemo)),
                ],
                SubComponents:
                [
                    new SubComponentInfo("Helse Vest", typeof(LogoHveDemo),
                    [
                        new DemoInfo(typeof(LogoHveDemo), "", "Helse Vest is one of the four regional health authorities in Norway, responsible for providing healthcare services to the population in the western part of the country."),
                        new DemoInfo(typeof(LogoDotsDemo), "Dots", "The dots in the Helse Vest logo represent the different healthcare institutions that are part of Helse Norge. The light dot represents the region of the institution.\nHelse Vest dots has the light dot aligned to the far left."),
                        new DemoInfo(typeof(LogoHviktDemo), "Helse Vest IKT", "Helse Vest IKT is the IT department of Helse Vest."),
                        new DemoInfo(typeof(LogoSavDemo), "Sjukehusapoteka Vest", "Sjukehusapoteka Vest is a pharmaceutical company owned by Helse Vest that provides pharmaceuticals and related services to the hospitals in Helse Vest."),
                    ]),
                    new SubComponentInfo("Helse Bergen", typeof(LogoHbeDemo),
                    [
                        new DemoInfo(typeof(LogoHbeDemo), "", "Helse Bergen is a Norwegian healthcare provider based in Bergen, Norway."),
                        new DemoInfo(typeof(LogoHbeHusDemo), "Haukeland universitestssjukehus", "Haukeland universitetssjukehus, Helse Bergen, is a university hospital in Bergen, Norway.\nHelse Bergen is anchored by Haukeland universitetssjukehus."),
                    ]),
                    new SubComponentInfo("Helse Førde", typeof(LogoHfdDemo),
                    [
                        new DemoInfo(typeof(LogoHfdDemo), "", "Helse Førde is the largest Norwegian healthcare provider in Sogn og Fjordane."),
                    ]),
                    new SubComponentInfo("Helse Fonna", typeof(LogoHfoDemo),
                    [
                        new DemoInfo(typeof(LogoHfoDemo), "", "Helse Fonna is a Norwegian healthcare provider based in Haugesund, Norway."),
                    ]),
                    new SubComponentInfo("Helse Stavanger", typeof(LogoHstDemo),
                    [
                        new DemoInfo(typeof(LogoHstDemo), ""),
                        new DemoInfo(typeof(LogoHstSusDemo), "Stavanger universitestssjukehus",
                            "Stavanger universitetssjukehus, Helse Stavanger, is a university hospital in Stavanger, Norway.\nIt is the third largest university hospital in Norway, and has the fourth largest birth center in Norway."),
                    ]),
                ]),
            new ComponentInfo("hviktor-spinner", "Hviktor Spinner",
                null,
                [
                    new DemoInfo(typeof(HviktorLoaderDemo)),
                ],
                XmlDocSummary: "The `Hviktor Spinner` is an animated `SVG` that is used to present a loading state to users.\nIt is a visual indicator that an operation is in progress, and can help improve the user experience by providing feedback during potentially long-running tasks.\n\n**Note**: `Hviktor Loader` should not be used in production environments for unofficial Hviktor applications.",
                IsExperimental: true),
        ];
    }

    /// <summary>Enriches component items with XML documentation and changelog dates, then wraps them in a <see cref="ComponentGroup"/>.</summary>
    private static ComponentGroup BuildGroup(string title, List<ComponentInfo> items, IComponentMetadataService metadataService, IChangelogDateProvider changelogDates)
    {
        var enriched = new ComponentInfo[items.Count];
        for (var i = 0; i < items.Count; i++)
        {
            var c = items[i];

            // Resolve changelog date in a single pass, avoiding intermediate record copies
            var lastUpdated = ResolveLastUpdated(c.Slug, c.Title, changelogDates);

            // Build enrichment values from metadata
            ClassDocumentation? docs = null;
            string? xmlSummary = null;

            if (c.ComponentType is not null)
            {
                docs = metadataService.GetClassDocumentation(c.ComponentType);
                if (docs is null)
                {
                    xmlSummary = metadataService.GetClassSummary(c.ComponentType);
                }
            }

            // Create a single enriched copy only when needed
            enriched[i] = (docs, xmlSummary, lastUpdated) switch
            {
                (not null, _, _) => c with
                {
                    Documentation = docs,
                    XmlDocSummary = docs.Summary ?? c.XmlDocSummary,
                    LastUpdated = lastUpdated ?? c.LastUpdated
                },
                (null, not null, _) => c with
                {
                    XmlDocSummary = xmlSummary,
                    LastUpdated = lastUpdated ?? c.LastUpdated
                },
                (null, null, not null) => c with { LastUpdated = lastUpdated.Value },
                _ => c
            };
        }

        return new ComponentGroup(title, enriched);
    }

    /// <summary>
    /// Resolves the last-updated date from the changelog for a component.
    /// Tries the slug directly, then without hyphens, then the title.
    /// </summary>
    private static DateTime? ResolveLastUpdated(string slug, string title, IChangelogDateProvider changelogDates)
    {
        return changelogDates.GetLastUpdated(slug)
               ?? changelogDates.GetLastUpdated(slug.Replace("-", ""))
               ?? changelogDates.GetLastUpdated(title);
    }
}