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
public class GetProductsByTypeHandlerTests {
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly GetProductsByTypeHandler _handler;

    public GetProductsByTypeHandlerTests() {
        _productRepositoryMock = new Mock<IProductRepository>();
        _handler = new GetProductsByTypeHandler(_productRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsProductResponseList() {
        // Arrange
        var typename = "exampleType";
        var query = new GetProductsByTypeQuery(typename);
        _productRepositoryMock.Setup(repo => repo.GetProductsByType(typename)).ReturnsAsync(new List<Product>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<ProductResponse>>(result);
    }
}
