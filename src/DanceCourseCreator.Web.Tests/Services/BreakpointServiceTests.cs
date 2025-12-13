using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MudBlazor;
using MudBlazor.Services;
using DanceCourseCreator.Web.Services;

namespace DanceCourseCreator.Web.Tests.Services;

/// <summary>
/// Unit tests for the BreakpointService that handles responsive breakpoint detection.
/// </summary>
[TestClass]
public class BreakpointServiceTests
{
    private Mock<IBrowserViewportService> _mockViewportService = null!;
    private Mock<ILogger<BreakpointService>> _mockLogger = null!;
    private BreakpointService _service = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _mockViewportService = new Mock<IBrowserViewportService>();
        _mockLogger = new Mock<ILogger<BreakpointService>>();
        _service = new BreakpointService(_mockViewportService.Object, _mockLogger.Object);
    }

    [TestMethod]
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
        Assert.AreNotEqual(Guid.Empty, subscriptionId);
    }

    [TestMethod]
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

    [TestMethod]
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

    [TestMethod]
    public async Task Unsubscribe_WithInvalidId_ShouldNotThrow()
    {
        // Arrange
        var invalidId = Guid.NewGuid();

        // Act & Assert
        try
        {
            await _service.Unsubscribe(invalidId);
            // Test passes if no exception is thrown
        }
        catch (Exception ex)
        {
            Assert.Fail($"Expected no exception, but got: {ex.Message}");
        }
    }

    [TestMethod]
    [DataRow(Breakpoint.Xs, true)]
    [DataRow(Breakpoint.Sm, true)]
    [DataRow(Breakpoint.Md, false)]
    [DataRow(Breakpoint.Lg, false)]
    [DataRow(Breakpoint.Xl, false)]
    [DataRow(Breakpoint.Xxl, false)]
    public async Task IsMobile_ShouldReturnCorrectValue(Breakpoint breakpoint, bool expectedIsMobile)
    {
        // Arrange
        _mockViewportService
            .Setup(x => x.GetCurrentBreakpointAsync())
            .ReturnsAsync(breakpoint);

        // Act
        var result = await _service.IsMobile();

        // Assert
        Assert.AreEqual(expectedIsMobile, result);
    }

    [TestMethod]
    [DataRow(Breakpoint.Xs, false)]
    [DataRow(Breakpoint.Sm, false)]
    [DataRow(Breakpoint.Md, true)]
    [DataRow(Breakpoint.Lg, false)]
    [DataRow(Breakpoint.Xl, false)]
    public async Task IsTablet_ShouldReturnCorrectValue(Breakpoint breakpoint, bool expectedIsTablet)
    {
        // Arrange
        _mockViewportService
            .Setup(x => x.GetCurrentBreakpointAsync())
            .ReturnsAsync(breakpoint);

        // Act
        var result = await _service.IsTablet();

        // Assert
        Assert.AreEqual(expectedIsTablet, result);
    }

    [TestMethod]
    [DataRow(Breakpoint.Xs, false)]
    [DataRow(Breakpoint.Sm, false)]
    [DataRow(Breakpoint.Md, false)]
    [DataRow(Breakpoint.Lg, true)]
    [DataRow(Breakpoint.Xl, true)]
    [DataRow(Breakpoint.Xxl, true)]
    public async Task IsDesktop_ShouldReturnCorrectValue(Breakpoint breakpoint, bool expectedIsDesktop)
    {
        // Arrange
        _mockViewportService
            .Setup(x => x.GetCurrentBreakpointAsync())
            .ReturnsAsync(breakpoint);

        // Act
        var result = await _service.IsDesktop();

        // Assert
        Assert.AreEqual(expectedIsDesktop, result);
    }

    [TestMethod]
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
        Assert.AreEqual(expectedBreakpoint, result);
    }

    [TestMethod]
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
        Assert.AreEqual(Breakpoint.Lg, result);
    }

    [TestMethod]
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

    [TestMethod]
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
        Assert.AreNotEqual(id1, id2);
        _mockViewportService.Verify(
            x => x.SubscribeAsync(It.IsAny<IBrowserViewportObserver>(), true),
            Times.Exactly(2));
    }
}
