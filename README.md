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

```text

in ocelot.Development.json file create Routes for each of the route in Basket.API BasketController.cs. for DownstreamHostAndPorts section Include Host: host.docker.internal and port 9001 from basket.api in docker-compose.override.yml. for DownstreamPathTemplate section Include /api/v1/Basket/. stringify value for "Port": 9001. for UpstreamPathTemplate do not include /api/v1. for DownstreamScheme key set value="http"

```

### Catalog.API

```text

in ocelot.Development.json file create Routes for each of the route in Catalog.API CatalogController.cs. for DownstreamHostAndPorts section Include Host: host.docker.internal and port 9000 from catalog.api in docker-compose.override.yml. for DownstreamPathTemplate section Include /api/v1/Catalog/. stringify value for "Port": 9000. for UpstreamPathTemplate do not include /api/v1. for DownstreamScheme key set value="http"

```

### Ordering.API

```text

in ocelot.Development.json file create Routes for each of the route in Ordering.API OrderController.cs. for DownstreamHostAndPorts section Include Host: host.docker.internal and port 9003 from ordering.api in docker-compose.override.yml. for DownstreamPathTemplate section Include /api/v1/Order/. stringify value for "Port": 9003. for UpstreamPathTemplate do not include /api/v1. for DownstreamScheme key set value="http"

```

### Disount.API

manually create the routes
