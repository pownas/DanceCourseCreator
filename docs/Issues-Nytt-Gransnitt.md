# Sub-Issues: Nytt Gränssnitt för DanceCourseCreator

**Version**: 1.0  
**Datum**: 2025-11-29  
**Relaterat till**: [Kravspecifikation-Nytt-Gransnitt.md](./Kravspecifikation-Nytt-Gransnitt.md) | [Implementeringsplan-Nytt-Gransnitt.md](./Implementeringsplan-Nytt-Gransnitt.md)

---

## 📋 Översikt

Detta dokument definierar de sub-issues som ska skapas för att implementera det nya gränssnittet enligt kravspecifikationen. Arbetet är uppdelat i 6 faser med tydliga delmoment, beroenden och acceptanskriterier.

**Total estimerad tid**: 12-16 veckor  
**Antal sub-issues**: 19 huvudissues + testning

---

## 🎯 Fas-struktur

| Fas | Namn | Estimat | Antal Issues | Prioritet |
|-----|------|---------|--------------|-----------|
| **Fas 1** | Grundläggande responsiv struktur | 2-3 veckor | 6 | Kritisk |
| **Fas 2** | Turbank och pattern-komponenter | 2-3 veckor | 7 | Kritisk |
| **Fas 3** | Lektionsbyggare med dra-och-släpp | 3-4 veckor | 9 | Kritisk |
| **Fas 4** | Kursplanering och tidslinje | 2-3 veckor | 6 | Hög |
| **Fas 5** | Kompakta vyer och responsiv polish | 2-3 veckor | 7 | Hög |
| **Fas 6** | Testning och optimering | 1-2 veckor | 6 | Kritisk |

---

## 📊 Beroendekarta

```
Fas 1 (Layout)
    │
    ▼
Fas 2 (Turbank)
    │
    ├───────────────┐
    ▼               ▼
Fas 3 (Lektion)  Fas 4 (Kurs)
    │               │
    └───────┬───────┘
            ▼
    Fas 5 (Polish)
            │
            ▼
    Fas 6 (Test)
```

---

## 🔴 Fas 1: Grundläggande responsiv struktur (2-3 veckor)

### Issue UI-001: Skapa AppLayout.razor med responsiv navigering

**Titel**: `[UI Fas 1] Skapa AppLayout.razor med responsiv navigering`

#### 📋 Beskrivning
Skapa huvudlayout-komponenten som hanterar responsiv navigering mellan mobil, surfplatta och desktop. Layouten ska stödja MudBlazor's breakpoint-system och inkludera grundläggande strukturella element.

#### 🎯 Krav från specifikation
- UI-001: Responsiv design för mobil, surfplatta, desktop
- UI-003: Sidebar på desktop
- UI-002: Bottom navigation på mobil
- Sektion 4.1: Designprinciper (Mobile-first)

#### 📦 Delleveranser
- [ ] AppLayout.razor med MudLayout-struktur
- [ ] MudAppBar med responsiv header
- [ ] Konditionell rendering baserat på breakpoints
- [ ] CSS-variabler för layoutmått
- [ ] Unit tests för layout-logik

#### ✅ Acceptanskriterier
- [ ] Layout anpassas korrekt vid alla breakpoints (Xs, Sm, Md, Lg, Xl)
- [ ] Header visas på alla skärmstorlekar
- [ ] Inga layout-shiftar vid resize
- [ ] Sidladdning < 2 sekunder
- [ ] Grundläggande navigation fungerar

#### ⏱️ Estimat
3 dagar

#### 🔗 Beroenden
Inga blockerande beroenden.

---

### Issue UI-002: Implementera MobileBottomNav

**Titel**: `[UI Fas 1] Implementera MobileBottomNav för mobil navigering`

#### 📋 Beskrivning
Skapa bottom navigation-komponent för mobila enheter enligt specifikationen. Komponenten ska endast visas på skärmar < 768px och inkludera ikoner för Dashboard, Turbank, Lektioner, Kurser och snabbåtgärd (+).

#### 🎯 Krav från specifikation
- UI-002: Bottom navigation på mobil
- Sektion 4.2: Mobil layout-struktur
- NFR-A02: Touch-vänliga knappar (min 44x44px)

#### 📦 Delleveranser
- [ ] MobileBottomNav.razor komponent
- [ ] Navigationsikoner med labels
- [ ] Aktiv-state indikering
- [ ] FAB för snabbåtgärd
- [ ] CSS för touch-optimering

#### ✅ Acceptanskriterier
- [ ] Visas endast på mobil (< 768px)
- [ ] Alla ikoner har minst 44x44px klickyta
- [ ] Aktiv sida markeras visuellt
- [ ] Smooth transition vid sidbyten
- [ ] FAB öppnar snabbmeny för ny lektion/tur

#### ⏱️ Estimat
2 dagar

#### 🔗 Beroenden
- UI-001 (AppLayout.razor)

---

### Issue UI-003: Implementera DesktopSidebar

**Titel**: `[UI Fas 1] Implementera kollapserbar DesktopSidebar`

#### 📋 Beskrivning
Skapa kollapserbar sidebar-komponent för desktop och surfplatta. Sidebaren ska innehålla navigation enligt specifikationen med undermeny för varje huvudsektion.

#### 🎯 Krav från specifikation
- UI-003: Sidebar på desktop (kollapserbar)
- Sektion 4.4: Desktop layout-struktur
- Sektion 3.1: Övergripande navigationsstruktur

#### 📦 Delleveranser
- [ ] DesktopSidebar.razor komponent
- [ ] Kollapserbar funktionalitet (240px ↔ 64px)
- [ ] Navigationsikoner med text
- [ ] Undermenyer för sektioner
- [ ] LocalStorage för kollaps-state

#### ✅ Acceptanskriterier
- [ ] Visas endast på desktop/surfplatta (≥ 960px)
- [ ] Kollapsar/expanderar med animation
- [ ] Undermeny för Turbank (Alla turer, Mina fav., Ny tur)
- [ ] State sparas mellan sidladdningar
- [ ] Hover-tooltip vid kollapsad sidebar

#### ⏱️ Estimat
2 dagar

#### 🔗 Beroenden
- UI-001 (AppLayout.razor)

---

