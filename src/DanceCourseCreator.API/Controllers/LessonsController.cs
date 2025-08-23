using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DanceCourseCreator.API.Data;
using DanceCourseCreator.API.DTOs;
using DanceCourseCreator.API.Models;
using System.Security.Claims;

namespace DanceCourseCreator.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LessonsController : ControllerBase
{
    private readonly DanceCourseDbContext _context;

    public LessonsController(DanceCourseDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LessonDto>>> GetLessons([FromQuery] string? courseId = null)
    {
        var query = _context.Lessons.AsQueryable();

        if (!string.IsNullOrEmpty(courseId))
        {
            query = query.Where(l => l.CourseId == courseId);
        }

        var lessons = await query.ToListAsync();

        var lessonDtos = lessons.Select(l => new LessonDto
        {
            Id = l.Id,
            CourseId = l.CourseId,
            Date = l.Date,
            Duration = l.Duration,
            Sections = l.Sections.Select(s => new LessonSectionDto
            {
                Id = s.Id,
                Type = s.Type.ToString(),
                Items = s.Items,
                Notes = s.Notes
            }).ToList(),
            TotalEstimatedMinutes = l.TotalEstimatedMinutes,
            Notes = l.Notes,
            Version = l.Version,
            CreatedBy = l.CreatedBy,
            Reviewers = l.Reviewers,
            History = l.History,
            CreatedAt = l.CreatedAt,
            UpdatedAt = l.UpdatedAt
        }).ToList();

        return Ok(lessonDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LessonDto>> GetLesson(string id)
    {
        var lesson = await _context.Lessons.FindAsync(id);

        if (lesson == null)
        {
            return NotFound();
        }

        var lessonDto = new LessonDto
        {
            Id = lesson.Id,
            CourseId = lesson.CourseId,
            Date = lesson.Date,
            Duration = lesson.Duration,
            Sections = lesson.Sections.Select(s => new LessonSectionDto
            {
                Id = s.Id,
                Type = s.Type.ToString(),
                Items = s.Items,
                Notes = s.Notes
            }).ToList(),
            TotalEstimatedMinutes = lesson.TotalEstimatedMinutes,
            Notes = lesson.Notes,
            Version = lesson.Version,
            CreatedBy = lesson.CreatedBy,
            Reviewers = lesson.Reviewers,
            History = lesson.History,
            CreatedAt = lesson.CreatedAt,
            UpdatedAt = lesson.UpdatedAt
        };

        return Ok(lessonDto);
    }

    [HttpPost]
    [Authorize(Roles = "Instructor,Editor,Admin")]
    public async Task<ActionResult<LessonDto>> CreateLesson(CreateLessonRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var lessonSections = request.Sections.Select(s => new LessonSection
        {
            Type = Enum.TryParse<LessonSectionType>(s.Type, true, out var type) ? type : LessonSectionType.Patterns,
            Items = s.Items,
            Notes = s.Notes
        }).ToList();

        // Calculate total estimated minutes based on section items
        int totalEstimatedMinutes = 0;
        foreach (var section in lessonSections)
        {
            // For now, we'll use a simple estimation - this could be enhanced to lookup actual pattern durations
            totalEstimatedMinutes += section.Items.Count * 5; // Assume 5 minutes per item
        }

        var lesson = new Lesson
        {
            CourseId = request.CourseId,
            Date = request.Date,
            Duration = request.Duration,
            Sections = lessonSections,
            TotalEstimatedMinutes = totalEstimatedMinutes,
            Notes = request.Notes,
            CreatedBy = userId
        };

        _context.Lessons.Add(lesson);
        await _context.SaveChangesAsync();

        var lessonDto = new LessonDto
        {
            Id = lesson.Id,
            CourseId = lesson.CourseId,
            Date = lesson.Date,
            Duration = lesson.Duration,
            Sections = lesson.Sections.Select(s => new LessonSectionDto
            {
                Id = s.Id,
                Type = s.Type.ToString(),
                Items = s.Items,
                Notes = s.Notes
            }).ToList(),
            TotalEstimatedMinutes = lesson.TotalEstimatedMinutes,
            Notes = lesson.Notes,
            Version = lesson.Version,
            CreatedBy = lesson.CreatedBy,
            Reviewers = lesson.Reviewers,
            History = lesson.History,
            CreatedAt = lesson.CreatedAt,
            UpdatedAt = lesson.UpdatedAt
        };

        return CreatedAtAction(nameof(GetLesson), new { id = lesson.Id }, lessonDto);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Instructor,Editor,Admin")]
    public async Task<ActionResult<LessonDto>> UpdateLesson(string id, CreateLessonRequest request)
    {
        var lesson = await _context.Lessons.FindAsync(id);

        if (lesson == null)
        {
            return NotFound();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRole = User.FindFirstValue(ClaimTypes.Role);
        
        if (lesson.CreatedBy != userId && userRole != "Admin")
        {
            return Forbid();
        }

        var lessonSections = request.Sections.Select(s => new LessonSection
        {
            Id = s.Id, // Preserve existing IDs if provided
            Type = Enum.TryParse<LessonSectionType>(s.Type, true, out var type) ? type : LessonSectionType.Patterns,
            Items = s.Items,
            Notes = s.Notes
        }).ToList();

        // Calculate total estimated minutes
        int totalEstimatedMinutes = 0;
        foreach (var section in lessonSections)
        {
            totalEstimatedMinutes += section.Items.Count * 5; // Assume 5 minutes per item
        }

        lesson.CourseId = request.CourseId;
        lesson.Date = request.Date;
        lesson.Duration = request.Duration;
        lesson.Sections = lessonSections;
        lesson.TotalEstimatedMinutes = totalEstimatedMinutes;
        lesson.Notes = request.Notes;
        lesson.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        var lessonDto = new LessonDto
        {
            Id = lesson.Id,
            CourseId = lesson.CourseId,
            Date = lesson.Date,
            Duration = lesson.Duration,
            Sections = lesson.Sections.Select(s => new LessonSectionDto
            {
                Id = s.Id,
                Type = s.Type.ToString(),
                Items = s.Items,
                Notes = s.Notes
            }).ToList(),
            TotalEstimatedMinutes = lesson.TotalEstimatedMinutes,
            Notes = lesson.Notes,
            Version = lesson.Version,
            CreatedBy = lesson.CreatedBy,
            Reviewers = lesson.Reviewers,
            History = lesson.History,
            CreatedAt = lesson.CreatedAt,
            UpdatedAt = lesson.UpdatedAt
        };

        return Ok(lessonDto);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Instructor,Editor,Admin")]
    public async Task<ActionResult> DeleteLesson(string id)
    {
        var lesson = await _context.Lessons.FindAsync(id);

        if (lesson == null)
        {
            return NotFound();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRole = User.FindFirstValue(ClaimTypes.Role);
        
        if (lesson.CreatedBy != userId && userRole != "Admin")
        {
            return Forbid();
        }

        _context.Lessons.Remove(lesson);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}