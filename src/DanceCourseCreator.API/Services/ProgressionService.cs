using DanceCourseCreator.API.Data;
using DanceCourseCreator.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DanceCourseCreator.API.Services;

/// <summary>
/// Service for analyzing course progression and coverage metrics
/// </summary>
public class ProgressionService
{
    private readonly DanceCourseDbContext _context;

    // Fundamental skills to track for WCS (from FR-021)
    private static readonly string[] FundamentalSkills = new[]
    {
        "Sugar Push",
        "Left Side Pass",
        "Right Side Pass",
        "Whip",
        "Connection",
        "Anchor",
        "Stretch",
        "Musicality"
    };

    public ProgressionService(DanceCourseDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Calculate coverage metrics for a course based on its lessons and patterns
    /// </summary>
    public async Task<CourseCoverageMetrics> CalculateCoverageAsync(string courseId)
    {
        var course = await _context.Courses
            .Include(c => c.Lessons)
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null)
        {
            throw new InvalidOperationException($"Course {courseId} not found");
        }

        var metrics = new CourseCoverageMetrics
        {
            CourseId = courseId,
            CourseName = course.Name,
            DurationWeeks = course.DurationWeeks
        };

        // Initialize coverage for all fundamental skills
        foreach (var skill in FundamentalSkills)
        {
            metrics.FundamentalsCoverage[skill] = new SkillCoverage
            {
                SkillName = skill,
                IsCovered = false,
                WeeksIntroduced = new List<int>(),
                RepetitionCount = 0
            };
        }

        // Analyze each lesson and its patterns
        var weekNumber = 1;
        foreach (var lesson in course.Lessons.OrderBy(l => l.CreatedAt))
        {
            if (weekNumber > course.DurationWeeks) break;

            var sections = lesson.Sections;
            var weekConcepts = new List<string>();

            foreach (var section in sections)
            {
                foreach (var itemId in section.Items)
                {
                    var pattern = await _context.Patterns.FindAsync(itemId);
                    if (pattern != null)
                    {
                        // Check if pattern name or tags match fundamental skills
                        foreach (var skill in FundamentalSkills)
                        {
                            if (IsPatternRelatedToSkill(pattern, skill))
                            {
                                var coverage = metrics.FundamentalsCoverage[skill];
                                coverage.IsCovered = true;
                                
                                if (!coverage.WeeksIntroduced.Contains(weekNumber))
                                {
                                    coverage.WeeksIntroduced.Add(weekNumber);
                                }
                                
                                coverage.RepetitionCount++;
                                
                                if (!weekConcepts.Contains(skill))
                                {
                                    weekConcepts.Add(skill);
                                }
                            }
                        }
                    }
                }
            }

            metrics.WeeklyProgress.Add(new WeekProgress
            {
                WeekNumber = weekNumber,
                Theme = weekNumber <= course.ThemesByWeek.Count ? course.ThemesByWeek[weekNumber - 1] : "",
                CoveredConcepts = weekConcepts,
                LessonId = lesson.Id,
                TotalMinutes = lesson.Duration
            });

            weekNumber++;
        }

        // Calculate overall statistics
        metrics.TotalSkillsCovered = metrics.FundamentalsCoverage.Values.Count(c => c.IsCovered);
        metrics.CoveragePercentage = (double)metrics.TotalSkillsCovered / FundamentalSkills.Length * 100;

        return metrics;
    }

    /// <summary>
    /// Analyze course for progression warnings and conflicts (FR-023)
    /// </summary>
    public async Task<ProgressionAnalysis> AnalyzeProgressionAsync(string courseId)
    {
        var course = await _context.Courses
            .Include(c => c.Lessons)
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null)
        {
            throw new InvalidOperationException($"Course {courseId} not found");
        }

        var analysis = new ProgressionAnalysis
        {
            CourseId = courseId,
            CourseName = course.Name,
            Level = course.Level.ToString()
        };

        var coverage = await CalculateCoverageAsync(courseId);

        // Check for missing fundamentals based on course level
        CheckMissingFundamentals(course.Level, coverage, analysis);

        // Check for overloading (too many new concepts per week)
        CheckForOverloading(coverage, analysis);

        // Check for proper spacing of repetitions
        CheckRepetitionSpacing(coverage, analysis);

        // Calculate progression score
        analysis.ProgressionScore = CalculateProgressionScore(coverage, analysis);

