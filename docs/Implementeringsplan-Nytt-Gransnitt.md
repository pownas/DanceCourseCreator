# Implementeringsplan: Nytt gränssnitt för DanceCourseCreator

**Version**: 1.0  
**Datum**: 2025-11-29  
**Relaterat**: [Kravspecifikation-Nytt-Gransnitt.md](./Kravspecifikation-Nytt-Gransnitt.md)

---

## 1. Sammanfattning

Denna implementeringsplan beskriver hur det nya gränssnittet för DanceCourseCreator ska utvecklas. Planen är uppdelad i tydliga faser med delmoment, tidsestimeringar, prioriteringar, risker och tekniska rekommendationer.

**Total estimerad tid**: 12-16 veckor  
**Rekommenderad teamstorlek**: 1-2 utvecklare

---

## 2. Fas-översikt

| Fas | Beskrivning | Estimat | Prioritet |
|-----|-------------|---------|-----------|
| **Fas 1** | Grundläggande responsiv struktur | 2-3 veckor | Kritisk |
| **Fas 2** | Turbank och pattern-komponenter | 2-3 veckor | Kritisk |
| **Fas 3** | Lektionsbyggare med dra-och-släpp | 3-4 veckor | Kritisk |
| **Fas 4** | Kursplanering och tidslinje | 2-3 veckor | Hög |
| **Fas 5** | Kompakta vyer och responsiv polish | 2-3 veckor | Hög |
| **Fas 6** | Testning och optimering | 1-2 veckor | Kritisk |

---

## 3. Detaljerad fasplanering

### Fas 1: Grundläggande responsiv struktur (2-3 veckor)

#### 3.1.1 Mål
Etablera en robust, responsiv layoutgrund som stöder mobil, surfplatta och desktop.

#### 3.1.2 Delmoment

| # | Uppgift | Beskrivning | Estimat | Beroenden |
|---|---------|-------------|---------|-----------|
| 1.1 | **Skapa AppLayout.razor** | Huvudlayout med responsiv navigering | 3 dagar | - |
| 1.2 | **Implementera MobileBottomNav** | Bottom navigation för mobil (< 768px) | 2 dagar | 1.1 |
| 1.3 | **Implementera DesktopSidebar** | Kollapserbar sidebar för desktop | 2 dagar | 1.1 |
| 1.4 | **Breakpoint-hantering** | CSS och Blazor-logik för responsiva brytpunkter | 1 dag | 1.1 |
| 1.5 | **Global SearchBar** | Sökfält i header med autocomplete | 2 dagar | 1.1 |
| 1.6 | **ViewMode toggle** | Växla mellan Kompakt/Standard/Utökad | 1 dag | 1.1 |

#### 3.1.3 Teknisk specifikation

**AppLayout.razor**
```razor
@inherits LayoutComponentBase

<MudLayout>
    <MudAppBar Elevation="1">
        <MudHidden Breakpoint="Breakpoint.MdAndUp">
            <MudIconButton Icon="@Icons.Material.Filled.Menu" 
                           OnClick="@ToggleDrawer" />
        </MudHidden>
        <MudText Typo="Typo.h6">DanceCourseCreator</MudText>
        <MudSpacer />
        <SearchBar />
        <MudIconButton Icon="@Icons.Material.Filled.Notifications" />
        <UserMenu />
    </MudAppBar>

    <MudHidden Breakpoint="Breakpoint.SmAndDown">
        <DesktopSidebar />
    </MudHidden>

    <MudMainContent>
        @Body
    </MudMainContent>

    <MudHidden Breakpoint="Breakpoint.MdAndUp">
        <MobileBottomNav />
    </MudHidden>
</MudLayout>
```

**CSS-struktur**
```css
/* _Layout.css */
.sidebar { width: var(--sidebar-width, 240px); }
.sidebar.collapsed { width: var(--sidebar-collapsed, 64px); }
.main-content { 
    margin-left: var(--sidebar-width); 
    transition: margin-left 0.3s ease;
}
@media (max-width: 959px) {
    .main-content { margin-left: 0; margin-bottom: 56px; }
}
```

#### 3.1.4 Acceptanskriterier
- [ ] Layout anpassas korrekt vid alla breakpoints
- [ ] Bottom navigation visas endast på mobil
- [ ] Sidebar kollapsar korrekt
- [ ] Inga layout-shiftar vid resize
- [ ] Sidladdning < 2 sekunder

