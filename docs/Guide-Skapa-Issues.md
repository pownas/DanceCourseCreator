# Guide: Hur man skapar GitHub Issues från Implementeringsplanen

Detta dokument beskriver hur varje planerat issue i [Implementeringsplan.md](./Implementeringsplan.md) ska skapas som ett GitHub Issue.

---

## 📝 Issue Template

Använd denna mall när du skapar varje issue i GitHub:

```markdown
## 📋 Beskrivning
[Kopiera beskrivningen från Implementeringsplan.md för detta issue]

## 🎯 Funktionella krav
[Lista relevanta FR-krav från Kravspecifikation.md]
- FR-XXX: [Beskrivning]

## 📦 Delleveranser
- [ ] [Komponent 1]
- [ ] [Komponent 2]
- [ ] [Komponent 3]
- [ ] Enhetstester
- [ ] E2E-tester
- [ ] Dokumentation

## ✅ Acceptanskriterier
[Kopiera alla acceptanskriterier från Implementeringsplan.md]
- [ ] [Kriterium 1]
- [ ] [Kriterium 2]

## 🔗 Beroenden
[Lista beroende issues]
- Blockerande: #[issue-nummer] måste vara klar först
- Relaterat: #[issue-nummer] kan göras parallellt

## ⏱️ Estimat
[X-Y dagar] utvecklingstid + test och review

## 🛠️ Tekniska anteckningar
[Specifika tekniska detaljer från Implementeringsplan.md]

**Bibliotek/verktyg:**
- [Lista relevanta NuGet-paket eller tekniker]

**Arkitektur:**
- [Vilka lager påverkas: API, Web, Models, Services]

## 📚 Relaterad dokumentation
- [Implementeringsplan](docs/Implementeringsplan.md) - Detaljerad plan för detta issue
- [Kravspecifikation](Kravspecifikation.md) - Funktionella krav
- [Specifik implementeringsguide om sådan finns]

## ✓ Definition of Done
- [ ] Kod implementerad enligt spec
- [ ] Enhetstester skrivna och passar (>70% coverage för nya components)
- [ ] E2E-tester uppdaterade/tillagda
- [ ] Ingen regression i befintliga tester
- [ ] Code review genomförd och godkänd
- [ ] Dokumentation uppdaterad (README, API docs, etc.)
- [ ] Merge request skapad och mergad till main
```

---

## 🏷️ Labels att använda

Varje issue ska ha följande labels:

### Prioritet (välj en):
- `priority: critical` - MUST (Fas 1)
- `priority: high` - SHOULD (Fas 2)
- `priority: medium` - COULD (Fas 3)
- `priority: low` - Framtida förbättringar

### Typ (välj en eller flera):
- `type: feature` - Ny funktionalitet
- `type: enhancement` - Förbättring av befintlig feature
- `type: bug` - Buggfix (om upptäckt under implementation)

### Område (välj en eller flera):
- `area: api` - Backend API-ändringar
- `area: frontend` - Blazor UI-ändringar
- `area: database` - Datamodell eller migrations
- `area: testing` - Test-relaterat
- `area: documentation` - Dokumentationsändringar

### Status (sätts automatiskt av projekthantering):
- `status: planned` - Planerat, ej påbörjat
- `status: ready` - Redo att starta (alla beroenden klara)
- `status: in-progress` - Aktivt arbete pågår
- `status: review` - Under code review
- `status: testing` - Under test
- `status: blocked` - Blockerad av annat issue

### Fas:
- `phase-1: mvp` - Fas 1 issues
- `phase-2: collaboration` - Fas 2 issues
- `phase-3: polish` - Fas 3 issues

---

## 📊 Projects och Milestones

### GitHub Project Board
Skapa ett GitHub Project med kolumner:
1. **📋 Backlog** - Alla planerade issues
2. **🚀 Ready** - Beroenden uppfyllda, kan startas
3. **👷 In Progress** - Aktivt arbete
4. **🔍 Review** - Code review eller testing
5. **✅ Done** - Completed och mergat

### Milestones
Skapa tre milestones:

