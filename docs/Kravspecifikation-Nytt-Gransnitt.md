# Kravspecifikation: Nytt gränssnitt för DanceCourseCreator

**Version**: 1.0  
**Datum**: 2025-11-29  
**Relaterat issue**: Kravspecifikation och implementeringsplan för nytt gränssnitt

---

## 1. Sammanfattning

Denna kravspecifikation beskriver det nya gränssnittet för DanceCourseCreator web-projektet. Målet är att skapa ett tydligare, mer intuitivt och responsivt gränssnitt som fungerar sömlöst på mobil, surfplatta och desktop. Fokus ligger på enkelhet, kompakta vyer och effektiva användarflöden för instruktörer som hanterar lektioner, skapar turer och lägger till information.

---

## 2. Syfte och mål

### 2.1 Syfte
Skapa ett moderniserat gränssnitt som förenklar och effektiviserar det dagliga arbetet för dansinstruktörer genom:
- Intuitivt flöde för att hantera lektioner och kurser
- Snabb och enkel process för att skapa och redigera turer (patterns)
- Responsiv design som fungerar väl på alla enheter
- Kompakta vyer som maximerar informationsdensitet utan att offra användarvänlighet

### 2.2 Mål
- **Användarupplevelse**: Reducera antalet klick för vanliga uppgifter med minst 40%
- **Responsivitet**: Fullständig funktionalitet på mobil, surfplatta och desktop
- **Prestanda**: Sidladdningstid under 2 sekunder, interaktionstid under 200ms
- **Tillgänglighet**: WCAG 2.1 AA-nivå
- **Enkelhet**: Ny instruktör ska kunna skapa sin första lektion inom 10 minuter

---

## 3. Användarflöden

### 3.1 Övergripande navigationsstruktur

```
┌─────────────────────────────────────────────────────────────┐
│  🏠 Dashboard                                               │
│  ├── 📚 Turbank (Patterns & Exercises)                     │
│  │   ├── Visa alla turer                                    │
│  │   ├── Skapa ny tur                                       │
│  │   └── Importera turer                                    │
│  ├── 📝 Lektioner                                           │
│  │   ├── Visa alla lektioner                                │
│  │   ├── Skapa ny lektion                                   │
│  │   └── Lektionsbyggare                                    │
│  ├── 🎓 Kurser                                              │
│  │   ├── Visa alla kurser                                   │
│  │   ├── Skapa ny kurs                                      │
│  │   └── Kursplaneringsvy                                   │
│  ├── 📋 Mallar                                              │
│  └── ⚙️ Inställningar                                      │
└─────────────────────────────────────────────────────────────┘
```

### 3.2 Användarflöde: Hantera lektioner

#### 3.2.1 Skapa ny lektion

```
[Start] → [Välj "Ny lektion"] → [Välj metod]
                                    ├── [Från mall] → [Välj mall] → [Anpassa] → [Spara]
                                    ├── [Från scratch] → [Lektionsbyggare] → [Spara]
                                    └── [Kopiera befintlig] → [Välj lektion] → [Anpassa] → [Spara]
```

**Detaljerat flöde - Lektionsbyggare:**

1. **Steg 1: Grundinformation** (30 sek)
   - Namn på lektion
   - Datum (valfritt)
   - Planerad längd (dropdown: 60/75/90/120 min)
   - Nivå (Beginner/Improver/Intermediate/Advanced)

2. **Steg 2: Välj sektioner** (1 min)
   - Fördefinierade sektionstyper visas som klickbara kort:
     - Uppvärmning
     - Teknik
     - Mönster (Patterns)
     - Kombination
     - Repetition
     - Social dans
   - Användaren klickar för att aktivera/inaktivera sektioner
   - Standardmallar för nivå föreslås

3. **Steg 3: Fyll i sektioner** (3-5 min)
   - Integrerad turbanksvy för att välja turer
   - Dra-och-släpp eller klicka för att lägga till
   - Real-time tidsberäkning och validering
   - Förkunskapsvarningar visas inline

4. **Steg 4: Granska och spara** (30 sek)
   - Sammanfattningsvy
   - Validering av total tid
   - Spara, Exportera, eller Spara som mall

#### 3.2.2 Redigera lektion

```
[Lektionslista] → [Klicka på lektion] → [Detaljvy med edit-läge]
                                             ├── [Inline-redigering av fält]
                                             ├── [Lägg till/ta bort patterns]
                                             └── [Spara ändringar]
```

