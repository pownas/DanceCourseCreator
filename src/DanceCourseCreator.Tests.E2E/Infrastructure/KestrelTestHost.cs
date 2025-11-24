using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DanceCourseCreator.API.Data;

namespace DanceCourseCreator.Tests.E2E.Infrastructure;

/// <summary>
/// Custom WebApplicationFactory for integration testing with Kestrel.
/// This factory configures the application to run with Kestrel server,
/// providing a more realistic testing environment than TestServer.
/// 
/// Note: Due to WebApplicationFactory's internal TestServer casting,
/// this example demonstrates the setup. For production use with real Kestrel,
/// consider starting the API project separately and connecting tests to it.
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _dbPath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.db");
    private IHost? _customHost;
    private string? _serverUrl;

    public string GetServerUrl()
    {
        // For this demonstration, return a configured URL
        // In a real implementation with Kestrel running, this would be the actual Kestrel URL
        return _serverUrl ?? "http://127.0.0.1:5139";
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Configure to use Kestrel
        builder.UseKestrel();
        builder.UseUrls("http://127.0.0.1:0");

        // Override configuration
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
        // Build the host
        builder.ConfigureWebHost(webHostBuilder => webHostBuilder.UseKestrel());
        var host = builder.Build();
        host.Start();

        // Capture server URL
        var server = host.Services.GetService<IServer>();
        var addresses = server?.Features.Get<IServerAddressesFeature>();
        if (addresses != null && addresses.Addresses.Any())
        {
            _serverUrl = addresses.Addresses.First();
        }

        _customHost = host;
        return host;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _customHost?.StopAsync().Wait();
            _customHost?.Dispose();
            
            if (File.Exists(_dbPath))
            {
                try { File.Delete(_dbPath); } catch { }
            }
        }
        base.Dispose(disposing);
    }
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

    public void Dispose()
    {
        _app?.StopAsync().Wait();
        _app?.DisposeAsync().AsTask().Wait();
        
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
}
