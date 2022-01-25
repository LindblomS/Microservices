FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /app

COPY Ordering/Ordering.Worker/ Ordering/Ordering.Worker/
COPY Ordering/Ordering.Contracts/ Ordering/Ordering.Contracts/
COPY EventBus/EventBusRabbitMQ/ EventBus/EventBusRabbitMQ/
COPY EventBus/EventBus/ EventBus/EventBus/

RUN dotnet publish -c release -o out Ordering/Ordering.Worker/Ordering.Worker.csproj

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Ordering.Worker.dll"]