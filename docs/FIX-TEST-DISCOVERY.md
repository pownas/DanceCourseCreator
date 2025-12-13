# Fix Test Explorer Discovery Issues

## Problem
Getting error: "Not all tests from the test run selection could be discovered. Make sure to build your test project."

E2E tests (Playwright tests) are not being discovered in Test Explorer.

## Root Cause
Test Explorer logs show:
```
No test is available in C:\GitHub\DanceCourseCreator\src\DanceCourseCreator.Tests.E2E\bin\Debug\net10.0\DanceCourseCreator.Tests.E2E.dll
```

This indicates that Playwright MSTest tests are not being discovered by the VSTest adapter.

## Solutions

### Solution 1: Clear Test Explorer Cache (Try This First) ✅

**Step 1: Close Visual Studio completely**

**Step 2: Delete Test Explorer cache:**

**PowerShell:**
```powershell
# Delete Test Explorer cache
Remove-Item -Path "$env:TEMP\VisualStudioTestExplorerExtensions" -Recurse -Force -ErrorAction SilentlyContinue

# Also delete TestStore
Remove-Item -Path "$env:LOCALAPPDATA\Microsoft\VisualStudio\*\ComponentModelCache\TestStore" -Recurse -Force -ErrorAction SilentlyContinue
```

**Command Prompt:**
```cmd
rmdir /s /q "%TEMP%\VisualStudioTestExplorerExtensions"
rmdir /s /q "%LOCALAPPDATA%\Microsoft\VisualStudio\18.0\ComponentModelCache\TestStore"
```

**Step 3: Restart Visual Studio**

**Step 4: Clean and Rebuild:**
```
Build → Clean Solution
Build → Rebuild Solution
```

**Step 5: Refresh Test Explorer:**
```
Test → Test Explorer (Ctrl+E, T)
Click the refresh icon
```

### Solution 2: Verify Playwright Installation

The E2E tests require Playwright browsers to be installed:

```bash
cd src/DanceCourseCreator.Tests.E2E
dotnet build
pwsh bin/Debug/net10.0/playwright.ps1 install
```

### Solution 3: Add Explicit Test SDK Package

Sometimes adding the explicit test SDK helps with discovery:

**Add to `DanceCourseCreator.Tests.E2E.csproj`:**

```xml
<ItemGroup>
  <PackageReference Include="Microsoft.NET.Test.Sdk" Version="18.0.1" />
  <PackageReference Include="MSTest" Version="4.0.2" />
  <!-- ... rest of packages ... -->
</ItemGroup>
```

### Solution 4: Verify Test Methods Are Proper

Make sure your E2E tests have proper attributes. Each test should have:

```csharp
[TestClass]
public class YourTestClass : PageTest
{
    [TestMethod]
    public async Task YourTest_Should_Work()
    {
        // Test code
    }
}
```

### Solution 5: Check for Inheritance Issues

Playwright tests inherit from `PageTest`. Make sure you're **NOT** using the `PlaywrightE2ETestBase` class that has `ClassInitialize` - this can cause discovery issues.

**Instead of:**
```csharp
public class YourTests : PlaywrightE2ETestBase // ❌ May cause issues
```

**Use:**
```csharp
public class YourTests : PageTest // ✅ Direct inheritance
```

### Solution 6: Run Tests from Command Line to Verify

Verify tests work outside of Test Explorer:

```bash
# Verify tests can be listed
dotnet test src/DanceCourseCreator.Tests.E2E --list-tests

# Run tests
dotnet test src/DanceCourseCreator.Tests.E2E
```

If tests work from command line but not Test Explorer, it's definitely a Test Explorer cache/discovery issue.

### Solution 7: Check Visual Studio Extensions

Make sure you have the latest test adapters:

1. **Tools → Extensions and Updates**
2. Check for updates to:
   - MSTest Test Adapter
   - Test Adapter for MSTest

### Solution 8: Nuclear Option - Reset Visual Studio

If nothing else works:

```
Tools → Import and Export Settings → Reset all settings
```

Then reinstall test adapters and rebuild.

## Recommended Workflow

**For Development (Recommended):**

Since Test Explorer has issues with Playwright tests, use command line:

```bash
# Terminal 1 - Start API
cd src/DanceCourseCreator.API
dotnet run --urls "http://localhost:5139"

# Terminal 2 - Start Web
cd src/DanceCourseCreator.Web
dotnet run --urls "http://localhost:5034"

# Terminal 3 - Run E2E tests
cd src/DanceCourseCreator.Tests.E2E
dotnet test

# Or run specific test
dotnet test --filter "FullyQualifiedName~CourseCreation"

# Or run by category
dotnet test --filter "TestCategory=Integration"
```

**For Unit Tests (Work Great in Test Explorer):**

The Web.Tests (25 tests) should work fine in Test Explorer:

```
Test Explorer → DanceCourseCreator.Web.Tests → Run All
```

## Why Playwright Tests Have Issues

Playwright tests are special because:

1. They require browser automation infrastructure
2. They inherit from `PageTest` which has special initialization
3. They need Playwright browsers installed
4. VSTest adapter sometimes struggles with their discovery

This is why many teams run Playwright tests from command line or CI/CD, not Test Explorer.

## Current Status

Based on logs:
- ✅ **Web.Tests**: 25 tests discovered successfully
- ❌ **E2E Tests**: Not being discovered ("No test is available")

## Quick Fix Summary

**Fastest Solution:**

1. Close Visual Studio
2. Delete: `%TEMP%\VisualStudioTestExplorerExtensions`
3. Restart Visual Studio  
4. Clean Solution
5. Rebuild Solution
6. Refresh Test Explorer

If E2E tests still don't appear, **use command line** for E2E tests and Test Explorer for unit tests.

## Alternative: Use Two Testing Approaches

**Test Explorer (IDE):**
- ✅ Use for: Web.Tests (unit tests, component tests)
- ✅ Interactive debugging
- ✅ Quick feedback

**Command Line:**
- ✅ Use for: E2E Tests (Playwright tests)
- ✅ More reliable
- ✅ CI/CD compatible
- ✅ Full control

This is actually the **industry standard approach** - unit tests in IDE, E2E tests from command line.

## Further Reading

- [Playwright .NET Testing](https://playwright.dev/dotnet/docs/test-runners)
- [MSTest Test Discovery Issues](https://github.com/microsoft/testfx/issues)
- [Visual Studio Test Explorer Troubleshooting](https://learn.microsoft.com/en-us/visualstudio/test/troubleshoot-test-explorer)

---

**TL;DR**: Clear Test Explorer cache, rebuild, and if E2E tests still don't show, use `dotnet test` from command line for E2E tests. This is normal and recommended for Playwright tests.
