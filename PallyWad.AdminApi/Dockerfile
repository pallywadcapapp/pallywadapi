#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PallyWad.AdminApi/PallyWad.AdminApi.csproj", "PallyWad.AdminApi/"]
COPY ["PallyWad.ServiceDefaults/PallyWad.ServiceDefaults.csproj", "PallyWad.ServiceDefaults/"]
COPY ["PallyWad.Services/PallyWad.Services.csproj", "PallyWad.Services/"]
COPY ["PallyWad.Application/PallyWad.Application.csproj", "PallyWad.Application/"]
COPY ["PallyWad.Domain/PallyWad.Domain.csproj", "PallyWad.Domain/"]
COPY ["PallyWad.Infrastructure/PallyWad.Infrastructure.csproj", "PallyWad.Infrastructure/"]
RUN dotnet restore "./PallyWad.AdminApi/PallyWad.AdminApi.csproj"
COPY . .
WORKDIR "/src/PallyWad.AdminApi"
RUN dotnet build "./PallyWad.AdminApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PallyWad.AdminApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PallyWad.AdminApi.dll"]