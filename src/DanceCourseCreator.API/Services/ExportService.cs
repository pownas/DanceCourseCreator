using DanceCourseCreator.API.Data;
using DanceCourseCreator.API.Models;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Text;

namespace DanceCourseCreator.API.Services;

public class ExportService
{
    private readonly DanceCourseDbContext _context;
    private readonly ILogger<ExportService> _logger;

    public ExportService(DanceCourseDbContext context, ILogger<ExportService> logger)
    {
        _context = context;
        _logger = logger;
        
        // Configure QuestPDF license (Community license for open-source/personal projects)
        QuestPDF.Settings.License = LicenseType.Community;
    }

    /// <summary>
    /// Export a course to PDF format
    /// </summary>
    public async Task<byte[]> ExportCourseToPdfAsync(string courseId, bool includeSchedule = true, bool includePatternDetails = true)
    {
        _logger.LogInformation("Exporting course {CourseId} to PDF", courseId);
        
        var course = await _context.Courses
            .Include(c => c.Lessons)
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null)
        {
            throw new InvalidOperationException($"Kurs med ID {courseId} hittades inte");
        }

        // Get all patterns used in the course
        var patternIds = new HashSet<string>();
        foreach (var lesson in course.Lessons)
        {
            foreach (var section in lesson.Sections)
            {
                patternIds.UnionWith(section.Items);
            }
        }

