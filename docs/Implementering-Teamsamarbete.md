# Implementering: Team Collaboration (Teamsamarbete)

## Översikt

Team Collaboration-systemet i DanceCourseCreator möjliggör effektivt samarbete mellan instruktörer inom team och organisationer. Systemet stöder delning av kursmaterial, kvalitetssäkring genom granskning och strukturerad åtkomstkontroll.

## Funktionella krav som implementeras

- **FR-070**: Organisations-/teamstruktur; dela bibliotek och mallar inom team.
- **FR-071**: Åtkomstnivåer: ägare, redaktör, läsare.
- **FR-032**: Kommentera och föreslå ändringar (review-läge) i team.
- **UC8**: Team-samarbete: kommentera, föreslå ändringar, godkänna.

## Datamodell

### Team-modellen

```csharp
public class Team
{
    public string Id { get; set; }                    // Unik identifierare
    public string Name { get; set; }                  // Team-namn
    public DateTime CreatedAt { get; set; }           // Skapad tidpunkt
    public DateTime UpdatedAt { get; set; }           // Senast uppdaterad
    
    // Navigation properties
    public ICollection<User> Members { get; set; }    // Team-medlemmar
}
```

### Utökad User-modell för team-funktionalitet

```csharp
public class User
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }                  // Systemroll
    public string? TeamId { get; set; }               // Tillhörande team
    public TeamRole? TeamRole { get; set; }           // Roll inom teamet
    
    // Navigation properties
    public Team? Team { get; set; }
}

public enum TeamRole
{
    Admin,      // Fullständig teamkontroll
    Editor,     // Kan redigera delat innehåll
    Reader      // Kan endast läsa delat innehåll
}
```

### Delningssystem

```csharp
public class ShareLink
{
    public string Id { get; set; }                    // Unik identifierare
    public string ResourceId { get; set; }            // Resurs som delas
    public ShareLinkType Type { get; set; }           // Lesson eller Course
    public ShareLinkVisibility Visibility { get; set; } // Public eller Private
    public DateTime? ExpiresAt { get; set; }          // Utgångsdatum
    public string Token { get; set; }                 // Åtkomsttoken
    public string CreatedBy { get; set; }             // Skapare
    public DateTime CreatedAt { get; set; }           // Skapad tidpunkt
    
    // Navigation properties
    public User? Creator { get; set; }
}
```

## Kärnfunktionalitet

### 1. Team-struktur och medlemskap

**Team-skapande**
- Instruktörer kan skapa team för sin organisation/skola
- Team-namn och beskrivning
- Automatisk admin-roll för skaparen

**Medlemshantering**
- Bjud in instruktörer via e-post
- Hantera medlemskaps-roller
- Ta bort medlemmar från team

**Roller och behörigheter**

| Roll | Rättigheter |
|------|-------------|
| **Admin** | Fullständig teamkontroll: bjuda in/ta bort medlemmar, hantera roller, moderera innehåll |
| **Editor** | Skapa/redigera delat innehåll, kommentera, föreslå ändringar |
| **Reader** | Läsa och exportera delat innehåll, kommentera (read-only) |

### 2. Innehållsdelning inom team

**Bibliotek-delning**
- Mönster och övningar kan delas med teamet
- Team-bibliotek synligt för alla medlemmar
- Centraliserat innehåll för konsistens

**Mall-delning**
- Lektions- och kursmallar delas inom teamet
- Standardiserade mallar för organisationen
- Versionshantering för team-mallar

**Kurs- och lektionsdelning**
- Färdiga kursplaner kan delas för återanvändning
- Kollegialt stöd och kvalitetssäkring
- Backup och kontinuitet vid sjukdom/semester

### 3. Samarbetsworkflow

**Review-processen**
1. **Skapande**: Editor skapar eller redigerar innehåll
2. **Förhandsgranskning**: Innehållet markeras för granskning
3. **Kommentarer**: Team-medlemmar lämnar feedback
4. **Ändringsförslag**: Konkreta förbättringsförslag
5. **Godkännande**: Admin eller senior editor godkänner
6. **Publicering**: Innehållet blir tillgängligt för teamet

**Kommentarsystem**
- Inline-kommentarer på specifika sektioner
- Diskussions-trådar för större förbättringar
- Taggning av team-medlemmar för input
- Lösning och arkivering av kommentarer

### 4. Kvalitetssäkring

