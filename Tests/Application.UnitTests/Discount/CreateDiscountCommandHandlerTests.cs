// FILEPATH: /C:/DEV/github.com/eShopping/Tests/Application.UnitTests/Discount/CreateDiscountCommandHandlerTest.cs

using Discount.Application.Handlers;
using Discount.Application.Commands;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using AutoMapper;
using Moq;
using Xunit;
using Discount.Grpc.Protos;

namespace Application.UnitTests.Discount;

public class CreateDiscountCommandHandlerTests
{
    private readonly Mock<IDiscountRepository> _mockDiscountRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CreateDiscountCommandHandler _handler;

    public CreateDiscountCommandHandlerTests()
    {
        _mockDiscountRepository = new Mock<IDiscountRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new CreateDiscountCommandHandler(_mockDiscountRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_CreatesDiscountAndReturnsCouponModel()
    {
        // Arrange
        var command = new CreateDiscountCommand { ProductName = "TestProduct", Amount = 10, Description = "TestDescription" };
        var coupon = new Coupon(productName: "TestProduct", description: "TestDescription", amount: 10);
        var couponModel = new CouponModel { ProductName = "TestProduct", Amount = 10, Description = "TestDescription" };

        _mockMapper.Setup(m => m.Map<Coupon>(command)).Returns(coupon);
        _mockMapper.Setup(m => m.Map<CouponModel>(coupon)).Returns(couponModel);
        _mockDiscountRepository.Setup(r => r.CreateDiscount(coupon)).ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(couponModel, result);
        _mockDiscountRepository.Verify(r => r.CreateDiscount(coupon), Times.Once);
        _mockMapper.Verify(m => m.Map<Coupon>(command), Times.Once);
        _mockMapper.Verify(m => m.Map<CouponModel>(coupon), Times.Once);
    }
}