**Designprincip**: Inline-redigering prioriteras över modaler för snabbhet.

### 3.3 Användarflöde: Skapa turer (Patterns)

#### 3.3.1 Skapa ny tur

```
[Turbank] → [+ Ny tur] → [Wizard eller Snabbformulär]
```

**Snabbformulär (standard - 2 min):**
```
┌────────────────────────────────────────────────────────────┐
│ Skapa ny tur                                      [X]      │
├────────────────────────────────────────────────────────────┤
│ Namn*:        [________________]                           │
│ Typ*:         [Pattern ▼]                                  │
│ Nivå*:        [Beginner ▼]                                 │
│ Counts:       [________________] (ex: 1&2, 3&4, 5&6)       │
│ Est. tid:     [10] min                                     │
│ BPM-range:    [88] - [104]                                 │
│                                                            │
│ Beskrivning:  [__________________________]                 │
│               [__________________________]                 │
│                                                            │
│ Taggar:       [+ Lägg till tagg]                           │
│               [fundamentals] [X]                           │
│                                                            │
│              [Avbryt]  [Spara och stäng]  [Spara och ny]   │
└────────────────────────────────────────────────────────────┘
```

**Avancerat formulär (expanderbart - för detaljerad metadata):**
- Steg-för-steg-instruktioner
- Handfattningar
- Förkunskaper (välj från befintliga turer)
- Relaterade turer
- Undervisningspunkter
- Vanliga fel
- Medialänkar (YouTube, Vimeo)
- Variationer

#### 3.3.2 Redigera befintlig tur

```
[Turbank] → [Klicka på tur] → [Detaljvy]
                                 ├── [Snabbredigera-ikon] → [Inline-fält]
                                 └── [Fullständig redigering] → [Avancerat formulär]
```

### 3.4 Användarflöde: Lägga till information

#### 3.4.1 Lägga till noter/anteckningar

**Kontextbaserade notatfält tillgängliga på:**
- Lektion → "Lärarnotiser" per sektion
- Tur/Övning → "Undervisningspunkter" och "Vanliga fel"
- Kurs → "Mål och teman per vecka"

**Markdown-stöd för formatering:**
```
┌────────────────────────────────────────────────────────────┐
│ Lärarnotiser                                   [ℹ️] [✏️]   │
├────────────────────────────────────────────────────────────┤
│ - Fokusera på **anchor-kvalitet**                         │
│ - Påminn om andning                                        │
│ - Vänta med variationer till nästa vecka                   │
│                                                            │
│ [B] [I] [•] [1.] [🔗]                 Spara automatiskt   │
└────────────────────────────────────────────────────────────┘
```

#### 3.4.2 Lägga till media och resurser

```
[Detaljvy för tur] → [Media-sektion] → [+ Lägg till]
                                           ├── [YouTube-länk]
                                           ├── [Vimeo-länk]
                                           ├── [Spotify-spellista]
                                           └── [Fil (framtid)]
```

**Validering och förhandsvisning:**
- URL valideras automatiskt
- Thumbnail visas för videolänkar
- BPM och artistinfo för Spotify (om API-integration)

---

## 4. Gränssnittsdesign per enhet

### 4.1 Designprinciper

| Princip | Beskrivning |
|---------|-------------|
| **Mobile-first** | Designa för mobil först, skala upp för större skärmar |
| **Progressiv disclosure** | Visa grundinfo först, expandera vid behov |
| **Kompakt men läsbar** | Maximera information utan att överväldiga |
| **Konsekvent navigation** | Samma mönster på alla enheter |
| **Touch-vänlig** | Minsta klickyta 44x44px |

### 4.2 Mobil (< 768px)

#### Layout-struktur

```
┌─────────────────────────────────────┐
│ ≡ DanceCourseCreator           🔔 👤│ ← Minimal header
├─────────────────────────────────────┤
│                                     │
│    [Huvudinnehåll - full bredd]     │
│                                     │
│                                     │
│                                     │
│                                     │
│                                     │
├─────────────────────────────────────┤
│ 🏠   📚   📝   🎓   +               │ ← Bottom navigation
└─────────────────────────────────────┘
```

#### Specifika anpassningar

