using Basket.Application.Commands;
using Basket.Application.Handlers;
using Basket.Core.Entities;
using Basket.Core.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitTests.Basket;
public class DeleteBasketByUserNameHandlerTests {
    private readonly Mock<IBasketRepository> _basketRepositoryMock;
    private readonly DeleteBasketByUserNameHandler _handler;

    public DeleteBasketByUserNameHandlerTests() {
        _basketRepositoryMock = new Mock<IBasketRepository>();
        _handler = new DeleteBasketByUserNameHandler(_basketRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsTrue() {
        // Arrange
        var command = new DeleteBasketByUserNameCommand(userName: "testUser");
        _basketRepositoryMock.Setup(x => x.DeleteBasket(It.IsAny<string>())).ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
    }
}