#### 3.1.5 Risker och mitigering

| Risk | Sannolikhet | Impact | Mitigering |
|------|-------------|--------|------------|
| MudBlazor breakpoint-konflikter | Medel | Medel | Använd MudHidden konsekvent |
| CSS-specificitetsproblem | Låg | Låg | Isolerade komponentstilar |

---

### Fas 2: Turbank och pattern-komponenter (2-3 veckor)

#### 3.2.1 Mål
Implementera en modern, filtrerbar turbank med kort- och tabellvy.

#### 3.2.2 Delmoment

| # | Uppgift | Beskrivning | Estimat | Beroenden |
|---|---------|-------------|---------|-----------|
| 2.1 | **PatternCard.razor** | Individuellt kort för tur/övning | 2 dagar | Fas 1 |
| 2.2 | **PatternList.razor** | Grid/tabell-vy med virtualisering | 3 dagar | 2.1 |
| 2.3 | **PatternFilters.razor** | Filterpanel (nivå, typ, taggar) | 2 dagar | 2.2 |
| 2.4 | **PatternDetailView.razor** | Fullständig detaljvy/drawer | 2 dagar | 2.1 |
| 2.5 | **PatternQuickAdd.razor** | Modal för snabbskapande | 2 dagar | 2.4 |
| 2.6 | **PatternEditor.razor** | Fullständigt redigeringsformulär | 3 dagar | 2.5 |
| 2.7 | **API-integration** | Service för CRUD-operationer | 2 dagar | 2.1-2.6 |

#### 3.2.3 Teknisk specifikation

**PatternCard.razor**
```razor
<MudCard Elevation="@(IsHovered ? 6 : 2)" 
         Class="@($"pattern-card {(Compact ? "compact" : "")}")"
         @onmouseenter="@(() => IsHovered = true)"
         @onmouseleave="@(() => IsHovered = false)">
    <MudCardHeader>
        <CardHeaderContent>
            <MudText Typo="Typo.h6">@Pattern.Name</MudText>
            <LevelBadge Level="@Pattern.Level" />
        </CardHeaderContent>
        <CardHeaderActions>
            @if (Selectable)
            {
                <MudIconButton Icon="@Icons.Material.Filled.Add" 
                               OnClick="@(() => OnSelect.InvokeAsync(Pattern))" />
            }
        </CardHeaderActions>
    </MudCardHeader>
    <MudCardContent>
        <MudText Typo="Typo.body2">
            @Pattern.Type · @Pattern.EstimatedMinutes min
        </MudText>
        <MudChipSet>
            @foreach (var tag in Pattern.Tags.Take(3))
            {
                <MudChip Size="Size.Small">@tag</MudChip>
            }
        </MudChipSet>
    </MudCardContent>
    <MudCardActions>
        <MudButton Size="Size.Small" OnClick="@(() => OnView.InvokeAsync(Pattern))">
            Visa detaljer
        </MudButton>
    </MudCardActions>
</MudCard>
```

**PatternService.cs**
```csharp
public class PatternService : IPatternService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;
    private List<PatternOrExercise>? _cache;
    private DateTime _cacheExpiry;

    public async Task<PagedResult<PatternOrExercise>> GetPatternsAsync(
        PatternFilter filter, 
        int page = 1, 
        int pageSize = 20)
    {
        // Cache-strategi för snabb filtrering
        if (_cache == null || DateTime.Now > _cacheExpiry)
        {
            _cache = await _http.GetFromJsonAsync<List<PatternOrExercise>>("/api/patterns");
            _cacheExpiry = DateTime.Now.AddMinutes(5);
        }

        var filtered = _cache.AsQueryable();
        
        if (filter.Level.HasValue)
            filtered = filtered.Where(p => p.Level == filter.Level);
        if (filter.Type.HasValue)
            filtered = filtered.Where(p => p.Type == filter.Type);
        if (!string.IsNullOrEmpty(filter.SearchTerm))
            filtered = filtered.Where(p => 
                p.Name.Contains(filter.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                p.Description?.Contains(filter.SearchTerm, StringComparison.OrdinalIgnoreCase) == true);

        var total = filtered.Count();
        var items = filtered.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        return new PagedResult<PatternOrExercise> { Items = items, TotalCount = total };
    }
}
```

