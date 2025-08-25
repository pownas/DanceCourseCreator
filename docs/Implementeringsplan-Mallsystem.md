# Implementeringsplan: Template System (Mallsystem)

## Översikt

Denna implementeringsplan strukturerar utvecklingsarbetet för kursmallssystemet i DanceCourseCreator. Planen baseras på den detaljerade funktionsspecifikationen i [Implementering-Mallsystem.md](./Implementering-Mallsystem.md) och de funktionella kraven FR-030, FR-031, och FR-032.

## Funktionella krav som implementeras

- **FR-030**: Skapa lektions- och kursmallar. Duplicera från mallar.
- **FR-031**: Versionera planer med historik och återställning.
- **FR-032**: Kommentera och föreslå ändringar (review-läge) i team.

## Huvudmoment och deluppgifter

### Fas 1: Grundläggande mallhantering (4-6 veckor)

#### 1.1 Backend API-utveckling
**Estimerad tid:** 3 veckor

**Deluppgifter:**
- [ ] **1.1.1** Implementera Template CRUD API-endpoints
  - `GET /api/templates` - Lista användarens mallar
  - `GET /api/templates/{id}` - Hämta specifik mall
  - `POST /api/templates` - Skapa ny mall
  - `PUT /api/templates/{id}` - Uppdatera mall
  - `DELETE /api/templates/{id}` - Ta bort mall
  - `GET /api/templates/team/{teamId}` - Hämta team-mallar

- [ ] **1.1.2** Implementera duplacerings-endpoint
  - `POST /api/templates/{id}/duplicate` - Duplicera mall till ny lektion/kurs
  - Hantera både lektions- och kursmallar
  - Säkerställ korruktionsfri kopiering

- [ ] **1.1.3** Databasintegration och migrationer
  - Skapa Entity Framework migrations för Template-tabellen
  - Konfigurera relationer till User och Team
  - Implementera DbContext-konfiguration

- [ ] **1.1.4** Validering och error handling
  - Input-validering för mallnamn, innehåll, och metadata
  - Felhantering för ogiltiga JSON-strukturer
  - Auktoriseringsvalidering (ägarskap och team-åtkomst)

**Beroenden:** User- och Team-modeller måste existera
**Leverans:** Fungerande REST API för mallhantering

#### 1.2 Datastrukturer och modeller
**Estimerad tid:** 1 vecka

**Deluppgifter:**
- [ ] **1.2.1** Validera och utöka Template-modellen
  - Säkerställ att modellen stödjer alla krav från specifikationen
  - Lägg till metadata-fält om nödvändigt
  - Definiera JSON-schema för Content-fältet

- [ ] **1.2.2** Skapa DTO-klasser för API
  - CreateTemplateRequest/Response
  - UpdateTemplateRequest/Response
  - TemplateListResponse med paginering
  - DuplicateTemplateRequest/Response

- [ ] **1.2.3** Implementera JSON-serialisering
  - Hantera lektionsmall JSON-struktur
  - Hantera kursmall JSON-struktur
  - Validering av JSON-innehåll mot schema

**Beroenden:** Inga
**Leverans:** Robusta datamodeller för mallsystemet

#### 1.3 Grundläggande frontend-integration
**Estimerad tid:** 2 veckor

**Deluppgifter:**
- [ ] **1.3.1** Skapa template service i Client
  - TypeScript-interfaces för Template-modellen
  - HTTP-klient för API-anrop
  - Error handling och loading states

- [ ] **1.3.2** Mall-lista komponent
  - Visa användarens mallar i tabellformat
  - Grundläggande sök- och filtreringsfunktioner
  - Actions för redigera, duplicera, ta bort

- [ ] **1.3.3** Skapa mall-dialog
  - "Spara som mall"-funktionalitet från lektions/kurs-editor
  - Formulär för mallnamn och beskrivning
  - Val av scope (lektion/kurs)

**Beroenden:** Grundläggande lektion/kurs-editor måste existera
**Leverans:** Grundläggande UI för mallhantering

### Fas 2: Avancerad mallhantering (3-4 veckor)

#### 2.1 Mallbibliotek och sökfunktioner
**Estimerad tid:** 2 veckor

**Deluppgifter:**
- [ ] **2.1.1** Utökad API för sökning och filtrering
  - `GET /api/templates/search?query=&level=&type=&owner=`
  - Textbaserad sökning i mallnamn och innehåll
  - Filtrering efter kategori, nivå, ägare
  - Sortering och paginering

- [ ] **2.1.2** Avancerat mallbibliotek-UI
  - Galleri-vy med förhandsvisning
  - Avancerade filter och sökfunktioner
  - Favoriter och "senast använda"
  - Statistik över användningsfrekvens

