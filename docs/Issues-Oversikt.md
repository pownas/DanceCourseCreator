# Issues-översikt för DanceCourseCreator

**Relaterat till**: [Implementeringsplan](./Implementeringsplan.md)  
**Datum**: 2025-11-21  
**Version**: 1.0

Detta dokument innehåller en snabböversikt över alla planerade issues för DanceCourseCreator-projektet. För detaljerad information om varje issue, se [Implementeringsplan.md](./Implementeringsplan.md).

---

## 📋 Issue-lista efter prioritet

### 🔴 MUST (Fas 1) - Kritisk funktionalitet

| # | Titel | Estimat | Krav | Status |
|---|-------|---------|------|--------|
| 1 | Fullständig Template-implementering | 5-8 dagar | FR-030 | ⏳ Planerad |
| 2 | Avancerad lektionsbyggare med tidsvalidering | 5-8 dagar | FR-010-013, BR-001, BR-003 | ⏳ Planerad |
| 3 | Kursplanering med progression och täckning | 5-7 dagar | FR-020-023, BR-004 | ⏳ Planerad |
| 4 | Export-funktionalitet (PDF/Markdown) | 5-8 dagar | FR-050-052 | ⏳ Planerad |

**Fas 1 Total estimat**: 20-31 dagar (4-6 veckor för 1 utvecklare, 8-12 veckor med test och review)

---

### 🟡 SHOULD (Fas 2) - Viktig funktionalitet

| # | Titel | Estimat | Krav | Status |
|---|-------|---------|------|--------|
| 5 | Delningslänkar och åtkomstkontroll | 4-6 dagar | FR-051, NFR-003 | ⏳ Planerad |
| 6 | Team Collaboration - Kommentarer och granskningar | 6-8 dagar | FR-032, FR-070-071, UC8 | ⏳ Planerad |
| 7 | Rekommendationssystem för patterns | 5-7 dagar | FR-013-014, UC4 | ⏳ Planerad |
| 8 | Media och musikintegration | 4-5 dagar | FR-040-041 | ⏳ Planerad |
| 9 | Import av patterns från CSV/JSON | 3-5 dagar | FR-060 | ⏳ Planerad |

**Fas 2 Total estimat**: 22-31 dagar (4-6 veckor för 1 utvecklare, 6-10 veckor med test och review)

---

### 🟢 COULD (Fas 3) - Värdefull funktionalitet

| # | Titel | Estimat | Krav | Status |
|---|-------|---------|------|--------|
| 10 | Rapporter och insikter | 4-6 dagar | FR-080-082 | ⏳ Planerad |
| 11 | Versionering och ändringshistorik | 5-7 dagar | FR-031, NFR-008 | ⏳ Planerad |
| 12 | Internationalisering (i18n) - Svenska/Engelska | 4-6 dagar | NFR-006 | ⏳ Planerad |
| 13 | WCAG-förbättringar för tillgänglighet | 3-5 dagar | NFR-002 | ⏳ Planerad |
| 14 | PWA och offline-funktionalitet | 4-6 dagar | NFR-005 | ⏳ Planerad |

**Fas 3 Total estimat**: 20-30 dagar (4-6 veckor för 1 utvecklare, 4-8 veckor med test och review)

---

### 🔵 Fas 4 - Deltagarhantering och schemaläggning

> **Not**: Fas 4 är identifierad baserat på [Systemoversyn.md](./Systemoversyn.md) och kräver ytterligare detaljerad planering.

| # | Titel | Estimat | Krav | Status |
|---|-------|---------|------|--------|
| 15 | Schemaläggning | 5-7 dagar | FR-100..104 | ⏳ Planerad |
| 16 | Inbjudningar och bokningsflöden | 6-8 dagar | FR-120..125 | ⏳ Planerad |
| 17 | Kalenderintegration (iCal, Google, Outlook) | 5-7 dagar | FR-110..114 | ⏳ Planerad |
| 18 | Deltagarstatistik och närvaro | 4-6 dagar | FR-140..145 | ⏳ Planerad |
| 19 | Bedömningssystem | 5-7 dagar | FR-130..135 | ⏳ Planerad |
| 20 | Automatiserad kursplanering | 8-12 dagar | FR-150..154 | ⏳ Planerad |

**Fas 4 Total estimat**: 33-47 dagar (7-10 veckor för 1 utvecklare)

---

## 📊 Status-symboler

- ⏳ **Planerad** - Issue har identifierats och dokumenterats
- 🚀 **Klar att starta** - Alla beroenden uppfyllda
- 👷 **Pågår** - Aktivt arbete
- 🔍 **Under review** - Code review eller testing
- ✅ **Klar** - Implementerad, testad, och mergad
- 🚫 **Blockerad** - Väntar på beroenden eller beslut
- ❌ **Avbruten** - Inte längre relevant