#### 3.2.4 Acceptanskriterier
- [ ] Turbank visar alla patterns med filtrering
- [ ] Växling mellan kort- och tabellvy fungerar
- [ ] Snabbfilter (nivå-chips) fungerar på mobil
- [ ] Sökresultat visas inom 200ms
- [ ] Detaljvy öppnas i drawer på desktop, helskärm på mobil

#### 3.2.5 Risker och mitigering

| Risk | Sannolikhet | Impact | Mitigering |
|------|-------------|--------|------------|
| Prestandaproblem vid stor turbank | Medel | Hög | Virtualisering med MudVirtualize |
| Cache-inkonsekvens | Låg | Medel | Cache-invalidering vid CRUD |

---

### Fas 3: Lektionsbyggare med dra-och-släpp (3-4 veckor)

#### 3.3.1 Mål
Implementera en intuitiv lektionsbyggare med dra-och-släpp, tidsvalidering och förkunskapskontroll.

#### 3.3.2 Delmoment

| # | Uppgift | Beskrivning | Estimat | Beroenden |
|---|---------|-------------|---------|-----------|
| 3.1 | **LessonBuilder.razor** | Huvudkomponent med layout | 3 dagar | Fas 2 |
| 3.2 | **LessonSection.razor** | Individuell sektion med drop-zone | 2 dagar | 3.1 |
| 3.3 | **SectionPatternPicker.razor** | Mini-turbank för val | 2 dagar | 3.1, Fas 2 |
| 3.4 | **DragDropContainer.razor** | MudBlazor DropContainer wrapper | 3 dagar | 3.1, 3.2 |
| 3.5 | **TimeIndicator.razor** | Real-time tidsberäkning | 1 dag | 3.1 |
| 3.6 | **ValidationService** | Förkunskaps- och tidsvalidering | 3 dagar | 3.1 |
| 3.7 | **LessonTimeline.razor** | Visuell tidslinje per sektion | 2 dagar | 3.5 |
| 3.8 | **Auto-save logik** | LocalStorage utkast var 30 sek | 2 dagar | 3.1 |
| 3.9 | **API för lesson CRUD** | Backend-integration | 2 dagar | 3.1-3.8 |

#### 3.3.3 Teknisk specifikation

