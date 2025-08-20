# Kravspecifikation: Applikation för kursmaterialskapande i West Coast Swing

Version: 1.0  
Datum: 2025-08-20  
Ägare: Produktteamet / Instruktörsråd

## 1. Syfte och mål
- Syfte: Underlätta för instruktörer att planera, strukturera och dela lektions- och kursplaner för WCS genom att välja och kombinera turer/övningar per kurstillfälle, med stöd för progression, metadata och export.
- Mål:
  - Minska planeringstid och öka kvalitet/konsekvens.
  - Säkerställa pedagogisk progression och adekvat svårighetsgrad.
  - Möjliggöra återanvändning av mallar och kunskapsdelning mellan instruktörer.

## 2. Målgrupp
- Primär: Danslärare/instruktörer (ensam eller i team).
- Sekundär: Kurskoordinatorer, dansskolor/klubbar.
- Eventuell: Assisterande lärare, utbildningsansvariga.

## 3. Omfattning och avgränsningar
- Inom scope: Bibliotek av WCS-turer/övningar, lektionsbyggare per kurstillfälle, kursplan (flertalet tillfällen), export/delning, metadata, progression/rekommendationer, mallar, versionering.
- Utanför initial scope: Elevhantering, betalningar, anmälningar, fullständigt LMS (kan övervägas senare).

## 4. Definitioner (WCS-specifika)
- Turer/Patterns: Ex. Sugar Push, Left/Right Side Pass, Whip, Variationer.
- Övningar: Teknikmoment (connection, anchor, stretch, timing, footwork).
- Slot-orientering, leverage/compression, anchor step: Grundprinciper i WCS.
- Nivåer: Beginner, Improver, Intermediate, Advanced (konfigurerbart).
- Taggar: Teknikfokus, tema, takt/tempo (BPM), handfattningar, rotationer, förkunskaper.

## 5. Roller och behörighet
- Instruktör: Skapa/ändra turer/övningar, lektioner, kurser, mallar. Dela/exportera.
- Redaktör (valfritt): Bidra och redigera inom team/organisation.
- Läsare: Se och exportera delade planer, ej redigera.
- Admin: Hantera användare, team, roller, globala inställningar.

## 6. Centrala användningsfall
- UC1: Skapa och underhålla bibliotek av turer/övningar med rik metadata.
- UC2: Bygga lektionsplan för ett kurstillfälle (3–8 moment) med sektioner, tider och struktur.
- UC3: Skapa kursplan (serie av tillfällen) med progression och täckning av grundblock.
- UC4: Få rekommendationer baserat på nivå, förkunskaper och tidigare innehåll.
- UC5: Exportera plan till PDF/Markdown/HTML; dela länk med kollegor.
- UC6: Versionera och återställa planer; skapa och använda mallar; duplicera tillfällen.
- UC7: Länka media (video), musikrekommendationer (BPM), notiser och vanliga fel.
- UC8: Team-samarbete: kommentera, föreslå ändringar, godkänna.

## 7. Funktionella krav (FR)

### 7.1 Bibliotek och metadata
- FR-001: Skapa/uppdatera/radera turer/övningar.
- FR-002: Metadata per objekt: namn, alias, nivå, typ (tur/övning), beskrivning, steg-för-steg, counts (1&2, 3&4, 5&6…), handfattningar, slot-riktning, rotationer, förkunskaper, relaterade turer, undervisningspunkter, vanliga fel, variationer, estimerad tid, rekommenderat BPM-intervall, media-/musiklänkar, taggar.
- FR-003: Kopplingar: relaterade turer och progressionsträd (ex: Sugar Push → Sugar Tuck → Tuck Turn Variation).
- FR-004: Sök/filtrering på nivå, taggar, typ, BPM, längd, uppfyllda/ej uppfyllda förkunskaper.