**Turbank på mobil:**
```
┌─────────────────────────────────────┐
│ 🔍 Sök...                      [≡]  │ ← Kompakt sökfält + filter
├─────────────────────────────────────┤
│ [Beginner] [Improver] [All nivåer]  │ ← Horisontell scrollbar
├─────────────────────────────────────┤
│ ┌─────────────────────────────────┐ │
│ │ Sugar Push                      │ │
│ │ ⭐ Beginner · 10 min · Pattern  │ │
│ │ [fundamentals] [connection]     │ │
│ │                          [+] [🔍]│ │
│ └─────────────────────────────────┘ │
│ ┌─────────────────────────────────┐ │
│ │ Left Side Pass                  │ │
│ │ ...                             │ │
│ └─────────────────────────────────┘ │
└─────────────────────────────────────┘
```

**Lektionsvy på mobil:**
- Kollapsade sektioner (expandera vid klick)
- "Swipe left" för snabbåtgärder (redigera, ta bort)
- FAB (Floating Action Button) för "Lägg till"

#### Kompakt vy (mobil)

**Syfte**: Ultra-minimalistisk vy för användning "på golvet" under lektion.

```
┌─────────────────────────────────────┐
│ Lektion 3 · Improver · 75 min       │
├─────────────────────────────────────┤
│ ▼ 1. Uppvärmning (10 min)           │
│   • Connection drill                 │
│                                     │
│ ▼ 2. Teknik (15 min)                │
│   • Anchor drill                     │
│                                     │
│ ▼ 3. Mönster (20 min)               │
│   • Left Side Pass                   │
│   • Right Side Pass                  │
│                                     │
│ ▼ 4. Kombination (20 min)           │
│   • SP → LSP → RSP                   │
│                                     │
│ ▼ 5. Social (10 min)                │
│   • Rotation till musik              │
├─────────────────────────────────────┤
│ [< Föregående]  [Nästa lektion >]   │
└─────────────────────────────────────┘
```

### 4.3 Surfplatta (768px - 1024px)

#### Layout-struktur

```
┌─────────────────────────────────────────────────────────────┐
│ ≡  DanceCourseCreator                              🔔 👤   │
├───────────────┬─────────────────────────────────────────────┤
│               │                                             │
│ 🏠 Dashboard  │     [Huvudinnehåll - 2/3 bredd]             │
│ 📚 Turbank    │                                             │
│ 📝 Lektioner  │                                             │
│ 🎓 Kurser     │                                             │
│ 📋 Mallar     │                                             │
│               │                                             │
│               │                                             │
│               │                                             │
│ ⚙️ Inställn.  │                                             │
└───────────────┴─────────────────────────────────────────────┘
```

#### Specifika anpassningar

**Turbank på surfplatta:**
- 2-kolumns kortvy
- Sidebar med filter (alltid synlig)
- Inline-förhandsvisning vid hover

**Lektionsbyggare på surfplatta:**
```
┌─────────────────────────────────────────────────────────────┐
│          LEKTIONSBYGGARE · Vecka 3 · Improver               │
├────────────────────────┬────────────────────────────────────┤
│   TURBANK (urval)      │   LEKTIONSSTRUKTUR                 │
│                        │                                    │
│ [🔍 Sök...]            │  1. Uppvärmning (10 min) [+]       │
│                        │     └─ Connection drill            │
│ [Sugar Push    ] [+]   │                                    │
│ [Left Side Pass] [+]   │  2. Teknik (15 min) [+]            │
│ [Anchor drill  ] [+]   │     └─ Anchor drill                │
│ [Whip          ] [+]   │                                    │
│                        │  3. Mönster (20 min) [+]           │
│ ─────────────────      │     └─ Left Side Pass              │
│ Valda: 3               │     └─ Right Side Pass             │
│ [Visa turbank]         │                                    │
│                        │  Total: 75 min ✅                  │
├────────────────────────┴────────────────────────────────────┤
│ [Avbryt]  [Spara utkast]  [Förhandsgranska]  [Spara]        │
└─────────────────────────────────────────────────────────────┘
```

### 4.4 Desktop (> 1024px)

#### Layout-struktur

```
┌────────────────────────────────────────────────────────────────────────────┐
│ [Logo] DanceCourseCreator    [🔍 Sök...]                    🔔  👤 Profil  │
├────────────────┬───────────────────────────────────────────────────────────┤
│                │                                                           │
│ 🏠 Dashboard   │     [Huvudinnehåll - 3/4 bredd med sidebar/panel]         │
│ ─────────────  │                                                           │
│ 📚 Turbank     │                                                           │
│   └ Alla turer │                                                           │
│   └ Mina fav.  │                                                           │
│   └ Ny tur     │                                                           │
│                │                                                           │
│ 📝 Lektioner   │                                                           │
│ 🎓 Kurser      │                                                           │
│ 📋 Mallar      │                                                           │
│                │                                                           │
│ ─────────────  │                                                           │
│ ⚙️ Inställn.   │                                                           │
└────────────────┴───────────────────────────────────────────────────────────┘
```

