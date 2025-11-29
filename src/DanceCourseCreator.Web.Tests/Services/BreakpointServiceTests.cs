using Microsoft.Extensions.Logging;
using Moq;
using MudBlazor;
using MudBlazor.Services;
using DanceCourseCreator.Web.Services;

namespace DanceCourseCreator.Web.Tests.Services;

/// <summary>
/// Unit tests for the BreakpointService that handles responsive breakpoint detection.
/// </summary>
public class BreakpointServiceTests
{
    private readonly Mock<IBrowserViewportService> _mockViewportService;
    private readonly Mock<ILogger<BreakpointService>> _mockLogger;
    private readonly BreakpointService _service;

    public BreakpointServiceTests()
    {
        _mockViewportService = new Mock<IBrowserViewportService>();
        _mockLogger = new Mock<ILogger<BreakpointService>>();
        _service = new BreakpointService(_mockViewportService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Subscribe_ShouldReturnValidGuid()
    {
        // Arrange
        Func<Breakpoint, Task> callback = _ => Task.CompletedTask;
        _mockViewportService
            .Setup(x => x.SubscribeAsync(It.IsAny<IBrowserViewportObserver>(), It.IsAny<bool>()))
            .Returns(Task.CompletedTask);

        // Act
        var subscriptionId = await _service.Subscribe(callback);

        // Assert
        Assert.NotEqual(Guid.Empty, subscriptionId);
    }

    [Fact]
    public async Task Subscribe_ShouldCallBrowserViewportService()
    {
        // Arrange
        Func<Breakpoint, Task> callback = _ => Task.CompletedTask;
        _mockViewportService
            .Setup(x => x.SubscribeAsync(It.IsAny<IBrowserViewportObserver>(), It.IsAny<bool>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.Subscribe(callback);

        // Assert
        _mockViewportService.Verify(
            x => x.SubscribeAsync(It.IsAny<IBrowserViewportObserver>(), true),
            Times.Once);
    }

    [Fact]
    public async Task Unsubscribe_ShouldCallBrowserViewportServiceUnsubscribe()
    {
        // Arrange
        Func<Breakpoint, Task> callback = _ => Task.CompletedTask;
        _mockViewportService
            .Setup(x => x.SubscribeAsync(It.IsAny<IBrowserViewportObserver>(), It.IsAny<bool>()))
            .Returns(Task.CompletedTask);
        _mockViewportService
            .Setup(x => x.UnsubscribeAsync(It.IsAny<IBrowserViewportObserver>()))
            .Returns(Task.CompletedTask);

        var subscriptionId = await _service.Subscribe(callback);

        // Act
        await _service.Unsubscribe(subscriptionId);

        // Assert
        _mockViewportService.Verify(
            x => x.UnsubscribeAsync(It.IsAny<IBrowserViewportObserver>()),
            Times.Once);
    }

    [Fact]
    public async Task Unsubscribe_WithInvalidId_ShouldNotThrow()
    {
        // Arrange
        var invalidId = Guid.NewGuid();

        // Act & Assert
        var exception = await Record.ExceptionAsync(() => _service.Unsubscribe(invalidId));
        Assert.Null(exception);
    }

    [Theory]
    [InlineData(Breakpoint.Xs, true)]
    [InlineData(Breakpoint.Sm, true)]
    [InlineData(Breakpoint.Md, false)]
    [InlineData(Breakpoint.Lg, false)]
    [InlineData(Breakpoint.Xl, false)]
    [InlineData(Breakpoint.Xxl, false)]
    public async Task IsMobile_ShouldReturnCorrectValue(Breakpoint breakpoint, bool expectedIsMobile)
    {
        // Arrange
        _mockViewportService
            .Setup(x => x.GetCurrentBreakpointAsync())
            .ReturnsAsync(breakpoint);

        // Act
        var result = await _service.IsMobile();

        // Assert
        Assert.Equal(expectedIsMobile, result);
    }

    [Theory]
    [InlineData(Breakpoint.Xs, false)]
    [InlineData(Breakpoint.Sm, false)]
    [InlineData(Breakpoint.Md, true)]
    [InlineData(Breakpoint.Lg, false)]
    [InlineData(Breakpoint.Xl, false)]
    public async Task IsTablet_ShouldReturnCorrectValue(Breakpoint breakpoint, bool expectedIsTablet)
    {
        // Arrange
        _mockViewportService
            .Setup(x => x.GetCurrentBreakpointAsync())
            .ReturnsAsync(breakpoint);

        // Act
        var result = await _service.IsTablet();

        // Assert
        Assert.Equal(expectedIsTablet, result);
    }

    [Theory]
    [InlineData(Breakpoint.Xs, false)]
    [InlineData(Breakpoint.Sm, false)]
    [InlineData(Breakpoint.Md, false)]
    [InlineData(Breakpoint.Lg, true)]
    [InlineData(Breakpoint.Xl, true)]
    [InlineData(Breakpoint.Xxl, true)]
    public async Task IsDesktop_ShouldReturnCorrectValue(Breakpoint breakpoint, bool expectedIsDesktop)
    {
        // Arrange
        _mockViewportService
            .Setup(x => x.GetCurrentBreakpointAsync())
            .ReturnsAsync(breakpoint);

        // Act
        var result = await _service.IsDesktop();

        // Assert
        Assert.Equal(expectedIsDesktop, result);
    }

    [Fact]
    public async Task GetCurrentBreakpoint_ShouldReturnBreakpointFromService()
    {
        // Arrange
        var expectedBreakpoint = Breakpoint.Md;
        _mockViewportService
            .Setup(x => x.GetCurrentBreakpointAsync())
            .ReturnsAsync(expectedBreakpoint);

        // Act
        var result = await _service.GetCurrentBreakpoint();

        // Assert
        Assert.Equal(expectedBreakpoint, result);
    }

    [Fact]
    public async Task GetCurrentBreakpoint_WhenServiceThrows_ShouldReturnDefaultBreakpoint()
    {
        // Arrange
        _mockViewportService
            .Setup(x => x.GetCurrentBreakpointAsync())
            .ThrowsAsync(new InvalidOperationException("Test exception"));

        // Act
        var result = await _service.GetCurrentBreakpoint();

        // Assert
        // Should return default cached value (Lg)
        Assert.Equal(Breakpoint.Lg, result);
    }

    [Fact]
    public async Task DisposeAsync_ShouldUnsubscribeAllObservers()
    {
        // Arrange
        Func<Breakpoint, Task> callback = _ => Task.CompletedTask;
        _mockViewportService
            .Setup(x => x.SubscribeAsync(It.IsAny<IBrowserViewportObserver>(), It.IsAny<bool>()))
            .Returns(Task.CompletedTask);
        _mockViewportService
            .Setup(x => x.UnsubscribeAsync(It.IsAny<IBrowserViewportObserver>()))
            .Returns(Task.CompletedTask);

        await _service.Subscribe(callback);
        await _service.Subscribe(callback);

        // Act
        await _service.DisposeAsync();

        // Assert
        _mockViewportService.Verify(
            x => x.UnsubscribeAsync(It.IsAny<IBrowserViewportObserver>()),
            Times.Exactly(2));
    }

    [Fact]
    public async Task Subscribe_MultipleSubscribers_ShouldAllReceiveCallbackId()
    {
        // Arrange
        _mockViewportService
            .Setup(x => x.SubscribeAsync(It.IsAny<IBrowserViewportObserver>(), It.IsAny<bool>()))
            .Returns(Task.CompletedTask);

        var callbackInvocations = new List<Guid>();
        
        Func<Breakpoint, Task> callback1 = _ => { callbackInvocations.Add(Guid.NewGuid()); return Task.CompletedTask; };
        Func<Breakpoint, Task> callback2 = _ => { callbackInvocations.Add(Guid.NewGuid()); return Task.CompletedTask; };

        // Act
        var id1 = await _service.Subscribe(callback1);
        var id2 = await _service.Subscribe(callback2);

        // Assert
        Assert.NotEqual(id1, id2);
        _mockViewportService.Verify(
            x => x.SubscribeAsync(It.IsAny<IBrowserViewportObserver>(), true),
            Times.Exactly(2));
    }
}