### 7.2 Lektionsbyggare (per tillfälle)
- FR-010: Sektioner: uppvärmning, teknik, mönster, kombination, repetition, socialdansfokus.
- FR-011: 3–8 moment per lektion med estimerad tid och summerad lektionslängd (ex. 60/75/90 min).
- FR-012: Validering: varna om förkunskaper ej uppfyllda, fel nivå, tidskonflikt.
- FR-013: Progressionshjälp: föreslå turer baserat på tidigare tillfällen och mål.
- FR-014: Styrd slump: välj X turer inom taggar/nivå, undvik repetitioner inom Y veckor.
- FR-015: Notatfält: lärarpunkter, vanliga fel, övningsupplägg, musikförslag.

### 7.3 Kursplan (serie)
- FR-020: Skapa kurs (ex: 8 veckor), definiera mål, nivå, tempo, teman per vecka.
- FR-021: Översikt över täckning av grundblock (pass/whip/sugar push, connection, anchor, stretch, musicality).
- FR-022: Repetitionsplanering (spaced repetition) av nyckelkoncept.
- FR-023: Konfliktvarningar: överbelastning, för snabb progression, saknade fundamentals.

### 7.4 Mallar och versionering
- FR-030: Skapa lektions- och kursmallar. Duplicera från mallar.
- FR-031: Versionera planer med historik och återställning.
- FR-032: Kommentera och föreslå ändringar (review-läge) i team.

### 7.5 Media och resurser
- FR-040: Bifoga video-/bildlänkar (YouTube/Vimeo/Drive); ev. egen uppladdning senare.
- FR-041: Länka musikförslag med BPM och stilkänsla (blues/soul/pop); stöd för spellistor.

### 7.6 Export och delning
- FR-050: Export av lektion/kurs till PDF/Markdown/HTML.
- FR-051: Delbar läslänk (offentlig/privat, tidsbegränsning).
- FR-052: Utskriftvänligt, kompakt läge.

### 7.7 Import och interoperabilitet
- FR-060: Importera turer/övningar från CSV/JSON/Markdown.
- FR-061: API för läsning av bibliotek och planer (framtid).

### 7.8 Behörighet och team
- FR-070: Organisations-/teamstruktur; dela bibliotek och mallar inom team.
- FR-071: Åtkomstnivåer: ägare, redaktör, läsare.

### 7.9 Rapporter och insikter
- FR-080: Täckningsgrad per kurs: vilka koncept/turer har behandlats.
- FR-081: Repetitionsstatistik; användningsfrekvens per tur/övning.
- FR-082: Kurssammanställning för annonsering (mål, innehåll, nivå).

## 8. Icke-funktionella krav (NFR)
- NFR-001 Prestanda: Sök/filtrering < 300 ms för bibliotek ≤ 5 000 objekt.
- NFR-002 Tillgänglighet: WCAG 2.1 AA där rimligt.
- NFR-003 Säkerhet: OAuth2/SSO (valfritt), 2FA-stöd, rollbaserad åtkomst, kryptering i transit och i vila för privata data.
- NFR-004 Driftsäkerhet: 99,5% månadsvis för molnvariant.
- NFR-005 Portabilitet: Responsiv webapp (desktop, surfplatta, mobil). PWA (offline-läsning) som “could”.
- NFR-006 Internationellt stöd: UI och innehåll minst svenska/engelska.
- NFR-007 Skalbarhet: Minst 100 samtidiga användare/team utan märkbar degradering.
- NFR-008 Loggning/spårbarhet: Ändringslogg för objekt och planer.

## 9. Datamodell (översikt)
- User(id, name, email, role, teamId)
- Team(id, name, members[])
- PatternOrExercise(
  id, type, name, aliases[], level, description, steps[], counts, holds[],
  slot, rotations, prerequisites[], related[], teachingPoints[],
  commonMistakes[], variations[], estimatedMinutes, bpmRange, tags[],
  mediaLinks[], createdBy, updatedAt
)
- Lesson(
  id, courseId?, date?, duration, sections[{type, items[], notes}],
  items[PatternRef|ExerciseRef + overrides], totalEstimatedMinutes,
  notes, version, createdBy, reviewers[], history[]
)
- Course(
  id, name, level, durationWeeks, goals[], themesByWeek[], lessons[],
  coverageMetrics, repetitionPlan
)
- Template(id, scope: lesson|course, content, owner/team)
- ShareLink(id, resourceId, type, visibility, expiresAt, token)

