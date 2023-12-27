using AutoMapper;
using Moq;
using Ordering.Application.Exceptions;
using Ordering.Application.Handlers;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using Ordering.Core.Common;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Order.Application.UnitTests;

public class GetOrderListQueryHandlerTetsts
{
    private readonly Mock<IOrderRepository> _mockOrderRepository;
    private readonly Mock<IMapper> _mockMapper;

    public GetOrderListQueryHandlerTetsts()
    {
        _mockOrderRepository = new Mock<IOrderRepository>();
        _mockMapper = new Mock<IMapper>();
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsOrderList()
    {
        // Arrange
        var userName = "TestUser";
        int orderId = 1;
        var orderList = new List<Ordering.Core.Entities.Order>
        {
            new TestOrder(orderId++){ UserName = userName},
            new TestOrder(orderId++){ UserName = userName},
            new TestOrder(orderId){ UserName = userName}
        };
        var orderResponse = orderList.Select(order => new OrderResponse
        {
            Id = order.Id,
            UserName = order.UserName,
            TotalPrice = order.TotalPrice ?? 0.0m,
            FirstName = order.FirstName,
            LastName = order.LastName,
            EmailAddress = order.EmailAddress,
            AddressLine = order.AddressLine,
            Country = order.Country,
            State = order.State,
            ZipCode = order.ZipCode,
            CardName = order.CardName,
            CardNumber = order.CardNumber,
            Expiration = order.Expiration,
            CVV = order.Cvv,
            PaymentMethod = order.PaymentMethod ?? 0
        }).ToList();

        _mockOrderRepository.Setup(repo => repo.GetOrdersByUserName(It.IsAny<string>())).ReturnsAsync(orderList);
        _mockMapper.Setup(mapper => mapper.Map<List<OrderResponse>>(It.IsAny<List<Ordering.Core.Entities.Order>>())).Returns(orderResponse);

        var handler = new GetOrderListQueryHandler(_mockOrderRepository.Object, _mockMapper.Object);
        var query = new GetOrderListQuery(userName);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(orderResponse.Count, result.Count);
        Assert.Equivalent(orderResponse, result);
        _mockOrderRepository.Verify(repo => repo.GetOrdersByUserName(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NullRequest_ThrowsArgumentNullException()
    {
        // Arrange
        var handler = new GetOrderListQueryHandler(_mockOrderRepository.Object, _mockMapper.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_OrderNotFound_ThrowsOrderNotFoundException()
    {
        // Arrange
        var userName = "TestUser";
        var handler = new GetOrderListQueryHandler(_mockOrderRepository.Object, _mockMapper.Object);
        var query = new GetOrderListQuery(userName);
        _mockOrderRepository.Setup(repo => repo.GetOrdersByUserName(It.IsAny<string>())).ReturnsAsync((List<Ordering.Core.Entities.Order>)null);
        _mockMapper.Setup(mapper => mapper.Map<List<OrderResponse>>(It.IsAny<List<Ordering.Core.Entities.Order>>())).Returns<List<OrderResponse>>(null);

        // Act & Assert
        await Assert.ThrowsAsync<OrderNotFoundException>(() => handler.Handle(query, CancellationToken.None));
        _mockMapper.Verify(mapper => mapper.Map<List<OrderResponse>>(It.IsAny<List<Ordering.Core.Entities.Order>>()), Times.Never);
    }

    public class TestOrder : Ordering.Core.Entities.Order
    {
        public TestOrder(int id = 1)
        {
            // Use reflection to set the Id property
            typeof(EntityBase)
                .GetProperty(nameof(EntityBase.Id))
                .SetValue(this, id);
        }
    }
}
