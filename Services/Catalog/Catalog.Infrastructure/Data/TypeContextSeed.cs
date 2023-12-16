using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data;
public class TypeContextSeed {
    public static void SeedData(IMongoCollection<ProductType> typeCollection) {
        bool checkTypes = typeCollection.Find(b => true).Any();
        string path = Path.Combine("Data", "SeedData", "types.json");
        if (!checkTypes) {
            var typesData = File.ReadAllText(path);
            var typesList = JsonSerializer.Deserialize<List<ProductType>>(typesData);
            if (typesList != null) {
                typeCollection.InsertMany(typesList);
            }
        }
    }
}

