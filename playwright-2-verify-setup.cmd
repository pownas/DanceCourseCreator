@echo off
REM Script to verify E2E test environment is ready

echo ========================================
echo E2E Test Environment Verification
echo ========================================
echo.

set ALL_OK=1

REM Check .NET SDK
echo [1/6] Checking .NET SDK...
dotnet --version > nul 2>&1
if errorlevel 1 (
    echo [FAIL] .NET SDK not found
    echo        Install from: https://dotnet.microsoft.com/download
    set ALL_OK=0
) else (
    for /f "tokens=*" %%i in ('dotnet --version') do set DOTNET_VERSION=%%i
    echo [OK]   .NET SDK version: %DOTNET_VERSION%
)
echo.

REM Check PowerShell
echo [2/6] Checking PowerShell...
pwsh --version > nul 2>&1
if errorlevel 1 (
    echo [WARN] PowerShell 7+ not found
    echo        Install from: https://github.com/PowerShell/PowerShell/releases
    echo        OR use Node.js alternative
) else (
    for /f "tokens=*" %%i in ('pwsh --version') do set PWSH_VERSION=%%i
    echo [OK]   PowerShell: %PWSH_VERSION%
)
echo.

REM Check if test project builds
echo [3/6] Checking test project...
cd src\DanceCourseCreator.Tests.E2E
dotnet build --verbosity quiet > nul 2>&1
if errorlevel 1 (
    echo [FAIL] Test project build failed
    echo        Run: dotnet build src\DanceCourseCreator.Tests.E2E
    set ALL_OK=0
) else (
    echo [OK]   Test project builds successfully
)
cd ..\..
echo.

REM Check Playwright browsers
echo [4/6] Checking Playwright browsers...
if exist "src\DanceCourseCreator.Tests.E2E\bin\Debug\net10.0\.playwright" (
    echo [OK]   Playwright browsers installed
) else (
    echo [FAIL] Playwright browsers not installed
    echo        Run: install-playwright.cmd
    set ALL_OK=0
)
echo.

REM Check ports availability
echo [5/6] Checking port availability...
netstat -ano | findstr ":5139" > nul 2>&1
if errorlevel 1 (
    echo [OK]   Port 5139 (API) is available
) else (
    echo [WARN] Port 5139 (API) is in use
    echo        Stop existing processes: taskkill /F /IM dotnet.exe
)

netstat -ano | findstr ":5034" > nul 2>&1
if errorlevel 1 (
    echo [OK]   Port 5034 (Web) is available
) else (
    echo [WARN] Port 5034 (Web) is in use
    echo        Stop existing processes: taskkill /F /IM dotnet.exe
)
echo.

REM Check database
echo [6/6] Checking database...
if exist "src\DanceCourseCreator.API\database.sqlite" (
    echo [OK]   Database file exists
) else (
    echo [INFO] Database will be created on first run
)
echo.

REM Summary
echo ========================================
echo Summary
echo ========================================
if %ALL_OK% equ 1 (
    echo [OK] Environment is ready for E2E tests!
    echo.
    echo Next steps:
    echo   1. Run: playwright-tests.cmd
    echo   2. Or manually start apps and run: dotnet test
    echo.
) else (
    echo [FAIL] Environment setup incomplete!
    echo.
    echo Required actions:
    if not exist "src\DanceCourseCreator.Tests.E2E\bin\Debug\net10.0\.playwright" (
        echo   1. Install Playwright browsers: install-playwright.cmd
    )
    echo.
    echo After fixing issues, run this script again.
    echo.
)

pause