### Issue UI-004: Breakpoint-hantering och CSS-struktur

**Titel**: `[UI Fas 1] Implementera breakpoint-hantering och CSS-struktur`

#### 📋 Beskrivning
Etablera CSS-variabler, breakpoint-system och globala stilar för responsiv design enligt MudBlazor-standarder.

#### 🎯 Krav från specifikation
- Sektion 5.5: Responsiv implementation
- Sektion 8.3: Spacing och layout
- NFR-P04: 60 FPS under interaktioner

#### 📦 Delleveranser
- [ ] CSS-variabler för spacing, färger, layout
- [ ] Breakpoint-tjänst för Blazor-logik
- [ ] Global stylesheet-struktur
- [ ] Utility-klasser för responsiv design

#### ✅ Acceptanskriterier
- [ ] Alla breakpoints fungerar enligt MudBlazor-spec
- [ ] CSS-variabler konsekvent användna
- [ ] Inga FOUC (Flash of Unstyled Content)
- [ ] Performant CSS utan onödiga överspecificeringar

#### ⏱️ Estimat
1 dag

#### 🔗 Beroenden
- UI-001 (AppLayout.razor)

---

### Issue UI-005: Global SearchBar med autocomplete

**Titel**: `[UI Fas 1] Implementera global SearchBar med autocomplete`

#### 📋 Beskrivning
Skapa global sökkomponent som söker över turer, lektioner och kurser med autocomplete-funktionalitet.

#### 🎯 Krav från specifikation
- TB-004: Fritextsökning
- Sektion 3.1: Global sökfunktion
- NFR-P03: Filter/sökning < 200ms

#### 📦 Delleveranser
- [ ] SearchBar.razor komponent
- [ ] Autocomplete med kategoriserade resultat
- [ ] Debounce för söktermer (300ms)
- [ ] Keyboard navigation i resultat
- [ ] Integration med befintliga services

#### ✅ Acceptanskriterier
- [ ] Autocomplete visas efter 2+ tecken
- [ ] Resultat kategoriseras (Turer, Lektioner, Kurser)
- [ ] Sökning < 200ms responstid
- [ ] Keyboard navigation fungerar (↑↓ Enter Esc)
- [ ] Responsivt (kompakt på mobil)

#### ⏱️ Estimat
2 dagar

#### 🔗 Beroenden
- UI-001 (AppLayout.razor)

---

### Issue UI-006: ViewMode toggle (Kompakt/Standard/Utökad)

**Titel**: `[UI Fas 1] Implementera ViewMode toggle`

#### 📋 Beskrivning
Skapa komponent och tjänst för att växla mellan kompakt, standard och utökad vy. Valet ska sparas per enhet i localStorage.

#### 🎯 Krav från specifikation
- UI-004: Växla kompakt/standard/utökad vy
- Sektion 4.5: Kompakt vy (alla enheter)

#### 📦 Delleveranser
- [ ] CompactViewToggle.razor komponent
- [ ] ViewModeService för state management
- [ ] LocalStorage persistens
- [ ] CSS-klasser för varje vyläge

#### ✅ Acceptanskriterier
- [ ] Toggle synlig i toolbar
- [ ] Vyläge sparas i localStorage
- [ ] Automatiskt val baserat på skärmstorlek vid första besök
- [ ] Alla komponenter respekterar vyläge

#### ⏱️ Estimat
1 dag

#### 🔗 Beroenden
- UI-001 (AppLayout.razor)

---

## 🔴 Fas 2: Turbank och pattern-komponenter (2-3 veckor)

### Issue UI-007: PatternCard.razor komponent

**Titel**: `[UI Fas 2] Skapa PatternCard.razor komponent`

#### 📋 Beskrivning
Skapa individuellt kort för visning av turer/övningar med kompakt och expanderad variant.

#### 🎯 Krav från specifikation
- TB-001: Visa lista av turer med kort eller tabellvy
- Sektion 5.2.1: PatternCard specifikation
- Sektion 4.2: Turbank på mobil

#### 📦 Delleveranser
- [ ] PatternCard.razor med props enligt spec
- [ ] Compact variant för mobil
- [ ] Hover-animation och elevation
- [ ] LevelBadge för nivåindikering
- [ ] TagChip-visning

#### ✅ Acceptanskriterier
- [ ] Visar namn, nivå, typ, estimerad tid
- [ ] Taggar visas som chips (max 3)
- [ ] Klickbar för val (om Selectable=true)
- [ ] Animerad hover-effekt (elevation 2→6)
- [ ] Compact mode på mobil

#### ⏱️ Estimat
2 dagar

#### 🔗 Beroenden
- UI-004 (CSS-struktur)

---

### Issue UI-008: PatternList.razor med grid/tabell och virtualisering

**Titel**: `[UI Fas 2] Implementera PatternList.razor med virtualisering`

#### 📋 Beskrivning
Skapa lista-/grid-komponent för turbanken med stöd för virtualisering vid stora dataset.

#### 🎯 Krav från specifikation
- TB-001: Visa lista av turer med kort eller tabellvy
- NFR-P05: Turbank med 500 objekt - smooth scrolling
- Sektion 5.5: Komponentanpassning per breakpoint

#### 📦 Delleveranser
- [ ] PatternList.razor med MudVirtualize
- [ ] Grid-vy (1-4 kolumner responsivt)
- [ ] Tabell-vy för desktop
- [ ] Toggle mellan kort/tabell
- [ ] Lazy loading integration

#### ✅ Acceptanskriterier
- [ ] 500+ objekt scrollar smooth
- [ ] Grid anpassas per breakpoint (1-4 kolumner)
- [ ] Tabellvy tillgänglig på desktop
- [ ] Toggle behåller filter-state
- [ ] Keyboard navigation i lista

#### ⏱️ Estimat
3 dagar

#### 🔗 Beroenden
- UI-007 (PatternCard)

---

### Issue UI-009: PatternFilters.razor med nivå, typ och taggar

**Titel**: `[UI Fas 2] Implementera PatternFilters.razor`

#### 📋 Beskrivning
Skapa filterkomponent för turbanken med stöd för nivå, typ och taggar.

