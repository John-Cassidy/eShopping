using Discount.Application.Handlers;
using Discount.Application.Queries;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using Grpc.Core;
using Moq;

namespace Application.UnitTests.Discount;
public class GetDiscountQueryHandlerTests
{
    [Fact]
    public async Task Handle_ValidRequest_ReturnsCouponModel()
    {
        // Arrange
        var productName = "SampleProduct";
        var discountRepositoryMock = new Mock<IDiscountRepository>();
        var handler = new GetDiscountQueryHandler(discountRepositoryMock.Object);
        var query = new GetDiscountQuery(productName: productName);
        var coupon = new Coupon(productName: productName);
        discountRepositoryMock.Setup(repo => repo.GetDiscount(productName)).ReturnsAsync(coupon);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<CouponModel>(result);
        Assert.Equal(productName, result.ProductName);
    }

    [Fact]
    public async Task Handle_InvalidRequest_ThrowsRpcException()
    {
        // Arrange
        var productName = "NonExistingProduct";
        var discountRepositoryMock = new Mock<IDiscountRepository>();
        var handler = new GetDiscountQueryHandler(discountRepositoryMock.Object);
        var query = new GetDiscountQuery(productName: productName);
        Coupon? coupon = null;
        discountRepositoryMock.Setup(repo => repo.GetDiscount(productName)).ReturnsAsync(coupon);

        // Act & Assert
        await Assert.ThrowsAsync<RpcException>(() => handler.Handle(query, CancellationToken.None));
    }
}