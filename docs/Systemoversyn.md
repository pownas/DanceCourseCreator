# Systemöversyn: DanceCourseCreator

**Version**: 1.0  
**Datum**: 2025-11-27  
**Relaterat issue**: Översyn av systemet och kravspecifikation för danskurs-/turapplikation

## 1. Sammanfattning

Detta dokument utgör en systematisk översyn av DanceCourseCreator-applikationen. Syftet är att:
1. Analysera nuvarande funktionalitet och flöden
2. Dokumentera styrkor och svagheter
3. Identifiera saknade eller förbättringsbehövande delar
4. Sammanställa en utökad kravspecifikation för framtida behov

---

## 2. Nuvarande funktioner och flöden

### 2.1 Implementerade funktioner

#### ✅ Autentisering och auktorisering
- **Status**: Fullständigt implementerad
- **Funktioner**:
  - JWT-baserad autentisering med BCrypt lösenordshashning
  - Användarregistrering och inloggning
  - Rollbaserad åtkomstkontroll (Instructor, Editor, Reader, Admin)
  - Skyddade routes och auktoriseringspolicies

#### ✅ Turbank (Patterns & Exercises)
- **Status**: Fullständigt implementerad
- **Funktioner**:
  - CRUD-operationer för West Coast Swing-mönster och övningar
  - Rik metadata: steg-för-steg instruktioner, undervisningspunkter, vanliga fel
  - BPM-intervall, timing, counts
  - Förkunskaper, relaterade mönster, taggar
  - Avancerad sökning och filtrering (typ, nivå, taggar, fritext)
  - Responsiv Material Design UI med MudBlazor

#### ✅ Lektionshantering
- **Status**: Grundläggande implementerad, avancerad under utveckling
- **Funktioner**:
  - Skapa och hantera individuella lektionsplaner
  - Strukturera lektioner med sektioner och tidsallokering
  - Länka mönster och övningar till lektioner
  - Drag-and-drop lektionsbyggare (LessonBuilderWithDragDrop.razor)
  - Tidsvalidering och varningar

#### ✅ Kurshantering
- **Status**: Grundläggande implementerad, avancerad under utveckling
- **Funktioner**:
  - Designa och hantera kurser över flera veckor
  - Spåra kursprogression och täckning
  - Koppla lektioner till kurser
  - Kursplaneringsverktyg (CourseTimelineBuilder.razor)
  - Progressionsvalidering och rekommendationer

#### ✅ Mallsystem (Templates)
- **Status**: Implementerad
- **Funktioner**:
  - Skapa mallar från befintliga lektioner och kurser
  - Lista och filtrera mallar
  - Instansiera nya lektioner/kurser från mallar
  - Duplicera mallar
  - Team- och användarbehörighet för mallar

#### ✅ Databas och API
- **Status**: Fullständigt implementerad
- **Teknisk stack**:
  - SQLite databas med Entity Framework Core
  - RESTful API med omfattande endpoints
  - Automatisk databasinitiering och seed-data
  - Health check och övervakningsendpoints
  - Swagger API-dokumentation

---

### 2.2 Flödesdiagram: Huvudanvändarflöden

