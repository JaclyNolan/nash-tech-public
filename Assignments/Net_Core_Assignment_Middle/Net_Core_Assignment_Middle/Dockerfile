#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Net_Core_Assignment_Middle/Net_Core_Assignment_Middle.csproj", "Net_Core_Assignment_Middle/"]
RUN dotnet restore "./Net_Core_Assignment_Middle/Net_Core_Assignment_Middle.csproj"
COPY . .
WORKDIR "/src/Net_Core_Assignment_Middle"
RUN dotnet build "./Net_Core_Assignment_Middle.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Net_Core_Assignment_Middle.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Net_Core_Assignment_Middle.dll"]