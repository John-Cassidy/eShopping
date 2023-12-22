using Xunit;
using Moq;
using Basket.Core.Repositories;
using Basket.Application.Handlers;
using Basket.Application.Commands;
using Basket.Core.Entities;
using Basket.Application.Responses;
using Basket.Application.GrpcService;
using Discount.Grpc.Protos;
using static Discount.Grpc.Protos.DiscountService;

namespace Basket.Application.UnitTests.Handlers;

public class CreateShoppingCartCommandHandlerTests {
    private readonly Mock<IBasketRepository> _basketRepositoryMock;
    private readonly Mock<DiscountService.DiscountServiceClient> _discountServiceClient;
    private readonly Mock<IDiscountGrpcService> _discountGrpcServiceMock;
    private readonly CreateShoppingCartCommandHandler _handler;

    public CreateShoppingCartCommandHandlerTests() {
        _basketRepositoryMock = new Mock<IBasketRepository>();
        _discountServiceClient = new Mock<DiscountService.DiscountServiceClient>();
        _discountGrpcServiceMock = new Mock<IDiscountGrpcService>();        
        _handler = new CreateShoppingCartCommandHandler(_basketRepositoryMock.Object, _discountGrpcServiceMock.Object);
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

        CouponModel coupon = new CouponModel {
            Amount = 10,
            Description = "Test Discount"
        };

        var expectedShoppingCartResponse = new ShoppingCartResponse {
            UserName = newShoppingCart.UserName,
            Items = command.Items.Select(item => new ShoppingCartItemResponse(quantity: item.Quantity, price: item.Price, productId: item.ProductId, imageFile: item.ImageFile, productName: item.ProductName)).ToList()
        };

        _discountGrpcServiceMock.Setup(x => x.GetDiscount(It.IsAny<string>())).ReturnsAsync(coupon);
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
