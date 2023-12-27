using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Ordering.Application.Handlers;
using Ordering.Application.Exceptions;
using Ordering.Application.Commands;
using Ordering.Core.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using MediatR;


namespace Order.Application.UnitTests.Handlers;

public class UpdateOrderCommandHandlerTests
{
    [Fact]
    public async Task Handle_ValidRequest_OrderUpdatedSuccessfully()
    {
        // Arrange
        var orderId = 1;
        TestOrder orderToUpdate = new TestOrder();
        var updateOrderCommand = new UpdateOrderCommand { Id = orderId };
        var orderRepositoryMock = new Mock<IOrderRepository>();
        var mapperMock = new Mock<IMapper>();
        var loggerMock = new Mock<ILogger<UpdateOrderCommandHandler>>();

        orderRepositoryMock.Setup(repo => repo.GetByIdAsync(orderId)).ReturnsAsync(orderToUpdate);
        mapperMock.Setup(mapper => mapper.Map(updateOrderCommand, orderToUpdate, typeof(UpdateOrderCommand), typeof(Ordering.Core.Entities.Order)));
        orderRepositoryMock.Setup(repo => repo.UpdateAsync(orderToUpdate)).Returns(Task.CompletedTask);

        var handler = new UpdateOrderCommandHandler(orderRepositoryMock.Object, mapperMock.Object, loggerMock.Object);

        // Act
        var result = await handler.Handle(updateOrderCommand, CancellationToken.None);

        // Assert
        Assert.Equal(Unit.Value, result);
        orderRepositoryMock.Verify(repo => repo.GetByIdAsync(orderId), Times.Once);
        mapperMock.Verify(mapper => mapper.Map(updateOrderCommand, orderToUpdate, typeof(UpdateOrderCommand), typeof(Ordering.Core.Entities.Order)), Times.Once);
        orderRepositoryMock.Verify(repo => repo.UpdateAsync(orderToUpdate), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidRequest_ThrowsOrderNotFoundException()
    {
        // Arrange
        var orderId = 1;
        var updateOrderCommand = new UpdateOrderCommand { Id = orderId };
        Ordering.Core.Entities.Order orderToUpdate = null;
        var orderRepositoryMock = new Mock<IOrderRepository>();
        var mapperMock = new Mock<IMapper>();
        var loggerMock = new Mock<ILogger<UpdateOrderCommandHandler>>();

        orderRepositoryMock.Setup(repo => repo.GetByIdAsync(orderId)).ReturnsAsync(orderToUpdate);

        var handler = new UpdateOrderCommandHandler(orderRepositoryMock.Object, mapperMock.Object, loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<OrderNotFoundException>(() => handler.Handle(updateOrderCommand, CancellationToken.None));
        orderRepositoryMock.Verify(repo => repo.GetByIdAsync(orderId), Times.Once);
        mapperMock.Verify(mapper => mapper.Map(updateOrderCommand, It.IsAny<Ordering.Core.Entities.Order>(), typeof(UpdateOrderCommand), typeof(Ordering.Core.Entities.Order)), Times.Never);
        orderRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Ordering.Core.Entities.Order>()), Times.Never);
    }

    public class TestOrder : Ordering.Core.Entities.Order
    {
        public TestOrder()
        {
            // Use reflection to set the Id property
            typeof(Ordering.Core.Common.EntityBase)
                .GetProperty(nameof(Ordering.Core.Common.EntityBase.Id))
                .SetValue(this, 1);
        }
    }
}