## 10. Affärsregler och valideringar
- BR-001: En lektion bör innehålla 3–8 moment; varning om utanför spann.
- BR-002: Förkunskaper måste vara uppfyllda eller manuellt bekräftade.
- BR-003: Total lektionslängd får ej överskrida angiven tid utan varning.
- BR-004: Fundamentals bör täckas inom de första 2–3 veckorna på beginner/improver.
- BR-005: Återupprepning av exakt samma tur > 2 gånger i rad varnas.

## 11. UI/UX-krav
- Biblioteksvy med snabbfilter, nivåchips/taggar, förkunskapsindikatorer.
- Dra-och-släpp till lektionssektioner; tidsindikator och varningar i realtid.
- Kursplan-tidslinje vecka-för-vecka; drag-n-drop mellan veckor; täckningsindikatorer.
- Redigeringsvy för tur/övning med förhandsvisning av export.
- Kompakt utskriftsvy för “på golvet”-användning.

## 12. Integrationer (nu eller senare)
- Inloggning: Google/Microsoft/Apple (SSO). E-post/lösen som grund.
- Media: YouTube/Vimeo-länkar; molnlagring (Drive/Dropbox) via länk.
- Export: PDF-rendering, Markdown-nedladdning.

## 13. Säkerhet och integritet
- Privata bibliotek/kurser som standard; explicit delning krävs.
- Åtkomstlogg per resurs. Versionshistorik (vem, vad, när).
- Dataexport per användare/team (GDPR-portabilitet).

## 14. Acceptanskriterier (exempel)
- AC-001: Instruktör kan skapa 10 turer med metadata och hitta dem via filter på nivå och tagg inom 2 sekunder.
- AC-002: Instruktör kan bygga en 75-minuterslektion med 5 moment; systemet visar total tid och varnar vid 80+ min.
- AC-003: Systemet föreslår minst 3 relevanta turer när nivå=Improver och senaste lektionen innehöll “Sugar Push”.
- AC-004: Export av lektionsplan till PDF innehåller rubriker, sektioner, punkter, tidsangivelser och noter.
- AC-005: Kursplan över 8 veckor visar “coverage” för fundamentals och varnar om Whip saknas efter vecka 4 (på Improver+).

## 15. Prioritering (MoSCoW)
- Must: FR-001..004, FR-010..015, FR-020..023, FR-030..032, FR-050..052.
- Should: FR-040..041, FR-060..061, FR-070..071, NFR-001..004, NFR-006.
- Could: FR-014, FR-080..082, NFR-005 (PWA), fler integrationer.
- Won’t (initialt): Elevhantering, betalningar, fullständigt LMS.

## 16. Teknisk riktning (förslag)
- Klient: Responsiv webbapp (SPA/SSR). PWA som “could”.
- Backend: REST/GraphQL API; relationell DB enligt datamodell.
- Auth: JWT/OAuth2; rollbaserad accesskontroll.
- Export: Tjänst för PDF/Markdown; server-side render för utskrifter.

## 17. Migrering och frödata
- Fröbibliotek (exempel):
  - Sugar Push, Left Side Pass, Right Side Pass, Whip (basic)
  - Tuck Turn, Inside/Outside Turn
  - Anchor-övningar, Stretch/Leverage-övningar
  - Connection-drills, Timing/Footwork-drills

## 18. Risker och antaganden
- Variation i terminologi mellan skolor — hanteras med alias/taggar.
- Upphovsrätt på media — initialt enbart länkning till källor.
- Hög granularitet i metadata kräver smidiga formulär, mallar och snabbduplicering.

## 19. Öppna frågor
- Nivåbeteckningar (svenska/engelska, hur många nivåer)?
- Viktigaste exportformat (PDF, Markdown, HTML) och vilka exportmallar?
- Teamfunktionalitet från start eller räcker “single instructor”?
- Privat app i början eller behövs delningslänkar direkt?
- Finns befintligt material att importera (CSV/JSON/Markdown)?
- Behov av offline-stöd (PWA) för lektionsläsning på plats?
- Föredragen inloggning (SSO vs e-post/lösen)?

