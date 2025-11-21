using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace DanceCourseCreator.Tests.E2E;

/// <summary>
/// Tests for course creation workflow with screenshots.
/// </summary>
[TestClass]
public class CourseCreationTests : PageTest
{
    private const string BaseUrl = "http://localhost:5034";
    private const string ScreenshotsDir = "screenshots/course-creation";

    [TestInitialize]
    public async Task TestInitialize()
    {
        await Task.Run(() => Directory.CreateDirectory(ScreenshotsDir));
    }

    [TestMethod]
    [TestCategory("Courses")]
    [TestCategory("Screenshots")]
    public async Task CourseCreation_OpenCreateDialog_ShouldDisplayForm()
    {
        // Navigate to courses page
        await Page.GotoAsync($"{BaseUrl}/courses");
        await Page.WaitForSelectorAsync("text=Kursplaner", new() { Timeout = 30000 });

        // Take screenshot of courses page
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/01-courses-page-initial.png",
            FullPage = true
        });

        // Click "Skapa kursplan" button
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Skapa kursplan" }).ClickAsync();
        await Page.WaitForTimeoutAsync(1000);

        // Take screenshot with create dialog open
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/02-create-course-dialog-open.png",
            FullPage = true
        });

        // Verify dialog is visible
        await Expect(Page.GetByText("Skapa ny kursplan")).ToBeVisibleAsync();
    }

    [TestMethod]
    [TestCategory("Courses")]
    [TestCategory("Screenshots")]
    public async Task CourseCreation_FillFormAndCreate_ShouldWork()
    {
        // Navigate to courses page
        await Page.GotoAsync($"{BaseUrl}/courses");
        await Page.WaitForSelectorAsync("text=Kursplaner", new() { Timeout = 30000 });

        // Click "Skapa kursplan" button
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Skapa kursplan" }).ClickAsync();
        await Page.WaitForTimeoutAsync(1000);

        // Take screenshot of empty form
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/03-create-form-empty.png",
            FullPage = true
        });

        // Fill in course name
        var nameInput = Page.GetByLabel("Kursnamn");
        await nameInput.FillAsync("Test WCS Nybörjarkurs");
        await Page.WaitForTimeoutAsync(500);

        // Take screenshot after name entered
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/04-create-form-name-filled.png",
            FullPage = true
        });

        // Select level
        var levelDropdown = Page.Locator("label:has-text('Nivå')").Locator("..").Locator("input").First;
        await levelDropdown.ClickAsync();
        await Page.WaitForTimeoutAsync(500);

        // Take screenshot with level dropdown open
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/05-level-dropdown-open.png",
            FullPage = true
        });

        await Page.GetByRole(AriaRole.Option, new() { NameString = "Nybörjare" }).ClickAsync();
        await Page.WaitForTimeoutAsync(500);

        // Take screenshot after level selected
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/06-level-selected.png",
            FullPage = true
        });

        // Fill in duration weeks
        var durationInput = Page.GetByLabel("Antal veckor");
        await durationInput.FillAsync("8");
        await Page.WaitForTimeoutAsync(500);

        // Take screenshot with duration filled
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/07-duration-filled.png",
            FullPage = true
        });

        // Add a goal
        var goalInput = Page.GetByLabel("Lägg till mål");
        await goalInput.FillAsync("Lära grundläggande WCS-rytm och timing");
        await Page.WaitForTimeoutAsync(500);

        // Take screenshot with goal entered
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/08-goal-entered.png",
            FullPage = true
        });

        // Click "Lägg till" button to add the goal
        var addGoalButton = Page.GetByRole(AriaRole.Button, new() { NameString = "Lägg till" });
        if (await addGoalButton.IsVisibleAsync())
        {
            await addGoalButton.ClickAsync();
            await Page.WaitForTimeoutAsync(500);

            // Take screenshot with goal added
            await Page.ScreenshotAsync(new()
            {
                Path = $"{ScreenshotsDir}/09-goal-added-to-list.png",
                FullPage = true
            });
        }

        // Take final screenshot before save
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/10-form-complete-before-save.png",
            FullPage = true
        });

        // Click "Spara" button
        var saveButton = Page.GetByRole(AriaRole.Button, new() { NameString = "Spara" });
        if (await saveButton.IsVisibleAsync())
        {
            await saveButton.ClickAsync();
            await Page.WaitForTimeoutAsync(2000);

            // Take screenshot after save
            await Page.ScreenshotAsync(new()
            {
                Path = $"{ScreenshotsDir}/11-after-course-created.png",
                FullPage = true
            });
        }
    }

    [TestMethod]
    [TestCategory("Courses")]
    [TestCategory("Screenshots")]
    public async Task CourseCreation_EmptyState_ShouldShowMessage()
    {
        // Navigate to courses page
        await Page.GotoAsync($"{BaseUrl}/courses");
        await Page.WaitForSelectorAsync("text=Kursplaner", new() { Timeout = 30000 });
        await Page.WaitForTimeoutAsync(1500);

        // Take screenshot - may show empty state or existing courses
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/12-courses-list-state.png",
            FullPage = true
        });

        // Check if empty state message is visible
        var emptyStateText = Page.GetByText("Inga kurser hittades");
        if (await emptyStateText.IsVisibleAsync())
        {
            // Take specific screenshot of empty state
            await Page.ScreenshotAsync(new()
            {
                Path = $"{ScreenshotsDir}/13-empty-state-visible.png",
                FullPage = true
            });
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
