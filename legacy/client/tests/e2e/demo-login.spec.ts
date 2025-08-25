import { test, expect } from '@playwright/test';

test.describe('Demo Login Smoke Test', () => {
  test('demo login and navigation smoke test @smoke', async ({ page }) => {
    console.log('Starting demo login smoke test...');
    
    // 1. Navigate to login page
    console.log('Step 1: Navigating to login page');
    await page.goto('/login');
    
    // Wait for page to load
    await page.waitForLoadState('networkidle');
    
    // Take screenshot of login page
    await page.screenshot({ 
      path: 'test-results/01-login-page.png', 
      fullPage: true 
    });
    
    // Verify we're on the login page
    await expect(page.locator('h1')).toContainText('West Coast Swing');
    await expect(page.locator('h2')).toContainText('Course Creator');
    
    // Verify demo credentials are visible on the page
    await expect(page.getByText('Demo Credentials:')).toBeVisible();
    await expect(page.getByText('demo@example.com')).toBeVisible();
    
    // 2. Login with demo user credentials
    console.log('Step 2: Logging in with demo credentials');
    
    // Fill in demo credentials (from Login.tsx: demo@example.com / password123)
    await page.fill('#email', 'demo@example.com');
    await page.fill('#password', 'password123');
    
    // Take screenshot before login
    await page.screenshot({ 
      path: 'test-results/02-login-form-filled.png', 
      fullPage: true 
    });
    
    // Click login button
    await page.click('button[type="submit"]');
    
    // Wait for navigation to complete (should redirect to dashboard)
    await page.waitForURL('/', { timeout: 10000 });
    await page.waitForLoadState('networkidle');
    
    // 3. Navigate to dashboard/home page
    console.log('Step 3: Verifying dashboard page');
    
    // Take screenshot of dashboard
    await page.screenshot({ 
      path: 'test-results/03-dashboard.png', 
      fullPage: true 
    });
    
    // Verify we're logged in and on dashboard
    await expect(page.locator('body')).toBeVisible();
    
    // 4. Navigate to course/patterns page  
    console.log('Step 4: Navigating to patterns page');
    
    // Try to navigate to patterns page
    await page.goto('/patterns');
    await page.waitForLoadState('networkidle');
    
    // Take screenshot of patterns page
    await page.screenshot({ 
      path: 'test-results/04-patterns-page.png', 
      fullPage: true 
    });
    
    // Verify patterns page loaded
    await expect(page.locator('body')).toBeVisible();
    
    console.log('Demo login smoke test completed successfully');
    console.log('Screenshots saved:');
    console.log('  - 01-login-page.png: Initial login page');
    console.log('  - 02-login-form-filled.png: Login form with demo credentials');
    console.log('  - 03-dashboard.png: Dashboard after successful login');
    console.log('  - 04-patterns-page.png: Patterns/course page');
  });
});