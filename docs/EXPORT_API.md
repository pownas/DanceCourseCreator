# Export API Documentation

## Overview

The Export API allows users to export courses and lessons to PDF or Markdown format. This functionality supports:
- Course plans with lessons, patterns, goals, and schedules
- Individual lesson plans with sections and pattern details
- Configurable export options

## Endpoints

### Export Course

**POST** `/api/export/course/{id}`

Exports a complete course plan including all lessons, patterns, and metadata.

#### Request Parameters

- `id` (path parameter) - The unique identifier of the course to export

#### Request Body

```json
{
  "format": 0,                    // 0 = PDF, 1 = Markdown
  "includeSchedule": true,        // Include weekly themes and schedule
  "includePatternDetails": true   // Include detailed pattern information
}
```

#### Response

Returns the exported file as a downloadable attachment:
- **PDF**: `application/pdf` with filename `course-{id}.pdf`
- **Markdown**: `text/markdown` with filename `course-{id}.md`

#### Example Request

```bash
curl -X POST http://localhost:5139/api/export/course/course-wcs-ht25 \
  -H "Content-Type: application/json" \
  -d '{
    "format": 0,
    "includeSchedule": true,
    "includePatternDetails": true
  }' \
  -o course-export.pdf
```

#### Example Response (PDF)

A PDF file containing:
- Course title and metadata (level, style, duration)
- Goals
- Weekly themes (if includeSchedule is true)
- Lessons with sections and patterns
- Pattern details (if includePatternDetails is true)

#### Example Response (Markdown)

```markdown
# Kursplan: West Coast Swing Grundkurs HT25

## Kursinformation

- **Nivå:** Beginner
- **Dansstil:** WestCoastSwing
- **Typ:** Weekly
- **Längd:** 8 veckor, 8 lektioner
- **Skapad:** 2025-10-06
- **Uppdaterad:** 2025-11-30

## Mål

- Lära sig grundläggande West Coast Swing turer och teknik
- Förstå slot-konceptet och WCS-estetiken

## Teman per vecka

### Vecka 1
Introduktion och grundsteg

...
```

### Export Lesson

**POST** `/api/export/lesson/{id}`

Exports a single lesson plan with all sections and patterns.

#### Request Parameters

- `id` (path parameter) - The unique identifier of the lesson to export

#### Request Body

```json
{
  "format": 0,                    // 0 = PDF, 1 = Markdown
  "includePatternDetails": true   // Include detailed pattern information
}
```

#### Response

Returns the exported file as a downloadable attachment:
- **PDF**: `application/pdf` with filename `lesson-{id}.pdf`
- **Markdown**: `text/markdown` with filename `lesson-{id}.md`

#### Example Request

```bash
curl -X POST http://localhost:5139/api/export/lesson/lesson-wcs-01 \
  -H "Content-Type: application/json" \
  -d '{
    "format": 1,
    "includePatternDetails": true
  }' \
  -o lesson-export.md
```

#### Example Response (Markdown)

```markdown
# Lektionsplan

**Kurs:** West Coast Swing Grundkurs HT25
**Datum:** Oplanerat
**Längd:** 90 minuter

## Anteckningar

Första lektionen fokuserar på grundläggande WCS-koncept

## Sektioner

### Warmup (10 min)

*Grundläggande uppvärmning och introduktion till WCS-musiken*

### Technique (20 min)

*Fokus på frame och anchor step*

#### Frame & Connection

- **Nivå:** Beginner
- **Typ:** Exercise
- **Beskrivning:** Grundläggande övning för rätt frame och connection
...
```

## Error Responses

### 404 Not Found

When the specified course or lesson ID doesn't exist:

```json
{
  "message": "Kurs med ID {id} hittades inte"
}
```

or

```json
{
  "message": "Lektion med ID {id} hittades inte"
}
```

### 400 Bad Request

When an invalid export format is specified:

```json
{
  "message": "Ogiltigt exportformat"
}
```

### 500 Internal Server Error

When an unexpected error occurs during export:

```json
{
  "message": "Ett fel uppstod vid export",
  "error": "Error details"
}
```

## Export Format Details

### PDF Export

The PDF export generates a professionally formatted document with:
- **Header**: Course/Lesson title centered at the top
- **Metadata**: Level, style, duration, dates
- **Content Sections**: Goals, themes, lessons, and patterns
- **Hierarchy**: Clear visual hierarchy with different font sizes
- **Footer**: Export timestamp
- **Page Layout**: A4 size with 2cm margins

When `includePatternDetails` is true, the PDF includes:
- Pattern descriptions
- Estimated time
- Teaching points
- Common mistakes

### Markdown Export

The Markdown export generates a structured document suitable for:
- Further editing in text editors
- Version control systems
- Documentation tools
- Web publishing platforms

