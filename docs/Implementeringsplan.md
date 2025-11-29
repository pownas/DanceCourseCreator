# Implementeringsplan för DanceCourseCreator

**Version**: 1.0  
**Datum**: 2025-11-21  
**Relaterat issue**: [Skapa en plan för implementering – bryt ner i konkreta issues]

## Översikt

Detta dokument innehåller en strukturerad plan för vidareutveckling av DanceCourseCreator-projektet. Planen bryts ner i konkreta, hanterbara issues som kan implementeras stegvis.

### Nulägesbedömning

**Vad som är implementerat** (✅):
- Grundläggande .NET 10 Blazor WebAssembly applikation
- Autentisering och JWT-baserad säkerhet
- Pattern & Exercise Library med CRUD-operationer
- Sök och filtrering av patterns/övningar
- Grundläggande Lesson Management
- Grundläggande Course Management
- SQLite databas med Entity Framework Core
- REST API med Swagger-dokumentation
- MudBlazor UI-komponenter
- End-to-end Playwright-tester
- Datamodeller för User, Team, Pattern/Exercise, Lesson, Course, Template

**Vad som saknas eller behöver förbättras**:
- Fullständig Template-funktionalitet (endast datamodell finns)
- Team Collaboration-funktioner (endast datamodell finns)
- Export-funktionalitet (PDF/Markdown/HTML)
- Delningslänkar och åtkomstkontroll
- Progression och rekommendationer
- Media och musikintegration
- Avancerad lektionsbyggare med dra-och-släpp
- Repetitionsplanering och coverage-metrics
- Import-funktionalitet
- Rapporter och insikter
- Internationalisering (i18n)
- WCAG-förbättringar

---

## Prioritering (MoSCoW)

### 🔴 MUST - Kritisk funktionalitet (Fas 1)

Dessa funktioner är absolut nödvändiga för att systemet ska vara användbart för instruktörer.

#### Issue 1: Fullständig Template-implementering
**Prioritet**: Highest  
**Estimat**: 5-8 dagar  
**Beroendet**: Inga  
**Krav**: FR-030

**Beskrivning**:
Implementera komplett template-system för lektioner och kurser enligt den befintliga datamodellen och dokumentationen i `docs/Implementering-Mallsystem.md`.

**Delleveranser**:
1. **API-endpoints för templates**
   - GET /api/templates (lista alla mallar användaren har åtkomst till)
   - GET /api/templates/{id} (hämta specifik mall)
   - POST /api/templates (skapa ny mall från lektion/kurs)
   - PUT /api/templates/{id} (uppdatera mall)
   - DELETE /api/templates/{id} (radera mall)
   - POST /api/templates/{id}/instantiate (skapa lektion/kurs från mall)

2. **Backend-logik**
   - TemplatesController implementation
   - TemplateService för affärslogik
   - Serialisering av Lesson/Course till Template.Content
   - Deserialisering och instantiering från Template

3. **Frontend-komponenter**
   - TemplatesList.razor - lista över tillgängliga mallar
   - TemplateEditor.razor - skapa/redigera mall
   - SaveAsTemplateDialog.razor - spara lektion/kurs som mall
   - CreateFromTemplateDialog.razor - välj och instansiera mall

4. **Tester**
   - Enhetstester för TemplateService
   - API-tester för alla endpoints
   - E2E-tester för template-workflow

**Acceptanskriterier**:
- [ ] Instruktör kan spara befintlig lektion som mall
- [ ] Instruktör kan spara befintlig kurs som mall
- [ ] Mallar visas i bibliotek med filter (Lesson/Course)
- [ ] Instruktör kan skapa ny lektion från mall
- [ ] Instruktör kan skapa ny kurs från mall
- [ ] Mallar kan redigeras och raderas
- [ ] Team-mallar är synliga för alla teammedlemmar
- [ ] Privata mallar är endast synliga för ägaren

---

