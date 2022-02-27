FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /app

COPY Catalog/Catalog.Contracts/ Catalog/Catalog.Contracts/
COPY Ordering/Ordering.Contracts/ Ordering/Ordering.Contracts/
COPY Clients/Management.WebApp/ Clients/Management.WebApp/
COPY EventBus/EventBus/ EventBus/EventBus/

RUN dotnet publish -c release -o out Clients/Management.WebApp/Management.WebApp.csproj

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Management.WebApp.dll"]