FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar y restaurar dependencias
COPY ["api_solsql.csproj", "./"]
RUN dotnet restore "./api_solsql.csproj"

# Copiar todo y compilar
COPY . .
WORKDIR "/src"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Imagen final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "api_solsql.dll"] 
