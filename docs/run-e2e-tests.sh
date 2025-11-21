#!/bin/bash

# Script to help run E2E tests for Dance Course Creator
# This script checks prerequisites and guides users through running tests

set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"
TEST_PROJECT="$PROJECT_ROOT/src/DanceCourseCreator.Tests.E2E"

echo "==================================="
echo "Dance Course Creator - E2E Tests"
echo "==================================="
echo ""

# Check if .NET is installed
if ! command -v dotnet &> /dev/null; then
    echo "❌ Error: .NET SDK is not installed"
    echo "Please install .NET 8.0 SDK from https://dotnet.microsoft.com/download"
    exit 1
fi

echo "✅ .NET SDK found: $(dotnet --version)"
echo ""

# Check if in correct directory
if [ ! -d "$TEST_PROJECT" ]; then
    echo "❌ Error: Cannot find test project directory"
    echo "Expected: $TEST_PROJECT"
    exit 1
fi

# Build test project
echo "📦 Building test project..."
cd "$TEST_PROJECT"
dotnet build --verbosity quiet

if [ $? -ne 0 ]; then
    echo "❌ Build failed"
    exit 1
fi

echo "✅ Build successful"
echo ""

# Check if Playwright browsers are installed
PLAYWRIGHT_DIR="$TEST_PROJECT/bin/Debug/net8.0/.playwright"
if [ ! -d "$PLAYWRIGHT_DIR" ]; then
    echo "⚠️  Playwright browsers not installed"
    echo "Installing Playwright Chromium browser..."
    
    # Try multiple installation methods for cross-platform compatibility
    if command -v pwsh &> /dev/null; then
        echo "Using PowerShell to install Playwright..."
        pwsh "$TEST_PROJECT/bin/Debug/net8.0/playwright.ps1" install chromium
    elif command -v node &> /dev/null && [ -f "$TEST_PROJECT/bin/Debug/net8.0/.playwright/package/cli.js" ]; then
        echo "Using Node.js to install Playwright..."
        node "$TEST_PROJECT/bin/Debug/net8.0/.playwright/package/cli.js" install chromium
    else
        echo "❌ Could not find PowerShell (pwsh) or Node.js"
        echo "Please install Playwright browsers manually using one of these methods:"
        echo "  Method 1 (PowerShell): pwsh bin/Debug/net8.0/playwright.ps1 install chromium"
        echo "  Method 2 (Node.js): node bin/Debug/net8.0/.playwright/package/cli.js install chromium"
        exit 1
    fi
    
    echo "✅ Playwright browsers installed"
else
    echo "✅ Playwright browsers already installed"
fi

echo ""
echo "==================================="
echo "Prerequisites Check"
echo "==================================="
echo ""

# Check if API is running
API_URL="http://localhost:5139/api/health"
echo "Checking if API is running at http://localhost:5139..."
if curl -s --max-time 2 "$API_URL" > /dev/null 2>&1; then
    echo "✅ API is running"
else
    echo "⚠️  API is not running"
    echo ""
    echo "Please start the API in another terminal:"
    echo "  cd src/DanceCourseCreator.API"
    echo "  dotnet run --urls \"http://localhost:5139\""
    echo ""
fi

# Check if Client is running
CLIENT_URL="http://localhost:5034"
echo "Checking if Client is running at http://localhost:5034..."
if curl -s --max-time 2 "$CLIENT_URL" > /dev/null 2>&1; then
    echo "✅ Client is running"
else
    echo "⚠️  Client is not running"
    echo ""
    echo "Please start the Client in another terminal:"
    echo "  cd src/DanceCourseCreator.Client"
    echo "  dotnet run --urls \"http://localhost:5034\""
    echo ""
fi

echo ""
echo "==================================="
echo "Running Tests"
echo "==================================="
echo ""

# Ask user what tests to run
echo "Select which tests to run:"
echo "  1) All tests"
echo "  2) Navigation tests only"
echo "  3) Pattern library tests only"
echo "  4) Course tests only"
echo "  5) Lesson & template tests only"
echo "  6) Screenshot tests only"
echo ""
read -p "Enter choice (1-6, default: 1): " choice
choice=${choice:-1}

TEST_FILTER=""
case $choice in
    1)
        echo "Running all tests..."
        ;;
    2)
        TEST_FILTER="--filter TestCategory=Navigation"
        echo "Running navigation tests..."
        ;;
    3)
        TEST_FILTER="--filter TestCategory=Patterns"
        echo "Running pattern library tests..."
        ;;
    4)
        TEST_FILTER="--filter TestCategory=Courses"
        echo "Running course tests..."
        ;;
    5)
        TEST_FILTER="--filter \"TestCategory=Lessons | TestCategory=Templates\""
        echo "Running lesson & template tests..."
        ;;
    6)
        TEST_FILTER="--filter TestCategory=Screenshots"
        echo "Running all screenshot tests..."
        ;;
    *)
        echo "Invalid choice, running all tests..."
        ;;
esac

echo ""
cd "$TEST_PROJECT"
dotnet test $TEST_FILTER --logger "console;verbosity=normal"

TEST_RESULT=$?

echo ""
echo "==================================="
echo "Test Results"
echo "==================================="
echo ""

if [ $TEST_RESULT -eq 0 ]; then
    echo "✅ All tests passed!"
else
    echo "❌ Some tests failed"
fi

echo ""
echo "Screenshots saved in: $TEST_PROJECT/screenshots/"
echo ""
echo "For more information, see:"
echo "  - README.md: $TEST_PROJECT/README.md"
echo "  - Test Documentation: $TEST_PROJECT/TEST_DOCUMENTATION.md"
echo ""
