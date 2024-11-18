FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ForumService/Forum.Presentation/Forum.Presentation.csproj", "src/ForumService/Forum.Presentation/"]
COPY ["src/ForumService/Forum.Application/Forum.Application.csproj", "src/ForumService/Forum.Application/"]
COPY ["src/ForumService/Forum.Domain/Forum.Domain.csproj", "src/ForumService/Forum.Domain/"]
COPY ["src/ForumService/Forum.Infrastructure/Forum.Infrastructure.csproj", "src/ForumService/Forum.Infrastructure/"]
RUN dotnet restore "./src/ForumService/Forum.Presentation/Forum.Presentation.csproj"
COPY . .
WORKDIR "/src/src/ForumService/Forum.Presentation"
RUN dotnet build "./Forum.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Forum.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Forum.Presentation.dll"]