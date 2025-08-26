using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace DanceCourseCreator.Client.Services;

/// <summary>
/// Sample service to demonstrate observability features are working
/// Shows logging, metrics, and tracing integration
/// </summary>
public interface IObservabilityTestService
{
    Task<string> TestObservabilityAsync();
}

public class ObservabilityTestService : IObservabilityTestService
{
    private readonly ILogger<ObservabilityTestService> _logger;
    private readonly HttpClient _httpClient;
    private static readonly ActivitySource ActivitySource = new("DanceCourseCreator.Client");

    public ObservabilityTestService(ILogger<ObservabilityTestService> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<string> TestObservabilityAsync()
    {
        using var activity = ActivitySource.StartActivity("TestObservability");
        
        _logger.LogInformation("Starting observability test - this should be captured by OpenTelemetry logging");
        
        try
        {
            // This HTTP call should be traced by OpenTelemetry HTTP instrumentation
            activity?.SetTag("test.operation", "http_call");
            
            // Make a simple HTTP call to test HTTP client instrumentation
            var response = await _httpClient.GetStringAsync("/api/health");
            
            _logger.LogInformation("HTTP call completed successfully - observability test passed");
            activity?.SetTag("test.result", "success");
            
            return "Observability features are working correctly!";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during observability test");
            activity?.SetTag("test.result", "error");
            throw;
        }
    }
}