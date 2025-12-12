using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DanceCourseCreator.API.DTOs;
using DanceCourseCreator.API.Services;

namespace DanceCourseCreator.API.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize] // Temporarily disabled for testing
public class ExportController : ControllerBase
{
    private readonly ExportService _exportService;
    private readonly ILogger<ExportController> _logger;

    public ExportController(ExportService exportService, ILogger<ExportController> logger)
    {
        _exportService = exportService;
        _logger = logger;
    }

    /// <summary>
    /// Export a course to the specified format (PDF or Markdown)
    /// </summary>
    /// <param name="id">Course ID</param>
    /// <param name="request">Export options</param>
    /// <returns>The exported file</returns>
    [HttpPost("course/{id}")]
    public async Task<IActionResult> ExportCourse(string id, [FromBody] ExportRequest request)
    {
        try
        {
            _logger.LogInformation("Export request for course {CourseId} in format {Format}", id, request.Format);

            if (request.Format == ExportFormat.PDF)
            {
                var pdfBytes = await _exportService.ExportCourseToPdfAsync(
                    id, 
                    request.IncludeSchedule, 
                    request.IncludePatternDetails);
                
                return File(pdfBytes, "application/pdf", $"course-{id}.pdf");
            }
            else if (request.Format == ExportFormat.Markdown)
            {
                var markdownContent = await _exportService.ExportCourseToMarkdownAsync(
                    id, 
                    request.IncludeSchedule, 
                    request.IncludePatternDetails);
                
                var bytes = System.Text.Encoding.UTF8.GetBytes(markdownContent);
                return File(bytes, "text/markdown", $"course-{id}.md");
            }
            else
            {
                return BadRequest(new { message = "Ogiltigt exportformat" });
            }
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Course {CourseId} not found", id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting course {CourseId}", id);
            return StatusCode(500, new { message = "Ett fel uppstod vid export", error = ex.Message });
        }
    }

    /// <summary>
    /// Export a lesson to the specified format (PDF or Markdown)
    /// </summary>
    /// <param name="id">Lesson ID</param>
    /// <param name="request">Export options</param>
    /// <returns>The exported file</returns>
    [HttpPost("lesson/{id}")]
    public async Task<IActionResult> ExportLesson(string id, [FromBody] ExportRequest request)
    {
        try
        {
            _logger.LogInformation("Export request for lesson {LessonId} in format {Format}", id, request.Format);

            if (request.Format == ExportFormat.PDF)
            {
                var pdfBytes = await _exportService.ExportLessonToPdfAsync(
                    id, 
                    request.IncludePatternDetails);
                
                return File(pdfBytes, "application/pdf", $"lesson-{id}.pdf");
            }
            else if (request.Format == ExportFormat.Markdown)
            {
                var markdownContent = await _exportService.ExportLessonToMarkdownAsync(
                    id, 
                    request.IncludePatternDetails);
                
                var bytes = System.Text.Encoding.UTF8.GetBytes(markdownContent);
                return File(bytes, "text/markdown", $"lesson-{id}.md");
            }
            else
            {
                return BadRequest(new { message = "Ogiltigt exportformat" });
            }
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Lesson {LessonId} not found", id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting lesson {LessonId}", id);
            return StatusCode(500, new { message = "Ett fel uppstod vid export", error = ex.Message });
        }
    }
}
