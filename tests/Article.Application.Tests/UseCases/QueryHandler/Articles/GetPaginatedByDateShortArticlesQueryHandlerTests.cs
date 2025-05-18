using Article.Application.Shared.Models.DTOs;
using Article.Application.UseCases.Query.Articles;
using Article.Application.UseCases.QueryHandler.Articles;
using Article.Domain.Abstractions.Repositories;
using AutoMapper;
using FluentAssertions;
using Moq;

namespace Article.Application.Tests.UseCases.QueryHandler.Articles;

public class GetPaginatedByDateShortArticlesQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetPaginatedByDateShortArticlesQueryHandler _handler;

    public GetPaginatedByDateShortArticlesQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetPaginatedByDateShortArticlesQueryHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WithoutCategories_ShouldCallGeneralRepositoryMethods()
    {
        // Arrange
        var query = new GetPaginatedByDateShortArticlesQuery
        {
            PageNo = 1,
            PageSize = 10,
            IsDocumentation = true
        };
        var articles = new List<Article.Domain.Entities.Article> { new() };
        var mappedDtos = new List<ShortArticleDTO> { new() };
        long totalCount = 15;

        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetPaginatedByDateWithoutBlocksArticlesAsync(
            query.PageNo, query.PageSize, query.IsDocumentation, It.IsAny<CancellationToken>()))
            .ReturnsAsync(articles);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.CountAsync(query.IsDocumentation, It.IsAny<CancellationToken>()))
            .ReturnsAsync(totalCount);
        _mapperMock.Setup(x => x.Map<IEnumerable<ShortArticleDTO>>(articles))
            .Returns(mappedDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().BeSameAs(mappedDtos);
        result.CurrentPage.Should().Be(query.PageNo);
        result.PageSize.Should().Be(query.PageSize);
        result.TotalPages.Should().Be(2); // 15 items / 10 per page = 2 pages

        _unitOfWorkMock.Verify(x => x.ArticleRepository.GetPaginatedByDateWithoutBlocksArticlesAsync(
            query.PageNo, query.PageSize, query.IsDocumentation, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.ArticleRepository.CountAsync(
            query.IsDocumentation, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithCategories_ShouldCallCategorySpecificRepositoryMethods()
    {
        // Arrange
        var categories = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var query = new GetPaginatedByDateShortArticlesQuery
        {
            PageNo = 2,
            PageSize = 5,
            Categories = categories,
            IsDocumentation = false
        };
        var articles = new List<Article.Domain.Entities.Article> { new(), new() };
        var mappedDtos = new List<ShortArticleDTO> { new(), new() };
        long totalCount = 12;

        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetPaginatedByDateWithoutBlocksArticlesAsync(
            query.PageNo, query.PageSize, categories, query.IsDocumentation, It.IsAny<CancellationToken>()))
            .ReturnsAsync(articles);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.CountByCategoryAsync(
            categories, query.IsDocumentation, It.IsAny<CancellationToken>()))
            .ReturnsAsync(totalCount);
        _mapperMock.Setup(x => x.Map<IEnumerable<ShortArticleDTO>>(articles))
            .Returns(mappedDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().BeSameAs(mappedDtos);
        result.CurrentPage.Should().Be(query.PageNo);
        result.PageSize.Should().Be(query.PageSize);
        result.TotalPages.Should().Be(3); // 12 items / 5 per page = 3 pages

        _unitOfWorkMock.Verify(x => x.ArticleRepository.GetPaginatedByDateWithoutBlocksArticlesAsync(
            query.PageNo, query.PageSize, categories, query.IsDocumentation, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.ArticleRepository.CountByCategoryAsync(
            categories, query.IsDocumentation, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithEmptyCategories_ShouldCallGeneralRepositoryMethods()
    {
        // Arrange
        var query = new GetPaginatedByDateShortArticlesQuery
        {
            PageNo = 1,
            PageSize = 10,
            Categories = new List<Guid>(),
            IsDocumentation = null
        };
        var articles = new List<Article.Domain.Entities.Article> { new() };
        var mappedDtos = new List<ShortArticleDTO> { new() };
        long totalCount = 1;

        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetPaginatedByDateWithoutBlocksArticlesAsync(
            query.PageNo, query.PageSize, query.IsDocumentation, It.IsAny<CancellationToken>()))
            .ReturnsAsync(articles);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.CountAsync(query.IsDocumentation, It.IsAny<CancellationToken>()))
            .ReturnsAsync(totalCount);
        _mapperMock.Setup(x => x.Map<IEnumerable<ShortArticleDTO>>(articles))
            .Returns(mappedDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().BeSameAs(mappedDtos);
        result.TotalPages.Should().Be(1);

        _unitOfWorkMock.Verify(x => x.ArticleRepository.GetPaginatedByDateWithoutBlocksArticlesAsync(
            query.PageNo, query.PageSize, query.IsDocumentation, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.ArticleRepository.CountAsync(
            query.IsDocumentation, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldCalculateTotalPagesCorrectly()
    {
        // Arrange
        var query = new GetPaginatedByDateShortArticlesQuery
        {
            PageNo = 1,
            PageSize = 3,
            IsDocumentation = true
        };
        var articles = new List<Article.Domain.Entities.Article> { new() };
        long totalCount = 10; // 10 items / 3 per page = 4 pages

        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetPaginatedByDateWithoutBlocksArticlesAsync(
            It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(articles);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.CountAsync(It.IsAny<bool?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(totalCount);
        _mapperMock.Setup(x => x.Map<IEnumerable<ShortArticleDTO>>(It.IsAny<IEnumerable<Article.Domain.Entities.Article>>()))
            .Returns(new List<ShortArticleDTO> { new() });

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.TotalPages.Should().Be(4); // 10/3 = 3.33 -> ceil to 4
    }

    [Fact]
    public async Task Handle_ShouldMapArticlesToShortArticleDTOs()
    {
        // Arrange
        var query = new GetPaginatedByDateShortArticlesQuery
        {
            PageNo = 1,
            PageSize = 10
        };
        var articles = new List<Article.Domain.Entities.Article> { new() };
        var expectedDtos = new List<ShortArticleDTO> { new() };

        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetPaginatedByDateWithoutBlocksArticlesAsync(
            It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(articles);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.CountAsync(It.IsAny<bool?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        _mapperMock.Setup(x => x.Map<IEnumerable<ShortArticleDTO>>(articles))
            .Returns(expectedDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().BeSameAs(expectedDtos);
        _mapperMock.Verify(x => x.Map<IEnumerable<ShortArticleDTO>>(articles), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNullIsDocumentation_ShouldPassNullToRepository()
    {
        // Arrange
        var query = new GetPaginatedByDateShortArticlesQuery
        {
            PageNo = 1,
            PageSize = 10,
            IsDocumentation = null
        };
        var articles = new List<Article.Domain.Entities.Article> { new() };

        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetPaginatedByDateWithoutBlocksArticlesAsync(
            query.PageNo, query.PageSize, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(articles);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.CountAsync(null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        _mapperMock.Setup(x => x.Map<IEnumerable<ShortArticleDTO>>(It.IsAny<IEnumerable<Article.Domain.Entities.Article>>()))
            .Returns(new List<ShortArticleDTO> { new() });

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(x => x.ArticleRepository.GetPaginatedByDateWithoutBlocksArticlesAsync(
            query.PageNo, query.PageSize, null, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.ArticleRepository.CountAsync(null, It.IsAny<CancellationToken>()), Times.Once);
    }
}