#### Issue 2: Avancerad lektionsbyggare med tidsvalidering
**Prioritet**: High  
**Estimat**: 5-8 dagar  
**Beroendet**: Inga  
**Krav**: FR-010, FR-011, FR-012, FR-013, BR-001, BR-003

**Beskrivning**:
Förbättra lektionsbyggaren med strukturerade sektioner, tidsvalidering och realtidsvarningar.

**Delleveranser**:
1. **Sektionshantering**
   - Definiera section-typer: Uppvärmning, Teknik, Mönster, Kombination, Repetition, Socialdans
   - LessonSection-modell i databas
   - API för att hantera sections i lektioner

2. **Tidsberäkning och validering**
   - Automatisk summering av lektionslängd baserat på patterns estimerade tid
   - Real-time varningar när total tid överstiger målet
   - Visuell progress-indikator för tidsanvändning

3. **Moment och struktur**
   - Stöd för 3-8 moment per lektion med varningar utanför spann
   - Drag-and-drop mellan sektioner (kräver BlazorDragDrop eller liknande)
   - Ordna om patterns inom sektioner

4. **Frontend-komponenter**
   - LessonBuilder.razor - huvudkomponent med sektioner
   - LessonTimeline.razor - visuell tidslinje
   - SectionEditor.razor - redigera sektion
   - PatternSelector.razor - lägg till patterns i sektion

5. **Tester**
   - Tester för tidsberäkning
   - Valideringstester för 3-8 moments-regel
   - E2E-test för komplett lektionsbyggande

**Acceptanskriterier**:
- [ ] Lektion kan delas in i definierade sektioner
- [ ] Varje sektion visar estimerad tid och pattern-lista
- [ ] Total lektionslängd beräknas automatiskt
- [ ] Varning visas om total tid > 80 minuter (för 75-min lektion)
- [ ] Varning visas om < 3 eller > 8 moment
- [ ] Patterns kan dras mellan sektioner
- [ ] Sektioner kan kollapsas/expanderas för översikt

---

#### Issue 3: Kursplanering med progression och täckning
**Prioritet**: High  
**Estimat**: 5-7 dagar  
**Beroendet**: Issue 2 (Lektionsbyggare)  
**Krav**: FR-020, FR-021, FR-022, FR-023, BR-004

**Beskrivning**:
Implementera kursplanering med veckovis översikt, progression och täckning av fundamentals.

**Delleveranser**:
1. **Kursstruktur och metadata**
   - Vecka-för-vecka struktur i Course-modellen
   - Mål per kurs och per vecka
   - Nivå och tempo-definitioner

2. **Coverage metrics**
   - Spåra vilka fundamentals som täckts (Sugar Push, Whip, Connection, Anchor, Stretch, etc.)
   - Beräkna täckningsgrad per kurs
   - Identifiera saknade fundamentals

3. **Progressionsvalidering**
   - Kontrollera att förkunskaper är uppfyllda
   - Varning om fundamentals saknas efter vecka 3 (Beginner/Improver)
   - Repetitionsplanering (samma pattern max 2 gånger i rad)

4. **Visualisering**
   - CourseTimeline.razor - veckovis tidslinje
   - CoverageMatrix.razor - visa täckning av koncept
   - ProgressionValidator.razor - visa varningar och rekommendationer

5. **Tester**
   - Coverage calculation-tester
   - Progression validation-tester
   - E2E-test för kursplanering

**Acceptanskriterier**:
- [ ] Kurs kan skapas med 4-12 veckor
- [ ] Varje vecka kan ha mål och tema
- [ ] Lektioner kan kopplas till specifika veckor
- [ ] Coverage-översikt visar vilka fundamentals som täckts
- [ ] Varning visas om Whip saknas efter vecka 4 (Improver+)
- [ ] Varning visas om Sugar Push inte finns i vecka 1-2 (Beginner)
- [ ] Repetitionsvarning för samma pattern > 2 veckor i rad

---

#### Issue 4: Export-funktionalitet (PDF/Markdown)
**Prioritet**: High  
**Estimat**: 5-8 dagar  
**Beroendet**: Issue 2 (strukturerade lektioner)  
**Krav**: FR-050, FR-051, FR-052

