# Fas 1: Kursplanering med progression och täckning

## Översikt

Detta dokument beskriver implementationen av Fas 1 (Phase 1) för kursplaneringsfunktionalitet med progression och täckning i DanceCourseCreator-applikationen.

## Funktionella krav som uppfylls

### FR-020: Skapa kurs med koppling till progression och täckning
- Kursstrukturen har redan stöd för mål, veckovis teman och lektioner
- Nya endpoints analyserar automatiskt progression och täckning baserat på kursinnehåll

### FR-021: Översikt över täckning av grundblock
Systemet spårar och visualiserar täckning av 8 grundläggande West Coast Swing-färdigheter:
- Sugar Push
- Left Side Pass
- Right Side Pass
- Whip
- Connection
- Anchor
- Stretch
- Musicality

För varje färdighet visas:
- Om den täcks i kursen
- Vilka veckor den introduceras
- Antal repetitioner
- Total täckningsgrad i procent

### FR-022: Repetitionsplanering (spaced repetition)
Systemet analyserar:
- Avstånd mellan repetitioner av samma koncept
- Varnar om repetitioner är för tätt (mindre än 2 veckor)
- Identifierar koncept som aldrig repeteras i längre kurser (>4 veckor)

### FR-023: Konfliktvarningar
Systemet upptäcker och varnar för:
- **Saknade fundamentals**: Viktiga färdigheter som saknas baserat på kursnivå
  - Nybörjare: Sugar Push, Connection, Anchor
  - Förbättrare: + Left Side Pass, Right Side Pass, Stretch
  - Medel: + Whip
  - Avancerad: Alla 8 färdigheter
- **Överbelastning**: För många nya koncept per vecka (max 3 rekommenderat)
- **Dålig repetitionsspacing**: Repetitioner för tätt eller helt utebliven repetition
- **Progressionshastighet**: Automatisk bedömning via progressionspoäng

## API-endpoints

### GET /api/courses/{id}/coverage
Returnerar täckningsmetrik för en kurs.

**Response exempel:**
```json
{
  "courseId": "abc123",
  "courseName": "WCS Nybörjarkurs",
  "durationWeeks": 8,
  "fundamentalsCoverage": {
    "Sugar Push": {
      "skillName": "Sugar Push",
      "isCovered": true,
      "weeksIntroduced": [1, 3, 5],
      "repetitionCount": 3
    },
    ...
  },
  "weeklyProgress": [
    {
      "weekNumber": 1,
      "theme": "Grundläggande rytm",
      "coveredConcepts": ["Sugar Push", "Connection"],
      "lessonId": "lesson-1",
      "totalMinutes": 90
    },
    ...
  ],
  "totalSkillsCovered": 6,
  "coveragePercentage": 75.0
}
```

### GET /api/courses/{id}/progression
Returnerar progressionsanalys med varningar och rekommendationer.

**Response exempel:**
```json
{
  "courseId": "abc123",
  "courseName": "WCS Nybörjarkurs",
  "level": "Beginner",
  "warnings": [
    {
      "severity": "High",
      "type": "MissingFundamental",
      "weekNumber": null,
      "message": "Saknar grundläggande färdighet för Beginner: Anchor",
      "recommendation": "Lägg till övningar eller turer som täcker Anchor"
    },
    {
      "severity": "Medium",
      "type": "Overloading",
      "weekNumber": 2,
      "message": "Vecka 2: För många nya koncept (5)",
      "recommendation": "Överväg att sprida ut koncepten över flera veckor eller ta bort 2 koncept"
    }
  ],
  "progressionScore": 72.5
}
```

## Frontend-komponenter

### CourseProgressionDialog
En omfattande dialogkomponent som visar:

1. **Progressionsbedömning**
   - Cirkulär progressindikator med poäng (0-100)
   - Färgkodning: Grön (≥80), Blå (≥60), Orange (≥40), Röd (<40)
   - Kursnivå och täckningsstatistik