---

## 🔗 Beroendekedjor

### Kritisk väg (måste göras i ordning):
```
Issue 1 (Templates) → Kan göras parallellt med andra
Issue 2 (Lektionsbyggare) → Issue 3 (Kursplanering) → Issue 7 (Rekommendationer)
Issue 2 (Lektionsbyggare) → Issue 4 (Export)
Issue 4 (Export) → Issue 5 (Delning)

Fas 4 beroenden:
Fas 3 → Issue 15 (Schemaläggning) → Issue 16 (Bokning) → Issue 18 (Statistik)
Issue 15 (Schemaläggning) → Issue 17 (Kalender)
Issue 18 (Statistik) → Issue 19 (Bedömning)
Fas 3 → Issue 20 (Automatisering)
```

### Oberoende issues (kan göras parallellt):
- Issue 1 (Templates)
- Issue 6 (Team Collaboration)
- Issue 8 (Media)
- Issue 9 (Import)
- Issue 10 (Rapporter)
- Issue 11 (Versionering)
- Issue 12 (i18n)
- Issue 13 (WCAG)
- Issue 14 (PWA)

---

## 🎯 Milstolpar

### Milstolpe 1: MVP - Komplett kursskapande (Efter Fas 1)
**Mål**: Instruktörer kan skapa, strukturera och exportera kurser
- ✅ Templates fungerar
- ✅ Lektionsbyggare med sektioner och tidskontroll
- ✅ Kursplanering med progression
- ✅ Export till PDF och Markdown

**Kriterier för godkänd milstolpe**:
- Alla Fas 1 issues är klara
- E2E-tester för hela flödet fungerar
- Minst 5 testinstruktörer har testat och godkänt
- Dokumentation uppdaterad

---

### Milstolpe 2: Team-funktionalitet (Efter Fas 2)
**Mål**: Flera instruktörer kan samarbeta effektivt
- ✅ Delning av kursmaterial
- ✅ Team collaboration med kommentarer
- ✅ Rekommendationssystem hjälper planering
- ✅ Media och musik integrerat
- ✅ Import av befintligt material

**Kriterier för godkänd milstolpe**:
- Alla Fas 2 issues är klara
- Team-funktioner testade med riktiga team
- Performance inom specificerade gränser
- Security audit genomförd

---

### Milstolpe 3: Production-ready (Efter Fas 3)
**Mål**: Professionell, tillgänglig produkt redo för release
- ✅ Rapporter och insikter
- ✅ Versionering och audit trail
- ✅ Flerspråksstöd (SV/EN)
- ✅ WCAG AA-compliance
- ✅ PWA med offline-funktionalitet

**Kriterier för godkänd milstolpe**:
- Alla Fas 3 issues är klara
- WCAG audit score > 90%
- Load testing genomfört (100+ samtidiga användare)
- Beta-test med 20+ instruktörer
- Launch-plan etablerad

---

### Milstolpe 4: Deltagarhantering (Efter Fas 4)
**Mål**: Fullständig hantering av deltagare, schemaläggning och uppföljning
- ✅ Schemaläggning av kurser med datum och tider
- ✅ Inbjudningslänkar och bokningsflöden
- ✅ Kalenderintegration (iCal, Google, Outlook)
- ✅ Närvaroregistrering och deltagarstatistik
- ✅ Bedömningssystem för deltagarutveckling
- ✅ AI-assisterad kursplanering

**Kriterier för godkänd milstolpe**:
- Alla Fas 4 issues är klara
- E-postnotifikationer fungerar tillförlitligt
- GDPR-compliance för deltagardata verifierad
- Minst 10 dansskolor/klubbar har testat funktionerna
- Dokumentation för deltagarhantering färdig

> **Not**: Se [Systemoversyn.md](./Systemoversyn.md) för detaljerade krav (FR-100 serien)

---

## 📈 Veckovis tidslinje (grov uppskattning)

### Fas 1: Grundfunktionalitet (Vecka 1-12)
- **Vecka 1-2**: Issue 1 - Templates
- **Vecka 3-4**: Issue 2 - Lektionsbyggare
- **Vecka 5-6**: Issue 3 - Kursplanering
- **Vecka 7-8**: Issue 4 - Export
- **Vecka 9-10**: Integration testing och bugfixar
- **Vecka 11-12**: Beta-testning och feedback

### Fas 2: Team och intelligens (Vecka 13-24)
- **Vecka 13-14**: Issue 5 - Delning
- **Vecka 15-17**: Issue 6 - Team Collaboration
- **Vecka 18-19**: Issue 7 - Rekommendationer
- **Vecka 20-21**: Issue 8 - Media och Issue 9 - Import (parallellt)
- **Vecka 22-23**: Integration testing
- **Vecka 24**: Sprint review och retrospektiv

