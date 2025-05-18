using Article.Application.Shared.Models.DTOs;
using Article.Application.UseCases.Query.Articles;
using Article.Application.UseCases.QueryHandler.Articles;
using Article.Domain.Abstractions.Repositories;
using AutoMapper;
using FluentAssertions;
using Moq;

namespace Article.Application.Tests.UseCases.QueryHandler.Articles;

public class GetPopularPaginatedShortArticlesQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetPopularPaginatedShortArticlesQueryHandler _handler;

    public GetPopularPaginatedShortArticlesQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetPopularPaginatedShortArticlesQueryHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WithoutCategories_ShouldCallGeneralRepositoryMethods()
    {
        // Arrange
        var query = new GetPopularPaginatedShortArticlesQuery
        {
            PageNo = 1,
            PageSize = 10,
            IsDocumentation = true
        };
        var articles = new List<Article.Domain.Entities.Article> { new() { VisitCount = 100 } };
        var mappedDtos = new List<ShortArticleDTO> { new() };
        long totalCount = 15;

        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetPopularPaginatedWithoutBlocksArticlesAsync(
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

        _unitOfWorkMock.Verify(x => x.ArticleRepository.GetPopularPaginatedWithoutBlocksArticlesAsync(
            query.PageNo, query.PageSize, query.IsDocumentation, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.ArticleRepository.CountAsync(
            query.IsDocumentation, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithCategories_ShouldCallCategorySpecificRepositoryMethods()
    {
        // Arrange
        var categories = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var query = new GetPopularPaginatedShortArticlesQuery
        {
            PageNo = 2,
            PageSize = 5,
            Categories = categories,
            IsDocumentation = false
        };
        var articles = new List<Article.Domain.Entities.Article>
        {
            new() { VisitCount = 150 },
            new() { VisitCount = 120 }
        };
        var mappedDtos = new List<ShortArticleDTO> { new(), new() };
        long totalCount = 12;

        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetPopularPaginatedWithoutBlocksArticlesAsync(
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

        _unitOfWorkMock.Verify(x => x.ArticleRepository.GetPopularPaginatedWithoutBlocksArticlesAsync(
            query.PageNo, query.PageSize, categories, query.IsDocumentation, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.ArticleRepository.CountByCategoryAsync(
            categories, query.IsDocumentation, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithEmptyCategories_ShouldCallGeneralRepositoryMethods()
    {
        // Arrange
        var query = new GetPopularPaginatedShortArticlesQuery
        {
            PageNo = 1,
            PageSize = 10,
            Categories = new List<Guid>(),
            IsDocumentation = null
        };
        var articles = new List<Article.Domain.Entities.Article> { new() { VisitCount = 80 } };
        var mappedDtos = new List<ShortArticleDTO> { new() };
        long totalCount = 1;

        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetPopularPaginatedWithoutBlocksArticlesAsync(
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

        _unitOfWorkMock.Verify(x => x.ArticleRepository.GetPopularPaginatedWithoutBlocksArticlesAsync(
            query.PageNo, query.PageSize, query.IsDocumentation, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.ArticleRepository.CountAsync(
            query.IsDocumentation, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnArticlesOrderedByPopularity()
    {
        // Arrange
        var query = new GetPopularPaginatedShortArticlesQuery
        {
            PageNo = 1,
            PageSize = 3
        };
        var articles = new List<Article.Domain.Entities.Article>
        {
            new() { VisitCount = 100 },
            new() { VisitCount = 200 },
            new() { VisitCount = 150 }
        };
        var mappedDtos = new List<ShortArticleDTO> { new(), new(), new() };

        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetPopularPaginatedWithoutBlocksArticlesAsync(
            It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(articles.OrderByDescending(a => a.VisitCount).ToList());
        _unitOfWorkMock.Setup(x => x.ArticleRepository.CountAsync(It.IsAny<bool?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(3);
        _mapperMock.Setup(x => x.Map<IEnumerable<ShortArticleDTO>>(It.IsAny<IEnumerable<Article.Domain.Entities.Article>>()))
            .Returns(mappedDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().HaveCount(3);
        // Verify the repository returns articles ordered by VisitCount descending
        _unitOfWorkMock.Verify(x => x.ArticleRepository.GetPopularPaginatedWithoutBlocksArticlesAsync(
            query.PageNo, query.PageSize, null, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNullIsDocumentation_ShouldPassNullToRepository()
    {
        // Arrange
        var query = new GetPopularPaginatedShortArticlesQuery
        {
            PageNo = 1,
            PageSize = 10,
            IsDocumentation = null
        };
        var articles = new List<Article.Domain.Entities.Article> { new() { VisitCount = 50 } };

        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetPopularPaginatedWithoutBlocksArticlesAsync(
            query.PageNo, query.PageSize, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(articles);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.CountAsync(null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        _mapperMock.Setup(x => x.Map<IEnumerable<ShortArticleDTO>>(It.IsAny<IEnumerable<Article.Domain.Entities.Article>>()))
            .Returns(new List<ShortArticleDTO> { new() });

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(x => x.ArticleRepository.GetPopularPaginatedWithoutBlocksArticlesAsync(
            query.PageNo, query.PageSize, null, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.ArticleRepository.CountAsync(null, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldCalculateTotalPagesCorrectlyWithRemainder()
    {
        // Arrange
        var query = new GetPopularPaginatedShortArticlesQuery
        {
            PageNo = 1,
            PageSize = 3,
            IsDocumentation = true
        };
        var articles = new List<Article.Domain.Entities.Article> { new() { VisitCount = 100 } };
        long totalCount = 10; // 10 items / 3 per page = 4 pages

        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetPopularPaginatedWithoutBlocksArticlesAsync(
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
}