**Innehållsstandarder**
- Team-specifika riktlinjer för innehåll
- Mallar för konsekvens
- Obligatoriska fält och metadata

**Granskningsrutiner**
- Automatisk notifiering vid nytt innehåll
- Eskalering av ogranskade förslag
- Rapporter över team-aktivitet

## Användarupplevelse

### Team Dashboard

**Översikt**
- Team-aktivitet och statistik
- Pågående granskningar
- Senaste delningar och uppdateringar

**Team-medlemmar**
- Lista över alla medlemmar med roller
- Aktivitetsstatus och senaste inloggning
- Kontaktinformation och expertområden

### Delnings-interface

**Snabbdelning**
- En-klicks delning med teamet
- Delningsinställningar (redigering/läs-only)
- Automatisk notifiering till team

**Avancerad delning**
- Selektiv delning med specifika medlemmar
- Tidsbegränsad delning
- Externa delningslänkar

### Samarbets-workspace

**Kommentarvy**
- Sidopanel med alla kommentarer
- Filtrering efter typ och status
- Direktmeddelanden till kommentatorer

**Versionshistorik**
- Visuell historik över ändringar
- Jämförelser mellan versioner
- Återställning till tidigare versioner

## Teknisk implementation

### API-endpoints (planerade)

```
// Team-hantering
GET    /api/teams                        // Användarens team
GET    /api/teams/{id}                   // Specifikt team
POST   /api/teams                        // Skapa team
PUT    /api/teams/{id}                   // Uppdatera team
DELETE /api/teams/{id}                   // Ta bort team

// Medlemskap
GET    /api/teams/{id}/members           // Team-medlemmar
POST   /api/teams/{id}/invite           // Bjud in medlem
PUT    /api/teams/{id}/members/{userId} // Uppdatera medlemsroll
DELETE /api/teams/{id}/members/{userId} // Ta bort medlem

// Delning
GET    /api/teams/{id}/shared           // Delat innehåll
POST   /api/share                       // Skapa delning
GET    /api/share/{token}               // Åtkomst via delningslänk
DELETE /api/share/{id}                  // Ta bort delning

// Kommentarer och review
GET    /api/content/{id}/comments       // Kommentarer för innehåll
POST   /api/content/{id}/comments       // Lägg till kommentar
PUT    /api/comments/{id}               // Uppdatera kommentar
DELETE /api/comments/{id}               // Ta bort kommentar
POST   /api/content/{id}/review         // Skicka för granskning
POST   /api/content/{id}/approve        // Godkänn innehåll
```

### Notifikationssystem

**Real-time uppdateringar**
- SignalR för live-kommentarer
- Push-notifikationer för viktiga händelser
- E-postsammanfattningar för team-aktivitet

**Notifikationstyper**
- Nya kommentarer på ditt innehåll
- Innehåll som behöver granskas
- Team-inviter och roll-ändringar
- Deadlines för granskningar

### Säkerhet och integritet

**Åtkomstkontroll**
- Role-based access control (RBAC)
- Resurs-specifika behörigheter
- Audit-log för alla team-aktiviteter

**Datahantering**
- Kryptering av känsligt innehåll
- GDPR-compliance för personuppgifter
- Dataexport för team-migration

## Framtida funktioner

### Avancerad samarbete
- Real-time samredigering av lektionsplaner
- Videokonferens-integration för diskussioner
- Automatisk översättning för internationella team

### Analytics och insights
- Team-produktivitetsmetrik
- Innehållsanvändnings-statistik
- Kvalitetsmetrik för granskningsprocesser

### Integration med externa system
- LMS-integration för kurshantering
- Kalender-integration för schemaläggning
- CRM-integration för elevhantering

## Säkerhetsöverväganden

### Dataskydd
- Rollbaserad åtkomst till känslig information
- Kryptering av delat innehåll
- Spårning av vem som accederat vilket innehåll

### Team-isolering
- Strikt separation mellan olika team
- Ingen möjlighet att se andra teams innehåll
- Säker övergång mellan team

### Revision och compliance
- Fullständig audit-trail för alla ändringar
- Möjlighet att exportera team-data
- Compliance med utbildnings-regleringar

## Relaterade komponenter

- **Mallsystem**: Team-mallar och delning av standarder
- **Behörighetssystem**: Integrerad rollhantering
- **Notifikationssystem**: Kommunikation och uppdateringar
- **Export-system**: Delning med externa parter
- **Versionshantering**: Spårning av team-bidrag