```
┌──────────────────────────────────────────────────────────────────────────┐
│                           HUVUDFLÖDEN                                     │
├──────────────────────────────────────────────────────────────────────────┤
│                                                                          │
│  1. SKAPA KURS                                                           │
│     ┌──────┐    ┌──────────┐    ┌──────────┐    ┌──────────┐            │
│     │Login │ -> │Dashboard │ -> │Ny Kurs   │ -> │Planera   │            │
│     └──────┘    └──────────┘    │(metadata)│    │Veckor    │            │
│                                 └──────────┘    └──────────┘            │
│                                                       │                  │
│                                                       v                  │
│                                               ┌──────────┐              │
│                                               │Lägg till │              │
│                                               │Lektioner │              │
│                                               └──────────┘              │
│                                                                          │
│  2. SKAPA LEKTION                                                        │
│     ┌──────────┐    ┌───────────┐    ┌──────────┐    ┌────────┐        │
│     │Välj kurs │ -> │Ny Lektion │ -> │Lägg till │ -> │Validera│        │
│     │(valfritt)│    │(sektioner)│    │Mönster   │    │Tid     │        │
│     └──────────┘    └───────────┘    └──────────┘    └────────┘        │
│                                                                          │
│  3. HANTERA TURBANK                                                      │
│     ┌──────────┐    ┌──────────┐    ┌──────────┐                        │
│     │Turbank   │ -> │Sök/      │ -> │Visa/     │                        │
│     │          │    │Filtrera  │    │Redigera  │                        │
│     └──────────┘    └──────────┘    └──────────┘                        │
│                                                                          │
│  4. ANVÄND MALLAR                                                        │
│     ┌──────────┐    ┌──────────┐    ┌──────────┐                        │
│     │Mallar    │ -> │Välj mall │ -> │Skapa     │                        │
│     │          │    │          │    │instans   │                        │
│     └──────────┘    └──────────┘    └──────────┘                        │
│                                                                          │
└──────────────────────────────────────────────────────────────────────────┘
```

---

## 3. Styrkor

### 3.1 Tekniska styrkor

| Område | Styrka | Beskrivning |
|--------|--------|-------------|
| **Arkitektur** | Modern .NET 10 stack | Framtidssäker teknik med Blazor Server |
| **Skalbarhet** | Aspire-baserad hosting | Färdig för mikrotjänster och skalning |
| **UI/UX** | MudBlazor Material Design | Professionellt och responsivt gränssnitt |
| **API** | RESTful med Swagger | Väldokumenterat och testbart |
| **Testning** | Playwright E2E-tester | Omfattande end-to-end testtäckning |
| **Säkerhet** | JWT + BCrypt | Industristandarder för autentisering |

### 3.2 Funktionella styrkor

| Funktion | Styrka |
|----------|--------|
| **Turbank** | Rik metadata per mönster ger pedagogiskt värde |
| **Lektionsbyggare** | Drag-and-drop med tidsvalidering underlättar planering |
| **Progressionsvalidering** | Automatiska varningar säkerställer pedagogisk kvalitet |
| **Mallsystem** | Återanvändning sparar tid och ökar konsistens |
| **Responsiv design** | Fungerar på desktop, surfplatta och mobil |

### 3.3 Dokumentation

| Dokument | Kvalitet |
|----------|----------|
| `Kravspecifikation.md` | ⭐⭐⭐⭐⭐ Omfattande och välstrukturerad |
| `Implementeringsplan.md` | ⭐⭐⭐⭐⭐ Detaljerad med konkreta issues |
| `README.md` | ⭐⭐⭐⭐ Bra onboarding för utvecklare |
| API-dokumentation (Swagger) | ⭐⭐⭐⭐ Automatgenererad och interaktiv |

---

## 4. Svagheter och förbättringsområden

### 4.1 Tekniska svagheter

| Område | Svaghet | Prioritet | Rekommendation |
|--------|---------|-----------|----------------|
| **Databas** | SQLite begränsar skalbarhet | Medel | Migrera till PostgreSQL/SQL Server för produktion |
| **Caching** | Ingen cache-strategi | Medel | Implementera Redis eller in-memory cache |
| **Offline** | Ingen PWA-funktionalitet | Låg | Implementera service worker för offline-läsning |
| **Logging** | Begränsad strukturerad logging | Låg | Integrera Serilog med strukturerad logging |
| **Monitoring** | Grundläggande health checks | Låg | Utöka med Application Insights eller liknande |

### 4.2 Funktionella svagheter

| Funktion | Svaghet | Påverkan | Status |
|----------|---------|----------|--------|
| **Export** | Saknas (PDF/Markdown) | Hög | Planerad i Issue 4 |
| **Delning** | Inga delningslänkar | Medel | Planerad i Issue 5 |
| **Teamsamarbete** | Endast grundläggande | Medel | Planerad i Issue 6 |
| **Rapporter** | Saknas helt | Låg | Planerad i Issue 10 |
| **Versionering** | Ingen ändringshistorik | Låg | Planerad i Issue 11 |
| **Import** | Saknas (CSV/JSON) | Låg | Planerad i Issue 9 |
| **i18n** | Endast svenska | Låg | Planerad i Issue 12 |

