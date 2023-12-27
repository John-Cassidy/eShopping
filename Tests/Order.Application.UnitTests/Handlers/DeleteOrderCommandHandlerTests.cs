using Microsoft.Extensions.Logging;
using Moq;
using Ordering.Application.Handlers;
using Ordering.Application.Exceptions;
using Ordering.Application.Commands;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Ordering.Core.Common;
using MediatR;

namespace Order.Application.UnitTests.Handlers;

public class DeleteOrderCommandHandlerTests
{
    private readonly Mock<IOrderRepository> _mockOrderRepository;
    private readonly Mock<ILogger<DeleteOrderCommandHandler>> _mockLogger;

    public DeleteOrderCommandHandlerTests()
    {
        _mockOrderRepository = new Mock<IOrderRepository>();
        _mockLogger = new Mock<ILogger<DeleteOrderCommandHandler>>();
    }

    [Fact]
    public async Task Handle_ValidRequest_DeletesOrderAndReturnsUnit()
    {
        // Arrange
        var orderId = 1;
        var orderToDelete = new TestOrder();

        _mockOrderRepository.Setup(repo => repo.GetByIdAsync(orderToDelete.Id)).ReturnsAsync(orderToDelete);
        var handler = new DeleteOrderCommandHandler(_mockOrderRepository.Object, _mockLogger.Object);
        var command = new DeleteOrderCommand(orderToDelete.Id);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(orderId, orderToDelete.Id);
        _mockOrderRepository.Verify(repo => repo.DeleteAsync(orderToDelete), Times.Once);
        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_NullRequest_ThrowsArgumentNullException()
    {
        // Arrange
        var handler = new DeleteOrderCommandHandler(_mockOrderRepository.Object, _mockLogger.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_OrderNotFound_ThrowsOrderNotFoundException()
    {
        // Arrange
        var orderId = 1;
        var handler = new DeleteOrderCommandHandler(_mockOrderRepository.Object, _mockLogger.Object);
        var command = new DeleteOrderCommand(orderId);
        _mockOrderRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync((Ordering.Core.Entities.Order)null);

        // Act & Assert
        await Assert.ThrowsAsync<OrderNotFoundException>(() => handler.Handle(command, CancellationToken.None));
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
