using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data;

public class OrderContextSeed
{
    // create SeedAsnyc method with OrderContext dbContext parameter and ILogger<OrderContextSeed> logger parameter
    public static async Task SeedAsync(OrderContext dbContext, ILogger<OrderContextSeed> logger)
    {
        // check if there is any data in the database
        if (!dbContext.Orders.Any())
        {
            // create new list of orders
            dbContext.Orders.AddRange(GetOrders());

            // save changes to the database
            await dbContext.SaveChangesAsync();

            // log information
            logger.LogInformation($"Seed database associated with context {typeof(OrderContext).Name}");
        }
    }

    private static IEnumerable<Order> GetOrders()
    {
        // return new list of orders
        return new List<Order>()
        {
            new Order()  {
                // add all properties and assign values
                UserName = "frizzo",
                FirstName = "Frank",
                LastName = "Rizzo",
                EmailAddress = "frankrizzo@gmail.com",
                AddressLine = "50 Main St.",
                Country = "USA",
                TotalPrice = 750,
                State = "MA",
                ZipCode = "02138",
                CardName = "Visa",
                CardNumber = "1234567890123456",
                CreatedBy = "Rizzo",
                Expiration = "12/25",
                Cvv = "123",
                PaymentMethod = 1,
                LastModifiedBy = "Rizzo",
                LastModifiedDate = DateTime.Now,
            }
        };
    }
}