**Beskrivning**:
Implementera export av lektioner och kurser till PDF och Markdown format.

**Delleveranser**:
1. **Markdown-export**
   - Service för att generera Markdown från Lesson
   - Service för att generera Markdown från Course
   - Templating med lesson/course-struktur enligt Kravspecifikation.md exempel
   - Download-funktionalitet i frontend

2. **PDF-export**
   - Integration med PDF-bibliotek (t.ex. QuestPDF eller SelectPdf)
   - Layout och styling för utskrift
   - Inkludera sektioner, patterns, timing, notes
   - Kompakt och utskriftsvänligt format

3. **Export-tjänst**
   - ExportService i backend
   - API-endpoints: POST /api/lessons/{id}/export och /api/courses/{id}/export
   - Query parameters för format (pdf/markdown/html)

4. **Frontend-komponenter**
   - ExportDialog.razor - välj format och alternativ
   - Preview-funktionalitet innan export
   - Download-hantering

5. **Tester**
   - Markdown generation-tester
   - PDF generation-tester
   - E2E-test för export-workflow

**Acceptanskriterier**:
- [ ] Lektion kan exporteras till Markdown med alla sektioner
- [ ] Lektion kan exporteras till PDF med formatering
- [ ] Kurs kan exporteras till Markdown med alla lektioner
- [ ] Kurs kan exporteras till PDF med veckovis struktur
- [ ] Exported PDF är utskriftsvänlig och läsbar
- [ ] Markdown följer template i Kravspecifikation.md
- [ ] Export inkluderar alla metadata (timing, notes, patterns)

---

### 🟡 SHOULD - Viktig funktionalitet (Fas 2)

Dessa funktioner ökar värdet betydligt och bör implementeras efter Must-features.

#### Issue 5: Delningslänkar och åtkomstkontroll
**Prioritet**: Medium  
**Estimat**: 4-6 dagar  
**Beroendet**: Issue 4 (Export)  
**Krav**: FR-051, NFR-003

**Beskrivning**:
Möjliggör delning av lektioner och kurser via genererade länkar med åtkomstkontroll.

**Delleveranser**:
1. **ShareLink-modell och databas**
   - ShareLink-entitet enligt Kravspecifikation.md datamodell
   - Token-generering och validering
   - Expiration-hantering

2. **API-endpoints**
   - POST /api/lessons/{id}/share - skapa delningslänk
   - POST /api/courses/{id}/share - skapa delningslänk
   - GET /api/share/{token} - hämta delat innehåll
   - DELETE /api/share/{token} - återkalla delning

3. **Åtkomstnivåer**
   - Public (öppet tillgängligt)
   - Private (kräver inloggning)
   - Token-baserad (enbart med länk)
   - Tidsbegränsning (expires after X days)

4. **Frontend-komponenter**
   - ShareDialog.razor - skapa och hantera delning
   - SharedView.razor - visa delat innehåll (read-only)
   - CopyLinkButton.razor - kopiera delningslänk

5. **Tester**
   - Token-generering och validering
   - Access control-tester
   - E2E-test för delning och åtkomst

**Acceptanskriterier**:
- [ ] Instruktör kan skapa delningslänk för lektion
- [ ] Instruktör kan välja åtkomstnivå (public/private)
- [ ] Länk kan sättas att upphöra efter X dagar
- [ ] Delad lektion visas read-only för mottagare
- [ ] Länk kan återkallas och blir då inaktiv
- [ ] Privata länkar kräver inloggning
- [ ] Public länkar är tillgängliga för alla

---

#### Issue 6: Team Collaboration - Kommentarer och granskningar
**Prioritet**: Medium  
**Estimat**: 6-8 dagar  
**Beroendet**: Team-struktur finns i datamodell  
**Krav**: FR-032, FR-070, FR-071, UC8

**Beskrivning**:
Implementera team-samarbetsfunktioner enligt `docs/Implementering-Teamsamarbete.md`.