**LessonBuilder.razor**
```razor
@page "/lessons/builder"
@page "/lessons/{LessonId}/edit"

<PageTitle>Lektionsbyggare</PageTitle>

<MudGrid>
    @* Vänster panel: Turbank-urval *@
    <MudItem xs="12" md="4">
        <MudPaper Class="pa-4">
            <MudText Typo="Typo.h6">Turbank</MudText>
            <SectionPatternPicker OnPatternSelected="@AddToSelection" />
            <MudDivider Class="my-4" />
            <MudText Typo="Typo.subtitle2">Valda (@_selectedPatterns.Count)</MudText>
            @foreach (var pattern in _selectedPatterns)
            {
                <MudChip OnClose="@(() => RemoveFromSelection(pattern))">
                    @pattern.Name
                </MudChip>
            }
        </MudPaper>
    </MudItem>

    @* Höger panel: Lektionsstruktur *@
    <MudItem xs="12" md="8">
        <MudPaper Class="pa-4">
            <MudStack Row="true" Justify="Justify.SpaceBetween">
                <MudText Typo="Typo.h5">@(_lesson?.Title ?? "Ny lektion")</MudText>
                <TimeIndicator TotalMinutes="@_totalMinutes" 
                               TargetMinutes="@_targetDuration" />
            </MudStack>

            <MudDropContainer T="LessonItem" 
                              Items="@_items"
                              ItemsSelector="@((item, zone) => item.SectionId == zone)"
                              ItemDropped="@OnItemDropped">
                <ChildContent>
                    @foreach (var section in _sections)
                    {
                        <LessonSection Section="@section" 
                                       OnAddPattern="@(() => ShowPatternPicker(section))" />
                    }
                </ChildContent>
                <ItemRenderer>
                    <MudPaper Class="pa-2 ma-1" Elevation="1">
                        <MudStack Row="true">
                            <MudIcon Icon="@Icons.Material.Filled.DragIndicator" />
                            <MudText>@context.Pattern.Name</MudText>
                            <MudSpacer />
                            <MudText Typo="Typo.caption">@context.Pattern.EstimatedMinutes min</MudText>
                        </MudStack>
                    </MudPaper>
                </ItemRenderer>
            </MudDropContainer>

            @* Valideringsvarningar *@
            @if (_warnings.Any())
            {
                <MudAlert Severity="Severity.Warning" Class="mt-4">
                    <ul>
                        @foreach (var warning in _warnings)
                        {
                            <li>@warning.Message</li>
                        }
                    </ul>
                </MudAlert>
            }
        </MudPaper>
    </MudItem>
</MudGrid>

<MudAppBar Bottom="true" Fixed="true" Color="Color.Surface" Elevation="2">
    <MudSpacer />
    <MudButton Variant="Variant.Text" OnClick="@Cancel">Avbryt</MudButton>
    <MudButton Variant="Variant.Outlined" OnClick="@SaveDraft">Spara utkast</MudButton>
    <MudButton Variant="Variant.Filled" Color="Color.Primary" OnClick="@Save">Spara</MudButton>
</MudAppBar>

@code {
    [Parameter] public string? LessonId { get; set; }
    
    private Lesson? _lesson;
    private List<LessonSection> _sections = new();
    private List<LessonItem> _items = new();
    private List<PatternOrExercise> _selectedPatterns = new();
    private List<ValidationWarning> _warnings = new();
    private int _totalMinutes => _items.Sum(i => i.Pattern.EstimatedMinutes);
    private int _targetDuration = 75;

    protected override async Task OnInitializedAsync()
    {
        if (!string.IsNullOrEmpty(LessonId))
        {
            _lesson = await LessonService.GetAsync(LessonId);
            // Populera sektioner och items från lesson
        }
        else
        {
            // Standardsektioner
            _sections = GetDefaultSections();
        }
        
        // Starta auto-save timer
        _autoSaveTimer = new Timer(AutoSave, null, 30000, 30000);
    }

    private void OnItemDropped(MudItemDropInfo<LessonItem> dropItem)
    {
        dropItem.Item.SectionId = dropItem.DropzoneIdentifier;
        ValidateLesson();
    }

    private void ValidateLesson()
    {
        _warnings = ValidationService.ValidateLesson(_lesson, _items, _targetDuration);
    }
}
```

**LessonValidationService.cs**
```csharp
public class LessonValidationService
{
    public List<ValidationWarning> ValidateLesson(
        Lesson lesson, 
        List<LessonItem> items, 
        int targetDuration)
    {
        var warnings = new List<ValidationWarning>();
        
        // 1. Tidskontroll
        var totalTime = items.Sum(i => i.Pattern.EstimatedMinutes);
        if (totalTime > targetDuration + 5)
        {
            warnings.Add(new ValidationWarning
            {
                Type = WarningType.Time,
                Message = $"Total tid ({totalTime} min) överstiger mål ({targetDuration} min)"
            });
        }

        // 2. Antal moment
        var patternCount = items.Count(i => i.Pattern.Type == PatternType.Pattern);
        if (patternCount < 3)
        {
            warnings.Add(new ValidationWarning
            {
                Type = WarningType.Count,
                Message = $"Lektionen har endast {patternCount} mönster (rekommenderat: 3-8)"
            });
        }

        // 3. Förkunskapskontroll
        var taughtPatterns = new HashSet<string>();
        foreach (var item in items.OrderBy(i => GetSectionOrder(i.SectionId)))
        {
            foreach (var prereq in item.Pattern.Prerequisites ?? new List<string>())
            {
                if (!taughtPatterns.Contains(prereq))
                {
                    warnings.Add(new ValidationWarning
                    {
                        Type = WarningType.Prerequisite,
                        Message = $"{item.Pattern.Name} kräver {prereq} som inte finns tidigare i lektionen"
                    });
                }
            }
            taughtPatterns.Add(item.Pattern.Id);
        }

        return warnings;
    }
}
```

#### 3.3.4 Acceptanskriterier
- [ ] Dra-och-släpp fungerar på desktop (mus) och surfplatta (touch)
- [ ] Tidsberäkning uppdateras i realtid
- [ ] Varning visas vid tidsöverskridning
- [ ] Förkunskapsvarningar visas inline
- [ ] Auto-save sparar utkast var 30:e sekund
- [ ] Lektion kan sparas till databas

#### 3.3.5 Risker och mitigering

