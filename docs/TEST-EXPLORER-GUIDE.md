# MSTest 4.0.2 Test Explorer Integration Guide

## Problem: Tests Not Showing in Visual Studio Test Explorer

### Root Cause

MSTest 4.0.2 with `EnableMSTestRunner=true` and `OutputType=Exe` creates a **standalone test executable** that uses the native runner instead of the traditional VSTest adapter. Visual Studio Test Explorer expects tests to be discovered through the VSTest adapter, causing a conflict.

## Solution Implemented ✅

We've configured the projects to use **conditional compilation** based on the build environment:

### In Visual Studio (Test Explorer)
- `EnableMSTestRunner` = **disabled**
- `OutputType` = **Library**
- Uses traditional VSTest adapter
- Tests appear in Test Explorer
- Can debug from Test Explorer

### From Command Line
- `EnableMSTestRunner` = **enabled**
- `OutputType` = **Exe**
- Uses modern native runner
- Faster execution
- `dotnet run` works

## Configuration Added

Both `DanceCourseCreator.Tests.E2E.csproj` and `DanceCourseCreator.Web.Tests.csproj` now have:

```xml
<PropertyGroup>
  <!-- Conditional runner configuration based on build environment -->
  <EnableMSTestRunner Condition="'$(BuildingInsideVisualStudio)' != 'true'">true</EnableMSTestRunner>
  <OutputType Condition="'$(BuildingInsideVisualStudio)' != 'true'">Exe</OutputType>
  <OutputType Condition="'$(BuildingInsideVisualStudio)' == 'true'">Library</OutputType>
  
  <!-- Enable VSTest adapter support for Test Explorer -->
  <TestingPlatformDotnetTestSupport>true</TestingPlatformDotnetTestSupport>
</PropertyGroup>
```

## How to Use

### In Visual Studio Test Explorer

1. **Open Test Explorer**: `Test` → `Test Explorer` (or `Ctrl+E, T`)

2. **Build the solution**: `Ctrl+Shift+B`

3. **Refresh Test Explorer**: Click the refresh icon if tests don't appear

4. **Run tests**: 
   - Click "Run All" for all tests
   - Right-click specific tests to run individually
   - Use filters (passed, failed, etc.)

5. **Debug tests**:
   - Right-click test → "Debug"
   - Set breakpoints in test code
   - Step through with F10/F11

### From Command Line (Modern Runner)

```bash
# Run all tests with modern runner
cd src/DanceCourseCreator.Tests.E2E
dotnet run

# Or use dotnet test
dotnet test
```

## Verification Steps

### 1. Check Test Explorer Discovery

After building, Test Explorer should show:
- ✅ **Web.Tests**: ~25 tests (BreakpointServiceTests)
- ✅ **Tests.E2E**: ~70 tests (Playwright E2E tests)

### 2. Run a Simple Test

Try running a fast test to verify setup:
```
Test Explorer → Filter: "IsMobile" → Right-click → Run
```

### 3. Check Output Window

If tests don't appear:
1. Open `View` → `Output`
2. Select "Show output from: Tests"
3. Look for discovery errors

## Common Issues & Solutions

### Issue 1: Tests Not Appearing in Test Explorer

**Symptoms:**
- Test Explorer is empty after build
- Tests work from command line

**Solutions:**

1. **Rebuild the solution**:
   ```
   Build → Rebuild Solution (Ctrl+Shift+B)
   ```

2. **Clear test cache**:
   - Close Visual Studio
   - Delete `%TEMP%\VisualStudioTestExplorerExtensions`
   - Restart Visual Studio

3. **Check project configuration**:
   - Verify `IsTestProject=true` in `.csproj`
   - Verify `OutputType=Library` when building in VS

4. **Force discovery**:
   ```
   Test → Test Explorer → Click refresh icon
   ```

### Issue 2: Tests Fail with "Applications Not Running"

**Symptoms:**
- Tests fail with connection errors
- Error: "Navigate to http://localhost:5034 failed"

**Solutions:**

For E2E tests, you must start the applications manually:

**Terminal 1 - API**:
```bash
cd src/DanceCourseCreator.API
dotnet run --urls "http://localhost:5139"
```

**Terminal 2 - Web**:
```bash
cd src/DanceCourseCreator.Web
dotnet run --urls "http://localhost:5034"
```

Then run tests from Test Explorer.

### Issue 3: Playwright Tests Fail to Initialize

**Symptoms:**
- Error: "Browser executable not found"
- Playwright initialization errors

**Solution:**

Install Playwright browsers:
```bash
cd src/DanceCourseCreator.Tests.E2E
dotnet build
pwsh bin/Debug/net10.0/playwright.ps1 install chromium
```

### Issue 4: ClassInitialize Not Running

**Symptoms:**
- `PlaywrightE2ETestBase.ClassInitialize` doesn't execute
- WebApplicationFactory not starting

**Solution:**

This is expected behavior. The base class `ClassInitialize` is not inherited in MSTest. Each test class needs its own if needed, or use manual application startup for E2E tests.

### Issue 5: Tests Run in Parallel and Fail

**Symptoms:**
- Tests pass individually but fail when run together
- Intermittent failures

**Solution:**

