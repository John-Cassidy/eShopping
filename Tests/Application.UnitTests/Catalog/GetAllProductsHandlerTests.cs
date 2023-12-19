using Catalog.Application.Handlers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using Moq;

namespace Application.UnitTests.Catalog;
public class GetAllProductsHandlerTests {
    [Fact]
    public async Task Handle_ValidRequest_ReturnsProductListResponse() {
        // Arrange
        var productRepositoryMock = new Mock<IProductRepository>();
        // productRepositoryMock.Setup(repo => repo.GetProducts(It.IsAny<CatalogSpecParams>())).ReturnsAsync(It.IsAny<Pagination<Product>>());
        productRepositoryMock.Setup(repo => repo.GetProducts(It.IsAny<CatalogSpecParams>())).ReturnsAsync(new Pagination<Product>(1, 1, 1, new List<Product>()));

        var handler = new GetAllProductsHandler(productRepositoryMock.Object);
        var specParams = new CatalogSpecParams();
        var query = new GetAllProductsQuery(specParams);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<Pagination<ProductResponse>>(result);
    }
}