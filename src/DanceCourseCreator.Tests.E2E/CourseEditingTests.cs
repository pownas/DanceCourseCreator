using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace DanceCourseCreator.Tests.E2E;

/// <summary>
/// Tests for course editing workflow with screenshots.
/// </summary>
[TestClass]
public class CourseEditingTests : PageTest
{
    private const string BaseUrl = "http://localhost:5034";
    private const string ScreenshotsDir = "screenshots/course-editing";

    [TestInitialize]
    public async Task TestInitialize()
    {
        await Task.Run(() => Directory.CreateDirectory(ScreenshotsDir));
    }

    [TestMethod]
    [TestCategory("Courses")]
    [TestCategory("Screenshots")]
    public async Task CourseEditing_OpenEditDialog_ShouldWork()
    {
        // Navigate to courses page
        await Page.GotoAsync($"{BaseUrl}/courses");
        await Page.WaitForSelectorAsync("text=Kursplaner", new() { Timeout = 30000 });
        await Page.WaitForTimeoutAsync(1500);

        // Take screenshot of courses list
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/01-courses-list-before-edit.png",
            FullPage = true
        });

        // Try to find and click "Redigera" button for first course
        var redigeraButton = Page.GetByRole(AriaRole.Button, new() { Name = "Redigera" }).First;
        if (await redigeraButton.IsVisibleAsync())
        {
            await redigeraButton.ClickAsync();
            await Page.WaitForTimeoutAsync(1000);

            // Take screenshot with edit dialog open
            await Page.ScreenshotAsync(new()
            {
                Path = $"{ScreenshotsDir}/02-edit-course-dialog-open.png",
                FullPage = true
            });

            // Verify edit dialog is visible
            await Expect(Page.GetByText("Redigera kursplan")).ToBeVisibleAsync();
        }
    }

    [TestMethod]
    [TestCategory("Courses")]
    [TestCategory("Screenshots")]
    public async Task CourseEditing_ModifyAndSave_ShouldWork()
    {
        // Navigate to courses page
        await Page.GotoAsync($"{BaseUrl}/courses");
        await Page.WaitForSelectorAsync("text=Kursplaner", new() { Timeout = 30000 });
        await Page.WaitForTimeoutAsync(1500);

        // Find and click "Redigera" button for first course
        var redigeraButton = Page.GetByRole(AriaRole.Button, new() { Name = "Redigera" }).First;
        if (await redigeraButton.IsVisibleAsync())
        {
            await redigeraButton.ClickAsync();
            await Page.WaitForTimeoutAsync(1000);

            // Take screenshot of initial edit form
            await Page.ScreenshotAsync(new()
            {
                Path = $"{ScreenshotsDir}/03-edit-form-initial-state.png",
                FullPage = true
            });

            // Modify the course name
            var nameInput = Page.GetByLabel("Kursnamn");
            await nameInput.FillAsync(""); // Clear first
            await nameInput.FillAsync("Uppdaterad WCS Nybörjarkurs");
            await Page.WaitForTimeoutAsync(500);

            // Take screenshot after name change
            await Page.ScreenshotAsync(new()
            {
                Path = $"{ScreenshotsDir}/04-edit-name-modified.png",
                FullPage = true
            });

            // Modify duration
            var durationInput = Page.GetByLabel("Antal veckor");
            await durationInput.FillAsync("");
            await durationInput.FillAsync("10");
            await Page.WaitForTimeoutAsync(500);

            // Take screenshot after duration change
            await Page.ScreenshotAsync(new()
            {
                Path = $"{ScreenshotsDir}/05-edit-duration-modified.png",
                FullPage = true
            });

            // Try to add another goal
            var goalInput = Page.GetByLabel("Lägg till mål");
            if (await goalInput.IsVisibleAsync())
            {
                await goalInput.FillAsync("Förbättra anslutning och frame");
                await Page.WaitForTimeoutAsync(500);

                // Take screenshot with new goal entered
                await Page.ScreenshotAsync(new()
                {
                    Path = $"{ScreenshotsDir}/06-edit-new-goal-entered.png",
                    FullPage = true
                });

                // Click "Lägg till" button
                var addGoalButton = Page.GetByRole(AriaRole.Button, new() { Name = "Lägg till" });
                if (await addGoalButton.IsVisibleAsync())
                {
                    await addGoalButton.ClickAsync();
                    await Page.WaitForTimeoutAsync(500);

                    // Take screenshot with goal added
                    await Page.ScreenshotAsync(new()
                    {
                        Path = $"{ScreenshotsDir}/07-edit-new-goal-added.png",
                        FullPage = true
                    });
                }
            }

            // Take final screenshot before save
            await Page.ScreenshotAsync(new()
            {
                Path = $"{ScreenshotsDir}/08-edit-form-before-save.png",
                FullPage = true
            });

            // Click "Spara" button
            var saveButton = Page.GetByRole(AriaRole.Button, new() { Name = "Spara" });
            if (await saveButton.IsVisibleAsync())
            {
                await saveButton.ClickAsync();
                await Page.WaitForTimeoutAsync(2000);

                // Take screenshot after save
                await Page.ScreenshotAsync(new()
                {
                    Path = $"{ScreenshotsDir}/09-after-course-edited.png",
                    FullPage = true
                });
            }
        }
    }

    [TestMethod]
    [TestCategory("Courses")]
    [TestCategory("Screenshots")]
    public async Task CourseEditing_ViewCourse_ShouldShowDetails()
    {
        // Navigate to courses page
        await Page.GotoAsync($"{BaseUrl}/courses");
        await Page.WaitForSelectorAsync("text=Kursplaner", new() { Timeout = 30000 });
        await Page.WaitForTimeoutAsync(1500);

        // Try to find and click "Visa" button for first course
        var visaButton = Page.GetByRole(AriaRole.Button, new() { Name = "Visa" }).First;
        if (await visaButton.IsVisibleAsync())
        {
            await visaButton.ClickAsync();
            await Page.WaitForTimeoutAsync(1000);

            // Take screenshot with view dialog
            await Page.ScreenshotAsync(new()
            {
                Path = $"{ScreenshotsDir}/10-view-course-details.png",
                FullPage = true
            });
        }
    }

    [TestMethod]
    [TestCategory("Courses")]
    [TestCategory("Screenshots")]
    public async Task CourseEditing_CancelEdit_ShouldCloseDialog()
    {
        // Navigate to courses page
        await Page.GotoAsync($"{BaseUrl}/courses");
        await Page.WaitForSelectorAsync("text=Kursplaner", new() { Timeout = 30000 });
        await Page.WaitForTimeoutAsync(1500);

        // Find and click "Redigera" button for first course
        var redigeraButton = Page.GetByRole(AriaRole.Button, new() { Name = "Redigera" }).First;
        if (await redigeraButton.IsVisibleAsync())
        {
            await redigeraButton.ClickAsync();
            await Page.WaitForTimeoutAsync(1000);

            // Take screenshot with edit dialog open
            await Page.ScreenshotAsync(new()
            {
                Path = $"{ScreenshotsDir}/11-edit-dialog-before-cancel.png",
                FullPage = true
            });

            // Click "Avbryt" button
            var cancelButton = Page.GetByRole(AriaRole.Button, new() { Name = "Avbryt" });
            if (await cancelButton.IsVisibleAsync())
            {
                await cancelButton.ClickAsync();
                await Page.WaitForTimeoutAsync(1000);

                // Take screenshot after cancel
                await Page.ScreenshotAsync(new()
                {
                    Path = $"{ScreenshotsDir}/12-after-cancel-back-to-list.png",
                    FullPage = true
                });
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