| Risk | Sannolikhet | Impact | Mitigering |
|------|-------------|--------|------------|
| Touch-dra-och-släpp buggar | Hög | Hög | Fallback: "Lägg till"-knappar |
| State-komplexitet | Medel | Medel | Isolerad state per sektion |
| Auto-save konflikter | Låg | Medel | Versionskontroll med timestamp |

---

### Fas 4: Kursplanering och tidslinje (2-3 veckor)

#### 3.4.1 Mål
Implementera veckovis kursplanering med visuell tidslinje och täckningsanalys.

#### 3.4.2 Delmoment

| # | Uppgift | Beskrivning | Estimat | Beroenden |
|---|---------|-------------|---------|-----------|
| 4.1 | **CoursePlanner.razor** | Huvudkomponent för kursplanering | 3 dagar | Fas 3 |
| 4.2 | **CourseTimeline.razor** | Veckovis tidslinje | 3 dagar | 4.1 |
| 4.3 | **WeekCard.razor** | Individuell veckovy | 2 dagar | 4.2 |
| 4.4 | **CoverageMatrix.razor** | Täckningsanalys-widget | 2 dagar | 4.1 |
| 4.5 | **CourseWizard.razor** | Steg-för-steg-guide | 3 dagar | 4.1 |
| 4.6 | **API för course CRUD** | Backend-integration | 2 dagar | 4.1-4.5 |

#### 3.4.3 Teknisk specifikation

**CourseTimeline.razor**
```razor
<MudPaper Class="pa-4 overflow-x-auto">
    <MudText Typo="Typo.h6" Class="mb-4">Kursplan: @Course.Name</MudText>
    
    <div class="course-timeline">
        <MudStack Row="true" Spacing="2">
            @for (int week = 1; week <= Course.DurationWeeks; week++)
            {
                var weekNum = week;
                <WeekCard Week="@weekNum" 
                          Lesson="@GetLessonForWeek(weekNum)"
                          Patterns="@GetPatternsForWeek(weekNum)"
                          Status="@GetWeekStatus(weekNum)"
                          OnEdit="@(() => EditWeek(weekNum))" />
            }
        </MudStack>
    </div>

    <MudDivider Class="my-4" />
    
    <CoverageMatrix Course="@Course" />
</MudPaper>

@code {
    [Parameter] public Course Course { get; set; } = null!;

    private Lesson? GetLessonForWeek(int week) => 
        Course.Lessons?.FirstOrDefault(l => l.WeekNumber == week);

    private List<PatternOrExercise> GetPatternsForWeek(int week) =>
        GetLessonForWeek(week)?.Items?.Select(i => i.Pattern).ToList() ?? new();

    private WeekStatus GetWeekStatus(int week)
    {
        var lesson = GetLessonForWeek(week);
        if (lesson == null) return WeekStatus.Empty;
        if (lesson.Warnings?.Any() == true) return WeekStatus.Warning;
        return WeekStatus.Complete;
    }
}
```

**CoverageMatrix.razor**
```razor
<MudSimpleTable Dense="true">
    <thead>
        <tr>
            <th>Fundamental</th>
            <th>Status</th>
            <th>Introduktion</th>
            <th>Senaste</th>
        </tr>
    </thead>
    <tbody>
        @foreach (var fundamental in _fundamentals)
        {
            <tr>
                <td>@fundamental.Name</td>
                <td>
                    <MudProgressLinear Color="@GetColor(fundamental.Coverage)" 
                                       Value="@(fundamental.Coverage * 100)" />
                </td>
                <td>@(fundamental.IntroducedWeek?.ToString() ?? "-")</td>
                <td>@(fundamental.LastSeenWeek?.ToString() ?? "-")</td>
            </tr>
        }
    </tbody>
</MudSimpleTable>

@code {
    [Parameter] public Course Course { get; set; } = null!;
    
    private List<FundamentalCoverage> _fundamentals = new();

    protected override void OnParametersSet()
    {
        _fundamentals = CoverageService.CalculateCoverage(Course);
    }
}
```

#### 3.4.4 Acceptanskriterier
- [ ] Veckovis tidslinje visas korrekt
- [ ] Horisontell scroll på mobil fungerar
- [ ] Täckningsmatris visar korrekta procentsatser
- [ ] Varningar visas för saknade fundamentals
- [ ] Kursplaneringsguide fungerar steg-för-steg

