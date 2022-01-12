FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /app

COPY Payment/Payment.API/ Payment/Payment.API/
COPY Payment/Payment.Contracts/ Payment/Payment.Contracts/
COPY EventBus/EventBusRabbitMQ/ EventBus/EventBusRabbitMQ/
COPY Order/Order.Contracts/ Order/Order.Contracts/
COPY EventBus/EventBus/ EventBus/EventBus/

RUN dotnet publish -c release -o out Payment/Payment.API/Payment.API.csproj

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Payment.API.dll"]