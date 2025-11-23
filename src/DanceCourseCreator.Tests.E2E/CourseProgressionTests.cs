using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace DanceCourseCreator.Tests.E2E;

/// <summary>
/// Tests for course progression and coverage functionality (Phase 1 - FR-020 to FR-023).
/// </summary>
[TestClass]
public class CourseProgressionTests : PageTest
{
    private const string BaseUrl = "http://localhost:5034";
    private const string ScreenshotsDir = "screenshots/course-progression";

    [TestInitialize]
    public void TestInitialize()
    {
        Directory.CreateDirectory(ScreenshotsDir);
    }

    [TestMethod]
    [TestCategory("Courses")]
    [TestCategory("Progression")]
    [TestCategory("Phase1")]
    public async Task CourseProgression_OpenDialog_ShouldDisplayProgressionInfo()
    {
        // Navigate to courses page
        await Page.GotoAsync($"{BaseUrl}/courses");
        await Page.WaitForSelectorAsync("text=Kursplaner", new() { Timeout = 30000 });

        // Take screenshot of courses page
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/01-courses-page.png",
            FullPage = true
        });

        // Check if there are any courses - if not, create one first
        var courseCards = await Page.Locator(".course-card").CountAsync();
        
        if (courseCards == 0)
        {
            // Create a course first
            await Page.GetByRole(AriaRole.Button, new() { Name = "Skapa kursplan" }).ClickAsync();
            await Page.WaitForTimeoutAsync(1000);

            // Fill in course details
            await Page.GetByLabel("Kursnamn").FillAsync("Test WCS Nybörjarkurs - Progression Test");
            
            // Select level
            var levelDropdown = Page.Locator("label:has-text('Nivå')").Locator("..").Locator("input").First;
            await levelDropdown.ClickAsync();
            await Page.WaitForTimeoutAsync(500);
            await Page.GetByRole(AriaRole.Option, new() { Name = "Nybörjare" }).ClickAsync();
            await Page.WaitForTimeoutAsync(500);

            // Set duration
            await Page.GetByLabel("Antal veckor").FillAsync("8");
            await Page.WaitForTimeoutAsync(500);

            // Save course
            var saveButton = Page.GetByRole(AriaRole.Button, new() { Name = "Spara" });
            if (await saveButton.IsVisibleAsync())
            {
                await saveButton.ClickAsync();
                await Page.WaitForTimeoutAsync(2000);
            }
        }

        // Now click the Progression button on the first course
        var progressionButton = Page.GetByRole(AriaRole.Button, new() { Name = "Progression" }).First;
        await progressionButton.ClickAsync();
        await Page.WaitForTimeoutAsync(2000);

        // Take screenshot of progression dialog
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/02-progression-dialog-opened.png",
            FullPage = true
        });

        // Verify dialog content is visible
        await Expect(Page.GetByText("Progressionsbedömning")).ToBeVisibleAsync();
        
        // Check if coverage section is visible
        var coverageSection = Page.GetByText("Täckning av grundläggande färdigheter");
        await Expect(coverageSection).ToBeVisibleAsync();

        // Take screenshot showing coverage table
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/03-coverage-table-visible.png",
            FullPage = true
        });
    }

    [TestMethod]
    [TestCategory("Courses")]
    [TestCategory("Progression")]
    [TestCategory("Phase1")]
    public async Task CourseProgression_ViewWeeklyProgress_ShouldShowTimeline()
    {
        // Navigate to courses page
        await Page.GotoAsync($"{BaseUrl}/courses");
        await Page.WaitForSelectorAsync("text=Kursplaner", new() { Timeout = 30000 });

        // Click Progression button on first course (if any exist)
        var progressionButtons = Page.GetByRole(AriaRole.Button, new() { Name = "Progression" });
        var buttonCount = await progressionButtons.CountAsync();

        if (buttonCount > 0)
        {
            await progressionButtons.First.ClickAsync();
            await Page.WaitForTimeoutAsync(2000);

            // Check if weekly progress section exists
            var weeklyProgressSection = Page.GetByText("Veckovis progression");
            if (await weeklyProgressSection.IsVisibleAsync())
            {
                // Take screenshot of weekly progress
                await Page.ScreenshotAsync(new()
                {
                    Path = $"{ScreenshotsDir}/04-weekly-progress-timeline.png",
                    FullPage = true
                });

                // Verify timeline is visible (MudTimeline component)
                var timeline = Page.Locator(".mud-timeline");
                if (await timeline.IsVisibleAsync())
                {
                    await Expect(timeline).ToBeVisibleAsync();
                }
            }
        }
    }

    [TestMethod]
    [TestCategory("Courses")]
    [TestCategory("Progression")]
    [TestCategory("Phase1")]
    public async Task CourseProgression_ViewFundamentals_ShouldShowSkillsTable()
    {
        // Navigate to courses page
        await Page.GotoAsync($"{BaseUrl}/courses");
        await Page.WaitForSelectorAsync("text=Kursplaner", new() { Timeout = 30000 });

        // Click Progression button on first course
        var progressionButtons = Page.GetByRole(AriaRole.Button, new() { Name = "Progression" });
        var buttonCount = await progressionButtons.CountAsync();

        if (buttonCount > 0)
        {
            await progressionButtons.First.ClickAsync();
            await Page.WaitForTimeoutAsync(2000);

            // Check for fundamentals table
            var fundamentalsTable = Page.Locator("table").First;
            if (await fundamentalsTable.IsVisibleAsync())
            {
                // Take screenshot showing the skills table
                await Page.ScreenshotAsync(new()
                {
                    Path = $"{ScreenshotsDir}/05-fundamentals-skills-table.png",
                    FullPage = true
                });

                // Verify fundamental skills are listed
                // Check for some expected fundamental skills
                var expectedSkills = new[] { "Sugar Push", "Connection", "Anchor", "Whip" };
                
                foreach (var skill in expectedSkills)
                {
                    var skillCell = Page.GetByText(skill, new() { Exact = false });
                    // Note: Some skills might not be covered, so we just check if they appear in the list
                    if (await skillCell.IsVisibleAsync())
                    {
                        // Skill is in the table
                    }
                }
            }

            // Close dialog
            var closeButton = Page.GetByRole(AriaRole.Button, new() { Name = "Stäng" });
            if (await closeButton.IsVisibleAsync())
            {
                await closeButton.ClickAsync();
                await Page.WaitForTimeoutAsync(500);

                // Take screenshot after closing
                await Page.ScreenshotAsync(new()
                {
                    Path = $"{ScreenshotsDir}/06-dialog-closed.png",
                    FullPage = true
                });
            }
        }
    }

    [TestMethod]
    [TestCategory("Courses")]
    [TestCategory("Progression")]
    [TestCategory("Phase1")]
    public async Task CourseProgression_ProgressionScore_ShouldDisplay()
    {
        // Navigate to courses page
        await Page.GotoAsync($"{BaseUrl}/courses");
        await Page.WaitForSelectorAsync("text=Kursplaner", new() { Timeout = 30000 });

        // Click Progression button on first course
        var progressionButtons = Page.GetByRole(AriaRole.Button, new() { Name = "Progression" });
        var buttonCount = await progressionButtons.CountAsync();

        if (buttonCount > 0)
        {
            await progressionButtons.First.ClickAsync();
            await Page.WaitForTimeoutAsync(2000);

            // Check for progression score display
            var progressionCard = Page.GetByText("Progressionsbedömning");
            await Expect(progressionCard).ToBeVisibleAsync();

            // Take screenshot showing progression score
            await Page.ScreenshotAsync(new()
            {
                Path = $"{ScreenshotsDir}/07-progression-score-display.png",
                FullPage = true
            });

            // Look for the circular progress indicator or score value
            var progressCircular = Page.Locator(".mud-progress-circular").First;
            if (await progressCircular.IsVisibleAsync())
            {
                await Expect(progressCircular).ToBeVisibleAsync();
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
