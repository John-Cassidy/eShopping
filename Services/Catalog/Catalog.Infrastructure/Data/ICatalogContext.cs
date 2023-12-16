using MongoDB.Driver;
using Catalog.Core.Entities;

namespace Catalog.Infrastructure.Data;
public interface ICatalogContext {
    IMongoCollection<Product> Products { get; }
    IMongoCollection<ProductType> Types { get; }
    IMongoCollection<ProductBrand> Brands { get; }
}
