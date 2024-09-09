FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src
COPY ["OperatorCode.Api/OperatorCode.Api.csproj", "OperatorCode.Api/"]
RUN dotnet restore "OperatorCode.Api/OperatorCode.Api.csproj"
COPY . .
WORKDIR "/src/OperatorCode.Api"
RUN dotnet build "OperatorCode.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "OperatorCode.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OperatorCode.Api.dll"]
