FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["api_solsql/api_solsql.csproj", "api_solsql/"]
RUN dotnet restore "api_solsql/api_solsql.csproj"

COPY . .
WORKDIR "/src/api_solsql"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "api_solsql.dll"]
