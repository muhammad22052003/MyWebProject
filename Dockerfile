# ����������� ����������� ����� ASP.NET
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

# ����������
FROM build AS publish
RUN dotnet publish "./MyWebProject.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ����������� ����������� ����� MySQL 8.0
FROM mysql:8.3
# ���������� ������ ��� ������������ root
ENV MYSQL_ROOT_PASSWORD=root
# �������� ���� ������ � ������������
ENV MYSQL_DATABASE=mydatabase
ENV MYSQL_USER=user
ENV MYSQL_PASSWORD=userpassword
# �������� ��� ������ ������������� � ����� /docker-entrypoint-initdb.d
ADD DB_Setup.sql /docker-entrypoint-initdb.d
# �������� ���� ��� ����������� � MySQL
EXPOSE 3306

# �������� �����
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyWebProject.dll"]
