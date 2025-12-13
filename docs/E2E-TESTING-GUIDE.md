# Running E2E Tests - Comprehensive Guide

This guide explains different ways to run the Playwright E2E tests for Dance Course Creator.

## 🎯 Overview

The E2E tests can run in two modes:

1. **Manual Mode** (Default) - You start the API and Web apps manually
2. **WebApplicationFactory Mode** - Tests automatically start the apps using `WebApplicationFactory`

## 🚀 Quick Start

### Option 1: Manual Mode (Recommended for Development)

This is the simplest approach for day-to-day development.

**Step 1: Start the applications**

Terminal 1 - Start API:
```bash
cd src/DanceCourseCreator.API
dotnet run --urls "http://localhost:5139"
```

Terminal 2 - Start Web:
```bash
cd src/DanceCourseCreator.Web
dotnet run --urls "http://localhost:5034"
```

**Step 2: Run the tests**

Terminal 3:
```bash
cd src/DanceCourseCreator.Tests.E2E

# Run all E2E tests
dotnet run

# Or run specific categories
dotnet test --filter "TestCategory=Navigation"
dotnet test --filter "TestCategory=Courses"
```

### Option 2: WebApplicationFactory Mode (CI/CD)

This mode automatically starts the applications using `WebApplicationFactory`. Best for CI/CD pipelines.

**Run tests with factory:**
```bash
cd src/DanceCourseCreator.Tests.E2E

# Set environment variable to enable factory mode
$env:UseWebApplicationFactory="true"
dotnet run

# Or with dotnet test
dotnet test -- TestRunParameters.Parameter(name=\"UseWebApplicationFactory\", value=\"true\")
```

## 📋 Prerequisites

### First Time Setup

1. **Install Playwright Browsers**:
   ```bash
   cd src/DanceCourseCreator.Tests.E2E
   dotnet build
   pwsh bin/Debug/net10.0/playwright.ps1 install chromium
   ```

2. **Verify .NET 10 SDK**:
   ```bash
   dotnet --version  # Should be 10.0.x or later
   ```

## 🔧 Test Modes Explained

### Manual Mode (Default)

**Advantages:**
- ✅ Full application startup with normal configuration
- ✅ Can debug applications easily
- ✅ See real-time logs from API and Web
- ✅ Faster test execution (apps stay running)
- ✅ Better for iterative development

**How it works:**
1. You start API on port 5139
2. You start Web on port 5034  
3. Tests connect to these running instances
4. Tests use Playwright to automate the browser

**When to use:**
- Local development
- Debugging test failures
- Exploring the application while tests run
- When you want to keep apps running between test runs

### WebApplicationFactory Mode

**Advantages:**
- ✅ No manual app startup needed
- ✅ Automatic cleanup after tests
- ✅ Isolated test database
- ✅ Perfect for CI/CD pipelines
- ✅ Tests are fully self-contained

**How it works:**
1. `WebApplicationFactory` starts API with Kestrel on port 5139
2. Tests create a temporary SQLite database
3. Database is seeded with test data
4. Tests run against the factory-hosted app
5. Everything cleans up automatically

**When to use:**
- CI/CD pipelines (GitHub Actions, Azure DevOps)
- Automated test runs without manual intervention
- When you want guaranteed cleanup
- Integration testing with API only

## 🎯 Running Tests

### By Category

```bash
# Navigation tests
dotnet test --filter "TestCategory=Navigation"

# Pattern/Turbank tests
dotnet test --filter "TestCategory=Patterns"

# Course management tests
dotnet test --filter "TestCategory=Courses"

# Lesson tests
dotnet test --filter "TestCategory=Lessons"

# Template tests
dotnet test --filter "TestCategory=Templates"

# Integration tests (use WebApplicationFactory automatically)
dotnet test --filter "TestCategory=Integration"

# All tests with screenshots
dotnet test --filter "TestCategory=Screenshots"
```

### By Test Class

```bash
# Course creation tests
dotnet test --filter "FullyQualifiedName~CourseCreationTests"

# Course editing tests
dotnet test --filter "FullyQualifiedName~CourseEditingTests"

# Home and navigation tests
dotnet test --filter "FullyQualifiedName~HomeAndNavigationTests"
```

### Individual Tests

```bash
# Run specific test method
dotnet test --filter "FullyQualifiedName~CourseCreation_OpenCreateDialog_ShouldDisplayForm"
```

### With Modern MSTest Runner

```bash
# Run all tests
cd src/DanceCourseCreator.Tests.E2E
dotnet run

# Run with Release configuration
dotnet run --configuration Release

# Pass arguments to MSTest runner
dotnet run -- --help
```