        var patterns = await _context.Patterns
            .Where(p => patternIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id, p => p);

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(11));

                page.Header()
                    .AlignCenter()
                    .Text(text =>
                    {
                        text.Span("Kursplan: ").Bold();
                        text.Span(course.Name).FontSize(16);
                    });

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(column =>
                    {
                        column.Spacing(10);

                        // Course information
                        column.Item().Text(text =>
                        {
                            text.Span("Nivå: ").Bold();
                            text.Span(course.Level.ToString());
                        });

                        column.Item().Text(text =>
                        {
                            text.Span("Dansstil: ").Bold();
                            text.Span(course.DanceStyle.ToString());
                        });

                        column.Item().Text(text =>
                        {
                            text.Span("Typ: ").Bold();
                            text.Span(course.Type.ToString());
                        });

                        column.Item().Text(text =>
                        {
                            text.Span("Längd: ").Bold();
                            text.Span($"{course.DurationWeeks} veckor, {course.PlannedLessonCount} lektioner");
                        });

                        // Goals
                        if (course.Goals.Any())
                        {
                            column.Item().PaddingTop(10).Text("Mål").Bold().FontSize(14);
                            foreach (var goal in course.Goals)
                            {
                                column.Item().Text($"• {goal}");
                            }
                        }

                        // Themes by week
                        if (includeSchedule && course.ThemesByWeek.Any())
                        {
                            column.Item().PaddingTop(10).Text("Teman per vecka").Bold().FontSize(14);
                            for (int i = 0; i < course.ThemesByWeek.Count; i++)
                            {
                                if (!string.IsNullOrWhiteSpace(course.ThemesByWeek[i]))
                                {
                                    column.Item().Text($"Vecka {i + 1}: {course.ThemesByWeek[i]}");
                                }
                            }
                        }

                        // Lessons
                        if (course.Lessons.Any())
                        {
                            column.Item().PaddingTop(10).Text("Lektioner").Bold().FontSize(14);
                            
                            foreach (var lesson in course.Lessons.OrderBy(l => l.Date))
                            {
                                column.Item().PaddingTop(10).Column(lessonColumn =>
                                {
                                    lessonColumn.Item().Text(text =>
                                    {
                                        text.Span($"Lektion {lesson.Date?.ToString("yyyy-MM-dd") ?? "Oplanerad"}").Bold();
                                        text.Span($" ({lesson.Duration} min)");
                                    });

                                    if (!string.IsNullOrWhiteSpace(lesson.Notes))
                                    {
                                        lessonColumn.Item().Text($"Anteckningar: {lesson.Notes}");
                                    }

                                    // Lesson sections
                                    foreach (var section in lesson.Sections)
                                    {
                                        lessonColumn.Item().PaddingTop(5).Column(sectionColumn =>
                                        {
                                            sectionColumn.Item().Text(text =>
                                            {
                                                text.Span($"{section.Type} ").Bold();
                                                text.Span($"({section.AllocatedMinutes} min)");
                                            });

                                            if (!string.IsNullOrWhiteSpace(section.Notes))
                                            {
                                                sectionColumn.Item().Text($"Anteckningar: {section.Notes}");
                                            }

                                            // Pattern items
                                            if (section.Items.Any())
                                            {
                                                foreach (var itemId in section.Items)
                                                {
                                                    if (patterns.TryGetValue(itemId, out var pattern))
                                                    {
                                                        sectionColumn.Item().Text($"  • {pattern.Name}");
                                                        
                                                        if (includePatternDetails)
                                                        {
                                                            if (!string.IsNullOrWhiteSpace(pattern.Description))
                                                            {
                                                                sectionColumn.Item().Text($"    {pattern.Description}").FontSize(9);
                                                            }
                                                            
                                                            if (pattern.EstimatedMinutes > 0)
                                                            {
                                                                sectionColumn.Item().Text($"    Tid: {pattern.EstimatedMinutes} min").FontSize(9);
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        sectionColumn.Item().Text($"  • {itemId}");
                                                    }
                                                }
                                            }
                                        });
                                    }
                                });
                            }
                        }
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(text =>
                    {
                        text.Span("Skapad: ");
                        text.Span(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                    });
            });
        });

        return document.GeneratePdf();
    }

    /// <summary>
    /// Export a course to Markdown format
    /// </summary>
    public async Task<string> ExportCourseToMarkdownAsync(string courseId, bool includeSchedule = true, bool includePatternDetails = true)
    {
        _logger.LogInformation("Exporting course {CourseId} to Markdown", courseId);
        
        var course = await _context.Courses
            .Include(c => c.Lessons)
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null)
        {
            throw new InvalidOperationException($"Kurs med ID {courseId} hittades inte");
        }

        // Get all patterns used in the course
        var patternIds = new HashSet<string>();
        foreach (var lesson in course.Lessons)
        {
            foreach (var section in lesson.Sections)
            {
                patternIds.UnionWith(section.Items);
            }
        }

        var patterns = await _context.Patterns
            .Where(p => patternIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id, p => p);

        var sb = new StringBuilder();
        
        // Title
        sb.AppendLine($"# Kursplan: {course.Name}");
        sb.AppendLine();

        // Course information
        sb.AppendLine("## Kursinformation");
        sb.AppendLine();
        sb.AppendLine($"- **Nivå:** {course.Level}");
        sb.AppendLine($"- **Dansstil:** {course.DanceStyle}");
        sb.AppendLine($"- **Typ:** {course.Type}");
        sb.AppendLine($"- **Längd:** {course.DurationWeeks} veckor, {course.PlannedLessonCount} lektioner");
        sb.AppendLine($"- **Skapad:** {course.CreatedAt:yyyy-MM-dd}");
        sb.AppendLine($"- **Uppdaterad:** {course.UpdatedAt:yyyy-MM-dd}");
        sb.AppendLine();

        // Goals
        if (course.Goals.Any())
        {
            sb.AppendLine("## Mål");
            sb.AppendLine();
            foreach (var goal in course.Goals)
            {
                sb.AppendLine($"- {goal}");
            }
            sb.AppendLine();
        }

        // Themes by week
        if (includeSchedule && course.ThemesByWeek.Any())
        {
            sb.AppendLine("## Teman per vecka");
            sb.AppendLine();
            for (int i = 0; i < course.ThemesByWeek.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(course.ThemesByWeek[i]))
                {
                    sb.AppendLine($"### Vecka {i + 1}");
                    sb.AppendLine(course.ThemesByWeek[i]);
                    sb.AppendLine();
                }
            }
        }

        // Lessons
        if (course.Lessons.Any())
        {
            sb.AppendLine("## Lektioner");
            sb.AppendLine();
            
            foreach (var lesson in course.Lessons.OrderBy(l => l.Date))
            {
                sb.AppendLine($"### Lektion {lesson.Date?.ToString("yyyy-MM-dd") ?? "Oplanerad"} ({lesson.Duration} min)");
                sb.AppendLine();

                if (!string.IsNullOrWhiteSpace(lesson.Notes))
                {
                    sb.AppendLine($"**Anteckningar:** {lesson.Notes}");
                    sb.AppendLine();
                }

                // Lesson sections
                foreach (var section in lesson.Sections)
                {
                    sb.AppendLine($"#### {section.Type} ({section.AllocatedMinutes} min)");
                    sb.AppendLine();

                    if (!string.IsNullOrWhiteSpace(section.Notes))
                    {
                        sb.AppendLine($"*{section.Notes}*");
                        sb.AppendLine();
                    }

                    // Pattern items
                    if (section.Items.Any())
                    {
                        foreach (var itemId in section.Items)
                        {
                            if (patterns.TryGetValue(itemId, out var pattern))
                            {
                                sb.AppendLine($"- **{pattern.Name}** ({pattern.Level})");
                                
                                if (includePatternDetails)
                                {
                                    if (!string.IsNullOrWhiteSpace(pattern.Description))
                                    {
                                        sb.AppendLine($"  - {pattern.Description}");
                                    }
                                    
                                    if (pattern.EstimatedMinutes > 0)
                                    {
                                        sb.AppendLine($"  - Tid: {pattern.EstimatedMinutes} min");
                                    }
                                    
                                    if (pattern.TeachingPoints.Any())
                                    {
                                        sb.AppendLine($"  - Undervisningspunkter:");
                                        foreach (var point in pattern.TeachingPoints)
                                        {
                                            sb.AppendLine($"    - {point}");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                sb.AppendLine($"- {itemId}");
                            }
                        }
                        sb.AppendLine();
                    }
                }
            }
        }

        sb.AppendLine("---");
        sb.AppendLine($"*Exporterad: {DateTime.Now:yyyy-MM-dd HH:mm}*");

        return sb.ToString();
    }

    /// <summary>
    /// Export a lesson to PDF format
    /// </summary>
    public async Task<byte[]> ExportLessonToPdfAsync(string lessonId, bool includePatternDetails = true)
    {
        _logger.LogInformation("Exporting lesson {LessonId} to PDF", lessonId);
        
        var lesson = await _context.Lessons
            .Include(l => l.Course)
            .FirstOrDefaultAsync(l => l.Id == lessonId);

        if (lesson == null)
        {
            throw new InvalidOperationException($"Lektion med ID {lessonId} hittades inte");
        }

        // Get all patterns used in the lesson
        var patternIds = new HashSet<string>();
        foreach (var section in lesson.Sections)
        {
            patternIds.UnionWith(section.Items);
        }

        var patterns = await _context.Patterns
            .Where(p => patternIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id, p => p);

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(11));

                page.Header()
                    .AlignCenter()
                    .Text(text =>
                    {
                        text.Span("Lektionsplan").Bold().FontSize(16);
                    });

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(column =>
                    {
                        column.Spacing(10);

                        // Lesson information
                        if (lesson.Course != null)
                        {
                            column.Item().Text(text =>
                            {
                                text.Span("Kurs: ").Bold();
                                text.Span(lesson.Course.Name);
                            });
                        }

                        column.Item().Text(text =>
                        {
                            text.Span("Datum: ").Bold();
                            text.Span(lesson.Date?.ToString("yyyy-MM-dd") ?? "Oplanerat");
                        });

                        column.Item().Text(text =>
                        {
                            text.Span("Längd: ").Bold();
                            text.Span($"{lesson.Duration} minuter");
                        });

                        if (!string.IsNullOrWhiteSpace(lesson.Notes))
                        {
                            column.Item().Text(text =>
                            {
                                text.Span("Anteckningar: ").Bold();
                                text.Span(lesson.Notes);
                            });
                        }

                        // Sections
                        column.Item().PaddingTop(10).Text("Sektioner").Bold().FontSize(14);

                        foreach (var section in lesson.Sections)
                        {
                            column.Item().PaddingTop(10).Column(sectionColumn =>
                            {
                                sectionColumn.Item().Text(text =>
                                {
                                    text.Span($"{section.Type} ").Bold().FontSize(12);
                                    text.Span($"({section.AllocatedMinutes} min)");
                                });

                                if (!string.IsNullOrWhiteSpace(section.Notes))
                                {
                                    sectionColumn.Item().Text($"Anteckningar: {section.Notes}");
                                }

                                // Pattern items
                                if (section.Items.Any())
                                {
                                    sectionColumn.Item().PaddingTop(5).Column(itemsColumn =>
                                    {
                                        foreach (var itemId in section.Items)
                                        {
                                            if (patterns.TryGetValue(itemId, out var pattern))
                                            {
                                                itemsColumn.Item().PaddingTop(5).Column(patternColumn =>
                                                {
                                                    patternColumn.Item().Text($"• {pattern.Name}").Bold();
                                                    
                                                    if (includePatternDetails)
                                                    {
                                                        patternColumn.Item().Text($"  Nivå: {pattern.Level}").FontSize(9);
                                                        
                                                        if (!string.IsNullOrWhiteSpace(pattern.Description))
                                                        {
                                                            patternColumn.Item().Text($"  {pattern.Description}").FontSize(9);
                                                        }
                                                        
                                                        if (pattern.EstimatedMinutes > 0)
                                                        {
                                                            patternColumn.Item().Text($"  Tid: {pattern.EstimatedMinutes} min").FontSize(9);
                                                        }
                                                        
                                                        if (pattern.TeachingPoints.Any())
                                                        {
                                                            patternColumn.Item().Text("  Undervisningspunkter:").FontSize(9);
                                                            foreach (var point in pattern.TeachingPoints)
                                                            {
                                                                patternColumn.Item().Text($"    - {point}").FontSize(9);
                                                            }
                                                        }
                                                        
                                                        if (pattern.CommonMistakes.Any())
                                                        {
                                                            patternColumn.Item().Text("  Vanliga fel:").FontSize(9);
                                                            foreach (var mistake in pattern.CommonMistakes)
                                                            {
                                                                patternColumn.Item().Text($"    - {mistake}").FontSize(9);
                                                            }
                                                        }
                                                    }
                                                });
                                            }
                                            else
                                            {
                                                itemsColumn.Item().Text($"• {itemId}");
                                            }
                                        }
                                    });
                                }
                            });
                        }

                        // Summary
                        column.Item().PaddingTop(15).Text("Sammanfattning").Bold().FontSize(14);
                        column.Item().Text($"Total tid: {lesson.Duration} minuter");
                        column.Item().Text($"Antal sektioner: {lesson.Sections.Count}");
                        column.Item().Text($"Totalt antal aktiviteter: {lesson.Sections.Sum(s => s.Items.Count)}");
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(text =>
                    {
                        text.Span("Skapad: ");
                        text.Span(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                    });
            });
        });

        return document.GeneratePdf();
    }

    /// <summary>
    /// Export a lesson to Markdown format
    /// </summary>
    public async Task<string> ExportLessonToMarkdownAsync(string lessonId, bool includePatternDetails = true)
    {
        _logger.LogInformation("Exporting lesson {LessonId} to Markdown", lessonId);
        
        var lesson = await _context.Lessons
            .Include(l => l.Course)
            .FirstOrDefaultAsync(l => l.Id == lessonId);

        if (lesson == null)
        {
            throw new InvalidOperationException($"Lektion med ID {lessonId} hittades inte");
        }

        // Get all patterns used in the lesson
        var patternIds = new HashSet<string>();
        foreach (var section in lesson.Sections)
        {
            patternIds.UnionWith(section.Items);
        }

        var patterns = await _context.Patterns
            .Where(p => patternIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id, p => p);

        var sb = new StringBuilder();
        
        // Title
        sb.AppendLine("# Lektionsplan");
        sb.AppendLine();

        // Lesson information
        if (lesson.Course != null)
        {
            sb.AppendLine($"**Kurs:** {lesson.Course.Name}");
        }
        sb.AppendLine($"**Datum:** {lesson.Date?.ToString("yyyy-MM-dd") ?? "Oplanerat"}");
        sb.AppendLine($"**Längd:** {lesson.Duration} minuter");
        sb.AppendLine();

        if (!string.IsNullOrWhiteSpace(lesson.Notes))
        {
            sb.AppendLine("## Anteckningar");
            sb.AppendLine();
            sb.AppendLine(lesson.Notes);
            sb.AppendLine();
        }

        // Sections
        sb.AppendLine("## Sektioner");
        sb.AppendLine();

        foreach (var section in lesson.Sections)
        {
            sb.AppendLine($"### {section.Type} ({section.AllocatedMinutes} min)");
            sb.AppendLine();

            if (!string.IsNullOrWhiteSpace(section.Notes))
            {
                sb.AppendLine($"*{section.Notes}*");
                sb.AppendLine();
            }

            // Pattern items
            if (section.Items.Any())
            {
                foreach (var itemId in section.Items)
                {
                    if (patterns.TryGetValue(itemId, out var pattern))
                    {
                        sb.AppendLine($"#### {pattern.Name}");
                        sb.AppendLine();
                        sb.AppendLine($"- **Nivå:** {pattern.Level}");
                        sb.AppendLine($"- **Typ:** {pattern.Type}");
                        
                        if (includePatternDetails)
                        {
                            if (!string.IsNullOrWhiteSpace(pattern.Description))
                            {
                                sb.AppendLine($"- **Beskrivning:** {pattern.Description}");
                            }
                            
                            if (pattern.EstimatedMinutes > 0)
                            {
                                sb.AppendLine($"- **Tid:** {pattern.EstimatedMinutes} minuter");
                            }
                            
                            if (pattern.TeachingPoints.Any())
                            {
                                sb.AppendLine("- **Undervisningspunkter:**");
                                foreach (var point in pattern.TeachingPoints)
                                {
                                    sb.AppendLine($"  - {point}");
                                }
                            }
                            
                            if (pattern.CommonMistakes.Any())
                            {
                                sb.AppendLine("- **Vanliga fel:**");
                                foreach (var mistake in pattern.CommonMistakes)
                                {
                                    sb.AppendLine($"  - {mistake}");
                                }
                            }
                            
                            if (pattern.Steps.Any())
                            {
                                sb.AppendLine("- **Steg:**");
                                foreach (var step in pattern.Steps)
                                {
                                    sb.AppendLine($"  - {step}");
                                }
                            }
                        }
                        
                        sb.AppendLine();
                    }
                    else
                    {
                        sb.AppendLine($"- {itemId}");
                        sb.AppendLine();
                    }
                }
            }
        }

        // Summary
        sb.AppendLine("## Sammanfattning");
        sb.AppendLine();
        sb.AppendLine($"- **Total tid:** {lesson.Duration} minuter");
        sb.AppendLine($"- **Antal sektioner:** {lesson.Sections.Count}");
        sb.AppendLine($"- **Totalt antal aktiviteter:** {lesson.Sections.Sum(s => s.Items.Count)}");
        sb.AppendLine();

        sb.AppendLine("---");
        sb.AppendLine($"*Exporterad: {DateTime.Now:yyyy-MM-dd HH:mm}*");

        return sb.ToString();
    }
}
