# Testing Guide - Dance Course Creator

This guide provides comprehensive information about testing in the Dance Course Creator application, using **MSTest 4.0.2** with the modern native test runner for .NET 10.

## 📋 Table of Contents

- [Overview](#overview)
- [Test Framework](#test-framework)
- [Project Structure](#project-structure)
- [Running Tests](#running-tests)
- [Unit Tests](#unit-tests)
- [E2E Tests](#e2e-tests)
- [Test Categories](#test-categories)
- [Best Practices](#best-practices)
- [CI/CD Integration](#cicd-integration)
- [Troubleshooting](#troubleshooting)

## Overview

The Dance Course Creator uses a multi-layered testing approach:

1. **Unit Tests** (`DanceCourseCreator.Web.Tests`) - Service layer and component tests
2. **E2E Tests** (`DanceCourseCreator.Tests.E2E`) - End-to-end Playwright tests with screenshot capture
3. **Integration Tests** - API integration tests within E2E project

### Testing Stack

| Component | Technology | Version |
|-----------|------------|---------|
| Test Framework | MSTest | 4.0.2 |
| E2E Testing | Playwright | 1.57.0 |
| Component Testing | bUnit | 2.2.2 |
| Mocking | Moq | 4.20.72 |
| Code Coverage | coverlet | 6.0.4 |
| Target Framework | .NET | 10.0 |

## Test Framework

### MSTest 4.0.2 Modern Runner

All test projects use **MSTest 4.0.2** with the native test runner, providing:

✅ **Performance**: 40% faster than traditional VSTest adapter  
✅ **Native Integration**: Built for .NET 10 without external dependencies  
✅ **Standalone Execution**: Run tests as executable with `dotnet run`  
✅ **Rich Assertions**: Modern assertion API with intuitive methods  
✅ **Better Diagnostics**: Improved error messages and test output  
✅ **Flexible Execution**: Support for both `dotnet run` and `dotnet test`  

### Configuration

Both test projects are configured with:

```xml
<PropertyGroup>
  <EnableMSTestRunner>true</EnableMSTestRunner>
  <TestingPlatformDotnetTestSupport>true</TestingPlatformDotnetTestSupport>
  <OutputType>Exe</OutputType>
  <IsTestProject>true</IsTestProject>
</PropertyGroup>

<ItemGroup>
  <PackageReference Include="MSTest" Version="4.0.2" />
</ItemGroup>
```

## Project Structure

```
DanceCourseCreator/
├── src/
│   ├── DanceCourseCreator.Web.Tests/           # Unit & Component Tests
│   │   ├── Services/                           # Service layer tests
│   │   │   └── BreakpointServiceTests.cs       # 25 tests
│   │   └── DanceCourseCreator.Web.Tests.csproj
│   │
│   └── DanceCourseCreator.Tests.E2E/           # End-to-End Tests
│       ├── HomeAndNavigationTests.cs           # Navigation flow tests
│       ├── TurbankTests.cs                     # Pattern library tests
│       ├── CourseCreationTests.cs              # Course creation workflow
│       ├── CourseEditingTests.cs               # Course editing workflow
│       ├── LessonAndTemplateTests.cs           # Lesson & template tests
│       ├── DemoLoginSmokeTests.cs              # Smoke tests
│       ├── PlaywrightIntegrationTests.cs       # API integration tests
│       ├── Infrastructure/                     # Test infrastructure
│       │   └── CustomWebApplicationFactory.cs  # Test server setup
│       ├── screenshots/                        # Auto-captured screenshots
│       └── DanceCourseCreator.Tests.E2E.csproj
└── docs/
    └── TESTING.md                              # This file
```

## Running Tests

### Quick Start

```bash
# Run all tests in solution
dotnet test

# Run unit tests only
dotnet run --project src/DanceCourseCreator.Web.Tests

# Run E2E tests only
dotnet run --project src/DanceCourseCreator.Tests.E2E
```

### Modern MSTest Runner (Recommended)

The modern runner provides better performance and real-time feedback:

```bash
# Web unit tests
cd src/DanceCourseCreator.Web.Tests
dotnet run

# E2E tests
cd src/DanceCourseCreator.Tests.E2E
dotnet run

# With specific configuration
dotnet run --configuration Release
```

**Output Example:**
```
MSTest v4.0.2 (UTC 2025-11-11) [win-arm64 - .NET 10.0.1]

[✓25/x0/↓0] DanceCourseCreator.Web.Tests.dll (net10.0|arm64)(2s)

Test run summary: Passed!
  total: 25
  failed: 0
  succeeded: 25
  skipped: 0
  duration: 2s 480ms
```

### Traditional Test Execution

```bash
# All tests
dotnet test

# Specific project
dotnet test src/DanceCourseCreator.Web.Tests

# With verbose output
dotnet test --logger "console;verbosity=detailed"

# With test results file
dotnet test --logger "trx;LogFileName=test-results.trx"
```

### Filter by Category

```bash
# Navigation tests
dotnet test --filter "TestCategory=Navigation"

# Pattern/Turbank tests
dotnet test --filter "TestCategory=Patterns"

# Course management tests
dotnet test --filter "TestCategory=Courses"

# Screenshot tests
dotnet test --filter "TestCategory=Screenshots"

# Integration tests
dotnet test --filter "TestCategory=Integration"

# Multiple categories (OR logic)
dotnet test --filter "TestCategory=Navigation|TestCategory=Patterns"

# Exclude categories
dotnet test --filter "TestCategory!=Screenshots"
```

### Filter by Test Name

```bash
# Run specific test method
dotnet test --filter "FullyQualifiedName~IsMobile"

# Run all tests in a class
dotnet test --filter "FullyQualifiedName~BreakpointServiceTests"

# Run tests matching pattern
dotnet test --filter "DisplayName~ShouldReturn"
```

## Unit Tests

### Web.Tests Project

**Location**: `src/DanceCourseCreator.Web.Tests`  
**Framework**: MSTest 4.0.2 + bUnit + Moq  
**Test Count**: 25 tests  
**Execution Time**: ~2.5 seconds  

#### Test Coverage

**BreakpointServiceTests.cs** (25 tests):
- Subscription management (subscribe, unsubscribe, multiple subscribers)
- Breakpoint detection (IsMobile, IsTablet, IsDesktop)
- Error handling and resilience
- Async disposal and cleanup
- Mock integration with MudBlazor services

#### Running Unit Tests

```bash
# Navigate to project
cd src/DanceCourseCreator.Web.Tests

# Run with modern runner
dotnet run

# Or traditional approach
dotnet test

# Run specific test
dotnet test --filter "FullyQualifiedName~Subscribe_ShouldReturnValidGuid"

# Run with coverage (requires coverlet)
dotnet test /p:CollectCoverage=true
```

#### Example Test Structure

```csharp
[TestClass]
public class BreakpointServiceTests
{
    private Mock<IBrowserViewportService> _mockViewportService = null!;
    private BreakpointService _service = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _mockViewportService = new Mock<IBrowserViewportService>();
        _service = new BreakpointService(_mockViewportService.Object, ...);
    }

    [TestMethod]
    [DataRow(Breakpoint.Xs, true)]
    [DataRow(Breakpoint.Md, false)]
    public async Task IsMobile_ShouldReturnCorrectValue(
        Breakpoint breakpoint, 
        bool expectedIsMobile)
    {
        // Arrange
        _mockViewportService
            .Setup(x => x.GetCurrentBreakpointAsync())
            .ReturnsAsync(breakpoint);

        // Act
        var result = await _service.IsMobile();

        // Assert
        Assert.AreEqual(expectedIsMobile, result);
    }
}
```

## E2E Tests

### Tests.E2E Project

**Location**: `src/DanceCourseCreator.Tests.E2E`  
**Framework**: MSTest 4.0.2 + Playwright 1.57.0  
**Test Categories**: 7 categories  
**Screenshot Output**: `screenshots/` folder  

#### Prerequisites

1. **Install Playwright Browsers** (first time):
   ```bash
   cd src/DanceCourseCreator.Tests.E2E
   dotnet build
   pwsh bin/Debug/net10.0/playwright.ps1 install chromium
   ```

2. **Start Applications**:
   
   **Option A - Aspire AppHost (Recommended)**:
   ```bash
   cd src/DanceCourseCreator.AppHost
   dotnet run
   ```
   
   **Option B - Manual Start**:
   ```bash
   # Terminal 1 - API
   cd src/DanceCourseCreator.API
   dotnet run --urls "http://localhost:5139"
   
   # Terminal 2 - Web
   cd src/DanceCourseCreator.Web
   dotnet run --urls "http://localhost:5034"
   ```

#### Test Files

| Test File | Tests | Categories | Description |
|-----------|-------|------------|-------------|
| `HomeAndNavigationTests.cs` | 8 | Navigation, Screenshots | Home page and navigation flow |
| `TurbankTests.cs` | 6 | Patterns, Screenshots | Pattern library browsing and filtering |
| `CourseCreationTests.cs` | 13 | Courses, Screenshots | Course creation workflow with validation |
| `CourseEditingTests.cs` | 12 | Courses, Screenshots | Course editing and modification |
| `LessonAndTemplateTests.cs` | 12 | Lessons, Templates, Screenshots | Lesson and template management |
| `DemoLoginSmokeTests.cs` | 2 | Smoke | Basic smoke tests |
| `PlaywrightIntegrationTests.cs` | 6 | Integration, Kestrel, CRUD | API integration tests |

#### Running E2E Tests

```bash
# All E2E tests
cd src/DanceCourseCreator.Tests.E2E
dotnet run

# By category
dotnet test --filter "TestCategory=Navigation"
dotnet test --filter "TestCategory=Patterns"
dotnet test --filter "TestCategory=Courses"

# Integration tests only
dotnet test --filter "TestCategory=Integration"

# Exclude screenshots (faster)
dotnet test --filter "TestCategory!=Screenshots"

# Specific workflow
dotnet test --filter "FullyQualifiedName~CourseCreation"
```

#### Screenshot Capture

All E2E tests automatically capture screenshots organized by feature:

```
screenshots/
├── navigation/              # Navigation flow (8 screenshots)
├── patterns/                # Turbank browsing (11 screenshots)
├── course-creation/         # Course creation (13 screenshots)
├── course-editing/          # Course editing (12 screenshots)
└── lessons-templates/       # Lessons and templates (12 screenshots)
```

Screenshots are captured at key points:
- Initial page load
- Before/after user actions
- Dialog opens
- Form submissions
- Result verification

#### Demo User

Tests use an auto-created demo user:
- **Email**: `demo@dancecourse.com`
- **Password**: `demo123`

## Test Categories

MSTest uses `[TestCategory]` attribute for organization:

| Category | Usage | Description |
|----------|-------|-------------|
| `Navigation` | E2E | Page navigation and routing tests |
| `Patterns` | E2E | Turbank/pattern library functionality |
| `Courses` | E2E | Course creation and management |
| `Lessons` | E2E | Lesson planning and organization |
| `Templates` | E2E | Template management features |
| `Screenshots` | E2E | All tests that capture screenshots |
| `Smoke` | E2E | Basic functionality smoke tests |
| `Integration` | E2E | API integration tests |
| `Kestrel` | E2E | Kestrel server-specific tests |
| `CRUD` | E2E | Create, Read, Update, Delete tests |
| `Playwright` | E2E | Playwright browser automation tests |

### Category Usage Examples

```csharp
[TestClass]
public class HomeAndNavigationTests : PageTest
{
    [TestMethod]
    [TestCategory("Navigation")]
    [TestCategory("Screenshots")]
    public async Task HomePage_ShouldLoad()
    {
        // Test implementation
    }
}
```

## Best Practices

### Test Organization

1. **Use TestInitialize/TestCleanup**:
   ```csharp
   [TestInitialize]
   public void TestInitialize()
   {
       // Setup before each test
   }

   [TestCleanup]
   public void TestCleanup()
   {
       // Cleanup after each test
   }
   ```

2. **Use Descriptive Test Names**:
   ```csharp
   [TestMethod]
   public async Task Subscribe_ShouldReturnValidGuid()
   {
       // Clear what the test verifies
   }
   ```

3. **Follow Arrange-Act-Assert Pattern**:
   ```csharp
   [TestMethod]
   public async Task Example_Test()
   {
       // Arrange
       var input = "test";
       
       // Act
       var result = await _service.Process(input);
       
       // Assert
       Assert.AreEqual("expected", result);
   }
   ```

### MSTest 4.0.2 Assertions

```csharp
// Equality
Assert.AreEqual(expected, actual);
Assert.AreNotEqual(unexpected, actual);

// Comparison (new in MSTest 4.x)
Assert.IsGreaterThan(actual, minimum);
Assert.IsGreaterThanOrEqualTo(actual, minimum);
Assert.IsLessThan(actual, maximum);
Assert.IsLessThanOrEqualTo(actual, maximum);

// Boolean
Assert.IsTrue(condition);
Assert.IsFalse(condition);

// Nullability
Assert.IsNull(value);
Assert.IsNotNull(value);

// String
Assert.Contains(substring, fullString);
Assert.StartsWith(prefix, fullString);
Assert.EndsWith(suffix, fullString);

// Collections
Assert.IsNotEmpty(collection);
```

### Data-Driven Tests

```csharp
[TestMethod]
[DataRow(Breakpoint.Xs, true)]
[DataRow(Breakpoint.Md, false)]
[DataRow(Breakpoint.Lg, false)]
public async Task IsMobile_ShouldReturnCorrectValue(
    Breakpoint breakpoint, 
    bool expected)
{
    // Test runs once for each DataRow
}
```

Note: `[DataTestMethod]` is obsolete in MSTest 4.0.2. Use `[TestMethod]` with `[DataRow]`.

### Async Testing

```csharp
[TestMethod]
public async Task AsyncOperation_ShouldComplete()
{
    // Properly await async operations
    var result = await _service.GetDataAsync();
    Assert.IsNotNull(result);
}
```

### Mocking with Moq

```csharp
[TestInitialize]
public void TestInitialize()
{
    _mockService = new Mock<IService>();
    _mockService
        .Setup(x => x.GetDataAsync())
        .ReturnsAsync(expectedData);
    
    _sut = new SystemUnderTest(_mockService.Object);
}

[TestMethod]
public async Task Should_CallMockedService()
{
    await _sut.PerformAction();
    
    _mockService.Verify(
        x => x.GetDataAsync(), 
        Times.Once);
}
```

## CI/CD Integration

### GitHub Actions

```yaml
name: Run Tests

on: [push, pull_request]

jobs:
  test:
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
    
    - name: Run Unit Tests
      run: dotnet run --project src/DanceCourseCreator.Web.Tests --no-build
    
    - name: Install Playwright
      run: |
        cd src/DanceCourseCreator.Tests.E2E
        pwsh bin/Debug/net10.0/playwright.ps1 install chromium
    
    - name: Start Applications
      run: |
        cd src/DanceCourseCreator.API
        dotnet run --no-build &
        cd ../DanceCourseCreator.Web
        dotnet run --no-build &
        sleep 10
    
    - name: Run E2E Tests
      run: dotnet run --project src/DanceCourseCreator.Tests.E2E --no-build
    
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
  displayName: 'Build solution'

- script: dotnet run --project src/DanceCourseCreator.Web.Tests --no-build
  displayName: 'Run unit tests'

- script: dotnet run --project src/DanceCourseCreator.Tests.E2E --no-build
  displayName: 'Run E2E tests'
```

## Troubleshooting

### Common Issues

#### 1. VSTest Not Supported Error

**Error**: `Testing with VSTest target is no longer supported by Microsoft.Testing.Platform on .NET 10`

**Solution**: Use `dotnet run` instead of `dotnet test`, or ensure project has:
```xml
<TestingPlatformDotnetTestSupport>true</TestingPlatformDotnetTestSupport>
```

#### 2. Playwright Browser Not Found

**Error**: Browser executable not found

**Solution**:
```bash
cd src/DanceCourseCreator.Tests.E2E
pwsh bin/Debug/net10.0/playwright.ps1 install chromium --force
```

#### 3. Application Not Running

**Error**: Tests fail with connection errors

**Solution**:
```bash
# Verify API
curl http://localhost:5139/api/health

# Verify Web
curl http://localhost:5034

# Check ports are not in use
netstat -an | findstr "5139"
netstat -an | findstr "5034"
```

#### 4. Tests Timeout

**Error**: Tests fail with timeout errors

**Solution**: Increase timeout in test code:
```csharp
await Page.WaitForSelectorAsync("selector", new() { Timeout = 60000 });
```

#### 5. Parallel Test Conflicts

**Error**: Tests interfere with each other

**Solution**: Disable parallelization:
```csharp
[assembly: DoNotParallelize]
```

Or configure controlled parallelization:
```csharp
[assembly: Parallelize(Workers = 2, Scope = ExecutionScope.ClassLevel)]
```

### Disable Telemetry

To disable MSTest telemetry:

**PowerShell**:
```powershell
$env:TESTINGPLATFORM_TELEMETRY_OPTOUT = "1"
```

**Bash**:
```bash
export TESTINGPLATFORM_TELEMETRY_OPTOUT=1
```

**Command Prompt**:
```cmd
set TESTINGPLATFORM_TELEMETRY_OPTOUT=1
```

### Debug Tests

**Visual Studio**:
1. Open Test Explorer (Test → Test Explorer)
2. Right-click test → Debug
3. Set breakpoints in test code

**VS Code**:
1. Open test file
2. Click "Debug Test" above test method
3. Use Debug Console for output

**Command Line**:
```bash
# Run with detailed logging
dotnet test --logger "console;verbosity=detailed"

# Run with diagnostic output
dotnet test --diag:diag.log
```

## Performance

### Typical Execution Times

| Test Suite | Test Count | Duration | Performance |
|------------|------------|----------|-------------|
| Web.Tests | 25 | ~2.5s | ⚡ Fast |
| E2E - Navigation | 8 | ~8-12s | 🔵 Normal |
| E2E - Patterns | 6 | ~15-20s | 🔵 Normal |
| E2E - Courses | 25 | ~26-34s | 🟡 Moderate |
| E2E - Integration | 6 | ~4-6s | ⚡ Fast |
| **Full Suite** | **70+** | **60-90s** | 🟢 Good |

### Optimization Tips

1. **Use Categories**: Run only needed tests
   ```bash
   dotnet test --filter "TestCategory=Smoke"  # Fast smoke tests only
   ```

2. **Parallel Execution**: Enable for unit tests
   ```csharp
   [assembly: Parallelize(Scope = ExecutionScope.MethodLevel)]
   ```

3. **Skip Screenshots**: Run without screenshot tests for faster feedback
   ```bash
   dotnet test --filter "TestCategory!=Screenshots"
   ```

4. **Use Modern Runner**: `dotnet run` is faster than `dotnet test`

## Additional Resources

- [MSTest Documentation](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest)
- [MSTest 4.0 Release Notes](https://devblogs.microsoft.com/dotnet/mstest-4-0-release/)
- [Playwright for .NET](https://playwright.dev/dotnet/)
- [bUnit Documentation](https://bunit.dev/)
- [Moq Documentation](https://github.com/moq/moq4)
- [Microsoft Testing Platform](https://aka.ms/testingplatform)

---

**Happy Testing! 🧪✅**
