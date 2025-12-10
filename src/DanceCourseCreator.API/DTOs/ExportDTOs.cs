using System.ComponentModel.DataAnnotations;

namespace DanceCourseCreator.API.DTOs;

public class ExportRequest
{
    [Required]
    public ExportFormat Format { get; set; } = ExportFormat.PDF;
    
    public bool IncludeSchedule { get; set; } = true;
    
    public bool IncludePatternDetails { get; set; } = true;
}

public enum ExportFormat
{
    PDF,
    Markdown
}