---

# Exempelmallar

Nedan följer konkreta mallar att kopiera/återanvända i appen eller vid export.

## A. Exempelmall: Lektion (75 min)

```markdown
# [Kursnamn] – Lektion [Vecka/Nummer]
Nivå: Beginner/Improver/Intermediate/Advanced
Datum: YYYY-MM-DD
Total tid: 75 min

## Mål för lektionen
- [ ] (Ex) Förstärka anchor-teknik och stretch
- [ ] Introducera [tur]: [ex. Left Side Pass Variation]
- [ ] Socialdansfokus: tydlig slot och connection

## Innehåll och tidsplan
1) Uppvärmning & Teknik (10 min)
   - Övning: Connection drill (leverage/compression)
   - Lärarpunkter: ram, andning, viktöverföring
   - Vanliga fel: för tidig egenambition, saknad stretch

2) Grundmönster/Repetition (10 min)
   - Sugar Push – kvalitet och timing (1&2, 3&4, 5&6)
   - Fokus: anchor, avslappnad arm, tydlig “stretch innan gå”

3) Nytt mönster 1 (15 min)
   - Left Side Pass
   - Förkunskaper: Sugar Push
   - Undervisningspunkter: slot, travel, handfattning
   - Vanliga fel: sidosteg ut ur slot, uppdriven arm

4) Nytt mönster 2 (15 min)
   - Right Side Pass
   - Förkunskaper: Sugar Push
   - Lärarpunkter: riktning, connection på 2/4
   - Variation: handbyte på 4

5) Kombinationsarbete (15 min)
   - Sekvens: Sugar Push → LSP → RSP
   - Fokus: timing, tydlig lead/follow, mjukt tempo (90–100 BPM)

6) Repetition & Social tillämpning (10 min)
   - Parrotation: använd sekvensen med 2 olika låtar
   - Musiktips: [Låt A – 94 BPM], [Låt B – 98 BPM]

## Noter
- Påminn om anchor-koordination med andning.
- Variera musiktempo för att känna stretch.

## Media
- Video: [YouTube-länk – Left/Right Side Pass tutorial]
- Spellista: [Spotify-länk – 90–105 BPM WCS]

## Checklista
- [ ] Förkunskaper uppfyllda (Sugar Push)
- [ ] Tidsram håller (±5 min)
- [ ] Export klar (PDF/MD)
```

## B. Exempelmall: Lektion (90 min, Intermediate)

```markdown
# [Kursnamn] – Lektion [Vecka/Nummer]
Nivå: Intermediate
Datum: YYYY-MM-DD
Total tid: 90 min

## Tema
- Whip-kvalitet och variationer
- Stretch & delay för musikalitet

## Tidsplan
- Teknikblock: 15 min (stretch/elasticity, riktad frame)
- Whip (basic) refresher: 10 min
- Whip inside roll: 15 min
- Whip outside roll: 15 min
- Kombinationsarbete: 20 min (Basic → Inside → Outside → Accent)
- Socialdansövning: 10 min (call & response till musiken)
- Q&A/Reflektion: 5 min

## Lärarpunkter
- Riktad rotation i 5–6
- Kontrollerat tempo i båge
- Ögonkontakt/kommunikation

## Vanliga fel
- Tappad slot vid rotation
- För hård hand/arm
- Missad förberedelse inför 4

## Förkunskaper
- Stabil anchor, Side Passes, Sugar Push, grundläggande Whip

## Media
- Video: [Vimeo-länk – Whip variations]
- Musik: [Spellista – 95–110 BPM]

## Utvärdering
- [ ] Deltagarna kan dansa Whip med 1 valfri variation
- [ ] Förbättrad stretch-känsla i 3–4
```

## C. Exempelmall: Kurs (8 veckor, Beginner → Improver)

