using AutoMapper;
using Discount.Application.Commands;
using Discount.Application.Handlers;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using Moq;

namespace Application.UnitTests.Discount;

public class UpdateDiscountCommandHandlerTests
{
    private readonly Mock<IDiscountRepository> _mockDiscountRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly UpdateDiscountCommandHandler _handler;

    public UpdateDiscountCommandHandlerTests()
    {
        _mockDiscountRepository = new Mock<IDiscountRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new UpdateDiscountCommandHandler(_mockDiscountRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_UpdatesDiscountAndReturnsCouponModel()
    {
        // Arrange
        var command = new UpdateDiscountCommand { Id = 1, ProductName = "TestProduct", Amount = 10, Description = "TestDescription" };
        var coupon = new Coupon(id: 1, productName: "TestProduct", description: "TestDescription", amount: 10);
        var couponModel = new CouponModel { Id = 1, ProductName = "TestProduct", Amount = 10, Description = "TestDescription" };

        _mockMapper.Setup(m => m.Map<Coupon>(command)).Returns(coupon);
        _mockMapper.Setup(m => m.Map<CouponModel>(coupon)).Returns(couponModel);
        _mockDiscountRepository.Setup(r => r.UpdateDiscount(coupon)).ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(couponModel, result);
        _mockDiscountRepository.Verify(r => r.UpdateDiscount(coupon), Times.Once);
        _mockMapper.Verify(m => m.Map<Coupon>(command), Times.Once);
        _mockMapper.Verify(m => m.Map<CouponModel>(coupon), Times.Once);
    }
}