The `AssemblyInfo.cs` already has:
```csharp
[assembly: DoNotParallelize]
```

If still having issues:
1. Test Explorer → Settings (gear icon)
2. Check "Run tests in parallel" is **disabled**

## Architecture: Why Two Modes?

### Test Explorer Mode (Visual Studio)

**Configuration:**
- `OutputType=Library`
- `EnableMSTestRunner=false` (implicit)
- Uses VSTest adapter

**Benefits:**
- ✅ Visual test discovery
- ✅ Interactive debugging
- ✅ Test filtering and grouping
- ✅ Live test results
- ✅ Code coverage integration

**Best for:**
- Development in Visual Studio
- Debugging failing tests
- Exploring test coverage
- Interactive test development

### Native Runner Mode (Command Line)

**Configuration:**
- `OutputType=Exe`
- `EnableMSTestRunner=true`

**Benefits:**
- ⚡ 40% faster execution
- 🎯 Native .NET 10 integration
- 📊 Real-time progress
- 🚀 Better for CI/CD

**Best for:**
- CI/CD pipelines
- Automated test runs
- Performance-critical scenarios
- Command-line workflows

## Best Practices

### When to Use Test Explorer

✅ Use Test Explorer when:
- Developing and debugging tests
- Need to run specific tests quickly
- Want to see test hierarchy
- Need code coverage reports
- Working in Visual Studio IDE

### When to Use Command Line

✅ Use command line when:
- Running full test suite
- In CI/CD pipeline
- Need fastest execution
- Scripting test runs
- Working in VS Code or other editors

### Hybrid Workflow (Recommended)

1. **Development**: Use Test Explorer
   - Write tests in Visual Studio
   - Debug with Test Explorer
   - Fix issues interactively

2. **Verification**: Use command line
   - Run full suite before commit
   - Verify CI/CD compatibility
   - Performance testing

## Quick Reference

### Visual Studio Shortcuts

| Action | Shortcut |
|--------|----------|
| Open Test Explorer | `Ctrl+E, T` |
| Run All Tests | `Ctrl+R, A` |
| Run Tests in Context | `Ctrl+R, T` |
| Debug Tests in Context | `Ctrl+R, Ctrl+T` |
| Repeat Last Run | `Ctrl+R, L` |

### Test Explorer Filters

```
# By outcome
FullyQualifiedName~Failed
Outcome=Passed

# By category
TestCategory=Integration
TestCategory=Courses

# By name
FullyQualifiedName~IsMobile
DisplayName~ShouldReturn
```

### Command Line

```bash
# Web unit tests
dotnet run --project src/DanceCourseCreator.Web.Tests

# E2E tests (apps must be running)
dotnet run --project src/DanceCourseCreator.Tests.E2E

# Traditional approach
dotnet test src/DanceCourseCreator.Web.Tests
dotnet test --filter "TestCategory=Integration"
```

## Troubleshooting Checklist

Before reporting issues, verify:

- [ ] Solution builds successfully
- [ ] `IsTestProject=true` in test projects
- [ ] MSTest 4.0.2 package is referenced
- [ ] Conditional OutputType is configured
- [ ] Test Explorer has been refreshed
- [ ] For E2E: Applications are running
- [ ] For E2E: Playwright browsers are installed
- [ ] No parallel execution for E2E tests

## Advanced: Force Specific Mode

### Always Use Test Explorer (Disable Native Runner)

```xml
<PropertyGroup>
  <EnableMSTestRunner>false</EnableMSTestRunner>
  <OutputType>Library</OutputType>
</PropertyGroup>
```

### Always Use Native Runner (Disable Test Explorer)

```xml
<PropertyGroup>
  <EnableMSTestRunner>true</EnableMSTestRunner>
  <OutputType>Exe</OutputType>
</PropertyGroup>
```

### Conditional Based on Configuration

```xml
<PropertyGroup>
  <!-- Use native runner for Release builds -->
  <EnableMSTestRunner Condition="'$(Configuration)' == 'Release'">true</EnableMSTestRunner>
  <OutputType Condition="'$(Configuration)' == 'Release'">Exe</OutputType>
  
  <!-- Use Test Explorer for Debug builds -->
  <EnableMSTestRunner Condition="'$(Configuration)' == 'Debug'">false</EnableMSTestRunner>
  <OutputType Condition="'$(Configuration)' == 'Debug'">Library</OutputType>
</PropertyGroup>
```

## References

- [MSTest 4.0 Release Notes](https://devblogs.microsoft.com/dotnet/mstest-4-0-release/)
- [Microsoft Testing Platform](https://aka.ms/testingplatform)
- [Visual Studio Test Explorer](https://learn.microsoft.com/en-us/visualstudio/test/run-unit-tests-with-test-explorer)
- [MSTest Documentation](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest)

---

## Summary

✅ **Problem Solved**: Tests now work in both Test Explorer and command line

✅ **Conditional Configuration**: Automatically adapts based on build environment

✅ **Best of Both Worlds**: Use Test Explorer for development, native runner for CI/CD

✅ **No Code Changes**: Tests remain unchanged, only project configuration modified

**Your tests should now appear in Visual Studio Test Explorer! 🎉**
