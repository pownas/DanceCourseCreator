using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace DanceCourseCreator.API.Models;

public class PatternOrExercise
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [Required]
    public PatternType Type { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;
    
    public string AliasesJson { get; set; } = "[]";
    
    public List<string> Aliases
    {
        get => JsonSerializer.Deserialize<List<string>>(AliasesJson) ?? new List<string>();
        set => AliasesJson = JsonSerializer.Serialize(value);
    }
    
    [Required]
    public DanceLevel Level { get; set; }
    
    public string Description { get; set; } = string.Empty;
    
    public string StepsJson { get; set; } = "[]";
    
    public List<string> Steps
    {
        get => JsonSerializer.Deserialize<List<string>>(StepsJson) ?? new List<string>();
        set => StepsJson = JsonSerializer.Serialize(value);
    }
    
    public string CountsJson { get; set; } = "[]";
    
    public List<string> Counts
    {
        get => JsonSerializer.Deserialize<List<string>>(CountsJson) ?? new List<string>();
        set => CountsJson = JsonSerializer.Serialize(value);
    }
    
    public string HoldsJson { get; set; } = "[]";
    
    public List<string> Holds
    {
        get => JsonSerializer.Deserialize<List<string>>(HoldsJson) ?? new List<string>();
        set => HoldsJson = JsonSerializer.Serialize(value);
    }
    
    public string Slot { get; set; } = string.Empty;
    
    public string RotationsJson { get; set; } = "[]";
    
    public List<string> Rotations
    {
        get => JsonSerializer.Deserialize<List<string>>(RotationsJson) ?? new List<string>();
        set => RotationsJson = JsonSerializer.Serialize(value);
    }
    
    public string PrerequisitesJson { get; set; } = "[]";
    
    public List<string> Prerequisites
    {
        get => JsonSerializer.Deserialize<List<string>>(PrerequisitesJson) ?? new List<string>();
        set => PrerequisitesJson = JsonSerializer.Serialize(value);
    }
    
    public string RelatedJson { get; set; } = "[]";
    
    public List<string> Related
    {
        get => JsonSerializer.Deserialize<List<string>>(RelatedJson) ?? new List<string>();
        set => RelatedJson = JsonSerializer.Serialize(value);
    }
    
    public string TeachingPointsJson { get; set; } = "[]";
    
    public List<string> TeachingPoints
    {
        get => JsonSerializer.Deserialize<List<string>>(TeachingPointsJson) ?? new List<string>();
        set => TeachingPointsJson = JsonSerializer.Serialize(value);
    }
    
    public string CommonMistakesJson { get; set; } = "[]";
    
    public List<string> CommonMistakes
    {
        get => JsonSerializer.Deserialize<List<string>>(CommonMistakesJson) ?? new List<string>();
        set => CommonMistakesJson = JsonSerializer.Serialize(value);
    }
    
    public string VariationsJson { get; set; } = "[]";
    
    public List<string> Variations
    {
        get => JsonSerializer.Deserialize<List<string>>(VariationsJson) ?? new List<string>();
        set => VariationsJson = JsonSerializer.Serialize(value);
    }
    
    public int EstimatedMinutes { get; set; }
    
    public int BpmRangeMin { get; set; }
    
    public int BpmRangeMax { get; set; }
    
    public string TagsJson { get; set; } = "[]";
    
    public List<string> Tags
    {
        get => JsonSerializer.Deserialize<List<string>>(TagsJson) ?? new List<string>();
        set => TagsJson = JsonSerializer.Serialize(value);
    }
    
    public string MediaLinksJson { get; set; } = "[]";
    
    public List<string> MediaLinks
    {
        get => JsonSerializer.Deserialize<List<string>>(MediaLinksJson) ?? new List<string>();
        set => MediaLinksJson = JsonSerializer.Serialize(value);
    }
    
    [Required]
    public string CreatedBy { get; set; } = string.Empty;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public User? Creator { get; set; }
}

public enum PatternType
{
    Pattern,
    Exercise
}

public enum DanceLevel
{
    Beginner,
    Improver,
    Intermediate,
    Advanced
}