FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DevOpsCalculator/DevOpsCalculator.csproj", "DevOpsCalculator/"]
RUN dotnet restore "DevOpsCalculator/DevOpsCalculator.csproj"
COPY . .
WORKDIR "/src/DevOpsCalculator"
RUN dotnet build "DevOpsCalculator.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "DevOpsCalculator.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DevOpsCalculator.dll"]
