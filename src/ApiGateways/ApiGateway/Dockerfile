FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /app

COPY ApiGateways/ApiGateway/ ApiGateways/ApiGateway/

RUN dotnet publish -c release -o out ApiGateways/ApiGateway/ApiGateway.csproj

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "ApiGateway.dll"]