### 4.3 Användarupplevelse

| Område | Svaghet | Rekommendation |
|--------|---------|----------------|
| **Onboarding** | Ingen guide för nya användare | Skapa "Kom igång"-wizard |
| **Hjälp** | Saknar kontextuell hjälp | Lägg till tooltips och hjälptexter |
| **Felhantering** | Generiska felmeddelanden | Förbättra med specifika, handlingsbara meddelanden |
| **Feedback** | Ingen användarfeedback-mekanism | Lägg till feedback-formulär |

---

## 5. Identifierade saknade funktioner

### 5.1 Från ursprunglig kravspecifikation (ej implementerade)

Följande funktioner finns i `Kravspecifikation.md` men är ännu ej implementerade:

| FR-kod | Funktion | Prioritet (MoSCoW) |
|--------|----------|-------------------|
| FR-050..052 | Export (PDF/Markdown/HTML) | **Must** |
| FR-040..041 | Media och musikintegration | Should |
| FR-060..061 | Import och API | Should |
| FR-070..071 | Team-behörighet | Should |
| FR-080..082 | Rapporter och insikter | Could |
| FR-031 | Versionering/historik | Could |

### 5.2 Nya funktioner (från issue-beskrivningen)

Följande funktioner efterfrågas i issue men saknas helt i nuvarande kravspecifikation:

| Funktion | Beskrivning | Rekommenderad prioritet |
|----------|-------------|------------------------|
| **Tidshantering/schema** | Schema för kurser och turer med specifika tider | Should |
| **Kalenderintegration** | Google Calendar, Outlook, iCal | Should |
| **Inbjudningar/bokning** | Bjud in deltagare, bokningsflöden | Should |
| **Bedömningssystem** | Prov och bedömning för deltagare | Could |
| **Deltagarstatistik** | Uppföljning på deltagare och kurser | Could |
| **Automatiserad planering** | AI-assisterad kursplanering | Could |

---

## 6. Utökad kravspecifikation

### 6.1 Nya funktionella krav (FR-100-serien)

#### FR-100: Tidshantering och schemaläggning

**FR-100**: Systemet ska stödja schemaläggning av kurser och lektioner med specifika datum och tider.
- **FR-101**: Varje kurs ska kunna ha ett schema med start- och slutdatum
- **FR-102**: Varje lektion ska kunna ha ett specifikt datum och tidpunkt
- **FR-103**: Systemet ska visa en kalendervy över planerade lektioner
- **FR-104**: Systemet ska varna vid schemakonflikter (överlappande lektioner)

**Datamodell (tillägg)**:
```csharp
public class Schedule
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [Required]
    public string CourseId { get; set; }
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string RecurrencePattern { get; set; } // Weekly, BiWeekly, etc.
    public TimeSpan DefaultStartTime { get; set; }
    public int DefaultDurationMinutes { get; set; }
    public string Location { get; set; }
    
    // Navigation properties
    public Course? Course { get; set; }
}
```

**Acceptanskriterier**:
- [ ] Instruktör kan sätta start- och slutdatum för kurs
- [ ] Lektioner kan tilldelas specifika datum och tider
- [ ] Kalendervy visar alla planerade lektioner
- [ ] Varning visas vid schemakonflikter

---

#### FR-110: Kalenderintegration

**FR-110**: Systemet ska kunna exportera och synkronisera scheman med externa kalendrar.
- **FR-111**: Export av kursschema till iCal-format (.ics)
- **FR-112**: Integration med Google Calendar (OAuth2)
- **FR-113**: Integration med Microsoft Outlook/365 (OAuth2)
- **FR-114**: Synkronisering av ändringar (tvåvägs)

**API-endpoints (förslag)**:
```
GET /api/courses/{id}/calendar/ical      - Exportera till iCal
POST /api/calendar/google/connect        - Anslut Google Calendar
POST /api/calendar/outlook/connect       - Anslut Outlook
PUT /api/calendar/sync                   - Synkronisera alla
```