**Delleveranser**:
1. **Kommentarssystem**
   - Comment-modell kopplad till Lesson/Course
   - API för CRUD-operationer på kommentarer
   - Threading av kommentarer (replies)

2. **Review-workflow**
   - Review-state: Draft, In Review, Approved, Rejected
   - Assigna reviewers till lesson/course
   - Notification-system för reviews

3. **Team-åtkomstkontroll**
   - Implementera TeamRole: Admin, Editor, Reader
   - Middleware för att validera team-access
   - Permission-checks i alla API-endpoints

4. **Frontend-komponenter**
   - CommentSection.razor - visa och lägg till kommentarer
   - ReviewPanel.razor - hantera review-state
   - TeamMembersList.razor - visa teammedlemmar
   - PermissionGuard.razor - conditional rendering baserat på roll

5. **Tester**
   - Permission-tester för olika roller
   - Comment CRUD-tester
   - Review workflow-tester
   - E2E-test för team-collaboration

**Acceptanskriterier**:
- [ ] Teammedlemmar kan kommentera på lektioner
- [ ] Kommentarer kan besvaras (threading)
- [ ] Lektion kan sättas i review-läge
- [ ] Reviewer kan godkänna eller avvisa
- [ ] Reader kan endast visa, inte redigera
- [ ] Editor kan redigera teamets innehåll
- [ ] Admin kan hantera teammedlemmar och roller

---

#### Issue 7: Rekommendationssystem för patterns
**Prioritet**: Medium  
**Estimat**: 5-7 dagar  
**Beroendet**: Issue 3 (Kursplanering)  
**Krav**: FR-013, FR-014, UC4

**Beskrivning**:
Implementera intelligent rekommendationssystem för patterns baserat på progression och kontext.

**Delleveranser**:
1. **Rekommendationsmotor**
   - RecommendationService som analyserar tidigare lektioner
   - Förkunskapsanalys (prerequisites fulfillment)
   - Nivåpassning baserat på kursnivå
   - Undvik repetitioner (samma pattern inom X veckor)

2. **Styrd slump-funktion**
   - Välj X patterns inom givna taggar/nivåer
   - Balansera mellan nya och kända patterns
   - Respektera repetitionsbegränsningar

3. **Progressionsanalys**
   - Identifiera nästa logiska steg i progressionsträd
   - Rekommendera variationer av kända patterns
   - Föreslå kompletterande övningar

4. **API-endpoints**
   - GET /api/recommendations/lesson - föreslå patterns för lektion
   - POST /api/recommendations/smart-fill - autofyll lektion med patterns
   - GET /api/recommendations/next-patterns - nästa steg i progression

5. **Frontend-komponenter**
   - RecommendationPanel.razor - visa förslag
   - SmartFillButton.razor - autofyll funktion
   - ProgressionTree.razor - visa progressionsträd

6. **Tester**
   - Recommendation algorithm-tester
   - Prerequisite validation-tester
   - E2E-test för rekommendationer

**Acceptanskriterier**:
- [ ] System föreslår 3+ patterns baserat på kursnivå
- [ ] Förslag respekterar förkunskaper
- [ ] Ingen pattern föreslås om den använts senaste 2 veckorna
- [ ] Styrd slump kan fylla en lektion med lämpliga patterns
- [ ] Progressionsträd visar nästa naturliga steg
- [ ] Rekommendationer balanserar nya och repetition

---

#### Issue 8: Media och musikintegration
**Prioritet**: Medium  
**Estimat**: 4-5 dagar  
**Beroendet**: Issue 2 (Lektionsbyggare)  
**Krav**: FR-040, FR-041

**Beskrivning**:
Integrera media-länkar och musikrekommendationer i patterns och lektioner.

**Delleveranser**:
1. **Media-länkar i patterns**
   - MediaLink-modell (URL, type, description)
   - Stöd för YouTube, Vimeo, Google Drive länkar
   - Validering av URL-format

2. **Musikrekommendationer**
   - MusicSuggestion-modell (song, artist, BPM, style, link)
   - Koppling till Pattern och Lesson
   - Spelliste-stöd (lista av songs)

