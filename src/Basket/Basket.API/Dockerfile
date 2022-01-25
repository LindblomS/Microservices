FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /app

COPY Ordering/Ordering.Contracts/ Ordering/Ordering.Contracts/
COPY Catalog/Catalog.Contracts/ Catalog/Catalog.Contracts/
COPY Basket/Basket.API/ Basket/Basket.API/
COPY Basket/Basket.Contracts/ Basket/Basket.Contracts/
COPY Basket/Basket.Domain/ Basket/Basket.Domain/
COPY Basket/Basket.Infrastructure/ Basket/Basket.Infrastructure/
COPY EventBus/EventBusRabbitMQ/ EventBus/EventBusRabbitMQ/
COPY EventBus/EventBus/ EventBus/EventBus/

RUN dotnet publish -c release -o out Basket/Basket.API/Basket.API.csproj

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Basket.API.dll"]