#### Specifika anpassningar

**Turbank på desktop:**
- 3-4 kolumns kortvy eller tabellvy (växlingsbar)
- Sidebar med avancerade filter
- Quick-preview panel vid hover/klick
- Dra-och-släpp direkt till lektionsbyggare (om öppen)

**Kursplaneringsvy på desktop:**
```
┌────────────────────────────────────────────────────────────────────────────┐
│  KURSPLAN: Beginner 8 Weeks                              [Exportera] [⚙️]  │
├────────────────────────────────────────────────────────────────────────────┤
│  ┌────────┐ ┌────────┐ ┌────────┐ ┌────────┐ ┌────────┐ ┌────────┐ ...    │
│  │ Vecka 1│ │ Vecka 2│ │ Vecka 3│ │ Vecka 4│ │ Vecka 5│ │ Vecka 6│        │
│  │ 75 min │ │ 75 min │ │ 75 min │ │ 75 min │ │ 75 min │ │ 75 min │        │
│  │────────│ │────────│ │────────│ │────────│ │────────│ │────────│        │
│  │•Sugar  │ │•LSP    │ │•RSP    │ │•Whip   │ │•Tuck   │ │•Whip   │        │
│  │ Push   │ │•Anchor │ │•Timing │ │•Stretch│ │ Turn   │ │ var.   │        │
│  │•Connec.│ │ drill  │ │ drill  │ │ focus  │ │•Rotat. │ │•Comb.  │        │
│  │✅      │ │✅      │ │✅      │ │✅      │ │⚠️      │ │        │        │
│  │[Edit]  │ │[Edit]  │ │[Edit]  │ │[Edit]  │ │[Edit]  │ │[Edit]  │        │
│  └────────┘ └────────┘ └────────┘ └────────┘ └────────┘ └────────┘        │
├────────────────────────────────────────────────────────────────────────────┤
│  TÄCKNING:  🟩 Fundamentals: 100%  │  🟨 Technique: 75%  │  🟥 Advanced: 0% │
├────────────────────────────────────────────────────────────────────────────┤
│  VARNINGAR: ⚠️ Vecka 5: Tuck Turn kräver Whip som inte repeteras sedan V4  │
└────────────────────────────────────────────────────────────────────────────┘
```

### 4.5 Kompakt vy (alla enheter)

#### Syfte
En ultra-kompakt vy för:
- Användning under pågående lektion ("på golvet")
- Utskrift
- Snabb referens

#### Design

```
┌─────────────────────────────────────────────────────┐
│ LEKTION 3 | Improver | 75 min | 2025-01-15          │
├─────────────────────────────────────────────────────┤
│ 1. Uppvärmning (10) · Connection drill              │
│ 2. Teknik (15) · Anchor drill                       │
│ 3. Mönster (20) · Left Side Pass, Right Side Pass   │
│ 4. Kombination (20) · SP → LSP → RSP sekvens        │
│ 5. Social (10) · Rotation 92-98 BPM                 │
├─────────────────────────────────────────────────────┤
│ Noter: Fokus på anchor. Mjuk arm. Tydlig slot.      │
│ Musik: [Spellista 90-100 BPM]                       │
└─────────────────────────────────────────────────────┘
```

#### Växling mellan vyer
- Knapp/toggle: `[Kompakt] [Standard] [Utökad]`
- Persistent val per enhet (sparas i localStorage)
- Automatiskt val baserat på skärmstorlek som standard

---

## 5. Tekniska krav

### 5.1 Frontend-arkitektur

#### Teknisk stack
- **Framework**: Blazor Server/.NET 10
- **UI-bibliotek**: MudBlazor 7.x
- **State Management**: Blazor inbyggd state + Cascading Parameters
- **Responsivitet**: CSS Grid + Flexbox, MudBlazor breakpoints

#### Komponentstruktur