3. **BPM-integration**
   - BPM-range per pattern (redan i modell)
   - Filtrera musik baserat på BPM
   - BPM-rekommendationer för lektion

4. **Frontend-komponenter**
   - MediaLinksPanel.razor - visa och hantera media
   - MusicSuggestions.razor - musikförslag
   - VideoEmbed.razor - inbäddad video (optional)
   - SpotifyPlaylistLink.razor - länka till playlists

5. **Tester**
   - URL-validering
   - BPM-filtering
   - E2E-test för media-hantering

**Acceptanskriterier**:
- [ ] Pattern kan ha flera media-länkar
- [ ] YouTube-länkar visas med preview (thumbnail)
- [ ] Lesson kan ha musikförslag med BPM
- [ ] BPM-range används för att filtrera musik
- [ ] Spelliste-länkar kan sparas per lektion
- [ ] Media-länkar visas i export (URL)

---

#### Issue 9: Import av patterns från CSV/JSON
**Prioritet**: Medium  
**Estimat**: 3-5 dagar  
**Beroendet**: Inga  
**Krav**: FR-060

**Beskrivning**:
Möjliggör import av patterns och övningar från standardformat.

**Delleveranser**:
1. **Import-format**
   - CSV-schema för patterns (name, level, type, description, etc.)
   - JSON-schema baserat på PatternOrExercise-modell
   - Markdown-format enligt Kravspec-exempel

2. **Import-service**
   - ImportService för parsing och validering
   - Duplicate-hantering (update eller skip)
   - Error reporting och validation

3. **API-endpoints**
   - POST /api/patterns/import - upload och importera fil
   - GET /api/patterns/import/template - hämta exempel-fil

4. **Frontend-komponenter**
   - ImportDialog.razor - välj fil och format
   - ImportPreview.razor - förhandsgranska före import
   - ImportResults.razor - visa resultat och fel

5. **Tester**
   - CSV parsing-tester
   - JSON parsing-tester
   - Validation-tester
   - E2E-test för import

**Acceptanskriterier**:
- [ ] CSV-fil med patterns kan importeras
- [ ] JSON-fil med patterns kan importeras
- [ ] Importförhandsgranskning visar vad som kommer skapas
- [ ] Duplicates hanteras (update eller skip)
- [ ] Valideringsfel rapporteras tydligt
- [ ] Importerad data sparas korrekt i databas

---

### 🟢 COULD - Värdefull funktionalitet (Fas 3)

Dessa funktioner förbättrar upplevelsen men är inte kritiska.

#### Issue 10: Rapporter och insikter
**Prioritet**: Low  
**Estimat**: 4-6 dagar  
**Beroendet**: Issue 3 (Kursplanering)  
**Krav**: FR-080, FR-081, FR-082

**Beskrivning**:
Skapa rapporter och dashboard för att ge instruktörer insikter om sina kurser.

**Delleveranser**:
1. **Täckningsrapporter**
   - Coverage per kurs (vilka patterns/concepts täckts)
   - Visualisering med charts (MudBlazor Charts)
   - Export av rapport till PDF

2. **Användningsstatistik**
   - Most used patterns
   - Patterns per level
   - Repetitionsfrekvens
   - Tidsanvändning per section-typ

3. **Kurssammanställning**
   - Automatisk generering av kursbeskrivning för annonsering
   - Innehållsöversikt
   - Nivå och mål

4. **Dashboard**
   - InstructorDashboard.razor - översikt
   - StatsPanel.razor - nyckeltal
   - RecentActivity.razor - senaste aktivitet

5. **Tester**
   - Statistics calculation-tester
   - Report generation-tester
   - E2E-test för dashboard

**Acceptanskriterier**:
- [ ] Dashboard visar antal patterns, lektioner, kurser
- [ ] Coverage-rapport visar täckning av fundamentals per kurs
- [ ] Usage statistics visar mest använda patterns
- [ ] Kurssammanställning genereras automatiskt
- [ ] Rapporter kan exporteras till PDF

