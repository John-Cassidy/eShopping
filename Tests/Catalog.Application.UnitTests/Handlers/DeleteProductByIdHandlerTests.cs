using Catalog.Application;
using Catalog.Application.Commands;
using Catalog.Application.Handlers;
using Catalog.Core.Repositories;
using MediatR;
using Moq;
using Xunit;

namespace Catalog.Application.UnitTests.Handlers;
public class DeleteProductByIdHandlerTests {
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly DeleteProductByIdHandler _handler;

    public DeleteProductByIdHandlerTests() {
        _productRepositoryMock = new Mock<IProductRepository>();
        _handler = new DeleteProductByIdHandler(_productRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsTrue() {
        // Arrange
        var command = new DeleteProductByIdCommand(Guid.NewGuid().ToString());
        _productRepositoryMock.Setup(repo => repo.DeleteProduct(command.Id)).ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
    }
}
