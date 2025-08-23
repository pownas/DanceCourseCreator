using System.ComponentModel.DataAnnotations;

namespace DanceCourseCreator.API.Models;

public class User
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public UserRole Role { get; set; } = UserRole.Instructor;
    
    public string? TeamId { get; set; }
    
    [Required]
    public string HashedPassword { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public Team? Team { get; set; }
    public ICollection<PatternOrExercise> CreatedPatterns { get; set; } = new List<PatternOrExercise>();
    public ICollection<Lesson> CreatedLessons { get; set; } = new List<Lesson>();
    public ICollection<Course> CreatedCourses { get; set; } = new List<Course>();
}

public enum UserRole
{
    Instructor,
    Editor,
    Reader,
    Admin
}