FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ArticleService/Article.Presentation/Article.Presentation.csproj", "src/ArticleService/Article.Presentation/"]
COPY ["src/ArticleService/Article.Application/Article.Application.csproj", "src/ArticleService/Article.Application/"]
COPY ["src/ArticleService/Article.Domain/Article.Domain.csproj", "src/ArticleService/Article.Domain/"]
COPY ["src/ArticleService/Article.Infrastructure/Article.Infrastructure.csproj", "src/ArticleService/Article.Infrastructure/"]
RUN dotnet restore "./src/ArticleService/Article.Presentation/Article.Presentation.csproj"
COPY . .
WORKDIR "/src/src/ArticleService/Article.Presentation"
RUN dotnet build "./Article.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Article.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Article.Presentation.dll"]