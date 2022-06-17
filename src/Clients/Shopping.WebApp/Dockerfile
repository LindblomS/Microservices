FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /app

COPY Clients/Shopping.WebApp/ Clients/Shopping.WebApp/
COPY Catalog/Catalog.Contracts/ Catalog/Catalog.Contracts/
COPY Basket/Basket.Contracts/ Basket/Basket.Contracts/
COPY EventBus/EventBus/ EventBus/EventBus/

RUN dotnet publish -c release -o out Clients/Shopping.WebApp/Shopping.WebApp.csproj

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Shopping.WebApp.dll"]