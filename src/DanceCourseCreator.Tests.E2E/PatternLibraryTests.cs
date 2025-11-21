using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace DanceCourseCreator.Tests.E2E;

/// <summary>
/// Tests for pattern library browsing and interaction with screenshots.
/// </summary>
[TestClass]
public class PatternLibraryTests : PageTest
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
    public async Task PatternLibrary_LoadAndBrowse_ShouldDisplayPatterns()
    {
        // Navigate to patterns page
        await Page.GotoAsync($"{BaseUrl}/patterns");
        await Page.WaitForSelectorAsync("text=Mönster- och övningsbibliotek", new() { Timeout = 30000 });

        // Capture initial patterns page
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/01-patterns-library-initial.png",
            FullPage = true
        });

        // Verify key elements
        await Expect(Page.GetByText("Mönster- och övningsbibliotek")).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Lägg till mönster" })).ToBeVisibleAsync();
    }

    [TestMethod]
    [TestCategory("Patterns")]
    [TestCategory("Screenshots")]
    public async Task PatternLibrary_FilterByType_ShouldWork()
    {
        // Navigate to patterns page
        await Page.GotoAsync($"{BaseUrl}/patterns");
        await Page.WaitForSelectorAsync("text=Mönster- och övningsbibliotek", new() { Timeout = 30000 });

        // Wait for patterns to load
        await Page.WaitForTimeoutAsync(1000);

        // Take screenshot before filtering
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/02-before-type-filter.png",
            FullPage = true
        });

        // Click on Type dropdown
        var typeDropdown = Page.Locator("label:has-text('Typ')").Locator("..").Locator("input").First;
        await typeDropdown.ClickAsync();
        await Page.WaitForTimeoutAsync(500);

        // Take screenshot with dropdown open
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/03-type-filter-dropdown-open.png",
            FullPage = true
        });

        // Select "Mönster" option
        await Page.GetByRole(AriaRole.Option, new() { Name = "Mönster" }).ClickAsync();
        await Page.WaitForTimeoutAsync(500);

        // Click filter button
        await Page.GetByRole(AriaRole.Button, new() { Name = "Filtrera" }).ClickAsync();
        await Page.WaitForTimeoutAsync(1000);

        // Take screenshot after filtering
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/04-after-type-filter-applied.png",
            FullPage = true
        });
    }

    [TestMethod]
    [TestCategory("Patterns")]
    [TestCategory("Screenshots")]
    public async Task PatternLibrary_FilterByLevel_ShouldWork()
    {
        // Navigate to patterns page
        await Page.GotoAsync($"{BaseUrl}/patterns");
        await Page.WaitForSelectorAsync("text=Mönster- och övningsbibliotek", new() { Timeout = 30000 });
        await Page.WaitForTimeoutAsync(1000);

        // Click on Level dropdown
        var levelDropdown = Page.Locator("label:has-text('Nivå')").Locator("..").Locator("input").First;
        await levelDropdown.ClickAsync();
        await Page.WaitForTimeoutAsync(500);

        // Take screenshot with dropdown open
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/05-level-filter-dropdown-open.png",
            FullPage = true
        });

        // Select "Nybörjare" option
        await Page.GetByRole(AriaRole.Option, new() { Name = "Nybörjare" }).ClickAsync();
        await Page.WaitForTimeoutAsync(500);

        // Click filter button
        await Page.GetByRole(AriaRole.Button, new() { Name = "Filtrera" }).ClickAsync();
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
    public async Task PatternLibrary_SearchPatterns_ShouldWork()
    {
        // Navigate to patterns page
        await Page.GotoAsync($"{BaseUrl}/patterns");
        await Page.WaitForSelectorAsync("text=Mönster- och övningsbibliotek", new() { Timeout = 30000 });
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
        await Page.WaitForTimeoutAsync(500);

        // Take screenshot with search text entered
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/08-search-text-entered.png",
            FullPage = true
        });

        // Click filter/search button
        await Page.GetByRole(AriaRole.Button, new() { Name = "Filtrera" }).ClickAsync();
        await Page.WaitForTimeoutAsync(1000);

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
    public async Task PatternLibrary_ViewPatternDetails_ShouldOpenDialog()
    {
        // Navigate to patterns page
        await Page.GotoAsync($"{BaseUrl}/patterns");
        await Page.WaitForSelectorAsync("text=Mönster- och övningsbibliotek", new() { Timeout = 30000 });
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
            var closeButton = Page.GetByRole(AriaRole.Button, new() { Name = "Stäng" }).Or(Page.Locator("button[aria-label='Close']"));
            if (await closeButton.IsVisibleAsync())
            {
                await closeButton.First.ClickAsync();
                await Page.WaitForTimeoutAsync(500);
            }
        }
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
