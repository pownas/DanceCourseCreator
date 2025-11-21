# Implementeringsguider - DanceCourseCreator

Denna katalog innehåller detaljerade implementeringsguider och planer för DanceCourseCreator-systemet.

## 🚀 Kom igång

### [⚡ SNABBSTART](./SNABBSTART.md)
**Börja här!** Välj din roll (utvecklare, PO, designer, QA) och få direkt vägledning för vad du ska göra härnäst.

---

## 🗺️ Projektplanering

### [🎯 Implementeringsplan](./Implementeringsplan.md)
**Huvuddokument** - Omfattande plan med alla features uppdelade i konkreta issues.

Innehåller:
- Komplett översikt av vad som är implementerat och vad som återstår
- 14 detaljerade issues med acceptanskriterier
- MoSCoW-prioritering (Must/Should/Could/Won't)
- Tekniska riktlinjer och kodstandard
- Estimat och tidsplanering

**Börja här** för att förstå projektets roadmap och nästa steg.

### [📋 Issues-översikt](./Issues-Oversikt.md)
Snabb referensguide över alla planerade issues.

Innehåller:
- Tabellöversikt med prioritet och estimat
- Beroendekedjor mellan issues
- Milstolpar och veckovis tidslinje
- Checklista för varje issue
- Snabbstart för utvecklare

**Använd detta** som dagligt referensdokument under utveckling.

---

## 📚 Implementeringsguider för specifika funktioner
Denna katalog innehåller detaljerade implementeringsguider och analyser för nyckelkomponenter i DanceCourseCreator-systemet.

## Tillgängliga dokument

### [🔍 Analys: Förbättrat danskursflöde](./Analys-Danskursflode.md)
Omfattande analys och förslag för att skapa ett intuitivt och flexibelt flöde för att bygga danskurser:
- Nulägesanalys av systemet och identifierade förbättringsområden
- Användarbehov och ideala arbetsflöden
- Konkreta förslag för turbank, lektionsbyggare och kursplanering
- Dataorganisation och nya API-endpoints
- 4-fas implementeringsplan med konkreta sprint-mål
- Diskussion av möjligheter, utmaningar och nästa steg

**Omfattar**: UC1-UC8, användarflödesdesign, turbanksintegration, kursgenereringsprocess

### [📋 Implementering: Mallsystem](./Implementering-Mallsystem.md)
Beskriver implementeringen av template-systemet som möjliggör för instruktörer att:
- Spara och återanvänd strukturer för lektioner och kurser
- Skapa standardiserade mallar för konsistens
- Dela mallar inom team för effektivt samarbete
- Hantera versioner och historik

**Täcker funktionella krav**: FR-030, FR-031, FR-032  
**Relaterat issue**: Issue 1 i Implementeringsplan.md

### [🤝 Implementering: Teamsamarbete](./Implementering-Teamsamarbete.md)
Beskriver implementeringen av team collaboration-systemet som möjliggör:
- Team-struktur med rollbaserad åtkomstkontroll
- Delning av kursmaterial inom organisationen
- Kommentarer och granskningsprocesser
- Kvalitetssäkring genom peer review

**Täcker funktionella krav**: FR-070, FR-071, FR-032, UC8  
**Relaterat issue**: Issue 6 i Implementeringsplan.md

### [📝 Implementeringsplan: Mallsystem](./Implementeringsplan-Mallsystem.md)
Detaljerad projektplan specifikt för mallsystem-implementeringen med tidslinje och milstolpar.

---

## 🧪 Test- och kvalitetsdokumentation

### [🎭 Playwright Implementation Summary](./PLAYWRIGHT_IMPLEMENTATION_SUMMARY.md)
Översikt över end-to-end testning med Playwright:
- Test-kategorier och coverage
- Screenshot-organisation
- Hur man kör tester
- Test-resultat och rapportering

### [♿ WCAG Compliance Report](./WCAG-Compliance-Report.md)
Tillgänglighetsstatus och compliance-nivå:
- WCAG 2.1 AA-checklistor
- Identifierade förbättringsområden
- Roadmap för tillgänglighet

**Relaterat issue**: Issue 13 i Implementeringsplan.md

---

## 📖 Relaterad dokumentation

- [📖 Fullständig kravspecifikation](../Kravspecifikation.md) - Omfattande kravdokumentation med alla FR/NFR
- [🏗️ Projekt-README](../README.md) - Översikt, getting started, och teknisk stack
- [📜 LICENSE](../LICENSE) - MIT License
- [🏗️ .NET 8 Implementation](../README.md) - Teknisk översikt av .NET-implementeringen
- [♿ WCAG Compliance](./WCAG-Compliance-Report.md) - Tillgänglighetsrapport

## Utvecklingsinformation

Dessa implementeringsguider är avsedda för:
- **Utvecklare** som implementerar dessa funktioner
- **Produktägare** som behöver förstå funktionaliteten
- **Instruktörer** som vill förstå systemets möjligheter
- **Team-ledare** som planerar rollouts och träning

## Teknisk arkitektur

Båda implementeringarna baseras på:
- **.NET 8** för backend-API
- **Blazor WebAssembly** för frontend
- **Entity Framework Core** för dataåtkomst
- **SQLite** för datalagring
- **JWT** för autentisering och behörigheter

## Nästa steg

För att implementera dessa funktioner:

1. **Läs kravspecifikationen** för fullständig kontext
2. **Granska de befintliga modellerna** i `src/DanceCourseCreator.API/Models/`
3. **Implementera controllers och services** enligt API-specifikationerna
4. **Bygg frontend-komponenter** med MudBlazor
5. **Testa grundligt** med fokus på användarupplevelse

## Bidrag

Denna dokumentation ska hållas uppdaterad när implementeringen framskrider. Vid ändringar i funktionalitet eller datamodeller, uppdatera motsvarande implementeringsguide.