#### Milestone 1: MVP - Komplett kursskapande
- **Due date**: 12 veckor från start
- **Issues**: #1, #2, #3, #4
- **Description**: "Instruktörer kan skapa, strukturera och exportera kurser"

#### Milestone 2: Team-funktionalitet
- **Due date**: 24 veckor från start
- **Issues**: #5, #6, #7, #8, #9
- **Description**: "Flera instruktörer kan samarbeta effektivt"

#### Milestone 3: Production-ready
- **Due date**: 32 veckor från start
- **Issues**: #10, #11, #12, #13, #14
- **Description**: "Professionell, tillgänglig produkt redo för release"

---

## 🔢 Issue-numrering och titlar

### Issue 1: Fullständig Template-implementering
**Title**: `[Fas 1] Fullständig Template-implementering`  
**Labels**: `priority: critical`, `type: feature`, `area: api`, `area: frontend`, `phase-1: mvp`  
**Milestone**: MVP - Komplett kursskapande

### Issue 2: Avancerad lektionsbyggare med tidsvalidering
**Title**: `[Fas 1] Avancerad lektionsbyggare med tidsvalidering`  
**Labels**: `priority: critical`, `type: feature`, `area: frontend`, `phase-1: mvp`  
**Milestone**: MVP - Komplett kursskapande

### Issue 3: Kursplanering med progression och täckning
**Title**: `[Fas 1] Kursplanering med progression och täckning`  
**Labels**: `priority: critical`, `type: feature`, `area: api`, `area: frontend`, `phase-1: mvp`  
**Milestone**: MVP - Komplett kursskapande  
**Dependencies**: Issue #2

### Issue 4: Export-funktionalitet (PDF/Markdown)
**Title**: `[Fas 1] Export-funktionalitet (PDF/Markdown)`  
**Labels**: `priority: critical`, `type: feature`, `area: api`, `phase-1: mvp`  
**Milestone**: MVP - Komplett kursskapande  
**Dependencies**: Issue #2

### Issue 5: Delningslänkar och åtkomstkontroll
**Title**: `[Fas 2] Delningslänkar och åtkomstkontroll`  
**Labels**: `priority: high`, `type: feature`, `area: api`, `area: database`, `phase-2: collaboration`  
**Milestone**: Team-funktionalitet  
**Dependencies**: Issue #4

### Issue 6: Team Collaboration - Kommentarer och granskningar
**Title**: `[Fas 2] Team Collaboration - Kommentarer och granskningar`  
**Labels**: `priority: high`, `type: feature`, `area: api`, `area: frontend`, `phase-2: collaboration`  
**Milestone**: Team-funktionalitet

### Issue 7: Rekommendationssystem för patterns
**Title**: `[Fas 2] Rekommendationssystem för patterns`  
**Labels**: `priority: high`, `type: feature`, `area: api`, `phase-2: collaboration`  
**Milestone**: Team-funktionalitet  
**Dependencies**: Issue #3

### Issue 8: Media och musikintegration
**Title**: `[Fas 2] Media och musikintegration`  
**Labels**: `priority: high`, `type: feature`, `area: api`, `area: frontend`, `phase-2: collaboration`  
**Milestone**: Team-funktionalitet  
**Dependencies**: Issue #2

### Issue 9: Import av patterns från CSV/JSON
**Title**: `[Fas 2] Import av patterns från CSV/JSON`  
**Labels**: `priority: high`, `type: feature`, `area: api`, `phase-2: collaboration`  
**Milestone**: Team-funktionalitet

### Issue 10: Rapporter och insikter
**Title**: `[Fas 3] Rapporter och insikter`  
**Labels**: `priority: medium`, `type: feature`, `area: api`, `area: frontend`, `phase-3: polish`  
**Milestone**: Production-ready  
**Dependencies**: Issue #3

### Issue 11: Versionering och ändringshistorik
**Title**: `[Fas 3] Versionering och ändringshistorik`  
**Labels**: `priority: medium`, `type: feature`, `area: api`, `area: database`, `phase-3: polish`  
**Milestone**: Production-ready

