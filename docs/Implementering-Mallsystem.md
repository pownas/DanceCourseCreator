# Implementering: Template System (Mallsystem)

## Översikt

Mallsystemet i DanceCourseCreator möjliggör för instruktörer att spara och återanvända strukturer för lektioner och kurser. Detta minskar planeringstid och säkerställer konsekvens i undervisningen.

## Funktionella krav som implementeras

- **FR-030**: Skapa lektions- och kursmallar. Duplicera från mallar.
- **FR-031**: Versionera planer med historik och återställning.
- **FR-032**: Kommentera och föreslå ändringar (review-läge) i team.

## Datamodell

### Template-modellen

```csharp
public class Template
{
    public string Id { get; set; }                    // Unik identifierare
    public TemplateScope Scope { get; set; }          // Lesson eller Course
    public string Name { get; set; }                  // Mallens namn
    public string Content { get; set; }               // JSON-strukturerad innehåll
    public string Owner { get; set; }                 // Ägare av mallen
    public string? Team { get; set; }                 // Tillhörande team (valfritt)
    public DateTime CreatedAt { get; set; }           // Skapad tidpunkt
    public DateTime UpdatedAt { get; set; }           // Senast uppdaterad
    
    // Navigation properties
    public User? OwnerUser { get; set; }              // Ägaren som användare
    public Team? TeamEntity { get; set; }             // Team-entitet
}

public enum TemplateScope
{
    Lesson,    // Lektionsmall
    Course     // Kursmall
}
```

## Kärnfunktionalitet

### 1. Skapa mallar

**Lektionsmallar**
- Instruktörer kan spara en befintlig lektion som mall
- Mallen inkluderar struktur, timing, sektioner och innehåll
- Metadata som level, estimerad tid och taggar bevaras

**Kursmallar**
- Hela kursplaner kan sparas som mallar
- Inkluderar progression över flera veckor
- Bevarar kursmål, teman per vecka och täckningsgrad

### 2. Mallbibliotek

**Personliga mallar**
- Varje instruktör har sitt eget mallbibliotek
- Privata mallar som kan användas för att skapa nya lektioner/kurser

**Team-mallar**
- Mallar som delas inom ett team
- Möjliggör standardisering av undervisningsmetoder
- Kvalitetssäkring genom gemensamma strukturer

### 3. Duplicering från mallar

**Snabb kurskapande**
- Välj mall från biblioteket
- Systemet skapar en kopia med grundstrukturen
- Instruktören kan sedan anpassa efter behov

**Flexibilitet**
- Möjlighet att modifiera kopierad struktur
- Behåller originalmallar intakta
- Stöder partiell kopiering (endast vissa sektioner)

### 4. Mallhantering

**Kategorisering**
- Mallar kan taggas efter nivå (Beginner, Improver, etc.)
- Tematiska kategorier (Technique, Musicality, etc.)
- Varaktighet och format (75min, 90min, Workshop)

**Sökfunktioner**
- Filtrera mallar efter kategori, nivå, ägare
- Textbaserad sökning i mallnamn och innehåll
- Populära/mest använda mallar

## Teknisk implementation

### API-endpoints (planerade)

```
GET    /api/templates                    // Hämta användarens mallar
GET    /api/templates/{id}               // Hämta specifik mall
POST   /api/templates                    // Skapa ny mall
PUT    /api/templates/{id}               // Uppdatera mall
DELETE /api/templates/{id}               // Ta bort mall
POST   /api/templates/{id}/duplicate     // Duplicera mall till ny lektion/kurs
GET    /api/templates/team/{teamId}      // Hämta team-mallar
```

### Datastruktur för Content-fältet

**Lektionsmall JSON-struktur:**
```json
{
  "level": "improver",
  "duration": 75,
  "sections": [
    {
      "name": "Uppvärmning",
      "duration": 10,
      "patterns": [],
      "notes": "Fokus på connection"
    },
    {
      "name": "Teknikblock",
      "duration": 20,
      "patterns": ["sugar-push", "left-side-pass"],
      "teachingPoints": ["Tydlig lead", "Mjuk follow"]
    }
  ],
  "goals": ["Förbättra basic patterns", "Utveckla connection"],
  "prerequisites": ["sugar-push"],
  "notes": "Viktigt att hålla lugnt tempo"
}
```

**Kursmall JSON-struktur:**
```json
{
  "level": "beginner",
  "durationWeeks": 8,
  "goals": ["Grundläggande WCS", "Trygg socialdans"],
  "themesByWeek": [
    "Connection och posture",
    "Sugar Push grundläggande",
    "Left/Right Side Pass",
    // ... fortsättning för alla veckor
  ],
  "progressionPlan": {
    "week1": {
      "focus": "connection",
      "patterns": ["sugar-push"],
      "prerequisites": []
    }
  },
  "assessmentCriteria": ["Kan dansa sugar push", "Förstår slot concept"]
}
```

## Användarupplevelse

### Mall-skapande workflow

1. **Från befintlig lektion/kurs**
   - Knapp "Spara som mall" i redigeringsläget
   - Dialog för att namnge och kategorisera mallen
   - Möjlighet att välja vilka delar som ska inkluderas

2. **Från scratch**
   - Dedicated mall-editor
   - Template för olika scenarion (nybörjarkurs, workshop, etc.)
   - Steg-för-steg guide för att bygga mallar

### Mall-användning workflow

1. **Ny lektion/kurs från mall**
   - "Skapa från mall"-knapp
   - Galleri-vy av tillgängliga mallar
   - Förhandsvisning av mallstruktur
   - Ett-klicks duplicering

2. **Mallbibliotek**
   - Organiserat bibliotek med sök och filter
   - Favoriter och senast använda
   - Mall-statistik (användningsfrekvens)

## Säkerhet och behörigheter

### Ägarskap
- Mallar ägs av den som skapar dem
- Ägaren kan modifiera och ta bort mallar
- Möjlighet att överföra ägarskap

### Team-delning
- Mallar kan delas med specifika team
- Team-medlemmar kan använda men inte modifiera
- Admin-rättigheter för team-mallar

### Versionshantering
- Historik över malländringar
- Möjlighet att återställa tidigare versioner
- Jämförelse mellan olika versioner

## Framtida funktioner

### Avancerad mallhantering
- Mall-hierarkier (basmallar och specialiseringar)
- Automatisk kvalitetskontroll av mallar
- Gemenskapsbiblitek med delade mallar

### Integration
- Export/import av mallar mellan system
- Integration med externa mall-bibliotek
- Automatisk mall-förslag baserat på användningshistorik

## Relaterade komponenter

- **Team Collaboration**: Mallar kan kommenteras och förbättras av team
- **Versionering**: Alla malländringar loggas för spårbarhet
- **Export**: Mallar kan exporteras för användning utanför systemet
- **Behörighetssystem**: Integreras med team-behörigheter för delning