---

#### Issue 11: Versionering och ändringshistorik
**Prioritet**: Low  
**Estimat**: 5-7 dagar  
**Beroendet**: Inga  
**Krav**: FR-031, NFR-008

**Beskrivning**:
Implementera versionhantering och ändringshistorik för lektioner och kurser.

**Delleveranser**:
1. **Version-modell**
   - Version-entitet med snapshot av innehåll
   - Automatic versioning vid större ändringar
   - Manual versioning (save as version X)

2. **Change tracking**
   - Audit log för alla ändringar
   - Vem, vad, när
   - Diff-visning mellan versioner

3. **Restore-funktionalitet**
   - Återställ till tidigare version
   - Preview före restore
   - Konfirmation och backup av nuvarande version

4. **API-endpoints**
   - GET /api/lessons/{id}/versions - lista versioner
   - GET /api/lessons/{id}/versions/{versionId} - hämta specifik version
   - POST /api/lessons/{id}/restore/{versionId} - återställ version

5. **Frontend-komponenter**
   - VersionHistory.razor - lista versioner
   - VersionDiff.razor - visa skillnader
   - RestoreDialog.razor - återställ version

6. **Tester**
   - Versioning logic-tester
   - Restore functionality-tester
   - E2E-test för version management

**Acceptanskriterier**:
- [ ] Ny version skapas automatiskt vid större ändringar
- [ ] Version kan skapas manuellt med kommentar
- [ ] Versionshistorik visar alla versioner med metadata
- [ ] Diff visar skillnader mellan versioner
- [ ] Tidigare version kan återställas
- [ ] Audit log spårar alla ändringar

---

#### Issue 12: Internationalisering (i18n) - Svenska/Engelska
**Prioritet**: Low  
**Estimat**: 4-6 dagar  
**Beroendet**: Inga  
**Krav**: NFR-006

**Beskrivning**:
Lägg till stöd för flera språk, initialt svenska och engelska.

**Delleveranser**:
1. **i18n-infrastruktur**
   - Integration med Blazor localization
   - Resource-filer för svenska (sv-SE)
   - Resource-filer för engelska (en-US)

2. **Översättning av UI**
   - Alla komponenter använder IStringLocalizer
   - Navigation, knappar, formulär
   - Felmeddelanden och validering

3. **Innehållsöversättning**
   - Pattern-namn och beskrivningar kan ha flera språk
   - Level-beteckningar (både svenska och engelska)
   - Export i valt språk

4. **Språkväxlare**
   - LanguageSwitcher.razor i navbar
   - Spara språkpreferens per användare
   - Cookie/localStorage för icke-inloggade

5. **Tester**
   - Localization-tester
   - Language switching-tester
   - E2E-test på båda språken

**Acceptanskriterier**:
- [ ] UI kan växlas mellan svenska och engelska
- [ ] Alla UI-texter översatta
- [ ] Språkval sparas per användare
- [ ] Pattern-innehåll kan ha översättningar
- [ ] Export respekterar valt språk
- [ ] Felmeddelanden visas på valt språk

---

#### Issue 13: WCAG-förbättringar för tillgänglighet
**Prioritet**: Low  
**Estimat**: 3-5 dagar  
**Beroendet**: Alla UI-komponenter  
**Krav**: NFR-002

**Beskrivning**:
Förbättra tillgänglighet för att uppnå WCAG 2.1 AA-nivå enligt `docs/WCAG-Compliance-Report.md`.

**Delleveranser**:
1. **Keyboard navigation**
   - Tab-ordning i alla formulär och dialoger
   - Keyboard shortcuts för vanliga åtgärder
   - Focus-indicators synliga och tydliga

2. **Screen reader-stöd**
   - ARIA-labels på alla interaktiva element
   - ARIA-live regions för dynamiskt innehåll
   - Semantisk HTML-struktur

3. **Visuell tillgänglighet**
   - Kontrastförhållanden minst 4.5:1
   - Textstorlek och spacing
   - Färgblindhet-vänliga färgval

