using System.ComponentModel.DataAnnotations;

namespace DanceCourseCreator.API.DTOs;

public class TemplateDto
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
    [Required]
    public string Scope { get; set; } = string.Empty;
    
    [Required]
    [StringLength(200, ErrorMessage = "Mallnamnet får inte vara längre än 200 tecken")]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public string Content { get; set; } = string.Empty;
    
    public string? Team { get; set; }
}

public class UpdateTemplateRequest
{
    [Required]
    [StringLength(200, ErrorMessage = "Mallnamnet får inte vara längre än 200 tecken")]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public string Content { get; set; } = string.Empty;
    
    public string? Team { get; set; }
}

public class DuplicateTemplateRequest
{
    [Required]
    [StringLength(200, ErrorMessage = "Namnet får inte vara längre än 200 tecken")]
    public string Name { get; set; } = string.Empty;
    
    public string? Team { get; set; }
    
    // Option to modify content during duplication
    public string? ModifiedContent { get; set; }
}

public class TemplateListResponse
{
    public List<TemplateDto> Templates { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public bool HasNext { get; set; }
    public bool HasPrevious { get; set; }
}

public class DuplicateTemplateResponse
{
    public string CreatedResourceId { get; set; } = string.Empty;
    public string ResourceType { get; set; } = string.Empty; // "Lesson" or "Course"
    public string Name { get; set; } = string.Empty;
}