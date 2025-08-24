# Implementeringsguider - DanceCourseCreator

Denna katalog innehåller detaljerade implementeringsguider för nyckelkomponenter i DanceCourseCreator-systemet.

## Tillgängliga implementeringsguider

### [📋 Implementering: Mallsystem](./Implementering-Mallsystem.md)
Beskriver implementeringen av template-systemet som möjliggör för instruktörer att:
- Spara och återanvänd strukturer för lektioner och kurser
- Skapa standardiserade mallar för konsistens
- Dela mallar inom team för effektivt samarbete
- Hantera versioner och historik

**Täcker funktionella krav**: FR-030, FR-031, FR-032

### [🤝 Implementering: Teamsamarbete](./Implementering-Teamsamarbete.md)
Beskriver implementeringen av team collaboration-systemet som möjliggör:
- Team-struktur med rollbaserad åtkomstkontroll
- Delning av kursmaterial inom organisationen
- Kommentarer och granskningsprocesser
- Kvalitetssäkring genom peer review

**Täcker funktionella krav**: FR-070, FR-071, FR-032, UC8

## Relaterad dokumentation

- [📖 Fullständig kravspecifikation](../Kravspecifikation.md) - Omfattande kravdokumentation
- [🏗️ .NET 8 Implementation](../README-dotnet.md) - Teknisk översikt av .NET-implementeringen
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