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

- Catalog.API: https://localhost:9000/swagger/index.html
