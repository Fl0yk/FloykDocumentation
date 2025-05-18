using Article.Application.Shared.Models.DTOs;
using Article.Application.UseCases.Query.Articles;
using Article.Application.UseCases.QueryHandler.Articles;
using Article.Domain.Abstractions.Repositories;
using AutoMapper;
using Core.Exceptions;
using Core.Models;
using Core.Providers.Interfaces;
using FluentAssertions;
using Moq;

namespace Article.Application.Tests.UseCases.QueryHandler.Articles;

public class GetArticleByIdQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IBaseCurrentUserProvider> _currentUserProviderMock;
    private readonly GetArticleByIdQueryHandler _handler;

    public GetArticleByIdQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _currentUserProviderMock = new Mock<IBaseCurrentUserProvider>();
        _handler = new GetArticleByIdQueryHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _currentUserProviderMock.Object);
    }

    [Fact]
    public async Task Handle_WhenArticleNotFound_ShouldThrowGuardNotFoundException()
    {
        // Arrange
        var query = new GetArticleByIdQuery { Id = Guid.NewGuid() };
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticleByIdAsync(query.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Article.Domain.Entities.Article)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<GuardNotFoundException>()
            .WithMessage($"Article with id {query.Id} was not fount");
    }

    [Fact]
    public async Task Handle_WhenCurrentUserIsNotAuthor_ShouldIncrementVisitCount()
    {
        // Arrange
        var query = new GetArticleByIdQuery { Id = Guid.NewGuid() };
        var article = new Article.Domain.Entities.Article
        {
            Id = query.Id,
            AuthorId = Guid.NewGuid(),
            VisitCount = 5
        };
        var currentUser = new CurrentUser { Id = Guid.NewGuid() }; // Different from author
        var articleDto = new ArticleDTO();

        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticleByIdAsync(query.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);
        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _mapperMock.Setup(x => x.Map<ArticleDTO>(article)).Returns(articleDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        article.VisitCount.Should().Be(6);
        _unitOfWorkMock.Verify(x => x.ArticleRepository.UpdateArticleAsync(article, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        result.Should().BeSameAs(articleDto);
    }

    [Fact]
    public async Task Handle_WhenCurrentUserIsAuthor_ShouldNotIncrementVisitCount()
    {
        // Arrange
        var query = new GetArticleByIdQuery { Id = Guid.NewGuid() };
        var currentUser = new CurrentUser { Id = Guid.NewGuid() };
        var article = new Article.Domain.Entities.Article
        {
            Id = query.Id,
            AuthorId = currentUser.Id,
            VisitCount = 5
        };
        var articleDto = new ArticleDTO();

        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticleByIdAsync(query.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);
        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _mapperMock.Setup(x => x.Map<ArticleDTO>(article)).Returns(articleDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        article.VisitCount.Should().Be(5);
        _unitOfWorkMock.Verify(x => x.ArticleRepository.UpdateArticleAsync(It.IsAny<Article.Domain.Entities.Article>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        result.Should().BeSameAs(articleDto);
    }

    [Fact]
    public async Task Handle_WhenNoCurrentUser_ShouldNotIncrementVisitCount()
    {
        // Arrange
        var query = new GetArticleByIdQuery { Id = Guid.NewGuid() };
        var article = new Article.Domain.Entities.Article
        {
            Id = query.Id,
            AuthorId = Guid.NewGuid(),
            VisitCount = 5
        };
        var articleDto = new ArticleDTO();

        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticleByIdAsync(query.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);
        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns((CurrentUser)null);
        _mapperMock.Setup(x => x.Map<ArticleDTO>(article)).Returns(articleDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        article.VisitCount.Should().Be(5);
        _unitOfWorkMock.Verify(x => x.ArticleRepository.UpdateArticleAsync(It.IsAny<Article.Domain.Entities.Article>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        result.Should().BeSameAs(articleDto);
    }

    [Fact]
    public async Task Handle_ShouldReturnMappedArticleDTO()
    {
        // Arrange
        var query = new GetArticleByIdQuery { Id = Guid.NewGuid() };
        var article = new Article.Domain.Entities.Article
        {
            Id = query.Id,
            AuthorId = Guid.NewGuid()
        };
        var expectedDto = new ArticleDTO { Id = article.Id };

        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticleByIdAsync(query.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);
        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns((CurrentUser)null);
        _mapperMock.Setup(x => x.Map<ArticleDTO>(article)).Returns(expectedDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeSameAs(expectedDto);
        _mapperMock.Verify(x => x.Map<ArticleDTO>(article), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenVisitCountIncremented_ShouldSaveChanges()
    {
        // Arrange
        var query = new GetArticleByIdQuery { Id = Guid.NewGuid() };
        var currentUser = new CurrentUser { Id = Guid.NewGuid() };
        var article = new Article.Domain.Entities.Article
        {
            Id = query.Id,
            AuthorId = Guid.NewGuid(), // Different from current user
            VisitCount = 10
        };
        var articleDto = new ArticleDTO();

        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticleByIdAsync(query.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);
        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _mapperMock.Setup(x => x.Map<ArticleDTO>(article)).Returns(articleDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        article.VisitCount.Should().Be(11);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}