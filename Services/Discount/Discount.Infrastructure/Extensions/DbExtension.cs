using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Discount.Infrastructure.Extensions;

public static class DbExtension {
    // create method to MigrateDatabase passing in IHost parameter returning IHost
    public static IHost MigrateDatabase<TContext>(this IHost host) {
        // create scope
        using var scope = host.Services.CreateScope();
        // get instance of services provider
        var services = scope.ServiceProvider;
        // create instance of TContext
        var config = services.GetRequiredService<IConfiguration>();
        // create instance of ILogger<TContext>
        var logger = services.GetRequiredService<ILogger<TContext>>();
        // apply any pending migrations
        try {
            logger.LogInformation("Discount postgresql DB Migration Started.");
            ApplyMigrations(config);
            logger.LogInformation("Discount postgresql DB Migration Completed.");
        } catch (System.Exception) {

            throw;
        }
        // return host
        return host;
    }

    private static void ApplyMigrations(IConfiguration config) {
        using var connection = new NpgsqlConnection(config.GetValue<string>("DatabaseSettings:ConnectionString"));
        connection.Open();
        using var command = new NpgsqlCommand();
        command.Connection = connection;
        command.CommandText = "DROP TABLE IF EXISTS Coupon";
        command.ExecuteNonQuery();

        command.CommandText = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY, ProductName VARCHAR(500) NOT NULL, Description TEXT, Amount INT)";
        command.ExecuteNonQuery();

        command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Adidas Quick Force Indoor Badminton Shoes', 'Shoe Discount', 500)";
        command.ExecuteNonQuery();

        command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Yonex VCORE Pro 100 A Tennis Racquet (270gm, Strung)', 'Racquet Discount', 700)";
        command.ExecuteNonQuery();

        connection.Close();
    }
}
