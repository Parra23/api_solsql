FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copiar solo el proyecto csproj
COPY ["api_solsql/api_solsql.csproj", "api_solsql/"]
RUN dotnet restore "api_solsql/api_solsql.csproj"

COPY . .
WORKDIR "/src/api_solsql"

RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "api_solsql.dll"]
