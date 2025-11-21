using System.ComponentModel.DataAnnotations;

namespace DanceCourseCreator.API.DTOs;

public class LessonDto
{
    public string Id { get; set; } = string.Empty;
    public string? CourseId { get; set; }
    public DateTime? Date { get; set; }
    public int Duration { get; set; } // minutes
    public List<LessonSectionDto> Sections { get; set; } = new();
    public int TotalEstimatedMinutes { get; set; }
    public string Notes { get; set; } = string.Empty;
    public int Version { get; set; } = 1;
    public string CreatedBy { get; set; } = string.Empty;
    public List<string> Reviewers { get; set; } = new();
    public List<string> History { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class LessonSectionDto
{
    public string Id { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public List<string> Items { get; set; } = new();
    public string Notes { get; set; } = string.Empty;
}

public class CreateLessonRequest
{
    public string? CourseId { get; set; }
    public DateTime? Date { get; set; }
    
    [Range(60, 300, ErrorMessage = "Lektionslängd måste vara mellan 60 och 300 minuter (1-5 timmar)")]
    public int Duration { get; set; } = 75; // Default to 75 minutes
    
    public List<LessonSectionDto> Sections { get; set; } = new();
    public string Notes { get; set; } = string.Empty;
}