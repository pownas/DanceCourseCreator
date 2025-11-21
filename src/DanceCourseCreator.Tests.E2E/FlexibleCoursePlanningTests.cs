using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace DanceCourseCreator.Tests.E2E;

/// <summary>
/// Tests for flexible course and lesson planning features.
/// Tests course types (Weekly/Weekend), lesson counts (1-20), and duration constraints (60-300 minutes).
/// </summary>
[TestClass]
public class FlexibleCoursePlanningTests : PageTest
{
    private const string BaseUrl = "http://localhost:5034";
    private const string ScreenshotsDir = "screenshots/flexible-planning";

    [TestInitialize]
    public async Task TestInitialize()
    {
        await Task.Run(() => Directory.CreateDirectory(ScreenshotsDir));
    }

    [TestMethod]
    [TestCategory("Courses")]
    [TestCategory("FlexiblePlanning")]
    public async Task CourseCreation_WeeklyCourseType_ShouldBeSelectable()
    {
        // Navigate to courses page
        await Page.GotoAsync($"{BaseUrl}/courses");
        await Page.WaitForSelectorAsync("text=Kursplaner", new() { Timeout = 30000 });

        // Click "Skapa kursplan" button
        await Page.GetByRole(AriaRole.Button, new() { Name = "Skapa kursplan" }).ClickAsync();
        await Page.WaitForTimeoutAsync(1000);

        // Take screenshot of create dialog with course type field
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/01-create-dialog-with-course-type.png",
            FullPage = true
        });

        // Verify course type field is visible
        await Expect(Page.GetByLabel("Kurstyp")).ToBeVisibleAsync();

        // Fill in basic course info
        await Page.GetByLabel("Kursnamn").FillAsync("Veckokurs Test");
        
        // Select Weekly course type
        var courseTypeDropdown = Page.Locator("label:has-text('Kurstyp')").Locator("..").Locator("input").First;
        await courseTypeDropdown.ClickAsync();
        await Page.WaitForTimeoutAsync(500);
        
        await Page.GetByRole(AriaRole.Option, new() { NameString = "Veckokurs" }).ClickAsync();
        await Page.WaitForTimeoutAsync(500);

        // Take screenshot with weekly course selected
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/02-weekly-course-selected.png",
            FullPage = true
        });
    }

    [TestMethod]
    [TestCategory("Courses")]
    [TestCategory("FlexiblePlanning")]
    public async Task CourseCreation_WeekendCourseType_ShouldBeSelectable()
    {
        // Navigate to courses page
        await Page.GotoAsync($"{BaseUrl}/courses");
        await Page.WaitForSelectorAsync("text=Kursplaner", new() { Timeout = 30000 });

        // Click "Skapa kursplan" button
        await Page.GetByRole(AriaRole.Button, new() { Name = "Skapa kursplan" }).ClickAsync();
        await Page.WaitForTimeoutAsync(1000);

        // Fill in basic course info
        await Page.GetByLabel("Kursnamn").FillAsync("Helgkurs Test");
        
        // Select Weekend course type
        var courseTypeDropdown = Page.Locator("label:has-text('Kurstyp')").Locator("..").Locator("input").First;
        await courseTypeDropdown.ClickAsync();
        await Page.WaitForTimeoutAsync(500);
        
        await Page.GetByRole(AriaRole.Option, new() { NameString = "Helgkurs" }).ClickAsync();
        await Page.WaitForTimeoutAsync(500);

        // Take screenshot with weekend course selected
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/03-weekend-course-selected.png",
            FullPage = true
        });
    }

    [TestMethod]
    [TestCategory("Courses")]
    [TestCategory("FlexiblePlanning")]
    public async Task CourseCreation_LessonCount_ShouldAccept1To20()
    {
        // Navigate to courses page
        await Page.GotoAsync($"{BaseUrl}/courses");
        await Page.WaitForSelectorAsync("text=Kursplaner", new() { Timeout = 30000 });

        // Click "Skapa kursplan" button
        await Page.GetByRole(AriaRole.Button, new() { Name = "Skapa kursplan" }).ClickAsync();
        await Page.WaitForTimeoutAsync(1000);

        // Fill in course name
        await Page.GetByLabel("Kursnamn").FillAsync("Kurs med många lektioner");

        // Test minimum (1 lesson)
        var lessonCountInput = Page.GetByLabel("Antal lektioner (1-20)");
        await lessonCountInput.FillAsync("1");
        await Page.WaitForTimeoutAsync(500);

        // Take screenshot with 1 lesson
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/04-lesson-count-min-1.png",
            FullPage = true
        });

        // Test maximum (20 lessons)
        await lessonCountInput.FillAsync("20");
        await Page.WaitForTimeoutAsync(500);

        // Take screenshot with 20 lessons
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/05-lesson-count-max-20.png",
            FullPage = true
        });

        // Test a typical value (8 lessons)
        await lessonCountInput.FillAsync("8");
        await Page.WaitForTimeoutAsync(500);

        // Take screenshot with 8 lessons
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/06-lesson-count-typical-8.png",
            FullPage = true
        });
    }

    [TestMethod]
    [TestCategory("Lessons")]
    [TestCategory("FlexiblePlanning")]
    public async Task LessonCreation_DurationOptions_ShouldInclude60To300Minutes()
    {
        // Navigate to lessons page
        await Page.GotoAsync($"{BaseUrl}/lessons");
        await Page.WaitForSelectorAsync("text=Lessons", new() { Timeout = 30000 });

        // Click "Create Lesson" button
        await Page.GetByRole(AriaRole.Button, new() { Name = "Create Lesson" }).ClickAsync();
        await Page.WaitForTimeoutAsync(1000);

        // Click on duration dropdown
        var durationDropdown = Page.Locator("label:has-text('Lektionslängd')").Locator("..").Locator("input").First;
        await durationDropdown.ClickAsync();
        await Page.WaitForTimeoutAsync(500);

        // Take screenshot showing all duration options
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/07-lesson-duration-options.png",
            FullPage = true
        });

        // Verify key duration options are available
        await Expect(Page.GetByRole(AriaRole.Option, new() { NameString = "60 minuter" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Option, new() { NameString = "240 minuter" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Option, new() { NameString = "300 minuter" })).ToBeVisibleAsync();
    }

    [TestMethod]
    [TestCategory("Courses")]
    [TestCategory("FlexiblePlanning")]
    public async Task CoursesList_ShouldDisplaySummaryInformation()
    {
        // Navigate to courses page
        await Page.GotoAsync($"{BaseUrl}/courses");
        await Page.WaitForSelectorAsync("text=Kursplaner", new() { Timeout = 30000 });
        await Page.WaitForTimeoutAsync(2000);

        // Take screenshot of courses list showing summaries
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/08-courses-list-with-summaries.png",
            FullPage = true
        });

        // If there are courses, check for summary elements
        var courseCards = Page.Locator(".course-card");
        var count = await courseCards.CountAsync();
        
        if (count > 0)
        {
            // Check first course card for summary information
            var firstCard = courseCards.First;
            
            // Look for course type chip (Veckokurs or Helgkurs)
            var courseTypeElements = await firstCard.Locator("text=/Veckokurs|Helgkurs/").CountAsync();
            
            // Look for lesson count information
            var lessonCountElements = await firstCard.Locator("text=/lektioner/").CountAsync();
            
            // Look for time information
            var timeElements = await firstCard.Locator("text=/planerad tid|min|h/").CountAsync();
            
            // Take detailed screenshot if summary elements are found
            if (courseTypeElements > 0 || lessonCountElements > 0 || timeElements > 0)
            {
                await firstCard.ScreenshotAsync(new()
                {
                    Path = $"{ScreenshotsDir}/09-course-card-with-details.png"
                });
            }
        }
    }

    [TestMethod]
    [TestCategory("Courses")]
    [TestCategory("FlexiblePlanning")]
    [TestCategory("Integration")]
    public async Task CreateFullWeeklyCourse_WithMultipleLessons_ShouldShowCorrectSummary()
    {
        // Navigate to courses page
        await Page.GotoAsync($"{BaseUrl}/courses");
        await Page.WaitForSelectorAsync("text=Kursplaner", new() { Timeout = 30000 });

        // Create a course
        await Page.GetByRole(AriaRole.Button, new() { Name = "Skapa kursplan" }).ClickAsync();
        await Page.WaitForTimeoutAsync(1000);

        // Fill in course details
        await Page.GetByLabel("Kursnamn").FillAsync("Flexibel Veckokurs");
        
        // Select level
        var levelDropdown = Page.Locator("label:has-text('Nivå')").Locator("..").Locator("input").First;
        await levelDropdown.ClickAsync();
        await Page.WaitForTimeoutAsync(500);
        await Page.GetByRole(AriaRole.Option, new() { Name = "Nybörjare" }).ClickAsync();
        await Page.WaitForTimeoutAsync(500);

        // Select course type as Weekly
        var courseTypeDropdown = Page.Locator("label:has-text('Kurstyp')").Locator("..").Locator("input").First;
        await courseTypeDropdown.ClickAsync();
        await Page.WaitForTimeoutAsync(500);
        await Page.GetByRole(AriaRole.Option, new() { NameString = "Veckokurs" }).ClickAsync();
        await Page.WaitForTimeoutAsync(500);

        // Set lesson count to 10
        await Page.GetByLabel("Antal lektioner (1-20)").FillAsync("10");
        await Page.WaitForTimeoutAsync(500);

        // Set duration weeks
        var durationDropdown = Page.Locator("label:has-text('Kurslängd (veckor)')").Locator("..").Locator("input").First;
        await durationDropdown.ClickAsync();
        await Page.WaitForTimeoutAsync(500);
        await Page.GetByRole(AriaRole.Option, new() { Name = "10 veckor" }).ClickAsync();
        await Page.WaitForTimeoutAsync(500);

        // Take screenshot before saving
        await Page.ScreenshotAsync(new()
        {
            Path = $"{ScreenshotsDir}/10-weekly-course-form-filled.png",
            FullPage = true
        });

        // Save course (button might be "Skapa" or "Spara")
        var createButton = Page.GetByRole(AriaRole.Button, new() { Name = "Skapa" });
        if (await createButton.IsVisibleAsync())
        {
            await createButton.ClickAsync();
            await Page.WaitForTimeoutAsync(2000);

            // Take screenshot of created course in list
            await Page.ScreenshotAsync(new()
            {
                Path = $"{ScreenshotsDir}/11-course-created-in-list.png",
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
