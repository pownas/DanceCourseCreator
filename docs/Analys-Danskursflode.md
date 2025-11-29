# Analys och förslag: Förbättrat danskursflöde

**Version**: 1.0  
**Datum**: 2025-11-21  
**Typ**: Analys och rekommendationer

## Sammanfattning

Denna analys utvärderar det befintliga DanceCourseCreator-systemet och presenterar konkreta förslag för att skapa ett intuitivt och flexibelt flöde för att bygga danskurser i West Coast Swing. Analysen fokuserar på hur turer (patterns), teknikövningar (exercises) och turbank integreras för att möjliggöra effektiv kursplanering och strukturering.

## Innehållsförteckning

1. [Nulägesanalys](#1-nulägesanalys)
2. [Användarbehov och arbetsflöde](#2-användarbehov-och-arbetsflöde)
3. [Identifierade utmaningar](#3-identifierade-utmaningar)
4. [Förslag: Turbank och bibliotekshantering](#4-förslag-turbank-och-bibliotekshantering)
5. [Förslag: Lektionsbyggare](#5-förslag-lektionsbyggare)
6. [Förslag: Kursplaneringsflöde](#6-förslag-kursplaneringsflöde)
7. [Funktioner och vyer som behövs](#7-funktioner-och-vyer-som-behövs)
8. [Dataorganisation](#8-dataorganisation)
9. [Intuitiv kursgenereringsprocess](#9-intuitiv-kursgenereringsprocess)
10. [Implementeringsprioriteringar](#10-implementeringsprioriteringar)
11. [Konkreta nästa steg](#11-konkreta-nästa-steg)

---

## 1. Nulägesanalys

### 1.1 Befintlig systemarkitektur

DanceCourseCreator är implementerad som en modern .NET 10-applikation med:

**Teknisk stack**:
- Backend: .NET 10 Web API med Entity Framework Core
- Frontend: Blazor Server med MudBlazor
- Databas: SQLite (skalbar till PostgreSQL/SQL Server)
- Autentisering: JWT med rollbaserad åtkomstkontroll

**Implementerade komponenter**:

1. **PatternOrExercise (Turbank/Bibliotek)**
   - Stöd för både turer (patterns) och teknikövningar (exercises)
   - Omfattande metadata: steg, counts, handfattningar, förkunskaper, relaterade turer
   - Undervisningspunkter och vanliga fel
   - Taggning och nivåindelning (Beginner → Advanced)
   - BPM-intervall och estimerad undervisningstid

2. **Lesson (Lektioner)**
   - Strukturerade sektioner (Warmup, Technique, Patterns, Combination, Repetition, Social)
   - Koppling till turer/övningar via Items-referenser
   - Tidsplanering och validering
   - Versionshantering och granskningsflöde

3. **Course (Kurser)**
   - Flerveckorsplanering
   - Mål och teman per vecka
   - Täckningsmetrik och repetitionsplanering
   - Koppling till flera lektioner

### 1.2 Styrkor i nuvarande implementation

✅ **Robust datamodell**: Omfattande metadata för turer och övningar  
✅ **Flexibel struktur**: Sektionsbaserad lektionsuppbyggnad  
✅ **Pedagogiskt fokus**: Förkunskaper, progressionsträd, teaching points  
✅ **Teknisk kvalitet**: Modern stack med god skalbarhet  
✅ **Användarhantering**: Rollbaserad åtkomst och teamsamarbete

### 1.3 Observerade gaps

❌ **Användargränssnitt för kursbygge**: Flödet för att bygga en komplett kurs från turbanken finns inte fullt utvecklat i UI  
❌ **Turbank-integration**: Saknar tydlig "turbank"-vy som stöder kursbyggande  
❌ **Visuell progression**: Ingen visuell representation av kurstidslinje och täckning  
❌ **Rekommendationssystem**: Begränsad intelligens för att föreslå turer baserat på kontext  
❌ **Dra-och-släpp-interface**: Ingen intuitiv dragfunktionalitet för att bygga lektioner/kurser  
❌ **Återanvändningsflöde**: Mallar finns i datamodellen men UI-stöd saknas

---

## 2. Användarbehov och arbetsflöde

### 2.1 Primära användarscenarier

**Scenario 1: Nybörjarkurs (8 veckor)**  
En instruktör vill skapa en komplett nybörjarkurs och behöver:
- Välja fundamentala turer från turbanken (Sugar Push, Side Pass, Whip)
- Fördela dessa över 8 veckor med logisk progression
- Säkerställa att förkunskaper är uppfyllda
- Balansera teknikövningar med mönsterinlärning
- Se total tidsåtgång per lektion och hela kursen

**Scenario 2: Tematisk workshop (1 dag)**  
En instruktör planerar en "Whip-intensiv" workshop och vill:
- Filtrera turbanken på "Whip" och relaterade variationer
- Se förkunskaper och dependencies
- Snabbt bygga en 3-timmarsstruktur med uppvärmning, progressiva övningar och kombination

**Scenario 3: Återanvändning och anpassning**  
En instruktör vill:
- Kopiera en tidigare kurs som mall
- Byta ut några turer baserat på deltagarnas förkunskaper
- Få förslag på ersättningsturer med liknande svårighetsgrad

### 2.2 Idealt arbetsflöde

```
1. TURBANK (Upptäck och förstå)
   └─> Filtrera och sök i bibliotek
   └─> Se metadata, förkunskaper, variationer
   └─> Markera favoriter / skapa "urval"

2. LEKTION (Komponera och tidssätta)
   └─> Välj mall eller börja från scratch
   └─> Dra turer/övningar till sektioner
   └─> Systemet validerar förkunskaper och tid
   └─> Förhandsgranska och justera

3. KURS (Strukturera progression)
   └─> Skapa kursplan med veckor
   └─> Dra lektioner till veckor eller bygga direkt
   └─> Visuell representation av täckning
   └─> Exportera och dela

4. ITERATION (Förbättra och återanvända)
   └─> Spara som mall
   └─> Versionshantering
   └─> Få feedback från kollegor
```

---

## 3. Identifierade utmaningar

### 3.1 UX-utmaningar

| Utmaning | Impact | Prioritet |
|----------|--------|-----------|
| **Turbank som "katalog" vs "arbetsyta"** | Användare behöver enkelt hitta OCH använda turer | Hög |
| **Överblickbarhet**: Svårt att se "vilka turer har jag redan använt i vecka 1-4?" | Risk för repetition eller saknade fundamentals | Hög |
| **Tidskomplexitet**: Instruktörer har begränsad tid, behöver snabba genvägar | Långsamt adoptionshinder | Medel |
| **Förkunskapsberoenden**: Vilka turer kan jag lägga in nu baserat på vad jag redan lärt ut? | Pedagogisk kvalitet | Hög |

### 3.2 Tekniska utmaningar

| Utmaning | Nuläge | Lösningsförslag |
|----------|--------|-----------------|
| **UI-komponenter för dra-och-släpp** | Saknas | Implementera med MudBlazor DropZones |
| **Rekommendationslogik** | Inte implementerad | Skapa algoritm baserat på metadata |
| **Visuell kurstidslinje** | Ingen representation | Bygga veckovis Gantt-lik vy |
| **Export och delning** | Backend finns, UI ofullständig | Komplettera med knapp och förhandsvisning |

### 3.3 Dataorganisation

| Behov | Nuvarande lösning | Förbättringsområde |
|-------|-------------------|---------------------|
| **Relationer mellan turer** | `Related[]`, `Prerequisites[]` | Bra struktur, men används inte visuellt |
| **Progressionsträd** | Dokumenterat i Kravspec | Saknas i implementation |
| **Täckningsmatris** | `CoverageMetrics` som JSON-sträng | Behöver struktureras och visualiseras |
| **Repetitionsplanering** | `RepetitionPlan` som JSON | Behöver algoritm och UI |

---

## 4. Förslag: Turbank och bibliotekshantering

### 4.1 Turbankvyn: Konceptuellt förslag

**Mål**: Skapa en "turbank" som fungerar både som katalog (browsing) och arbetsyta (selection).

#### 4.1.1 Huvudfunktioner

**A. Avancerad filtrering och sökning**
```
Filteralternativ:
- [x] Nivå (Beginner, Improver, Intermediate, Advanced)
- [x] Typ (Pattern vs Exercise)
- [x] Tags (fundamentals, musicality, rotation, etc.)
- [ ] Förkunskaper uppfyllda baserat på vald kurs/lektion
- [ ] BPM-intervall (slider 80-120 BPM)
- [ ] Estimerad undervisningstid (<10 min, 10-20 min, >20 min)
- [ ] "Favoriter" och "Mina egna"
```

**B. Visuell representation**

Förslag på layout:
```
┌─────────────────────────────────────────────────────┐
│  TURBANK                         [+ Ny tur]          │
├─────────────────────────────────────────────────────┤
│  Sök: [________________]  Nivå: [All v]  Typ: [All v]│
│  Tags: [fundamentals] [connection] [X advanced]      │
│                                                       │
│  ┌──────────┐ ┌──────────┐ ┌──────────┐             │
│  │ Sugar    │ │ Left Side│ │ Whip     │             │
│  │ Push     │ │ Pass     │ │          │             │
│  │ ⭐ Beginn.│ │ ⭐ Beginn.│ │ ⭐⭐ Impro.│             │
│  │ 10 min   │ │ 12 min   │ │ 15 min   │             │
│  │ [+] [🔍] │ │ [+] [🔍] │ │ [+] [🔍] │             │
│  └──────────┘ └──────────┘ └──────────┘             │
│                                                       │
│  Välda turer (3): [Sugar Push] [LSP] [Whip] [Rensa] │
└─────────────────────────────────────────────────────┘
```

Funktioner:
- **[+]**: Lägg till i "arbetsyta" (för att senare dra till lektion)
- **[🔍]**: Se detaljerad metadata (modal/drawer)
- **Kort**: Visuella kort med nivå, tid, och quick-add

**C. Detaljvy för turbankobjekt**

När man klickar på [🔍], visa drawer/modal med:
```
Tur: Left Side Pass
Typ: Pattern
Nivå: Beginner

Beskrivning:
Follower passerar till leaderens vänstra sida i slot.

Counts: 1&2, 3&4, 5&6
Handfattningar: RH-LH start, optional handbyte på 4

Förkunskaper: Sugar Push
Relaterade: Right Side Pass, Sugar Push variations

Undervisningspunkter:
- Tydlig slot-travel
- Förberedelse på 2
- Lätthet i arm

Vanliga fel:
- Avviker från slot
- "Drag" istället för bjudning

BPM: 88-104
Estimerad tid: 10-15 min

[Redigera] [Använd i lektion] [Stäng]
```

### 4.2 Smart urvalsfunktioner

**"Quick Course Builder"**  
Låt systemet föreslå en komplett kursstruktur baserat på:
```
Input:
- Nivå: Beginner
- Antal veckor: 8
- Fokusområden: [Fundamentals] [Basic patterns]

Output:
System genererar förslag på:
Vecka 1: Sugar Push + Connection drills
Vecka 2: Left Side Pass + Anchor exercises
Vecka 3: Right Side Pass + Timing drills
...
```

**"Missing Fundamentals"-varning**  
Om instruktören planerar vecka 5 och ännu inte lagt in Whip (men nivån är Improver+), visa:
```
⚠️ Rekommendation: Whip är en fundamental för Improver-nivå och har inte introducerats än.
   Förslag: Lägg till i Vecka 3 eller 4.
```

### 4.3 Implementation: Backend-stöd

Behöver API-endpoints för:
```csharp
// Hämta rekommenderade turer baserat på kontext
GET /api/patterns/recommendations?lessonId={id}&level=Beginner&excludedIds=...

// Validera förkunskaper för en lektion/kurs
POST /api/lessons/{id}/validate-prerequisites

// Hämta progressionsträd för en tur
GET /api/patterns/{id}/progression-tree
```

**Rekommendationslogik** (pseudo-algoritm):
```csharp
public List<PatternOrExercise> GetRecommendations(
    DanceLevel level, 
    List<string> alreadyTaughtIds,
    List<string> tags)
{
    // 1. Filtrera på nivå (samma eller lägre)
    var candidates = _patterns.Where(p => p.Level <= level);
    
    // 2. Filtrera på uppfyllda förkunskaper
    candidates = candidates.Where(p => 
        p.Prerequisites.All(pre => alreadyTaughtIds.Contains(pre))
    );
    
    // 3. Prioritera utifrån tags och "Related"-kopplingar
    var scored = candidates.Select(p => new {
        Pattern = p,
        Score = CalculateScore(p, tags, alreadyTaughtIds)
    }).OrderByDescending(x => x.Score);
    
    // 4. Returnera top 10
    return scored.Take(10).Select(x => x.Pattern).ToList();
}
```

---

## 5. Förslag: Lektionsbyggare

### 5.1 Dra-och-släpp-interface

**Konceptuell layout**:
```
┌─────────────────────────────────────────────────────────────┐
│  LEKTION: Vecka 2 - Improver                                │
│  Varaktighet: 75 min                          [Spara] [Avbry]│
├────────────────────┬────────────────────────────────────────┤
│  TURBANK (urval)   │  LEKTIONSSTRUKTUR                      │
│                    │                                          │
│  [Sugar Push]      │  1. Uppvärmning (10 min) [+]            │
│  [Left Side Pass]  │     └─ Connection drill                 │
│  [Whip Variation]  │                                          │
│  [Anchor drill]    │  2. Teknik (15 min) [+]                 │
│                    │     └─ Anchor drill                      │
│  [Visa turbank]    │     └─ (Dra hit övningar)               │
│                    │                                          │
│                    │  3. Mönster (20 min) [+]                │
│                    │     └─ Left Side Pass                    │
│                    │     └─ (Dra hit turer)                   │
│                    │                                          │
│                    │  4. Kombination (20 min) [+]            │
│                    │     └─ Sugar Push → LSP sekvens          │
│                    │                                          │
│                    │  5. Social dans (10 min) [+]            │
│                    │                                          │
│                    │  Total: 75 min ✅                        │
│                    │  Förkunskaper: ✅ Uppfyllda              │
└────────────────────┴────────────────────────────────────────┘
```

### 5.2 Funktioner

**A. Dra-och-släpp-zoner**
- Använd MudBlazor `MudDropZone` och `MudDropContainer`
- Dra turer från urval till sektioner
- Dra mellan sektioner för omorganisering
- Automatisk tidsberäkning

**B. Real-time validering**

När man lägger till en tur:
```
✅ Left Side Pass: Förkunskaper uppfyllda (Sugar Push lärd i Vecka 1)
⚠️ Whip Inside Roll: Kräver Whip (inte lärd än) - Vill du lägga till ändå?
```

När total tid överskrids:
```
⚠️ Total lektionstid (82 min) överstiger mål (75 min). 
   Överväg att ta bort eller korta:
   - Kombination: 20 → 15 min
   - Social: 10 → 5 min
```

**C. Sektionsmallar**

Fördefinierade sektioner:
```
Mall: "Standard 75-min Beginner Lesson"
- Uppvärmning (10 min): Connection + Posture drills
- Teknik (10 min): Anchor quality
- Mönster 1 (15 min): Huvudtur med genomgång
- Mönster 2 (15 min): Sekundär tur eller variation
- Kombination (15 min): Sekvens och drill
- Social (10 min): Fri dans med rotation

[Använd mall] [Anpassa]
```

### 5.3 Implementation: UI-komponenter

**MudBlazor DropZone-exempel**:
```razor
<MudDropContainer T="PatternOrExerciseItem" 
                  Items="@availablePatterns" 
                  ItemsSelector="@((item, dropzone) => item.AssignedSection == dropzone)"
                  ItemDropped="@OnPatternDropped">
    
    <ChildContent>
        <!-- Uppvärmning -->
        <MudDropZone T="PatternOrExerciseItem" Identifier="Warmup" Class="mud-height-full">
            <MudText>Uppvärmning (@GetSectionDuration("Warmup") min)</MudText>
        </MudDropZone>
        
        <!-- Teknik -->
        <MudDropZone T="PatternOrExerciseItem" Identifier="Technique" Class="mud-height-full">
            <MudText>Teknik (@GetSectionDuration("Technique") min)</MudText>
        </MudDropZone>
        
        <!-- ... fler sektioner -->
    </ChildContent>
    
    <ItemRenderer>
        <MudPaper Class="pa-2 ma-2">
            <MudText>@context.Pattern.Name</MudText>
            <MudChip Size="Size.Small">@context.Pattern.EstimatedMinutes min</MudChip>
        </MudPaper>
    </ItemRenderer>
</MudDropContainer>

@code {
    // Wrapper class för att hålla pattern och dess tilldelade sektion
    public class PatternOrExerciseItem
    {
        public PatternOrExercise Pattern { get; set; }
        public string AssignedSection { get; set; }
    }
}
```

**Backend-stöd**:
```csharp
// DTOs för lektionsbyggnad
public class LessonBuilderRequest
{
    public string? TemplateId { get; set; }
    public int Duration { get; set; }
    public DanceLevel Level { get; set; }
    public List<SectionRequest> Sections { get; set; }
}

public class SectionRequest
{
    public LessonSectionType Type { get; set; }
    public List<string> PatternIds { get; set; }
    public int EstimatedMinutes { get; set; }
}

// API endpoint
[HttpPost("api/lessons/build")]
public async Task<ActionResult<LessonDTO>> BuildLesson(LessonBuilderRequest request)
{
    // Validera tider
    // Kontrollera förkunskaper
    // Skapa lektion
    // Returnera med valideringsvarningar
}
```

---

## 6. Förslag: Kursplaneringsflöde

### 6.1 Visuell kurstidslinje

**Målet**: En veckovis översikt där instruktören kan se och redigera hela kursens uppbyggnad.

**Konceptuell layout**:
```
┌──────────────────────────────────────────────────────────────┐
│  KURS: Beginner 8 Weeks - Fall 2025                          │
│  Nivå: Beginner  Varaktighet: 8 veckor         [Exportera]   │
├──────────────────────────────────────────────────────────────┤
│  Vecka 1  │ Vecka 2  │ Vecka 3  │ Vecka 4  │ Vecka 5  │ ... │
│  [75 min] │ [75 min] │ [75 min] │ [75 min] │ [75 min] │     │
│───────────┤──────────┤──────────┤──────────┤──────────┤─────│
│ • Sugar   │• LSP     │• RSP     │• Whip    │• Tuck    │     │
│   Push    │• Anchor  │• Timing  │• Stretch │  Turn    │     │
│ • Connect.│  drill   │  drill   │  focus   │• Rotation│     │
│   drill   │• (SP rep)│• (LSP rep)│        │         │     │
│           │          │          │          │          │     │
│ ✅ Fund.  │✅ Fund.  │✅ Prog.  │✅ Prog.  │⚠️ Review?│     │
│ [Redigera]│[Redigera]│[Redigera]│[Redigera]│[Redigera]│     │
└───────────┴──────────┴──────────┴──────────┴──────────┴─────┘

Täckning:
🟩 Fundamentals: 100% (Sugar Push, LSP, RSP, Whip)
🟨 Technique: 75% (Anchor, Timing, Stretch) - Saknas: Frame control
🟥 Advanced: 0% (planerad för vecka 7-8)
```

### 6.2 Täckningsmatris och progression

**Vad ska visualiseras**:

1. **Fundamentals-täckning**
   - Vilka grundläggande turer har introducerats?
   - Finns det luckor? (t.ex. Whip saknas i vecka 1-4 för Improver)

2. **Teknikområden**
   - Connection, Anchor, Stretch, Frame, Musicality
   - Har varje område täckts tillräckligt?

3. **Repetitionspacing**
   - Hur ofta återkommer viktiga turer?
   - Spaced repetition: Tur X bör repeteras i vecka Y

**Implementation med visuella indikatorer**:
```razor
<MudChip Color="@GetCoverageColor("Fundamentals")" Variant="Variant.Filled">
    Fundamentals: @GetCoveragePercentage("Fundamentals")%
</MudChip>

<MudTimeline>
    @foreach (var week in Course.Weeks)
    {
        <MudTimelineItem Color="@GetWeekColor(week)">
            <MudText>Vecka @week.Number</MudText>
            <MudChipSet>
                @foreach (var pattern in week.Patterns)
                {
                    <MudChip Size="Size.Small">@pattern.Name</MudChip>
                }
            </MudChipSet>
        </MudTimelineItem>
    }
</MudTimeline>
```

### 6.3 Kursplaneringsassistent

**Funktionalitet**: Hjälp instruktören att snabbt bygga en kurs med AI-assisterade förslag.

**Wizard-flöde**:
```
Steg 1: Grundinformation
  - Kursnamn: [________________]
  - Nivå: [Beginner v]
  - Antal veckor: [8]
  - Lektionstid per vecka: [75 min]
  - Huvudmål: [Fundamentals och trygg socialdans]

Steg 2: Tema och fokus
  Välj fokusområden:
  [x] Fundamentals (Sugar Push, Side Passes, Whip)
  [x] Connection och frame
  [x] Timing och musicality
  [ ] Advanced patterns
  [ ] Competition prep

Steg 3: Generera förslag
  Systemet skapar en komplett 8-veckorsplan baserat på:
  - Nivå och pedagogisk progression
  - Valda fokusområden
  - Förkunskapsberoenden
  - Spaced repetition

  [Generera automatiskt] [Bygg manuellt]

Steg 4: Granska och anpassa
  [Visuell tidslinje med föreslagna lektioner]
  Instruktören kan:
  - Byta ut enskilda turer
  - Flytta lektioner mellan veckor
  - Lägga till extra övningar
  - Justera fokus per vecka

Steg 5: Spara och exportera
  [Spara som mall] [Spara kurs] [Exportera PDF]
```

### 6.4 Implementation: Kursgenereringsalgoritm

**Backend-logik för att generera kursförslag**:
```csharp
// Request DTO
public class CourseGenerationRequest
{
    public string Name { get; set; }
    public DanceLevel Level { get; set; }
    public int DurationWeeks { get; set; }
    public List<string> FocusAreas { get; set; } // t.ex. ["Fundamentals", "Musicality"]
}

public class CourseGenerator
{
    public Course GenerateCourse(CourseGenerationRequest request)
    {
        var course = new Course
        {
            Name = request.Name,
            Level = request.Level,
            DurationWeeks = request.DurationWeeks
        };
        
        // 1. Identifiera fundamentals för nivån
        var fundamentals = GetFundamentalsForLevel(request.Level);
        
        // 2. Fördela fundamentals över första halvan av kursen
        var firstHalfWeeks = request.DurationWeeks / 2;
        DistributePatternsAcrossWeeks(fundamentals, firstHalfWeeks, course);
        
        // 3. Identifiera progressiva turer (variationer)
        var progressivePatterns = GetProgressivePatternsBasedOn(fundamentals);
        
        // 4. Fördela progressive patterns över andra halvan
        var secondHalfWeeks = request.DurationWeeks - firstHalfWeeks;
        DistributePatternsAcrossWeeks(progressivePatterns, secondHalfWeeks, course, startWeek: firstHalfWeeks);
        
        // 5. Lägg till teknikövningar baserat på fokusområden
        foreach (var focusArea in request.FocusAreas)
        {
            AddTechniqueExercises(course, focusArea);
        }
        
        // 6. Implementera spaced repetition för nyckelkoncept
        ApplySpacedRepetition(course);
        
        // 7. Validera förkunskaper och tid
        ValidateCourse(course);
        
        return course;
    }
    
    private void DistributePatternsAcrossWeeks(
        List<PatternOrExercise> patterns, 
        int weekCount, 
        Course course,
        int startWeek = 0)
    {
        // Sortera patterns efter komplexitet och dependencies
        var sortedPatterns = patterns.OrderBy(p => 
            p.Prerequisites.Count + GetComplexityScore(p)
        ).ToList();
        
        int patternsPerWeek = Math.Max(1, patterns.Count / weekCount);
        
        for (int week = 0; week < weekCount; week++)
        {
            var weekPatterns = sortedPatterns
                .Skip(week * patternsPerWeek)
                .Take(patternsPerWeek)
                .ToList();
            
            CreateLessonForWeek(course, startWeek + week, weekPatterns);
        }
    }
}
```

---

## 7. Funktioner och vyer som behövs

### 7.1 Övergripande navigationsstruktur

```
DanceCourseCreator/
├── Dashboard (översikt)
│   ├── Senaste kurser
│   ├── Statistik
│   └── Snabbåtgärder
│
├── 📚 Turbank
│   ├── Bibliotek (alla turer/övningar)
│   ├── Mina favoriter
│   ├── Lägg till ny tur
│   └── Import från CSV/JSON
│
├── 📝 Lektioner
│   ├── Lista alla lektioner
│   ├── Skapa ny lektion
│   │   ├── Från mall
│   │   ├── Från scratch med dra-och-släpp
│   │   └── Kopiera befintlig
│   └── Redigera lektion
│
├── 🎓 Kurser
│   ├── Lista alla kurser
│   ├── Skapa ny kurs
│   │   ├── Kursplaneringsassistent (wizard)
│   │   ├── Från mall
│   │   └── Manuell uppbyggnad
│   ├── Kurstidslinje (visuell veckoplanering)
│   ├── Täckningsanalys
│   └── Exportera kurs
│
├── 📋 Mallar
│   ├── Lektionsmallar
│   ├── Kursmallar
│   ├── Mina mallar
│   └── Delade teammallar
│
└── ⚙️ Inställningar
    ├── Profil
    ├── Team
    └── Exportinställningar
```

### 7.2 Prioriterade vyer att implementera

| Vy | Prioritet | Beskrivning | Status |
|----|-----------|-------------|---------|
| **Turbank-bibliotek** | Hög | Filtrerbar lista med turbankobjekt, kort-layout | Behöver UI-förbättring |
| **Detaljvy för tur** | Hög | Full metadata för varje tur inkl. relaterade och förkunskaper | Finns, behöver förbättra |
| **Lektionsbyggare** | Hög | Dra-och-släpp-interface för att bygga lektioner | Behöver implementera |
| **Kurstidslinje** | Hög | Veckovis översikt över hela kursen | Behöver implementera |
| **Kursplaneringsassistent** | Medel | Wizard för att generera kursförslag | Behöver implementera |
| **Täckningsanalys** | Medel | Visuell representation av fundamentals/teknik-coverage | Behöver implementera |
| **Mallbibliotek** | Medel | Spara och återanvänd lektioner/kurser som mallar | Backend finns, UI saknas |
| **Export-funktionalitet** | Medel | PDF/Markdown-export med formatering | Backend finns, UI behöver kompletteras |

---

## 8. Dataorganisation

### 8.1 Nuvarande datamodell (sammanfattning)

```csharp
PatternOrExercise
├── Metadata: name, aliases, level, description
├── Pedagogiskt: steps, counts, prerequisites, teachingPoints, commonMistakes
├── Tekniskt: holds, slot, rotations, bpmRange, estimatedMinutes
└── Relations: related[], prerequisites[], variations[]

Lesson
├── Struktur: sections[] med items-referenser
├── Metadata: duration, totalEstimatedMinutes, notes
├── Pedagogiskt: reviewers[], history[], version
└── Relation: courseId

Course
├── Metadata: name, level, durationWeeks, goals[]
├── Struktur: lessons[], themesByWeek[]
├── Analys: coverageMetrics, repetitionPlan
└── Relations: creator, lessons collection
```

### 8.2 Förbättringsförslag för datamodellen

**A. Progressionsträd och relationer**

Lägg till explicit progressionsträd-struktur:
```csharp
public class ProgressionNode
{
    public string PatternId { get; set; }
    public List<string> PrerequisiteIds { get; set; }
    public List<string> NextStepIds { get; set; }
    public int RecommendedWeek { get; set; } // För en given kurslängd
    public DanceLevel MinimumLevel { get; set; }
}

// Exempel:
// Sugar Push → Left Side Pass → LSP Variation
// Sugar Push → Sugar Tuck → Tuck Turn Variation
```

**B. Strukturerad täckningsmetrik**

Istället för `CoverageMetrics` som generisk JSON-sträng:
```csharp
public class CourseCoverageMetrics
{
    public Dictionary<string, double> FundamentalsCoverage { get; set; }
    // "SugarPush": 1.0, "LeftSidePass": 1.0, "Whip": 0.5 (introducerad men ej etablerad)
    
    public Dictionary<string, int> TechniqueAreaCounts { get; set; }
    // "Connection": 3, "Anchor": 4, "Timing": 2
    
    public List<string> MissingFundamentals { get; set; }
    // ["RightSidePass"] om inte lärd än men nivån kräver det
    
    public double OverallCompleteness { get; set; } // 0.0 - 1.0
}
```

**C. Spaced repetition-plan**

Strukturerad repetitionsplanering baserad på algoritmer för distribuerad inlärning:
```csharp
public class RepetitionSchedule
{
    public string PatternId { get; set; }
    public DateTime IntroducedAt { get; set; } // Vecka det introducerades
    public List<DateTime> RepetitionDates { get; set; } // Föreslagna repetitioner baserade på 1-2-4-8 veckorsmönster
    public RepetitionStatus Status { get; set; }
}

public enum RepetitionStatus
{
    NotIntroduced,
    RecentlyIntroduced, // < 1 vecka sedan
    NeedsRepetition,    // 1-2 veckor sedan, föreslå repetition
    WellEstablished     // > 3 repetitioner med 2+ veckors spacing
}
```

### 8.3 API-endpoints för förbättrad dataåtkomst

**Nya endpoints**:
```csharp
// Progressionsinformation
GET /api/patterns/{id}/progression-path
  → Returnerar { previous: [...], current: pattern, next: [...] }

// Kursanalys
GET /api/courses/{id}/coverage-analysis
  → Returnerar CourseCoverageMetrics

POST /api/courses/{id}/validate-progression
  → Validerar att förkunskaper följs och returnerar varningar

// Repetitionsplanering
GET /api/courses/{id}/repetition-schedule
  → Returnerar RepetitionSchedule[] för alla patterns i kursen

// Rekommendationer
POST /api/recommendations/next-patterns
  Body: { courseId, currentWeek, level, alreadyTaughtPatterns }
  → Returnerar föreslagna patterns för nästa vecka
```

---

## 9. Intuitiv kursgenereringsprocess

### 9.1 Designprinciper för intuitivt flöde

1. **Progressiv disclosure**: Visa bara vad som är relevant för nuvarande steg
2. **Instant feedback**: Realtidsvalidering och visuella indikatorer
3. **Flexibilitet**: Både guidat (wizard) och fritt (manuellt) flöde
4. **Återanvändning**: Enkelt att utgå från mallar och befintligt innehåll
5. **Visuell klarhet**: Grafisk representation av progression och täckning

### 9.2 Två parallella flöden

#### Flöde A: Snabb kursbyggare (guidat)

**För**: Nybörjare eller när man vill komma igång snabbt

```
Starta → Wizard → 
  [Steg 1: Kursinfo] → 
  [Steg 2: Fokusområden] → 
  [Steg 3: AI-generering] → 
  [Steg 4: Granska/anpassa] → 
  Spara/Exportera
```

**Fördelar**:
- Minimalt antal klick till första versionen
- Pedagogiskt välgrundad utgångspunkt
- Lätt att justera efteråt

#### Flöde B: Manuell kursbyggare (flexibelt)

**För**: Erfarna instruktörer som vill ha full kontroll

```
Skapa tom kurs → 
  [Veckoplanering: dra lektioner/turer till veckor] → 
  [Detaljredigera varje lektion] → 
  [Verifiera med täckningsanalys] → 
  Spara/Exportera
```

**Fördelar**:
- Total kontroll över struktur
- Bygga iterativt vecka för vecka
- Anpassa efter deltagarnas behov

### 9.3 UX-mönster att implementera

**A. Smart defaults**
```
När en lektion skapas:
- Förfyll standardsektioner (Warmup, Technique, Patterns, etc.)
- Föreslå lämplig varaktighet baserat på nivå (75 min för Beginner)
- Visa "Vanliga mallar" för snabb start
```

**B. Contextual help**
```
Vid varje steg, visa hjälptext:
"Tips: För en 8-veckors Beginner-kurs, rekommenderar vi 
 3-4 fundamentala turer spridda över första halvan."

[Läs mer] [Visa exempel]
```

**C. Undo/Redo och autosave**
```
- Autospara varje 30 sekunder
- Versionering med historik
- Knapp för "Återställ till senaste sparad version"
```

**D. Förhandsvisning**
```
Varje vy har "Förhandsvisning"-läge:
[Redigeringsläge] [Förhandsvisning] [Exportvy]

Förhandsvisning visar hur kursen ser ut för:
- Instruktören (komplett info)
- Eleven (om delad: endast highlights)
- Export (PDF/print)
```

---

## 10. Implementeringsprioriteringar

### 10.1 Fas 1: Grundläggande flöde (Veckor 1-4)

**Mål**: Möjliggöra grundläggande kursbyggande med turbanken

| Uppgift | Beskrivning | Estimerad tid |
|---------|-------------|---------------|
| **Turbank UI** | Förbättrad filtrerbar listvy med kortvyer | 1 vecka |
| **Detaljvy för turer** | Modal/drawer med full metadata | 3 dagar |
| **Lektionsbyggare v1** | Grundläggande dra-och-släpp (MudBlazor DropZones) | 1 vecka |
| **Validering** | Real-time validering av förkunskaper och tid | 3 dagar |
| **Kurstidslinje v1** | Enkel veckovis översikt | 1 vecka |

**Utfall Fas 1**: Instruktör kan manuellt bygga en kurs med turer från turbanken.

### 10.2 Fas 2: Intelligens och automatisering (Veckor 5-8)

**Mål**: Lägg till rekommendationer och täckningsanalys

| Uppgift | Beskrivning | Estimerad tid |
|---------|-------------|---------------|
| **Rekommendationsalgoritm** | Backend-logik för att föreslå turer | 1 vecka |
| **Täckningsanalys** | Beräkning och visualisering av coverage metrics | 1 vecka |
| **Kursplaneringsassistent** | Wizard för AI-genererad kurs | 1,5 veckor |
| **Repetitionsplanering** | Spaced repetition-algoritm och UI | 4 dagar |

**Utfall Fas 2**: Systemet ger pedagogiskt grundade förslag och hjälper till att bygga kompletta kurser.

### 10.3 Fas 3: Återanvändning och delning (Veckor 9-12)

**Mål**: Mallar, export och teamsamarbete

| Uppgift | Beskrivning | Estimerad tid |
|---------|-------------|---------------|
| **Mallbibliotek UI** | Spara och använda lektions-/kursmallar | 1 vecka |
| **Export-funktionalitet** | PDF/Markdown-export med design | 1 vecka |
| **Teamdelning** | Dela mallar och kurser inom team (FR-070/071) | 1 vecka |
| **Kommentarer och review** | Samarbetsflöde för kursgranskning | 4 dagar |

**Utfall Fas 3**: Komplett ekosystem för att skapa, dela och iterera på kursmaterial.

### 10.4 Fas 4: Avancerade funktioner (Veckor 13+)

**Mål**: Förfina och utöka

| Uppgift | Beskrivning | Estimerad tid |
|---------|-------------|---------------|
| **Progressionsträd-visualisering** | Grafisk vy av turrelationer | 1 vecka |
| **Import-funktionalitet** | CSV/JSON-import av turbank | 3 dagar |
| **Rapporter och insikter** | FR-080..082: Användningsstatistik | 1 vecka |
| **Mobiloptimering** | PWA och responsiv design | 1,5 veckor |

---

## 11. Konkreta nästa steg

### 11.1 Omedelbar sprint (Vecka 1)

**Fokus**: Turbank och grundläggande lektionsbyggare

#### Uppgift 1: Förbättra Turbank-vyn
```
1. Skapa ny UI-komponent: PatternBankView.razor
2. Implementera filterlogik (nivå, typ, tags)
3. Designa kortvyer med MudBlazor MudCard
4. Lägg till "quick add"-funktion till arbetsyta
5. Testa med befintlig pattern-data
```

**Tekniska tasks**:
- [ ] Skapa `PatternBankView.razor` i `Client/Pages/`
- [ ] Implementera filter-state med Blazor state management
- [ ] Skapa `PatternCard.razor` komponent
- [ ] API-anrop för filtrering: uppdatera `PatternsService.cs`
- [ ] Unit tests för filter-logik

#### Uppgift 2: Lektionsbyggare med dra-och-släpp
```
1. Skapa LessonBuilderView.razor
2. Implementera MudBlazor DropContainer med zoner per sektion
3. Lägg till real-time tidsberäkning
4. Implementera validering av förkunskaper (frontend + backend)
5. Spara lektion till databas
```

**Tekniska tasks**:
- [ ] Skapa `LessonBuilderView.razor` i `Client/Pages/`
- [ ] Implementera `OnPatternDropped` event handler
- [ ] Skapa `LessonValidationService.cs` för validering
- [ ] API-endpoint: `POST /api/lessons/build` i `LessonsController.cs`
- [ ] DTO: `LessonBuilderRequest` och `LessonValidationResponse`

### 11.2 Sprint 2 (Vecka 2-3)

**Fokus**: Kurstidslinje och täckningsanalys

#### Uppgift 3: Visuell kurstidslinje
```
1. Skapa CourseTimelineView.razor
2. Veckovis layout med MudBlazor Timeline/Grid
3. Dra-och-släpp för att flytta lektioner mellan veckor
4. Visuella indikatorer för täckning per vecka
5. Quick-edit drawer för varje vecka
```

**Tekniska tasks**:
- [ ] Skapa `CourseTimelineView.razor`
- [ ] Implementera veckovis grid-layout
- [ ] API: `GET /api/courses/{id}/timeline` med veckodata
- [ ] Implementera drag-mellan-veckor-logik
- [ ] CSS för visuella täckningsindikatorer

#### Uppgift 4: Täckningsanalys
```
1. Backend: Skapa CourseCoverageService.cs
2. Algoritm för att beräkna fundamentals/technique-coverage
3. Frontend: Visualisera med MudBlazor Charts
4. Varningar för missing fundamentals
5. Exportera coverage-rapport
```

**Tekniska tasks**:
- [ ] Skapa `CourseCoverageService.cs` i `API/Services/`
- [ ] Implementera `CalculateCoverageMetrics()`
- [ ] API-endpoint: `GET /api/courses/{id}/coverage-analysis`
- [ ] Skapa `CoverageAnalysisView.razor` komponent
- [ ] MudBlazor Charts för visualisering

### 11.3 Sprint 3 (Vecka 4-5)

**Fokus**: Rekommendationer och kursplaneringsassistent

#### Uppgift 5: Rekommendationsalgoritm
```
1. Backend: Skapa RecommendationService.cs
2. Algoritm baserad på level, prerequisites, tags
3. API-endpoint för recommendations
4. Frontend: Visa "Föreslagna turer" i lektionsbyggare
5. Testa med olika scenarier
```

#### Uppgift 6: Kursplaneringsassistent (Wizard)
```
1. Skapa CourseWizard.razor med steg-för-steg-flöde
2. Steg 1: Grundinfo (MudStepper)
3. Steg 2: Fokusområden (checkboxes)
4. Steg 3: Generera förslag (backend-anrop till CourseGenerator)
5. Steg 4: Granska och anpassa (visuell tidslinje)
6. Spara komplett kurs
```

### 11.4 Definition of Done för varje uppgift

Varje uppgift anses klar när:
- [ ] Kod är skriven och testad lokalt
- [ ] Unit tests finns där tillämpligt
- [ ] UI är responsiv och följer MudBlazor-designsystem
- [ ] API-dokumentation uppdaterad (Swagger)
- [ ] Code review genomförd
- [ ] Manuellt testad i dev-miljö
- [ ] Dokumentation i `/docs` uppdaterad

---

## 12. Möjligheter och fördelar

### 12.1 Pedagogiska fördelar

| Fördel | Beskrivning | Impact |
|--------|-------------|---------|
| **Konsekvent progression** | Systemet säkerställer att förkunskaper följs | Hög |
| **Täckning av fundamentals** | Automatisk varning om viktiga koncept saknas | Hög |
| **Spaced repetition** | Nyckelkoncept återkommer i rätt intervall | Medel |
| **Kunskapsdelning** | Mallar och best practices delas mellan instruktörer | Hög |

### 12.2 Tidsbesparingar

| Aktivitet | Nuläge (utan system) | Med förbättrat flöde | Besparing |
|-----------|----------------------|----------------------|-----------|
| **Planera 8-veckors kurs** | 4-6 timmar | 1-2 timmar (med wizard) | 60-70% |
| **Skapa enstaka lektion** | 30-60 min | 10-20 min (med mallar) | 50-65% |
| **Hitta rätt tur i anteckningar** | 5-10 min | < 30 sekunder (turbank) | 95% |
| **Exportera material för delning** | 30 min (manuell formatering) | 2 min (automatisk export) | 93% |

### 12.3 Kvalitetsförbättringar

- **Färre misstag**: Automatisk validering förhindrar pedagogiska fel
- **Bättre struktur**: Mallar säkerställer välavvägda lektioner
- **Enklare iteration**: Versionshantering möjliggör snabba justeringar
- **Dokumentation**: All metadata finns sparad och sökbar

---

## 13. Utmaningar och risker

### 13.1 Tekniska utmaningar

| Utmaning | Risk | Mitigering |
|----------|------|-----------|
| **Dra-och-släpp-komplexitet** | Buggig UX på touchskärmar | Testa på olika enheter, fallback till knappar |
| **Prestanda vid stora turbanker** | Långsam filtrering vid > 500 turer | Implementera paginering och lazy loading |
| **Rekommendationsalgoritm** | Ger irrelevanta förslag | Iterativ förbättring, användarfeedback-loop |

### 13.2 UX-utmaningar

| Utmaning | Risk | Mitigering |
|----------|------|-----------|
| **Överväldigande UI** | För många funktioner på en gång | Progressiv disclosure, guidad onboarding |
| **Inlärningskurva** | Instruktörer har inte tid lära sig komplicerat system | Wizards, tutorials, tooltips |
| **Mobilanvändning** | Mindre skärm = svårare dra-och-släpp | Responsiv design, alternativa input-metoder |

### 13.3 Organisatoriska utmaningar

| Utmaning | Risk | Mitigering |
|----------|------|-----------|
| **Adoption**: Instruktörer fortsätter med gamla metoder | Låg ROI | Pilotgrupp, visa tidsbesparing, onboarding |
| **Datakvalitet**: Turbank med dålig metadata | Systemet ger dåliga förslag | Import av välstrukturerad seed-data, guidelines |
| **Variation i terminologi**: Olika skolor använder olika namn | Förvirring | Robust alias-system, anpassningsbara taggar |

---

## 14. Framtida möjligheter (utöver initial scope)

### 14.1 AI-assisterad kursgenerering

- **GPT-baserade förslag**: Använd LLM för att föreslå undervisningspunkter
- **Musikrekommendationer**: AI som lyssnar på BPM och stilkänsla
- **Automatisk feedback**: Analysera kursprestanda och föreslå justeringar

### 14.2 Community-funktioner

- **Delningsmarknadsplats**: Instruktörer kan sälja/dela kurser
- **Rating och reviews**: Kommentarer på mallar och kurser
- **Diskussionsforum**: Pedagogiska diskussioner kring specifika turer

### 14.3 Elevperspektivet

- **Elevportal**: Elever ser kommande lektioner och kan förbereda sig
- **Progressspårning**: Elever bockar av lärdomar och får feedback
- **Video-integration**: Direkta länkar till videotutorials för hemträning

---

## 15. Slutsatser

### 15.1 Huvudrekommendationer

**1. Prioritera turbank-upplevelsen**  
Turbanken är kärnan i systemet. En intuitiv, filtrerbar och visuell turbank är grundstenen för allt annat.

**2. Implementera dra-och-släpp-lektionsbyggare**  
Detta är det mest värdefulla verktyget för daglig användning. Fokusera på enkelhet och real-time feedback.

**3. Bygg kurstidslinjen med visuell täckning**  
Instruktörer måste snabbt kunna se hela kursen och identifiera luckor. Visuell representation är avgörande.

**4. Lägg till intelligent assistent**  
Rekommendationer och kursgenerering gör systemet från "verktyg" till "intelligent partner". Detta differentierar DanceCourseCreator.

**5. Möjliggör återanvändning och delning**  
Mallar och teamsamarbete skapar nätverkseffekter och ökar värdet för alla användare.

### 15.2 Framgångsfaktorer

- ✅ **Snabb time-to-value**: Instruktören ska kunna skapa sin första kurs inom 30 minuter
- ✅ **Pedagogisk trovärdighet**: Systemet måste förstå WCS-pedagogik och progression
- ✅ **Flexibilitet**: Både wizards och manuell kontroll måste finnas
- ✅ **Visuell klarhet**: Grafisk representation av progression och täckning
- ✅ **Mobilvänlig**: Många instruktörer planerar på plats på surfplatta

### 15.3 Mätbara mål

**Efter Fas 1 (4 veckor)**:
- [ ] Instruktör kan manuellt bygga en 8-veckorskurs på < 2 timmar
- [ ] Turbank-sökning returnerar resultat på < 200 ms

**Efter Fas 2 (8 veckor)**:
- [ ] Kursplaneringsassistent genererar en komplett Beginner-kurs på < 5 minuter
- [ ] Rekommendationsalgoritmen ger minst 3 relevanta förslag i 90% av fallen

**Efter Fas 3 (12 veckor)**:
- [ ] 50% av kurser skapas från mallar
- [ ] Export till PDF tar < 10 sekunder

---

## Appendix A: Ordlista

| Term | Engelska | Definition |
|------|----------|------------|
| **Turbank** | Pattern Bank | Bibliotek av WCS-turer och teknikövningar |
| **Turer** | Patterns | Dansmönster (t.ex. Sugar Push, Whip) |
| **Teknikövningar** | Exercises | Fokuserade drills (t.ex. Anchor drill, Connection exercise) |
| **Förkunskaper** | Prerequisites | Turer/koncept som måste läras innan en given tur |
| **Progression** | Progression | Pedagogisk uppbyggnad från enkelt till svårt |
| **Täckning** | Coverage | Hur många fundamentala koncept som täckts i en kurs |
| **Spaced repetition** | Spaced repetition | Återkommande träning med ökande intervall |

---

## Appendix B: Referensmaterial

- **Kravspecifikation.md**: Fullständig kravdokumentation
- **README.md**: Teknisk översikt och installation
- **docs/Implementering-Mallsystem.md**: Detaljer om template-systemet
- **docs/Implementering-Teamsamarbete.md**: Teamfunktionalitet

---

**Baserad på**: Kravspecifikation v1.0, befintlig kod-analys, WCS-pedagogiska best practices

**Nästa steg**: Börja med Sprint 1 - Turbank och Lektionsbyggare (se avsnitt 11.1)