2. **Varningar och rekommendationer**
   - Lista med alla upptäckta problem
   - Allvarlighetsnivå (Hög/Medel/Låg) med ikoner
   - Konkreta rekommendationer för varje varning

3. **Täckningstabellen**
   - Alla 8 grundläggande färdigheter
   - Status (Täcks/Täcks ej)
   - Vilka veckor färdigheten introduceras
   - Antal repetitioner

4. **Veckovis progression**
   - Tidslinje-vy med alla veckor
   - Tema för varje vecka
   - Koncept som täcks
   - Total tid i minuter

### Courses.razor uppdateringar
- Ny "Progression"-knapp på varje kurskort
- Ikon: TrendingUp
- Öppnar CourseProgressionDialog

## Teknisk implementation

### Backend (API)

**ProgressionService.cs**
- Analyserar kursinnehåll och genererar metriker
- Jämför mönster/övningar mot fundamentala färdigheter
- Kontrollerar mönsternamn, taggar och undervisningspunkter
- Beräknar progressionspoäng baserat på täckning och varningar

**CoursesController.cs**
- Injicerar ProgressionService
- Exponerar två nya endpoints för coverage och progression
- Hanterar fel och returnerar lämpliga HTTP-statuskoder

### Frontend (Web)

**Models.cs tillägg**
- `CourseCoverageMetrics`
- `SkillCoverage`
- `WeekProgress`
- `ProgressionAnalysis`
- `ProgressionWarning`

**CoursesService.cs uppdateringar**
- `GetCourseCoverageAsync(string id)`
- `GetCourseProgressionAsync(string id)`

**Ny komponent: CourseProgressionDialog.razor**
- MudBlazor-baserad dialogkomponent
- Responsiv design
- Färgkodade visualiseringar
- Tidslinje för veckovis progression

## Användning

1. **Skapa en kurs** med lektioner och mönster/övningar
2. **Klicka på "Progression"** på kurskortet
3. **Granska progressionspoängen** för en snabb översikt
4. **Läs varningarna** och följ rekommendationerna
5. **Kontrollera täckningstabellen** för att se vilka färdigheter som saknas
6. **Använd tidslinjen** för att se hur kursen utvecklas vecka för vecka

## Valideringskriterier

- ✅ API-endpoints returnerar korrekt data
- ✅ Frontend visar progressionspoäng
- ✅ Varningar visas med korrekt allvarlighetsgrad
- ✅ Täckningstabellen listar alla 8 färdigheter
- ✅ Tidslinje visar veckovis progression
- ✅ Bygget lyckas utan fel
- ✅ Komponenten öppnas när "Progression"-knappen klickas

## Framtida förbättringar

1. **Automatiska förslag**: AI-baserade rekommendationer för att förbättra kursen
2. **Jämförelser mellan kurser**: Se hur olika kurser täcker samma färdigheter
3. **Exportfunktion**: Exportera progressionsrapporten som PDF
4. **Interaktiv redigering**: Klicka på en vecka i tidslinjen för att redigera lektionen
5. **Anpassade färdigheter**: Låt instruktörer definiera egna färdigheter att spåra
6. **Progression mellan kurser**: Spåra hur elever utvecklas över flera kursnivåer

## Testning

E2E-tester finns i `CourseProgressionTests.cs`:
- Öppna progressionsdialog
- Verifiera progressionspoäng
- Kontrollera täckningstabellen
- Granska veckovis progression

Kör tester med:
```bash
dotnet test --filter "TestCategory=Progression"
```

## Säkerhet och prestanda

- Ingen känslig data exponeras i progressionsanalys
- Cachas inte mellan requests (varje analys är live)
- Liten datavolym (vanligtvis <50KB per kurs)
- Snabb beräkning (<100ms för typisk 8-veckors kurs)

## Referenser

- Kravspecifikation: FR-020, FR-021, FR-022, FR-023
- Issue: [Fas 1] Kursplanering med progression och täckning
- Dokumentation: Se även README.md och Kravspecifikation.md