### Issue 12: Internationalisering (i18n) - Svenska/Engelska
**Title**: `[Fas 3] Internationalisering (i18n) - Svenska/Engelska`  
**Labels**: `priority: medium`, `type: enhancement`, `area: frontend`, `phase-3: polish`  
**Milestone**: Production-ready

### Issue 13: WCAG-förbättringar för tillgänglighet
**Title**: `[Fas 3] WCAG-förbättringar för tillgänglighet`  
**Labels**: `priority: medium`, `type: enhancement`, `area: frontend`, `phase-3: polish`  
**Milestone**: Production-ready

### Issue 14: PWA och offline-funktionalitet
**Title**: `[Fas 3] PWA och offline-funktionalitet`  
**Labels**: `priority: medium`, `type: feature`, `area: frontend`, `phase-3: polish`  
**Milestone**: Production-ready

---

## 🔄 Workflow för att skapa issues

### Steg 1: Förberedelse
1. Läs [Implementeringsplan.md](./Implementeringsplan.md) noggrant
2. Identifiera vilken fas du vill börja med (rekommenderat: Fas 1)
3. Kontrollera att GitHub Project och Milestones är uppsatta

### Steg 2: Skapa issue
1. Gå till GitHub Issues → New Issue
2. Använd issue-template ovan
3. Kopiera relevant information från Implementeringsplan.md
4. Sätt korrekt titel enligt formatet `[Fas X] Issue-titel`
5. Lägg till alla relevanta labels
6. Välj rätt milestone
7. Om issue har beroenden, länka till dessa med "Depends on #X"

### Steg 3: Länka issues
1. I varje issue, lägg till länkar under "Related issues"
2. Använd GitHub keywords: "Depends on #X", "Blocks #Y", "Related to #Z"
3. Detta skapar automatiska länkar mellan issues

### Steg 4: Uppdatera Project Board
1. Lägg till issue i rätt kolumn (troligen "Backlog")
2. Om alla beroenden är klara, flytta till "Ready"

### Steg 5: Referera till planen
I varje issue, lägg till denna text i slutet av beskrivningen:
```markdown
---
**📋 Del av implementeringsplan**: Se [Implementeringsplan.md](docs/Implementeringsplan.md) för fullständig kontext och [Issues-Översikt.md](docs/Issues-Oversikt.md) för hela issue-listan.

**Refererar till originalärende**: #[issue-nummer för denna planeringsissue]
```

---

## 📌 Best Practices

### När du skapar issues:
- ✅ **Var specifik** - Kopiera exakt text från Implementeringsplan.md
- ✅ **Inkludera alla acceptanskriterier** - Dessa blir checkboxar i issue
- ✅ **Länka beroenden** - Använd "Depends on #X" syntax
- ✅ **Sätt rätt labels** - Hjälper med filtrering och prioritering
- ✅ **Estimera realistiskt** - Använd estimaten från planen

### Under implementation:
- ✅ **Uppdatera checkboxar** - Kryssa i delleveranser när de är klara
- ✅ **Kommentera progress** - Lägg till kommentarer vid viktiga milstolpar
- ✅ **Länka commits** - Referera till issue i commit messages (`#issue-nummer`)
- ✅ **Uppdatera status** - Flytta issue i Project Board när status ändras

### Vid completion:
- ✅ **Verifiera DoD** - Alla Definition of Done-punkter måste vara klara
- ✅ **Länka PR** - Pull Request ska referera till issue
- ✅ **Uppdatera dokumentation** - README och andra docs vid behov
- ✅ **Stäng med message** - "Closes #X" i PR-beskrivning

---

## 🎯 Exempel: Komplett Issue Creation

### Exempel för Issue 1 (Template-system)

**GitHub Issue:**