```
Components/
├── Shared/
│   ├── AppLayout.razor         # Huvudlayout med navigation
│   ├── MobileBottomNav.razor   # Mobil bottom navigation
│   ├── DesktopSidebar.razor    # Desktop sidebar
│   ├── SearchBar.razor         # Global sökfunktion
│   ├── FilterPanel.razor       # Återanvändbar filterkomponent
│   └── CompactViewToggle.razor # Växla mellan vyer
│
├── PatternBank/
│   ├── PatternList.razor       # Lista/grid av turer
│   ├── PatternCard.razor       # Individuellt kort
│   ├── PatternDetailView.razor # Detaljvy
│   ├── PatternEditor.razor     # Skapa/redigera formulär
│   ├── PatternQuickAdd.razor   # Snabbformulär (modal)
│   └── PatternFilters.razor    # Filteralternativ
│
├── Lessons/
│   ├── LessonList.razor        # Lista av lektioner
│   ├── LessonBuilder.razor     # Huvudbyggarkomponent
│   ├── LessonSection.razor     # Individuell sektion
│   ├── LessonCompactView.razor # Kompakt vy
│   ├── LessonTimeline.razor    # Visuell tidslinje
│   └── SectionPatternPicker.razor # Välj patterns för sektion
│
├── Courses/
│   ├── CourseList.razor        # Lista av kurser
│   ├── CoursePlanner.razor     # Kursplaneringsvy
│   ├── CourseTimeline.razor    # Veckovis tidslinje
│   ├── CoverageMatrix.razor    # Täckningsanalys
│   └── CourseWizard.razor      # Steg-för-steg-guide
│
└── Common/
    ├── DragDropContainer.razor # Wrapper för dra-och-släpp
    ├── TimeIndicator.razor     # Visar tid med validering
    ├── LevelBadge.razor        # Nivåindikator
    ├── TagChip.razor           # Tagg-visning
    └── MediaPreview.razor      # Video/musik-preview
```

### 5.2 Komponentspecifikationer

#### 5.2.1 PatternCard

**Props:**
```csharp
[Parameter] public PatternOrExercise Pattern { get; set; }
[Parameter] public bool ShowDetails { get; set; } = false;
[Parameter] public bool Selectable { get; set; } = true;
[Parameter] public EventCallback<PatternOrExercise> OnSelect { get; set; }
[Parameter] public EventCallback<PatternOrExercise> OnView { get; set; }
```

**Funktionalitet:**
- Visar namn, nivå, typ, estimerad tid
- Visar taggar som chips
- Klickbar för val (om `Selectable`)
- Knapp för att visa detaljer
- Animerad hover-effekt

#### 5.2.2 LessonBuilder

**Props:**
```csharp
[Parameter] public Lesson? Lesson { get; set; }
[Parameter] public int TargetDuration { get; set; } = 75;
[Parameter] public DanceLevel Level { get; set; }
[Parameter] public EventCallback<Lesson> OnSave { get; set; }
```

**State:**
```csharp
private List<LessonSection> sections = new();
private List<PatternOrExercise> selectedPatterns = new();
private int totalEstimatedTime => CalculateTotalTime();
private List<ValidationWarning> warnings = new();
```

**Funktionalitet:**
- Dra-och-släpp med MudBlazor `MudDropContainer`
- Real-time tidsberäkning
- Validering av förkunskaper
- Varningar för tid/antal moment
- Auto-save var 30:e sekund (utkast)

#### 5.2.3 CourseTimeline

**Props:**
```csharp
[Parameter] public Course Course { get; set; }
[Parameter] public bool EditMode { get; set; } = true;
[Parameter] public EventCallback<Course> OnUpdate { get; set; }
```

**Funktionalitet:**
- Veckovis tidslinje (scrollbar horisontellt på mobil)
- Varje vecka som klickbar kolumn
- Visar turer och varningar per vecka
- Dra lektioner mellan veckor
- Visuella indikatorer för täckning

### 5.3 API-krav

#### Nya endpoints

```csharp
// Pattern Bank
GET    /api/patterns                    // Lista med filter & paginering
GET    /api/patterns/{id}               // Enskild tur
POST   /api/patterns                    // Skapa ny
PUT    /api/patterns/{id}               // Uppdatera
DELETE /api/patterns/{id}               // Ta bort
GET    /api/patterns/{id}/related       // Relaterade turer
GET    /api/patterns/search?q={term}    // Textsökning

// Lessons
GET    /api/lessons                     // Lista med filter
GET    /api/lessons/{id}                // Enskild lektion
GET    /api/lessons/{id}/compact        // Kompakt format
POST   /api/lessons                     // Skapa ny
PUT    /api/lessons/{id}                // Uppdatera
POST   /api/lessons/validate            // Validera lektion
POST   /api/lessons/{id}/duplicate      // Duplicera

// Courses
GET    /api/courses                     // Lista med filter
GET    /api/courses/{id}                // Enskild kurs
GET    /api/courses/{id}/timeline       // Veckovis data
GET    /api/courses/{id}/coverage       // Täckningsanalys
POST   /api/courses                     // Skapa ny
PUT    /api/courses/{id}                // Uppdatera
POST   /api/courses/generate            // AI-genererad kurs

// Rekommendationer
GET    /api/recommendations/patterns    // Föreslagna turer
POST   /api/recommendations/lesson      // Föreslå lektionsinnehåll
```

