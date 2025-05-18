using Article.Application.Shared.Models.DTOs;
using Article.Application.UseCases.Query.Articles;
using Article.Application.UseCases.QueryHandler.Articles;
using Article.Domain.Abstractions.Repositories;
using Article.Domain.Entities;
using AutoMapper;
using Core.Exceptions;
using Core.Models;
using Core.Providers.Interfaces;
using FluentAssertions;
using Moq;

namespace Article.Application.Tests.UseCases.QueryHandler.Articles;

public class GetCurrentUserShortArticlesQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IBaseCurrentUserProvider> _currentUserProviderMock;
    private readonly GetCurrentUserShortArticlesQueryHandler _handler;

    public GetCurrentUserShortArticlesQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _currentUserProviderMock = new Mock<IBaseCurrentUserProvider>();
        _handler = new GetCurrentUserShortArticlesQueryHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _currentUserProviderMock.Object);
    }

    [Fact]
    public async Task Handle_WhenCurrentUserIsNull_ShouldThrowGuardForbiddenException()
    {
        // Arrange
        var query = new GetCurrentUserShortArticlesQuery { PageNo = 1, PageSize = 10 };
        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns((CurrentUser)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<GuardForbiddenException>()
            .WithMessage("Current user is null");
    }

    [Fact]
    public async Task Handle_WhenDbUserNotFound_ShouldThrowGuardNotFoundException()
    {
        // Arrange
        var query = new GetCurrentUserShortArticlesQuery { PageNo = 1, PageSize = 10 };
        var currentUser = new CurrentUser { Id = Guid.NewGuid() };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(currentUser.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<GuardNotFoundException>()
            .WithMessage("Db user is null");
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldReturnPaginatedResult()
    {
        // Arrange
        var query = new GetCurrentUserShortArticlesQuery { PageNo = 2, PageSize = 5 };
        var currentUser = new CurrentUser { Id = Guid.NewGuid() };
        var dbUser = new User
        {
            Id = currentUser.Id,
            Username = "testuser",
            PublicUsername = "public_testuser"
        };
        var articles = new List<Article.Domain.Entities.Article>
        {
            new() { Id = Guid.NewGuid(), AuthorId = currentUser.Id },
            new() { Id = Guid.NewGuid(), AuthorId = currentUser.Id }
        };
        var mappedDtos = new List<ShortArticleDTO>
        {
            new() { Id = articles[0].Id },
            new() { Id = articles[1].Id }
        };
        long totalCount = 12;

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(currentUser.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(dbUser);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetPaginatedByAuthorWithoutBlocksArticlesAsync(
            currentUser.Id, query.PageNo, query.PageSize, It.IsAny<CancellationToken>()))
            .ReturnsAsync(articles);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.CountByAuthorAsync(dbUser.Id, It.IsAny<CancellationToken>()))
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

        foreach (var item in result.Items)
        {
            item.AuthorUsername.Should().Be(dbUser.Username);
            item.AuthorPublicUsername.Should().Be(dbUser.PublicUsername);
        }
    }

    [Fact]
    public async Task Handle_ShouldCalculateCorrectTotalPages()
    {
        // Arrange
        var query = new GetCurrentUserShortArticlesQuery { PageNo = 1, PageSize = 3 };
        var currentUser = new CurrentUser { Id = Guid.NewGuid() };
        var dbUser = new User { Id = currentUser.Id };
        var articles = new List<Article.Domain.Entities.Article> { new() };
        long totalCount = 10; // 10 items total, 3 per page = 4 pages

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(currentUser.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(dbUser);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetPaginatedByAuthorWithoutBlocksArticlesAsync(
            It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(articles);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.CountByAuthorAsync(dbUser.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(totalCount);
        _mapperMock.Setup(x => x.Map<IEnumerable<ShortArticleDTO>>(It.IsAny<IEnumerable<Article.Domain.Entities.Article>>()))
            .Returns(new List<ShortArticleDTO>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.TotalPages.Should().Be(4); // 10 items / 3 per page = 4 pages
    }

    [Fact]
    public async Task Handle_ShouldSetAuthorInfoOnAllItems()
    {
        // Arrange
        var query = new GetCurrentUserShortArticlesQuery();
        var currentUser = new CurrentUser { Id = Guid.NewGuid() };
        var dbUser = new User
        {
            Id = currentUser.Id,
            Username = "test_username",
            PublicUsername = "test_public_username"
        };
        var articles = new List<Article.Domain.Entities.Article>
        {
            new() { Id = Guid.NewGuid() },
            new() { Id = Guid.NewGuid() }
        };
        var mappedDtos = articles.Select(a => new ShortArticleDTO { Id = a.Id }).ToList();

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(currentUser.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(dbUser);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetPaginatedByAuthorWithoutBlocksArticlesAsync(
            It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(articles);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.CountByAuthorAsync(dbUser.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(2);
        _mapperMock.Setup(x => x.Map<IEnumerable<ShortArticleDTO>>(articles))
            .Returns(mappedDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().OnlyContain(x =>
            x.AuthorUsername == dbUser.Username &&
            x.AuthorPublicUsername == dbUser.PublicUsername);
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryWithCorrectParameters()
    {
        // Arrange
        var query = new GetCurrentUserShortArticlesQuery { PageNo = 3, PageSize = 5 };
        var currentUser = new CurrentUser { Id = Guid.NewGuid() };
        var dbUser = new User { Id = currentUser.Id };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(currentUser.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(dbUser);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetPaginatedByAuthorWithoutBlocksArticlesAsync(
            It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Article.Domain.Entities.Article>());
        _unitOfWorkMock.Setup(x => x.ArticleRepository.CountByAuthorAsync(dbUser.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);
        _mapperMock.Setup(x => x.Map<IEnumerable<ShortArticleDTO>>(It.IsAny<IEnumerable<Article.Domain.Entities.Article>>()))
            .Returns(new List<ShortArticleDTO>());

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(x => x.ArticleRepository.GetPaginatedByAuthorWithoutBlocksArticlesAsync(
            currentUser.Id, query.PageNo, query.PageSize, It.IsAny<CancellationToken>()), Times.Once);
    }
}