The Markdown format uses:
- `#` for main title (H1)
- `##` for major sections (H2)
- `###` for subsections (H3)
- `####` for pattern details (H4)
- `**bold**` for emphasis
- `*italics*` for notes
- `-` for lists

## Export Options

### includeSchedule (Course Export Only)

- **true**: Includes weekly themes and schedule information
- **false**: Omits schedule, showing only basic course information and lessons

### includePatternDetails

- **true**: Includes full pattern details (description, teaching points, common mistakes, steps)
- **false**: Includes only pattern names and basic information

This option is useful for:
- Creating instructor-focused exports with full details (true)
- Creating student-focused exports with simplified content (false)

## Use Cases

### For Instructors

1. **Course Planning**: Export course plans to PDF for review and printing
2. **Lesson Preparation**: Export individual lessons with full pattern details
3. **Sharing**: Export to Markdown for sharing with other instructors via email or chat
4. **Archiving**: Export to PDF for long-term archival storage

### For Course Coordinators

1. **Documentation**: Export courses to Markdown for including in documentation systems
2. **Announcements**: Export simplified course plans (without pattern details) for promotional materials
3. **Reporting**: Export courses for reporting to management or sponsors

### For Dance Schools

1. **Curriculum Planning**: Export multiple courses to compare and plan curriculum
2. **Marketing**: Export course outlines for marketing materials
3. **Quality Assurance**: Review exported lessons to ensure consistency across instructors

## Integration Examples

### JavaScript/TypeScript

```typescript
async function exportCourse(courseId: string, format: 'pdf' | 'markdown') {
  const response = await fetch(`/api/export/course/${courseId}`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({
      format: format === 'pdf' ? 0 : 1,
      includeSchedule: true,
      includePatternDetails: true,
    }),
  });

  if (!response.ok) {
    throw new Error(`Export failed: ${response.statusText}`);
  }

  const blob = await response.blob();
  const url = window.URL.createObjectURL(blob);
  const a = document.createElement('a');
  a.href = url;
  a.download = `course-${courseId}.${format === 'pdf' ? 'pdf' : 'md'}`;
  document.body.appendChild(a);
  a.click();
  window.URL.revokeObjectURL(url);
  document.body.removeChild(a);
}
```

### Python

```python
import requests

def export_lesson(lesson_id, format='pdf', include_details=True):
    url = f'http://localhost:5139/api/export/lesson/{lesson_id}'
    payload = {
        'format': 0 if format == 'pdf' else 1,
        'includePatternDetails': include_details
    }
    
    response = requests.post(url, json=payload)
    
    if response.status_code == 200:
        filename = f'lesson-{lesson_id}.{format}'
        with open(filename, 'wb') as f:
            f.write(response.content)
        print(f'Exported to {filename}')
    else:
        print(f'Export failed: {response.json()}')
```

### C# (.NET)

```csharp
public async Task<byte[]> ExportCourseAsync(string courseId, ExportFormat format)
{
    var client = new HttpClient();
    var request = new ExportRequest
    {
        Format = format,
        IncludeSchedule = true,
        IncludePatternDetails = true
    };
    
    var response = await client.PostAsJsonAsync(
        $"http://localhost:5139/api/export/course/{courseId}", 
        request);
    
    response.EnsureSuccessStatusCode();
    return await response.Content.ReadAsByteArrayAsync();
}
```

## Technical Details

### PDF Generation

The export functionality uses **QuestPDF** (Community License) for PDF generation:
- High-quality PDF rendering
- Professional layout and typography
- Fast generation (< 1 second for typical courses)
- No external dependencies

### Markdown Generation

The Markdown export uses native .NET `StringBuilder` for efficient text generation:
- Standard Markdown format
- Compatible with all major Markdown parsers
- Human-readable and editable

### Performance

Typical export times:
- **Course PDF**: 200-500ms for 8-week course with 8 lessons
- **Course Markdown**: 50-100ms for same course
- **Lesson PDF**: 100-200ms for 90-minute lesson with 5 sections
- **Lesson Markdown**: 20-50ms for same lesson

### Limitations

- Maximum recommended course size: 52 weeks, 20 lessons
- Maximum recommended lesson size: 300 minutes, 10 sections
- PDF file size typically < 500KB for standard courses
- Markdown file size typically < 100KB for standard courses

## Security Considerations

The export endpoints currently do not enforce authentication (marked as `// [Authorize]` temporarily disabled for testing). In production:

1. Enable authentication by uncommenting `[Authorize]` attributes
2. Implement role-based access control
3. Add rate limiting to prevent abuse
4. Log all export operations for auditing

## Future Enhancements

Potential improvements for future versions:
- HTML export format
- Excel export for course schedules
- Custom PDF templates
- Batch export (multiple courses/lessons)
- Email integration for direct sharing
- Cloud storage integration (Google Drive, Dropbox)
- Print-optimized layouts
- Internationalization (multiple languages)