#### 🎯 Krav från specifikation
- TB-002: Filtrera på nivå (Beginner-Advanced)
- TB-003: Filtrera på typ (Pattern/Exercise)
- TB-005: Filtrera på taggar
- NFR-P03: Filter < 200ms

#### 📦 Delleveranser
- [ ] PatternFilters.razor komponent
- [ ] Nivåfilter med chips (horisontell scroll på mobil)
- [ ] Typfilter (dropdown eller chips)
- [ ] Tagg-flerval med autocomplete
- [ ] Clear all filters-knapp

#### ✅ Acceptanskriterier
- [ ] Filter påverkar listan inom 200ms
- [ ] Filter kan kombineras
- [ ] Horisontell scroll för chips på mobil
- [ ] Filter-state persisteras i URL
- [ ] Aktiva filter visas tydligt

#### ⏱️ Estimat
2 dagar

#### 🔗 Beroenden
- UI-008 (PatternList)

---

### Issue UI-010: PatternDetailView.razor (drawer/modal)

**Titel**: `[UI Fas 2] Implementera PatternDetailView.razor`

#### 📋 Beskrivning
Skapa detaljvy för enskild tur med all metadata. Öppnas i drawer på desktop, helskärm på mobil.

#### 🎯 Krav från specifikation
- TB-007: Detaljvy för tur
- TB-009: Visa relaterade turer
- TB-010: Visa progressionsträd
- Sektion 3.3.2: Redigera befintlig tur

#### 📦 Delleveranser
- [ ] PatternDetailView.razor komponent
- [ ] Responsiv drawer/modal
- [ ] Alla metadata-fält (beskrivning, counts, BPM, etc.)
- [ ] Relaterade turer-sektion
- [ ] Media-preview (YouTube, Vimeo)
- [ ] Edit-knapp integration

#### ✅ Acceptanskriterier
- [ ] All metadata visas korrekt
- [ ] Drawer på desktop (≥960px)
- [ ] Helskärm på mobil (<960px)
- [ ] Relaterade turer klickbara
- [ ] Media-preview fungerar för YouTube/Vimeo

#### ⏱️ Estimat
2 dagar

#### 🔗 Beroenden
- UI-007 (PatternCard)

---

### Issue UI-011: PatternQuickAdd.razor (snabbskapande modal)

**Titel**: `[UI Fas 2] Implementera PatternQuickAdd.razor för snabbskapande`

#### 📋 Beskrivning
Skapa snabbformulär för att skapa nya turer med minimal data (<30 sek).

#### 🎯 Krav från specifikation
- TB-006: Snabbskapa tur (modal) - <30 sek
- Sektion 3.3.1: Skapa ny tur (snabbformulär)

#### 📦 Delleveranser
- [ ] PatternQuickAdd.razor modal
- [ ] Minimalt formulär (namn, typ, nivå, counts, tid)
- [ ] Tagg-autocomplete
- [ ] Validering med feedback
- [ ] "Spara och ny"-knapp

#### ✅ Acceptanskriterier
- [ ] Tur skapas med minimal data på <30 sek
- [ ] Validering av obligatoriska fält
- [ ] Inline-felmeddelanden
- [ ] "Spara och stäng" + "Spara och ny"-knappar
- [ ] Keyboard shortcuts (Enter för spara)

#### ⏱️ Estimat
2 dagar

#### 🔗 Beroenden
- UI-010 (PatternDetailView)

---

### Issue UI-012: PatternEditor.razor (fullständigt redigeringsformulär)

**Titel**: `[UI Fas 2] Implementera PatternEditor.razor med all metadata`

#### 📋 Beskrivning
Skapa fullständigt redigeringsformulär för turer med alla metadata-fält.

#### 🎯 Krav från specifikation
- Sektion 3.3.1: Avancerat formulär
- TB-007: Detaljvy för tur (editbar)
- Sektion 3.4: Lägga till information

#### 📦 Delleveranser
- [ ] PatternEditor.razor med alla fält
- [ ] Steg-för-steg-instruktioner (rich text)
- [ ] Förkunskaper (välj från befintliga turer)
- [ ] Relaterade turer-länkning
- [ ] Medialänkar (YouTube, Vimeo, Spotify)
- [ ] Variationer-sektion

#### ✅ Acceptanskriterier
- [ ] Alla metadata-fält från spec implementerade
- [ ] Förkunskaper valda via autocomplete
- [ ] Media-validering (URL-format)
- [ ] Auto-save draft var 30:e sekund
- [ ] Formulär kan nås från quick-add via "Utöka"

#### ⏱️ Estimat
3 dagar

#### 🔗 Beroenden
- UI-011 (PatternQuickAdd)

---

### Issue UI-013: PatternService med API-integration och caching

**Titel**: `[UI Fas 2] Implementera PatternService med caching`

#### 📋 Beskrivning
Skapa service för CRUD-operationer mot turbanks-API med intelligent caching.

#### 🎯 Krav från specifikation
- Sektion 5.3: API-krav
- Sektion 5.4: Datahantering och caching
- NFR-P03: Filter/sökning < 200ms

#### 📦 Delleveranser
- [ ] PatternService.cs med HTTP-klient
- [ ] In-memory cache med 5 min TTL
- [ ] Cache-invalidering vid CRUD
- [ ] Filter/sökning på cachad data
- [ ] Paginering-stöd

#### ✅ Acceptanskriterier
- [ ] Alla CRUD-operationer fungerar
- [ ] Cache returnerar resultat inom 50ms
- [ ] Cache invalideras korrekt vid ändringar
- [ ] Parallella requests hanteras korrekt
- [ ] Error handling med retry-logik

#### ⏱️ Estimat
2 dagar

#### 🔗 Beroenden
- UI-008 till UI-012 (Pattern-komponenter)

---

## 🔴 Fas 3: Lektionsbyggare med dra-och-släpp (3-4 veckor)

### Issue UI-014: LessonBuilder.razor huvudkomponent

**Titel**: `[UI Fas 3] Implementera LessonBuilder.razor huvudkomponent`

#### 📋 Beskrivning
Skapa huvudkomponent för lektionsbyggaren med split-panel layout (turbank + lektionsstruktur).

#### 🎯 Krav från specifikation
- LE-001: Skapa lektion med sektioner (3-8)
- Sektion 5.2.2: LessonBuilder specifikation
- Sektion 3.2.1: Detaljerat flöde - Lektionsbyggare

