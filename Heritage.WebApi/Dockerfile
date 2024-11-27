# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Heritage.WebApi/Heritage.WebApi.csproj", "Heritage.WebApi/"]
COPY ["Heritage.Persistance/Heritage.Persistance.csproj", "Heritage.Persistance/"]
COPY ["Heritage.Application/Heritage.Application.csproj", "Heritage.Application/"]
COPY ["Heritage.Domain/Heritage.Domain.csproj", "Heritage.Domain/"]
RUN dotnet restore "./Heritage.WebApi/Heritage.WebApi.csproj"
COPY . .
WORKDIR "/src/Heritage.WebApi"
RUN dotnet build "./Heritage.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Heritage.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
ENV SECRET=HELLO
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Heritage.WebApi.dll"]
