using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace DanceCourseCreator.API.Models;

public class Course
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public DanceLevel Level { get; set; }
    
    public CourseType Type { get; set; } = CourseType.Weekly;
    
    public int DurationWeeks { get; set; }
    
    public int PlannedLessonCount { get; set; }
    
    public string GoalsJson { get; set; } = "[]";
    
    public List<string> Goals
    {
        get => JsonSerializer.Deserialize<List<string>>(GoalsJson) ?? new List<string>();
        set => GoalsJson = JsonSerializer.Serialize(value);
    }
    
    public string ThemesByWeekJson { get; set; } = "[]";
    
    public List<string> ThemesByWeek
    {
        get => JsonSerializer.Deserialize<List<string>>(ThemesByWeekJson) ?? new List<string>();
        set => ThemesByWeekJson = JsonSerializer.Serialize(value);
    }
    
    public string LessonsJson { get; set; } = "[]";
    
    public List<string> LessonIds
    {
        get => JsonSerializer.Deserialize<List<string>>(LessonsJson) ?? new List<string>();
        set => LessonsJson = JsonSerializer.Serialize(value);
    }
    
    public string CoverageMetrics { get; set; } = "{}";
    
    public string RepetitionPlan { get; set; } = "{}";
    
    [Required]
    public string CreatedBy { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public User? Creator { get; set; }
    public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}

public enum CourseType
{
    Weekly,      // Veckokurs: 4-20 lektioner à 1 timme per vecka
    Weekend      // Helgkurs: 1-2 dagar, 4 timmar per dag
}