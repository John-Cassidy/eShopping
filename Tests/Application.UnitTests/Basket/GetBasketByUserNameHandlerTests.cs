using Basket.Application.Handlers;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Entities;
using Basket.Core.Repositories;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitTests.Basket;
public class GetBasketByUserNameHandlerTests {
    private readonly Mock<IBasketRepository> _mockBasketRepository;
    private readonly GetBasketByUserNameHandler _handler;

    public GetBasketByUserNameHandlerTests() {
        _mockBasketRepository = new Mock<IBasketRepository>();
        _handler = new GetBasketByUserNameHandler(_mockBasketRepository.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsShoppingCartResponse() {
        // Arrange
        var query = new GetBasketByUserNameQuery(userName: "testUser");
        ShoppingCart shoppingCart = new ShoppingCart{ 
            UserName = "testUser", 
            Items = new List<ShoppingCartItem> { 
                new ShoppingCartItem(quantity: 1, price: 99.99m, productId: new Guid().ToString(), imageFile: "images/products/adidas_shoe-2.png", productName: "Test Product") 
            }
        };
        var expectedShoppingCartResponse = new ShoppingCartResponse {
            UserName = shoppingCart.UserName,
            Items = shoppingCart.Items.Select(item => new ShoppingCartItemResponse(quantity: item.Quantity, price: item.Price, productId: item.ProductId, imageFile: item.ImageFile, productName: item.ProductName)).ToList()
        };

        _mockBasketRepository.Setup(repo => repo.GetBasket(query.UserName)).ReturnsAsync(shoppingCart);

        // Act
        var actualShoppingCartResponse = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(actualShoppingCartResponse);
        Assert.IsType<ShoppingCartResponse>(actualShoppingCartResponse);
        Assert.Equal(expectedShoppingCartResponse.UserName, actualShoppingCartResponse.UserName);
        Assert.Equivalent(expectedShoppingCartResponse.Items, actualShoppingCartResponse.Items);
        Assert.Equivalent(expectedShoppingCartResponse, actualShoppingCartResponse);
    }
}