---

### Fas 5: Kompakta vyer och responsiv polish (2-3 veckor)

#### 3.5.1 Mål
Implementera kompakta vyer för "på golvet"-användning och finslipa responsivitet.

#### 3.5.2 Delmoment

| # | Uppgift | Beskrivning | Estimat | Beroenden |
|---|---------|-------------|---------|-----------|
| 5.1 | **LessonCompactView.razor** | Ultra-kompakt lektionsvy | 2 dagar | Fas 3 |
| 5.2 | **PatternCompactCard.razor** | Minimal pattern-representation | 1 dag | Fas 2 |
| 5.3 | **ViewModeService** | Global state för vyläge | 1 dag | - |
| 5.4 | **Print stylesheet** | CSS för utskriftsvänlig vy | 1 dag | 5.1 |
| 5.5 | **Touch-optimering** | Förbättrad touch-upplevelse | 3 dagar | Alla |
| 5.6 | **Swipe-gester** | Swipe för snabbåtgärder på mobil | 2 dagar | 5.5 |
| 5.7 | **Responsiv final QA** | Testa alla breakpoints | 2 dagar | Alla |

#### 3.5.3 Teknisk specifikation

**LessonCompactView.razor**
```razor
<MudPaper Class="lesson-compact @(IsPrintMode ? "print-mode" : "")" Elevation="0">
    <header class="lesson-compact-header">
        <span class="lesson-title">@Lesson.Title</span>
        <span class="lesson-meta">
            @Lesson.Level · @Lesson.TotalMinutes min
            @if (Lesson.Date.HasValue)
            {
                <span> · @Lesson.Date.Value.ToString("yyyy-MM-dd")</span>
            }
        </span>
    </header>
    
    <div class="lesson-compact-sections">
        @foreach (var section in Lesson.Sections)
        {
            <div class="section-row @(IsExpanded(section) ? "expanded" : "")"
                 @onclick="@(() => ToggleSection(section))">
                <span class="section-number">@section.Order.</span>
                <span class="section-name">@section.Type (@section.TotalMinutes)</span>
                <span class="section-patterns">
                    @string.Join(", ", section.Items.Select(i => i.Pattern.Name))
                </span>
            </div>
            @if (IsExpanded(section))
            {
                <div class="section-details">
                    @foreach (var item in section.Items)
                    {
                        <div class="pattern-item">
                            • @item.Pattern.Name 
                            <span class="time">@item.Pattern.EstimatedMinutes min</span>
                        </div>
                    }
                    @if (!string.IsNullOrEmpty(section.Notes))
                    {
                        <div class="section-notes">
                            <em>@section.Notes</em>
                        </div>
                    }
                </div>
            }
        }
    </div>
    
    @if (!string.IsNullOrEmpty(Lesson.Notes))
    {
        <footer class="lesson-compact-notes">
            <strong>Noter:</strong> @Lesson.Notes
        </footer>
    }
</MudPaper>

<style>
    .lesson-compact {
        font-family: var(--mud-typography-body1-family);
        font-size: 14px;
        line-height: 1.4;
    }
    .lesson-compact-header {
        display: flex;
        justify-content: space-between;
        padding: 8px 12px;
        border-bottom: 1px solid var(--mud-palette-divider);
    }
    .section-row {
        display: grid;
        grid-template-columns: auto 1fr 2fr;
        gap: 8px;
        padding: 6px 12px;
        cursor: pointer;
    }
    .section-row:hover { background: var(--mud-palette-action-default-hover); }
    
    @media print {
        .lesson-compact { font-size: 12px; }
        .section-row { cursor: default; }
        .section-row:hover { background: none; }
    }
</style>
```

#### 3.5.4 Acceptanskriterier
- [ ] Kompakt vy visar all information på en skärm
- [ ] Sektioner kan expanderas/kollapsas
- [ ] Utskriftsvy ser bra ut i print preview
- [ ] Touch-gester fungerar flytande på mobil
- [ ] Inga layout-problem vid resize

---

### Fas 6: Testning och optimering (1-2 veckor)

#### 3.6.1 Mål
Säkerställa kvalitet, prestanda och tillgänglighet.

#### 3.6.2 Delmoment

