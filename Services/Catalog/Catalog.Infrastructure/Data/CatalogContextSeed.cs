using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data;
public class CatalogContextSeed {
    public static void SeedData(IMongoCollection<Product> productCollection) {
        bool checkProducts = productCollection.Find(p => true).Any();
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Seed", "products.json");
        if (!checkProducts) {
            var productsData = File.ReadAllText(path);
            var productsList = JsonSerializer.Deserialize<List<Product>>(productsData);
            if (productsList != null) {
                productCollection.InsertManyAsync(productsList);
            }
        }
    }
}
