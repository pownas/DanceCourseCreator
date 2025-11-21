using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace DanceCourseCreator.Tests.E2E;

/// <summary>
/// Demo screenshots showing the flexible planning features
/// </summary>
[TestClass]
public class DemoScreenshots : PageTest
{
    private const string BaseUrl = "http://localhost:5034";
    private const string ScreenshotsDir = "/tmp/demo-screenshots";

    [TestInitialize]
    public async Task TestInitialize()
    {
        await Task.Run(() => Directory.CreateDirectory(ScreenshotsDir));
    }

    [TestMethod]
    [TestCategory("Demo")]
    public async Task CaptureFlexiblePlanningFeatures()
    {
        // Navigate to courses page
        await Page.GotoAsync($"{BaseUrl}/courses");
        await Page.WaitForSelectorAsync("text=Kursplaner", new() { Timeout = 30000 });
        await Page.WaitForTimeoutAsync(2000);

        // Screenshot 1: Initial courses page
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/01-courses-page-initial.png",
            FullPage = true
        });

        // Click "Skapa kursplan" button
        await Page.GetByRole(AriaRole.Button, new() { Name = "Skapa kursplan" }).ClickAsync();
        await Page.WaitForTimeoutAsync(1500);

        // Screenshot 2: Create course dialog showing new fields
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/02-create-course-dialog-new-fields.png",
            FullPage = true
        });

        // Fill in course name
        await Page.GetByLabel("Kursnamn").FillAsync("Helgkurs - WCS Nybörjare");
        await Page.WaitForTimeoutAsync(500);

        // Select level
        var levelDropdown = Page.Locator("label:has-text('Nivå')").Locator("..").Locator("input").First;
        await levelDropdown.ClickAsync();
        await Page.WaitForTimeoutAsync(500);
        await Page.GetByRole(AriaRole.Option, new() { Name = "Nybörjare" }).ClickAsync();
        await Page.WaitForTimeoutAsync(500);

        // Select course type
        var typeDropdown = Page.Locator("label:has-text('Kurstyp')").Locator("..").Locator("input").First;
        await typeDropdown.ClickAsync();
        await Page.WaitForTimeoutAsync(500);

        // Screenshot 3: Course type dropdown showing options
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/03-course-type-dropdown.png",
            FullPage = true
        });

        await Page.GetByRole(AriaRole.Option, new() { Name = "Helgkurs (intensivkurs)" }).ClickAsync();
        await Page.WaitForTimeoutAsync(500);

        // Set lesson count
        await Page.GetByLabel("Antal lektioner (1-20)").FillAsync("4");
        await Page.WaitForTimeoutAsync(500);

        // Select duration
        var durationDropdown = Page.Locator("label:has-text('Kurslängd (veckor)')").Locator("..").Locator("input").First;
        await durationDropdown.ClickAsync();
        await Page.WaitForTimeoutAsync(500);
        await Page.GetByRole(AriaRole.Option, new() { Name = "1 vecka (intensiv/helg)" }).ClickAsync();
        await Page.WaitForTimeoutAsync(500);

        // Screenshot 4: Weekend course filled
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/04-weekend-course-filled.png",
            FullPage = true
        });

        // Save course
        await Page.GetByRole(AriaRole.Button, new() { Name = "Skapa" }).ClickAsync();
        await Page.WaitForTimeoutAsync(3000);

        // Screenshot 5: Course created showing summary
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/05-course-with-summary.png",
            FullPage = true
        });

        // Create a weekly course
        await Page.GetByRole(AriaRole.Button, new() { Name = "Skapa kursplan" }).ClickAsync();
        await Page.WaitForTimeoutAsync(1500);

        await Page.GetByLabel("Kursnamn").FillAsync("Veckokurs - WCS Medel");
        await Page.WaitForTimeoutAsync(500);

        var levelDropdown2 = Page.Locator("label:has-text('Nivå')").Locator("..").Locator("input").First;
        await levelDropdown2.ClickAsync();
        await Page.WaitForTimeoutAsync(500);
        await Page.GetByRole(AriaRole.Option, new() { Name = "Medel" }).ClickAsync();
        await Page.WaitForTimeoutAsync(500);

        var typeDropdown2 = Page.Locator("label:has-text('Kurstyp')").Locator("..").Locator("input").First;
        await typeDropdown2.ClickAsync();
        await Page.WaitForTimeoutAsync(500);
        await Page.GetByRole(AriaRole.Option, new() { Name = "Veckokurs (regelbunden träning)" }).ClickAsync();
        await Page.WaitForTimeoutAsync(500);

        await Page.GetByLabel("Antal lektioner (1-20)").FillAsync("10");
        await Page.WaitForTimeoutAsync(500);

        var durationDropdown2 = Page.Locator("label:has-text('Kurslängd (veckor)')").Locator("..").Locator("input").First;
        await durationDropdown2.ClickAsync();
        await Page.WaitForTimeoutAsync(500);
        await Page.GetByRole(AriaRole.Option, new() { Name = "10 veckor" }).ClickAsync();
        await Page.WaitForTimeoutAsync(500);

        // Screenshot 6: Weekly course filled
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/06-weekly-course-filled.png",
            FullPage = true
        });

        await Page.GetByRole(AriaRole.Button, new() { Name = "Skapa" }).ClickAsync();
        await Page.WaitForTimeoutAsync(3000);

        // Screenshot 7: Multiple courses with summaries
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/07-multiple-courses-with-summaries.png",
            FullPage = true
        });

        // Navigate to lessons
        await Page.GotoAsync($"{BaseUrl}/lessons");
        await Page.WaitForSelectorAsync("text=Lessons", new() { Timeout = 30000 });
        await Page.WaitForTimeoutAsync(2000);

        // Screenshot 8: Lessons page
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/08-lessons-page.png",
            FullPage = true
        });

        await Page.GetByRole(AriaRole.Button, new() { Name = "Create Lesson" }).ClickAsync();
        await Page.WaitForTimeoutAsync(1500);

        // Click duration dropdown
        var lessonDurationDropdown = Page.Locator("label:has-text('Lektionslängd')").Locator("..").Locator("input").First;
        await lessonDurationDropdown.ClickAsync();
        await Page.WaitForTimeoutAsync(500);

        // Screenshot 9: Lesson duration options (60-300 minutes)
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/09-lesson-duration-options.png",
            FullPage = true
        });
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
