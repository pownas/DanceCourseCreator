using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using DanceCourseCreator.API.Data;

namespace DanceCourseCreator.Tests.E2E.Infrastructure;

/// <summary>
/// WebApplicationFactory for E2E tests with Playwright.
/// Starts the API application on configurable port for Playwright browser testing.
/// </summary>
public class PlaywrightWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _dbPath = Path.Combine(Path.GetTempPath(), $"e2e_test_{Guid.NewGuid()}.db");
    private const int ApiPort = 5139;
    private const int WebPort = 5034;

    public string ApiUrl => $"http://localhost:{ApiPort}";
    public string WebUrl => $"http://localhost:{WebPort}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseKestrel(options =>
        {
            options.ListenLocalhost(ApiPort);
        });

        // Use test database
        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = $"Data Source={_dbPath}",
                ["Logging:LogLevel:Default"] = "Warning",
                ["Logging:LogLevel:Microsoft.AspNetCore"] = "Warning"
            });
        });

        builder.ConfigureServices(services =>
        {
            // Replace DbContext with test database
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<DanceCourseDbContext>));
            
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<DanceCourseDbContext>(options =>
            {
                options.UseSqlite($"Data Source={_dbPath}");
            });
        });
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        // Build and start the API host
        var host = builder.Build();
        host.Start();

        // Give the server time to start
        Thread.Sleep(1000);

        // Ensure database is created and seeded
        using (var scope = host.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<DanceCourseDbContext>();
            db.Database.EnsureCreated();
            SeedTestData(db);
        }

        return host;
    }

    private void SeedTestData(DanceCourseDbContext context)
    {
        // Seed data is already handled by Program.cs startup
        // Add any additional test-specific data here if needed
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            // Clean up test database
            if (File.Exists(_dbPath))
            {
                try
                {
                    File.Delete(_dbPath);
                }
                catch
                {
                    // Ignore cleanup errors
                }
            }
        }
        base.Dispose(disposing);
    }
}