#### Response-format

```json
// GET /api/patterns?level=Beginner&type=Pattern&page=1&pageSize=20
{
  "items": [
    {
      "id": "uuid",
      "name": "Sugar Push",
      "type": "Pattern",
      "level": "Beginner",
      "estimatedMinutes": 10,
      "bpmRange": { "min": 88, "max": 104 },
      "tags": ["fundamentals", "connection"],
      "thumbnailUrl": null
    }
  ],
  "totalCount": 45,
  "page": 1,
  "pageSize": 20,
  "hasMore": true
}
```

### 5.4 Datahantering

#### State Management

**Global state (via Blazor services):**
```csharp
public class AppState
{
    public User? CurrentUser { get; set; }
    public ViewMode CurrentViewMode { get; set; } = ViewMode.Standard;
    public FilterState PatternFilters { get; set; } = new();
    public Lesson? CurrentDraftLesson { get; set; }
    public event Action? OnChange;
}
```

**Lokal caching:**
- Turbank cachelagras i minne (5 min TTL)
- Aktuell lektion sparas i localStorage som utkast
- Filter-preferenser sparas per användare

#### Offline-stöd (framtid)

För framtida PWA-implementation:
- IndexedDB för lokal turbank
- Service Worker för caching av statiska resurser
- Sync-queue för ändringar gjorda offline

### 5.5 Responsiv implementation

#### Breakpoints (MudBlazor)

```css
/* Xs: 0-599px (mobil) */
/* Sm: 600-959px (stor mobil/liten surfplatta) */
/* Md: 960-1279px (surfplatta) */
/* Lg: 1280-1919px (laptop/desktop) */
/* Xl: 1920px+ (stor skärm) */
```

#### Komponentanpassning

```razor
@* Exempel: PatternList.razor *@

<MudHidden Breakpoint="Breakpoint.SmAndDown" Invert="true">
    @* Mobil: en kolumn, kompakta kort *@
    <MudGrid>
        @foreach (var pattern in patterns)
        {
            <MudItem xs="12">
                <PatternCard Pattern="@pattern" Compact="true" />
            </MudItem>
        }
    </MudGrid>
</MudHidden>

<MudHidden Breakpoint="Breakpoint.SmAndDown">
    @* Desktop: multi-kolumn grid eller tabell *@
    @if (ViewMode == "Grid")
    {
        <MudGrid>
            @foreach (var pattern in patterns)
            {
                <MudItem xs="12" sm="6" md="4" lg="3">
                    <PatternCard Pattern="@pattern" />
                </MudItem>
            }
        </MudGrid>
    }
    else
    {
        <MudTable Items="@patterns" Dense="true" Hover="true">
            @* Tabellvy *@
        </MudTable>
    }
</MudHidden>
```

---

## 6. Funktionella krav

### 6.1 Turbank

| ID | Krav | Prioritet | Acceptanskriterier |
|----|------|-----------|-------------------|
| TB-001 | Visa lista av turer med kort eller tabellvy | Hög | Användaren kan växla mellan kort- och tabellvy |
| TB-002 | Filtrera på nivå (Beginner-Advanced) | Hög | Filter påverkar listan inom 200ms |
| TB-003 | Filtrera på typ (Pattern/Exercise) | Hög | Filter kan kombineras |
| TB-004 | Fritextsökning | Hög | Söker i namn, alias, beskrivning |
| TB-005 | Filtrera på taggar | Medel | Flerval av taggar möjligt |
| TB-006 | Snabbskapa tur (modal) | Hög | Tur skapas med minimal data på <30 sek |
| TB-007 | Detaljvy för tur | Hög | All metadata visas, editbar |
| TB-008 | Markera favoriter | Låg | Favoriter sparas per användare |
| TB-009 | Visa relaterade turer | Medel | Länkade turer visas i detaljvy |
| TB-010 | Visa progressionsträd | Medel | Visuellt träd av förkunskaper |