| # | Uppgift | Beskrivning | Estimat | Beroenden |
|---|---------|-------------|---------|-----------|
| 6.1 | **Unit tests** | Tester för services och validering | 2 dagar | Alla |
| 6.2 | **E2E tests** | Playwright-tester för huvudflöden | 3 dagar | Alla |
| 6.3 | **Prestandaoptimering** | Profiling och optimering | 2 dagar | Alla |
| 6.4 | **Tillgänglighetsaudit** | WCAG 2.1 AA kontroll | 2 dagar | Alla |
| 6.5 | **Cross-browser test** | Testa Chrome, Firefox, Safari, Edge | 1 dag | Alla |
| 6.6 | **Dokumentation** | Komponentdokumentation | 1 dag | Alla |

#### 3.6.3 Testplan

**Unit tests (xUnit)**
```csharp
public class LessonValidationServiceTests
{
    [Fact]
    public void ValidateLesson_ExceedsTime_ReturnsTimeWarning()
    {
        // Arrange
        var service = new LessonValidationService();
        var items = new List<LessonItem>
        {
            new() { Pattern = new() { EstimatedMinutes = 50 } },
            new() { Pattern = new() { EstimatedMinutes = 40 } }
        };

        // Act
        var warnings = service.ValidateLesson(null, items, 75);

        // Assert
        Assert.Contains(warnings, w => w.Type == WarningType.Time);
    }

    [Fact]
    public void ValidateLesson_MissingPrerequisite_ReturnsPrerequisiteWarning()
    {
        // Arrange
        var service = new LessonValidationService();
        var items = new List<LessonItem>
        {
            new() { Pattern = new() { Name = "LSP", Prerequisites = new[] { "SP" } } }
        };

        // Act
        var warnings = service.ValidateLesson(null, items, 75);

        // Assert
        Assert.Contains(warnings, w => w.Type == WarningType.Prerequisite);
    }
}
```

**E2E tests (Playwright)**
```csharp
[Test]
public async Task LessonBuilder_CreateLesson_Success()
{
    // Navigate to lesson builder
    await Page.GotoAsync("/lessons/builder");
    
    // Add patterns
    await Page.ClickAsync("[data-testid='pattern-picker']");
    await Page.ClickAsync("text=Sugar Push");
    await Page.ClickAsync("[data-testid='add-to-section-Patterns']");
    
    // Check time indicator
    var timeIndicator = await Page.TextContentAsync("[data-testid='time-indicator']");
    Assert.That(timeIndicator, Does.Contain("10 min"));
    
    // Save lesson
    await Page.ClickAsync("text=Spara");
    
    // Verify success
    await Expect(Page.Locator(".mud-snackbar-success")).ToBeVisibleAsync();
}
```

---

## 4. Beroenden och ordning

```
Fas 1 (Layout)
    │
    ▼
Fas 2 (Turbank)
    │
    ├───────────────┐
    ▼               ▼
Fas 3 (Lektion)  Fas 4 (Kurs)
    │               │
    └───────┬───────┘
            ▼
    Fas 5 (Polish)
            │
            ▼
    Fas 6 (Test)
```

**Parallell utveckling möjlig:**
- Fas 2 och Fas 4 kan delvis överlappa
- Fas 5 och Fas 6 kan överlappa

---

## 5. Risker och mitigering

### 5.1 Tekniska risker

| Risk | Sannolikhet | Impact | Mitigering |
|------|-------------|--------|------------|
| **Blazor-prestanda vid komplex dra-och-släpp** | Hög | Hög | Virtualisering, lazy loading, fallback UI |
| **MudBlazor-begränsningar** | Medel | Medel | Custom components vid behov |
| **Touch-kompatibilitet** | Hög | Hög | Tidig testning på enheter, polyfills |
| **State management-komplexitet** | Medel | Medel | Tydlig arkitektur, dokumentation |

### 5.2 Projektrisker

| Risk | Sannolikhet | Impact | Mitigering |
|------|-------------|--------|------------|
| **Scope creep** | Hög | Hög | Strikt prioritering, MVP-fokus |
| **Estimatavvikelser** | Medel | Medel | Buffer i varje fas (20%) |
| **Beroende av befintlig API** | Medel | Medel | Mocka API tidigt |

### 5.3 Riskreduceringsplan

1. **Tidig prototyp** (Vecka 2): Dra-och-släpp-POC för att validera tekniskt tillvägagångssätt
2. **Enhetstestning** (Löpande): Automatiserade tester för kritiska funktioner
3. **Användarfeedback** (Vecka 6): Intern demo och feedback-loop

