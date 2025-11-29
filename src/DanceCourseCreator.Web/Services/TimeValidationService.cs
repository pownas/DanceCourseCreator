using DanceCourseCreator.Web.Models;

namespace DanceCourseCreator.Web.Services;

/// <summary>
/// Service for validating lesson time allocations and requirements
/// </summary>
public interface ITimeValidationService
{
    TimeValidationResult ValidateLesson(CreateLessonRequest lesson, List<PatternOrExercise>? patterns = null);
    int CalculateEstimatedTime(List<LessonSection> sections, List<PatternOrExercise>? patterns = null);
    int CalculateSectionEstimatedTime(LessonSection section, List<PatternOrExercise>? patterns = null);
}

public class TimeValidationService : ITimeValidationService
{
    // Validation constants based on requirements (FR-011, FR-012)
    private const int MinLessonDuration = 60;  // 1 hour
    private const int MaxLessonDuration = 300; // 5 hours
    private const int MinSectionsCount = 3;
    private const int MaxSectionsCount = 8;
    private const int TimeBufferPercentage = 15; // 15% buffer tolerance
    private const int DefaultItemEstimate = 5; // Default minutes for unknown items
    private const int AllocationDiffWarningPercentage = 10; // Warn if allocated time differs by more than 10%
    private const int SectionTimeDiffWarningPercentage = 50; // Warn if section time differs by more than 50%

    public TimeValidationResult ValidateLesson(CreateLessonRequest lesson, List<PatternOrExercise>? patterns = null)
    {
        var result = new TimeValidationResult();

        // Validate lesson duration
        ValidateLessonDuration(lesson, result);

        // Validate section count
        ValidateSectionCount(lesson, result);

        // Calculate times
        var estimatedTime = CalculateEstimatedTime(lesson.Sections, patterns);
        var allocatedTime = lesson.Sections.Sum(s => s.AllocatedMinutes);

        // Validate time allocation
        ValidateTimeAllocation(lesson, estimatedTime, allocatedTime, result);

        // Validate individual sections
        ValidateSections(lesson, patterns, result);

        return result;
    }

    private void ValidateLessonDuration(CreateLessonRequest lesson, TimeValidationResult result)
    {
        if (lesson.Duration < MinLessonDuration)
        {
            result.AddError($"Lektionslängden ({lesson.Duration} min) är för kort. Minimum är {MinLessonDuration} minuter.");
        }
        else if (lesson.Duration > MaxLessonDuration)
        {
            result.AddWarning($"Lektionslängden ({lesson.Duration} min) är mycket lång. Maximum rekommenderat är {MaxLessonDuration} minuter.");
        }
    }

    private void ValidateSectionCount(CreateLessonRequest lesson, TimeValidationResult result)
    {
        var sectionCount = lesson.Sections.Count;
        if (sectionCount < MinSectionsCount)
        {
            result.AddWarning($"Endast {sectionCount} sektion(er). Rekommenderat minimum är {MinSectionsCount} sektioner per lektion.");
        }
        else if (sectionCount > MaxSectionsCount)
        {
            result.AddWarning($"Lektionen har {sectionCount} sektioner. Rekommenderat maximum är {MaxSectionsCount} sektioner.");
        }
    }

    private void ValidateTimeAllocation(CreateLessonRequest lesson, int estimatedTime, int allocatedTime, TimeValidationResult result)
    {
        // Check if allocated time matches lesson duration
        if (allocatedTime > 0)
        {
            var allocationDiff = Math.Abs(lesson.Duration - allocatedTime);
            var allocationDiffPercent = (double)allocationDiff / lesson.Duration * 100;

            if (allocationDiff > 0)
            {
                if (allocatedTime > lesson.Duration)
                {
                    result.AddError($"Total allokerad tid ({allocatedTime} min) överskrider lektionslängden ({lesson.Duration} min) med {allocationDiff} minuter.");
                }
                else if (allocationDiffPercent > AllocationDiffWarningPercentage)
                {
                    result.AddWarning($"Total allokerad tid ({allocatedTime} min) är {allocationDiff} minuter mindre än lektionslängden ({lesson.Duration} min).");
                }
                else
                {
                    result.AddInfo($"Total allokerad tid: {allocatedTime} min av {lesson.Duration} min.");
                }
            }
        }
        else
        {
            result.AddInfo("Ingen tid allokerad till sektioner ännu.");
        }

        // Check estimated time vs lesson duration
        var timeDiff = Math.Abs(lesson.Duration - estimatedTime);
        var timeDiffPercent = (double)timeDiff / lesson.Duration * 100;

        if (timeDiffPercent > TimeBufferPercentage)
        {
            if (estimatedTime > lesson.Duration)
            {
                result.AddError($"Estimerad tid ({estimatedTime} min) överskrider planerad lektionslängd ({lesson.Duration} min) med {timeDiff} minuter ({timeDiffPercent:F1}%).");
            }
            else
            {
                result.AddWarning($"Estimerad tid ({estimatedTime} min) är betydligt mindre än planerad lektionslängd ({lesson.Duration} min). Skillnad: {timeDiff} minuter ({timeDiffPercent:F1}%).");
            }
        }
        else if (timeDiff > 0)
        {
            result.AddInfo($"Estimerad tid ({estimatedTime} min) ligger inom acceptabelt intervall för lektionslängd ({lesson.Duration} min).");
        }
    }

