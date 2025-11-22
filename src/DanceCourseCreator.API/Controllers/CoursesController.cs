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
// [Authorize] // Temporarily disabled for testing
public class CoursesController : ControllerBase
{
    private readonly DanceCourseDbContext _context;

    public CoursesController(DanceCourseDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses(
        [FromQuery] string? level = null,
        [FromQuery] string? danceStyle = null,
        [FromQuery] string? search = null)
    {
        var query = _context.Courses.AsQueryable();

        if (!string.IsNullOrEmpty(level) && Enum.TryParse<DanceLevel>(level, true, out var danceLevel))
        {
            query = query.Where(c => c.Level == danceLevel);
        }

        if (!string.IsNullOrEmpty(danceStyle) && Enum.TryParse<DanceStyle>(danceStyle, true, out var style))
        {
            query = query.Where(c => c.DanceStyle == style);
        }

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(c => c.Name.Contains(search) || c.Goals.Any(g => g.Contains(search)));
        }

        var courses = await query.Include(c => c.Lessons).ToListAsync();

        var courseDtos = courses.Select(c => new CourseDto
        {
            Id = c.Id,
            Name = c.Name,
            Level = c.Level.ToString(),
            DanceStyle = c.DanceStyle.ToString(),
            Type = c.Type.ToString(),
            DurationWeeks = c.DurationWeeks,
            PlannedLessonCount = c.PlannedLessonCount,
            Goals = c.Goals,
            ThemesByWeek = c.ThemesByWeek,
            LessonIds = c.LessonIds,
            CoverageMetrics = c.CoverageMetrics,
            RepetitionPlan = c.RepetitionPlan,
            CreatedBy = c.CreatedBy,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt,
            ActualLessonCount = c.Lessons.Count,
            TotalPlannedMinutes = c.Lessons.Sum(l => l.Duration)
        }).ToList();

        return Ok(courseDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CourseDto>> GetCourse(string id)
    {
        var course = await _context.Courses
            .Include(c => c.Lessons)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (course == null)
        {
            return NotFound();
        }

        var courseDto = new CourseDto
        {
            Id = course.Id,
            Name = course.Name,
            Level = course.Level.ToString(),
            DanceStyle = course.DanceStyle.ToString(),
            Type = course.Type.ToString(),
            DurationWeeks = course.DurationWeeks,
            PlannedLessonCount = course.PlannedLessonCount,
            Goals = course.Goals,
            ThemesByWeek = course.ThemesByWeek,
            LessonIds = course.LessonIds,
            CoverageMetrics = course.CoverageMetrics,
            RepetitionPlan = course.RepetitionPlan,
            CreatedBy = course.CreatedBy,
            CreatedAt = course.CreatedAt,
            UpdatedAt = course.UpdatedAt,
            ActualLessonCount = course.Lessons.Count,
            TotalPlannedMinutes = course.Lessons.Sum(l => l.Duration)
        };

        return Ok(courseDto);
    }

    [HttpPost]
    // [Authorize(Roles = "Instructor,Editor,Admin")] // Temporarily disabled for testing
    public async Task<ActionResult<CourseDto>> CreateCourse(CreateCourseRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "test-user"; // Default for testing
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        if (!Enum.TryParse<DanceLevel>(request.Level, true, out var level))
        {
            return BadRequest("Ogiltig dansnivå");
        }
        
        if (!Enum.TryParse<DanceStyle>(request.DanceStyle, true, out var danceStyle))
        {
            return BadRequest("Ogiltig dansstil");
        }
        
        if (!Enum.TryParse<CourseType>(request.Type, true, out var courseType))
        {
            return BadRequest("Ogiltig kurstyp");
        }

        // Ensure ThemesByWeek has the right number of entries
        var themesByWeek = request.ThemesByWeek.ToList();
        while (themesByWeek.Count < request.DurationWeeks)
        {
            themesByWeek.Add(string.Empty);
        }
        if (themesByWeek.Count > request.DurationWeeks)
        {
            themesByWeek = themesByWeek.Take(request.DurationWeeks).ToList();
        }

        var course = new Course
        {
            Name = request.Name,
            Level = level,
            DanceStyle = danceStyle,
            Type = courseType,
            DurationWeeks = request.DurationWeeks,
            PlannedLessonCount = request.PlannedLessonCount,
            Goals = request.Goals,
            ThemesByWeek = themesByWeek,
            CreatedBy = userId
        };

        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        var courseDto = new CourseDto
        {
            Id = course.Id,
            Name = course.Name,
            Level = course.Level.ToString(),
            DanceStyle = course.DanceStyle.ToString(),
            Type = course.Type.ToString(),
            DurationWeeks = course.DurationWeeks,
            PlannedLessonCount = course.PlannedLessonCount,
            Goals = course.Goals,
            ThemesByWeek = course.ThemesByWeek,
            LessonIds = course.LessonIds,
            CoverageMetrics = course.CoverageMetrics,
            RepetitionPlan = course.RepetitionPlan,
            CreatedBy = course.CreatedBy,
            CreatedAt = course.CreatedAt,
            UpdatedAt = course.UpdatedAt,
            ActualLessonCount = 0,
            TotalPlannedMinutes = 0
        };

        return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, courseDto);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Instructor,Editor,Admin")]
    public async Task<ActionResult<CourseDto>> UpdateCourse(string id, UpdateCourseRequest request)
    {
        var course = await _context.Courses.FindAsync(id);

        if (course == null)
        {
            return NotFound();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRole = User.FindFirstValue(ClaimTypes.Role);
        
        if (course.CreatedBy != userId && userRole != "Admin")
        {
            return Forbid();
        }

        if (!Enum.TryParse<DanceLevel>(request.Level, true, out var level))
        {
            return BadRequest("Ogiltig dansnivå");
        }
        
        if (!Enum.TryParse<DanceStyle>(request.DanceStyle, true, out var danceStyle))
        {
            return BadRequest("Ogiltig dansstil");
        }
        
        if (!Enum.TryParse<CourseType>(request.Type, true, out var courseType))
        {
            return BadRequest("Ogiltig kurstyp");
        }

        // Ensure ThemesByWeek has the right number of entries
        var themesByWeek = request.ThemesByWeek.ToList();
        while (themesByWeek.Count < request.DurationWeeks)
        {
            themesByWeek.Add(string.Empty);
        }
        if (themesByWeek.Count > request.DurationWeeks)
        {
            themesByWeek = themesByWeek.Take(request.DurationWeeks).ToList();
        }

        course.Name = request.Name;
        course.Level = level;
        course.DanceStyle = danceStyle;
        course.Type = courseType;
        course.DurationWeeks = request.DurationWeeks;
        course.PlannedLessonCount = request.PlannedLessonCount;
        course.Goals = request.Goals;
        course.ThemesByWeek = themesByWeek;
        course.LessonIds = request.LessonIds;
        course.CoverageMetrics = request.CoverageMetrics;
        course.RepetitionPlan = request.RepetitionPlan;
        course.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        
        // Reload course with lessons to calculate totals
        await _context.Entry(course).Collection(c => c.Lessons).LoadAsync();

        var courseDto = new CourseDto
        {
            Id = course.Id,
            Name = course.Name,
            Level = course.Level.ToString(),
            DanceStyle = course.DanceStyle.ToString(),
            Type = course.Type.ToString(),
            DurationWeeks = course.DurationWeeks,
            PlannedLessonCount = course.PlannedLessonCount,
            Goals = course.Goals,
            ThemesByWeek = course.ThemesByWeek,
            LessonIds = course.LessonIds,
            CoverageMetrics = course.CoverageMetrics,
            RepetitionPlan = course.RepetitionPlan,
            CreatedBy = course.CreatedBy,
            CreatedAt = course.CreatedAt,
            UpdatedAt = course.UpdatedAt,
            ActualLessonCount = course.Lessons.Count,
            TotalPlannedMinutes = course.Lessons.Sum(l => l.Duration)
        };

        return Ok(courseDto);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Instructor,Editor,Admin")]
    public async Task<ActionResult> DeleteCourse(string id)
    {
        var course = await _context.Courses.FindAsync(id);

        if (course == null)
        {
            return NotFound();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRole = User.FindFirstValue(ClaimTypes.Role);
        
        if (course.CreatedBy != userId && userRole != "Admin")
        {
            return Forbid();
        }

        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Get coverage metrics for a course - what fundamental skills are covered
    /// </summary>
    [HttpGet("{id}/coverage")]
    public async Task<ActionResult<object>> GetCourseCoverage(string id)
    {
        var course = await _context.Courses
            .Include(c => c.Lessons)
            .ThenInclude(l => l.Sections)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (course == null)
        {
            return NotFound();
        }

        // TODO: Implement coverage analysis based on lessons and patterns
        // This would analyze which fundamental skills (Sugar Push, Whip, Connection, etc.) are covered
        var coverage = new
        {
            FundamentalsProgress = new
            {
                SugarPush = true,
                LeftSidePass = true,
                RightSidePass = false,
                Whip = false,
                Connection = true,
                Anchor = true,
                Stretch = false,
                Musicality = false
            },
            WeeklyProgress = Enumerable.Range(1, course.DurationWeeks)
                .Select(week => new
                {
                    Week = week,
                    Theme = week <= course.ThemesByWeek.Count ? course.ThemesByWeek[week - 1] : "",
                    CompletedConcepts = new[] { "Sugar Push", "Connection" }, // Mock data
                    PlannedConcepts = new[] { "Left Side Pass" } // Mock data
                }).ToList()
        };

        return Ok(coverage);
    }
}