---

## 6. Tekniska rekommendationer

### 6.1 Arkitekturval

| Område | Rekommendation | Motivering |
|--------|---------------|------------|
| **UI-bibliotek** | MudBlazor 7.x | Redan i projekt, bra responsivitet |
| **State** | Fluxor eller inbyggd | Komplex state i lektionsbyggare |
| **Caching** | In-memory + LocalStorage | Snabb filtrering, offline-stöd |
| **Dra-och-släpp** | MudBlazor DropContainer | Native Blazor, god touch-support |

### 6.2 Designmönster

**Component Composition**
```
LessonBuilder
├── LessonHeader (info, actions)
├── PatternPicker (urval)
├── SectionList
│   └── Section (per typ)
│       └── SectionItems (dra-släpp)
├── ValidationPanel (varningar)
└── ActionBar (spara, avbryt)
```

**Service Layer**
```
Components → Services → API
    │
    └── LocalStorage (utkast, preferenser)
```

### 6.3 Kodstandarder

- **Namngivning**: PascalCase för komponenter, camelCase för privata fält
- **Fil per komponent**: En .razor-fil per komponent
- **CSS**: Scoped styles i komponenten, globala i _Host.cshtml
- **Testning**: Minst 70% coverage på services

---

## 7. Resursplanering

### 7.1 Team-rekommendation

| Roll | Antal | Ansvarsområden |
|------|-------|----------------|
| **Fullstack-utvecklare** | 1-2 | Komponentutveckling, API-integration |
| **UI/UX (deltid)** | 0.5 | Design review, tillgänglighet |
| **QA (deltid)** | 0.5 | Testning, cross-browser |

### 7.2 Tidsplan (exempel: 1 utvecklare)

```
Vecka 1-2:   Fas 1 (Layout)
Vecka 3-5:   Fas 2 (Turbank)
Vecka 6-9:   Fas 3 (Lektionsbyggare)
Vecka 10-12: Fas 4 (Kursplanering)
Vecka 13-14: Fas 5 (Polish)
Vecka 15-16: Fas 6 (Test)
```

### 7.3 Milstolpar

| Milstolpe | Vecka | Leverans |
|-----------|-------|----------|
| **M1: Layout klar** | 2 | Responsiv navigation på alla enheter |
| **M2: Turbank klar** | 5 | Filtrerbar turbank med CRUD |
| **M3: MVP Lektionsbyggare** | 9 | Skapa lektion med dra-och-släpp |
| **M4: Kursplanering** | 12 | Veckovis tidslinje och täckning |
| **M5: Beta-release** | 14 | Fullständig funktionalitet |
| **M6: Release** | 16 | Testad och dokumenterad |

---

## 8. Definition of Done

### 8.1 Per delmoment
- [ ] Kod implementerad enligt specifikation
- [ ] Unit tests skrivna och gröna
- [ ] Code review genomförd
- [ ] Dokumentation uppdaterad
- [ ] Manuellt testad på minst 2 enheter

### 8.2 Per fas
- [ ] Alla delmoment markerade som done
- [ ] Integrationstester gröna
- [ ] Prestandamål uppfyllda
- [ ] Tillgänglighetskrav kontrollerade
- [ ] Demobar för stakeholders

### 8.3 För release
- [ ] Alla faser klara
- [ ] E2E-tester gröna
- [ ] WCAG 2.1 AA audit godkänd
- [ ] Performance budget uppfyllt
- [ ] Användardokumentation klar
- [ ] Deployment pipeline fungerar

---

## 9. Relaterade dokument

- [Kravspecifikation-Nytt-Gransnitt.md](./Kravspecifikation-Nytt-Gransnitt.md) - Detaljerade krav
- [Kravspecifikation.md](../Kravspecifikation.md) - Övergripande systemkrav
- [Analys-Danskursflode.md](./Analys-Danskursflode.md) - Flödesanalys
- [Drag-Drop-Implementation-Guide.md](./Drag-Drop-Implementation-Guide.md) - Teknisk guide

---

**Dokumentversion**: 1.0  
**Skapad**: 2025-11-29  
**Senast uppdaterad**: 2025-11-29  
**Granskad av**: -  
**Godkänd av**: -
