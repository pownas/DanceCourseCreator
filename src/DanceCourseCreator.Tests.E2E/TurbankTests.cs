using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace DanceCourseCreator.Tests.E2E;

/// <summary>
/// Tests for turbank browsing and interaction with screenshots.
/// </summary>
[TestClass]
public class TurbankTests : PageTest
{
    private const string BaseUrl = "http://localhost:5034";
    private const string ScreenshotsDir = "screenshots/patterns";

    [TestInitialize]
    public async Task TestInitialize()
    {
        await Task.Run(() => Directory.CreateDirectory(ScreenshotsDir));
    }

    [TestMethod]
    [TestCategory("Patterns")]
    [TestCategory("Screenshots")]
    public async Task Turbank_LoadAndBrowse_ShouldDisplayPatterns()
    {
        await WaitForPatternsPageAsync();

        // Capture initial patterns page
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/01-patterns-library-initial.png",
            FullPage = true
        });

        // Verify key elements
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Turbank", Exact = true })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Lägg till mönster" })).ToBeVisibleAsync();
    }

    [TestMethod]
    [TestCategory("Patterns")]
    public async Task Turbank_CreatePattern_ShouldSaveAndShowItInLibrary()
    {
        await Page.GotoAsync($"{BaseUrl}/login");
        await Page.WaitForFunctionAsync(
            "() => document.querySelector('input[aria-required=\"true\"]')?.getAttributeNames().some(name => name.startsWith('_bl_'))");
        await Page.GetByLabel("E-post").FillAsync("instructor@dancecourse.com");
        await Page.GetByLabel("Lösenord").FillAsync("instructor123");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Logga in" }).ClickAsync();
        await Page.WaitForURLAsync($"{BaseUrl}/");

        await WaitForPatternsPageAsync();
        await Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Lägg till mönster" })).ToBeVisibleAsync();

        await Page.GetByRole(AriaRole.Button, new() { Name = "Lägg till mönster" }).ClickAsync();
        await Expect(Page.GetByText("Börja med namn, typ och nivå.", new() { Exact = false })).ToBeVisibleAsync();

        var patternName = $"E2E WCS Pattern {Guid.NewGuid():N}";
        await Page.GetByLabel("Namn").FillAsync(patternName);
        await Page.GetByLabel("Beskrivning").FillAsync("Ett testmönster för lektioner.");
        await Page.GetByLabel("Steg").FillAsync("Förberedelse\nPassering\nAnchor");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Lägg till", Exact = true }).ClickAsync();

        await Expect(Page.GetByText(patternName, new() { Exact = true })).ToBeVisibleAsync();
        await Expect(Page.GetByText("Ett testmönster för lektioner.", new() { Exact = true }).First).ToBeVisibleAsync();
    }

    [TestMethod]
    [TestCategory("Patterns")]
    [TestCategory("Screenshots")]
    public async Task Turbank_FilterByType_ShouldWork()
    {
        // Navigate to patterns page
        await WaitForPatternsPageAsync();

        // Wait for patterns to load
        await Page.WaitForTimeoutAsync(1000);

        // Take screenshot before filtering
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/02-before-type-filter.png",
            FullPage = true
        });

        // Click on Type dropdown
        await Page.GetByLabel("Typ").ClickAsync();
        await Page.WaitForTimeoutAsync(500);

        // Take screenshot with dropdown open
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/03-type-filter-dropdown-open.png",
            FullPage = true
        });

        // Select "Mönster" option
        await Page.GetByText("Mönster", new() { Exact = true }).ClickAsync();
        await Page.WaitForTimeoutAsync(500);

        // Click filter button
        await Page.GetByRole(AriaRole.Button, new() { Name = "Visa resultat" }).ClickAsync();
        await Page.WaitForTimeoutAsync(1000);
        await Expect(Page.GetByText("ÖVNING", new() { Exact = true })).ToHaveCountAsync(0);

        // Take screenshot after filtering
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/04-after-type-filter-applied.png",
            FullPage = true
        });

        await Page.GetByRole(AriaRole.Button, new() { Name = "Rensa filter" }).ClickAsync();
        await Expect(Page.GetByText("ÖVNING", new() { Exact = true }).First).ToBeVisibleAsync();
    }

    [TestMethod]
    [TestCategory("Patterns")]
    [TestCategory("Screenshots")]
    public async Task Turbank_FilterByLevel_ShouldWork()
    {
        // Navigate to patterns page
        await WaitForPatternsPageAsync();
        await Page.WaitForTimeoutAsync(1000);

        // Click on Level dropdown
        await Page.GetByLabel("Nivå").ClickAsync();
        await Page.WaitForTimeoutAsync(500);

        // Take screenshot with dropdown open
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/05-level-filter-dropdown-open.png",
            FullPage = true
        });

        // Select "Nybörjare" option
        await Page.GetByText("Nybörjare", new() { Exact = true }).ClickAsync();
        await Page.WaitForTimeoutAsync(500);

        // Click filter button
        await Page.GetByRole(AriaRole.Button, new() { Name = "Visa resultat" }).ClickAsync();
        await Page.WaitForTimeoutAsync(1000);

        // Take screenshot after filtering
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/06-after-level-filter-applied.png",
            FullPage = true
        });
    }

    [TestMethod]
    [TestCategory("Patterns")]
    [TestCategory("Screenshots")]
    public async Task Turbank_SearchPatterns_ShouldWork()
    {
        // Navigate to patterns page
        await WaitForPatternsPageAsync();
        await Page.WaitForTimeoutAsync(1000);

        // Take screenshot before search
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/07-before-search.png",
            FullPage = true
        });

        // Enter search text
        var searchBox = Page.GetByLabel("Sök mönster...");
        await searchBox.FillAsync("Push");

        // Take screenshot with search text entered
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/08-search-text-entered.png",
            FullPage = true
        });

        await searchBox.PressAsync("Enter");
        await Expect(Page.GetByText("Sugar Push", new() { Exact = true })).ToBeVisibleAsync();

        // Take screenshot after search
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/09-after-search-applied.png",
            FullPage = true
        });
    }

    [TestMethod]
    [TestCategory("Patterns")]
    [TestCategory("Screenshots")]
    public async Task Turbank_ViewPatternDetails_ShouldOpenDialog()
    {
        // Navigate to patterns page
        await WaitForPatternsPageAsync();
        await Page.WaitForTimeoutAsync(1500);

        // Take screenshot before clicking view
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/10-before-view-pattern.png",
            FullPage = true
        });

        // Try to find and click "Visa" button for first pattern
        var visaButton = Page.GetByRole(AriaRole.Button, new() { Name = "Visa" }).First;
        if (await visaButton.IsVisibleAsync())
        {
            await visaButton.ClickAsync();
            await Page.WaitForTimeoutAsync(1000);

            // Take screenshot with dialog open
            await Page.ScreenshotAsync(new()
            {
                Path = $"{ScreenshotsDir}/11-pattern-details-dialog.png",
                FullPage = true
            });

            // Close dialog
            var closeButton = Page.GetByRole(AriaRole.Button, new() { Name = "Stäng" })
                .Or(Page.Locator("button[aria-label='Close']"));
            if (await closeButton.IsVisibleAsync())
            {
                await closeButton.First.ClickAsync();
                await Page.WaitForTimeoutAsync(500);
            }
        }
    }

    private async Task WaitForPatternsPageAsync()
    {
        await Page.GotoAsync($"{BaseUrl}/patterns");
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Turbank", Exact = true })).ToBeVisibleAsync();
        await Page.WaitForFunctionAsync(
            "() => [...document.querySelectorAll('button')].some(button => [...button.attributes].some(attribute => attribute.name.startsWith('_bl_')))");
    }

    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions()
        {
            ViewportSize = new ViewportSize() { Width = 1920, Height = 1080 },
            IgnoreHTTPSErrors = true,
        };
    }
}
