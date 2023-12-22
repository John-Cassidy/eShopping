using Discount.Application.Commands;
using Discount.Application.Handlers;
using Discount.Core.Repositories;
using Moq;

namespace Discount.Application.UnitTests.Handlers;

public class DeleteDiscountCommandHandlerTests
{
    private readonly Mock<IDiscountRepository> _mockDiscountRepository;
    private readonly DeleteDiscountCommandHandler _handler;

    public DeleteDiscountCommandHandlerTests()
    {
        _mockDiscountRepository = new Mock<IDiscountRepository>();
        _handler = new DeleteDiscountCommandHandler(_mockDiscountRepository.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_CallsDeleteDiscountAndReturnsTrue()
    {
        // Arrange
        var command = new DeleteDiscountCommand(productName: "TestProduct");

        _mockDiscountRepository.Setup(r => r.DeleteDiscount(command.ProductName)).ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        _mockDiscountRepository.Verify(r => r.DeleteDiscount(command.ProductName), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidRequest_CallsDeleteDiscountAndReturnsFalse()
    {
        // Arrange
        var command = new DeleteDiscountCommand(productName: "TestProduct");

        _mockDiscountRepository.Setup(r => r.DeleteDiscount(command.ProductName)).ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        _mockDiscountRepository.Verify(r => r.DeleteDiscount(command.ProductName), Times.Once);
    }
}
