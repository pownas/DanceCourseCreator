using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace DanceCourseCreator.Tests.E2E;

/// <summary>
/// Tests for home page and main navigation functionality with screenshots.
/// </summary>
[TestClass]
public class HomeAndNavigationTests : PageTest
{
    private const string BaseUrl = "http://localhost:5034";
    private const string ScreenshotsDir = "screenshots/navigation";

    [TestInitialize]
    public async Task TestInitialize()
    {
        await Task.Run(() => Directory.CreateDirectory(ScreenshotsDir));
    }

    [TestMethod]
    [TestCategory("Navigation")]
    [TestCategory("Screenshots")]
    public async Task HomePageLoad_ShouldDisplayCorrectContent()
    {
        // Navigate to home page
        await Page.GotoAsync(BaseUrl);
        await Page.WaitForSelectorAsync("text=Välkommen till WCS Kursskapare", new() { Timeout = 30000 });

        // Capture initial home page
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/01-home-page-initial.png",
            FullPage = true
        });

        // Verify key elements are visible
        await Expect(Page.GetByText("Välkommen till WCS Kursskapare")).ToBeVisibleAsync();
        await Expect(Page.GetByText("Biblioteksöversikt")).ToBeVisibleAsync();
        await Expect(Page.GetByText("Snabbåtgärder")).ToBeVisibleAsync();
    }

    [TestMethod]
    [TestCategory("Navigation")]
    [TestCategory("Screenshots")]
    public async Task Navigation_ThroughMainSections_ShouldWork()
    {
        // Start at home page
        await Page.GotoAsync(BaseUrl);
        await Page.WaitForSelectorAsync("text=Välkommen till WCS Kursskapare", new() { Timeout = 30000 });

        // Navigate to Patterns page
        await Page.GetByRole(AriaRole.Link, new() { Name = "Mönster" }).First.ClickAsync();
        await Page.WaitForSelectorAsync("text=Turbank", new() { Timeout = 10000 });
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/02-patterns-page.png",
            FullPage = true
        });
        await Expect(Page.GetByText("Turbank")).ToBeVisibleAsync();

        // Navigate to Lessons page
        await Page.GetByRole(AriaRole.Link, new() { Name = "Lektioner" }).First.ClickAsync();
        await Page.WaitForSelectorAsync("text=Lektionsplaner", new() { Timeout = 10000 });
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/03-lessons-page.png",
            FullPage = true
        });
        await Expect(Page.GetByText("Lektionsplaner")).ToBeVisibleAsync();

        // Navigate to Courses page
        await Page.GetByRole(AriaRole.Link, new() { Name = "Kurser" }).First.ClickAsync();
        await Page.WaitForSelectorAsync("text=Kursplaner", new() { Timeout = 10000 });
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/04-courses-page.png",
            FullPage = true
        });
        await Expect(Page.GetByText("Kursplaner")).ToBeVisibleAsync();

        // Navigate to Templates page
        await Page.GetByRole(AriaRole.Link, new() { Name = "Mallar" }).First.ClickAsync();
        await Page.WaitForSelectorAsync("text=Lektionsmallar", new() { Timeout = 10000 });
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/05-templates-page.png",
            FullPage = true
        });
        await Expect(Page.GetByText("Lektionsmallar")).ToBeVisibleAsync();

        // Navigate back to Home
        await Page.GetByRole(AriaRole.Link, new() { Name = "Hem" }).First.ClickAsync();
        await Page.WaitForSelectorAsync("text=Välkommen till WCS Kursskapare", new() { Timeout = 10000 });
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/06-back-to-home.png",
            FullPage = true
        });
        await Expect(Page.GetByText("Välkommen till WCS Kursskapare")).ToBeVisibleAsync();
    }

    [TestMethod]
    [TestCategory("Navigation")]
    [TestCategory("Screenshots")]
    public async Task QuickActions_OnHomePage_ShouldNavigate()
    {
        // Navigate to home page
        await Page.GotoAsync(BaseUrl);
        await Page.WaitForSelectorAsync("text=Välkommen till WCS Kursskapare", new() { Timeout = 30000 });

        // Take screenshot before clicking quick action
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/07-quick-actions-visible.png",
            FullPage = true
        });

        // Click "Kom igång" button for turbank
        var komIgangButton = Page.GetByRole(AriaRole.Button, new() { Name = "Kom igång" }).First;
        await komIgangButton.ClickAsync();
        
        // Wait for navigation to patterns page
        await Page.WaitForSelectorAsync("text=Turbank", new() { Timeout = 10000 });
        
        // Take screenshot after navigation
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/08-after-quick-action-click.png",
            FullPage = true
        });

        // Verify we're on the patterns page
        await Expect(Page).ToHaveURLAsync(new Regex(".*\\/patterns"));
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
