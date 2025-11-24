using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using DanceCourseCreator.API.Data;

namespace DanceCourseCreator.Tests.E2E.Infrastructure;

/// <summary>
/// Custom WebApplicationFactory for integration testing with Kestrel.
/// This factory configures the application to run with Kestrel server instead of TestServer,
/// which provides a more realistic testing environment that matches production hosting.
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _dbPath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.db");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Configure to use Kestrel with a random port
        builder.UseKestrel();
        builder.UseUrls("http://127.0.0.1:0"); // Port 0 means use any available port

        // Override the connection string to use a test database
        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = $"Data Source={_dbPath}"
            });
        });
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        // Build and start the host with Kestrel
        builder.ConfigureWebHost(webHostBuilder => webHostBuilder.UseKestrel());

        var host = builder.Build();
        host.Start();
        
        // Wait a moment for the database to be created and seeded by Program.cs
        System.Threading.Thread.Sleep(1000);
        
        // Add additional test data if needed
        using (var scope = host.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<DanceCourseDbContext>();
            SeedAdditionalTestData(db);
        }

        return host;
    }

    /// <summary>
    /// Seeds additional test data beyond what Program.cs already creates.
    /// </summary>
    private void SeedAdditionalTestData(DanceCourseDbContext context)
    {
        // Add a sample pattern for testing (if not already present)
        if (!context.Patterns.Any(p => p.Id == "test-pattern-1"))
        {
            var testPattern = new DanceCourseCreator.API.Models.PatternOrExercise
            {
                Id = "test-pattern-1",
                Type = DanceCourseCreator.API.Models.PatternType.Pattern,
                Name = "Test Sugar Push",
                Aliases = new List<string> { "Sugar Tuck" },
                Level = DanceCourseCreator.API.Models.DanceLevel.Beginner,
                DanceStyle = DanceCourseCreator.API.Models.DanceStyle.WestCoastSwing,
                Description = "A basic pattern for testing",
                Steps = new List<string> { "Leader steps back on left", "Follower steps forward on right" },
                Counts = new List<string> { "1&2", "3&4", "5&6" },
                Holds = new List<string> { "Handshake", "Closed" },
                Slot = "Center",
                Rotations = new List<string> { "None" },
                Prerequisites = new List<string>(),
                Related = new List<string>(),
                TeachingPoints = new List<string> { "Maintain connection", "Stay on time" },
                CommonMistakes = new List<string> { "Rushing the timing" },
                Variations = new List<string> { "With turn", "With styling" },
                EstimatedMinutes = 15,
                BpmRangeMin = 90,
                BpmRangeMax = 110,
                Tags = new List<string> { "foundational", "beginner-friendly" },
                MediaLinks = new List<string>(),
                CreatedBy = "demo-user-id",  // Use demo user created by Program.cs
                UpdatedAt = DateTime.UtcNow
            };
            context.Patterns.Add(testPattern);
            context.SaveChanges();
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            // Clean up the test database file
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
