using System.Reflection;
using Catalog.API.Middleware;
using Catalog.Application.Handlers;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Repositories;
using Common.Logging.Correlation;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Catalog.API;

public class Startup
{
    public IConfiguration Configuration;
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApiVersioning();
        services.AddCors(options =>
        {
            //TODO read the same from settings for prod deployment
            options.AddPolicy("CorsPolicy",
                policy => { policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin(); });
        });
        // .AddVersionedApiExplorer(
        // options =>
        // {
        //     options.GroupNameFormat = "'v'VVV";
        //     options.SubstituteApiVersionInUrl = true;
        // });        

        var connectionString = Configuration["DatabaseSettings:ConnectionString"];
        if (connectionString == null)
        {
            throw new InvalidOperationException("Database connection string is not configured.");
        }
        services.AddHealthChecks()
            .AddMongoDb(connectionString,
                name: "CatalogDBCheck",
                HealthStatus.Degraded);

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog.API", Version = "v1" });
        });

        services.AddAutoMapper(typeof(Startup));
        services.AddMediatR(typeof(CreateProductHandler).GetTypeInfo().Assembly);
        services.AddScoped<ICatalogContext, CatalogContext>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IBrandRepository, ProductRepository>();
        services.AddScoped<ITypesRepository, ProductRepository>();
        services.AddScoped<ICorrelationIdGenerator, CorrelationIdGenerator>();

        // without authentication
        services.AddControllers();

        // // with authentication localhost:9099
        // var userPolicy = new AuthorizationPolicyBuilder()
        //     .RequireAuthenticatedUser()
        //     .Build();

        // services.AddControllers(config =>
        // {
        //     config.Filters.Add(new AuthorizeFilter(userPolicy));
        // });


        // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //         .AddJwtBearer(options =>
        //         {
        //             options.Authority = "https://localhost:9099";
        //             options.Audience = "Catalog";
        //         });
        // services.AddAuthorization(options =>
        // {
        //     options.AddPolicy("CatalogApi", policy => policy.RequireClaim("scope", "catalogapi"));
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
        //             ValidAudience = "Catalog",
        //         };
        //         // options.Audience = "Catalog";    
        //         options.Audience = "https://id-local.eshopping.com:44344/resources";
        //     });
        // services.AddAuthorization(options =>
        //     options.AddPolicy("CatalogApi", policy =>
        //     {
        //         policy.RequireAuthenticatedUser();
        //         policy.RequireClaim("scope", "catalogapi");
        //     })
        // );
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) //, IApiVersionDescriptionProvider provider)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog.API v1"));
        }

        // var nginxPath = "/catalog";
        // if (env.IsEnvironment("Local"))
        // {
        //     app.UseDeveloperExceptionPage();
        //     app.UseSwagger();
        //     app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog.API v1"));
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
        //                 $"Catalog API {description.GroupName.ToUpperInvariant()}");
        //             options.RoutePrefix = string.Empty;
        //         }

        //         options.DocumentTitle = "Catalog API Documentation";
        //     });
        // }

        // app.UseMiddleware<AuthorizationLoggingMiddleware>();
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors("CorsPolicy");
        app.UseAuthentication();
        app.UseStaticFiles();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers(); // .RequireAuthorization("ApiScope");
            endpoints.MapHealthChecks("/health", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        });
    }
}
