FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY LiteraryLounge.API.csproj .
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish -c release -o /app

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "LiteraryLounge.API.dll"]