**Acceptanskriterier**:
- [ ] Kurs kan exporteras till .ics-fil
- [ ] Användare kan ansluta Google Calendar
- [ ] Användare kan ansluta Outlook-kalender
- [ ] Ändringar i schema synkroniseras automatiskt

---

#### FR-120: Inbjudningar och bokningsflöden

**FR-120**: Systemet ska hantera inbjudningar till kurser och bokningsprocessen.
- **FR-121**: Skapa inbjudan för kurs med deltagarlänk
- **FR-122**: Registrering via inbjudningslänk (deltagare)
- **FR-123**: Väntelista för fullbokade kurser
- **FR-124**: E-postnotifikationer för bekräftelser
- **FR-125**: Påminnelser inför lektioner

**Datamodell (tillägg)**:
```csharp
public class CourseInvitation
{
    public string Id { get; set; }
    public string CourseId { get; set; }
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
    public int MaxParticipants { get; set; }
    public int CurrentParticipants { get; set; }
}

public class CourseRegistration
{
    public string Id { get; set; }
    public string CourseId { get; set; }
    public string ParticipantEmail { get; set; }
    public string ParticipantName { get; set; }
    public RegistrationStatus Status { get; set; }
    public DateTime RegisteredAt { get; set; }
}

public enum RegistrationStatus
{
    Registered,   // Bekräftad registrering
    Waitlist,     // På väntelista
    Cancelled,    // Avbokad
    Completed     // Genomfört kursen
}
```

**Acceptanskriterier**:
- [ ] Instruktör kan skapa inbjudningslänk för kurs
- [ ] Deltagare kan registrera sig via länk
- [ ] Väntelista aktiveras vid full kurs
- [ ] Bekräftelsemail skickas vid registrering
- [ ] Påminnelser skickas inför lektioner

---

#### FR-130: Bedömnings- och provsystem

**FR-130**: Systemet ska stödja bedömning och uppföljning av deltagare.
- **FR-131**: Definiera bedömningskriterier per nivå
- **FR-132**: Registrera deltagarutvärdering per lektion
- **FR-133**: Nivåtest/prov för certifiering
- **FR-134**: Progressionsspårning per deltagare
- **FR-135**: Rapporter över deltagarutveckling

**Datamodell (tillägg)**:
```csharp
public class AssessmentCriteria
{
    public string Id { get; set; }
    public DanceLevel Level { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> RequiredSkills { get; set; }
}

public class ParticipantAssessment
{
    public string Id { get; set; }
    public string ParticipantId { get; set; }
    public string CourseId { get; set; }
    public string LessonId { get; set; }
    public string AssessmentCriteriaId { get; set; }
    public int Score { get; set; } // 1-5 or similar
    public string Notes { get; set; }
    public DateTime AssessedAt { get; set; }
    public string AssessedBy { get; set; }
}
```

**Acceptanskriterier**:
- [ ] Bedömningskriterier kan definieras per nivå
- [ ] Instruktör kan bedöma deltagare per lektion
- [ ] Progressionsöversikt visar deltagarutveckling
- [ ] Rapporter kan genereras för enskilda deltagare

---

#### FR-140: Deltagarstatistik och uppföljning

**FR-140**: Systemet ska tillhandahålla statistik och insikter om deltagare.
- **FR-141**: Närvaroregistrering per lektion
- **FR-142**: Närvarostatistik per deltagare och kurs
- **FR-143**: Aggregerad kursstatistik (genomsnittsdeltagande, avhopp)
- **FR-144**: Export av deltagarlistor
- **FR-145**: Dashboard med nyckeltal

**API-endpoints (förslag)**:
```
GET /api/courses/{id}/attendance              - Hämta närvaro
POST /api/lessons/{id}/attendance            - Registrera närvaro
GET /api/statistics/courses/{id}             - Kursstatistik
GET /api/statistics/participants/{id}        - Deltagarstatistik
GET /api/statistics/dashboard                - Instruktörsdashboard
```

