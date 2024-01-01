# eShopping

Based on Udemy Course: [.Net Core Microservices using Clean Architecture Implementation](https://www.udemy.com/course/creating-net-core-microservices-using-clean-architecture)

Medium Blog: [Complete guid to build enterprise edition application end-to-end](https://blog.stackademic.com/creating-net-core-microservices-using-clean-architecture-d229d1683ec9)

Original Authors Github Repo: [eShopping](https://github.com/rahulsahay19/eShopping/tree/master)

## GitHub Action Badges

[![build and test](https://github.com/John-Cassidy/eShopping/actions/workflows/build-and-test.yaml/badge.svg)](https://github.com/John-Cassidy/eShopping/actions/workflows/build-and-test.yaml)

## Start Project

Create blank Solution and folder structure

```powershell
dotnet new sln -n eShopping

mkdir ApiGateways
mkdir Infrastructure
mkdir Services
```

Build Command

```powershell
dotnet build eShopping.sln /property:GenerateFullPaths=true /consoleloggerparameters:NoSummary
```

## Catalog Microservice

```powershell
cd Services
mkdir Catalog
```

create 4 projects following Clean Architecture:

- Catalog.API
- Catalog.Application
- Catalog.Core
- Catalog.Infrastructure

## URLS

- Catalog.API: http://localhost:9000/swagger/index.html

  - http://localhost:9000/api/v1/Catalog/GetAllProducts
  - curl -X 'GET' 'http://localhost:9000/api/v1/Catalog/GetAllProducts' -H 'accept:text/plain'

- Basket.API: http://localhost:9001/swagger/index.html

- Discount.API: runs GRPC service on port: 9002 / use pgadmin to confirm data is available in postres db

- Ordering.API: http://localhost:9003/swagger/index.html

- eshopping-reverseproxy (NGINX Reverse Proxy)

  - http://10.0.0.12:44344
  - http://localhost:44344
  - http://host.docker.internal:44344
  - https://id-local.eshopping.com:44344

once hosts file updated: http://id-local.eshopping.com:44344

http://id-local.eshopping.com:44344/catalog.index.html
http://id-local.eshopping.com:44344/basket.index.html
http://id-local.eshopping.com:44344/order.index.html

## Dockerfile

Build Catalog.API image: open prompt in solution folder and rull following command:

```powershell

docker build --pull --rm -f "Services\Catalog\Catalog.API\Dockerfile" -t catalogapi:latest .
```

## Docker Compose

Create docker compose project in Visual Studio after Services\Catalog\Catalog.API\Dockerfile created.

run docker-compose:

```powershell

NOTE: REBUILD IMAGES TO INCLUDE CODE CHANGES AND START
docker-compose -f docker-compose.yml -f docker-compose.override.yml up --build
NOTE: START CONTAINERS FROM EXISTING IMAGES WITHOUT REBUILDING
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
NOTE: STOP RUNNING CONTAINERS AND REMOVE CONTAINERS
docker-compose -f docker-compose.yml -f docker-compose.override.yml down

```

## PGAdmin4 running in Docker Desktop

Connect to running postgres container 'dicountdb'

Name: DiscountServer
Hostname/address: host.docker.internal
Port: 5432
Maintenance database: postgres
Username: yourusername
Password: yourpassword

## Ordering

### dotnet ef migrations (option 1)

When setting up development for first time with SqlServer container, run manual migration from command line in solution folder.

```powershell
dotnet ef migrations add InitialCreate -p Services/Ordering/Ordering.Infrastructure -s Services/Ordering/Ordering.Api
```

Where InitialCreate is the name that we will give our migration, you can change this name with details of what your migration refers to, such as changing a field in a table, adding or removing fields, by convention we try to detail the update that the migration will do.

After -p (project) we pass the name of the solution that contains our DbContext in the case of Infrastructure and -s (solution) we pass our main project in the case of the API.

If everything went well after running the command you will get a message like this: 'Done. To undo this action, use ‘ef migrations remove’'

The migrations remove command is used to remove the created migration if it is not as you wanted.

```powershell
dotnet ef migrations remove -p Services/Ordering/Ordering.Infrastructure -s Services/Ordering/Ordering.Api
```

```powershell
dotnet ef database update -s Services/Ordering/Ordering.Api
```

### Package Manager Console Commands (option 2)

Open Package Manager Console and select > Ordering.Infrastructure project

Run command:

> Add-Migration InitialCreate

## MessageBus / Message Queue

[MassTransit RabbitMQ Transport Documentation](https://masstransit.io/documentation/transports/rabbitmq)

[MassTransit RabbitMQ Bus Transport Configuration](https://masstransit.io/documentation/configuration/transports/rabbitmq)

MassTransit v8 is the first major release since the availability of .NET 6. MassTransit v8 works a significant portion of the underlying components into a more manageable solution structure. Focused on the developer experience, while maintaining compatibility with previous versions, this release brings together the entire MassTransit stack.

[Upgrade Documentation for MassTransit v8](https://masstransit.io/support/upgrade#version-8)

[Documentation for MassTransit v8.1](https://masstransit.io/support/upgrade#version-81)

### RabbitMQ Management

[Local Development URL:](http://localhost:15672/)
username: guest
password: guest

## Application Gateway

use Ocelot Nuget Package to setup API Gateway.
[Ocelot API Gateway Documentation](https://ocelot.readthedocs.io/en/latest/introduction/gettingstarted.html)
[Microsoft Learn - Implement API Gateways with Ocelot](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/multi-container-microservice-net-applications/implement-api-gateways-with-ocelot)

### use Copilot Chat to create the Routes in ocelot.Development.json and ocelot.Local.json

### Basket API

> open ocelot.Development.json
> open BasketController.cs and keep file selected
> paste following command in Copilot Chat:
> you can modify the text below to generate routes for: Catalog.API, Ordering.API, and Discount.API

```text

in ocelot.Development.json file create Routes for each of the route in Basket.API BasketController.cs. for DownstreamHostAndPorts section Include Host: host.docker.internal and port 9001 from basket.api in docker-compose.override.yml. for DownstreamPathTemplate section Include /api/v1/Basket/. stringify value for "Port": 9001. for UpstreamPathTemplate do not include /api/v1. for DownstreamScheme key set value="http"

```

## Ocelot API Gateway

Setup project to route requests to apis.

### Copilot Request input on how to test Ocelot API Gateway

```text
How to create and run tests for Ocelot.APIGateway project that is hosted in docker container with port 9010. this api runs api that routes requests to Catalog.API, Basket.API, Discount.API, and Order.API with configurations in file ocelot.Development.json. To accomplish this it configures the gateway using nuget packages: Ocelot v22.01 and Ocelot.Cache.CacheManager v22.01.
```

### Copilot Reponse

To create and run tests for the Ocelot.APIGateway project, you can use a testing framework like xUnit. Here's a step-by-step guide:

1. **Create a new xUnit Test Project:** In Visual Studio, create a new xUnit Test Project in the same solution as your Ocelot.APIGateway project.

2. Add necessary NuGet packages: Add the `Microsoft.AspNetCore.TestHost` and `Microsoft.AspNetCore.Mvc.Testing` NuGet packages to your test project. These packages will allow you to create a test server and make HTTP requests to it.

3. **Create a custom WebApplicationFactory:** This class will create a TestServer that shares the same configuration and service registrations as your Ocelot.APIGateway project.

4. **Write your tests:** Create test methods that send HTTP requests to your API gateway and assert the responses.

5. **Run your tests:** Use the `dotnet test` command to run your tests.

Here's an example of how you can do this:

**Step 1: Create a new xUnit Test Project**

```powershell
dotnet new xunit -n Ocelot.APIGateway.Tests
```

**Step 2: Add necessary NuGet packages**

```powershell
dotnet add package Microsoft.AspNetCore.TestHost
dotnet add package Microsoft.AspNetCore.Mvc.Testing
```

**Step 3: Create a custom WebApplicationFactory**

```C#
public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseStartup<TStartup>();
    }
}
```

**Step 4: Write your tests**

```C#
public class GatewayTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly HttpClient _client;

    public GatewayTests(CustomWebApplicationFactory<Startup> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetCatalog_ReturnsSuccessStatusCode()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/Catalog");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();
    }
}
```

**Step 5: Run your tests**

```powershell
dotnet test
```

This will run your tests and display the results in the terminal.

## Duende IdentityServer v7 for .NET 8

Duende Software company offers IdentityServer dotnet project templates as a flexible and standards-compliant OpenID Connect and OAuth 2.x framework for ASP.NET Core.

[IdentityServer v7 Documentation](https://docs.duendesoftware.com/identityserver/v7/)

[GitHub Repo](https://github.com/DuendeSoftware/IdentityServer)

[Templates](https://github.com/DuendeSoftware/IdentityServer.Templates)

### Install Templates

```powershell

> dotnet new install Duende.IdentityServer.Templates

The following template packages will be installed:
   Duende.IdentityServer.Templates

Success: Duende.IdentityServer.Templates::6.3.1 installed the following templates:
Template Name                                               Short Name     Language  Tags
----------------------------------------------------------  -------------  --------  ------------------
Duende BFF Host using a Remote API                          bff-remoteapi  [C#]      Web/IdentityServer
Duende BFF using a Local API                                bff-localapi   [C#]      Web/IdentityServer
Duende IdentityServer Empty                                 isempty        [C#]      Web/IdentityServer
Duende IdentityServer Quickstart UI (UI assets only)        isui           [C#]      Web/IdentityServer
Duende IdentityServer with ASP.NET Core Identity            isaspid        [C#]      Web/IdentityServer
Duende IdentityServer with Entity Framework Stores          isef           [C#]      Web/IdentityServer
Duende IdentityServer with In-Memory Stores and Test Users  isinmem        [C#]      Web/IdentityServer
```

Uninstall with: `dotnet new uninstall Duende.IdentityServer.Templates`

### Project Creation

run following command from `Infrastructure` folder:

```powershell
dotnet new isinmem -n EShopping.Identity
```

this will install v6. after creation update:

- project net8
- identityserver nuget package to v7 preview 3
- update other nuget packages to net8 version respectively

[Steps when upgrading v6 to v7](https://docs.duendesoftware.com/identityserver/v7/upgrades/v6.3_to_v7.0/)
[Notes on 7.0.0-preview2](https://github.com/DuendeSoftware/IdentityServer/releases/tag/7.0.0-preview.2)

### Client Credential Flow

The Client Credentials Flow involves an application exchanging its application credentials, such as clientId and clientSecret, for an access_token.

## Nginx ApiGateway

create nginx.local.conf file to configure nginx api gateway and reverse proxy
create nginx.Dockerfile to build image
add sections to docker-compose.yml and docker-compose.override.yml for:

- nginx reverseproxy @port=9200
- identityserver @port=9009

build/run docker-compose
test nginx with route: [id-local.eshopping.com:9200](id-local.eshopping.com:9200)

### Update hosts file

Update hosts file to include resolution to URI: 127.0.0.1 id-local.eshopping.com

### Create Cert with OpenSSL

[Article on how to Create Self Signed Certificate](https://sockettools.com/kb/creating-certificate-using-openssl/)
[Article on how to Create Self Signed Certificate](https://www.humankode.com/asp-net-core/develop-locally-with-https-self-signed-certificates-and-asp-net-core/)

create id-local.conf file
run bash command from nginx folder:
`NOTE: modify password before running script!`

```bash
openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout id-local.key -out id-local.crt -config id-local.conf -passin pass:[password]
```

this will create id-local.key, id-local.crt

run bash command from nginx folder and enter password:

```bash
openssl pkcs12 -export -out id-local.pfx -inkey id-local.key -in id-local.crt
```

This will create id-local.pfx file.
Import certificate into on your machine.