    private void ValidateSections(CreateLessonRequest lesson, List<PatternOrExercise>? patterns, TimeValidationResult result)
    {
        for (int i = 0; i < lesson.Sections.Count; i++)
        {
            var section = lesson.Sections[i];
            var sectionEstimated = CalculateSectionEstimatedTime(section, patterns);

            // Validate section has content
            if (section.Items.Count == 0)
            {
                result.AddWarning($"Sektion {i + 1} ({section.Type}) har inga mönster/övningar tillagda.");
            }

            // Validate section time allocation
            if (section.AllocatedMinutes > 0)
            {
                var sectionDiff = Math.Abs(section.AllocatedMinutes - sectionEstimated);
                var sectionDiffPercent = sectionEstimated > 0 ? (double)sectionDiff / sectionEstimated * 100 : 0;

                if (sectionDiffPercent > SectionTimeDiffWarningPercentage)
                {
                    if (section.AllocatedMinutes < sectionEstimated)
                    {
                        result.AddWarning($"Sektion {i + 1} ({section.Type}): Allokerad tid ({section.AllocatedMinutes} min) kan vara för kort för innehållet (estimerat {sectionEstimated} min).");
                    }
                    else
                    {
                        result.AddInfo($"Sektion {i + 1} ({section.Type}): Allokerad tid ({section.AllocatedMinutes} min) ger god buffert mot estimerad tid ({sectionEstimated} min).");
                    }
                }
            }
            else if (section.Items.Count > 0)
            {
                result.AddInfo($"Sektion {i + 1} ({section.Type}): Ingen tid allokerad (estimerat {sectionEstimated} min för {section.Items.Count} moment).");
            }
        }
    }

    public int CalculateEstimatedTime(List<LessonSection> sections, List<PatternOrExercise>? patterns = null)
    {
        return sections.Sum(s => CalculateSectionEstimatedTime(s, patterns));
    }

    public int CalculateSectionEstimatedTime(LessonSection section, List<PatternOrExercise>? patterns = null)
    {
        int total = 0;
        foreach (var itemId in section.Items)
        {
            var pattern = patterns?.FirstOrDefault(p => p.Id == itemId);
            if (pattern != null)
            {
                total += pattern.EstimatedMinutes;
            }
            else
            {
                total += DefaultItemEstimate;
            }
        }
        return total;
    }
}

/// <summary>
/// Result of time validation with categorized messages
/// </summary>
public class TimeValidationResult
{
    public List<ValidationMessage> Messages { get; set; } = new();
    
    public bool IsValid => !Messages.Any(m => m.Severity == ValidationSeverity.Error);
    public bool HasWarnings => Messages.Any(m => m.Severity == ValidationSeverity.Warning);
    public bool HasErrors => Messages.Any(m => m.Severity == ValidationSeverity.Error);
    
    public IEnumerable<ValidationMessage> Errors => Messages.Where(m => m.Severity == ValidationSeverity.Error);
    public IEnumerable<ValidationMessage> Warnings => Messages.Where(m => m.Severity == ValidationSeverity.Warning);
    public IEnumerable<ValidationMessage> Infos => Messages.Where(m => m.Severity == ValidationSeverity.Info);

    public void AddError(string message)
    {
        Messages.Add(new ValidationMessage(ValidationSeverity.Error, message));
    }

    public void AddWarning(string message)
    {
        Messages.Add(new ValidationMessage(ValidationSeverity.Warning, message));
    }

    public void AddInfo(string message)
    {
        Messages.Add(new ValidationMessage(ValidationSeverity.Info, message));
    }
}

public class ValidationMessage
{
    public ValidationSeverity Severity { get; set; }
    public string Message { get; set; }

    public ValidationMessage(ValidationSeverity severity, string message)
    {
        Severity = severity;
        Message = message;
    }
}

public enum ValidationSeverity
{
    Info,
    Warning,
    Error
}
