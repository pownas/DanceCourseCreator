using System.ComponentModel.DataAnnotations;

namespace DanceCourseCreator.API.DTOs;

public class PatternDto
{
    public string Id { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public List<string> Aliases { get; set; } = new();
    public string Level { get; set; } = string.Empty;
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
    public BpmRangeDto BpmRange { get; set; } = new();
    public List<string> Tags { get; set; } = new();
    public List<string> MediaLinks { get; set; } = new();
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
}

public class BpmRangeDto
{
    public int Min { get; set; }
    public int Max { get; set; }
}

public class CreatePatternRequest
{
    [Required]
    public string Type { get; set; } = string.Empty;
    
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;
    
    public List<string> Aliases { get; set; } = new();
    
    [Required]
    public string Level { get; set; } = string.Empty;
    
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
    public BpmRangeDto BpmRange { get; set; } = new();
    public List<string> Tags { get; set; } = new();
    public List<string> MediaLinks { get; set; } = new();
}