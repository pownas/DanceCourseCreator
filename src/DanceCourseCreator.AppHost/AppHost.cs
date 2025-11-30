using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Aspire.Hosting.ApplicationModel;

var builder = DistributedApplication.CreateBuilder(args);

// Suppress DataProtection warnings
builder.Services.Configure<LoggerFilterOptions>(options =>
{
    options.Rules.Add(new LoggerFilterRule(null, "Microsoft.AspNetCore.DataProtection", LogLevel.Error, null));
});

// Check if running on Raspberry Pi
var isRaspberryPi = Environment.GetEnvironmentVariable("DANCECOURSE_RASPBERRY_PI") == "true";

// Configure API with proper network binding for Raspberry Pi
var api = builder.AddProject<Projects.DanceCourseCreator_API>("api")
    .WithHttpHealthCheck("/health")
    .WithExternalHttpEndpoints();

if (isRaspberryPi)
{
    // Force API to bind to all interfaces on Raspberry Pi
    api.WithEnvironment("ASPNETCORE_URLS", "http://0.0.0.0:7177");
}

// Configure Web with proper network binding for Raspberry Pi
var web = builder.AddProject<Projects.DanceCourseCreator_Web>("web")
    .WithHttpHealthCheck("/health")
    .WithExternalHttpEndpoints()
    .WaitFor(api)
    .WithReference(api);

if (isRaspberryPi)
{
    // Force Web to bind to all interfaces on Raspberry Pi
    web.WithEnvironment("ASPNETCORE_URLS", "http://0.0.0.0:5001");
}

// Add a hosted service to print message after startup
builder.Services.AddHostedService<StartupMessageService>();

await builder.Build().RunAsync();

// Hosted service to print message after all services are started
class StartupMessageService : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Small delay to ensure dashboard URL is printed first
        _ = Task.Run(async () =>
        {
            await Task.Delay(500, cancellationToken);
            Console.WriteLine("========================================");
            Console.WriteLine("✅ All services started successfully");
            Console.WriteLine("========================================");
        }, cancellationToken);
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
