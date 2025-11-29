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

var api = builder.AddProject<Projects.DanceCourseCreator_API>("api")
    .WithHttpHealthCheck("/health")
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.DanceCourseCreator_Web>("web")
    .WithHttpHealthCheck("/health")
    .WithExternalHttpEndpoints()
    .WaitFor(api)
    .WithReference(api);

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
