# End-to-End Testing with Playwright

This directory contains Playwright end-to-end tests for the Dance Course Creator application.

## Test Files

### `demo-login.spec.ts`
A smoke test that verifies the basic functionality of the application:
- ✅ Login page loads correctly
- ✅ Demo user can login with credentials `demo@example.com` / `password123`
- ✅ Dashboard loads after successful login
- ✅ Patterns page is accessible
- ✅ Screenshots are captured at each step

This test is tagged with `@smoke` for easy filtering and is designed to quickly verify that the system loads correctly for the demo user and that the interface displays as expected.

## Prerequisites

1. Install dependencies:
   ```bash
   npm install
   ```

2. Start the development server (both client and server):
   ```bash
   # From the legacy/ directory
   npm run dev
   ```

3. The application should be running on:
   - Client: http://localhost:3000
   - Server: http://localhost:3001

## Running Tests

### Run all E2E tests
```bash
npm run test:e2e
```

### Run only smoke tests
```bash
npm run test:smoke
```

### Run tests with UI mode (interactive)
```bash
npm run test:e2e:ui
```

### Run specific test file
```bash
npx playwright test demo-login.spec.ts
```

## Screenshots

Tests automatically capture screenshots at key steps and save them to `test-results/`:
- `01-login-page.png` - Initial login page
- `02-login-form-filled.png` - Login form with demo credentials filled
- `03-dashboard.png` - Dashboard after successful login
- `04-patterns-page.png` - Patterns/course page

Screenshots are excluded from git via `.gitignore`.

## Test Configuration

The Playwright configuration (`playwright.config.ts`) is set up to:
- Use system Chromium browser
- Run tests against `http://localhost:3000`
- Take screenshots on failure
- Generate traces for debugging
- Use HTML reporter for detailed results

## Demo Credentials

The smoke test uses the demo user credentials that are displayed on the login page:
- **Email**: demo@example.com
- **Password**: password123

## Tags

Tests use the following tags for easy filtering:
- `@smoke` - Quick smoke tests for basic functionality verification

## Troubleshooting

### Test fails with "Executable doesn't exist"
The test is configured to use system Chromium. Ensure it's installed:
```bash
which chromium
```

### Application not starting
Make sure both client and server dependencies are installed and the development server is running:
```bash
cd legacy/
npm install
npm run dev
```

### Network timeouts
Increase timeout values in `playwright.config.ts` if the application takes longer to start.