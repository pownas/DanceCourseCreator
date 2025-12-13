# E2E Tests - Quick Fix Summary

## Problem

Running `playwright-tests.cmd` or `dotnet test` resulted in:
```
No test is available in DanceCourseCreator.Tests.E2E.dll
```

## Root Cause

**Playwright browsers were not installed.** The tests require Chromium browser binaries (~200MB) to run, which are not included in the repository.

## Solution

### Quick Fix (Recommended)

Run the installation script:
```cmd
install-playwright.cmd
```

Then run tests:
```cmd
playwright-tests.cmd
```

### Manual Fix

```cmd
cd src\DanceCourseCreator.Tests.E2E
dotnet build
pwsh bin\Debug\net10.0\playwright.ps1 install chromium
```

## What Was Changed

### ✅ Files Created

1. **`install-playwright.cmd`**
   - Standalone script to install Playwright browsers
   - Handles errors gracefully
   - Shows installation progress

2. **`verify-e2e-setup.cmd`**
   - Checks all prerequisites
   - Verifies environment is ready
   - Reports missing components

3. **`playwright-tests.cmd` (Enhanced)**
   - Now checks for Playwright browsers before running
   - Auto-installs if missing
   - Better error handling and progress indicators

4. **`docs/PLAYWRIGHT-TROUBLESHOOTING.md`**
   - Complete troubleshooting guide
   - Common issues and solutions
   - Debugging commands

### ✅ Files Updated

1. **`src/DanceCourseCreator.Tests.E2E/README.md`**
   - Added prominent Windows quick start section
   - Added "Common Issue" section at top
   - Added helper scripts table

## How to Use (Step by Step)

### First Time Setup

```cmd
# Step 1: Verify environment (optional)
verify-e2e-setup.cmd

# Step 2: Install Playwright browsers
install-playwright.cmd

# Step 3: Run tests
playwright-tests.cmd
```

### Regular Usage

After initial setup, just run:
```cmd
playwright-tests.cmd
```

## Verification

After running `install-playwright.cmd`, you should see:
```
✓ Playwright Chromium browser is now installed.
  You can now run E2E tests with: playwright-tests.cmd
```

And the folder `src\DanceCourseCreator.Tests.E2E\bin\Debug\net10.0\.playwright` should exist with browser files.

## If It Still Doesn't Work

### 1. Check PowerShell

```cmd
pwsh --version
```

If not installed:
- Download from: https://github.com/PowerShell/PowerShell/releases
- Or use Node.js alternative (see troubleshooting guide)

### 2. Use Verification Script

```cmd
verify-e2e-setup.cmd
```

This will tell you exactly what's missing.

### 3. Manual Verification

```cmd
# Check if browsers are installed
dir src\DanceCourseCreator.Tests.E2E\bin\Debug\net10.0\.playwright

# Check if apps can start
cd src\DanceCourseCreator.API
dotnet run --urls http://localhost:5139
# (Ctrl+C to stop)

cd ..\DanceCourseCreator.Web
dotnet run --urls http://localhost:5034
# (Ctrl+C to stop)
```

### 4. Check Documentation

Full troubleshooting guide:
```
docs\PLAYWRIGHT-TROUBLESHOOTING.md
```

## Why This Happened

Playwright is a browser automation framework that requires actual browser binaries. These are:
- **Large** (~200MB for Chromium)
- **Platform-specific** (Windows, Linux, macOS)
- **Not included in NuGet packages** (must be downloaded separately)

This is by design to keep the test project lightweight and allow different browser configurations.

## Related Documentation

- **`docs/PLAYWRIGHT-TROUBLESHOOTING.md`** - Complete troubleshooting
- **`docs/TEST-EXPLORER-FIX.md`** - Test Explorer issues
- **`docs/FIX-TEST-DISCOVERY.md`** - Test discovery problems
- **`docs/PLAYWRIGHT-TEST-RUNNER-ANALYSIS.md`** - Runner analysis
- **`docs/E2E-TESTING-GUIDE.md`** - Complete E2E testing guide

## Quick Command Reference

```cmd
# Install browsers
install-playwright.cmd

# Verify setup
verify-e2e-setup.cmd

# Run all tests
playwright-tests.cmd

# Run specific test
cd src\DanceCourseCreator.Tests.E2E
dotnet test --filter "FullyQualifiedName~HealthCheck"

# Run with visible browser (debugging)
set HEADED=1
dotnet test
```

## Summary

✅ **Problem:** "No test is available" error  
✅ **Cause:** Playwright browsers not installed  
✅ **Solution:** Run `install-playwright.cmd`  
✅ **Verification:** Run `verify-e2e-setup.cmd`  
✅ **Testing:** Run `playwright-tests.cmd`  

**Your E2E tests should now work! 🎭✅**
