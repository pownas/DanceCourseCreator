using DanceCourseCreator.API.Data;
using DanceCourseCreator.API.Models;
using DanceCourseCreator.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text;
using Xunit;

namespace DanceCourseCreator.API.Tests.Services;

public class ExportServiceTests
{
    private DanceCourseDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<DanceCourseDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new DanceCourseDbContext(options);
    }

    private ExportService CreateExportService(DanceCourseDbContext context)
    {
        var mockLogger = new Mock<ILogger<ExportService>>();
        return new ExportService(context, mockLogger.Object);
    }

    [Fact]
    public async Task ExportCourseToPdfAsync_WithValidCourse_ReturnsNonEmptyByteArray()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var service = CreateExportService(context);

        var course = new Course
        {
            Id = "test-course-1",
            Name = "Nybörjarkurs West Coast Swing",
            Level = DanceLevel.Beginner,
            DanceStyle = DanceStyle.WestCoastSwing,
            Type = CourseType.Weekly,
            DurationWeeks = 6,
            PlannedLessonCount = 6,
            Goals = new List<string> { "Lära grundsteg", "Förstå connection" },
            ThemesByWeek = new List<string> { "Grundsteg", "Anchor", "Sugar Push", "Side Pass", "Whip", "Kombination" },
            CreatedBy = "test-user"
        };

        var lesson = new Lesson
        {
            Id = "test-lesson-1",
            CourseId = course.Id,
            Date = DateTime.UtcNow,
            Duration = 75,
            Notes = "Första lektionen - fokus på grunderna",
            CreatedBy = "test-user"
        };

        var pattern = new PatternOrExercise
        {
            Id = "pattern-1",
            Name = "Sugar Push",
            Type = PatternType.Pattern,
            Level = DanceLevel.Beginner,
            DanceStyle = DanceStyle.WestCoastSwing,
            Description = "Grundläggande Sugar Push",
            EstimatedMinutes = 15,
            CreatedBy = "test-user"
        };

        context.Courses.Add(course);
        context.Patterns.Add(pattern);
        context.Lessons.Add(lesson);
        
        // Set sections after adding to avoid tracking issues
        lesson.Sections = new List<LessonSection>
        {
            new LessonSection
            {
                Id = "section-1",
                Type = LessonSectionType.Warmup,
                AllocatedMinutes = 10,
                Notes = "Uppvärmning och stretching"
            },
            new LessonSection
            {
                Id = "section-2",
                Type = LessonSectionType.Patterns,
                AllocatedMinutes = 30,
                Items = new List<string> { "pattern-1" },
                Notes = "Grundläggande steg"
            }
        };
        
        await context.SaveChangesAsync();

        // Act
        var result = await service.ExportCourseToPdfAsync(course.Id, includeSchedule: true, includePatternDetails: true);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        
        // PDF files start with "%PDF-"
        var header = Encoding.UTF8.GetString(result.Take(5).ToArray());
        Assert.StartsWith("%PDF-", header);
    }

    [Fact]
    public async Task ExportCourseToMarkdownAsync_WithValidCourse_ReturnsFormattedMarkdown()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var service = CreateExportService(context);

        var course = new Course
        {
            Id = "test-course-2",
            Name = "Improverkurs West Coast Swing",
            Level = DanceLevel.Improver,
            DanceStyle = DanceStyle.WestCoastSwing,
            Type = CourseType.Weekly,
            DurationWeeks = 8,
            PlannedLessonCount = 8,
            Goals = new List<string> { "Utveckla musikalitet", "Lära variationer" },
            ThemesByWeek = new List<string> { "Tema 1", "Tema 2", "Tema 3", "Tema 4", "Tema 5", "Tema 6", "Tema 7", "Tema 8" },
            CreatedBy = "test-user"
        };

        var lesson = new Lesson
        {
            Id = "test-lesson-2",
            CourseId = course.Id,
            Date = DateTime.UtcNow,
            Duration = 90,
            Notes = "Fokus på teknik",
            CreatedBy = "test-user"
        };

        context.Courses.Add(course);
        context.Lessons.Add(lesson);
        
        lesson.Sections = new List<LessonSection>
        {
            new LessonSection
            {
                Id = "section-3",
                Type = LessonSectionType.Technique,
                AllocatedMinutes = 20,
                Notes = "Connection och frame"
            }
        };
        
        await context.SaveChangesAsync();

        // Act
        var result = await service.ExportCourseToMarkdownAsync(course.Id, includeSchedule: true, includePatternDetails: true);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        
        // Check for expected markdown headers and content
        Assert.Contains("# Kursplan: Improverkurs West Coast Swing", result);
        Assert.Contains("## Kursinformation", result);
        Assert.Contains("## Mål", result);
        Assert.Contains("## Teman per vecka", result);
        Assert.Contains("## Lektioner", result);
        Assert.Contains("- **Nivå:** Improver", result);
        Assert.Contains("- **Dansstil:** WestCoastSwing", result);
        Assert.Contains("Utveckla musikalitet", result);
    }

    [Fact]
    public async Task ExportLessonToPdfAsync_WithValidLesson_ReturnsNonEmptyByteArray()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var service = CreateExportService(context);

        var lesson = new Lesson
        {
            Id = "test-lesson-3",
            Date = DateTime.UtcNow,
            Duration = 75,
            Notes = "Testlektion",
            CreatedBy = "test-user"
        };

        var pattern = new PatternOrExercise
        {
            Id = "pattern-2",
            Name = "Left Side Pass",
            Type = PatternType.Pattern,
            Level = DanceLevel.Beginner,
            DanceStyle = DanceStyle.WestCoastSwing,
            Description = "Left side pass med rätt frame",
            EstimatedMinutes = 10,
            TeachingPoints = new List<string> { "Håll frame", "Följ ledningen" },
            CommonMistakes = new List<string> { "För hårt grepp", "Glömmer anchor" },
            CreatedBy = "test-user"
        };

        context.Patterns.Add(pattern);
        context.Lessons.Add(lesson);
        
        lesson.Sections = new List<LessonSection>
        {
            new LessonSection
            {
                Id = "section-4",
                Type = LessonSectionType.Warmup,
                AllocatedMinutes = 10,
                Items = new List<string> { "pattern-2" },
                Notes = "Uppvärmning"
            }
        };
        
        await context.SaveChangesAsync();

        // Act
        var result = await service.ExportLessonToPdfAsync(lesson.Id, includePatternDetails: true);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        
        // PDF files start with "%PDF-"
        var header = Encoding.UTF8.GetString(result.Take(5).ToArray());
        Assert.StartsWith("%PDF-", header);
    }

    [Fact]
    public async Task ExportLessonToMarkdownAsync_WithValidLesson_ReturnsFormattedMarkdown()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var service = CreateExportService(context);

        var lesson = new Lesson
        {
            Id = "test-lesson-4",
            Date = DateTime.UtcNow,
            Duration = 90,
            Notes = "Avancerade tekniker",
            CreatedBy = "test-user"
        };

        var pattern = new PatternOrExercise
        {
            Id = "pattern-3",
            Name = "Whip",
            Type = PatternType.Pattern,
            Level = DanceLevel.Intermediate,
            DanceStyle = DanceStyle.WestCoastSwing,
            Description = "Klassisk whip",
            EstimatedMinutes = 20,
            Steps = new List<string> { "1&2", "3&4", "5&6", "7&8" },
            TeachingPoints = new List<string> { "Anchor point", "Connection", "Timing" },
            CreatedBy = "test-user"
        };

        context.Patterns.Add(pattern);
        context.Lessons.Add(lesson);
        
        lesson.Sections = new List<LessonSection>
        {
            new LessonSection
            {
                Id = "section-5",
                Type = LessonSectionType.Patterns,
                AllocatedMinutes = 40,
                Items = new List<string> { "pattern-3" },
                Notes = "Fokus på nya mönster"
            }
        };
        
        await context.SaveChangesAsync();

        // Act
        var result = await service.ExportLessonToMarkdownAsync(lesson.Id, includePatternDetails: true);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        
        // Check for expected markdown headers and content
        Assert.Contains("# Lektionsplan", result);
        Assert.Contains("## Sektioner", result);
        Assert.Contains("### Patterns", result);
        Assert.Contains("#### Whip", result);
        Assert.Contains("**Nivå:** Intermediate", result);
        Assert.Contains("**Typ:** Pattern", result);
        Assert.Contains("Klassisk whip", result);
        Assert.Contains("**Undervisningspunkter:**", result);
    }

    [Fact]
    public async Task ExportCourseToPdfAsync_WithInvalidCourseId_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var service = CreateExportService(context);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await service.ExportCourseToPdfAsync("non-existent-course"));
    }

    [Fact]
    public async Task ExportLessonToPdfAsync_WithInvalidLessonId_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var service = CreateExportService(context);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await service.ExportLessonToPdfAsync("non-existent-lesson"));
    }

    [Fact]
    public async Task ExportCourseToMarkdownAsync_WithoutPatternDetails_ExcludesPatternDetails()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var service = CreateExportService(context);

        var course = new Course
        {
            Id = "test-course-3",
            Name = "Test Course",
            Level = DanceLevel.Beginner,
            DanceStyle = DanceStyle.WestCoastSwing,
            Type = CourseType.Weekly,
            DurationWeeks = 4,
            PlannedLessonCount = 4,
            CreatedBy = "test-user"
        };

        var lesson = new Lesson
        {
            Id = "test-lesson-5",
            CourseId = course.Id,
            Date = DateTime.UtcNow,
            Duration = 60,
            CreatedBy = "test-user"
        };

        var pattern = new PatternOrExercise
        {
            Id = "pattern-4",
            Name = "Test Pattern",
            Type = PatternType.Pattern,
            Level = DanceLevel.Beginner,
            DanceStyle = DanceStyle.WestCoastSwing,
            Description = "This should not appear when includePatternDetails is false",
            EstimatedMinutes = 15,
            TeachingPoints = new List<string> { "This should not appear" },
            CreatedBy = "test-user"
        };

        context.Courses.Add(course);
        context.Patterns.Add(pattern);
        context.Lessons.Add(lesson);
        
        lesson.Sections = new List<LessonSection>
        {
            new LessonSection
            {
                Id = "section-6",
                Type = LessonSectionType.Patterns,
                AllocatedMinutes = 30,
                Items = new List<string> { "pattern-4" }
            }
        };
        
        await context.SaveChangesAsync();

        // Act
        var result = await service.ExportCourseToMarkdownAsync(course.Id, includeSchedule: true, includePatternDetails: false);

        // Assert
        Assert.NotNull(result);
        Assert.Contains("Test Pattern", result); // Pattern name should appear
        Assert.DoesNotContain("This should not appear", result); // But details should not
    }
}
