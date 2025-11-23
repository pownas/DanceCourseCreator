# Tidsvalidering i Lektionsbyggaren (Phase 1)

## Översikt

Den avancerade lektionsbyggaren med tidsvalidering hjälper dig att skapa balanserade och realistiska lektionsplaner genom att:
- Automatiskt beräkna estimerad tid baserat på innehåll
- Validera att total tid är inom rimliga gränser
- Varna när tidsallokering inte matchar innehållet
- Blockera sparande vid kritiska valideringsfel

## Hur det fungerar

### 1. Grundläggande tidsvalidering

När du skapar eller redigerar en lektion valideras automatiskt:

**Lektionslängd:**
- Minimum: 60 minuter (1 timme)
- Maximum rekommenderat: 300 minuter (5 timmar)
- Varning visas om längden är utanför rekommenderat intervall

**Antal sektioner:**
- Rekommenderat: 3-8 sektioner per lektion
- Varning visas om antalet är utanför detta intervall

### 2. Sektionsbaserad tidsallokering

För varje sektion i din lektion kan du:
- **Ange allokerad tid** - Hur många minuter planerar du för denna sektion?
- **Se estimerad tid** - Systemet beräknar automatiskt baserat på mönster/övningar
- **Få färgkodad feedback**:
  - 🟢 Grön: Bra matchning mellan allokerad och estimerad tid
  - 🔵 Blå: Acceptabel skillnad
  - 🟡 Gul: Stor skillnad - överväg justering
  - ⚪ Grå: Ingen tid allokerad ännu

### 3. Visuell tidsöversikt

I lektionssammanfattningen ser du:
- **Total allokerad tid** - Summan av alla sektioners tid
- **Total estimerad tid** - Beräknad tid baserat på innehåll
- **Tidsskillnad** - Differens mellan planerad och estimerad tid
- **Förloppsindikator** - Visar hur mycket av lektionen som har allokerad tid

**Färgkoder på förloppsindikatorn:**
- 🔴 Röd: >100% - Allokerat mer tid än lektionslängd (FEL!)
- 🟢 Grön: 90-100% - Bra tidsallokering
- 🔵 Blå: 70-90% - OK, lite buffert kvar
- 🟡 Gul: 50-70% - Mycket obokad tid
- ⚪ Grå: <50% - Stor del obokad

### 4. Valideringsmeddelanden

Systemet ger tre typer av meddelanden:

**🔴 Fel (Error):**
- Blockerar sparande av lektionen
- Måste åtgärdas innan du kan fortsätta
- Exempel: "Total allokerad tid (95 min) överskrider lektionslängden (90 min) med 5 minuter."

**🟡 Varningar (Warning):**
- Låter dig spara men rekommenderar ändringar
- Indikerar potentiella problem
- Exempel: "Endast 2 sektion(er). Rekommenderat minimum är 3 sektioner per lektion."

**🔵 Information (Info):**
- Hjälpsam feedback om din lektionsplan
- Inga åtgärder krävs
- Exempel: "Sektion 1 (Warmup): Allokerad tid (20 min) ger god buffert mot estimerad tid (15 min)."

## Exempel: Skapa en 90-minuters lektion

### Steg 1: Sätt lektionslängd
Välj **90 minuter** från rullgardinsmenyn.

### Steg 2: Lägg till sektioner
Lägg till sektioner för din lektion:
1. Warmup
2. Technique
3. Patterns
4. Social

### Steg 3: Fyll i sektioner med innehåll
Lägg till mönster/övningar i varje sektion.

### Steg 4: Allokera tid
För varje sektion, ange allokerad tid:
- Warmup: 15 minuter
- Technique: 20 minuter
- Patterns: 40 minuter
- Social: 15 minuter
- **Total: 90 minuter** ✅

### Steg 5: Kontrollera validering
Scrolla ner till "Tidsöversikt och validering":
- ✅ Förloppsindikatorn visar 100% (grön)
- ✅ Inga felmeddelanden
- ✅ "Skapa lektion"-knappen är aktiv

### Steg 6: Spara
Klicka på "Skapa lektion" för att spara din plan.

## Vanliga frågor

### Måste jag fylla i tid för alla sektioner?
Nej, tidsallokering är valfritt. Men du får bättre översikt och validering om du anger tid.

### Vad händer om jag allokerar mer tid än lektionslängden?
Du får ett felmeddelande och kan inte spara lektionen förrän du justerat tiderna.

### Vad betyder "estimerad tid"?
Det är tiden som systemet beräknar baserat på mönster/övningar i sektionen. Varje mönster har en förvald estimerad tid.

### Kan jag ignorera varningar?
Ja, varningar blockerar inte sparande. De är rekommendationer för bättre lektionsplanering.

### Hur beräknas estimerad tid?
- Varje mönster/övning har en `EstimatedMinutes`-egenskap
- Systemet summerar dessa för varje sektion
- Om ett mönster saknar uppskattning används 5 minuter som standard

### Vad händer med befintliga lektioner?
Befintliga lektioner har `AllocatedMinutes = 0` för alla sektioner, vilket är ett giltigt tillstånd. Du kan redigera och lägga till tidsallokering när som helst.

## Valideringsregler (teknisk referens)

### Lektionsnivå
- **Minimum längd**: 60 minuter (Error om kortare)
- **Maximum längd**: 300 minuter (Warning om längre)
- **Sektionsantal**: 3-8 rekommenderat (Warning om utanför)
- **Tidsbuffert**: 15% tolerans för varningar

### Sektionsnivå
- **Allokering vs längd**: Error om total > lektionslängd
- **Allokering skillnad**: Warning om 10%+ skillnad från längd
- **Sektion vs estimat**: Warning om 50%+ skillnad

## Support och feedback

Har du frågor eller förslag om tidsvalideringen? Kontakta utvecklingsteamet eller öppna en issue på GitHub.

---

**Version**: 1.0 (Phase 1 MVP)
**Datum**: 2025-11-23
**Relaterade issues**: #[Fas 1] Avancerad lektionsbyggare med tidsvalidering
