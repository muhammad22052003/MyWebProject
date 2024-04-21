# Используйте официальный образ ASP.NET
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

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

# Используйте официальный образ MySQL 8.0
FROM mysql:8.3
# Установите пароль для пользователя root
ENV MYSQL_ROOT_PASSWORD=root
# Создайте базу данных и пользователя
ENV MYSQL_DATABASE=mydatabase
ENV MYSQL_USER=user
ENV MYSQL_PASSWORD=userpassword
# Добавьте ваш скрипт инициализации в папку /docker-entrypoint-initdb.d
ADD DB_Setup.sql /docker-entrypoint-initdb.d
# Откройте порт для подключения к MySQL
EXPOSE 3306

# Конечный образ
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyWebProject.dll"]
