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
public class PatternsController : ControllerBase
{
    private readonly DanceCourseDbContext _context;

    public PatternsController(DanceCourseDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PatternDto>>> GetPatterns(
        [FromQuery] string? type = null,
        [FromQuery] string? level = null,
        [FromQuery] string? danceStyle = null,
        [FromQuery] string? search = null,
        [FromQuery] List<string>? tags = null)
    {
        var query = _context.Patterns.AsQueryable();

        if (!string.IsNullOrEmpty(type) && Enum.TryParse<PatternType>(type, true, out var patternType))
        {
            query = query.Where(p => p.Type == patternType);
        }

        if (!string.IsNullOrEmpty(level) && Enum.TryParse<DanceLevel>(level, true, out var danceLevel))
        {
            query = query.Where(p => p.Level == danceLevel);
        }

        if (!string.IsNullOrEmpty(danceStyle) && Enum.TryParse<DanceStyle>(danceStyle, true, out var style))
        {
            query = query.Where(p => p.DanceStyle == style);
        }

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(p => p.Name.Contains(search) || p.Description.Contains(search));
        }

        if (tags != null && tags.Any())
        {
            foreach (var tag in tags)
            {
                query = query.Where(p => p.TagsJson.Contains($"\"{tag}\""));
            }
        }

        var patterns = await query.ToListAsync();

        var patternDtos = patterns.Select(p => new PatternDto
        {
            Id = p.Id,
            Type = p.Type.ToString(),
            Name = p.Name,
            Aliases = p.Aliases,
            Level = p.Level.ToString(),
            DanceStyle = p.DanceStyle.ToString(),
            Description = p.Description,
            Steps = p.Steps,
            Counts = p.Counts,
            Holds = p.Holds,
            Slot = p.Slot,
            Rotations = p.Rotations,
            Prerequisites = p.Prerequisites,
            Related = p.Related,
            TeachingPoints = p.TeachingPoints,
            CommonMistakes = p.CommonMistakes,
            Variations = p.Variations,
            EstimatedMinutes = p.EstimatedMinutes,
            BpmRange = new BpmRangeDto { Min = p.BpmRangeMin, Max = p.BpmRangeMax },
            Tags = p.Tags,
            MediaLinks = p.MediaLinks,
            CreatedBy = p.CreatedBy,
            UpdatedAt = p.UpdatedAt
        }).ToList();

        return Ok(patternDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PatternDto>> GetPattern(string id)
    {
        var pattern = await _context.Patterns.FindAsync(id);

        if (pattern == null)
        {
            return NotFound();
        }

        var patternDto = new PatternDto
        {
            Id = pattern.Id,
            Type = pattern.Type.ToString(),
            Name = pattern.Name,
            Aliases = pattern.Aliases,
            Level = pattern.Level.ToString(),
            DanceStyle = pattern.DanceStyle.ToString(),
            Description = pattern.Description,
            Steps = pattern.Steps,
            Counts = pattern.Counts,
            Holds = pattern.Holds,
            Slot = pattern.Slot,
            Rotations = pattern.Rotations,
            Prerequisites = pattern.Prerequisites,
            Related = pattern.Related,
            TeachingPoints = pattern.TeachingPoints,
            CommonMistakes = pattern.CommonMistakes,
            Variations = pattern.Variations,
            EstimatedMinutes = pattern.EstimatedMinutes,
            BpmRange = new BpmRangeDto { Min = pattern.BpmRangeMin, Max = pattern.BpmRangeMax },
            Tags = pattern.Tags,
            MediaLinks = pattern.MediaLinks,
            CreatedBy = pattern.CreatedBy,
            UpdatedAt = pattern.UpdatedAt
        };

        return Ok(patternDto);
    }

    [HttpPost]
    // [Authorize(Roles = "Instructor,Editor,Admin")] // Temporarily disabled for testing
    public async Task<ActionResult<PatternDto>> CreatePattern(CreatePatternRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "test-user"; // Default for testing
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        if (!Enum.TryParse<PatternType>(request.Type, true, out var type))
        {
            return BadRequest("Invalid pattern type");
        }

        if (!Enum.TryParse<DanceLevel>(request.Level, true, out var level))
        {
            return BadRequest("Invalid dance level");
        }

        if (!Enum.TryParse<DanceStyle>(request.DanceStyle, true, out var danceStyle))
        {
            return BadRequest("Invalid dance style");
        }

        var pattern = new PatternOrExercise
        {
            Type = type,
            Name = request.Name,
            Aliases = request.Aliases,
            Level = level,
            DanceStyle = danceStyle,
            Description = request.Description,
            Steps = request.Steps,
            Counts = request.Counts,
            Holds = request.Holds,
            Slot = request.Slot,
            Rotations = request.Rotations,
            Prerequisites = request.Prerequisites,
            Related = request.Related,
            TeachingPoints = request.TeachingPoints,
            CommonMistakes = request.CommonMistakes,
            Variations = request.Variations,
            EstimatedMinutes = request.EstimatedMinutes,
            BpmRangeMin = request.BpmRange.Min,
            BpmRangeMax = request.BpmRange.Max,
            Tags = request.Tags,
            MediaLinks = request.MediaLinks,
            CreatedBy = userId
        };

        _context.Patterns.Add(pattern);
        await _context.SaveChangesAsync();

        var patternDto = new PatternDto
        {
            Id = pattern.Id,
            Type = pattern.Type.ToString(),
            Name = pattern.Name,
            Aliases = pattern.Aliases,
            Level = pattern.Level.ToString(),
            DanceStyle = pattern.DanceStyle.ToString(),
            Description = pattern.Description,
            Steps = pattern.Steps,
            Counts = pattern.Counts,
            Holds = pattern.Holds,
            Slot = pattern.Slot,
            Rotations = pattern.Rotations,
            Prerequisites = pattern.Prerequisites,
            Related = pattern.Related,
            TeachingPoints = pattern.TeachingPoints,
            CommonMistakes = pattern.CommonMistakes,
            Variations = pattern.Variations,
            EstimatedMinutes = pattern.EstimatedMinutes,
            BpmRange = new BpmRangeDto { Min = pattern.BpmRangeMin, Max = pattern.BpmRangeMax },
            Tags = pattern.Tags,
            MediaLinks = pattern.MediaLinks,
            CreatedBy = pattern.CreatedBy,
            UpdatedAt = pattern.UpdatedAt
        };

        return CreatedAtAction(nameof(GetPattern), new { id = pattern.Id }, patternDto);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Instructor,Editor,Admin")]
    public async Task<ActionResult<PatternDto>> UpdatePattern(string id, CreatePatternRequest request)
    {
        var pattern = await _context.Patterns.FindAsync(id);

        if (pattern == null)
        {
            return NotFound();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRole = User.FindFirstValue(ClaimTypes.Role);
        
        if (pattern.CreatedBy != userId && userRole != "Admin")
        {
            return Forbid();
        }

        if (!Enum.TryParse<PatternType>(request.Type, true, out var type))
        {
            return BadRequest("Invalid pattern type");
        }

        if (!Enum.TryParse<DanceLevel>(request.Level, true, out var level))
        {
            return BadRequest("Invalid dance level");
        }

        if (!Enum.TryParse<DanceStyle>(request.DanceStyle, true, out var danceStyle))
        {
            return BadRequest("Invalid dance style");
        }

        pattern.Type = type;
        pattern.Name = request.Name;
        pattern.Aliases = request.Aliases;
        pattern.Level = level;
        pattern.DanceStyle = danceStyle;
        pattern.Description = request.Description;
        pattern.Steps = request.Steps;
        pattern.Counts = request.Counts;
        pattern.Holds = request.Holds;
        pattern.Slot = request.Slot;
        pattern.Rotations = request.Rotations;
        pattern.Prerequisites = request.Prerequisites;
        pattern.Related = request.Related;
        pattern.TeachingPoints = request.TeachingPoints;
        pattern.CommonMistakes = request.CommonMistakes;
        pattern.Variations = request.Variations;
        pattern.EstimatedMinutes = request.EstimatedMinutes;
        pattern.BpmRangeMin = request.BpmRange.Min;
        pattern.BpmRangeMax = request.BpmRange.Max;
        pattern.Tags = request.Tags;
        pattern.MediaLinks = request.MediaLinks;
        pattern.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        var patternDto = new PatternDto
        {
            Id = pattern.Id,
            Type = pattern.Type.ToString(),
            Name = pattern.Name,
            Aliases = pattern.Aliases,
            Level = pattern.Level.ToString(),
            DanceStyle = pattern.DanceStyle.ToString(),
            Description = pattern.Description,
            Steps = pattern.Steps,
            Counts = pattern.Counts,
            Holds = pattern.Holds,
            Slot = pattern.Slot,
            Rotations = pattern.Rotations,
            Prerequisites = pattern.Prerequisites,
            Related = pattern.Related,
            TeachingPoints = pattern.TeachingPoints,
            CommonMistakes = pattern.CommonMistakes,
            Variations = pattern.Variations,
            EstimatedMinutes = pattern.EstimatedMinutes,
            BpmRange = new BpmRangeDto { Min = pattern.BpmRangeMin, Max = pattern.BpmRangeMax },
            Tags = pattern.Tags,
            MediaLinks = pattern.MediaLinks,
            CreatedBy = pattern.CreatedBy,
            UpdatedAt = pattern.UpdatedAt
        };

        return Ok(patternDto);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Instructor,Editor,Admin")]
    public async Task<ActionResult> DeletePattern(string id)
    {
        var pattern = await _context.Patterns.FindAsync(id);

        if (pattern == null)
        {
            return NotFound();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRole = User.FindFirstValue(ClaimTypes.Role);
        
        if (pattern.CreatedBy != userId && userRole != "Admin")
        {
            return Forbid();
        }

        _context.Patterns.Remove(pattern);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}