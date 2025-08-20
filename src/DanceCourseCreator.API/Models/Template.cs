using System.ComponentModel.DataAnnotations;

namespace DanceCourseCreator.API.Models;

public class Template
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [Required]
    public TemplateScope Scope { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;
    
    public string Content { get; set; } = string.Empty; // JSON string
    
    [Required]
    public string Owner { get; set; } = string.Empty;
    
    public string? Team { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public User? OwnerUser { get; set; }
    public Team? TeamEntity { get; set; }
}

public class ShareLink
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [Required]
    public string ResourceId { get; set; } = string.Empty;
    
    [Required]
    public ShareLinkType Type { get; set; }
    
    [Required]
    public ShareLinkVisibility Visibility { get; set; }
    
    public DateTime? ExpiresAt { get; set; }
    
    [Required]
    public string Token { get; set; } = Guid.NewGuid().ToString();
    
    [Required]
    public string CreatedBy { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public User? Creator { get; set; }
}

public enum TemplateScope
{
    Lesson,
    Course
}

public enum ShareLinkType
{
    Lesson,
    Course
}

public enum ShareLinkVisibility
{
    Public,
    Private
}