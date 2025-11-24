# Playwright Integration Tests med WebApplicationFactory och Kestrel

## Översikt

Detta dokument beskriver hur man konfigurerar och kör Playwright-integrationstester som använder den nya Web Application Factory i .NET 10 tillsammans med Kestrel-servern. Denna approach ger mer realistiska integrationstester som körs mot en faktisk webbserver istället för TestServer.

## Arkitektur

### Komponentöversikt

```
┌─────────────────────────────────────────────────────────┐
│  PlaywrightIntegrationTests                             │
│  - Test cases using Playwright + HttpClient             │
└────────────────┬────────────────────────────────────────┘
                 │
                 │ uses
                 ▼
┌─────────────────────────────────────────────────────────┐
│  CustomWebApplicationFactory                            │
│  - Configures test environment                          │
│  - Sets up in-memory database                          │
│  - Seeds test data                                     │
│  - Starts Kestrel on random port                      │
└────────────────┬────────────────────────────────────────┘
                 │
                 │ hosts
                 ▼
┌─────────────────────────────────────────────────────────┐
│  DanceCourseCreator.API (Program)                       │
│  - Actual API application                               │
│  - Running on Kestrel server                           │
│  - Using in-memory database for isolation              │
└─────────────────────────────────────────────────────────┘
```

### Nyckelkomponenter

1. **CustomWebApplicationFactory**: Anpassad factory som konfigurerar testmiljön
   - Använder in-memory databas för isolering mellan tester
   - Startar Kestrel på en slumpmässig port
   - Förbereder testdata (seed data)

2. **PlaywrightIntegrationTests**: Testklassen som innehåller integrationstesterna
   - Använder både Playwright och HttpClient för testning
   - Demonstrerar CRUD-operationer
   - Visar hur man kombinerar UI-tester med API-tester

## Förutsättningar

### Nödvändiga NuGet-paket

Följande paket krävs i testprojektet (`DanceCourseCreator.Tests.E2E.csproj`):

```xml
<ItemGroup>
  <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.6.0" />
  <PackageReference Include="MSTest.TestAdapter" Version="3.0.4" />
  <PackageReference Include="MSTest.TestFramework" Version="3.0.4" />
  <PackageReference Include="Microsoft.Playwright.MSTest" Version="1.40.0" />
  <PackageReference Include="Microsoft.AspNetCore.Mvc.Testing" Version="10.0.0" />
  <PackageReference Include="Microsoft.EntityFrameworkCore.InMemory" Version="10.0.0" />
</ItemGroup>
```

### Projektrelaterade krav

API-projektet måste exponera `Program`-klassen för testning:

```csharp
// I Program.cs, lägg till i slutet av filen:
public partial class Program { }
```

Testprojektet måste ha en projektreference till API-projektet:

```xml
<ItemGroup>
  <ProjectReference Include="..\DanceCourseCreator.API\DanceCourseCreator.API.csproj" />
</ItemGroup>
```

### Installation av Playwright browsers

Innan testerna körs första gången, installera Playwright browsers:

```bash
# Från testprojektets rotkatalog
pwsh bin/Debug/net10.0/playwright.ps1 install

# Eller använd dotnet tool
dotnet tool install --global Microsoft.Playwright.CLI
playwright install
```

## Konfiguration

### CustomWebApplicationFactory

Factory-klassen konfigurerar testmiljön:

```csharp
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Ersätt produktionsdatabasen med in-memory databas
            // Detta säkerställer att tester är isolerade och snabba
            
            // Lägg till testspecifik konfiguration här
        });

        // VIKTIGT: Använd Kestrel istället för TestServer
        builder.UseKestrel();
        builder.UseUrls("http://127.0.0.1:0"); // Port 0 = slumpmässig tillgänglig port
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        // Denna override säkerställer att Kestrel faktiskt startas
        builder.ConfigureWebHost(webHostBuilder => webHostBuilder.UseKestrel());
        var host = builder.Build();
        host.Start();
        return host;
    }
}
```

### Varför Kestrel istället för TestServer?

**TestServer**:
- Kör in-process utan faktisk nätverkskommunikation
- Snabbare men mindre realistiskt
- Kan missa nätverksrelaterade problem

**Kestrel**:
- ✅ Faktisk webbserver som används i produktion
- ✅ Realistisk HTTP-kommunikation över nätverk
- ✅ Testar middleware-pipeline komplett
- ✅ Upptäcker nätverksrelaterade problem
- ✅ Bättre för end-to-end tester med Playwright

## Körning av tester

### Köra alla integrationstester

```bash
# Från repository root
dotnet test src/DanceCourseCreator.Tests.E2E --filter TestCategory=Integration

# Med verbose output
dotnet test src/DanceCourseCreator.Tests.E2E --filter TestCategory=Integration --logger "console;verbosity=detailed"
```

### Köra specifika testkategorier

```bash
# Köra endast Kestrel-tester
dotnet test --filter TestCategory=Kestrel

# Köra endast CRUD-tester
dotnet test --filter TestCategory=CRUD

# Köra endast Playwright + Kestrel tester
dotnet test --filter "TestCategory=Playwright&TestCategory=Kestrel"
```

### Köra ett specifikt test

```bash
dotnet test --filter "FullyQualifiedName~PatternsCRUD_ShouldWorkWithKestrel"
```

### Köra tester med parallell exekvering

```bash
# MSTest stödjer parallell exekvering på klassnivå
dotnet test --parallel
```

## Testexempel

### Grundläggande CRUD-test

