using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;
using DanceCourseCreator.Tests.E2E.Infrastructure;
using System.Net.Http.Json;
using System.Text.Json;

namespace DanceCourseCreator.Tests.E2E;

/// <summary>
/// Integration tests using Playwright with WebApplicationFactory and Kestrel.
/// These tests demonstrate how to use the new Web Application Factory in .NET 10
/// with Kestrel server for realistic integration testing.
/// </summary>
[TestClass]
public class PlaywrightIntegrationTests : PageTest
{
    private CustomWebApplicationFactory? _factory;
    private HttpClient? _httpClient;
    private string _serverUrl = string.Empty;

    [TestInitialize]
    public async Task TestInitialize()
    {
        // Create the factory which will configure and build (but not fully start) the host
        _factory = new CustomWebApplicationFactory();
        
        // Get configured URL - in practice this demonstrates the setup
        // For real Kestrel tests, you'd start the API separately
        _serverUrl = _factory.GetServerUrl();
        
        // Create an HttpClient pointing to the API
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(_serverUrl)
        };
        
        Console.WriteLine($"Test server configured for: {_serverUrl}");
        Console.WriteLine("Note: These tests demonstrate WebApplicationFactory setup for Kestrel.");
        Console.WriteLine("For production use, start the API project separately for true Kestrel integration.");
        
