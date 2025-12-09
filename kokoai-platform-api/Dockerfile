FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["kokoai-platform-api/kokoai-platform-api.csproj", "kokoai-platform-api/"]
RUN dotnet restore "kokoai-platform-api/kokoai-platform-api.csproj"
COPY . .
WORKDIR "/src/kokoai-platform-api"
RUN dotnet build "./kokoai-platform-api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./kokoai-platform-api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "kokoai-platform-api.dll"]