**Acceptanskriterier**:
- [ ] Närvaro kan registreras per lektion
- [ ] Statistik visar närvarograd per deltagare
- [ ] Kursstatistik visar aggregerade siffror
- [ ] Dashboard ger överblick över alla kurser

---

#### FR-150: Automatiserad kursplanering

**FR-150**: Systemet ska stödja automatiserad och AI-assisterad kursplanering.
- **FR-151**: Automatisk generering av kursschema baserat på parametrar
- **FR-152**: Intelligent tilldelning av mönster baserat på progression
- **FR-153**: Rekommendationer baserat på deltagarsammansättning
- **FR-154**: Optimering av lektionsinnehåll för lärandemål

**Algoritm-koncept**:
```
Input:
- Kursnivå (Beginner, Improver, etc.)
- Antal veckor
- Lektionslängd (60/75/90 min)
- Tillgängliga mönster i turbank

Output:
- Genererat kursschema med veckovis innehåll
- Progressionskarta
- Täckningsmatris för fundamentals
```

**Acceptanskriterier**:
- [ ] System kan generera kursförslag automatiskt
- [ ] Genererade kurser respekterar progressionsregler
- [ ] Användare kan justera och godkänna förslag
- [ ] Rekommendationer baseras på befintliga data

---

### 6.2 Nya icke-funktionella krav (NFR-100-serien)

| NFR-kod | Krav | Beskrivning |
|---------|------|-------------|
| NFR-101 | E-postintegration | Systemet ska kunna skicka e-post (SMTP/SendGrid) |
| NFR-102 | Notifikationer | Stöd för push-notifikationer (webb/mobil) |
| NFR-103 | Skalbarhet deltagare | Stöd för minst 500 deltagare per organisation |
| NFR-104 | GDPR-compliance | Deltagardata ska kunna raderas på begäran |
| NFR-105 | Backup/Restore | Automatisk backup av deltagardata dagligen |

---

### 6.3 Prioritering av nya krav (MoSCoW)

| Prioritet | Krav | Motivering |
|-----------|------|------------|
| **Must** | FR-100 (Schemaläggning) | Grundläggande för kursadministration |
| **Must** | FR-120 (Inbjudningar) | Nödvändigt för deltagarhantering |
| **Should** | FR-110 (Kalenderintegration) | Stor användarvärde |
| **Should** | FR-140 (Statistik) | Viktigt för uppföljning |
| **Could** | FR-130 (Bedömning) | Värdefull men ej kritisk |
| **Could** | FR-150 (Automatisering) | Innovation, senare fas |

---

## 7. Rekommenderad implementeringsordning

### 7.1 Utökad fasindelning

**Befintlig plan** (från Implementeringsplan.md):
- Fas 1: Must-funktioner (Issue 1-4)
- Fas 2: Should-funktioner (Issue 5-9)
- Fas 3: Could-funktioner (Issue 10-14)

**Ny fas 4: Deltagarhantering**

| Issue | Funktion | Estimat | Beroende av |
|-------|----------|---------|-------------|
| 15 | Schemaläggning (FR-100) | 5-7 dagar | Fas 1 |
| 16 | Inbjudningar/Bokning (FR-120) | 6-8 dagar | Issue 15 |
| 17 | Kalenderintegration (FR-110) | 5-7 dagar | Issue 15 |
| 18 | Deltagarstatistik (FR-140) | 4-6 dagar | Issue 16 |
| 19 | Bedömningssystem (FR-130) | 5-7 dagar | Issue 18 |
| 20 | Automatiserad planering (FR-150) | 8-12 dagar | Fas 3 |

**Total estimat Fas 4**: 33-47 dagar (7-10 veckor)

---

## 8. Riskanalys

### 8.1 Identifierade risker

| Risk | Sannolikhet | Konsekvens | Åtgärd |
|------|-------------|------------|--------|
| Scope creep med deltagarfunktioner | Hög | Hög | Strikt MoSCoW-prioritering |
| Kalenderintegration komplexitet | Medel | Medel | Börja med iCal-export endast |
| E-postleverans problem | Medel | Hög | Använd etablerad tjänst (SendGrid) |
| GDPR-compliance för deltagardata | Medel | Hög | Designa in från start |
| Performance med många deltagare | Låg | Medel | Tidig lasttest, optimering |