### Fas 3: Polish (Vecka 25-32)
- **Vecka 25-26**: Issue 10 - Rapporter och Issue 11 - Versionering
- **Vecka 27-28**: Issue 12 - i18n och Issue 13 - WCAG
- **Vecka 29-30**: Issue 14 - PWA
- **Vecka 31**: Slutlig testning och polish
- **Vecka 32**: Launch preparation och dokumentation

### Fas 4: Deltagarhantering (Vecka 33-42)
- **Vecka 33-34**: Issue 15 - Schemaläggning
- **Vecka 35-36**: Issue 16 - Inbjudningar och bokningsflöden
- **Vecka 37-38**: Issue 17 - Kalenderintegration
- **Vecka 39-40**: Issue 18 - Deltagarstatistik och Issue 19 - Bedömning (parallellt)
- **Vecka 41-42**: Issue 20 - Automatiserad planering
- **Vecka 43**: Slutlig testning och GDPR-granskning

---

## 🔄 Iterativ utveckling

Varje issue följer denna cykel:
1. **Planering** (0.5 dag) - Detaljerad teknisk design
2. **Implementation** (3-6 dagar) - Kod och enhetstester
3. **Testing** (1 dag) - E2E-tester och manuell verifiering
4. **Review** (0.5 dag) - Code review och feedback
5. **Polish** (0.5 dag) - Adressera review-kommentarer
6. **Documentation** (0.5 dag) - Uppdatera docs och README

Total overhead: ~3 dagar per issue utöver ren implementation

---

## 📝 Checklista för varje issue

När ett issue påbörjas:
- [ ] Läs Implementeringsplan.md för detaljer
- [ ] Granska relaterade krav i Kravspecifikation.md
- [ ] Identifiera beroenden och blockerare
- [ ] Skapa teknisk design-anteckning
- [ ] Skapa branch: `feature/issue-{number}-{short-name}`

Under implementation:
- [ ] Följ kodstandard och best practices
- [ ] Skriv enhetstester parallellt med kod
- [ ] Uppdatera E2E-tester vid UI-ändringar
- [ ] Commit ofta med beskrivande meddelanden
- [ ] Push regelbundet för backup

Före issue completion:
- [ ] Alla acceptanskriterier uppfyllda
- [ ] Enhetstester körs och passerar
- [ ] E2E-tester körs och passerar
- [ ] Ingen regression i befintliga tester
- [ ] Code review begärd och genomförd
- [ ] Dokumentation uppdaterad
- [ ] PR skapad med länk till issue

Efter merge:
- [ ] Issue markeras som klar
- [ ] Releasenoteringar uppdaterade
- [ ] Nästa beroende issue kan starta

---

## 🚀 Snabbstart för utvecklare

### Setup för nytt issue:
```bash
# 1. Uppdatera main branch
git checkout main
git pull origin main

# 2. Skapa feature branch
git checkout -b feature/issue-{number}-{name}

# 3. Läs issue-detaljer i Implementeringsplan.md
# 4. Börja koda!
```

### Testing-workflow:
```bash
# Kör enhetstester
dotnet test

# Kör E2E-tester
cd src/DanceCourseCreator.Tests.E2E
dotnet test

# Starta app för manuell testning
cd src/DanceCourseCreator.API && dotnet run &
cd src/DanceCourseCreator.Web && dotnet run
```

---

## 📞 Kontakt och support

För frågor om denna implementeringsplan:
- **Tekniska frågor**: Skapa diskussion i GitHub Discussions
- **Bug reports**: Skapa issue med "bug" label
- **Feature requests**: Skapa issue med "enhancement" label
- **Dokumentation**: Uppdatera direkt via PR

---

## 📚 Relaterad dokumentation

- [Implementeringsplan](./Implementeringsplan.md) - Detaljerad plan för varje issue
- [Systemoversyn](./Systemoversyn.md) - Systemöversyn med utökade krav (Fas 4)
- [Kravspecifikation](../Kravspecifikation.md) - Fullständiga funktionella krav
- [README](../README.md) - Projektöversikt och getting started
- [Implementering: Mallsystem](./Implementering-Mallsystem.md)
- [Implementering: Teamsamarbete](./Implementering-Teamsamarbete.md)
- [WCAG Compliance Report](./WCAG-Compliance-Report.md)
- [Playwright Test Summary](./PLAYWRIGHT_IMPLEMENTATION_SUMMARY.md)

---

**Senast uppdaterad**: 2025-11-27  
**Nästa granskning**: Vid completion av varje fas
