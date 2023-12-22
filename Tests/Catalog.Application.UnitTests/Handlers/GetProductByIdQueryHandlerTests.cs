using Catalog.Application.Handlers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Moq;

namespace Catalog.Application.UnitTests.Handlers;
public class GetProductByIdQueryHandlerTests {
    [Fact]
    public async Task Handle_ValidId_ReturnsProductResponse() {
        // Arrange
        var brandRepositoryMock = new Mock<IProductRepository>();
        var handler = new GetProductByIdQueryHandler(brandRepositoryMock.Object);
        var query = new GetProductByIdQuery(Guid.NewGuid().ToString()); // Pass the 'id' parameter

        brandRepositoryMock.Setup(repo => repo.GetProduct(It.IsAny<string>())).ReturnsAsync(new Product());

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ProductResponse>(result);
    }
}