## 🏗️ Understanding the Architecture

### Test Infrastructure

```
DanceCourseCreator.Tests.E2E/
├── Infrastructure/
│   ├── CustomWebApplicationFactory.cs          # API-only factory
│   ├── PlaywrightWebApplicationFactory.cs      # Full-stack factory (API + Web)
│   └── PlaywrightE2ETestBase.cs               # Base class for E2E tests
├── HomeAndNavigationTests.cs                   # UI tests (Manual mode)
├── CourseCreationTests.cs                      # UI tests (Manual mode)
├── CourseEditingTests.cs                       # UI tests (Manual mode)
├── TurbankTests.cs                             # UI tests (Manual mode)
├── LessonAndTemplateTests.cs                   # UI tests (Manual mode)
├── PlaywrightIntegrationTests.cs              # API tests (Factory mode)
└── DemoLoginSmokeTests.cs                      # Smoke tests (Manual mode)
```

### Test Types

#### 1. UI Tests (Manual Mode Default)
- Test the complete UI workflow
- Require both API and Web applications running
- Capture screenshots for documentation
- Examples: `CourseCreationTests`, `HomeAndNavigationTests`

#### 2. Integration Tests (Factory Mode)
- Test API endpoints directly
- Use `WebApplicationFactory` automatically
- Don't require Web application
- Examples: `PlaywrightIntegrationTests`

## 📊 Test Execution Matrix

| Test Class | Default Mode | Can Use Factory? | Requires Web? |
|------------|-------------|------------------|---------------|
| HomeAndNavigationTests | Manual | ❌ | ✅ |
| CourseCreationTests | Manual | ❌ | ✅ |
| CourseEditingTests | Manual | ❌ | ✅ |
| TurbankTests | Manual | ❌ | ✅ |
| LessonAndTemplateTests | Manual | ❌ | ✅ |
| DemoLoginSmokeTests | Manual | ❌ | ✅ |
| PlaywrightIntegrationTests | Factory | ✅ | ❌ |

## 🔍 Debugging Tests

### In Visual Studio

1. Open Test Explorer (Test → Test Explorer)
2. Right-click test → Debug
3. Set breakpoints in test code
4. Use Debug Console for output

### In VS Code

1. Install C# Dev Kit extension
2. Click "Debug Test" above test method
3. Set breakpoints in test code
4. Use Debug Console

### With Manual Mode