```markdown
## 📋 Beskrivning
Implementera komplett template-system för lektioner och kurser enligt den befintliga datamodellen och dokumentationen i `docs/Implementering-Mallsystem.md`.

Template-systemet möjliggör för instruktörer att:
- Spara befintliga lektioner och kurser som återanvändbara mallar
- Skapa nya lektioner/kurser från mallar för konsistent struktur
- Dela mallar inom team för effektivt samarbete
- Hantera och organisera mallbibliotek

## 🎯 Funktionella krav
- **FR-030**: Skapa lektions- och kursmallar. Duplicera från mallar.

Från Kravspecifikation.md sektion 7.4

## 📦 Delleveranser
- [ ] API-endpoints för templates (GET, POST, PUT, DELETE, instantiate)
- [ ] Backend-logik: TemplatesController och TemplateService
- [ ] Serialisering/deserialisering av Lesson/Course till Template.Content
- [ ] Frontend-komponenter: TemplatesList, TemplateEditor, SaveAsTemplateDialog, CreateFromTemplateDialog
- [ ] Enhetstester för TemplateService
- [ ] API-tester för alla endpoints
- [ ] E2E-tester för template-workflow
- [ ] Dokumentation uppdaterad

## ✅ Acceptanskriterier
- [ ] Instruktör kan spara befintlig lektion som mall
- [ ] Instruktör kan spara befintlig kurs som mall
- [ ] Mallar visas i bibliotek med filter (Lesson/Course)
- [ ] Instruktör kan skapa ny lektion från mall
- [ ] Instruktör kan skapa ny kurs från mall
- [ ] Mallar kan redigeras och raderas
- [ ] Team-mallar är synliga för alla teammedlemmar
- [ ] Privata mallar är endast synliga för ägaren

## 🔗 Beroenden
Inga blockerande beroenden. Detta issue kan startas direkt.

## ⏱️ Estimat
5-8 dagar utvecklingstid + 2-3 dagar test och review = **7-11 dagar total**

## 🛠️ Tekniska anteckningar

**Datamodell finns redan:**
- `Template` entity i `Models/Template.cs`
- Befintliga `Lesson` och `Course` modeller att serialisera

**Behöver implementera:**
- `TemplatesController` i API
- `TemplateService` för affärslogik
- JSON serialisering för `Template.Content`
- Blazor-komponenter för UI

**Bibliotek/verktyg:**
- System.Text.Json för serialisering
- MudBlazor komponenter för UI
- Existing API patterns för controllers

**Arkitektur:**
- API Layer: New controller + service
- Web Layer: New pages/components + service
- Database: Modellen finns redan

## 📚 Relaterad dokumentation
- [Implementeringsplan](docs/Implementeringsplan.md) - Issue 1 detaljer
- [Implementering: Mallsystem](docs/Implementering-Mallsystem.md) - Detaljerad spec
- [Kravspecifikation](Kravspecifikation.md) - FR-030

## ✓ Definition of Done
- [ ] Kod implementerad enligt spec
- [ ] Enhetstester skrivna och passar (>70% coverage)
- [ ] API-tester för alla endpoints
- [ ] E2E-tester för create → save as template → instantiate workflow
- [ ] Ingen regression i befintliga tester
- [ ] Code review genomförd och godkänd
- [ ] README uppdaterad med template-funktionalitet
- [ ] API docs (Swagger) uppdaterade
- [ ] Pull request mergad till main

---
**📋 Del av implementeringsplan**: Se [Implementeringsplan.md](docs/Implementeringsplan.md) för fullständig kontext och [Issues-Översikt.md](docs/Issues-Oversikt.md) för hela issue-listan.

**Refererar till originalärende**: #[nummer för planeringsissuet]
```

**Labels:** `priority: critical`, `type: feature`, `area: api`, `area: frontend`, `area: database`, `phase-1: mvp`  
**Milestone:** MVP - Komplett kursskapande  
**Assignees:** [Utvecklare som ska göra detta]  
**Project:** DanceCourseCreator Roadmap → Backlog

---

## 📞 Support och frågor

Om du har frågor om hur issues ska skapas:
1. Läs [Implementeringsplan.md](./Implementeringsplan.md) för tekniska detaljer
2. Konsultera [Issues-Översikt.md](./Issues-Oversikt.md) för beroenden
3. Skapa en diskussion i GitHub Discussions om oklarheter
4. Referera alltid tillbaka till Kravspecifikation.md för kontext

---

**Senast uppdaterad**: 2025-11-21  
**Version**: 1.0
