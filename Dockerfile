# ������� ����� ASP.NET
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# ����� ��� ������
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MyWebProject.csproj", "."]
RUN dotnet restore "./MyWebProject.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "./MyWebProject.csproj" -c $BUILD_CONFIGURATION -o /app/build

# ����� MySQL
FROM mysql:latest
COPY ./itransition_task4.sql /docker-entrypoint-initdb.d/
ENV MYSQL_DATABASE=itransition_task4
ENV MYSQL_USER=root
ENV MYSQL_PASSWORD=root
ENV MYSQL_ROOT_PASSWORD=root

# ����������
FROM build AS publish
RUN dotnet publish "./MyWebProject.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# �������� �����
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyWebProject.dll"]
