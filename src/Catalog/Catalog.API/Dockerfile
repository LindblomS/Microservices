FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /app

COPY Catalog/Catalog.API/ Catalog/Catalog.API/
COPY Catalog/Catalog.Domain/ Catalog/Catalog.Domain/
COPY Catalog/Catalog.Contracts/ Catalog/Catalog.Contracts/
COPY Catalog/Catalog.Infrastructure/ Catalog/Catalog.Infrastructure/
COPY Ordering/Ordering.Contracts/ Ordering/Ordering.Contracts/
COPY EventBus/EventBusRabbitMQ/ EventBus/EventBusRabbitMQ/
COPY EventBus/EventBus/ EventBus/EventBus/

RUN dotnet publish -c release -o out Catalog/Catalog.API/Catalog.API.csproj

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Catalog.API.dll"]