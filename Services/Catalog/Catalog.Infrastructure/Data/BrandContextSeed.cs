﻿using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data;
public static class BrandContextSeed {

    public static void SeedData(IMongoCollection<ProductBrand> brandCollection) {
        bool checkBrands = brandCollection.Find(b => true).Any();
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Seed", "brands.json");
        if (!checkBrands) {
            var brandsData = File.ReadAllText(path);
            var brandsList = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
            if (brandsList != null) {
                brandCollection.InsertMany(brandsList);
            }
        }
    }
}
