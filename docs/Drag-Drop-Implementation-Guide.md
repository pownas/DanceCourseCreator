# Förbättringar av Kursskapandeflödet - Implementationsguide

## Översikt

Denna guide beskriver de nya drag-and-drop-funktionerna och förbättringarna som gjorts för att göra kursskapandet enklare och mer intuitivt.

## Nya Komponenter

### 1. LessonBuilderWithDragDrop.razor

En helt ny lektionsbyggare med följande funktioner:

#### Huvudfunktioner
- **Drag-and-Drop Interface**: Dra moment från turbanken direkt till lektionssektioner
- **Real-time Tidsvalidering**: Se direkt om din lektion är för lång, för kort eller perfekt
- **Visuell Feedback**: Färgkodade indikatorer för tid och status
- **Mobilvänlig**: Alternativa kontroller för enheter utan mus
- **Sektionshantering**: Enkelt lägga till, ta bort och organisera sektioner

#### Användningsflöde
1. **Skapa/Redigera Lektion**: Öppna lektionsbyggaren från Lektioner-sidan
2. **Lägg till Sektioner**: Klicka "Lägg till sektion" för att skapa nya avsnitt
3. **Välj Moment**: 
   - Desktop: Dra moment från turbanken till sektioner
   - Mobil: Använd "Lägg till moment"-knappen
4. **Filtrera och Sök**: Använd filteralternativ för att hitta rätt moment snabbt
5. **Verifiera Tid**: Kontrollera tidsbalken - den visar om lektionen passar inom planerad tid
6. **Spara**: Dina ändringar sparas automatiskt i databasen

#### Visuella Indikatorer

**Tidsvalidering:**
- 🟢 Grön: Perfekt tid (80-100% av planerad tid)
- 🔵 Blå: Mer tid finns (< 80%)
- 🟡 Gul: Lite för lång (100-110%)
- 🔴 Röd: För lång! (> 110%)

**Sektionstyper:**
- 🏃 Uppvärmning (Warmup)
- ⚙️ Teknik (Technique)
- 🧠 Turer (Patterns)
- 🔀 Kombination (Combination)
- 🔁 Repetition (Repetition)
- 👥 Socialdans (Social)

### 2. CourseTimelineBuilder.razor

En visuell kurstidslinje för att organisera kurser vecka för vecka.

#### Huvudfunktioner
- **Veckovis Översikt**: Se alla veckor i kursen i en tidslinje
- **Lektionshantering**: Lägg till, redigera och ta bort lektioner per vecka
- **Täckningsöversikt**: Se kursstatus och progression
- **Veckoteman**: Definiera fokusområden för varje vecka
- **Statistik**: Total tid, antal lektioner, och kompletthetsstatus

#### Användningsflöde
1. **Öppna Tidslinje**: Från Kurser-sidan, klicka "Visa" på en kurs
2. **Se Översikt**: Få en överblick över kursstatus och mål
3. **Hantera Veckor**: 
   - Lägg till lektioner till specifika veckor
   - Redigera befintliga lektioner
   - Ta bort lektioner från veckor
4. **Verifiera Progression**: Se hur många lektioner som är skapade vs planerade

## Design och UX-principer

### Responsiv Design
- **Desktop (> 768px)**: Full drag-and-drop-funktionalitet
- **Tablet**: Hybridläge med både drag-and-drop och knappar
- **Mobil (< 768px)**: Primärt knapp-baserad interaktion med touch-vänliga kontroller

### Färgkodning
- **Primär (Blå)**: Huvudåtgärder och turer
- **Sekundär (Lila)**: Övningar och alternativa åtgärder
- **Success (Grön)**: Nybörjare, godkända statusar
- **Warning (Orange)**: Medelnivå, varningar
- **Error (Röd)**: Avancerad, problem

### Accessibility
- Touch-vänliga mål (minimum 44x44px)
- Tydliga visuella indikatorer
- Alternativa interaktionsmetoder
- Kontrast och läsbarhet

## Teknisk Implementation

### MudBlazor-komponenter Använda
- `MudDropContainer` & `MudDropZone`: För drag-and-drop
- `MudTimeline`: För veckovis visualisering
- `MudProgressLinear`: För tidsvalidering
- `MudCard` & `MudChip`: För visuell presentation
- `MudDialog`: För modala interaktioner

### Dataflöde
```
PatternOrExercise (Turbanken)
    ↓
LessonSection (Sektion med moment)
    ↓
Lesson (Komplett lektion)
    ↓
Course (Kurs med flera lektioner)
```

### State Management
- Blazor komponent state för UI-hantering
- Service layer för API-anrop
- Real-time uppdateringar via Blazor's change detection

## Nästa Steg och Framtida Förbättringar

### Fas 2 - Kommande Förbättringar
- [ ] AI-baserade rekommendationer för moment baserat på nivå och förkunskaper
- [ ] Automatisk täckningsanalys (visa vilka fundamentals som saknas)
- [ ] Spaced repetition-algoritm för optimal inlärning
- [ ] Exportfunktion till PDF/Markdown för lektionsplaner

### Fas 3 - Avancerade Funktioner
- [ ] Mallbibliotek för snabb kursuppbyggnad
- [ ] Teamsamarbete med kommentarer och delning
- [ ] Progressionsträd-visualisering
- [ ] Importfunktion för CSV/JSON-data

## Vanliga Användningsfall

### Skapa en 8-veckors Nybörjarkurs
1. Gå till Kurser → Skapa kursplan
2. Ange grundinfo: Namn, Nivå (Nybörjare), Dansstil (West Coast Swing)
3. Sätt antal veckor: 8, Antal lektioner: 8
4. Definiera mål och veckoteman
5. Klicka "Visa" för att öppna tidslinjen
6. För varje vecka, skapa en lektion med lektionsbyggaren
7. Lägg till moment från turbanken till varje lektion
8. Verifiera att tiden och progressionen är korrekt

### Redigera en Befintlig Lektion
1. Gå till Lektioner
2. Klicka "Edit" på lektionen du vill ändra
3. Använd drag-and-drop för att organisera om moment
4. Lägg till nya moment med knappen "Lägg till moment"
5. Justera allokerad tid per sektion
6. Spara ändringarna

## Felsökning

### Problem: Drag-and-drop fungerar inte
**Lösning**: Detta är normalt på touch-enheter. Använd "Lägg till moment"-knappen istället.

### Problem: Tidsindikatorn visar rött
**Lösning**: Lektionen är för lång. Ta bort moment eller öka lektionslängden i lektionsdetaljerna.

### Problem: Kan inte hitta ett moment
**Lösning**: Använd filtreringsalternativen (Nivå, Typ) och sökfältet för att begränsa resultaten.

## Support och Feedback

För frågor, bugrapporter eller funktionsförslag, skapa en issue på GitHub-projektet.

## Se Också
- [Analys-Danskursflode.md](./Analys-Danskursflode.md) - Detaljerad analys av kursflödet
- [Kravspecifikation.md](../Kravspecifikation.md) - Fullständiga funktionella krav
- [README.md](../README.md) - Projektöversikt och installation
