# Imagem base de runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Imagem de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY Vicporsy.ProjectAPI.csproj .
RUN dotnet restore "./Vicporsy.ProjectAPI.csproj"

COPY . .
RUN dotnet build "Vicporsy.ProjectAPI.csproj" -c Release

# Publicação
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Vicporsy.ProjectAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Vicporsy.ProjectAPI.dll"]