#### 📦 Delleveranser
- [ ] LessonBuilder.razor med split-layout
- [ ] Grundinformation-panel (namn, datum, längd, nivå)
- [ ] Responsiv layout (horisontell på desktop, vertikal på mobil)
- [ ] Bottom action bar (Avbryt, Spara utkast, Spara)
- [ ] Route-stöd (/lessons/builder, /lessons/{id}/edit)

#### ✅ Acceptanskriterier
- [ ] Split-panel fungerar på desktop
- [ ] Vertikal layout på mobil
- [ ] Grundinfo kan redigeras inline
- [ ] Spara-knappar alltid synliga
- [ ] Stöd för redigering av befintlig lektion

#### ⏱️ Estimat
3 dagar

#### 🔗 Beroenden
- Fas 2 komplett (Turbank-komponenter)

---

### Issue UI-015: LessonSection.razor med drop-zone

**Titel**: `[UI Fas 3] Implementera LessonSection.razor med drop-zone`

#### 📋 Beskrivning
Skapa sektionskomponent med drop-zone för dra-och-släpp av patterns.

#### 🎯 Krav från specifikation
- LE-001: Min 3, max 8 sektioner
- LE-002: Dra-och-släpp turer till sektioner
- Sektion 3.2.1: Sektionstyper (Uppvärmning, Teknik, etc.)

#### 📦 Delleveranser
- [ ] LessonSection.razor komponent
- [ ] Drop-zone med visuell indikering
- [ ] Sektionshuvud (typ, tid, antal patterns)
- [ ] Kollapsbar/expanderbar
- [ ] + Lägg till pattern-knapp

#### ✅ Acceptanskriterier
- [ ] Drop-zone markeras vid drag-over
- [ ] Patterns kan läggas till via drop eller knapp
- [ ] Tid per sektion beräknas automatiskt
- [ ] Sektioner kan kollapsa/expandera
- [ ] Visuell ordningsindikering (1, 2, 3...)

#### ⏱️ Estimat
2 dagar

#### 🔗 Beroenden
- UI-014 (LessonBuilder)

---

### Issue UI-016: SectionPatternPicker.razor (mini-turbank)

**Titel**: `[UI Fas 3] Implementera SectionPatternPicker.razor`

#### 📋 Beskrivning
Skapa mini-turbank för val av patterns till sektioner, integrerad i lektionsbyggaren.

#### 🎯 Krav från specifikation
- Sektion 3.2.1 Steg 3: Integrerad turbanksvy
- Sektion 4.3: Lektionsbyggare på surfplatta

#### 📦 Delleveranser
- [ ] SectionPatternPicker.razor komponent
- [ ] Sökfält med filter
- [ ] Kompakt pattern-lista
- [ ] Val-state (valda patterns markerade)
- [ ] "Visa full turbank"-länk

#### ✅ Acceptanskriterier
- [ ] Snabbsökning i mini-turbank
- [ ] Nivåfilter tillgängligt
- [ ] Valda patterns visas separat
- [ ] Kan navigera till full turbank
- [ ] Fungerar i split-panel på desktop

#### ⏱️ Estimat
2 dagar

#### 🔗 Beroenden
- UI-014 (LessonBuilder)
- UI-009 (PatternFilters)

---

### Issue UI-017: DragDropContainer.razor med MudBlazor integration

**Titel**: `[UI Fas 3] Implementera DragDropContainer.razor med touch-stöd`

#### 📋 Beskrivning
Skapa wrapper-komponent för MudBlazor DropContainer med förbättrat touch-stöd och visuell feedback.

#### 🎯 Krav från specifikation
- LE-002: Dra-och-släpp fungerar på desktop och surfplatta
- NFR-P04: Dra-och-släpp 60 FPS under drag
- Sektion 3.2.1 Steg 3: Dra-och-släpp eller klicka

#### 📦 Delleveranser
- [ ] DragDropContainer.razor wrapper
- [ ] Touch-event handling
- [ ] Visuell drag-preview
- [ ] Drop-zone highlighting
- [ ] Fallback till click-to-add på mobil

#### ✅ Acceptanskriterier
- [ ] Dra-och-släpp fungerar på desktop
- [ ] Touch-drag fungerar på surfplatta
- [ ] Visuell preview under drag
- [ ] 60 FPS under drag-operation
- [ ] Fallback-knapp på mobil

#### ⏱️ Estimat
3 dagar

#### 🔗 Beroenden
- UI-015 (LessonSection)

---

### Issue UI-018: TimeIndicator.razor med real-time beräkning

**Titel**: `[UI Fas 3] Implementera TimeIndicator.razor med validering`

#### 📋 Beskrivning
Skapa komponent för real-time tidsberäkning och visualisering av lektionslängd.

#### 🎯 Krav från specifikation
- LE-003: Real-time tidsberäkning (< 100ms)
- LE-004: Varning vid tidsöverskridning
- Sektion 3.2.1 Steg 3: Real-time tidsberäkning

#### 📦 Delleveranser
- [ ] TimeIndicator.razor komponent
- [ ] Progress bar visualisering
- [ ] Färgkodning (grön/gul/röd)
- [ ] Tooltip med detaljer
- [ ] Animation vid förändring

#### ✅ Acceptanskriterier
- [ ] Uppdateras inom 100ms vid ändring
- [ ] Grön: inom mål, Gul: +5 min, Röd: >+5 min
- [ ] Visar total tid / mål tid
- [ ] Visuell feedback vid överskridning
- [ ] Tooltip visar tid per sektion

#### ⏱️ Estimat
1 dag

#### 🔗 Beroenden
- UI-014 (LessonBuilder)

---

### Issue UI-019: LessonValidationService med förkunskapskontroll

**Titel**: `[UI Fas 3] Implementera LessonValidationService`

#### 📋 Beskrivning
Skapa service för validering av lektioner med tids- och förkunskapskontroll.

#### 🎯 Krav från specifikation
- LE-004: Varning vid tidsöverskridning
- LE-005: Validering av förkunskaper
- Sektion 5.2.2: ValidationService specifikation