```csharp
[TestMethod]
[TestCategory("Integration")]
[TestCategory("CRUD")]
public async Task PatternsCRUD_ShouldWorkWithKestrel()
{
    // CREATE
    var newPattern = new { /* pattern data */ };
    var createResponse = await _httpClient.PostAsJsonAsync("/api/patterns", newPattern);
    Assert.IsTrue(createResponse.IsSuccessStatusCode);
    
    // READ
    var getResponse = await _httpClient.GetAsync("/api/patterns");
    Assert.IsTrue(getResponse.IsSuccessStatusCode);
    
    // UPDATE
    var updateResponse = await _httpClient.PutAsJsonAsync($"/api/patterns/{id}", updatedPattern);
    Assert.IsTrue(updateResponse.IsSuccessStatusCode);
    
    // DELETE
    var deleteResponse = await _httpClient.DeleteAsync($"/api/patterns/{id}");
    Assert.IsTrue(deleteResponse.IsSuccessStatusCode);
}
```

### Playwright + API test

```csharp
[TestMethod]
[TestCategory("Playwright")]
public async Task Playwright_CanAccessKestrelAPI()
{
    // Använd Playwright för att navigera till API:et
    await Page.GotoAsync($"{_serverUrl}/api/health");
    await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    
    var content = await Page.ContentAsync();
    Assert.IsTrue(content.Contains("OK"));
}
```

## Felsökning

### Vanliga problem och lösningar

#### Problem: "Port already in use"

**Lösning**: Factory använder port 0 vilket ger en slumpmässig ledig port. Om problemet kvarstår, kontrollera att gamla testprocesser har stängts:

```bash
# Windows
netstat -ano | findstr :5139
taskkill /PID <process_id> /F

# Linux/Mac
lsof -i :5139
kill -9 <process_id>
```

#### Problem: "Playwright browsers not installed"

**Lösning**: Installera browsers:

```bash
pwsh bin/Debug/net10.0/playwright.ps1 install chromium
```

#### Problem: "Database seeding fails"

**Lösning**: Verifiera att in-memory database konfigureras korrekt. Kontrollera att `UseInMemoryDatabase` anropas innan services byggs.

#### Problem: "Tests timeout"

**Lösning**: Öka timeout i test-attributet:

```csharp
[TestMethod]
[Timeout(60000)] // 60 sekunder
public async Task MyLongRunningTest() { }
```

### Debug-tips

1. **Logga serveradressen**:
```csharp
Console.WriteLine($"Server running at: {_serverUrl}");
```

2. **Inspektera HTTP-responses**:
```csharp
var content = await response.Content.ReadAsStringAsync();
Console.WriteLine($"Response: {content}");
```

3. **Använd Playwright Inspector**:
```bash
# Sätt miljövariabel innan test
$env:PWDEBUG=1
dotnet test --filter MyTestName
```

## Best Practices

### 1. Test Isolation

Varje test ska vara helt isolerat:
- ✅ Använd in-memory databas med unikt namn per factory-instans
- ✅ Rensa state i `[TestCleanup]`
- ✅ Undvik delad state mellan tester

### 2. Testdata

Skapa tydlig och reproducerbar testdata:
- ✅ Seed grundläggande data i factory
- ✅ Skapa testspecifik data i individuella tester
- ✅ Använd beskrivande namn (t.ex. "Test Sugar Push")

### 3. Assertions

Skriv tydliga och informativa assertions:
- ✅ Inkludera felmeddelanden: `Assert.IsTrue(condition, "Helpful message")`
- ✅ Logga viktig information: `Console.WriteLine($"Created ID: {id}")`
- ✅ Verifiera både positiva och negativa scenarion

### 4. Performance

Optimera testhastighet:
- ✅ Använd in-memory databas
- ✅ Återanvänd factory när möjligt
- ✅ Kör oberoende tester parallellt
- ❌ Undvik onödiga `Thread.Sleep()` eller fasta delays

### 5. Underhåll

Gör tester lättunderhållna:
- ✅ Gruppera relaterade tester i samma fil
- ✅ Använd beskrivande testnamn
- ✅ Kommentera komplexa testscenarier
- ✅ Håll tester enkla och fokuserade

## Kontinuerlig Integration (CI)

### GitHub Actions exempel

```yaml
name: Integration Tests

on: [push, pull_request]

jobs:
  test:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '10.0.x'
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Build
      run: dotnet build --no-restore
    
    - name: Install Playwright browsers
      run: pwsh src/DanceCourseCreator.Tests.E2E/bin/Debug/net10.0/playwright.ps1 install chromium
    
    - name: Run integration tests
      run: dotnet test --no-build --filter TestCategory=Integration --logger "trx;LogFileName=test-results.trx"
    
    - name: Upload test results
      if: always()
      uses: actions/upload-artifact@v3
      with:
        name: test-results
        path: '**/test-results.trx'
```

## Referenser

- [Microsoft Docs: Integration tests in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-10.0)
- [Microsoft Docs: Using WebApplicationFactory with Playwright](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-10.0)
- [Playwright .NET Documentation](https://playwright.dev/dotnet/)
- [MSTest Documentation](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest)

## Sammanfattning

Denna implementation visar:
- ✅ Användning av Web Application Factory i .NET 10
- ✅ Konfiguration för att köra mot Kestrel istället för TestServer
- ✅ Komplett CRUD-test exempel mot Patterns API
- ✅ Integration av Playwright för end-to-end testing
- ✅ Best practices för testisolation och underhåll
- ✅ Dokumentation för konfiguration och körning

Med denna approach får du robusta integrationstester som körs mot en realistisk hostingmiljö och är framtidssäkrade för produktion.
