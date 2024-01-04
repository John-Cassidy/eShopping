FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# USER app
WORKDIR /app
ENV ASPNETCORE_URLS http://*:9009;https://*:9099
EXPOSE 9009
EXPOSE 9099

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Infrastructure/EShopping.Identity/EShopping.Identity.csproj", "Infrastructure/EShopping.Identity/"]
COPY ["Infrastructure/Common.Logging/Common.Logging.csproj", "Infrastructure/Common.Logging/"]
RUN dotnet restore "./Infrastructure/EShopping.Identity/./EShopping.Identity.csproj"
COPY . .
WORKDIR "/src/Infrastructure/EShopping.Identity"
RUN dotnet build "./EShopping.Identity.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EShopping.Identity.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EShopping.Identity.dll"]