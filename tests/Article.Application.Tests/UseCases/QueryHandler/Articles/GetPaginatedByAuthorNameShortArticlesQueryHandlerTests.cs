using Article.Application.Shared.Models.DTOs;
using Article.Application.UseCases.Query.Articles;
using Article.Application.UseCases.QueryHandler.Articles;
using Article.Domain.Abstractions.Repositories;
using Article.Domain.Entities;
using AutoMapper;
using Core.Exceptions;
using FluentAssertions;
using Moq;

namespace Article.Application.Tests.UseCases.QueryHandler.Articles;

public class GetPaginatedByAuthorNameShortArticlesQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetPaginatedByAuthorNameShortArticlesQueryHandler _handler;

    public GetPaginatedByAuthorNameShortArticlesQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetPaginatedByAuthorNameShortArticlesQueryHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WhenAuthorNotFound_ShouldThrowGuardNotFoundException()
    {
        // Arrange
        var query = new GetPaginatedByAuthorNameShortArticlesQuery
        {
            AuthorId = Guid.NewGuid(),
            PageNo = 1,
            PageSize = 10
        };

        _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(query.AuthorId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<GuardNotFoundException>()
            .WithMessage($"Author with id {query.AuthorId} not found");
    }

    [Fact]
    public async Task Handle_WhenEmptyPage_ShouldThrowGuardArgumentException()
    {
        // Arrange
        var query = new GetPaginatedByAuthorNameShortArticlesQuery
        {
            AuthorId = Guid.NewGuid(),
            PageNo = 2,
            PageSize = 10
        };
        var author = new User { Id = query.AuthorId };
        var emptyArticles = Enumerable.Empty<Article.Domain.Entities.Article>();

        _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(query.AuthorId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(author);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetPaginatedByAuthorWithoutBlocksArticlesAsync(
            query.AuthorId, query.PageNo, query.PageSize, It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyArticles);

        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<GuardArgumentException>()
            .WithMessage("Get an empty articles page");
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldReturnPaginatedResult()
    {
        // Arrange
        var query = new GetPaginatedByAuthorNameShortArticlesQuery
        {
            AuthorId = Guid.NewGuid(),
            PageNo = 1,
            PageSize = 5
        };
        var author = new User { Id = query.AuthorId };
        var articles = new List<Article.Domain.Entities.Article>
        {
            new() { Id = Guid.NewGuid(), AuthorId = author.Id },
            new() { Id = Guid.NewGuid(), AuthorId = author.Id }
        };
        var mappedDtos = new List<ShortArticleDTO>
        {
            new() { Id = articles[0].Id },
            new() { Id = articles[1].Id }
        };
        long totalCount = 12;

        _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(query.AuthorId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(author);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetPaginatedByAuthorWithoutBlocksArticlesAsync(
            query.AuthorId, query.PageNo, query.PageSize, It.IsAny<CancellationToken>()))
            .ReturnsAsync(articles);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.CountByAuthorAsync(author.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(totalCount);
        _mapperMock.Setup(x => x.Map<IEnumerable<ShortArticleDTO>>(articles))
            .Returns(mappedDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.CurrentPage.Should().Be(query.PageNo);
        result.PageSize.Should().Be(query.PageSize);
        result.TotalPages.Should().Be((int)Math.Ceiling((double)totalCount / query.PageSize));
    }

    [Fact]
    public async Task Handle_ShouldCalculateCorrectTotalPages()
    {
        // Arrange
        var query = new GetPaginatedByAuthorNameShortArticlesQuery
        {
            AuthorId = Guid.NewGuid(),
            PageNo = 1,
            PageSize = 3
        };
        var author = new User { Id = query.AuthorId };
        var articles = new List<Article.Domain.Entities.Article> { new() };
        long totalCount = 10; // 10 items total, 3 per page = 4 pages

        _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(query.AuthorId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(author);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetPaginatedByAuthorWithoutBlocksArticlesAsync(
            It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(articles);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.CountByAuthorAsync(author.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(totalCount);
        _mapperMock.Setup(x => x.Map<IEnumerable<ShortArticleDTO>>(It.IsAny<IEnumerable<Article.Domain.Entities.Article>>()))
            .Returns(new List<ShortArticleDTO>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.TotalPages.Should().Be(4); // 10 items / 3 per page = 4 pages
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryWithCorrectParameters()
    {
        // Arrange
        var query = new GetPaginatedByAuthorNameShortArticlesQuery
        {
            AuthorId = Guid.NewGuid(),
            PageNo = 3,
            PageSize = 5
        };
        var author = new User { Id = query.AuthorId };
        var articles = new List<Article.Domain.Entities.Article> { new() };

        _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(query.AuthorId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(author);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetPaginatedByAuthorWithoutBlocksArticlesAsync(
            It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(articles);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.CountByAuthorAsync(author.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        _mapperMock.Setup(x => x.Map<IEnumerable<ShortArticleDTO>>(It.IsAny<IEnumerable<Article.Domain.Entities.Article>>()))
            .Returns(new List<ShortArticleDTO> { new() });

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(x => x.ArticleRepository.GetPaginatedByAuthorWithoutBlocksArticlesAsync(
            query.AuthorId, query.PageNo, query.PageSize, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldMapArticlesToShortArticleDTOs()
    {
        // Arrange
        var query = new GetPaginatedByAuthorNameShortArticlesQuery
        {
            AuthorId = Guid.NewGuid(),
            PageNo = 1,
            PageSize = 10
        };
        var author = new User { Id = query.AuthorId };
        var articles = new List<Article.Domain.Entities.Article> { new() };
        var expectedDtos = new List<ShortArticleDTO> { new() };

        _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(query.AuthorId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(author);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetPaginatedByAuthorWithoutBlocksArticlesAsync(
            It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(articles);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.CountByAuthorAsync(author.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        _mapperMock.Setup(x => x.Map<IEnumerable<ShortArticleDTO>>(articles))
            .Returns(expectedDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().BeSameAs(expectedDtos);
        _mapperMock.Verify(x => x.Map<IEnumerable<ShortArticleDTO>>(articles), Times.Once);
    }
}