- [ ] **2.1.3** Kategorisering och taggning
  - Lägg till tags och kategorier till Template-modellen
  - UI för att hantera kategorier
  - Automatisk kategorisering baserat på innehåll

**Beroenden:** Fas 1 måste vara klar
**Leverans:** Komplett mallbibliotek med sök- och filterfunktioner

#### 2.2 Team-mallar och delning
**Estimerad tid:** 2 veckor

**Deluppgifter:**
- [ ] **2.2.1** Team-mallhantering API
  - Utöka endpoints för team-specifika mallar
  - Behörighetskontroll för team-åtkomst
  - API för att dela/sluta dela mallar med team

- [ ] **2.2.2** Team-mallar UI
  - Separata vyer för personliga vs team-mallar
  - Delningsfunktionalitet i UI
  - Team-medlemmar kan se men inte redigera

- [ ] **2.2.3** Säkerhet och behörigheter
  - Implementera role-based access control
  - Auditlogg för malländringar
  - Ägarskap och överföring av mallar

**Beroenden:** Team-system måste existera
**Leverans:** Fungerande team-delning av mallar

### Fas 3: Versionering och samarbete (3-4 veckor)

#### 3.1 Versionshantering (FR-031)
**Estimerad tid:** 2 veckor

**Deluppgifter:**
- [ ] **3.1.1** Version-datamodell
  - Skapa TemplateVersion-modell
  - Historik över malländringar
  - Metadata för vem, vad, när

- [ ] **3.1.2** Versionshantering API
  - `GET /api/templates/{id}/versions` - Hämta versionshistorik
  - `POST /api/templates/{id}/versions` - Skapa ny version
  - `POST /api/templates/{id}/restore/{versionId}` - Återställ version

- [ ] **3.1.3** Versionshantering UI
  - Versionshistorik-vy
  - Jämförelser mellan versioner
  - Återställningsfunktionalitet

**Beroenden:** Fas 1-2 måste vara klara
**Leverans:** Komplett versionhanteringssystem

#### 3.2 Kommentarer och review (FR-032)
**Estimerad tid:** 2 veckor

**Deluppgifter:**
- [ ] **3.2.1** Kommentarsystem-modeller
  - TemplateComment-modell
  - ReviewStatus för mallar
  - Notifieringssystem

- [ ] **3.2.2** Review-process API
  - `POST /api/templates/{id}/comments` - Lägg till kommentar
  - `POST /api/templates/{id}/review` - Skicka för granskning
  - `POST /api/templates/{id}/approve` - Godkänn mall

- [ ] **3.2.3** Samarbets-UI
  - Kommentarsystem i mallredigeraren
  - Review-workflow UI
  - Notifieringar för team-medlemmar

**Beroenden:** Team-system och notifieringar måste existera
**Leverans:** Komplett review- och kommentarsystem

### Fas 4: Integration och polish (2-3 veckor)

#### 4.1 Integration med huvudsystem
**Estimerad tid:** 1.5 veckor

**Deluppgifter:**
- [ ] **4.1.1** Integration med lektionsbyggare
  - "Skapa från mall"-knapp i lektionsbyggaren
  - Smidig övergång från mall till redigerbart innehåll
  - Bevara mallreferenser för spårbarhet

- [ ] **4.1.2** Integration med kursplanering
  - Stöd för kursmallar i kursbyggaren
  - Mallbaserad kursskapning
  - Progression och täckningsanalys för mallar

- [ ] **4.1.3** Export-integration
  - Mallar i PDF/Markdown-export
  - Import/export av mallar mellan system
  - Backup och återställning

**Beroenden:** Lektion- och kursbyggare måste existera
**Leverans:** Seamless integration med huvudfunktioner

#### 4.2 Performance och användbarhet
**Estimerad tid:** 1 vecka

**Deluppgifter:**
- [ ] **4.2.1** Performance-optimering
  - Caching av ofta använda mallar
  - Lazy loading för stora mallbibliotek
  - Optimera databas-queries

- [ ] **4.2.2** UX-förbättringar
  - Drag-and-drop för mallorganisation
  - Keyboard shortcuts
  - Tooltips och hjälptexter

- [ ] **4.2.3** Mobil-responsivitet
  - Mobilanpassat mallbibliotek
  - Touch-vänlig mallhantering
  - Offline-support för mallar

**Beroenden:** Alla tidigare faser
**Leverans:** Polerad och performant mallhantering

## Tekniska lösningar och integrationspunkter

### Databasdesign
```sql
-- Template-tabellen (redan implementerad)
Templates (Id, Scope, Name, Content, Owner, Team, CreatedAt, UpdatedAt)

-- Nya tabeller för versionering
TemplateVersions (Id, TemplateId, VersionNumber, Content, ChangedBy, ChangedAt, ChangeDescription)

-- Kommentarer
TemplateComments (Id, TemplateId, UserId, Comment, CreatedAt, IsResolved)

-- Favoriter och användningsstatistik
TemplateFavorites (UserId, TemplateId, AddedAt)
TemplateUsage (Id, TemplateId, UserId, UsedAt, Action)
```

