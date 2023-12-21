using System.Reflection;
using Discount.API.Services;
using Discount.Application.Handlers;
using Discount.Core.Repositories;
using Discount.Infrastructure.Repositories;
using MediatR;

namespace Discount.API;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMediatR(typeof(CreateDiscountCommandHandler).GetTypeInfo().Assembly);
        services.AddAutoMapper(typeof(Startup));
        services.AddScoped<IDiscountRepository, DiscountRepository>();
        services.AddGrpc();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGrpcService<DiscountService>();
            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync(
                    "Communication with gRPC endpoints must be made through a gRPC client.");
            });
        });
    }

}
