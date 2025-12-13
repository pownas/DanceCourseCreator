# Dance Course Creator - Playwright E2E Tests

This project contains comprehensive Playwright end-to-end tests for the Dance Course Creator application, including automated screenshot capture for documentation and regression testing.

## 🚀 Quick Start (Windows)

**Easiest Way - Use Provided Scripts:**

1. **First Time Setup** (install Playwright browsers):
   ```cmd
   install-playwright.cmd
   ```
   *Or verify everything is ready:*
   ```cmd
   verify-e2e-setup.cmd
   ```

2. **Run All Tests** (starts apps automatically):
   ```cmd
   playwright-tests.cmd
   ```

**That's it! The script handles everything for you.** ✅

---

## 🚀 Quick Start (Manual Mode)

**Most Common Usage (Manual Mode):**

1. **Start applications** (two terminals):
   ```bash
   # Terminal 1 - API
   cd src/DanceCourseCreator.API
   dotnet run --urls "http://localhost:5139"
   
   # Terminal 2 - Web
   cd src/DanceCourseCreator.Web
   dotnet run --urls "http://localhost:5034"
   ```

2. **Run tests** (third terminal):
   ```bash
   cd src/DanceCourseCreator.Tests.E2E
   dotnet test
   ```

**📖 For comprehensive testing options, see [E2E Testing Guide](../../docs/E2E-TESTING-GUIDE.md)**

---

## ⚠️ Common Issue: "No test is available"

If you see this error:
```
No test is available in DanceCourseCreator.Tests.E2E.dll
```

**Solution:** Install Playwright browsers first!
```cmd
install-playwright.cmd
```

**Why?** Playwright tests require browser binaries (~200MB) that aren't included in the repository. They must be downloaded separately.

📖 **Full troubleshooting guide:** [PLAYWRIGHT-TROUBLESHOOTING.md](../../docs/PLAYWRIGHT-TROUBLESHOOTING.md)

---

## 📁 Helper Scripts

| Script | Purpose | When to Use |
|--------|---------|-------------|
| `install-playwright.cmd` | Install Playwright browsers | First time, or after "No test is available" error |
| `verify-e2e-setup.cmd` | Check if environment is ready | Before running tests, troubleshooting |
| `playwright-tests.cmd` | Run all E2E tests | After setup, for full test run |

---

## Prerequisites

- .NET 10.0 SDK
- Playwright browsers installed (see Setup section)

## Test Modes

This project supports two testing modes:

1. **Manual Mode** (Default) - You start the applications manually
   - Best for local development
   - Easier debugging
   - See full application logs

2. **WebApplicationFactory Mode** - Tests start the API automatically
   - Best for CI/CD pipelines
   - Automatic cleanup
   - Currently supports API-only integration tests

📖 See [E2E Testing Guide](../../docs/E2E-TESTING-GUIDE.md) for detailed mode explanations

## Test Framework

This project uses **MSTest 4.0.2** with the modern native test runner:

- ✅ **Native .NET Runner** - No VSTest adapter overhead
- ✅ **Standalone Execution** - Can run with `dotnet run`
- ✅ **Fast Performance** - Optimized for .NET 10
- ✅ **Rich Assertions** - `Assert.IsGreaterThan`, `Assert.IsGreaterThanOrEqualTo`, etc.
- ✅ **Playwright Integration** - `Microsoft.Playwright.MSTest` v1.57.0

## Setup

1. **Install Playwright browsers** (first time only):
   ```bash
   # Navigate to test project
   cd src/DanceCourseCreator.Tests.E2E
   
   # Build the project
   dotnet build
   
   # Install Playwright browsers
   pwsh bin/Debug/net10.0/playwright.ps1 install chromium
   ```
   
   Alternative installation:
   ```bash
   node bin/Debug/net10.0/.playwright/package/cli.js install chromium
   ```

2. **Start the applications**:
   
   **Option 1 - Using Aspire AppHost (Recommended):**
   ```bash
   cd src/DanceCourseCreator.AppHost
   dotnet run
   ```
   
   **Option 2 - Manual start:**
   
   Terminal 1 - API:
   ```bash
   cd src/DanceCourseCreator.API
   dotnet run --urls "http://localhost:5139"
   ```
   
   Terminal 2 - Web:
   ```bash
   cd src/DanceCourseCreator.Web
   dotnet run --urls "http://localhost:5034"
   ```

## Running Tests

