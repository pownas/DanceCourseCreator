# Dance Course Creator - Playwright E2E Tests

This project contains Playwright end-to-end tests for the Dance Course Creator application.

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
   node bin/Debug/net8.0/.playwright/package/cli.js install chromium
   ```

2. **Start the applications**:
   
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

### Run Smoke Tests Only
```bash
cd src/DanceCourseCreator.Tests.E2E
dotnet test --filter "TestCategory=Smoke"
```

### Run Tests with Verbose Output
```bash
cd src/DanceCourseCreator.Tests.E2E
dotnet test --logger "console;verbosity=detailed"
```

## Test Categories

- **Smoke**: Basic functionality tests that verify the application loads and core navigation works

## Demo User Credentials

The tests use a demo user that is automatically created when the API starts:

- **Email**: `demo@dancecourse.com`
- **Password**: `demo123`

## Screenshots

Tests automatically capture screenshots and save them to the `screenshots/` folder:

- `01-home-page.png` - Initial home page
- `02-login-page.png` - Login page
- `03-after-login-attempt.png` - After attempting login
- `04-courses-page.png` - Courses page
- `05-final-home-page.png` - Final home page

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