### 6.2 Lektioner

| ID | Krav | Prioritet | Acceptanskriterier |
|----|------|-----------|-------------------|
| LE-001 | Skapa lektion med sektioner | Hög | Min 3, max 8 sektioner stöds |
| LE-002 | Dra-och-släpp turer till sektioner | Hög | Fungerar på desktop och surfplatta |
| LE-003 | Real-time tidsberäkning | Hög | Total tid uppdateras inom 100ms |
| LE-004 | Varning vid tidsöverskridning | Hög | Varning visas om total > mål + 5 min |
| LE-005 | Validering av förkunskaper | Hög | Varning om prerequisites saknas |
| LE-006 | Kompakt vy för lektion | Hög | "På golvet"-vy fungerar på mobil |
| LE-007 | Duplicera lektion | Medel | Ny kopia skapas med (Kopia) i namn |
| LE-008 | Exportera till PDF | Medel | PDF genereras med formatering |
| LE-009 | Exportera till Markdown | Medel | MD-fil laddas ned |
| LE-010 | Autosave utkast | Medel | Sparas var 30 sek |

### 6.3 Kurser

| ID | Krav | Prioritet | Acceptanskriterier |
|----|------|-----------|-------------------|
| KU-001 | Skapa kurs med 4-12 veckor | Hög | Antal veckor kan ändras |
| KU-002 | Veckovis tidslinje | Hög | Visuell representation på alla enheter |
| KU-003 | Koppla lektioner till veckor | Hög | Lektion kan flyttas mellan veckor |
| KU-004 | Täckningsanalys | Medel | Visar % av fundamentals täckta |
| KU-005 | Varningar för progression | Medel | Varning om fundamental saknas |
| KU-006 | Kursplaneringsassistent | Medel | Wizard genererar förslag |
| KU-007 | Exportera kursplan | Medel | PDF med alla veckor/lektioner |

### 6.4 Allmänt gränssnitt

| ID | Krav | Prioritet | Acceptanskriterier |
|----|------|-----------|-------------------|
| UI-001 | Responsiv design | Hög | Fungerande på mobil, surfplatta, desktop |
| UI-002 | Bottom navigation på mobil | Hög | Synlig på alla mobilskärmar |
| UI-003 | Sidebar på desktop | Hög | Kollapserbar sidebar |
| UI-004 | Växla kompakt/standard/utökad vy | Medel | Knapp tillgänglig i toolbar |
| UI-005 | Dark/Light theme | Låg | Växlingsbar i inställningar |
| UI-006 | Laddningsindikator | Hög | Synlig vid API-anrop >500ms |
| UI-007 | Felhantering med meddelanden | Hög | Toast-meddelanden vid fel |
| UI-008 | Keyboard navigation | Medel | Tab-ordning logisk |
| UI-009 | Touch-vänliga knappar | Hög | Min 44x44px klickyta |

---

## 7. Icke-funktionella krav

### 7.1 Prestanda

| ID | Krav | Mål |
|----|------|-----|
| NFR-P01 | Initial sidladdning | < 2 sekunder |
| NFR-P02 | API-responstid | < 300ms för 95% av anrop |
| NFR-P03 | Filter/sökning | < 200ms för resultat |
| NFR-P04 | Dra-och-släpp | 60 FPS under drag |
| NFR-P05 | Turbank med 500 objekt | Smooth scrolling |

### 7.2 Tillgänglighet

| ID | Krav | Standard |
|----|------|----------|
| NFR-A01 | Kontrastförhållande | WCAG 2.1 AA (4.5:1) |
| NFR-A02 | Tangentbordsnavigation | Alla funktioner tillgängliga |
| NFR-A03 | Skärmläsarstöd | ARIA-labels på interaktiva element |
| NFR-A04 | Focus-indikatorer | Synliga på alla interaktiva element |

### 7.3 Säkerhet

| ID | Krav |
|----|------|
| NFR-S01 | Autentisering krävs för alla redigeringsoperationer |
| NFR-S02 | CSRF-skydd på alla POST/PUT/DELETE |
| NFR-S03 | Input-validering på server och klient |
| NFR-S04 | Rollbaserad åtkomst (Instruktör, Editor, Reader) |

### 7.4 Skalbarhet

| ID | Krav |
|----|------|
| NFR-SC01 | Hantera 100 samtidiga användare |
| NFR-SC02 | Turbank upp till 5000 objekt |
| NFR-SC03 | 1000 lektioner per användare |

---