#### 📦 Delleveranser
- [ ] LessonValidationService.cs
- [ ] Tidskontroll mot mål ± 5 min
- [ ] Antal-moment-kontroll (3-8 patterns)
- [ ] Förkunskapskontroll (prerequisites)
- [ ] ValidationWarning modell

#### ✅ Acceptanskriterier
- [ ] Varningar genereras för tid >mål+5 min
- [ ] Varningar för <3 eller >8 patterns
- [ ] Förkunskapsvarningar visar saknad pattern
- [ ] Varningar inkluderar lösningsförslag
- [ ] Unit tests för alla valideringsfall

#### ⏱️ Estimat
3 dagar

#### 🔗 Beroenden
- UI-014 (LessonBuilder)

---

### Issue UI-020: LessonTimeline.razor visuell tidslinje

**Titel**: `[UI Fas 3] Implementera LessonTimeline.razor`

#### 📋 Beskrivning
Skapa visuell tidslinje som visar sektioner och patterns proportionellt mot tid.

#### 🎯 Krav från specifikation
- Sektion 5.1: LessonTimeline.razor specifikation
- Sektion 3.2.1 Steg 4: Sammanfattningsvy

#### 📦 Delleveranser
- [ ] LessonTimeline.razor komponent
- [ ] Proportionell tidslinje per sektion
- [ ] Färgkodning per sektionstyp
- [ ] Hover för detaljer
- [ ] Responsiv (horisontell/vertikal)

#### ✅ Acceptanskriterier
- [ ] Sektioner visas proportionellt
- [ ] Färger matchar sektionstyper
- [ ] Hover visar patterns och tid
- [ ] Fungerar på alla skärmstorlekar
- [ ] Interaktiv: klick öppnar sektion

#### ⏱️ Estimat
2 dagar

#### 🔗 Beroenden
- UI-015 (LessonSection)
- UI-018 (TimeIndicator)

---

### Issue UI-021: Auto-save logik med localStorage

**Titel**: `[UI Fas 3] Implementera auto-save för lektionsutkast`

#### 📋 Beskrivning
Implementera auto-save av lektionsutkast till localStorage var 30:e sekund.

#### 🎯 Krav från specifikation
- LE-010: Autosave utkast var 30 sek
- Sektion 5.4: LocalStorage för utkast

#### 📦 Delleveranser
- [ ] DraftService för localStorage-hantering
- [ ] Timer-baserad auto-save (30 sek)
- [ ] Versionering med timestamp
- [ ] Återställ utkast-dialog
- [ ] Rensa utkast vid lyckad spara

#### ✅ Acceptanskriterier
- [ ] Utkast sparas var 30:e sekund
- [ ] Dialog frågar om återställning vid start
- [ ] Utkast rensas efter lyckad API-spara
- [ ] Timestamp visas i UI ("Senast sparad...")
- [ ] Fungerar även vid sidnavigering

#### ⏱️ Estimat
2 dagar

#### 🔗 Beroenden
- UI-014 (LessonBuilder)

---

### Issue UI-022: LessonService med API-integration

**Titel**: `[UI Fas 3] Implementera LessonService med API-integration`

#### 📋 Beskrivning
Skapa service för CRUD-operationer mot lektions-API.

#### 🎯 Krav från specifikation
- Sektion 5.3: API-krav för Lessons
- LE-007: Duplicera lektion

#### 📦 Delleveranser
- [ ] LessonService.cs med HTTP-klient
- [ ] CRUD-operationer
- [ ] Duplicera-funktion
- [ ] Kompakt format-hämtning
- [ ] Validerings-endpoint integration

#### ✅ Acceptanskriterier
- [ ] Alla CRUD-operationer fungerar
- [ ] Duplicera skapar kopia med "(Kopia)" i namn
- [ ] Kompakt format tillgängligt via /compact
- [ ] Server-side validering anropas
- [ ] Error handling med användarvänliga meddelanden

#### ⏱️ Estimat
2 dagar

#### 🔗 Beroenden
- UI-019 (LessonValidationService)

---

## 🟡 Fas 4: Kursplanering och tidslinje (2-3 veckor)

### Issue UI-023: CoursePlanner.razor huvudkomponent

**Titel**: `[UI Fas 4] Implementera CoursePlanner.razor`

#### 📋 Beskrivning
Skapa huvudkomponent för kursplanering med veckovis tidslinje.

#### 🎯 Krav från specifikation
- KU-001: Skapa kurs med 4-12 veckor
- KU-002: Veckovis tidslinje
- KU-003: Koppla lektioner till veckor

#### 📦 Delleveranser
- [ ] CoursePlanner.razor med grundlayout
- [ ] Kursinfo-panel (namn, nivå, veckor)
- [ ] Integration med tidslinje
- [ ] Action bar (exportera, inställningar)

#### ✅ Acceptanskriterier
- [ ] Antal veckor kan ändras (4-12)
- [ ] Kursinfo redigerbar inline
- [ ] Tidslinje integrerad
- [ ] Responsiv layout

#### ⏱️ Estimat
3 dagar

#### 🔗 Beroenden
- Fas 3 komplett

---

### Issue UI-024: CourseTimeline.razor med veckokolumner

**Titel**: `[UI Fas 4] Implementera CourseTimeline.razor`

#### 📋 Beskrivning
Skapa veckovis tidslinje med horisontell scroll och interaktiva veckokort.

#### 🎯 Krav från specifikation
- KU-002: Veckovis tidslinje
- Sektion 5.2.3: CourseTimeline specifikation
- Sektion 4.4: Kursplaneringsvy på desktop

#### 📦 Delleveranser
- [ ] CourseTimeline.razor komponent
- [ ] Horisontell scrollbar
- [ ] WeekCard integration
- [ ] Dra lektioner mellan veckor

#### ✅ Acceptanskriterier
- [ ] Horisontell scroll på mobil
- [ ] Alla veckor synliga (scroll om många)
- [ ] Dra-och-släpp mellan veckor fungerar
- [ ] Visuella indikatorer per vecka

#### ⏱️ Estimat
3 dagar

#### 🔗 Beroenden
- UI-023 (CoursePlanner)

---

### Issue UI-025: WeekCard.razor individuell veckovy

**Titel**: `[UI Fas 4] Implementera WeekCard.razor`

#### 📋 Beskrivning
Skapa kort för individuell vecka med lektion, patterns och status.

