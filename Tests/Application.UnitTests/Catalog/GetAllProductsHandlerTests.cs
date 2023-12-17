using Catalog.Application.Handlers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using Moq;

namespace Application.UnitTests.Catalog;
public class GetAllProductsHandlerTests {
    [Fact]
    public async Task Handle_ValidRequest_ReturnsProductListResponse() {
        // Arrange
        var productRepositoryMock = new Mock<IProductRepository>();
        var handler = new GetAllProductsHandler(productRepositoryMock.Object);
        var query = new GetAllProductsQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IList<ProductResponse>>(result);
    }
}