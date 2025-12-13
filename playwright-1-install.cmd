@echo off
REM Script to install Playwright browsers for E2E tests

echo === Playwright Browser Installation ===

echo This will install Chromium browser for Playwright tests.
echo Installation size: ~200MB

pause

echo Building test project...
cd src\DanceCourseCreator.Tests.E2E
dotnet build

if errorlevel 1 (
    echo === ‼️ ERROR: Build failed! ‼️ ===
    echo Please fix build errors before installing Playwright browsers.
    pause
    exit /b 1
)

echo Installing Chromium browser...
echo This may take a few minutes...

pwsh bin\Debug\net10.0\playwright.ps1 install chromium

if errorlevel 1 (
    echo === ‼️ ERROR: Installation failed! ‼️ ===

    echo Troubleshooting:
    echo 1. Make sure PowerShell is installed
    echo 2. Try running as Administrator
    echo 3. Check your internet connection

    echo Alternative installation:
    echo   node bin\Debug\net10.0\.playwright\package\cli.js install chromium
    pause
    exit /b 1
)

echo === ✅ Installation successful! ✅ ===
echo Playwright Chromium browser is now installed.
echo You can now run E2E tests with: "playwright-3-test.cmd"
pause