#### 🎯 Krav från specifikation
- Sektion 4.4: Kursplaneringsvy (veckokort)
- KU-003: Koppla lektioner till veckor

#### 📦 Delleveranser
- [ ] WeekCard.razor komponent
- [ ] Visa kopplad lektion
- [ ] Pattern-lista i kort
- [ ] Status-ikon (✅, ⚠️, tom)
- [ ] Edit-knapp

#### ✅ Acceptanskriterier
- [ ] Visar veckonummer och tid
- [ ] Patterns listas kompakt
- [ ] Statusikoner korrekt per vecka
- [ ] Klickbar för redigering
- [ ] Drop-zone för lektion

#### ⏱️ Estimat
2 dagar

#### 🔗 Beroenden
- UI-024 (CourseTimeline)

---

### Issue UI-026: CoverageMatrix.razor täckningsanalys

**Titel**: `[UI Fas 4] Implementera CoverageMatrix.razor`

#### 📋 Beskrivning
Skapa widget för visualisering av täckning av fundamentals i kursen.

#### 🎯 Krav från specifikation
- KU-004: Täckningsanalys
- KU-005: Varningar för progression
- Sektion 4.4: Täckningsindikering

#### 📦 Delleveranser
- [ ] CoverageMatrix.razor komponent
- [ ] Progress bars per fundamental
- [ ] Introduktion/senaste vecka-info
- [ ] Färgkodning (grön/gul/röd)

#### ✅ Acceptanskriterier
- [ ] Visar % täckning per fundamental
- [ ] Färgkodning baserat på täckning
- [ ] Introduktions- och senaste-vecka visas
- [ ] Varningar för saknade fundamentals

#### ⏱️ Estimat
2 dagar

#### 🔗 Beroenden
- UI-023 (CoursePlanner)

---

### Issue UI-027: CourseWizard.razor steg-för-steg-guide

**Titel**: `[UI Fas 4] Implementera CourseWizard.razor`

#### 📋 Beskrivning
Skapa steg-för-steg-guide för att skapa nya kurser.

#### 🎯 Krav från specifikation
- KU-006: Kursplaneringsassistent
- Sektion 3.2.1: Steg-för-steg flöde

#### 📦 Delleveranser
- [ ] CourseWizard.razor med steg-indikator
- [ ] Steg 1: Grundinfo (namn, nivå, veckor)
- [ ] Steg 2: Välj/skapa lektioner per vecka
- [ ] Steg 3: Granska och spara
- [ ] Navigation (Bakåt, Framåt, Avsluta)

#### ✅ Acceptanskriterier
- [ ] 3 steg: Info → Lektioner → Granska
- [ ] Stegindikatorn visar progress
- [ ] Kan navigera bakåt utan dataförlust
- [ ] Förslag baserat på nivå
- [ ] Sammanfattning före sparande

#### ⏱️ Estimat
3 dagar

#### 🔗 Beroenden
- UI-023 (CoursePlanner)

---

### Issue UI-028: CourseService med API-integration

**Titel**: `[UI Fas 4] Implementera CourseService`

#### 📋 Beskrivning
Skapa service för CRUD-operationer mot kurs-API med täckning och tidslinje.

#### 🎯 Krav från specifikation
- Sektion 5.3: API-krav för Courses
- KU-007: Exportera kursplan

#### 📦 Delleveranser
- [ ] CourseService.cs med HTTP-klient
- [ ] CRUD-operationer
- [ ] Tidslinje-endpoint (/timeline)
- [ ] Täcknings-endpoint (/coverage)
- [ ] Export-integration

#### ✅ Acceptanskriterier
- [ ] Alla CRUD-operationer fungerar
- [ ] Tidslinje-data hämtas korrekt
- [ ] Täckningsanalys beräknas på server
- [ ] Export genererar PDF/Markdown

#### ⏱️ Estimat
2 dagar

#### 🔗 Beroenden
- UI-023 till UI-027

---

## 🟡 Fas 5: Kompakta vyer och responsiv polish (2-3 veckor)

### Issue UI-029: LessonCompactView.razor

**Titel**: `[UI Fas 5] Implementera LessonCompactView.razor`

#### 📋 Beskrivning
Skapa ultra-kompakt vy för "på golvet"-användning under lektion.

#### 🎯 Krav från specifikation
- LE-006: Kompakt vy för lektion
- Sektion 4.5: Kompakt vy (alla enheter)
- Sektion 4.2: Kompakt vy (mobil)

#### 📦 Delleveranser
- [ ] LessonCompactView.razor komponent
- [ ] Kollapserbara sektioner
- [ ] Minimal styling
- [ ] Print-optimerad CSS
- [ ] Navigation mellan lektioner

#### ✅ Acceptanskriterier
- [ ] All info på en skärm
- [ ] Klick expanderar sektion
- [ ] Utskriftsvy ser bra ut
- [ ] Snabb navigering till nästa/föregående
- [ ] Fungerar offline (service worker framtid)

#### ⏱️ Estimat
2 dagar

#### 🔗 Beroenden
- Fas 3 och 4 komplett

---

### Issue UI-030: PatternCompactCard.razor

**Titel**: `[UI Fas 5] Implementera PatternCompactCard.razor`

#### 📋 Beskrivning
Skapa minimal representation av pattern för kompakta vyer.

#### 🎯 Krav från specifikation
- Sektion 4.5: Kompakt vy för alla enheter
- TB-001: Kortvy variant

#### 📦 Delleveranser
- [ ] PatternCompactCard.razor
- [ ] Endast namn, nivå, tid
- [ ] Klickbar för detaljer
- [ ] Minimal footprint

#### ✅ Acceptanskriterier
- [ ] Visar endast essentiell info
- [ ] Höjd max 40px
- [ ] Klick öppnar detaljvy
- [ ] Används i kompakta listor

#### ⏱️ Estimat
1 dag

#### 🔗 Beroenden
- UI-029 (LessonCompactView)

---

### Issue UI-031: ViewModeService med global state

**Titel**: `[UI Fas 5] Implementera ViewModeService`

#### 📋 Beskrivning
Skapa centraliserad service för vyläges-hantering.

#### 🎯 Krav från specifikation
- UI-004: Växla kompakt/standard/utökad
- Sektion 5.4: AppState specifikation