When apps are running manually, you can:
- See application logs in their terminal windows
- Use browser DevTools (Playwright runs headed browser with `HEADED=1`)
- Refresh pages manually to see current state
- Use API Swagger UI (http://localhost:5139/swagger)

### Headed Browser Mode

Run Playwright in headed mode to see the browser:

**PowerShell:**
```powershell
$env:HEADED="1"
dotnet test --filter "FullyQualifiedName~CourseCreation"
```

**Bash:**
```bash
HEADED=1 dotnet test --filter "FullyQualifiedName~CourseCreation"
```

### Slow Motion

Run tests in slow motion to see actions:

**PowerShell:**
```powershell
$env:SLOW_MO="1000"  # 1 second delay between actions
dotnet test --filter "FullyQualifiedName~CourseCreation"
```

## 🚨 Troubleshooting

### Problem: Tests fail with "Connection refused"

**Solution:**
```bash
# Verify apps are running and accessible
curl http://localhost:5139/api/health  # API
curl http://localhost:5034             # Web

# Check if ports are in use
netstat -an | findstr "5139"
netstat -an | findstr "5034"

# Kill any stuck processes
# PowerShell:
Get-Process | Where-Object {$_.Name -like "*DanceCourseCreator*"} | Stop-Process
```

### Problem: Playwright browser not found

**Solution:**
```bash
cd src/DanceCourseCreator.Tests.E2E
pwsh bin/Debug/net10.0/playwright.ps1 install chromium --force
```

### Problem: Tests timeout waiting for elements

**Solution:**
```bash
# Increase timeout in test code
await Page.WaitForSelectorAsync("selector", new() { Timeout = 60000 });

# Or ensure applications have fully started
# Wait 5-10 seconds after starting apps before running tests
```

### Problem: WebApplicationFactory can't start Web app

**Current Limitation:**
The `PlaywrightWebApplicationFactory` currently only starts the API. For UI tests, you must start the Web application manually.

**Workaround:**
Use Manual Mode for UI tests:
```bash
# Terminal 1 - API
cd src/DanceCourseCreator.API
dotnet run --urls "http://localhost:5139"

# Terminal 2 - Web
cd src/DanceCourseCreator.Web
dotnet run --urls "http://localhost:5034"

# Terminal 3 - Tests
cd src/DanceCourseCreator.Tests.E2E
dotnet run
```

### Problem: Database locked errors

**Solution:**
```bash
# Clean up test databases
Remove-Item $env:TEMP\e2e_test_*.db
Remove-Item $env:TEMP\test_*.db

# Ensure only one test run at a time
# Don't run tests in parallel for E2E tests
```

## 📈 Performance Tips

### 1. Keep Apps Running (Manual Mode)

Instead of restarting apps for each test run:
```bash
# Start once
cd src/DanceCourseCreator.API
dotnet run --urls "http://localhost:5139"

# Keep running, run tests multiple times
cd src/DanceCourseCreator.Tests.E2E
dotnet test --filter "TestCategory=Courses"
# ... fix issues ...
dotnet test --filter "TestCategory=Courses"
```

### 2. Run Specific Categories

Don't run all tests during development:
```bash
# Fast smoke tests
dotnet test --filter "TestCategory=Smoke"

# Only the feature you're working on
dotnet test --filter "TestCategory=Courses"
```

### 3. Skip Screenshot Tests

Screenshots slow down tests:
```bash
# Run without screenshot tests
dotnet test --filter "TestCategory!=Screenshots"
```

### 4. Use Modern MSTest Runner

Faster than traditional `dotnet test`:
```bash
dotnet run --project src/DanceCourseCreator.Tests.E2E
```

## 🔄 CI/CD Integration

### GitHub Actions

```yaml
name: E2E Tests

on: [push, pull_request]

jobs:
  e2e-tests:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v4
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: '10.0.x'
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Build
      run: dotnet build --no-restore
    
    - name: Install Playwright
      run: |
        cd src/DanceCourseCreator.Tests.E2E
        pwsh bin/Debug/net10.0/playwright.ps1 install chromium --with-deps
    
    - name: Start Applications
      run: |
        cd src/DanceCourseCreator.API
        dotnet run --no-build --urls "http://localhost:5139" &
        cd ../DanceCourseCreator.Web
        dotnet run --no-build --urls "http://localhost:5034" &
        sleep 15  # Wait for apps to start
    
    - name: Run E2E Tests
      run: |
        cd src/DanceCourseCreator.Tests.E2E
        dotnet run --no-build
    
    - name: Upload Screenshots
      if: failure()
      uses: actions/upload-artifact@v4
      with:
        name: test-screenshots
        path: src/DanceCourseCreator.Tests.E2E/screenshots/
```

### Azure DevOps

```yaml
trigger:
  - main

pool:
  vmImage: 'ubuntu-latest'

steps:
- task: UseDotNet@2
  inputs:
    version: '10.0.x'

- script: dotnet restore
  displayName: 'Restore packages'

- script: dotnet build --no-restore
  displayName: 'Build'

- script: |
    cd src/DanceCourseCreator.Tests.E2E
    pwsh bin/Debug/net10.0/playwright.ps1 install chromium --with-deps
  displayName: 'Install Playwright'

- script: |
    cd src/DanceCourseCreator.API
    dotnet run --no-build --urls "http://localhost:5139" &
    cd ../DanceCourseCreator.Web
    dotnet run --no-build --urls "http://localhost:5034" &
    sleep 15
  displayName: 'Start applications'

- script: |
    cd src/DanceCourseCreator.Tests.E2E
    dotnet run --no-build
  displayName: 'Run E2E tests'
```

## 📚 Additional Resources

- [MSTest Integration Tests Documentation](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-10.0&pivots=mstest)
- [Playwright for .NET](https://playwright.dev/dotnet/)
- [WebApplicationFactory](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests#basic-tests-with-the-default-webapplicationfactory)
- [MSTest 4.0 Documentation](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest)

## 🎯 Summary

**For Local Development:**
1. Start API: `dotnet run` (port 5139)
2. Start Web: `dotnet run` (port 5034)
3. Run tests: `dotnet run` from E2E project

**For CI/CD:**
1. Use GitHub Actions or Azure DevOps examples above
2. Apps start automatically in background
3. Tests run with modern MSTest runner
4. Screenshots uploaded on failure

**For Integration Testing (API only):**
1. Use `PlaywrightIntegrationTests` class
2. Tests automatically use `WebApplicationFactory`
3. No manual startup required

---

**Ready to run comprehensive E2E tests! 🎭✅**
