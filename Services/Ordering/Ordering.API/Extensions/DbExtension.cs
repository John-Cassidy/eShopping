using Microsoft.EntityFrameworkCore;

namespace Ordering.API;

public static class DbExtension
{
    // create static method MigrateDatabase<TContext> that extends IHost with Action<TContext, IServiceProvider> seeder where TContext : DbContext
    // inside the method, get the service scope factory from the host services
    // create a scope from the scope factory
    // get the service provider from the scope
    // get the TContext from the service provider
    // call the Migrate method on the TContext
    // call the seeder with the TContext and the service provider
    // dispose the scope
    // return the host
    public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder)
        where TContext : DbContext
    {
        using var scope = host.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var logger = serviceProvider.GetRequiredService<ILogger<TContext>>();
        var context = serviceProvider.GetRequiredService<TContext>();
        try
        {
            logger.LogInformation($"Started Db Migration: {typeof(TContext).Name}");
            CallSeeder(seeder, serviceProvider, context);
            logger.LogInformation($"Migration Completed: {typeof(TContext).Name}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"An error occurred while migrating the database used on context {typeof(TContext).Name}");
        }
        return host;
    }

    private static void CallSeeder<TContext>(Action<TContext, IServiceProvider> seeder, IServiceProvider serviceProvider, TContext context) where TContext : DbContext
    {
        context.Database.Migrate();
        seeder(context, serviceProvider);
    }
}
