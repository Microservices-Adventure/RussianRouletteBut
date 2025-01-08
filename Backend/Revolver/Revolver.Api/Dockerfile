FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8084

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Backend/Revolver/Revolver.Api/Revolver.Api.csproj", "Backend/Revolver/Revolver.Api/"]
COPY ["Backend/Revolver/Revolver.Domain/Revolver.Domain.csproj", "Backend/Revolver/Revolver.Domain/"]
RUN dotnet restore "Backend/Revolver/Revolver.Api/Revolver.Api.csproj"
COPY . .
WORKDIR "/src/Backend/Revolver/Revolver.Api"
RUN dotnet build "Revolver.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Revolver.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Revolver.Api.dll"]