        return analysis;
    }

    private bool IsPatternRelatedToSkill(PatternOrExercise pattern, string skill)
    {
        // Check pattern name
        if (pattern.Name.Contains(skill, StringComparison.OrdinalIgnoreCase))
            return true;

        // Check tags
        if (pattern.Tags.Any(t => t.Contains(skill, StringComparison.OrdinalIgnoreCase)))
            return true;

        // Check teaching points
        if (pattern.TeachingPoints.Any(tp => tp.Contains(skill, StringComparison.OrdinalIgnoreCase)))
            return true;

        return false;
    }

    private void CheckMissingFundamentals(DanceLevel level, CourseCoverageMetrics coverage, ProgressionAnalysis analysis)
    {
        // Define required fundamentals by level
        var requiredForBeginner = new[] { "Sugar Push", "Connection", "Anchor" };
        var requiredForImprover = new[] { "Sugar Push", "Left Side Pass", "Right Side Pass", "Connection", "Anchor", "Stretch" };
        var requiredForIntermediate = new[] { "Sugar Push", "Left Side Pass", "Right Side Pass", "Whip", "Connection", "Anchor", "Stretch" };

        string[] required = level switch
        {
            DanceLevel.Beginner => requiredForBeginner,
            DanceLevel.Improver => requiredForImprover,
            DanceLevel.Intermediate => requiredForIntermediate,
            DanceLevel.Advanced => FundamentalSkills,
            _ => requiredForBeginner
        };

        foreach (var skill in required)
        {
            if (!coverage.FundamentalsCoverage[skill].IsCovered)
            {
                analysis.Warnings.Add(new ProgressionWarning
                {
                    Severity = WarningLevel.High,
                    Type = WarningType.MissingFundamental,
                    Message = $"Saknar grundläggande färdighet för {level}: {skill}",
                    Recommendation = $"Lägg till övningar eller turer som täcker {skill}"
                });
            }
        }
    }

    private void CheckForOverloading(CourseCoverageMetrics coverage, ProgressionAnalysis analysis)
    {
        const int MaxNewConceptsPerWeek = 3;

        foreach (var week in coverage.WeeklyProgress)
        {
            if (week.CoveredConcepts.Count > MaxNewConceptsPerWeek)
            {
                analysis.Warnings.Add(new ProgressionWarning
                {
                    Severity = WarningLevel.Medium,
                    Type = WarningType.Overloading,
                    WeekNumber = week.WeekNumber,
                    Message = $"Vecka {week.WeekNumber}: För många nya koncept ({week.CoveredConcepts.Count})",
                    Recommendation = $"Överväg att sprida ut koncepten över flera veckor eller ta bort {week.CoveredConcepts.Count - MaxNewConceptsPerWeek} koncept"
                });
            }
        }
    }

    private void CheckRepetitionSpacing(CourseCoverageMetrics coverage, ProgressionAnalysis analysis)
    {
        const int MinWeeksForRepetition = 2;

        foreach (var skill in coverage.FundamentalsCoverage.Values.Where(s => s.IsCovered))
        {
            if (skill.WeeksIntroduced.Count > 1)
            {
                // Check spacing between repetitions
                for (int i = 1; i < skill.WeeksIntroduced.Count; i++)
                {
                    var spacing = skill.WeeksIntroduced[i] - skill.WeeksIntroduced[i - 1];
                    if (spacing < MinWeeksForRepetition)
                    {
                        analysis.Warnings.Add(new ProgressionWarning
                        {
                            Severity = WarningLevel.Low,
                            Type = WarningType.PoorSpacing,
                            Message = $"{skill.SkillName}: Repetition för snabbt (vecka {skill.WeeksIntroduced[i - 1]} till {skill.WeeksIntroduced[i]})",
                            Recommendation = $"Överväg att öka avståndet mellan repetitioner av {skill.SkillName}"
                        });
                    }
                }
            }
            else if (skill.WeeksIntroduced.Count == 1 && coverage.DurationWeeks > 4)
            {
                // Skill is only covered once in a longer course
                analysis.Warnings.Add(new ProgressionWarning
                {
                    Severity = WarningLevel.Medium,
                    Type = WarningType.NoRepetition,
                    Message = $"{skill.SkillName}: Ingen repetition i en {coverage.DurationWeeks}-veckors kurs",
                    Recommendation = $"Lägg till repetition av {skill.SkillName} för bättre inlärning"
                });
            }
        }
    }

    private double CalculateProgressionScore(CourseCoverageMetrics coverage, ProgressionAnalysis analysis)
    {
        double score = 100.0;

        // Deduct points for warnings
        score -= analysis.Warnings.Count(w => w.Severity == WarningLevel.High) * 15;
        score -= analysis.Warnings.Count(w => w.Severity == WarningLevel.Medium) * 7;
        score -= analysis.Warnings.Count(w => w.Severity == WarningLevel.Low) * 3;

        // Add points for good coverage
        score += coverage.CoveragePercentage * 0.2;

        return Math.Max(0, Math.Min(100, score));
    }
}

// Data models for progression and coverage

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
    public WarningLevel Severity { get; set; }
    public WarningType Type { get; set; }
    public int? WeekNumber { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Recommendation { get; set; } = string.Empty;
}

public enum WarningLevel
{
    Low,
    Medium,
    High
}

public enum WarningType
{
    MissingFundamental,
    Overloading,
    NoRepetition,
    PoorSpacing,
    TooFastProgression
}