        await Task.CompletedTask;
    }

    [TestCleanup]
    public async Task TestCleanup()
    {
        _httpClient?.Dispose();
        
        if (_factory != null)
        {
            await _factory.DisposeAsync();
        }
    }

    /// <summary>
    /// Test the health check endpoint to verify the API is running.
    /// This demonstrates basic connectivity testing with Kestrel.
    /// </summary>
    [TestMethod]
    [TestCategory("Integration")]
    [TestCategory("Kestrel")]
    public async Task HealthCheck_ShouldReturnOK()
    {
        // Act
        var response = await _httpClient!.GetAsync("/api/health");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.IsTrue(response.IsSuccessStatusCode, $"Health check failed with status {response.StatusCode}");
        Assert.Contains(content, "OK", StringComparison.Ordinal, "Health check response should contain 'OK'");
        
        Console.WriteLine($"✓ Health check passed: {content}");
    }

    /// <summary>
    /// Test CRUD operations on the Patterns API endpoint using HTTP client.
    /// Demonstrates Create, Read, Update, Delete operations against the Kestrel server.
    /// </summary>
    [TestMethod]
    [TestCategory("Integration")]
    [TestCategory("Kestrel")]
    [TestCategory("CRUD")]
    public async Task PatternsCRUD_ShouldWorkWithKestrel()
    {
        // === CREATE ===
        var newPattern = new
        {
            Type = "Pattern",
            Name = "Integration Test Left Side Pass",
            Aliases = new[] { "LSP Test" },
            Level = "Beginner",
            DanceStyle = "WestCoastSwing",
            Description = "Test pattern created by integration test",
            Steps = "Test steps",
            Counts = "1,2,3&4,5&6",
            Holds = new[] { "Handshake" },
            Slot = "Left",
            Rotations = "90 degrees",
            Prerequisites = new string[] { },
            Related = new string[] { },
            TeachingPoints = new[] { "Test teaching point" },
            CommonMistakes = new[] { "Test mistake" },
            Variations = new[] { "Test variation" },
            EstimatedMinutes = 10,
            BpmRange = new { Min = 90, Max = 110 },
            Tags = new[] { "test", "integration" },
            MediaLinks = new string[] { }
        };

        var createResponse = await _httpClient!.PostAsJsonAsync("/api/patterns", newPattern);
        Assert.IsTrue(createResponse.IsSuccessStatusCode, 
            $"Create failed with status {createResponse.StatusCode}: {await createResponse.Content.ReadAsStringAsync()}");
        
        var createdPattern = await createResponse.Content.ReadFromJsonAsync<JsonElement>();
        var patternId = createdPattern.GetProperty("id").GetString();
        Assert.IsNotNull(patternId, "Created pattern should have an ID");
        Assert.IsFalse(string.IsNullOrEmpty(patternId), "Pattern ID should not be empty");
        
        Console.WriteLine($"✓ Created pattern with ID: {patternId}");

        // === READ (Get All) ===
        var getAllResponse = await _httpClient!.GetAsync("/api/patterns");
        Assert.IsTrue(getAllResponse.IsSuccessStatusCode, "Get all patterns failed");
        
        var allPatterns = await getAllResponse.Content.ReadFromJsonAsync<JsonElement>();
        var patternsArray = allPatterns.EnumerateArray().ToList();
        Assert.IsGreaterThanOrEqualTo(patternsArray.Count, 2, "Should have at least 2 patterns (seeded + created)");
        
        Console.WriteLine($"✓ Retrieved {patternsArray.Count} patterns");

        // === READ (Get Single) ===
        var getOneResponse = await _httpClient!.GetAsync($"/api/patterns/{patternId}");
        Assert.IsTrue(getOneResponse.IsSuccessStatusCode, "Get single pattern failed");
        
        var retrievedPattern = await getOneResponse.Content.ReadFromJsonAsync<JsonElement>();
        var retrievedName = retrievedPattern.GetProperty("name").GetString();
        Assert.AreEqual("Integration Test Left Side Pass", retrievedName, "Retrieved pattern name should match");
        
        Console.WriteLine($"✓ Retrieved pattern: {retrievedName}");

        // === UPDATE ===
        var updatePattern = new
        {
            Type = "Pattern",
            Name = "UPDATED Integration Test Pattern",
            Aliases = new[] { "LSP Test", "Updated" },
            Level = "Improver",  // Changed level
            DanceStyle = "WestCoastSwing",
            Description = "UPDATED: Test pattern created by integration test",
            Steps = "Updated test steps",
            Counts = "1,2,3&4,5&6",
            Holds = new[] { "Handshake", "Closed" },  // Added hold
            Slot = "Left",
            Rotations = "90 degrees",
            Prerequisites = new string[] { },
            Related = new string[] { },
            TeachingPoints = new[] { "Updated teaching point" },
            CommonMistakes = new[] { "Updated mistake" },
            Variations = new[] { "Updated variation" },
            EstimatedMinutes = 15,  // Changed duration
            BpmRange = new { Min = 85, Max = 115 },  // Changed BPM
            Tags = new[] { "test", "integration", "updated" },
            MediaLinks = new string[] { }
        };

        var updateResponse = await _httpClient!.PutAsJsonAsync($"/api/patterns/{patternId}", updatePattern);
        Assert.IsTrue(updateResponse.IsSuccessStatusCode, 
            $"Update failed with status {updateResponse.StatusCode}: {await updateResponse.Content.ReadAsStringAsync()}");
        
        var updatedPattern = await updateResponse.Content.ReadFromJsonAsync<JsonElement>();
        var updatedName = updatedPattern.GetProperty("name").GetString();
        Assert.AreEqual("UPDATED Integration Test Pattern", updatedName, "Updated pattern name should match");
        
        Console.WriteLine($"✓ Updated pattern: {updatedName}");

        // === DELETE ===
        var deleteResponse = await _httpClient!.DeleteAsync($"/api/patterns/{patternId}");
        Assert.IsTrue(deleteResponse.IsSuccessStatusCode, 
            $"Delete failed with status {deleteResponse.StatusCode}");
        
        Console.WriteLine($"✓ Deleted pattern with ID: {patternId}");

        // Verify deletion
        var getDeletedResponse = await _httpClient.GetAsync($"/api/patterns/{patternId}");
        Assert.AreEqual(System.Net.HttpStatusCode.NotFound, getDeletedResponse.StatusCode, 
            "Deleted pattern should return 404");
        
        Console.WriteLine("✓ Verified pattern was deleted");
    }

    /// <summary>
    /// Test using Playwright to interact with the API through the browser.
    /// This demonstrates combining Playwright UI testing with WebApplicationFactory.
    /// </summary>
    [TestMethod]
    [TestCategory("Integration")]
    [TestCategory("Kestrel")]
    [TestCategory("Playwright")]
    public async Task Playwright_CanAccessKestrelAPI()
    {
        // Navigate to the health endpoint
        await Page.GotoAsync($"{_serverUrl}/api/health");
        
        // Wait for the response to load
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        
        // Get the page content
        var content = await Page.ContentAsync();
        
        // Verify the response contains expected data
        Assert.IsTrue(content.Contains("OK") || content.Contains("status"), 
            "Page should contain health check response");
        
        Console.WriteLine($"✓ Playwright successfully accessed Kestrel API at {_serverUrl}");
        Console.WriteLine($"✓ Response preview: {content.Substring(0, Math.Min(200, content.Length))}...");
    }

    /// <summary>
    /// Test filtering patterns by level through the API.
    /// This demonstrates query parameter handling in integration tests.
    /// </summary>
    [TestMethod]
    [TestCategory("Integration")]
    [TestCategory("Kestrel")]
    [TestCategory("CRUD")]
    public async Task PatternsFilter_ByLevel_ShouldWork()
    {
        // Act - Get patterns filtered by Beginner level
        var response = await _httpClient!.GetAsync("/api/patterns?level=Beginner");
        
        // Assert
        Assert.IsTrue(response.IsSuccessStatusCode, "Filter by level failed");
        
        var patterns = await response.Content.ReadFromJsonAsync<JsonElement>();
        var patternsArray = patterns.EnumerateArray().ToList();
        
        // Verify all returned patterns are Beginner level
        foreach (var pattern in patternsArray)
        {
            var level = pattern.GetProperty("level").GetString();
            Assert.AreEqual("Beginner", level, "All patterns should be Beginner level");
        }
        
        Console.WriteLine($"✓ Retrieved {patternsArray.Count} Beginner level patterns");
    }

    /// <summary>
    /// Test searching patterns by name through the API.
    /// This demonstrates search functionality in integration tests.
    /// </summary>
    [TestMethod]
    [TestCategory("Integration")]
    [TestCategory("Kestrel")]
    [TestCategory("CRUD")]
    public async Task PatternsSearch_ByName_ShouldWork()
    {
        // Act - Search for patterns containing "Sugar"
        var response = await _httpClient!.GetAsync("/api/patterns?search=Sugar");
        
        // Assert
        Assert.IsTrue(response.IsSuccessStatusCode, "Search failed");
        
        var patterns = await response.Content.ReadFromJsonAsync<JsonElement>();
        var patternsArray = patterns.EnumerateArray().ToList();
        
        Assert.IsNotEmpty(patternsArray, "Should find at least one pattern with 'Sugar' in the name");
        
        // Verify all returned patterns contain "Sugar" in name or description
        foreach (var pattern in patternsArray)
        {
            var name = pattern.GetProperty("name").GetString() ?? "";
            var description = pattern.GetProperty("description").GetString() ?? "";
            
            Assert.IsTrue(name.Contains("Sugar", StringComparison.OrdinalIgnoreCase) || 
                         description.Contains("Sugar", StringComparison.OrdinalIgnoreCase),
                         "Pattern should contain 'Sugar' in name or description");
        }
        
        Console.WriteLine($"✓ Found {patternsArray.Count} patterns matching 'Sugar'");
    }

    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions()
        {
            ViewportSize = new ViewportSize() { Width = 1280, Height = 720 },
            IgnoreHTTPSErrors = true,
        };
    }
}