### Using Modern MSTest Runner (Recommended)

The project is configured with `EnableMSTestRunner=true`, allowing standalone execution:

```bash
cd src/DanceCourseCreator.Tests.E2E

# Run all tests with MSTest native runner
dotnet run

# Run with specific configuration
dotnet run --configuration Release
```

**Output includes:**
- Test execution progress with real-time updates
- Summary: total, failed, succeeded, skipped tests
- Duration and performance metrics
- Telemetry information (can be disabled)

### Traditional Test Execution

```bash
cd src/DanceCourseCreator.Tests.E2E

# Run all tests
dotnet test

# Run tests with verbose output
dotnet test --logger "console;verbosity=detailed"
```

### Run Tests by Category

MSTest 4.0.2 supports powerful filtering with `[TestCategory]` attributes:

```bash
# Navigation tests
dotnet test --filter "TestCategory=Navigation"

# Pattern library tests
dotnet test --filter "TestCategory=Patterns"

# Course tests
dotnet test --filter "TestCategory=Courses"

# Lesson tests
dotnet test --filter "TestCategory=Lessons"

# Template tests
dotnet test --filter "TestCategory=Templates"

# All tests that capture screenshots
dotnet test --filter "TestCategory=Screenshots"

# Smoke tests only
dotnet test --filter "TestCategory=Smoke"

# Integration tests (API testing)
dotnet test --filter "TestCategory=Integration"

# Kestrel-specific tests
dotnet test --filter "TestCategory=Kestrel"

# CRUD operation tests
dotnet test --filter "TestCategory=CRUD"
```

### Advanced Filtering

```bash
# Multiple categories (OR logic)
dotnet test --filter "TestCategory=Navigation|TestCategory=Patterns"

# Exclude categories
dotnet test --filter "TestCategory!=Screenshots"

# Run specific test method
dotnet test --filter "FullyQualifiedName~HomeAndNavigation"

# Combine filters
dotnet test --filter "TestCategory=Integration&TestCategory=CRUD"
```

## Test Files

### HomeAndNavigationTests.cs
Tests for home page and main navigation functionality:
- Home page loading and content verification
- Navigation through all main sections (Patterns, Lessons, Courses, Templates)
- Quick action buttons functionality
- **Categories**: Navigation, Screenshots

### TurbankTests.cs
Tests for turbank browsing and interaction:
- Pattern library loading and display
- Filtering by type (Pattern/Exercise)
- Filtering by level (Beginner, Improver, etc.)
- Search functionality
- Viewing pattern details
- **Categories**: Patterns, Screenshots

### CourseCreationTests.cs
Tests for course creation workflow:
- Opening create course dialog
- Filling form with course details
- Adding course goals
- Saving new courses
- Empty state handling
- **Categories**: Courses, Screenshots

### CourseEditingTests.cs
Tests for course editing workflow:
- Opening edit dialog for existing courses
- Modifying course details
- Adding additional goals
- Saving changes
- Viewing course details
- Canceling edits
- **Categories**: Courses, Screenshots

### LessonAndTemplateTests.cs
Tests for lesson and template workflows:
- Lesson page loading and navigation
- Creating new lessons
- Template page loading
- Creating new templates
- Viewing and editing templates
- Duplicating templates
- **Categories**: Lessons, Templates, Screenshots

### DemoLoginSmokeTests.cs
Basic smoke tests for login and navigation:
- Demo user login flow
- Basic page navigation
- **Categories**: Smoke

### PlaywrightIntegrationTests.cs
API integration tests using Playwright:
- Health check endpoint verification
- CRUD operations on Patterns API
- Filtering and search functionality
- HTTP client integration with WebApplicationFactory
- **Categories**: Integration, Kestrel, CRUD, Playwright

## Test Categories

MSTest 4.0.2 uses `[TestCategory]` attribute for organization:

- **Navigation**: Tests related to page navigation and routing
- **Patterns**: Tests for turbank functionality
- **Courses**: Tests for course creation and management
- **Lessons**: Tests for lesson planning
- **Templates**: Tests for template management
- **Screenshots**: All tests that capture screenshots
- **Smoke**: Basic functionality tests
- **Integration**: API integration tests
- **Kestrel**: Kestrel server-specific tests
- **CRUD**: Create, Read, Update, Delete operation tests
- **Playwright**: Playwright-specific browser automation tests

## Demo User Credentials

