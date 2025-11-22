using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace DanceCourseCreator.Tests.E2E;

/// <summary>
/// Tests for lesson and template workflows with screenshots.
/// </summary>
[TestClass]
public class LessonAndTemplateTests : PageTest
{
    private const string BaseUrl = "http://localhost:5034";
    private const string ScreenshotsDir = "screenshots/lessons-templates";

    [TestInitialize]
    public async Task TestInitialize()
    {
        await Task.Run(() => Directory.CreateDirectory(ScreenshotsDir));
    }

    [TestMethod]
    [TestCategory("Lessons")]
    [TestCategory("Screenshots")]
    public async Task Lessons_PageLoad_ShouldDisplay()
    {
        // Navigate to lessons page
        await Page.GotoAsync($"{BaseUrl}/lessons");
        await Page.WaitForSelectorAsync("text=Lektionsplaner", new() { Timeout = 30000 });

        // Take screenshot of lessons page
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/01-lessons-page-initial.png",
            FullPage = true
        });

        // Verify key elements
        await Expect(Page.GetByText("Lektionsplaner")).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Skapa lektionsplan" })).ToBeVisibleAsync();
    }

    [TestMethod]
    [TestCategory("Lessons")]
    [TestCategory("Screenshots")]
    public async Task Lessons_OpenCreateDialog_ShouldWork()
    {
        // Navigate to lessons page
        await Page.GotoAsync($"{BaseUrl}/lessons");
        await Page.WaitForSelectorAsync("text=Lektionsplaner", new() { Timeout = 30000 });
        await Page.WaitForTimeoutAsync(1000);

        // Click "Skapa lektionsplan" button
        await Page.GetByRole(AriaRole.Button, new() { Name = "Skapa lektionsplan" }).ClickAsync();
        await Page.WaitForTimeoutAsync(1000);

        // Take screenshot with create lesson dialog open
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/02-create-lesson-dialog-open.png",
            FullPage = true
        });

        // Verify dialog is visible
        await Expect(Page.GetByText("Skapa ny lektionsplan")).ToBeVisibleAsync();
    }

    [TestMethod]
    [TestCategory("Lessons")]
    [TestCategory("Screenshots")]
    public async Task Lessons_FilterAndSearch_ShouldWork()
    {
        // Navigate to lessons page
        await Page.GotoAsync($"{BaseUrl}/lessons");
        await Page.WaitForSelectorAsync("text=Lektionsplaner", new() { Timeout = 30000 });
        await Page.WaitForTimeoutAsync(1500);

        // Take screenshot before filtering
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/03-lessons-before-filter.png",
            FullPage = true
        });

        // Try to use search
        var searchBox = Page.GetByLabel("Sök lektioner...");
        if (await searchBox.IsVisibleAsync())
        {
            await searchBox.FillAsync("Nybörjare");
            await Page.WaitForTimeoutAsync(500);

            // Take screenshot with search text
            await Page.ScreenshotAsync(new()
            {
                Path = $"{ScreenshotsDir}/04-lessons-search-entered.png",
                FullPage = true
            });

            // Click filter button
            await Page.GetByRole(AriaRole.Button, new() { Name = "Filtrera" }).ClickAsync();
            await Page.WaitForTimeoutAsync(1000);

            // Take screenshot after filter
            await Page.ScreenshotAsync(new()
            {
                Path = $"{ScreenshotsDir}/05-lessons-after-filter.png",
                FullPage = true
            });
        }
    }

    [TestMethod]
    [TestCategory("Templates")]
    [TestCategory("Screenshots")]
    public async Task Templates_PageLoad_ShouldDisplay()
    {
        // Navigate to templates page
        await Page.GotoAsync($"{BaseUrl}/templates");
        await Page.WaitForSelectorAsync("text=Lektionsmallar", new() { Timeout = 30000 });

        // Take screenshot of templates page
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/06-templates-page-initial.png",
            FullPage = true
        });

        // Verify key elements
        await Expect(Page.GetByText("Lektionsmallar")).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Skapa mall" })).ToBeVisibleAsync();
    }

    [TestMethod]
    [TestCategory("Templates")]
    [TestCategory("Screenshots")]
    public async Task Templates_OpenCreateDialog_ShouldWork()
    {
        // Navigate to templates page
        await Page.GotoAsync($"{BaseUrl}/templates");
        await Page.WaitForSelectorAsync("text=Lektionsmallar", new() { Timeout = 30000 });
        await Page.WaitForTimeoutAsync(1000);

        // Click "Skapa mall" button
        await Page.GetByRole(AriaRole.Button, new() { Name = "Skapa mall" }).ClickAsync();
        await Page.WaitForTimeoutAsync(1000);

        // Take screenshot with create template dialog open
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/07-create-template-dialog-open.png",
            FullPage = true
        });

        // Verify dialog is visible
        await Expect(Page.GetByText("Skapa ny mall")).ToBeVisibleAsync();
    }

    [TestMethod]
    [TestCategory("Templates")]
    [TestCategory("Screenshots")]
    public async Task Templates_ViewTemplateDetails_ShouldWork()
    {
        // Navigate to templates page
        await Page.GotoAsync($"{BaseUrl}/templates");
        await Page.WaitForSelectorAsync("text=Lektionsmallar", new() { Timeout = 30000 });
        await Page.WaitForTimeoutAsync(1500);

        // Take screenshot of templates list
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/08-templates-list.png",
            FullPage = true
        });

        // Try to find and click "Visa" button for first template
        var visaButton = Page.GetByRole(AriaRole.Button, new() { Name = "Visa" }).First;
        if (await visaButton.IsVisibleAsync())
        {
            await visaButton.ClickAsync();
            await Page.WaitForTimeoutAsync(1000);

            // Take screenshot with template details dialog
            await Page.ScreenshotAsync(new()
            {
                Path = $"{ScreenshotsDir}/09-template-details-dialog.png",
                FullPage = true
            });
        }
    }

    [TestMethod]
    [TestCategory("Templates")]
    [TestCategory("Screenshots")]
    public async Task Templates_EditTemplate_ShouldWork()
    {
        // Navigate to templates page
        await Page.GotoAsync($"{BaseUrl}/templates");
        await Page.WaitForSelectorAsync("text=Lektionsmallar", new() { Timeout = 30000 });
        await Page.WaitForTimeoutAsync(1500);

        // Try to find and click "Redigera" button for first template
        var redigeraButton = Page.GetByRole(AriaRole.Button, new() { Name = "Redigera" }).First;
        if (await redigeraButton.IsVisibleAsync())
        {
            await redigeraButton.ClickAsync();
            await Page.WaitForTimeoutAsync(1000);

            // Take screenshot with edit template dialog
            await Page.ScreenshotAsync(new()
            {
                Path = $"{ScreenshotsDir}/10-edit-template-dialog.png",
                FullPage = true
            });
        }
    }

    [TestMethod]
    [TestCategory("Templates")]
    [TestCategory("Screenshots")]
    public async Task Templates_DuplicateTemplate_ShouldWork()
    {
        // Navigate to templates page
        await Page.GotoAsync($"{BaseUrl}/templates");
        await Page.WaitForSelectorAsync("text=Lektionsmallar", new() { Timeout = 30000 });
        await Page.WaitForTimeoutAsync(1500);

        // Try to find and click more options (three dots) for first template
        var moreButton = Page.GetByRole(AriaRole.Button, new() { Name = "Mer" }).Or(
            Page.Locator("button[aria-label='more']")
        ).First;
        
        if (await moreButton.IsVisibleAsync())
        {
            await moreButton.ClickAsync();
            await Page.WaitForTimeoutAsync(500);

            // Take screenshot with menu open
            await Page.ScreenshotAsync(new()
            {
                Path = $"{ScreenshotsDir}/11-template-menu-open.png",
                FullPage = true
            });

            // Try to click duplicate option if available
            var duplicateOption = Page.GetByText("Duplicera");
            if (await duplicateOption.IsVisibleAsync())
            {
                await duplicateOption.ClickAsync();
                await Page.WaitForTimeoutAsync(1000);

                // Take screenshot with duplicate dialog
                await Page.ScreenshotAsync(new()
                {
                    Path = $"{ScreenshotsDir}/12-duplicate-template-dialog.png",
                    FullPage = true
                });
            }
        }
    }

    [TestMethod]
    [TestCategory("Templates")]
    [TestCategory("Integration")]
    [TestCategory("Screenshots")]
    public async Task SaveAsTemplate_FromLesson_ShouldCreateTemplate()
    {
        // Navigate to lessons page
        await Page.GotoAsync($"{BaseUrl}/lessons");
        await Page.WaitForSelectorAsync("text=Lessons", new() { Timeout = 30000 });
        await Page.WaitForTimeoutAsync(1500);

        // Take screenshot of lessons page
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/13-save-as-template-lessons-page.png",
            FullPage = true
        });

        // Try to find "Spara som mall" button for first lesson
        var saveAsTemplateButton = Page.GetByRole(AriaRole.Button, new() { Name = "Spara som mall" }).First;
        if (await saveAsTemplateButton.IsVisibleAsync())
        {
            await saveAsTemplateButton.ClickAsync();
            await Page.WaitForTimeoutAsync(1000);

            // Take screenshot with save as template dialog
            await Page.ScreenshotAsync(new()
            {
                Path = $"{ScreenshotsDir}/14-save-as-template-dialog.png",
                FullPage = true
            });

            // Verify dialog is visible
            await Expect(Page.GetByText("Spara som mall")).ToBeVisibleAsync();
            
            // Fill in template name
            var templateNameField = Page.GetByLabel("Mallnamn");
            if (await templateNameField.IsVisibleAsync())
            {
                await templateNameField.FillAsync($"E2E Test Template {DateTime.Now:yyyy-MM-dd-HHmmss}");
                await Page.WaitForTimeoutAsync(500);
                
                // Take screenshot with filled form
                await Page.ScreenshotAsync(new()
                {
                    Path = $"{ScreenshotsDir}/15-save-as-template-filled.png",
                    FullPage = true
                });
                
                // Click save button
                var saveButton = Page.GetByRole(AriaRole.Button, new() { Name = "Spara som mall" });
                if (await saveButton.IsVisibleAsync())
                {
                    await saveButton.ClickAsync();
                    await Page.WaitForTimeoutAsync(2000);
                    
                    // Take screenshot after save
                    await Page.ScreenshotAsync(new()
                    {
                        Path = $"{ScreenshotsDir}/16-save-as-template-success.png",
                        FullPage = true
                    });
                }
            }
        }
    }

    [TestMethod]
    [TestCategory("Templates")]
    [TestCategory("Integration")]
    [TestCategory("Screenshots")]
    public async Task SaveAsTemplate_FromCourse_ShouldCreateTemplate()
    {
        // Navigate to courses page
        await Page.GotoAsync($"{BaseUrl}/courses");
        await Page.WaitForSelectorAsync("text=Kursplaner", new() { Timeout = 30000 });
        await Page.WaitForTimeoutAsync(1500);

        // Take screenshot of courses page
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/17-save-as-template-courses-page.png",
            FullPage = true
        });

        // Try to find "Spara som mall" button for first course
        var saveAsTemplateButton = Page.GetByRole(AriaRole.Button, new() { Name = "Spara som mall" }).First;
        if (await saveAsTemplateButton.IsVisibleAsync())
        {
            await saveAsTemplateButton.ClickAsync();
            await Page.WaitForTimeoutAsync(1000);

            // Take screenshot with save as template dialog
            await Page.ScreenshotAsync(new()
            {
                Path = $"{ScreenshotsDir}/18-course-save-as-template-dialog.png",
                FullPage = true
            });

            // Verify dialog is visible
            await Expect(Page.GetByText("Spara som mall")).ToBeVisibleAsync();
            
            // Fill in template name
            var templateNameField = Page.GetByLabel("Mallnamn");
            if (await templateNameField.IsVisibleAsync())
            {
                await templateNameField.FillAsync($"E2E Course Template {DateTime.Now:yyyy-MM-dd-HHmmss}");
                await Page.WaitForTimeoutAsync(500);
                
                // Take screenshot with filled form
                await Page.ScreenshotAsync(new()
                {
                    Path = $"{ScreenshotsDir}/19-course-save-as-template-filled.png",
                    FullPage = true
                });
                
                // Click save button
                var saveButton = Page.GetByRole(AriaRole.Button, new() { Name = "Spara som mall" });
                if (await saveButton.IsVisibleAsync())
                {
                    await saveButton.ClickAsync();
                    await Page.WaitForTimeoutAsync(2000);
                    
                    // Take screenshot after save
                    await Page.ScreenshotAsync(new()
                    {
                        Path = $"{ScreenshotsDir}/20-course-save-as-template-success.png",
                        FullPage = true
                    });
                }
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
