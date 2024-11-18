FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/IdentityService/Identity.Presentation/Identity.Presentation.csproj", "src/IdentityService/Identity.Presentation/"]
COPY ["src/IdentityService/Identity.Application/Identity.Application.csproj", "src/IdentityService/Identity.Application/"]
COPY ["src/IdentityService/Identity.DataAccess/Identity.DataAccess.csproj", "src/IdentityService/Identity.DataAccess/"]
RUN dotnet restore "./src/IdentityService/Identity.Presentation/Identity.Presentation.csproj"
COPY . .
WORKDIR "/src/src/IdentityService/Identity.Presentation"
RUN dotnet build "./Identity.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Identity.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.Presentation.dll"]