```markdown
# Kursplan: West Coast Swing – 8 veckor
Nivå: Beginner → Improver
Mål: Grundläggande byggblock, trygg socialdans i moderat tempo

## Övergripande mål
- Fundamentals: Anchor, Slot, Connection
- Grundmönster: Sugar Push, L/R Side Pass, Whip
- Teknik: Stretch, frame, timing
- Socialdansfokus: etikett, anpassning, musikval

## Vecka-för-vecka

### V1 – Introduktion & Fundamentals
- Teknik: Posture, frame, connection (leverage/compression)
- Mönster: Sugar Push
- Musik: 88–96 BPM
- Hemuppgift: Lyssna på 2 låtar, klappa 1&2, 3&4, 5&6

### V2 – Side Passes
- Mönster: Left Side Pass, Right Side Pass
- Teknik: Anchor-kvalitet
- Repetition: Sugar Push
- Musik: 90–100 BPM

### V3 – Whip (introduktion)
- Mönster: Whip basic
- Teknik: Kontrollerad rotation, slot-integritet
- Repetition: Side Passes

### V4 – Variationer & Sekvenser
- Variationer: Handbyte i pass, små accenter
- Sekvens: SP → LSP → RSP → Whip
- Socialdansövning: rotation till 2 låtar

### V5 – Fördjupning i teknik
- Teknik: Timing/delay, stretch innan release
- Mönster: Sugar Tuck (om redo)
- Repetition: Whip

### V6 – Musicality light
- Tema: Fraslyssning, enkel accentuering
- Mönster: Tuck turn variation
- Repetition: Grundmönster

### V7 – Konsolidering
- Kombinationsarbete: bygga egen sekvens i par
- Teknik: transitions, säker handfattning
- Socialdans: feedback-rundor

### V8 – Repetition & Examenssocial
- Repetition av alla fundamentals
- Mini-uppvisning i smågrupper
- Socialdans: 3–4 låtar, valfria accenter

## Täckningsmatris (fundamentals)
- Anchor: V1–V3, V5, V7–V8
- Connection: V1–V2, V5
- Slot: V1–V4
- Whip: V3–V5, V7–V8

## Utvärderingskriterier
- [ ] Deltagare kan dansa SP, LSP, RSP, Whip i komforttempo
- [ ] Visar grundläggande stretch och anchor-kvalitet
- [ ] Kan anpassa sig till partner och musik

## Media & resurser
- Video: Grundmönster-playlist (YouTube)
- Musik: 88–105 BPM spellista
```

## D. Exempel på export: Lektionsplan (kompakt Markdown)

```markdown
# Lektion 3 (75 min) – Improver
- Mål: Förfina anchor & introducera RSP-variation
- Innehåll:
  - Teknik (10): Stretch drill
  - Repetition (10): Sugar Push
  - Nytt (15): Left Side Pass
  - Nytt (15): Right Side Pass (handbyte)
  - Kombi (15): SP → LSP → RSP
  - Social (10): Rotation med 2 låtar (92–98 BPM)
- Noter: Mjuk arm, tydlig slot
- Media: [Video], [Spellista]
```

## E. Exempel: Biblioteksobjekt (tur/övning) – fältguide

```markdown
Namn: Left Side Pass
Typ: Tur
Nivå: Beginner
Counts: 1&2, 3&4, 5&6
Handoff/Handfattning: RH-LH (start), ev. handbyte på 4
Slot: Rak, follower passerar på vänster sida
Förkunskaper: Sugar Push
Relaterade: Right Side Pass, Sugar Push
Undervisningspunkter: Travel i slot, förberedelse på 2, lätthet i arm
Vanliga fel: Avviker från slot, “drag” istället för bjudning
BPM: 88–104
Taggar: fundamentals, pass, slot
Media: [YouTube-länk]
Estimerad tid vid undervisning: 10–15 min
```

---

# Checklista inför implementation (sammanfattning)
- [ ] Bibliotek med metadata (FR-001..004)
- [ ] Lektionsbyggare med tider, sektioner och validering (FR-010..015)
- [ ] Kursplan med progression och täckning (FR-020..023)
- [ ] Mallar, versionering, kommentarer (FR-030..032)
- [ ] Export och delning (FR-050..052)
- [ ] Import (FR-060) och teambehörighet (FR-070..071)
- [ ] Rapporter/insikter (FR-080..082)
- [ ] NFR: Prestanda, säkerhet, tillgänglighet, skalbarhet
