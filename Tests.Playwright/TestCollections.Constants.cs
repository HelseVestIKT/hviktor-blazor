using Tests.Playwright.Fixtures;

namespace Tests.Playwright;

#region Collection Definitions

[CollectionDefinition(TestCollections.Compliance)]
public class ComplianceCollection : ICollectionFixture<TestsFixture>;

[CollectionDefinition(TestCollections.Prerequisite)]
public class PrerequisiteCollection : ICollectionFixture<TestsFixture>;

[CollectionDefinition(TestCollections.Composition)]
public class CompositionCollection : ICollectionFixture<TestsFixture>;

[CollectionDefinition(TestCollections.Performance, DisableParallelization = true)]
public class PerformanceCollection : ICollectionFixture<TestsFixture>;

[CollectionDefinition(TestCollections.Visual)]
public class VisualCollection : ICollectionFixture<TestsFixture>;

[CollectionDefinition(TestCollections.UserInteraction)]
public class UserInteractionCollection : ICollectionFixture<TestsFixture>;

#endregion

/// <summary>
/// Test collection names and traits for organizing Playwright tests.
/// </summary>
public static class TestCollections
{
    /// <summary>Basic health checks - run first, fast fail if infrastructure is broken</summary>
    public const string Prerequisite = "Prerequisite";

    public const string Compliance = "Compliance";
    public const string Composition = "Composition";
    public const string Performance = "Performance";
    public const string Visual = "Visual";
    public const string UserInteraction = "User Interaction";

    // Trait categories for filtering
    public static class Traits
    {
        public const string Collection = "Collection";
        public const string Category = "Category";
        public const string Tag = "Tags";
        public const string Component = "Component";
    }

    /// <summary>
    /// <b>Axe-core Tags</b>
    /// <br/><br/>
    /// Each rule in axe-core has a number of tags. These provide metadata about the rule. Each rule has one tag that indicates which WCAG version / level it belongs to, or if it doesn’t, it has the best-practice tag. If the rule is required by WCAG, there is a tag that references the success criterion number. For example, the wcag111 tag means a rule is required for WCAG 2 success criterion 1.1.1.
    /// <br/><br/>
    /// The experimental, ACT, TT, and section508 tags are only added to some rules. Each rule with a section508 tag also has a tag to indicate what requirement in old Section 508 the rule is required by. For example section508.22.a.
    /// </summary>
    /// <Docs>
    /// <see href="https://www.deque.com/axe/core-documentation/api-documentation#axecore-tags"/>
    /// </Docs>
    public static class Tags
    {
        /// <summary><b>Accessibility standard / purpose:</b><br/>Common accessibility best practices</summary>
        public const string BestPractice = "best-practice";

        /// <summary><b>Accessibility standard / purpose:</b><br/>Rule required under EN 301 549</summary>
        public const string En301549 = "EN-301-549";

        /// <summary><b>Accessibility standard / purpose:</b><br/>Rule required under RGAA</summary>
        public const string RgaAv4 = "RGAAv4";

        /// <summary><b>Accessibility standard / purpose:</b><br/>Tests all WCAG levels (A, AA, AAA) and best practices in a single run</summary>
        public const string Wcag = "wcag";

        /// <summary>Tests WCAG levels (A)</summary>
        public const string Wcag2a1 = "wcag2a";

        /// <summary>Tests WCAG levels (AA)</summary>
        public const string Wcag2a2 = "wcag2aa";

        /// <summary>Tests WCAG levels (AAA)</summary>
        public const string Wcag2a3 = "wcag2aaa";

        /// <summary><b>Accessibility standard / purpose:</b><br/>Cutting-edge rules, disabled by default</summary>
        public const string Experimental = "experimental";
    }

    /// <summary>
    /// <b>Axe-core Tags - Category names</b>
    /// <br/><br/>
    /// All rules have a cat.* tag, which indicates what type of content it is part of. The following cat.* tags exist in axe-core:
    /// </summary>
    /// <Docs>
    /// <see href="https://www.deque.com/axe/core-documentation/api-documentation#axecore-tags"/>
    /// </Docs>
    public static class Categories
    {
        public const string Aria = "cat.aria";
        public const string Color = "cat.color";
        public const string Forms = "cat.forms";
        public const string Wcag21A = "wcag21a";
        public const string Keyboard = "cat.keyboard";
        public const string Language = "cat.language";
        public const string NameRoleValue = "cat.name-role-value";
        public const string Parsing = "cat.parsing";
        public const string Semantics = "cat.semantics";
        public const string SensoryAndVisualCues = "cat.sensory-and-visual-cues";
        public const string Section508 = "section508";
        public const string Structure = "cat.structure";
        public const string Tables = "cat.tables";
        public const string TextAlternatives = "cat.text-alternatives";
        public const string TimeAndMedia = "cat.time-and-media";
    }
}