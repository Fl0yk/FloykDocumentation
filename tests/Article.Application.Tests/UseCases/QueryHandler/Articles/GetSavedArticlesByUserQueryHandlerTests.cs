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

public class GetSavedArticlesByUserQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IBaseCurrentUserProvider> _currentUserProviderMock;
    private readonly GetSavedArticlesByUserQueryHandler _handler;

    public GetSavedArticlesByUserQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _currentUserProviderMock = new Mock<IBaseCurrentUserProvider>();
        _handler = new GetSavedArticlesByUserQueryHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _currentUserProviderMock.Object);
    }

    [Fact]
    public async Task Handle_WhenCurrentUserIsNull_ShouldThrowGuardForbiddenException()
    {
        // Arrange
        var query = new GetSavedArticlesByUserQuery();
        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns((CurrentUser)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<GuardForbiddenException>()
            .WithMessage("Current user is null");
    }

    [Fact]
    public async Task Handle_WhenUserNotFound_ShouldThrowGuardNotFoundException()
    {
        // Arrange
        var query = new GetSavedArticlesByUserQuery();
        var currentUser = new CurrentUser { Id = Guid.NewGuid() };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(currentUser.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<GuardNotFoundException>()
            .WithMessage($"User with id {currentUser.Id} was not found");
    }

    [Fact]
    public async Task Handle_WhenHasSavedArticles_ShouldReturnMappedArticles()
    {
        // Arrange
        var query = new GetSavedArticlesByUserQuery();
        var currentUser = new CurrentUser { Id = Guid.NewGuid() };
        var savedArticleIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var dbUser = new User
        {
            Id = currentUser.Id,
            SavedArticles = savedArticleIds.Select(id => new SavedArticle { ArticleId = id }).ToList()
        };
        var articles = savedArticleIds.Select(id => new Article.Domain.Entities.Article { Id = id }).ToList();
        var expectedDtos = articles.Select(a => new ShortArticleDTO { Id = a.Id }).ToList();

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(currentUser.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(dbUser);
        _unitOfWorkMock.Setup(x => x.UserRepository.WithSavedArticles(dbUser, It.IsAny<CancellationToken>()))
            .ReturnsAsync(dbUser);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticlesByIdsAsync(
            savedArticleIds, It.IsAny<CancellationToken>()))
            .ReturnsAsync(articles);
        _mapperMock.Setup(x => x.Map<IEnumerable<ShortArticleDTO>>(articles))
            .Returns(expectedDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedDtos);
        _unitOfWorkMock.Verify(x => x.ArticleRepository.GetArticlesByIdsAsync(
            savedArticleIds, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldOnlyReturnArticlesThatExist()
    {
        // Arrange
        var query = new GetSavedArticlesByUserQuery();
        var currentUser = new CurrentUser { Id = Guid.NewGuid() };
        var savedArticleIds = new List<Guid>
        {
            Guid.NewGuid(), // Exists
            Guid.NewGuid()  // Doesn't exist
        };
        var dbUser = new User
        {
            Id = currentUser.Id,
            SavedArticles = savedArticleIds.Select(id => new SavedArticle { ArticleId = id }).ToList()
        };
        var existingArticle = new Article.Domain.Entities.Article { Id = savedArticleIds[0] };
        var expectedDtos = new List<ShortArticleDTO> { new() { Id = existingArticle.Id } };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(currentUser.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(dbUser);
        _unitOfWorkMock.Setup(x => x.UserRepository.WithSavedArticles(dbUser, It.IsAny<CancellationToken>()))
            .ReturnsAsync(dbUser);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticlesByIdsAsync(
            It.IsAny<IEnumerable<Guid>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Article.Domain.Entities.Article> { existingArticle });
        _mapperMock.Setup(x => x.Map<IEnumerable<ShortArticleDTO>>(It.Is<IEnumerable<Article.Domain.Entities.Article>>(a => a.Single().Id == existingArticle.Id)))
            .Returns(expectedDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        result.Should().BeEquivalentTo(expectedDtos);
    }
}