using System.Reflection;
using Basket.Application.GrpcService;
using Basket.Application.Handlers;
using Basket.Core.Repositories;
using Basket.Infrastructure;
using Common.Logging.Correlation;
using Discount.Grpc.Protos;
using HealthChecks.UI.Client;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;

namespace Basket.API;

public class Startup
{
    public IConfiguration Configuration;

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // without authentication
        services.AddControllers();

        services.AddApiVersioning();
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policy =>
            {
                //TODO read the same from settings for prod deployment
                policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
            });
        });
        services.AddVersionedApiExplorer(
            options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

        // add redis
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = Configuration.GetValue<string>("CacheSettings:ConnectionString");
        });

        // add mediatr
        services.AddMediatR(typeof(CreateShoppingCartCommandHandler).GetTypeInfo().Assembly);
        services.AddScoped<IBasketRepository, BasketRepository>();
        services.AddScoped<ICorrelationIdGenerator, CorrelationIdGenerator>();
        services.AddAutoMapper(typeof(Startup));

        // add scoped DiscountGrpcService
        services.AddScoped<IDiscountGrpcService, DiscountGrpcService>();
        // add grpc client DiscountService.DiscountServiceClient
        services.AddGrpcClient<DiscountService.DiscountServiceClient>(options =>
        {
            options.Address = new Uri(Configuration["GrpcSettings:DiscountUrl"]);
        });

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket.API", Version = "v1" });
        });
        services.AddHealthChecks()
            .AddRedis(Configuration["CacheSettings:ConnectionString"], "Redis Health", HealthStatus.Degraded);
        services.AddMassTransit(config =>
        {
            config.UsingRabbitMq((ct, cfg) =>
            {
                cfg.Host(Configuration["EventBusSettings:HostAddress"]);
            });
        });
        services.AddMassTransitHostedService(); // for MassTransit 7.0

        // // with authentication localhost:9099
        // var userPolicy = new AuthorizationPolicyBuilder()
        //     .RequireAuthenticatedUser()
        //     .Build();

        // services.AddControllers(config =>
        // {
        //     config.Filters.Add(new AuthorizeFilter(userPolicy));
        // });

        // services
        //     .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //     .AddJwtBearer(options =>
        //     {
        //         options.Authority = "https://localhost:9099";
        //         options.Audience = "Basket";
        //     });
        // services.AddAuthorization(options =>
        // {
        //     options.AddPolicy("BasketApi", policy => policy.RequireClaim("scope", "basketapi"));
        // });

        // // with authentication id-local.eshopping.com:44344
        // services
        //     .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //     .AddJwtBearer(options =>
        //     {
        //         options.Authority = "https://id-local.eshopping.com:44344";
        //         options.RequireHttpsMetadata = false;
        //         options.TokenValidationParameters = new TokenValidationParameters
        //         {
        //             ValidateIssuer = true,
        //             ValidateAudience = true,
        //             ValidateLifetime = true,
        //             ValidIssuer = "https://id-local.eshopping.com:44344",
        //             ValidAudience = "Basket",
        //         };
        //         // options.Audience = "Basket";    
        //         options.Audience = "https://id-local.eshopping.com:44344/resources";
        //     });
        // services.AddAuthorization(options =>
        //     options.AddPolicy("BasketApi", policy =>
        //     {
        //         policy.RequireAuthenticatedUser();
        //         policy.RequireClaim("scope", "basketapi");
        //     })
        // );

    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog.API v1"));
        }

        // var nginxPath = "/basket";
        // if (env.IsEnvironment("Local"))
        // {
        //     app.UseDeveloperExceptionPage();
        //     app.UseSwagger();
        //     app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket.API v1"));
        // }
        // if (env.IsDevelopment())
        // {
        //     app.UseDeveloperExceptionPage();
        //     app.UseForwardedHeaders(new ForwardedHeadersOptions
        //     {
        //         ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        //     });
        //     app.UseSwagger();
        //     app.UseSwaggerUI(options =>
        //     {
        //         foreach (var description in provider.ApiVersionDescriptions)
        //         {
        //             options.SwaggerEndpoint($"{nginxPath}/swagger/{description.GroupName}/swagger.json",
        //                 $"Basket API {description.GroupName.ToUpperInvariant()}");
        //             options.RoutePrefix = string.Empty;
        //         }

        //         options.DocumentTitle = "Basket API Documentation";

        //     });
        // }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors("CorsPolicy");
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/health", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        });
    }
}