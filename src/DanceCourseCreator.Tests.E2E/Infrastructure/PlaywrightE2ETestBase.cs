using Microsoft.Playwright.MSTest;
using Microsoft.Playwright;
using DanceCourseCreator.Tests.E2E.Infrastructure;

namespace DanceCourseCreator.Tests.E2E;

/// <summary>
/// Base class for Playwright E2E tests with WebApplicationFactory.
/// Automatically manages application lifecycle for tests.
/// </summary>
public abstract class PlaywrightE2ETestBase : PageTest
{
    protected static PlaywrightWebApplicationFactory? Factory;
    protected static string BaseUrl => Factory?.WebUrl ?? "http://localhost:5034";
    protected static string ApiUrl => Factory?.ApiUrl ?? "http://localhost:5139";
    
    /// <summary>
    /// Set to true to use WebApplicationFactory (starts apps automatically).
    /// Set to false to use manually started applications.
    /// </summary>
    protected virtual bool UseWebApplicationFactory => false;

    [ClassInitialize]
    public static async Task ClassInitialize(TestContext context)
    {
        // Check if we should use WebApplicationFactory
        var useFactory = context.Properties.ContainsKey("UseWebApplicationFactory") 
            && bool.TryParse(context.Properties["UseWebApplicationFactory"]?.ToString(), out var result) 
            && result;

        if (useFactory && Factory == null)
        {
            Factory = new PlaywrightWebApplicationFactory();
            
            // Give the server time to fully start
            await Task.Delay(2000);
            
            Console.WriteLine($"WebApplicationFactory started:");
            Console.WriteLine($"  API URL: {Factory.ApiUrl}");
            Console.WriteLine($"  Web URL: {Factory.WebUrl}");
        }
        else if (!useFactory)
        {
            Console.WriteLine("Using manually started applications.");
            Console.WriteLine("Ensure API is running on http://localhost:5139");
            Console.WriteLine("Ensure Web is running on http://localhost:5034");
        }
    }

    [ClassCleanup]
    public static async Task ClassCleanup()
    {
        if (Factory != null)
        {
            await Factory.DisposeAsync();
            Factory = null;
            Console.WriteLine("WebApplicationFactory disposed.");
        }
    }

    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions()
        {
            ViewportSize = new ViewportSize() { Width = 1920, Height = 1080 },
            IgnoreHTTPSErrors = true,
            BaseURL = BaseUrl
        };
    }
}
