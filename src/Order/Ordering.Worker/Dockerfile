FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /app

COPY Order/Ordering.Worker/ Order/Ordering.Worker/
COPY Order/Order.Contracts/ Order/Order.Contracts/
COPY EventBus/EventBusRabbitMQ/ EventBus/EventBusRabbitMQ/
COPY EventBus/EventBus/ EventBus/EventBus/

RUN dotnet publish -c release -o out Order/Ordering.Worker/Ordering.Worker.csproj

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Ordering.Worker.dll"]