using Catalog.Application;
using Catalog.Application.Handlers;
using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;
using Moq;
using Xunit;

namespace Application.UnitTests.Catalog;
public class GetProductsByBrandHandlerTests {
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly GetProductsByBrandHandler _handler;

    public GetProductsByBrandHandlerTests() {
        _productRepositoryMock = new Mock<IProductRepository>();
        _handler = new GetProductsByBrandHandler(_productRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsProductResponseList() {
        // Arrange
        var brandName = "exampleBrand";
        var query = new GetProductsByBrandQuery(brandName);
        _productRepositoryMock.Setup(repo => repo.GetProductsByBrand(brandName)).ReturnsAsync(new List<Product>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<ProductResponse>>(result);
    }
}