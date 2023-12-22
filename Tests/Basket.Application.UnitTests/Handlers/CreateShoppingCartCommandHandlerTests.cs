using Xunit;
using Moq;
using Basket.Core.Repositories;
using Basket.Application.Handlers;
using Basket.Application.Commands;
using Basket.Core.Entities;
using Basket.Application.Responses;

namespace Basket.Application.UnitTests.Handlers;

public class CreateShoppingCartCommandHandlerTests {
    private readonly Mock<IBasketRepository> _basketRepositoryMock;
    private readonly CreateShoppingCartCommandHandler _handler;

    public CreateShoppingCartCommandHandlerTests() {
        _basketRepositoryMock = new Mock<IBasketRepository>();
        _handler = new CreateShoppingCartCommandHandler(_basketRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsShoppingCartResponse() {
        // Arrange
        CreateShoppingCartCommand command = new CreateShoppingCartCommand(
            userName: "testuser",
            items: new List<ShoppingCartItem> {
                new ShoppingCartItem(quantity: 1, price: 99.99m, productId: new Guid().ToString(), imageFile: "images/products/adidas_shoe-2.png", productName: "Test Product")
            });

        ShoppingCart newShoppingCart = new ShoppingCart {
            UserName = command.UserName,
            Items = command.Items
        };

        var expectedShoppingCartResponse = new ShoppingCartResponse {
            UserName = newShoppingCart.UserName,
            Items = command.Items.Select(item => new ShoppingCartItemResponse(quantity: item.Quantity, price: item.Price, productId: item.ProductId, imageFile: item.ImageFile, productName: item.ProductName)).ToList()
        };

        _basketRepositoryMock.Setup(x => x.UpdateBasket(It.IsAny<ShoppingCart>())).ReturnsAsync(newShoppingCart);

        // Act
        var actualShoppingCartResponse = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(actualShoppingCartResponse);
        Assert.IsType<ShoppingCartResponse>(actualShoppingCartResponse);
        Assert.Equal(expectedShoppingCartResponse.UserName, actualShoppingCartResponse.UserName);
        Assert.Equivalent(expectedShoppingCartResponse.Items, actualShoppingCartResponse.Items);
        Assert.Equivalent(expectedShoppingCartResponse, actualShoppingCartResponse);
    }
}