### API-arkitektur
- RESTful API med konsekvent namngivning
- JWT-baserad auktorisering
- Input-validering med FluentValidation
- Swagger/OpenAPI-dokumentation
- Rate limiting för API-säkerhet

### Frontend-arkitektur
- Component-baserad arkitektur (React/Vue/Angular)
- State management för mallar (Redux/Vuex/NgRx)
- Real-time updates med SignalR för teamsamarbete
- Progressive Web App-funktioner

### Integrationspunkter
1. **User Management System** - För ägarskap och behörigheter
2. **Team System** - För team-mallar och delning
3. **Notification System** - För review-process och kommentarer
4. **Export System** - För PDF/Markdown-export av mallar
5. **Search Engine** - För avancerad mallsökning
6. **Audit System** - För spårning av malländringar

## Tidsplan och resurser

### Fas-baserad utveckling (Total: 12-17 veckor)

| Fas | Funktionalitet | Veckor | Beroenden |
|-----|----------------|--------|-----------|
| 1 | Grundläggande mallhantering | 4-6 | User/Team-modeller |
| 2 | Avancerad mallhantering | 3-4 | Fas 1 klar |
| 3 | Versionering och samarbete | 3-4 | Team-system |
| 4 | Integration och polish | 2-3 | Lektion/kurs-byggare |

### Resursallokering

**Backend-utvecklare (1 person):**
- API-utveckling och databasdesign
- Säkerhet och behörighetshantering
- Integration med befintliga system

**Frontend-utvecklare (1 person):**
- UI/UX-komponenter
- State management
- Användarinteraktion och workflows

**Fullstack-utvecklare (1 person):**
- Koordinering mellan frontend och backend
- Testing och kvalitetssäkring
- Performance-optimering

### Milestones och leveranser

**Milestone 1 (Vecka 6):** Grundläggande mallsystem
- Skapa, redigera, ta bort mallar
- Enkel duplicering från mallar
- Grundläggande UI för mallhantering

**Milestone 2 (Vecka 10):** Komplett mallbibliotek
- Avancerad sökning och filtrering
- Team-mallar och delning
- Kategorisering och taggning

**Milestone 3 (Vecka 14):** Samarbete och versionering
- Versionshistorik och återställning
- Kommentar- och review-system
- Notifieringar och team-samarbete

**Milestone 4 (Vecka 17):** Färdig integration
- Komplett integration med huvudsystem
- Performance-optimerat
- Produktionsklart

## Risker och utmaningar

### Tekniska risker
- **Databasprestanda** vid stora mallbibliotek
  - *Mitigation:* Indexering, caching, paginering
- **JSON-strukturkomplexitet** för kursmallar
  - *Mitigation:* Väl definierade schemas, validering
- **Concurrent editing** av mallar i team
  - *Mitigation:* Optimistic locking, konflikthantering

### Funktionella risker
- **Användaracceptans** för komplext UI
  - *Mitigation:* Iterativ UX-design, användartestning
- **Team-adoption** av mallsystemet
  - *Mitigation:* Onboarding, training, gradvis utbyggnad

### Projektrisker
- **Scope creep** från funktionsrika krav
  - *Mitigation:* Tydlig fas-indelning, prioritering
- **Beroenden** på andra systemkomponenter
  - *Mitigation:* Tidigt samarbete, mock-implementationer

## Kvalitetssäkring

### Testing-strategi
- **Enhetstester** för all business logic
- **Integrationstester** för API-endpoints
- **End-to-end-tester** för kritiska användarflöden
- **Performance-tester** för stora mallbibliotek

### Code review-process
- Alla ändringar genomgår peer review
- Automatiserad statisk kodanalys
- Säkerhetsgranskning för behörigheter

## Framtida utbyggnad

### Fas 5: Avancerade funktioner (framtida)
- Mall-hierarkier och arv
- Automatisk kvalitetskontroll
- Machine learning för mallförslag
- Community-mallar och delning
- Integration med externa bibliotek

### Skalbarhet
- Mikrotjänst-arkitektur för större team
- CDN för mall-assets
- Multi-tenant support för organisationer

## Slutsats

Denna implementeringsplan ger en strukturerad approach för att utveckla mallsystemet i DanceCourseCreator. Genom att dela upp arbetet i tydliga faser med konkreta leveranser kan teamet arbeta systematiskt mot målet att leverera ett komplett och användbart mallsystem som uppfyller alla funktionella krav.

Planen balanserar funktionell komplettering med teknisk kvalitet och ger flexibilitet för anpassningar under utvecklingsprocessen.