The tests use a demo user that is automatically created when the API starts:

- **Email**: `demo@dancecourse.com`
- **Password**: `demo123`

## Screenshots

Tests automatically capture screenshots organized by category in the `screenshots/` folder:

### Navigation Screenshots (`screenshots/navigation/`)
- `01-home-page-initial.png` - Initial home page view
- `02-patterns-page.png` - Patterns/library page
- `03-lessons-page.png` - Lessons page
- `04-courses-page.png` - Courses page
- `05-templates-page.png` - Templates page
- `06-back-to-home.png` - Return to home page
- `07-quick-actions-visible.png` - Quick action buttons
- `08-after-quick-action-click.png` - After clicking quick action

### Turbank Screenshots (`screenshots/patterns/`)
- `01-patterns-library-initial.png` - Initial patterns page
- `02-before-type-filter.png` - Before applying type filter
- `03-type-filter-dropdown-open.png` - Type filter dropdown
- `04-after-type-filter-applied.png` - After filtering by type
- `05-level-filter-dropdown-open.png` - Level filter dropdown
- `06-after-level-filter-applied.png` - After filtering by level
- `07-before-search.png` - Before search
- `08-search-text-entered.png` - Search text entered
- `09-after-search-applied.png` - After search applied
- `10-before-view-pattern.png` - Before viewing pattern details
- `11-pattern-details-dialog.png` - Pattern details dialog

### Course Creation Screenshots (`screenshots/course-creation/`)
- `01-courses-page-initial.png` - Courses page
- `02-create-course-dialog-open.png` - Create dialog
- `03-create-form-empty.png` - Empty form
- `04-create-form-name-filled.png` - Name field filled
- `05-level-dropdown-open.png` - Level selection dropdown
- `06-level-selected.png` - Level selected
- `07-duration-filled.png` - Duration field filled
- `08-goal-entered.png` - Goal text entered
- `09-goal-added-to-list.png` - Goal added to list
- `10-form-complete-before-save.png` - Complete form before save
- `11-after-course-created.png` - After course saved
- `12-courses-list-state.png` - Courses list state
- `13-empty-state-visible.png` - Empty state message

### Course Editing Screenshots (`screenshots/course-editing/`)
- `01-courses-list-before-edit.png` - Courses list
- `02-edit-course-dialog-open.png` - Edit dialog opened
- `03-edit-form-initial-state.png` - Initial edit form
- `04-edit-name-modified.png` - Name modified
- `05-edit-duration-modified.png` - Duration modified
- `06-edit-new-goal-entered.png` - New goal entered
- `07-edit-new-goal-added.png` - New goal added
- `08-edit-form-before-save.png` - Before saving changes
- `09-after-course-edited.png` - After changes saved
- `10-view-course-details.png` - Viewing course details
- `11-edit-dialog-before-cancel.png` - Before canceling edit
- `12-after-cancel-back-to-list.png` - After canceling

### Lesson & Template Screenshots (`screenshots/lessons-templates/`)
- `01-lessons-page-initial.png` - Lessons page
- `02-create-lesson-dialog-open.png` - Create lesson dialog
- `03-lessons-before-filter.png` - Before filtering lessons
- `04-lessons-search-entered.png` - Search text entered
- `05-lessons-after-filter.png` - After filter applied
- `06-templates-page-initial.png` - Templates page
- `07-create-template-dialog-open.png` - Create template dialog
- `08-templates-list.png` - Templates list
- `09-template-details-dialog.png` - Template details
- `10-edit-template-dialog.png` - Edit template dialog
- `11-template-menu-open.png` - Template context menu
- `12-duplicate-template-dialog.png` - Duplicate template dialog

## MSTest 4.0.2 Features

### Modern Assertions
```csharp
// Comparison assertions (new in MSTest 4.x)
Assert.IsGreaterThan(count, 0, "Should have items");
Assert.IsGreaterThanOrEqualTo(patterns.Count, 2, "Minimum patterns");
Assert.IsLessThan(duration, 1000, "Fast response");

// String assertions
Assert.Contains("OK", response, StringComparison.Ordinal);
Assert.StartsWith("http", url);
Assert.EndsWith(".png", filename);

// Collection assertions
Assert.IsNotEmpty(collection, "Should have elements");
Assert.HasElements(list);
```

