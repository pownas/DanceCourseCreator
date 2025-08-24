using System.ComponentModel.DataAnnotations;

namespace DanceCourseCreator.API.DTOs;

public class CourseDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public int DurationWeeks { get; set; }
    public List<string> Goals { get; set; } = new();
    public List<string> ThemesByWeek { get; set; } = new();
    public List<string> LessonIds { get; set; } = new();
    public string CoverageMetrics { get; set; } = "{}";
    public string RepetitionPlan { get; set; } = "{}";
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateCourseRequest
{
    [Required]
    [StringLength(200, ErrorMessage = "Kursnamnet får inte vara längre än 200 tecken")]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public string Level { get; set; } = string.Empty;
    
    [Range(1, 52, ErrorMessage = "Kurslängden måste vara mellan 1 och 52 veckor")]
    public int DurationWeeks { get; set; } = 6;
    
    public List<string> Goals { get; set; } = new();
    public List<string> ThemesByWeek { get; set; } = new();
}

public class UpdateCourseRequest
{
    [Required]
    [StringLength(200, ErrorMessage = "Kursnamnet får inte vara längre än 200 tecken")]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public string Level { get; set; } = string.Empty;
    
    [Range(1, 52, ErrorMessage = "Kurslängden måste vara mellan 1 och 52 veckor")]
    public int DurationWeeks { get; set; }
    
    public List<string> Goals { get; set; } = new();
    public List<string> ThemesByWeek { get; set; } = new();
    public List<string> LessonIds { get; set; } = new();
    public string CoverageMetrics { get; set; } = "{}";
    public string RepetitionPlan { get; set; } = "{}";
}