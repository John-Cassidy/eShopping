using Xunit;
using Moq;
using Catalog.Application;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Application.Handlers;
using Catalog.Application.Commands;

namespace Application.UnitTests.Catalog;
public class UpdateProductHandlerTests {
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly UpdateProductHandler _updateProductHandler;

    public UpdateProductHandlerTests() {
        _productRepositoryMock = new Mock<IProductRepository>();
        _updateProductHandler = new UpdateProductHandler(_productRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsTrue() {
        // Arrange
        var updateProductCommand = new UpdateProductCommand {
            Id = Guid.NewGuid().ToString(),
            Name = "New Product Name",
            Description = "New Product Description",
            ImageFile = "new_product_image.jpg",
            Price = 9.99m,
            Summary = "New Product Summary",
            Brands = new ProductBrand {
                Id = Guid.NewGuid().ToString(),
                Name = "Test Brand"
            },
            Types = new ProductType {
                Id = Guid.NewGuid().ToString(),
                Name = "Test Type"
            }
        };

        _productRepositoryMock.Setup(repo => repo.UpdateProduct(It.IsAny<Product>()))
            .ReturnsAsync(true);

        // Act
        var result = await _updateProductHandler.Handle(updateProductCommand, CancellationToken.None);

        // Assert
        Assert.True(result);
    }
}