### Test Lifecycle
```csharp
[TestInitialize]     // Run before each test
[TestCleanup]        // Run after each test
[ClassInitialize]    // Run once before all tests in class
[ClassCleanup]       // Run once after all tests in class
[AssemblyInitialize] // Run once before all tests in assembly
[AssemblyCleanup]    // Run once after all tests in assembly
```

### Data-Driven Tests
```csharp
[TestMethod]
[DataRow("value1", 1)]
[DataRow("value2", 2)]
[DataRow("value3", 3)]
public void DataDrivenTest(string input, int expected)
{
    // Test implementation
}
```

Note: `[DataTestMethod]` is obsolete in MSTest 4.0.2. Use `[TestMethod]` with `[DataRow]` instead.

## Performance

**Test Execution Times** (approximate):
- **Navigation Tests**: ~8-12 seconds
- **Turbank Tests**: ~15-20 seconds  
- **Course Creation Tests**: ~12-16 seconds
- **Course Editing Tests**: ~14-18 seconds
- **Lesson & Template Tests**: ~10-14 seconds
- **Integration Tests**: ~4-6 seconds
- **Full Test Suite**: ~60-90 seconds

**Modern MSTest Runner Benefits:**
- ⚡ 40% faster than VSTest adapter
- 🎯 Direct execution without adapter overhead
- 📊 Real-time progress updates
- 🔍 Better error messages and diagnostics

## Troubleshooting

### Browser Installation Issues
If Playwright browser installation fails, try:
```bash
# Alternative installation method
pwsh bin/Debug/net10.0/playwright.ps1 install chromium --force

# Check installed browsers
pwsh bin/Debug/net10.0/playwright.ps1 install --dry-run
```

### Application Not Running
Ensure both API and Web are running on the correct ports:
```bash
# Check API health
curl http://localhost:5139/api/health

# Check Web application
curl http://localhost:5034

# Or use PowerShell
Invoke-WebRequest http://localhost:5139/api/health
```

### Test Failures
1. **Verify applications are running** and accessible
2. **Check demo user exists** in database
3. **Increase timeout values** if network is slow (edit test files)
4. **Check screenshot folder** for visual debugging
5. **Run tests individually** to isolate failures:
   ```bash
   dotnet test --filter "FullyQualifiedName~SpecificTestName"
   ```

### MSTest Runner Issues

If you see errors about VSTest not being supported:

```bash
# Ensure project has modern runner enabled
# Check .csproj file for:
<EnableMSTestRunner>true</EnableMSTestRunner>
<TestingPlatformDotnetTestSupport>true</TestingPlatformDotnetTestSupport>
<OutputType>Exe</OutputType>

# Rebuild the project
dotnet clean
dotnet build

# Use dotnet run instead of dotnet test
dotnet run --project src/DanceCourseCreator.Tests.E2E
```

### Disable Telemetry

To disable MSTest telemetry collection:
```bash
# PowerShell
$env:TESTINGPLATFORM_TELEMETRY_OPTOUT = "1"

# Command Prompt
set TESTINGPLATFORM_TELEMETRY_OPTOUT=1

# Bash/Linux
export TESTINGPLATFORM_TELEMETRY_OPTOUT=1
```

## Test Implementation Notes

- Tests use **MSTest 4.0.2** with native runner for optimal performance
- `[TestInitialize]` and `[TestCleanup]` methods manage test lifecycle
- Tests are designed to be resilient to application errors
- Screenshots provide visual verification of application state
- Each test is independent and can run in isolation
- Parallel execution supported (can be configured with `[assembly: Parallelize]`)

## CI/CD Integration

For continuous integration pipelines:

```yaml
# Example GitHub Actions workflow
- name: Run E2E Tests
  run: |
    # Start applications in background
    cd src/DanceCourseCreator.API
    dotnet run &
    cd ../DanceCourseCreator.Web
    dotnet run &
    
    # Wait for applications to start
    sleep 10
    
    # Run tests with modern runner
    cd ../DanceCourseCreator.Tests.E2E
    dotnet run
```

Or use traditional approach:
```yaml
- name: Run E2E Tests
  run: dotnet test src/DanceCourseCreator.Tests.E2E --logger trx
```

## Additional Resources

- [MSTest Documentation](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest)
- [MSTest 4.0 Release Notes](https://devblogs.microsoft.com/dotnet/mstest-4-0-release/)
- [Playwright for .NET](https://playwright.dev/dotnet/)
- [Microsoft Testing Platform](https://aka.ms/testingplatform)