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

namespace Catalog.Application.UnitTests.Handlers;
public class GetProductsByNameQueryHandlerTests {
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly GetProductsByNameQueryHandler _handler;

    public GetProductsByNameQueryHandlerTests() {
        _productRepositoryMock = new Mock<IProductRepository>();
        _handler = new GetProductsByNameQueryHandler(_productRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsProductResponseList() {
        // Arrange
        var productName = "example";
        var query = new GetProductsByNameQuery(productName);
        _productRepositoryMock.Setup(repo => repo.GetProductsByName(query.Name)).ReturnsAsync(new List<Product>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<ProductResponse>>(result);
    }
}
