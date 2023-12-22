using Xunit;
using Moq;
using Catalog.Core.Repositories;
using Catalog.Application.Mappers;
using Catalog.Application.Responses;
using Catalog.Application;
using Catalog.Core.Entities;
using Catalog.Application.Handlers;
using Catalog.Application.Commands;

namespace Catalog.Application.UnitTests.Handlers;
public class CreateProductHandlerTests {
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly CreateProductHandler _handler;

    public CreateProductHandlerTests() {
        _productRepositoryMock = new Mock<IProductRepository>();
        _handler = new CreateProductHandler(_productRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsProductResponse() {
        // Arrange
        CreateProductCommand command = new CreateProductCommand {
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
        Product newProduct = new Product {
            Id = Guid.NewGuid().ToString(),
            Name = command.Name,
            Summary = command.Summary,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price,
            Brands = command.Brands,
            Types = command.Types
        };       
        var expectedProductResponse = new ProductResponse {
            Id = newProduct.Id,
            Name = newProduct.Name,
            Summary = newProduct.Summary,
            Description = newProduct.Description,
            ImageFile = newProduct.ImageFile,
            Price = newProduct.Price,
            Brands = new BrandResponse {
                Id = newProduct.Brands.Id,
                Name = newProduct.Brands.Name
            },
            Types = new TypesResponse {
                Id = newProduct.Types.Id,
                Name = newProduct.Types.Name
            }
        };

        _productRepositoryMock.Setup(r => r.CreateProduct(It.IsAny<Product>())).ReturnsAsync(newProduct);

        // Act
        var actualProductResponse = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equivalent(expectedProductResponse, actualProductResponse);
    }   
}