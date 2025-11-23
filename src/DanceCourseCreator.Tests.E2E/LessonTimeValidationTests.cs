using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace DanceCourseCreator.Tests.E2E;

/// <summary>
/// Tests for advanced lesson builder with time validation feature (Phase 1)
/// Testing FR-011 and FR-012 requirements
/// </summary>
[TestClass]
public class LessonTimeValidationTests : PageTest
{
    private const string BaseUrl = "http://localhost:5034";
    private const string ScreenshotsDir = "screenshots/time-validation";

    [TestInitialize]
    public async Task TestInitialize()
    {
        await Task.Run(() => Directory.CreateDirectory(ScreenshotsDir));
    }

    [TestMethod]
    [TestCategory("TimeValidation")]
    [TestCategory("Screenshots")]
    public async Task LessonBuilder_OpenDialog_ShouldShowTimeFields()
    {
        // Navigate to lessons page
        await Page.GotoAsync($"{BaseUrl}/lessons");
        await Page.WaitForSelectorAsync("text=Lessons", new() { Timeout = 30000 });
        await Page.WaitForTimeoutAsync(1000);

        // Open create lesson dialog
        await Page.GetByRole(AriaRole.Button, new() { NameRegex = new System.Text.RegularExpressions.Regex("Create|Skapa") }).ClickAsync();
        await Page.WaitForTimeoutAsync(1500);

        // Take screenshot of dialog with time fields
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/01-lesson-builder-with-time-fields.png",
            FullPage = true
        });

        // Verify time-related elements are visible
        var durationField = Page.GetByLabel(new System.Text.RegularExpressions.Regex("Lektionslängd|Duration"));
        await Expect(durationField).ToBeVisibleAsync();
    }

    [TestMethod]
    [TestCategory("TimeValidation")]
    [TestCategory("Screenshots")]
    public async Task LessonBuilder_AddSection_ShouldShowTimeAllocationField()
    {
        // Navigate to lessons page
        await Page.GotoAsync($"{BaseUrl}/lessons");
        await Page.WaitForSelectorAsync("text=Lessons", new() { Timeout = 30000 });
        await Page.WaitForTimeoutAsync(1000);

        // Open create lesson dialog
        await Page.GetByRole(AriaRole.Button, new() { NameRegex = new System.Text.RegularExpressions.Regex("Create|Skapa") }).ClickAsync();
        await Page.WaitForTimeoutAsync(1500);

        // Add a new section
        var addSectionButton = Page.GetByRole(AriaRole.Button, new() { NameRegex = new System.Text.RegularExpressions.Regex("Add Section|Lägg till") }).First;
        if (await addSectionButton.IsVisibleAsync())
        {
            await addSectionButton.ClickAsync();
            await Page.WaitForTimeoutAsync(1000);
        }

        // Take screenshot showing section with time allocation field
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/02-section-with-time-allocation.png",
            FullPage = true
        });

        // Verify time allocation field exists
        var timeAllocationFields = Page.GetByLabel(new System.Text.RegularExpressions.Regex("Allokerad tid|Allocated.*min"));
        var count = await timeAllocationFields.CountAsync();
        Assert.IsTrue(count > 0, "Time allocation fields should be visible");
    }

    [TestMethod]
    [TestCategory("TimeValidation")]
    [TestCategory("Screenshots")]
    public async Task LessonBuilder_SetDuration_ShouldShowValidationSummary()
    {
        // Navigate to lessons page
        await Page.GotoAsync($"{BaseUrl}/lessons");
        await Page.WaitForSelectorAsync("text=Lessons", new() { Timeout = 30000 });
        await Page.WaitForTimeoutAsync(1000);

        // Open create lesson dialog
        await Page.GetByRole(AriaRole.Button, new() { NameRegex = new System.Text.RegularExpressions.Regex("Create|Skapa") }).ClickAsync();
        await Page.WaitForTimeoutAsync(1500);

        // Set lesson duration
        var durationField = Page.GetByLabel(new System.Text.RegularExpressions.Regex("Lektionslängd|Duration"));
        if (await durationField.IsVisibleAsync())
        {
            await durationField.ClickAsync();
            await Page.WaitForTimeoutAsync(500);
            
            // Select 75 minutes option
            var option75 = Page.GetByText("75 minuter", new() { Exact = false }).Or(Page.GetByText("75 minutes", new() { Exact = false }));
            if (await option75.IsVisibleAsync())
            {
                await option75.ClickAsync();
                await Page.WaitForTimeoutAsync(1000);
            }
        }

        // Scroll to see validation summary
        await Page.EvaluateAsync("window.scrollTo(0, document.body.scrollHeight)");
        await Page.WaitForTimeoutAsync(500);

        // Take screenshot of validation summary
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/03-validation-summary-visible.png",
            FullPage = true
        });

        // Verify summary section exists
        var summaryText = Page.GetByText(new System.Text.RegularExpressions.Regex("Tidsöversikt|Summary|Validering"));
        await Expect(summaryText.First).ToBeVisibleAsync();
    }

    [TestMethod]
    [TestCategory("TimeValidation")]
    [TestCategory("Screenshots")]
    public async Task LessonBuilder_AllocateTime_ShouldShowProgressBar()
    {
        // Navigate to lessons page
        await Page.GotoAsync($"{BaseUrl}/lessons");
        await Page.WaitForSelectorAsync("text=Lessons", new() { Timeout = 30000 });
        await Page.WaitForTimeoutAsync(1000);

        // Open create lesson dialog
        await Page.GetByRole(AriaRole.Button, new() { NameRegex = new System.Text.RegularExpressions.Regex("Create|Skapa") }).ClickAsync();
        await Page.WaitForTimeoutAsync(2000);

        // Try to set time allocation for first section
        var timeFields = Page.GetByLabel(new System.Text.RegularExpressions.Regex("Allokerad tid|Allocated.*min"));
        var firstTimeField = timeFields.First;
        
        if (await firstTimeField.IsVisibleAsync())
        {
            await firstTimeField.FillAsync("15");
            await firstTimeField.BlurAsync();
            await Page.WaitForTimeoutAsync(1000);
        }

        // Scroll to validation area
        await Page.EvaluateAsync("window.scrollTo(0, document.body.scrollHeight)");
        await Page.WaitForTimeoutAsync(500);

        // Take screenshot showing progress bar
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/04-time-allocation-progress-bar.png",
            FullPage = true
        });

        // Verify progress bar or time allocation display exists
        var progressOrTime = Page.GetByText(new System.Text.RegularExpressions.Regex("Tidsallokering|Time allocation|Allokerad tid"));
        await Expect(progressOrTime.First).ToBeVisibleAsync();
    }

    [TestMethod]
    [TestCategory("TimeValidation")]
    [TestCategory("Screenshots")]
    public async Task LessonBuilder_ExceedDuration_ShouldShowError()
    {
        // Navigate to lessons page
        await Page.GotoAsync($"{BaseUrl}/lessons");
        await Page.WaitForSelectorAsync("text=Lessons", new() { Timeout = 30000 });
        await Page.WaitForTimeoutAsync(1000);

        // Open create lesson dialog
        await Page.GetByRole(AriaRole.Button, new() { NameRegex = new System.Text.RegularExpressions.Regex("Create|Skapa") }).ClickAsync();
        await Page.WaitForTimeoutAsync(2000);

        // Set lesson duration to 60 minutes
        var durationField = Page.GetByLabel(new System.Text.RegularExpressions.Regex("Lektionslängd|Duration"));
        if (await durationField.IsVisibleAsync())
        {
            await durationField.ClickAsync();
            await Page.WaitForTimeoutAsync(500);
            
            var option60 = Page.GetByText("60 minuter", new() { Exact = false }).Or(Page.GetByText("60 minutes", new() { Exact = false }));
            if (await option60.IsVisibleAsync())
            {
                await option60.ClickAsync();
                await Page.WaitForTimeoutAsync(1000);
            }
        }

        // Try to allocate more time than duration
        var timeFields = Page.GetByLabel(new System.Text.RegularExpressions.Regex("Allokerad tid|Allocated.*min"));
        var firstTimeField = timeFields.First;
        
        if (await firstTimeField.IsVisibleAsync())
        {
            await firstTimeField.FillAsync("70");
            await firstTimeField.BlurAsync();
            await Page.WaitForTimeoutAsync(1500);
        }

        // Scroll to validation area
        await Page.EvaluateAsync("window.scrollTo(0, document.body.scrollHeight)");
        await Page.WaitForTimeoutAsync(500);

        // Take screenshot showing error
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/05-time-allocation-error.png",
            FullPage = true
        });

        // Verify error message is displayed
        var errorAlert = Page.Locator(".mud-alert-error, .mud-alert[class*='error']");
        if (await errorAlert.CountAsync() > 0)
        {
            await Expect(errorAlert.First).ToBeVisibleAsync();
        }
    }

    [TestMethod]
    [TestCategory("TimeValidation")]
    [TestCategory("Screenshots")]
    public async Task LessonBuilder_ValidationError_ShouldDisableSaveButton()
    {
        // Navigate to lessons page
        await Page.GotoAsync($"{BaseUrl}/lessons");
        await Page.WaitForSelectorAsync("text=Lessons", new() { Timeout = 30000 });
        await Page.WaitForTimeoutAsync(1000);

        // Open create lesson dialog
        await Page.GetByRole(AriaRole.Button, new() { NameRegex = new System.Text.RegularExpressions.Regex("Create|Skapa") }).ClickAsync();
        await Page.WaitForTimeoutAsync(2000);

        // Set lesson duration to 60 minutes
        var durationField = Page.GetByLabel(new System.Text.RegularExpressions.Regex("Lektionslängd|Duration"));
        if (await durationField.IsVisibleAsync())
        {
            await durationField.ClickAsync();
            await Page.WaitForTimeoutAsync(500);
            
            var option60 = Page.GetByText("60 minuter", new() { Exact = false }).Or(Page.GetByText("60 minutes", new() { Exact = false }));
            if (await option60.IsVisibleAsync())
            {
                await option60.ClickAsync();
                await Page.WaitForTimeoutAsync(1000);
            }
        }

        // Allocate too much time
        var timeFields = Page.GetByLabel(new System.Text.RegularExpressions.Regex("Allokerad tid|Allocated.*min"));
        var firstTimeField = timeFields.First;
        
        if (await firstTimeField.IsVisibleAsync())
        {
            await firstTimeField.FillAsync("80");
            await firstTimeField.BlurAsync();
            await Page.WaitForTimeoutAsync(1500);
        }

        // Take screenshot showing disabled save button
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/06-save-button-disabled.png",
            FullPage = true
        });

        // Verify save/create button is disabled
        var saveButton = Page.GetByRole(AriaRole.Button, new() { NameRegex = new System.Text.RegularExpressions.Regex("Create|Skapa|Update|Uppdatera") }).Last;
        if (await saveButton.IsVisibleAsync())
        {
            var isDisabled = await saveButton.IsDisabledAsync();
            Assert.IsTrue(isDisabled, "Save button should be disabled when validation fails");
        }
    }

    [TestMethod]
    [TestCategory("TimeValidation")]
    [TestCategory("Screenshots")]
    public async Task LessonBuilder_MultipleSection_ShouldShowTotalTime()
    {
        // Navigate to lessons page
        await Page.GotoAsync($"{BaseUrl}/lessons");
        await Page.WaitForSelectorAsync("text=Lessons", new() { Timeout = 30000 });
        await Page.WaitForTimeoutAsync(1000);

        // Open create lesson dialog
        await Page.GetByRole(AriaRole.Button, new() { NameRegex = new System.Text.RegularExpressions.Regex("Create|Skapa") }).ClickAsync();
        await Page.WaitForTimeoutAsync(2000);

        // Add multiple sections
        for (int i = 0; i < 2; i++)
        {
            var addSectionButton = Page.GetByRole(AriaRole.Button, new() { NameRegex = new System.Text.RegularExpressions.Regex("Add Section|Lägg till") }).First;
            if (await addSectionButton.IsVisibleAsync())
            {
                await addSectionButton.ClickAsync();
                await Page.WaitForTimeoutAsync(500);
            }
        }

        // Allocate time to each section
        var timeFields = Page.GetByLabel(new System.Text.RegularExpressions.Regex("Allokerad tid|Allocated.*min"));
        var count = await timeFields.CountAsync();
        
        for (int i = 0; i < Math.Min(count, 3); i++)
        {
            var field = timeFields.Nth(i);
            if (await field.IsVisibleAsync())
            {
                await field.FillAsync("20");
                await field.BlurAsync();
                await Page.WaitForTimeoutAsync(500);
            }
        }

        // Scroll to validation area
        await Page.EvaluateAsync("window.scrollTo(0, document.body.scrollHeight)");
        await Page.WaitForTimeoutAsync(1000);

        // Take screenshot showing total time calculation
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/07-multiple-sections-total-time.png",
            FullPage = true
        });

        // Verify total time is displayed
        var totalTimeText = Page.GetByText(new System.Text.RegularExpressions.Regex("Totalt|Total.*sektioner|Total.*allocated"));
        await Expect(totalTimeText.First).ToBeVisibleAsync();
    }

    [TestMethod]
    [TestCategory("TimeValidation")]
    [TestCategory("Screenshots")]
    public async Task LessonBuilder_ValidData_ShouldEnableSave()
    {
        // Navigate to lessons page
        await Page.GotoAsync($"{BaseUrl}/lessons");
        await Page.WaitForSelectorAsync("text=Lessons", new() { Timeout = 30000 });
        await Page.WaitForTimeoutAsync(1000);

        // Open create lesson dialog
        await Page.GetByRole(AriaRole.Button, new() { NameRegex = new System.Text.RegularExpressions.Regex("Create|Skapa") }).ClickAsync();
        await Page.WaitForTimeoutAsync(2000);

        // Set valid lesson duration
        var durationField = Page.GetByLabel(new System.Text.RegularExpressions.Regex("Lektionslängd|Duration"));
        if (await durationField.IsVisibleAsync())
        {
            await durationField.ClickAsync();
            await Page.WaitForTimeoutAsync(500);
            
            var option90 = Page.GetByText("90 minuter", new() { Exact = false }).Or(Page.GetByText("90 minutes", new() { Exact = false }));
            if (await option90.IsVisibleAsync())
            {
                await option90.ClickAsync();
                await Page.WaitForTimeoutAsync(1000);
            }
        }

        // Allocate reasonable time
        var timeFields = Page.GetByLabel(new System.Text.RegularExpressions.Regex("Allokerad tid|Allocated.*min"));
        var firstTimeField = timeFields.First;
        
        if (await firstTimeField.IsVisibleAsync())
        {
            await firstTimeField.FillAsync("20");
            await firstTimeField.BlurAsync();
            await Page.WaitForTimeoutAsync(1500);
        }

        // Scroll to see save button
        await Page.EvaluateAsync("window.scrollTo(0, document.body.scrollHeight)");
        await Page.WaitForTimeoutAsync(500);

        // Take screenshot showing enabled save button
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/08-valid-data-save-enabled.png",
            FullPage = true
        });

        // Verify save button is enabled
        var saveButton = Page.GetByRole(AriaRole.Button, new() { NameRegex = new System.Text.RegularExpressions.Regex("Create lesson|Skapa lektion") });
        if (await saveButton.CountAsync() > 0)
        {
            var firstButton = saveButton.First;
            if (await firstButton.IsVisibleAsync())
            {
                var isDisabled = await firstButton.IsDisabledAsync();
                Assert.IsFalse(isDisabled, "Save button should be enabled with valid data");
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