## 8. Designspecifikation

### 8.1 Färgschema

**Primär palett (MudBlazor defaults + anpassningar):**

| Användning | Ljust tema | Mörkt tema |
|------------|------------|------------|
| Primary | #1976D2 (blå) | #90CAF9 |
| Secondary | #7C4DFF (lila) | #B388FF |
| Accent | #FF4081 (rosa) | #FF80AB |
| Background | #FAFAFA | #121212 |
| Surface | #FFFFFF | #1E1E1E |
| Text Primary | #212121 | #FFFFFF |
| Text Secondary | #757575 | #B0B0B0 |
| Success | #4CAF50 | #81C784 |
| Warning | #FF9800 | #FFB74D |
| Error | #F44336 | #E57373 |

**Nivåfärger:**
| Nivå | Färg |
|------|------|
| Beginner | #4CAF50 (grön) |
| Improver | #2196F3 (blå) |
| Intermediate | #FF9800 (orange) |
| Advanced | #F44336 (röd) |

### 8.2 Typografi

```css
/* MudBlazor defaults med anpassningar */
--mud-typography-body1-size: 1rem;      /* 16px */
--mud-typography-body2-size: 0.875rem;  /* 14px */
--mud-typography-h5-size: 1.5rem;       /* 24px, sidrubriker */
--mud-typography-h6-size: 1.25rem;      /* 20px, sektionsrubriker */
--mud-typography-caption-size: 0.75rem; /* 12px, metadata */
```

### 8.3 Spacing och layout

```css
/* Standard spacing (MudBlazor) */
--mud-spacing-1: 4px;
--mud-spacing-2: 8px;
--mud-spacing-3: 12px;
--mud-spacing-4: 16px;
--mud-spacing-6: 24px;
--mud-spacing-8: 32px;

/* Layout */
--sidebar-width: 240px;
--sidebar-collapsed: 64px;
--bottom-nav-height: 56px;
--toolbar-height: 64px;
--card-min-height: 120px;
```

### 8.4 Komponentstil-riktlinjer

**Kort (PatternCard, LessonCard):**
- Border-radius: 8px
- Elevation: 2 (normalt), 6 (hover)
- Padding: 16px
- Minsta höjd: 120px

**Knappar:**
- Primär åtgärd: Filled, Primary color
- Sekundär åtgärd: Outlined
- Tertiär/Cancel: Text button
- FAB: 56x56px, primary color

**Formulär:**
- Input height: 40px
- Label: Above input
- Error text: Below input, Error color
- Spacing mellan fält: 16px

---

## 9. Acceptanskriterier för MVP

### 9.1 Turbank
- [ ] Användare kan lista alla turer med filtrering på nivå och typ
- [ ] Användare kan skapa ny tur via snabbformulär (<2 min)
- [ ] Användare kan se detaljvy för tur med all metadata
- [ ] Sök returnerar resultat inom 200ms

### 9.2 Lektioner
- [ ] Användare kan skapa en lektion med 3-8 sektioner
- [ ] Dra-och-släpp fungerar på desktop och surfplatta
- [ ] Tidsvarning visas när total tid överstiger mål
- [ ] Kompakt vy fungerar på mobil

### 9.3 Responsivitet
- [ ] Applikationen är fullt funktionell på mobil (320px+)
- [ ] Bottom navigation visas på mobil
- [ ] Sidebar visas på desktop
- [ ] Alla interaktiva element har minst 44x44px klickyta

### 9.4 Prestanda
- [ ] Sidladdning under 2 sekunder
- [ ] Inga synliga lagg vid dra-och-släpp

---

## 10. Bilagor

### 10.1 Wireframes (referens)

Se separat dokument: `docs/wireframes/` (att skapas)

### 10.2 Användarresor (User Journeys)

Se detaljerade användarresor i avsnitt 3.

### 10.3 Relaterade dokument

- [Kravspecifikation.md](../Kravspecifikation.md) - Fullständig systemkravspecifikation
- [Implementeringsplan.md](./Implementeringsplan.md) - Övergripande implementeringsplan
- [Analys-Danskursflode.md](./Analys-Danskursflode.md) - Detaljerad flödesanalys
- [WCAG-Compliance-Report.md](./WCAG-Compliance-Report.md) - Tillgänglighetsstatus

---

**Dokumentversion**: 1.0  
**Skapad**: 2025-11-29  
**Senast uppdaterad**: 2025-11-29  
**Granskad av**: -  
**Godkänd av**: -
