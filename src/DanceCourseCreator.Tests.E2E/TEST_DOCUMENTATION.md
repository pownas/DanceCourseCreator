# Playwright Test Documentation

## Översikt

Detta dokument beskriver de automatiserade Playwright-testerna för Dance Course Creator-applikationen. Testerna är utformade för att:

1. **Verifiera funktionalitet** - Säkerställa att användarflöden fungerar som förväntat
2. **Dokumentera användargränssnitt** - Tillhandahålla visuell dokumentation genom screenshots
3. **Regressionstestning** - Upptäcka oavsiktliga förändringar i UI/funktionalitet
4. **Utbildning** - Hjälpa nya teammedlemmar förstå applikationen

## Test-struktur

Testerna är organiserade i separata filer baserat på funktionsområde:

### 1. HomeAndNavigationTests.cs
**Syfte:** Testa startsidan och navigering mellan olika delar av applikationen

**Användarflöden som testas:**
- Ladda startsidan och verifiera innehåll
- Navigera till Mönster, Lektioner, Kurser och Mallar
- Navigera tillbaka till startsidan
- Använda snabbåtgärder ("Kom igång"-knappar)

**Screenshots som genereras:**
- Startsidan med välkomsttext och statistik
- Varje huvudsektion (Mönster, Lektioner, Kurser, Mallar)
- Snabbåtgärder och deras resultat

**Praktiskt värde:**
- Visar applikationens grundläggande struktur
- Demonstrerar hur användare navigerar mellan olika delar
- Dokumenterar alla tillgängliga sektioner

### 2. PatternLibraryTests.cs
**Syfte:** Testa mönsterbiblioteket och dess filtreringsfunktioner

**Användarflöden som testas:**
- Ladda och visa mönsterbiblioteket
- Filtrera mönster efter typ (Mönster/Övning)
- Filtrera mönster efter nivå (Nybörjare, Förbättrare, etc.)
- Söka efter specifika mönster
- Visa detaljerad information om ett mönster

**Screenshots som genereras:**
- Mönsterbiblioteket med alla mönster
- Filter-dropdowns öppna
- Resultat efter filtrering
- Sökfunktion i användning
- Mönsterdetaljer i dialog

**Praktiskt värde:**
- Visar hur instruktörer hittar mönster de behöver
- Demonstrerar filtreringsfunktionalitet
- Dokumenterar mönsterinformation som visas

### 3. CourseCreationTests.cs
**Syfte:** Testa processen för att skapa en ny kursplan

**Användarflöden som testas:**
- Öppna dialogrutan för att skapa kurs
- Fylla i kursnamn
- Välja kursnivå
- Ange kursens längd (antal veckor)
- Lägga till kursmål
- Spara den nya kursen
- Hantera tomma tillstånd (när inga kurser finns)

**Screenshots som genereras:**
- Kurssidan initialt
- Tom skapandeformulär
- Formulär med ifylld information steg för steg
- Nivå-dropdown öppen
- Mållista med tillagda mål
- Resultat efter att kursen har skapats

**Praktiskt värde:**
- Visar komplett arbetsflöde för kursskapande
- Dokumenterar alla fält som behövs
- Demonstrerar hur mål läggs till
- Hjälper nya användare förstå kursplaneringsprocessen

### 4. CourseEditingTests.cs
**Syfte:** Testa redigering av befintliga kursplaner

**Användarflöden som testas:**
- Öppna redigeringsdialogrutan för en befintlig kurs
- Modifiera kursnamn
- Ändra kurslängd
- Lägga till ytterligare mål
- Spara ändringar
- Visa kursdetaljer
- Avbryta redigering

**Screenshots som genereras:**
- Kurslista med redigeringsalternativ
- Redigeringsformulär med befintlig data
- Stegvisa ändringar av fält
- Nya mål som läggs till
- Resultat efter sparande
- Visningsläge för kursdetaljer
- Avbrutna ändringar

**Praktiskt värde:**
- Visar hur befintliga kurser kan uppdateras
- Dokumenterar skillnaden mellan visa och redigera
- Demonstrerar att ändringar kan avbrytas
- Hjälper användare förstå versionering och uppdateringar

### 5. LessonAndTemplateTests.cs
**Syfte:** Testa lektionsplanering och mallhantering

**Användarflöden som testas:**
- Ladda lektionssidan
- Öppna dialogrutan för att skapa lektion
- Filtrera och söka lektioner
- Ladda mallsidan
- Skapa ny mall
- Visa malldetaljer
- Redigera mall
- Duplicera mall

**Screenshots som genereras:**
- Lektionssidan med lektionsplaner
- Skapandedialog för lektioner
- Filtrering av lektioner
- Mallsidan med tillgängliga mallar
- Mallskapande dialog
- Malldetaljer
- Redigerings- och dupliceringsfunktioner

