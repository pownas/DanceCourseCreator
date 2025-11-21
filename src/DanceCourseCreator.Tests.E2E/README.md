# Dance Course Creator - Playwright E2E Tests

This project contains comprehensive Playwright end-to-end tests for the Dance Course Creator application, including automated screenshot capture for documentation and regression testing.

## Prerequisites

- .NET 8.0 SDK
- Both API and Client applications running:
  - API: `http://localhost:5139`
  - Client: `http://localhost:5034`

## Setup

1. **Install Playwright browsers** (first time only):
   ```bash
   # Navigate to test project
   cd src/DanceCourseCreator.Tests.E2E
   
   # Build the project
   dotnet build
   
   # Install Playwright browsers
   pwsh bin/Debug/net8.0/playwright.ps1 install chromium
   ```
   
   Alternative installation:
   ```bash
   node bin/Debug/net8.0/.playwright/package/cli.js install chromium
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
   
   Terminal 2 - Client:
   ```bash
   cd src/DanceCourseCreator.Client
   dotnet run --urls "http://localhost:5034"
   ```

## Running Tests

### Run All Tests
```bash
cd src/DanceCourseCreator.Tests.E2E
dotnet test
```

### Run Tests by Category
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
```

### Run Tests with Verbose Output
```bash
cd src/DanceCourseCreator.Tests.E2E
dotnet test --logger "console;verbosity=detailed"
```

## Test Files

### HomeAndNavigationTests.cs
Tests for home page and main navigation functionality:
- Home page loading and content verification
- Navigation through all main sections (Patterns, Lessons, Courses, Templates)
- Quick action buttons functionality

### PatternLibraryTests.cs
Tests for pattern library browsing and interaction:
- Pattern library loading and display
- Filtering by type (Pattern/Exercise)
- Filtering by level (Beginner, Improver, etc.)
- Search functionality
- Viewing pattern details

### CourseCreationTests.cs
Tests for course creation workflow:
- Opening create course dialog
- Filling form with course details
- Adding course goals
- Saving new courses
- Empty state handling

### CourseEditingTests.cs
Tests for course editing workflow:
- Opening edit dialog for existing courses
- Modifying course details
- Adding additional goals
- Saving changes
- Viewing course details
- Canceling edits

### LessonAndTemplateTests.cs
Tests for lesson and template workflows:
- Lesson page loading and navigation
- Creating new lessons
- Template page loading
- Creating new templates
- Viewing and editing templates
- Duplicating templates

### DemoLoginSmokeTests.cs
Basic smoke tests for login and navigation:
- Demo user login flow
- Basic page navigation

## Test Categories

- **Navigation**: Tests related to page navigation and routing
- **Patterns**: Tests for pattern library functionality
- **Courses**: Tests for course creation and management
- **Lessons**: Tests for lesson planning
- **Templates**: Tests for template management
- **Screenshots**: All tests that capture screenshots
- **Smoke**: Basic functionality tests

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

### Pattern Library Screenshots (`screenshots/patterns/`)
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

## Troubleshooting

### Browser Installation Issues
If Playwright browser installation fails, try:
```bash
# Alternative installation method
pwsh bin/Debug/net8.0/playwright.ps1 install chromium --force
```

### Application Not Running
Ensure both API and Client are running on the correct ports:
- Check API: `curl http://localhost:5139/api/health`
- Check Client: `curl http://localhost:5034`

### Test Failures
1. Verify applications are running and accessible
2. Check that demo user exists in database
3. Increase timeout values if network is slow
4. Check screenshot folder for visual debugging

## Test Implementation Notes

- Tests are designed to be resilient to application errors (some errors are expected in the current implementation)
- Login functionality is tested but may not complete successfully due to application issues
- Navigation and page loading are the primary focus of smoke tests
- Screenshots provide visual verification of application state