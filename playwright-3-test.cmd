@echo off
REM Quick script to run Playwright E2E tests on Windows

echo ========================================
echo DanceCourseCreator E2E Test Runner
echo ========================================
echo.

REM Check if Playwright browsers are installed
echo [1/5] Checking Playwright browsers...
if not exist "src\DanceCourseCreator.Tests.E2E\bin\Debug\net10.0\.playwright" (
    echo Playwright browsers not found. Installing...
    echo.
    cd src\DanceCourseCreator.Tests.E2E
    dotnet build
    echo.
    echo Installing Chromium browser for Playwright...
    pwsh bin\Debug\net10.0\playwright.ps1 install chromium
    if errorlevel 1 (
        echo.
        echo ERROR: Failed to install Playwright browsers
        echo Please install manually:
        echo   cd src\DanceCourseCreator.Tests.E2E
        echo   pwsh bin\Debug\net10.0\playwright.ps1 install chromium
        echo.
        pause
        exit /b 1
    )
    cd ..\..
    echo.
    echo Playwright browsers installed successfully!
) else (
    echo Playwright browsers already installed.
)
echo.

echo [2/5] Starting API application...
start "DanceCourseCreator API" cmd /c "cd src\DanceCourseCreator.API && dotnet run --urls http://localhost:5139"

echo [3/5] Starting Web application...
start "DanceCourseCreator Web" cmd /c "cd src\DanceCourseCreator.Web && dotnet run --urls http://localhost:5034"

echo [4/5] Waiting for applications to start...
timeout /t 15 /nobreak > nul

echo [5/5] Running E2E tests...
echo.

REM Run tests
cd src\DanceCourseCreator.Tests.E2E
dotnet test --logger "console;verbosity=detailed"

set TEST_RESULT=%ERRORLEVEL%

echo.
echo ========================================
if %TEST_RESULT% equ 0 (
    echo Tests completed successfully!
) else (
    echo Tests failed or had errors!
)
echo ========================================
echo.
echo Press any key to stop applications...
pause > nul

REM Kill the started processes
echo Stopping applications...
taskkill /FI "WindowTitle eq DanceCourseCreator API*" /F > nul 2>&1
taskkill /FI "WindowTitle eq DanceCourseCreator Web*" /F > nul 2>&1

echo Applications stopped.
echo.
