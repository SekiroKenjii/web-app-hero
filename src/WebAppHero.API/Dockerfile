FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80
EXPOSE 443

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /
COPY ["src/WebAppHero.API/WebAppHero.API.csproj", "src/WebAppHero.API/"]
COPY ["src/WebAppHero.Application/WebAppHero.Application.csproj", "src/WebAppHero.Application/"]
COPY ["src/WebAppHero.Contract/WebAppHero.Contract.csproj", "src/WebAppHero.Contract/"]
COPY ["src/WebAppHero.Domain/WebAppHero.Domain.csproj", "src/WebAppHero.Domain/"]
COPY ["src/WebAppHero.Infrastructure/WebAppHero.Infrastructure.csproj", "src/WebAppHero.Infrastructure/"]
COPY ["src/WebAppHero.Persistence/WebAppHero.Persistence.csproj", "src/WebAppHero.Persistence/"]
RUN dotnet restore "src/WebAppHero.API/WebAppHero.API.csproj"
COPY . .
WORKDIR "/src/WebAppHero.API"
RUN dotnet build "WebAppHero.API.csproj" -c "$BUILD_CONFIGURATION" -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "WebAppHero.API.csproj" -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebAppHero.API.dll"]