4. **Formulär och validering**
   - Tydliga labels och placeholder-text
   - Inline error messages
   - Required fields markerade

5. **Tester**
   - Automated accessibility testing (axe-core)
   - Manual keyboard testing
   - Screen reader testing
   - Contrast validation

**Acceptanskriterier**:
- [ ] Alla formulär är keyboard-navigerbara
- [ ] Screen readers kan navigera hela appen
- [ ] Kontrastförhållanden uppfyller WCAG AA
- [ ] Focus indicators är synliga
- [ ] Felmeddelanden är associerade med input-fält
- [ ] Automated accessibility score > 90%

---

#### Issue 14: PWA och offline-funktionalitet
**Prioritet**: Low  
**Estimat**: 4-6 dagar  
**Beroendet**: Export (Issue 4)  
**Krav**: NFR-005

**Beskrivning**:
Konvertera applikationen till en Progressive Web App med offline-stöd.

**Delleveranser**:
1. **PWA-manifest**
   - manifest.json med app-metadata
   - Icons i olika storlekar
   - Installera-prompting

2. **Service Worker**
   - Cache-strategi för statiska assets
   - Offline-fallback
   - Background sync för ändringar

3. **Offline data access**
   - IndexedDB för lokal datalagring
   - Sync när online igen
   - Konflikthantering

4. **Offline-indikator**
   - Visa online/offline-status
   - Notifiera när förändringar synkas

5. **Tester**
   - Offline functionality-tester
   - Sync-tester
   - PWA compliance-tester

**Acceptanskriterier**:
- [ ] App kan installeras som PWA
- [ ] Pattern library kan läsas offline
- [ ] Lessons kan läsas offline
- [ ] Ändringar sparas lokalt och synkas vid online
- [ ] Offline-status visas tydligt
- [ ] Export fungerar offline för cachad data

---

### ⚪ WON'T - Inte i initial scope (Framtida faser)

Dessa funktioner ligger utanför den initiala scopet men kan övervägas längre fram.

#### Ej prioriterat nu:
- **Betalningar och fakturering** - Integration med betalsystem
- **Fullständigt LMS** - Quizzes, certifikat (bedömning i begränsad form planeras i Fas 4)
- **Native mobile apps** - iOS/Android-appar
- **Video upload och hosting** - Egen videoplattform
- **Live-videointegration** - Zoom/Teams-integration
- **Avancerad analytics** - Machine learning för pattern-rekommendationer
- **Multi-tenant SaaS** - Flera organisationer i samma instance

#### Flyttat till Fas 4 (tidigare WON'T):
- **Elevhantering** → Issue 16, 18 (Deltagarregistrering, närvaro)
- **Anmälningssystem** → Issue 16 (Inbjudningar och bokningsflöden)
- **Bedömning** → Issue 19 (Bedömningssystem - begränsad omfattning)

> **Not**: Se [Systemoversyn.md](./Systemoversyn.md) för detaljerad analys av nya krav.

---

## Implementeringsordning

### Fas 1: Grundläggande funktionalitet (8-12 veckor)
1. **Issue 1**: Template-system (5-8 dagar)
2. **Issue 2**: Avancerad lektionsbyggare (5-8 dagar)
3. **Issue 3**: Kursplanering med progression (5-7 dagar)
4. **Issue 4**: Export-funktionalitet (5-8 dagar)

**Mål**: Komplett grundsystem för lektions- och kursplanering

### Fas 2: Samarbete och förbättringar (6-10 veckor)
5. **Issue 5**: Delningslänkar (4-6 dagar)
6. **Issue 6**: Team Collaboration (6-8 dagar)
7. **Issue 7**: Rekommendationssystem (5-7 dagar)
8. **Issue 8**: Media och musik (4-5 dagar)
9. **Issue 9**: Import-funktionalitet (3-5 dagar)

**Mål**: Teamsamarbete och intelligent assistans