### 8.2 Antaganden

1. Deltagarhantering kan begränsas till registrering och närvaro initialt
2. Betalningsintegration hålls utanför scope (extern hantering)
3. Kalenderintegration kan börja med envägs-export
4. E-postfunktionalitet kan använda tredjepartstjänst

---

## 9. Slutsats och rekommendationer

### 9.1 Sammanfattning

DanceCourseCreator är en välstrukturerad applikation med solid teknisk grund. De befintliga implementeringsplanerna täcker de flesta kursskapande-behov, men saknar hantering av deltagare och schemaläggning som efterfrågas i issue-beskrivningen.

### 9.2 Primära rekommendationer

1. **Slutför Fas 1-3 först** - De befintliga planerade funktionerna är väl genomarbetade
2. **Lägg till Fas 4** - Deltagarhantering som ny fas efter Fas 3
3. **Börja med schemaläggning** - Grundläggande för alla deltagarfunktioner
4. **Iterativ approach** - Implementera MVP för varje funktion, iterera baserat på feedback

### 9.3 Nästa steg

1. [ ] Granska och godkänna denna systemöversyn
2. [ ] Uppdatera Implementeringsplan.md med Fas 4
3. [ ] Skapa GitHub issues för Issue 15-20
4. [ ] Samla in feedback från slutanvändare
5. [ ] Prioritera baserat på användarfeedback

---

## 10. Appendix

### A. Relaterad dokumentation

- [Kravspecifikation.md](../Kravspecifikation.md) - Ursprunglig kravspec
- [Implementeringsplan.md](./Implementeringsplan.md) - Detaljerad implementeringsplan
- [Issues-Oversikt.md](./Issues-Oversikt.md) - Översikt befintliga issues
- [SNABBSTART.md](./SNABBSTART.md) - Guide för olika roller

### B. Teknisk arkitektur (nuvarande)

```
┌────────────────────────────────────────────────────────────────┐
│                        FRONTEND                                 │
│   ┌─────────────────────────────────────────────────────────┐  │
│   │           Blazor Server (.NET 10)                       │  │
│   │   ┌─────────┐ ┌─────────┐ ┌─────────┐ ┌─────────┐      │  │
│   │   │ Pages   │ │Components│ │ Services│ │ Models  │      │  │
│   │   └─────────┘ └─────────┘ └─────────┘ └─────────┘      │  │
│   └─────────────────────────────────────────────────────────┘  │
│                              │                                  │
│                              │ SignalR + HTTP/REST             │
│                              v                                  │
│   ┌─────────────────────────────────────────────────────────┐  │
│   │              ASP.NET Web API (.NET 10)                  │  │
│   │   ┌───────────┐ ┌─────────┐ ┌─────────┐ ┌─────────┐    │  │
│   │   │Controllers│ │ Services│ │  DTOs   │ │ Models  │    │  │
│   │   └───────────┘ └─────────┘ └─────────┘ └─────────┘    │  │
│   └─────────────────────────────────────────────────────────┘  │
│                              │                                  │
│                              │ Entity Framework Core            │
│                              v                                  │
│   ┌─────────────────────────────────────────────────────────┐  │
│   │                     SQLite Database                      │  │
│   │   Users | Teams | Patterns | Lessons | Courses | Templates│  │
│   └─────────────────────────────────────────────────────────┘  │
│                                                                 │
└────────────────────────────────────────────────────────────────┘
```

### C. Datamodell-förändringar (sammanfattning)

Nya entiteter som föreslås för Fas 4:
- `Schedule` - Schemaläggning av kurser
- `CourseInvitation` - Inbjudningar till kurser
- `CourseRegistration` - Deltagarregistreringar
- `Attendance` - Närvaroregistrering
- `AssessmentCriteria` - Bedömningskriterier
- `ParticipantAssessment` - Deltagarutvärdering

---

**Dokumentägare**: Utvecklingsteamet  
**Senast uppdaterad**: 2025-11-27  
**Nästa granskning**: Vid projektplanering för Fas 4
