using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace DanceCourseCreator.API.Models;

public class LessonSection
{
    public LessonSectionType Type { get; set; }
    
    public string ItemsJson { get; set; } = "[]";
    
    public List<string> Items
    {
        get => JsonSerializer.Deserialize<List<string>>(ItemsJson) ?? new List<string>();
        set => ItemsJson = JsonSerializer.Serialize(value);
    }
    
    public string Notes { get; set; } = string.Empty;
}

public class Lesson
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    public string? CourseId { get; set; }
    
    public DateTime? Date { get; set; }
    
    public int Duration { get; set; } // minutes
    
    public string SectionsJson { get; set; } = "[]";
    
    public List<LessonSection> Sections
    {
        get => JsonSerializer.Deserialize<List<LessonSection>>(SectionsJson) ?? new List<LessonSection>();
        set => SectionsJson = JsonSerializer.Serialize(value);
    }
    
    public int TotalEstimatedMinutes { get; set; }
    
    public string Notes { get; set; } = string.Empty;
    
    public int Version { get; set; } = 1;
    
    [Required]
    public string CreatedBy { get; set; } = string.Empty;
    
    public string ReviewersJson { get; set; } = "[]";
    
    public List<string> Reviewers
    {
        get => JsonSerializer.Deserialize<List<string>>(ReviewersJson) ?? new List<string>();
        set => ReviewersJson = JsonSerializer.Serialize(value);
    }
    
    public string HistoryJson { get; set; } = "[]";
    
    public List<string> History
    {
        get => JsonSerializer.Deserialize<List<string>>(HistoryJson) ?? new List<string>();
        set => HistoryJson = JsonSerializer.Serialize(value);
    }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public User? Creator { get; set; }
    public Course? Course { get; set; }
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