**Praktiskt värde:**
- Visar hur lektionsplaner skapas och hanteras
- Dokumenterar mallsystemet för återanvändning
- Demonstrerar arbetsflödet för duplicering av mallar
- Hjälper instruktörer optimera sitt arbete med mallar

## Så här används testerna

### För kvalitetssäkring
1. Kör testerna regelbundet efter kodändringar
2. Granska screenshots för att upptäcka oönskade UI-förändringar
3. Använd kategorier för att testa specifika områden:
   ```bash
   dotnet test --filter "TestCategory=Courses"
   ```

### För dokumentation
1. Screenshots kan användas i användarhandböcker
2. Visuell demonstration av funktioner för intressenter
3. Onboarding-material för nya teammedlemmar
4. Marknadsföringsmaterial som visar applikationens kapacitet

### För utveckling
1. Validera att nya funktioner fungerar som förväntat
2. Säkerställa att ändringar inte bryter befintlig funktionalitet
3. Förstå användarupplevelsen genom olika flöden
4. Identifiera förbättringsmöjligheter i UI/UX

## Screenshot-organisation

Alla screenshots organiseras i underkataloger baserat på funktionsområde:

```
screenshots/
├── navigation/          # Navigering och startsida
├── patterns/            # Mönsterbibliotek
├── course-creation/     # Kursskapande
├── course-editing/      # Kursredigering
└── lessons-templates/   # Lektioner och mallar
```

Varje screenshot har ett beskrivande namn som börjar med ett sekvensnummer för att visa flödet.

## Bästa praxis

### När du skriver nya tester:
1. **Följ namnkonventionen** - Använd beskrivande namn som förklarar vad som testas
2. **Lägg till TestCategory-attribut** - Gör det enkelt att köra relaterade tester
3. **Inkludera Screenshots-kategorin** - För alla tester som tar screenshots
4. **Organisera screenshots** - Använd underkataloger för olika funktionsområden
5. **Lägg till väntetider** - Använd `WaitForSelectorAsync` eller `WaitForTimeoutAsync` för att säkerställa UI har laddats
6. **Hantera fel gracefully** - Tester bör vara robusta mot mindre UI-ändringar

### När du underhåller tester:
1. **Uppdatera selektorer** - Om UI-element ändras, uppdatera `GetByRole`, `GetByLabel`, etc.
2. **Verifiera screenshots** - Granska nya screenshots efter ändringar
3. **Dokumentera förändringar** - Uppdatera README och denna fil när tester ändras
4. **Behåll konsistens** - Använd samma viewport-storlek och inställningar i alla tester

## Felsökning

### Tester misslyckas
1. Kontrollera att både API och Client körs på rätt portar
2. Verifiera att Playwright-browsers är installerade
3. Öka timeout-värden för långsamma miljöer
4. Granska felmeddelanden och screenshots

### Screenshots saknas
1. Kontrollera att screenshots-katalogen skapas i TestInitialize
2. Verifiera att sökvägar är korrekta
3. Se till att testet inte misslyckas innan screenshot tas

### UI-element hittas inte
1. Vänta på att element ska laddas med `WaitForSelectorAsync`
2. Använd Playwright Inspector för att identifiera element: `pwsh bin/Debug/net8.0/playwright.ps1 codegen http://localhost:5034`
3. Kontrollera att element verkligen existerar i UI:t
4. Prova alternativa selektorer (GetByText, GetByLabel, etc.)

## Framtida förbättringar

Möjliga förbättringar av test-suite:
1. **Parallell exekvering** - Kör tester samtidigt för snabbare feedback
2. **Cross-browser testning** - Testa i Firefox och Safari förutom Chromium
3. **Mobil viewport** - Lägg till tester för responsiv design
4. **Accessibility-tester** - Verifiera WCAG-compliance
5. **Performance-tester** - Mät laddningstider och rendering
6. **API-mocking** - Isolera frontend-tester från backend-beroenden
7. **Visual regression testing** - Automatisk jämförelse av screenshots
8. **Exportfunktionalitet** - Tester för PDF/Markdown-export när funktionen implementeras

## Sammanfattning

Dessa Playwright-tester utgör en omfattande test-suite som:
- ✅ Täcker alla huvudsakliga användarflöden
- ✅ Genererar visuell dokumentation genom screenshots
- ✅ Hjälper till att upptäcka regressioner
- ✅ Stödjer kvalitetssäkring och utveckling
- ✅ Ger insikt i hur applikationen används

Testerna är organiserade logiskt, väldokumenterade och enkla att underhålla och utöka.
