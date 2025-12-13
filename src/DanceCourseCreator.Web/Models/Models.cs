namespace DanceCourseCreator.Web.Models;

public class User
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? TeamId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class RegisterRequest
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = "instructor";
}

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public User User { get; set; } = new();
}

public class PatternOrExercise
{
    public string Id { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public List<string> Aliases { get; set; } = new();
    public string Level { get; set; } = string.Empty;
    public string DanceStyle { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Steps { get; set; } = new();
    public List<string> Counts { get; set; } = new();
    public List<string> Holds { get; set; } = new();
    public string Slot { get; set; } = string.Empty;
    public List<string> Rotations { get; set; } = new();
    public List<string> Prerequisites { get; set; } = new();
    public List<string> Related { get; set; } = new();
    public List<string> TeachingPoints { get; set; } = new();
    public List<string> CommonMistakes { get; set; } = new();
    public List<string> Variations { get; set; } = new();
    public int EstimatedMinutes { get; set; }
    public BpmRange BpmRange { get; set; } = new();
    public List<string> Tags { get; set; } = new();
    public List<string> MediaLinks { get; set; } = new();
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
}

public class BpmRange
{
    public int Min { get; set; }
    public int Max { get; set; }
}

public class CreatePatternRequest
{
    public string Type { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public List<string> Aliases { get; set; } = new();
    public string Level { get; set; } = string.Empty;
    public string DanceStyle { get; set; } = "WCS";
    public string Description { get; set; } = string.Empty;
    public List<string> Steps { get; set; } = new();
    public List<string> Counts { get; set; } = new();
    public List<string> Holds { get; set; } = new();
    public string Slot { get; set; } = string.Empty;
    public List<string> Rotations { get; set; } = new();
    public List<string> Prerequisites { get; set; } = new();
    public List<string> Related { get; set; } = new();
    public List<string> TeachingPoints { get; set; } = new();
    public List<string> CommonMistakes { get; set; } = new();
    public List<string> Variations { get; set; } = new();
    public int EstimatedMinutes { get; set; }
    public BpmRange BpmRange { get; set; } = new();
    public List<string> Tags { get; set; } = new();
    public List<string> MediaLinks { get; set; } = new();
}

public class LessonSection
{
    public string Id { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public List<string> Items { get; set; } = new();
    public string Notes { get; set; } = string.Empty;
    public int AllocatedMinutes { get; set; } = 0;
}

public class Lesson
{
    public string Id { get; set; } = string.Empty;
    public string? CourseId { get; set; }
    public DateTime? Date { get; set; }
    public int Duration { get; set; } // minutes
    public List<LessonSection> Sections { get; set; } = new();
    public int TotalEstimatedMinutes { get; set; }
    public string Notes { get; set; } = string.Empty;
    public int Version { get; set; } = 1;
    public string CreatedBy { get; set; } = string.Empty;
    public List<string> Reviewers { get; set; } = new();
    public List<string> History { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateLessonRequest
{
    public string? CourseId { get; set; }
    public DateTime? Date { get; set; }
    public int Duration { get; set; } = 75; // Default to 75 minutes
    public List<LessonSection> Sections { get; set; } = new();
    public string Notes { get; set; } = string.Empty;
}

public enum LessonSectionType
{
    Warmup,
    Technique,
    Patterns,
    Combination,
    Repetition,
    Social
}

public class Course
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public string DanceStyle { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int DurationWeeks { get; set; }
    public int PlannedLessonCount { get; set; }
    public List<string> Goals { get; set; } = new();
    public List<string> ThemesByWeek { get; set; } = new();
    public List<string> LessonIds { get; set; } = new();
    public string CoverageMetrics { get; set; } = "{}";
    public string RepetitionPlan { get; set; } = "{}";
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int ActualLessonCount { get; set; }
    public int TotalPlannedMinutes { get; set; }
}

public class CreateCourseRequest
{
    public string Name { get; set; } = string.Empty;
    public string Level { get; set; } = "beginner";
    public string DanceStyle { get; set; } = "WestCoastSwing";
    public string Type { get; set; } = "Weekly";
    public int DurationWeeks { get; set; } = 6;
    public int PlannedLessonCount { get; set; } = 6;
    public List<string> Goals { get; set; } = new();
    public List<string> ThemesByWeek { get; set; } = new();
}

// Template models
public class Template
{
    public string Id { get; set; } = string.Empty;
    public string Scope { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Owner { get; set; } = string.Empty;
    public string? Team { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateTemplateRequest
{
    public string Scope { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Team { get; set; }
}

public class UpdateTemplateRequest
{
    public string Name { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Team { get; set; }
}

public class DuplicateTemplateRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Team { get; set; }
    public string? ModifiedContent { get; set; }
}

public class TemplateListResponse
{
    public List<Template> Templates { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public bool HasNext { get; set; }
    public bool HasPrevious { get; set; }
}

public class DuplicateTemplateResponse
{
    public string CreatedResourceId { get; set; } = string.Empty;
    public string ResourceType { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
// Progression and Coverage models (Phase 1 - FR-020 to FR-023)

public class CourseCoverageMetrics
{
    public string CourseId { get; set; } = string.Empty;
    public string CourseName { get; set; } = string.Empty;
    public int DurationWeeks { get; set; }
    public Dictionary<string, SkillCoverage> FundamentalsCoverage { get; set; } = new();
    public List<WeekProgress> WeeklyProgress { get; set; } = new();
    public int TotalSkillsCovered { get; set; }
    public double CoveragePercentage { get; set; }
}

public class SkillCoverage
{
    public string SkillName { get; set; } = string.Empty;
    public bool IsCovered { get; set; }
    public List<int> WeeksIntroduced { get; set; } = new();
    public int RepetitionCount { get; set; }
}

public class WeekProgress
{
    public int WeekNumber { get; set; }
    public string Theme { get; set; } = string.Empty;
    public List<string> CoveredConcepts { get; set; } = new();
    public string LessonId { get; set; } = string.Empty;
    public int TotalMinutes { get; set; }
}

public class ProgressionAnalysis
{
    public string CourseId { get; set; } = string.Empty;
    public string CourseName { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public List<ProgressionWarning> Warnings { get; set; } = new();
    public double ProgressionScore { get; set; }
}

public class ProgressionWarning
{
    public string Severity { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int? WeekNumber { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Recommendation { get; set; } = string.Empty;
}

// Export models
public class ExportRequest
{
    public ExportFormat Format { get; set; } = ExportFormat.PDF;
    public bool IncludeSchedule { get; set; } = true;
    public bool IncludePatternDetails { get; set; } = true;
}

public enum ExportFormat
{
    PDF,
    Markdown
}
