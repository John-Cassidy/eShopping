using Xunit;
using Moq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Ordering.Application.Handlers;
using Ordering.Core.Repositories;
using Ordering.Core.Entities;
using System.Threading.Tasks;
using System.Threading;
using Ordering.Application.Commands;
using Ordering.Core.Common;

namespace Order.Application.UnitTests.Handlers;

public class CheckoutOrderCommandHandlerTests
{
    private readonly Mock<IOrderRepository> _mockOrderRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<CheckoutOrderCommandHandler>> _mockLogger;

    public CheckoutOrderCommandHandlerTests()
    {
        _mockOrderRepository = new Mock<IOrderRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<CheckoutOrderCommandHandler>>();
    }

    [Fact]
    public async Task Handle_ValidRequest_AddsOrderSuccessfully()
    {
        // Arrange
        var handler = new CheckoutOrderCommandHandler(_mockOrderRepository.Object, _mockMapper.Object, _mockLogger.Object);
        var request = new CheckoutOrderCommand();
        var orderEntity = new Ordering.Core.Entities.Order();
        TestOrder newOrderEntity = new TestOrder();
        _mockMapper.Setup(m => m.Map<Ordering.Core.Entities.Order>(request)).Returns(orderEntity);
        _mockOrderRepository.Setup(r => r.AddAsync(orderEntity)).ReturnsAsync(newOrderEntity as Ordering.Core.Entities.Order);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal((newOrderEntity as Ordering.Core.Entities.Order).Id, result);
        _mockOrderRepository.Verify(r => r.AddAsync(orderEntity), Times.Once);
        _mockLogger.Verify(l =>
            l.Log(LogLevel.Information,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_NullRequest_ThrowsArgumentNullException()
    {
        // Arrange
        var handler = new CheckoutOrderCommandHandler(_mockOrderRepository.Object, _mockMapper.Object, _mockLogger.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null, CancellationToken.None));
    }

    public class TestOrder : Ordering.Core.Entities.Order
    {
        public TestOrder()
        {
            // Use reflection to set the Id property
            typeof(EntityBase)
                .GetProperty(nameof(EntityBase.Id))
                .SetValue(this, 1);
        }
    }
}
