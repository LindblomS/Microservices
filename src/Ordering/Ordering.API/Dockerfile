FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /app

COPY . .
RUN dotnet publish -c release -o out Ordering/Ordering.API/Ordering.API.csproj

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Ordering.API.dll"]