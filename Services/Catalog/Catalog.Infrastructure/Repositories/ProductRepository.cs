using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using Catalog.Infrastructure.Data;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories;
public class ProductRepository : IProductRepository, IBrandRepository, ITypesRepository
{

    private readonly ICatalogContext _context;

    public ProductRepository(ICatalogContext context)
    {
        _context = context;
    }

    public async Task<Pagination<Product>> GetProducts(CatalogSpecParams catalogSpecParams)
    {
        var builder = Builders<Product>.Filter;
        var filter = Builders<Product>.Filter.Empty;
        if (!string.IsNullOrEmpty(catalogSpecParams.Search))
        {
            filter = filter & builder.Regex(x => x.Name, new BsonRegularExpression(catalogSpecParams.Search, "i"));
        }
        if (!string.IsNullOrEmpty(catalogSpecParams.BrandId))
        {
            filter = filter & builder.Eq(x => x.Brands.Id, catalogSpecParams.BrandId);
        }
        if (!string.IsNullOrEmpty(catalogSpecParams.TypeId))
        {
            filter = filter & builder.Eq(x => x.Types.Id, catalogSpecParams.TypeId);
        }
        if (!string.IsNullOrEmpty(catalogSpecParams.Sort))
        {
            return new Pagination<Product>
            {
                PageSize = catalogSpecParams.PageSize,
                PageIndex = catalogSpecParams.PageIndex,
                Data = await DataFilter(catalogSpecParams, filter),
                Count = await _context.Products.CountDocumentsAsync(p =>
                    true) //TODO: Need to check while applying with UI
            };
        }

        return new Pagination<Product>
        {
            PageSize = catalogSpecParams.PageSize,
            PageIndex = catalogSpecParams.PageIndex,
            Data = await _context
                .Products
                .Find(filter)
                .Sort(Builders<Product>.Sort.Ascending("Name"))
                .Skip(catalogSpecParams.PageSize * (catalogSpecParams.PageIndex - 1))
                .Limit(catalogSpecParams.PageSize)
                .ToListAsync(),
            Count = await _context.Products.CountDocumentsAsync(p => true)
        };
    }

    private async Task<IReadOnlyList<Product>> DataFilter(CatalogSpecParams catalogSpecParams, FilterDefinition<Product> filter)
    {
        return await _context
            .Products
            .Find(filter)
            .Sort(GetSortDefinition(catalogSpecParams.Sort))
            .Skip(catalogSpecParams.PageSize * (catalogSpecParams.PageIndex - 1))
            .Limit(catalogSpecParams.PageSize)
            .ToListAsync();
    }

    private SortDefinition<Product> GetSortDefinition(string? sortName)
    {
        switch (sortName)
        {
            case "priceAsc":
                return Builders<Product>.Sort.Ascending("Price");
            case "priceDesc":
                return Builders<Product>.Sort.Descending("Price");
            default:
                return Builders<Product>.Sort.Ascending("Name");
        }
    }

    public async Task<Product> GetProduct(string id)
    {
        return await _context
            .Products
            .Find(p => p.Id == id)
            .FirstOrDefaultAsync();
    }
    public async Task<IEnumerable<Product>> GetProductsByBrand(string name)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Brands.Name, name);
        return await _context
            .Products
            .Find(filter)
            .ToListAsync();
    }
    public async Task<IEnumerable<Product>> GetProductsByType(string name)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Types.Name, name);
        return await _context
            .Products
            .Find(filter)
            .ToListAsync();
    }
    public async Task<IEnumerable<Product>> GetProductsByName(string name)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Regex(p => p.Name, new BsonRegularExpression(name, "i"));
        return await _context
            .Products
            .Find(filter)
            .ToListAsync();

    }
    public async Task<Product> CreateProduct(Product product)
    {
        await _context.Products.InsertOneAsync(product);
        return product;
    }
    public async Task<bool> UpdateProduct(Product product)
    {
        var updateResult = await _context
            .Products
            .ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);
        return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
    }
    public async Task<bool> DeleteProduct(string id)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);
        DeleteResult deleteResult = await _context
            .Products
            .DeleteOneAsync(filter);
        return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
    }
    public async Task<IEnumerable<ProductBrand>> GetAllBrands()
    {
        return await _context
            .Brands
            .Find(p => true)
            .ToListAsync();
    }
    public async Task<IEnumerable<ProductType>> GetAllTypes()
    {
        return await _context
            .Types
            .Find(p => true)
            .ToListAsync();
    }
}