#### 📦 Delleveranser
- [ ] ViewModeService.cs
- [ ] LocalStorage persistens
- [ ] Enhetsberoende default
- [ ] Event för vyändring

#### ✅ Acceptanskriterier
- [ ] Vyläge sparas per enhet
- [ ] Komponenter reagerar på ändringar
- [ ] Default baserat på skärmstorlek
- [ ] API för att läsa/ändra vyläge

#### ⏱️ Estimat
1 dag

#### 🔗 Beroenden
- UI-006 (ViewMode toggle)

---

### Issue UI-032: Print stylesheet

**Titel**: `[UI Fas 5] Implementera print stylesheet`

#### 📋 Beskrivning
Skapa CSS för utskriftsvänliga vyer av lektioner och kurser.

#### 🎯 Krav från specifikation
- LE-008: Exportera till PDF
- Sektion 4.5: Utskrift

#### 📦 Delleveranser
- [ ] Print.css med @media print
- [ ] Sida-brytning vid sektioner
- [ ] Dölj interaktiva element
- [ ] Optimerad typografi

#### ✅ Acceptanskriterier
- [ ] Print preview ser professionellt ut
- [ ] Sidbrytningar på rätt ställen
- [ ] Inga nav-element skrivs ut
- [ ] QR-kod för länk (valfritt)

#### ⏱️ Estimat
1 dag

#### 🔗 Beroenden
- UI-029 (LessonCompactView)

---

### Issue UI-033: Touch-optimering

**Titel**: `[UI Fas 5] Implementera touch-optimering`

#### 📋 Beskrivning
Förbättra touch-upplevelsen på mobil och surfplatta.

#### 🎯 Krav från specifikation
- UI-009: Touch-vänliga knappar (min 44x44px)
- NFR-P04: 60 FPS under interaktion
- Sektion 4.1: Touch-vänlig princip

#### 📦 Delleveranser
- [ ] Audit av alla interaktiva element
- [ ] CSS för touch-target storlekar
- [ ] Touch-feedback (visuell)
- [ ] Scroll-optimering

#### ✅ Acceptanskriterier
- [ ] Alla knappar minst 44x44px
- [ ] Touch-feedback vid tryck
- [ ] Smooth scrolling på alla listor
- [ ] Ingen accidental tap

#### ⏱️ Estimat
3 dagar

#### 🔗 Beroenden
- Alla UI-komponenter

---

### Issue UI-034: Swipe-gester på mobil

**Titel**: `[UI Fas 5] Implementera swipe-gester`

#### 📋 Beskrivning
Lägg till swipe-gester för snabbåtgärder på mobil.

#### 🎯 Krav från specifikation
- Sektion 4.2: "Swipe left" för snabbåtgärder

#### 📦 Delleveranser
- [ ] Swipe-left för edit/delete
- [ ] Visuell feedback vid swipe
- [ ] Konfigurerbar per komponent
- [ ] Ångra-funktionalitet

#### ✅ Acceptanskriterier
- [ ] Swipe vänster visar åtgärder
- [ ] Smooth animation
- [ ] Åtgärder: Redigera, Ta bort
- [ ] Kan stängas med swipe höger

#### ⏱️ Estimat
2 dagar

#### 🔗 Beroenden
- UI-033 (Touch-optimering)

---

### Issue UI-035: Responsiv final QA

**Titel**: `[UI Fas 5] Responsiv QA och polish`

#### 📋 Beskrivning
Genomgående testning av responsivitet på alla breakpoints.

#### 🎯 Krav från specifikation
- UI-001: Responsiv design
- NFR-P01: Sidladdning < 2 sek

#### 📦 Delleveranser
- [ ] Test på fysiska enheter
- [ ] Fix av upptäckta issues
- [ ] Performance-optimering
- [ ] Dokumentation av kända begränsningar

#### ✅ Acceptanskriterier
- [ ] Fungerar på iPhone, Android
- [ ] Fungerar på iPad, Android tablet
- [ ] Fungerar på desktop (Chrome, Firefox, Safari, Edge)
- [ ] Inga layout-problem vid resize

#### ⏱️ Estimat
2 dagar

#### 🔗 Beroenden
- Alla Fas 5 issues

---

## 🔴 Fas 6: Testning och optimering (1-2 veckor)

### Issue UI-036: Unit tests för services

**Titel**: `[UI Fas 6] Skriv unit tests för nya services`

#### 📋 Beskrivning
Skriv unit tests för alla nya services (PatternService, LessonService, ValidationService, etc.).

#### 🎯 Krav från specifikation
- Sektion 6.2: Unit tests (xUnit)
- Definition of Done: >70% coverage

#### 📦 Delleveranser
- [ ] Tests för PatternService
- [ ] Tests för LessonService
- [ ] Tests för LessonValidationService
- [ ] Tests för CourseService
- [ ] Tests för ViewModeService

#### ✅ Acceptanskriterier
- [ ] >70% code coverage på services
- [ ] Alla edge cases täckta
- [ ] Mockning av HTTP-anrop
- [ ] Alla tests gröna

#### ⏱️ Estimat
2 dagar

#### 🔗 Beroenden
- Alla Fas 1-5 services

---

### Issue UI-037: E2E tests med Playwright

**Titel**: `[UI Fas 6] Skriv E2E tests med Playwright`

#### 📋 Beskrivning
Skriv E2E-tester för huvudflöden enligt specifikationen.

#### 🎯 Krav från specifikation
- Sektion 6.3: E2E tests (Playwright)
- Test: Skapa lektion, spara, exportera

#### 📦 Delleveranser
- [ ] Test: Skapa pattern via snabbformulär
- [ ] Test: Skapa lektion med dra-och-släpp
- [ ] Test: Tidsvalidering i lektionsbyggare
- [ ] Test: Kursplanering och täckning
- [ ] Test: Kompakt vy på mobil

#### ✅ Acceptanskriterier
- [ ] Alla huvudflöden har E2E-test
- [ ] Tests körs i CI/CD
- [ ] Mobilvy testad via viewport
- [ ] Alla tests gröna

#### ⏱️ Estimat
3 dagar

#### 🔗 Beroenden
- Alla Fas 1-5 komponenter

