using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Application.Responses;

 public class ProductResponse {
        // create properties for ProductResponse based on Product entity

        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("Name")]
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string ImageFile { get; set; }        
        public BrandResponse Brands { get; set; }
        public TypesResponse Types { get; set; }
        public decimal Price { get; set; }
 }
