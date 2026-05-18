FROM mcr.microsoft.com/dotnet/sdk:9.0
WORKDIR /app
COPY . .
RUN dotnet restore
RUN dotnet publish -c release -o out
EXPOSE 8080
ENTRYPOINT ["dotnet", "out/LiteraryLounge.API.dll"]