---

### Issue UI-038: Prestandaoptimering

**Titel**: `[UI Fas 6] Prestandaoptimering och profiling`

#### 📋 Beskrivning
Optimera prestanda enligt NFR-krav.

#### 🎯 Krav från specifikation
- NFR-P01: Sidladdning < 2 sek
- NFR-P02: API < 300ms
- NFR-P03: Filter < 200ms
- NFR-P04: 60 FPS

#### 📦 Delleveranser
- [ ] Profiling med browser DevTools
- [ ] Lazy loading av komponenter
- [ ] Optimera re-renders
- [ ] Bundle size-analys

#### ✅ Acceptanskriterier
- [ ] Sidladdning < 2 sekunder
- [ ] Filter/sökning < 200ms
- [ ] Dra-och-släpp 60 FPS
- [ ] Bundle size dokumenterad

#### ⏱️ Estimat
2 dagar

#### 🔗 Beroenden
- Alla Fas 1-5

---

### Issue UI-039: Tillgänglighetsaudit (WCAG 2.1 AA)

**Titel**: `[UI Fas 6] Tillgänglighetsaudit WCAG 2.1 AA`

#### 📋 Beskrivning
Genomför tillgänglighetsaudit enligt WCAG 2.1 AA.

#### 🎯 Krav från specifikation
- NFR-A01: Kontrastförhållande 4.5:1
- NFR-A02: Tangentbordsnavigation
- NFR-A03: ARIA-labels
- NFR-A04: Focus-indikatorer

#### 📦 Delleveranser
- [ ] axe-core scan av alla sidor
- [ ] Manuell tangentbordstest
- [ ] Kontrastkontroll
- [ ] Fix av upptäckta issues

#### ✅ Acceptanskriterier
- [ ] Inga kritiska a11y-fel
- [ ] Kontrast 4.5:1 uppfyllt
- [ ] Tab-ordning logisk
- [ ] ARIA-labels på alla interaktiva element

#### ⏱️ Estimat
2 dagar

#### 🔗 Beroenden
- Alla Fas 1-5

---

### Issue UI-040: Cross-browser testning

**Titel**: `[UI Fas 6] Cross-browser testning`

#### 📋 Beskrivning
Testa applikationen i alla stödda webbläsare.

#### 🎯 Krav från specifikation
- UI-001: Responsiv design
- NFR-SC01: 100 samtidiga användare

#### 📦 Delleveranser
- [ ] Test i Chrome
- [ ] Test i Firefox
- [ ] Test i Safari
- [ ] Test i Edge
- [ ] Fix av webbläsarspecifika issues

#### ✅ Acceptanskriterier
- [ ] Fungerar i Chrome 90+
- [ ] Fungerar i Firefox 90+
- [ ] Fungerar i Safari 14+
- [ ] Fungerar i Edge 90+

#### ⏱️ Estimat
1 dag

#### 🔗 Beroenden
- Alla Fas 1-5

---

### Issue UI-041: Dokumentation

**Titel**: `[UI Fas 6] Komponentdokumentation`

#### 📋 Beskrivning
Dokumentera alla nya komponenter och services.

#### 🎯 Krav från specifikation
- Definition of Done: Dokumentation uppdaterad

#### 📦 Delleveranser
- [ ] README-uppdatering
- [ ] Komponent-API dokumentation
- [ ] Användningsexempel
- [ ] Troubleshooting-sektion

#### ✅ Acceptanskriterier
- [ ] Alla nya komponenter dokumenterade
- [ ] Props och events beskrivna
- [ ] Exempelkod inkluderad
- [ ] README uppdaterad

#### ⏱️ Estimat
1 dag

#### 🔗 Beroenden
- Alla Fas 1-5

---

## 📊 Labels och Milestones

### Labels att använda

| Label | Användning |
|-------|------------|
| `ui-redesign` | Alla issues i detta projekt |
| `phase-1-layout` | Fas 1 issues |
| `phase-2-patterns` | Fas 2 issues |
| `phase-3-lessons` | Fas 3 issues |
| `phase-4-courses` | Fas 4 issues |
| `phase-5-polish` | Fas 5 issues |
| `phase-6-testing` | Fas 6 issues |
| `priority: critical` | Kritiska issues |
| `priority: high` | Högt prioriterade |
| `area: frontend` | Frontend-ändringar |
| `area: api` | API-integration |

### Milestones

| Milestone | Fas | Due (relativt) |
|-----------|-----|----------------|
| **UI Redesign - Phase 1: Layout** | Fas 1 | Vecka 2-3 |
| **UI Redesign - Phase 2: Patterns** | Fas 2 | Vecka 5-6 |
| **UI Redesign - Phase 3: Lessons** | Fas 3 | Vecka 9-10 |
| **UI Redesign - Phase 4: Courses** | Fas 4 | Vecka 12-13 |
| **UI Redesign - Phase 5: Polish** | Fas 5 | Vecka 14-15 |
| **UI Redesign - Phase 6: Release** | Fas 6 | Vecka 16 |

---

## 📝 Issue Creation Checklist

För varje issue, säkerställ:

- [ ] Titel följer format: `[UI Fas X] Beskrivning`
- [ ] Beskrivning kopierad från detta dokument
- [ ] Krav-referenser inkluderade
- [ ] Delleveranser som checkboxar
- [ ] Acceptanskriterier listade
- [ ] Estimat angivet
- [ ] Beroenden länkade
- [ ] Labels tillagda
- [ ] Milestone vald

---

## 📚 Relaterad dokumentation

- [Kravspecifikation-Nytt-Gransnitt.md](./Kravspecifikation-Nytt-Gransnitt.md) - Fullständig kravspecifikation
- [Implementeringsplan-Nytt-Gransnitt.md](./Implementeringsplan-Nytt-Gransnitt.md) - Detaljerad implementeringsplan
- [Guide-Skapa-Issues.md](./Guide-Skapa-Issues.md) - Mall för att skapa issues
- [Issues-Oversikt.md](./Issues-Oversikt.md) - Övrig issue-dokumentation

---

**Dokumentversion**: 1.0  
**Skapad**: 2025-11-29  
**Senast uppdaterad**: 2025-11-29  
**Granskad av**: -  
**Godkänd av**: -
