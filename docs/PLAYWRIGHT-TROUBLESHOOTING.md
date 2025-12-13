# Playwright E2E Tests - Troubleshooting Guide

## Error: "No test is available in DanceCourseCreator.Tests.E2E.dll"

This error occurs when **Playwright browsers are not installed**. The tests require Chromium to be installed before they can run.

### Quick Fix

Run the installation script:
```cmd
install-playwright.cmd
```

Or install manually:
```cmd
cd src\DanceCourseCreator.Tests.E2E
dotnet build
pwsh bin\Debug\net10.0\playwright.ps1 install chromium
```

### Why This Happens

Playwright tests require actual browser binaries to run. When you first clone the repository or build the test project, these browsers are not included. They need to be downloaded separately.

**Browser Size:** ~200MB for Chromium

### Verification

After installation, verify the browsers are installed:
```cmd
dir src\DanceCourseCreator.Tests.E2E\bin\Debug\net10.0\.playwright
```

You should see a `.playwright` folder containing browser files.

---

## Error: "pwsh is not recognized"

### Solution 1: Install PowerShell

PowerShell 7+ is required for Playwright installation.

**Download:** https://github.com/PowerShell/PowerShell/releases

**Or with winget:**
```cmd
winget install Microsoft.PowerShell
```

### Solution 2: Use Node.js Alternative

If PowerShell is not available, use Node.js:
```cmd
cd src\DanceCourseCreator.Tests.E2E
dotnet build
node bin\Debug\net10.0\.playwright\package\cli.js install chromium
```

---

## Error: "Applications not responding"

If the test runner reports connection errors:

### 1. Check if ports are in use

```cmd
netstat -ano | findstr "5139"
netstat -ano | findstr "5034"
```

### 2. Kill existing processes

```cmd
taskkill /F /IM dotnet.exe
```

### 3. Increase startup wait time

Edit `playwright-tests.cmd` and change:
```cmd
timeout /t 15 /nobreak > nul
```
to:
```cmd
timeout /t 30 /nobreak > nul
```

---

## Error: Test discovery works but tests fail

### Check application health

Before running tests, verify apps are running:

**API:**
```cmd
curl http://localhost:5139/api/health
```

**Web:**
```cmd
curl http://localhost:5034
```

### Check Playwright browser

Run a single test with detailed output:
```cmd
cd src\DanceCourseCreator.Tests.E2E
dotnet test --filter "FullyQualifiedName~HealthCheck" --logger "console;verbosity=detailed"
```

---

## Common Issues and Solutions

### Issue 1: Tests timeout

**Solution:** Increase timeout in test code or start apps manually first.

### Issue 2: Database locked errors

**Solution:** Stop all running instances:
```cmd
taskkill /F /IM dotnet.exe
del src\DanceCourseCreator.API\*.db
del src\DanceCourseCreator.API\*.db-shm
del src\DanceCourseCreator.API\*.db-wal
```

### Issue 3: Port already in use

**Solution:** Change ports in test script:
```cmd
REM In playwright-tests.cmd
dotnet run --urls http://localhost:5140  REM Changed from 5139
```

### Issue 4: Browser crashes

**Solution:** Reinstall Playwright browsers:
```cmd
cd src\DanceCourseCreator.Tests.E2E
pwsh bin\Debug\net10.0\playwright.ps1 uninstall
pwsh bin\Debug\net10.0\playwright.ps1 install chromium
```

---

## Debugging Tests

### Run with headed browser (visible)

Set environment variable before running:
```cmd
set HEADED=1
playwright-tests.cmd
```

### Run with slow motion

```cmd
set SLOW_MO=1000
playwright-tests.cmd
```

### Run single test

```cmd
cd src\DanceCourseCreator.Tests.E2E
dotnet test --filter "FullyQualifiedName~CourseCreation_OpenCreateDialog"
```

### View test output

```cmd
dotnet test --logger "console;verbosity=detailed"
```

---

## Environment Setup Checklist

Before running E2E tests, ensure:

- [ ] .NET 10 SDK installed
- [ ] PowerShell 7+ installed (or Node.js)
- [ ] Playwright browsers installed (run `install-playwright.cmd`)
- [ ] Ports 5034, 5139 are available
- [ ] No other instances of the app running
- [ ] Database files are not locked

---

## Scripts Overview

| Script | Purpose | When to Use |
|--------|---------|-------------|
| `install-playwright.cmd` | Install Playwright browsers | First time setup, after errors |
| `playwright-tests.cmd` | Run all E2E tests | After installation, for full test run |
| `dotnet test` | Run tests manually | For debugging specific tests |

---

## Getting Help

### Check logs

```cmd
cd src\DanceCourseCreator.Tests.E2E
dotnet test --logger "console;verbosity=diagnostic" > test-output.txt 2>&1
```

### Create minimal reproduction

```cmd
# Test if API works
cd src\DanceCourseCreator.API
dotnet run

# In another terminal, test if Web works
cd src\DanceCourseCreator.Web
dotnet run

# Verify with curl
curl http://localhost:5139/api/health
curl http://localhost:5034
```

### GitHub Issues

If you still have issues, create a GitHub issue with:
1. Output from `dotnet --version`
2. Output from `pwsh --version` or `node --version`
3. Content of test-output.txt
4. Steps you've already tried

---

## Quick Commands Reference

```cmd
# Install browsers (first time)
install-playwright.cmd

# Run all tests
playwright-tests.cmd

# Run specific test category
cd src\DanceCourseCreator.Tests.E2E
dotnet test --filter "TestCategory=Integration"

# Run with visual browser
set HEADED=1 && dotnet test

# Clean and rebuild
dotnet clean
dotnet build
pwsh bin\Debug\net10.0\playwright.ps1 install chromium

# Stop all apps
taskkill /F /IM dotnet.exe
```

---

## See Also

- [TEST-EXPLORER-FIX.md](TEST-EXPLORER-FIX.md) - Test Explorer issues
- [FIX-TEST-DISCOVERY.md](FIX-TEST-DISCOVERY.md) - Test discovery troubleshooting
- [E2E-TESTING-GUIDE.md](E2E-TESTING-GUIDE.md) - Complete E2E testing guide
- [PLAYWRIGHT-TEST-RUNNER-ANALYSIS.md](PLAYWRIGHT-TEST-RUNNER-ANALYSIS.md) - Runner analysis

---

**Still having issues? The most common cause is not having Playwright browsers installed. Run `install-playwright.cmd` first!**
