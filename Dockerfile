# Базовый образ ASP.NET
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
EXPOSE 3306
EXPOSE 3307
EXPOSE 33060

# Образ для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MyWebProject.csproj", "."]
RUN dotnet restore "./MyWebProject.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "./MyWebProject.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Публикация
FROM build AS publish
RUN dotnet publish "./MyWebProject.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Конечный образ
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyWebProject.dll"]
