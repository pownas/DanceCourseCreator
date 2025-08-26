using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace DanceCourseCreator.Client.Extensions;

/// <summary>
/// WASM-specific implementation of Aspire ServiceDefaults for observability
/// Provides logging, metrics, and tracing for Blazor WebAssembly applications
/// </summary>
public static class WebAssemblyServiceDefaults
{
    /// <summary>
    /// Adds Aspire ServiceDefaults to the WebAssembly host builder
    /// Configures OpenTelemetry observability
    /// </summary>
    public static WebAssemblyHostBuilder AddServiceDefaults(this WebAssemblyHostBuilder builder)
    {
        builder.ConfigureOpenTelemetry();
        
        return builder;
    }

    /// <summary>
    /// Configures OpenTelemetry for Blazor WebAssembly applications
    /// Sets up logging, metrics, and tracing suitable for browser environment
    /// </summary>
    public static WebAssemblyHostBuilder ConfigureOpenTelemetry(this WebAssemblyHostBuilder builder)
    {
        // Configure logging with OpenTelemetry
        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });

        // Configure OpenTelemetry for WASM
        builder.Services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                // Add HTTP client instrumentation (suitable for WASM)
                metrics.AddHttpClientInstrumentation();
            })
            .WithTracing(tracing =>
            {
                // Add tracing for the application and HTTP client
                tracing.AddSource("DanceCourseCreator.Client")
                    .AddHttpClientInstrumentation();
            });

        // Add exporters if configured
        builder.AddOpenTelemetryExporters();

        return builder;
    }

    /// <summary>
    /// Adds OpenTelemetry exporters based on configuration
    /// Supports OTLP exporter for sending telemetry data
    /// </summary>
    private static WebAssemblyHostBuilder AddOpenTelemetryExporters(this WebAssemblyHostBuilder builder)
    {
        var useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

        if (useOtlpExporter)
        {
            builder.Services.AddOpenTelemetry().UseOtlpExporter();
        }

        // Note: Azure Monitor exporter would need to be configured separately for WASM if required
        // as it may have different requirements in browser environment

        return builder;
    }
}