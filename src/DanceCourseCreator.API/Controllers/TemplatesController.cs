using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DanceCourseCreator.API.Data;
using DanceCourseCreator.API.DTOs;
using DanceCourseCreator.API.Models;
using System.Security.Claims;
using System.Text.Json;

namespace DanceCourseCreator.API.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize] // Temporarily disabled for testing
public class TemplatesController : ControllerBase
{
    private readonly DanceCourseDbContext _context;

    public TemplatesController(DanceCourseDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<TemplateListResponse>> GetTemplates(
        [FromQuery] string? scope = null,
        [FromQuery] string? search = null,
        [FromQuery] string? teamId = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "test-user"; // Default for testing
        
        var query = _context.Templates.AsQueryable();

        // Filter by scope if provided
        if (!string.IsNullOrEmpty(scope) && Enum.TryParse<TemplateScope>(scope, true, out var templateScope))
        {
            query = query.Where(t => t.Scope == templateScope);
        }

        // Filter by search term
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(t => t.Name.Contains(search) || t.Content.Contains(search));
        }

        // Filter by access: user's own templates or team templates they have access to
        if (!string.IsNullOrEmpty(teamId))
        {
            query = query.Where(t => t.Team == teamId);
        }
        else
        {
            query = query.Where(t => t.Owner == userId || (t.Team != null && _context.Users
                .Where(u => u.Id == userId && u.TeamId == t.Team)
                .Any()));
        }

        var totalCount = await query.CountAsync();
        
        var templates = await query
            .OrderByDescending(t => t.UpdatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var templateDtos = templates.Select(t => new TemplateDto
        {
            Id = t.Id,
            Scope = t.Scope.ToString(),
            Name = t.Name,
            Content = t.Content,
            Owner = t.Owner,
            Team = t.Team,
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt
        }).ToList();

        var response = new TemplateListResponse
        {
            Templates = templateDtos,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            HasNext = page * pageSize < totalCount,
            HasPrevious = page > 1
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TemplateDto>> GetTemplate(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "test-user"; // Default for testing
        
        var template = await _context.Templates
            .FirstOrDefaultAsync(t => t.Id == id);

        if (template == null)
        {
            return NotFound();
        }

        // Check access: user owns template or has access via team
        var hasAccess = template.Owner == userId || 
                       (template.Team != null && await _context.Users
                           .AnyAsync(u => u.Id == userId && u.TeamId == template.Team));

        if (!hasAccess)
        {
            return Forbid();
        }

        var templateDto = new TemplateDto
        {
            Id = template.Id,
            Scope = template.Scope.ToString(),
            Name = template.Name,
            Content = template.Content,
            Owner = template.Owner,
            Team = template.Team,
            CreatedAt = template.CreatedAt,
            UpdatedAt = template.UpdatedAt
        };

        return Ok(templateDto);
    }

    [HttpPost]
    // [Authorize(Roles = "Instructor,Editor,Admin")] // Temporarily disabled for testing
    public async Task<ActionResult<TemplateDto>> CreateTemplate(CreateTemplateRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "test-user"; // Default for testing
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        if (!Enum.TryParse<TemplateScope>(request.Scope, true, out var scope))
        {
            return BadRequest("Ogiltig mall-typ (scope)");
        }

        // Validate JSON content
        if (!IsValidJson(request.Content))
        {
            return BadRequest("Ogiltigt JSON-innehåll");
        }

        // If team is specified, verify user has access to that team
        if (!string.IsNullOrEmpty(request.Team))
        {
            var userInTeam = await _context.Users
                .AnyAsync(u => u.Id == userId && u.TeamId == request.Team);
            
            if (!userInTeam)
            {
                return BadRequest("Du har inte tillgång till det angivna teamet");
            }
        }

        var template = new Template
        {
            Scope = scope,
            Name = request.Name,
            Content = request.Content,
            Owner = userId,
            Team = request.Team
        };

        _context.Templates.Add(template);
        await _context.SaveChangesAsync();

        var templateDto = new TemplateDto
        {
            Id = template.Id,
            Scope = template.Scope.ToString(),
            Name = template.Name,
            Content = template.Content,
            Owner = template.Owner,
            Team = template.Team,
            CreatedAt = template.CreatedAt,
            UpdatedAt = template.UpdatedAt
        };

        return CreatedAtAction(nameof(GetTemplate), new { id = template.Id }, templateDto);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Instructor,Editor,Admin")]
    public async Task<ActionResult<TemplateDto>> UpdateTemplate(string id, UpdateTemplateRequest request)
    {
        var template = await _context.Templates.FindAsync(id);

        if (template == null)
        {
            return NotFound();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRole = User.FindFirstValue(ClaimTypes.Role);
        
        // Only template owner or admin can update
        if (template.Owner != userId && userRole != "Admin")
        {
            return Forbid();
        }

        // Validate JSON content
        if (!IsValidJson(request.Content))
        {
            return BadRequest("Ogiltigt JSON-innehåll");
        }

        // If team is specified, verify user has access to that team
        if (!string.IsNullOrEmpty(request.Team))
        {
            var userInTeam = await _context.Users
                .AnyAsync(u => u.Id == userId && u.TeamId == request.Team);
            
            if (!userInTeam)
            {
                return BadRequest("Du har inte tillgång till det angivna teamet");
            }
        }

        template.Name = request.Name;
        template.Content = request.Content;
        template.Team = request.Team;
        template.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        var templateDto = new TemplateDto
        {
            Id = template.Id,
            Scope = template.Scope.ToString(),
            Name = template.Name,
            Content = template.Content,
            Owner = template.Owner,
            Team = template.Team,
            CreatedAt = template.CreatedAt,
            UpdatedAt = template.UpdatedAt
        };

        return Ok(templateDto);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Instructor,Editor,Admin")]
    public async Task<ActionResult> DeleteTemplate(string id)
    {
        var template = await _context.Templates.FindAsync(id);

        if (template == null)
        {
            return NotFound();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRole = User.FindFirstValue(ClaimTypes.Role);
        
        // Only template owner or admin can delete
        if (template.Owner != userId && userRole != "Admin")
        {
            return Forbid();
        }

        _context.Templates.Remove(template);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("{id}/duplicate")]
    // [Authorize(Roles = "Instructor,Editor,Admin")] // Temporarily disabled for testing
    public async Task<ActionResult<DuplicateTemplateResponse>> DuplicateTemplate(string id, DuplicateTemplateRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "test-user"; // Default for testing
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var template = await _context.Templates.FindAsync(id);

        if (template == null)
        {
            return NotFound();
        }

        // Check access: user owns template or has access via team
        var hasAccess = template.Owner == userId || 
                       (template.Team != null && await _context.Users
                           .AnyAsync(u => u.Id == userId && u.TeamId == template.Team));

        if (!hasAccess)
        {
            return Forbid();
        }

        // Use modified content if provided, otherwise use original template content
        var contentToUse = !string.IsNullOrEmpty(request.ModifiedContent) 
            ? request.ModifiedContent 
            : template.Content;

        // Validate JSON content
        if (!IsValidJson(contentToUse))
        {
            return BadRequest("Ogiltigt JSON-innehåll");
        }

        string createdResourceId;
        string resourceType;

        if (template.Scope == TemplateScope.Lesson)
        {
            var lesson = await CreateLessonFromTemplate(template, request.Name, contentToUse, userId);
            createdResourceId = lesson.Id;
            resourceType = "Lesson";
        }
        else // Course
        {
            var course = await CreateCourseFromTemplate(template, request.Name, contentToUse, userId);
            createdResourceId = course.Id;
            resourceType = "Course";
        }

        var response = new DuplicateTemplateResponse
        {
            CreatedResourceId = createdResourceId,
            ResourceType = resourceType,
            Name = request.Name
        };

        return Ok(response);
    }

    [HttpGet("team/{teamId}")]
    public async Task<ActionResult<IEnumerable<TemplateDto>>> GetTeamTemplates(string teamId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "test-user"; // Default for testing
        
        // Verify user has access to the team
        var userInTeam = await _context.Users
            .AnyAsync(u => u.Id == userId && u.TeamId == teamId);
        
        if (!userInTeam)
        {
            return Forbid();
        }

        var templates = await _context.Templates
            .Where(t => t.Team == teamId)
            .OrderByDescending(t => t.UpdatedAt)
            .ToListAsync();

        var templateDtos = templates.Select(t => new TemplateDto
        {
            Id = t.Id,
            Scope = t.Scope.ToString(),
            Name = t.Name,
            Content = t.Content,
            Owner = t.Owner,
            Team = t.Team,
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt
        }).ToList();

        return Ok(templateDtos);
    }

    private async Task<Lesson> CreateLessonFromTemplate(Template template, string name, string content, string userId)
    {
        var lesson = new Lesson
        {
            CreatedBy = userId
        };

        try
        {
            // Parse the template content and map to lesson properties
            var templateData = JsonSerializer.Deserialize<JsonElement>(content);
            
            if (templateData.TryGetProperty("duration", out var durationElement))
            {
                lesson.Duration = durationElement.GetInt32();
            }

            if (templateData.TryGetProperty("sections", out var sectionsElement))
            {
                lesson.SectionsJson = sectionsElement.GetRawText();
            }

            if (templateData.TryGetProperty("notes", out var notesElement))
            {
                lesson.Notes = notesElement.GetString() ?? string.Empty;
            }

            // Add lesson name from request (not from template)
            // This will be handled by adding metadata or as part of the lesson structure
            
        }
        catch (JsonException)
        {
            // If parsing fails, create a basic lesson with the raw content in notes
            lesson.Notes = $"Skapad från mall: {template.Name}\n\nMallinnehåll:\n{content}";
        }

        _context.Lessons.Add(lesson);
        await _context.SaveChangesAsync();

        return lesson;
    }

    private async Task<Course> CreateCourseFromTemplate(Template template, string name, string content, string userId)
    {
        var course = new Course
        {
            Name = name,
            CreatedBy = userId,
            Level = DanceLevel.Beginner // Default, can be overridden by template
        };

        try
        {
            // Parse the template content and map to course properties
            var templateData = JsonSerializer.Deserialize<JsonElement>(content);
            
            if (templateData.TryGetProperty("level", out var levelElement))
            {
                var levelStr = levelElement.GetString();
                if (!string.IsNullOrEmpty(levelStr) && Enum.TryParse<DanceLevel>(levelStr, true, out var level))
                {
                    course.Level = level;
                }
            }

            if (templateData.TryGetProperty("durationWeeks", out var durationElement))
            {
                course.DurationWeeks = durationElement.GetInt32();
            }

            if (templateData.TryGetProperty("goals", out var goalsElement))
            {
                var goals = JsonSerializer.Deserialize<List<string>>(goalsElement.GetRawText()) ?? new List<string>();
                course.Goals = goals;
            }

            if (templateData.TryGetProperty("themesByWeek", out var themesElement))
            {
                var themes = JsonSerializer.Deserialize<List<string>>(themesElement.GetRawText()) ?? new List<string>();
                course.ThemesByWeek = themes;
            }
        }
        catch (JsonException)
        {
            // If parsing fails, set basic properties
            course.DurationWeeks = 6; // Default duration
        }

        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        return course;
    }

    private static bool IsValidJson(string jsonString)
    {
        if (string.IsNullOrWhiteSpace(jsonString))
        {
            return false;
        }

        try
        {
            JsonSerializer.Deserialize<JsonElement>(jsonString);
            return true;
        }
        catch (JsonException)
        {
            return false;
        }
    }
}