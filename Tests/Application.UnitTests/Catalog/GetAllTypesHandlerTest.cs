using Catalog.Application.Handlers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using Moq;

namespace Application.UnitTests.Catalog;
public class GetAllTypesHandlerTests {
    [Fact]
    public async Task Handle_ValidRequest_ReturnsTypesResponseList() {
        // Arrange
        var typesRepositoryMock = new Mock<ITypesRepository>();
        var handler = new GetAllTypesHandler(typesRepositoryMock.Object);
        var query = new GetAllTypesQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.IsType<List<TypesResponse>>(result);
    }
}