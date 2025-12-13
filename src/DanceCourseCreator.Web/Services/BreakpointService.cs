using MudBlazor;
using MudBlazor.Services;

namespace DanceCourseCreator.Web.Services;

/// <summary>
/// Service interface for handling breakpoint detection and responsive behavior.
/// Wraps MudBlazor's IBrowserViewportService with additional convenience methods.
/// </summary>
public interface IBreakpointService
{
    /// <summary>
    /// Subscribes to breakpoint changes and returns a subscription ID for cleanup.
    /// </summary>
    /// <param name="callback">Callback function invoked when breakpoint changes.</param>
    /// <returns>Subscription ID for unsubscribing later.</returns>
    Task<Guid> Subscribe(Func<Breakpoint, Task> callback);
    
    /// <summary>
    /// Unsubscribes from breakpoint changes.
    /// </summary>
    /// <param name="subscriptionId">The subscription ID returned from Subscribe.</param>
    Task Unsubscribe(Guid subscriptionId);
    
    /// <summary>
    /// Checks if the current breakpoint is considered mobile (Xs or Sm).
    /// </summary>
    /// <returns>True if on mobile, false otherwise.</returns>
    Task<bool> IsMobile();
    
    /// <summary>
    /// Checks if the current breakpoint is considered tablet (Md).
    /// </summary>
    /// <returns>True if on tablet, false otherwise.</returns>
    Task<bool> IsTablet();
    
    /// <summary>
    /// Checks if the current breakpoint is considered desktop (Lg or Xl).
    /// </summary>
    /// <returns>True if on desktop, false otherwise.</returns>
    Task<bool> IsDesktop();
    
    /// <summary>
    /// Gets the current breakpoint.
    /// </summary>
    /// <returns>The current breakpoint.</returns>
    Task<Breakpoint> GetCurrentBreakpoint();
}

/// <summary>
/// Implementation of IBreakpointService that wraps MudBlazor's breakpoint functionality.
/// </summary>
public class BreakpointService : IBreakpointService, IAsyncDisposable
{
    private readonly IBrowserViewportService _browserViewportService;
    private readonly ILogger<BreakpointService> _logger;
    private readonly Dictionary<Guid, BreakpointObserver> _observers = new();
    private Breakpoint _currentBreakpoint = Breakpoint.Lg;
    private readonly SemaphoreSlim _lock = new(1, 1);
    private bool _disposed = false;

    public BreakpointService(
        IBrowserViewportService browserViewportService,
        ILogger<BreakpointService> logger)
    {
        _browserViewportService = browserViewportService;
        _logger = logger;
    }

    public async Task<Guid> Subscribe(Func<Breakpoint, Task> callback)
    {
        if (_disposed)
        {
            _logger.LogWarning("Attempted to subscribe to disposed BreakpointService");
            return Guid.Empty;
        }

        await _lock.WaitAsync();
        try
        {
            var subscriptionId = Guid.NewGuid();
            var observer = new BreakpointObserver(subscriptionId, callback, UpdateCurrentBreakpoint, _logger);
            _observers[subscriptionId] = observer;

            // Subscribe to MudBlazor's viewport service
            await _browserViewportService.SubscribeAsync(observer, fireImmediately: true);
            
            _logger.LogDebug("Subscribed to breakpoint service with ID: {SubscriptionId}", subscriptionId);

            return subscriptionId;
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task Unsubscribe(Guid subscriptionId)
    {
        if (_disposed)
        {
            _logger.LogDebug("Skipping unsubscribe for {SubscriptionId} - service already disposed", subscriptionId);
            return;
        }

        await _lock.WaitAsync();
        try
        {
            if (_observers.TryGetValue(subscriptionId, out var observer))
            {
                await _browserViewportService.UnsubscribeAsync(observer);
                _observers.Remove(subscriptionId);
                _logger.LogDebug("Unsubscribed from breakpoint service with ID: {SubscriptionId}", subscriptionId);
            }
        }
        finally
        {
            _lock.Release();
        }
    }

    private void UpdateCurrentBreakpoint(Breakpoint breakpoint)
    {
        _currentBreakpoint = breakpoint;
    }

    public async Task<bool> IsMobile()
    {
        var breakpoint = await GetCurrentBreakpoint();
        return breakpoint <= Breakpoint.Sm;
    }

    public async Task<bool> IsTablet()
    {
        var breakpoint = await GetCurrentBreakpoint();
        return breakpoint == Breakpoint.Md;
    }

    public async Task<bool> IsDesktop()
    {
        var breakpoint = await GetCurrentBreakpoint();
        return breakpoint >= Breakpoint.Lg;
    }

    public async Task<Breakpoint> GetCurrentBreakpoint()
    {
        try
        {
            return await _browserViewportService.GetCurrentBreakpointAsync();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to get current breakpoint, using cached value: {Breakpoint}", _currentBreakpoint);
            return _currentBreakpoint;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        foreach (var observer in _observers.Values)
        {
            try
            {
                await _browserViewportService.UnsubscribeAsync(observer);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error unsubscribing observer during disposal");
            }
        }
        _observers.Clear();
        _lock.Dispose();
    }

    /// <summary>
    /// Observer class that implements IBrowserViewportObserver for MudBlazor integration.
    /// </summary>
    private class BreakpointObserver : IBrowserViewportObserver
    {
        private readonly Func<Breakpoint, Task> _callback;
        private readonly Action<Breakpoint> _updateCurrent;
        private readonly ILogger<BreakpointService> _logger;

        public Guid Id { get; }

        public BreakpointObserver(
            Guid id, 
            Func<Breakpoint, Task> callback, 
            Action<Breakpoint> updateCurrent,
            ILogger<BreakpointService> logger)
        {
            Id = id;
            _callback = callback;
            _updateCurrent = updateCurrent;
            _logger = logger;
        }

        public async Task NotifyBrowserViewportChangeAsync(BrowserViewportEventArgs browserViewportEventArgs)
        {
            try
            {
                _updateCurrent(browserViewportEventArgs.Breakpoint);
                await _callback(browserViewportEventArgs.Breakpoint);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error invoking breakpoint callback for observer {ObserverId}", Id);
            }
        }

        ResizeOptions IBrowserViewportObserver.ResizeOptions { get; } = new()
        {
            ReportRate = 100,
            NotifyOnBreakpointOnly = true
        };
    }
}
