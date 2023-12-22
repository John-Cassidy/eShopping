using AutoMapper;
using Catalog.Application.Handlers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Moq;

namespace Catalog.Application.UnitTests.Handlers;
public class GetAllBrandsHandlerTests {
    [Fact]
    public async Task Handle_Should_Return_BrandResponseList() {
        // Arrange
        var brandList = new List<ProductBrand>();
        var brandResponseList = new List<BrandResponse>();
        var brandRepositoryMock = new Mock<IBrandRepository>();
        var handler = new GetAllBrandsHandler(brandRepositoryMock.Object);

        brandRepositoryMock.Setup(repo => repo.GetAllBrands()).ReturnsAsync(brandList);

        var query = new GetAllBrandsQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(brandResponseList, result);
    }
}