### Fas 3: Polish och optimering (4-8 veckor)
10. **Issue 10**: Rapporter och insikter (4-6 dagar)
11. **Issue 11**: Versionering (5-7 dagar)
12. **Issue 12**: Internationalisering (4-6 dagar)
13. **Issue 13**: WCAG-förbättringar (3-5 dagar)
14. **Issue 14**: PWA och offline (4-6 dagar)

**Mål**: Professionell, tillgänglig och robust produkt

### Fas 4: Deltagarhantering och schemaläggning (7-10 veckor)
15. **Issue 15**: Schemaläggning (5-7 dagar)
16. **Issue 16**: Inbjudningar och bokningsflöden (6-8 dagar)
17. **Issue 17**: Kalenderintegration (5-7 dagar)
18. **Issue 18**: Deltagarstatistik och närvaro (4-6 dagar)
19. **Issue 19**: Bedömningssystem (5-7 dagar)
20. **Issue 20**: Automatiserad kursplanering (8-12 dagar)

**Mål**: Fullständig hantering av deltagare, schemaläggning och uppföljning

> **Not**: Fas 4 är identifierad baserat på [Systemoversyn.md](./Systemoversyn.md) och kräver ytterligare detaljerad planering.

---

## Issue-templates

När nya issues skapas ska de följa denna struktur:

### Issue Template
```markdown
## Beskrivning
[Kortfattad beskrivning av funktionen]

## Funktionella krav
- FR-XXX: [Kravbeskrivning från Kravspecifikation.md]

## Delleveranser
- [ ] [Delkomponent 1]
- [ ] [Delkomponent 2]
- [ ] [Tester]

## Acceptanskriterier
- [ ] [Specifikt verifierbart kriterium]

## Beroenden
- [ ] Issue #X måste vara klar först

## Estimat
[X-Y dagar]

## Tekniska anteckningar
[Relevanta tekniska detaljer, bibliotek, arkitektur]

## Definition of Done
- [ ] Kod implementerad och testad
- [ ] Enhetstester skrivna och godkända
- [ ] E2E-tester uppdaterade
- [ ] Dokumentation uppdaterad
- [ ] Code review genomförd
- [ ] Ingen regressionstester failar
```

---

## Tekniska riktlinjer

### Kodstandard
- .NET 10 coding conventions
- Async/await för alla I/O-operationer
- Dependency injection för services
- Repository pattern för data access (valfritt)
- SOLID principles

### Testing
- Enhetstester med xUnit
- Integration tests för API
- E2E-tester med Playwright
- Minst 70% code coverage

### Performance
- API response time < 300ms (målsättning)
- Lazy loading av stora listor
- Pagination för alla list-endpoints
- Caching av statisk data

### Säkerhet
- Input validation på alla endpoints
- SQL injection prevention (EF Core parameterisering)
- XSS protection
- CSRF tokens
- Proper error handling (ej exponera stack traces)

---

## Relaterad dokumentation

- [Kravspecifikation](../Kravspecifikation.md) - Fullständig kravdokumentation
- [Systemoversyn](./Systemoversyn.md) - Systemöversyn med utökade krav (Fas 4)
- [Implementering: Mallsystem](./Implementering-Mallsystem.md) - Template system detaljer
- [Implementering: Teamsamarbete](./Implementering-Teamsamarbete.md) - Team collaboration detaljer
- [WCAG Compliance Report](./WCAG-Compliance-Report.md) - Tillgänglighetsstatus
- [Playwright Test Summary](./PLAYWRIGHT_IMPLEMENTATION_SUMMARY.md) - E2E-tester

---

## Bidrag och feedback

Denna plan är ett levande dokument och kommer uppdateras baserat på:
- Feedback från instruktörer och användare
- Tekniska utmaningar och upptäckter
- Prioritetsförändringar
- Nya möjligheter och integrationer

För varje nytt issue som skapas ska en referens till detta dokument inkluderas för kontext.

---

**Dokumentägare**: Utvecklingsteamet  
**Senast uppdaterad**: 2025-11-27  
**Nästa granskning**: Efter Fas 1 completion
