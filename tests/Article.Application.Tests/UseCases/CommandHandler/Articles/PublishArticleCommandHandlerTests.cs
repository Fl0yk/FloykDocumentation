using Article.Application.UseCases.Comand.Articles;
using Article.Application.UseCases.ComandHandler.Articles;
using Article.Domain.Abstractions.Repositories;
using Core.Exceptions;
using Core.Models;
using Core.Providers.Interfaces;
using FluentAssertions;
using Moq;

namespace Article.Application.Tests.UseCases.CommandHandler.Articles;

public class PublishArticleCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IBaseCurrentUserProvider> _currentUserProviderMock;
    private readonly PublishArticleCommandHandler _handler;

    public PublishArticleCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _currentUserProviderMock = new Mock<IBaseCurrentUserProvider>();
        _handler = new PublishArticleCommandHandler(
            _unitOfWorkMock.Object,
            _currentUserProviderMock.Object);
    }

    [Fact]
    public async Task Handle_WhenCurrentUserIsNull_ShouldThrowGuardForbiddenException()
    {
        // Arrange
        var command = new PublishArticleCommand { ArticleId = Guid.NewGuid() };
        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns((CurrentUser)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<GuardForbiddenException>()
            .WithMessage("Current user is null");
    }

    [Fact]
    public async Task Handle_WhenArticleNotFound_ShouldThrowGuardNotFoundException()
    {
        // Arrange
        var command = new PublishArticleCommand { ArticleId = Guid.NewGuid() };
        var currentUser = new CurrentUser { Id = Guid.NewGuid(), Roles = new[] { "Author" } };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticleByIdAsync(command.ArticleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Article.Domain.Entities.Article)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<GuardNotFoundException>()
            .WithMessage($"Article with id {command.ArticleId} was not found");
    }

    [Fact]
    public async Task Handle_WhenArticleAlreadyPublished_ShouldThrowGuardArgumentException()
    {
        // Arrange
        var command = new PublishArticleCommand { ArticleId = Guid.NewGuid() };
        var currentUser = new CurrentUser { Id = Guid.NewGuid(), Roles = new[] { "Author" } };
        var article = new Article.Domain.Entities.Article
        {
            Id = command.ArticleId,
            AuthorId = currentUser.Id,
            IsPublished = true
        };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticleByIdAsync(command.ArticleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<GuardArgumentException>()
            .WithMessage("This article has already been published");
    }

    [Fact]
    public async Task Handle_WhenUserIsNotAuthor_ShouldThrowGuardForbiddenException()
    {
        // Arrange
        var command = new PublishArticleCommand { ArticleId = Guid.NewGuid() };
        var currentUser = new CurrentUser { Id = Guid.NewGuid(), Roles = new[] { "Author" } };
        var article = new Article.Domain.Entities.Article
        {
            Id = command.ArticleId,
            AuthorId = Guid.NewGuid(), // Different author
            IsPublished = false
        };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticleByIdAsync(command.ArticleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<GuardForbiddenException>()
            .WithMessage($"The user with id {currentUser.Id} is not author of this article");
    }

    [Fact]
    public async Task Handle_WhenAdminPublishes_ShouldSetPublishedAndDocumentationFlags()
    {
        // Arrange
        var command = new PublishArticleCommand { ArticleId = Guid.NewGuid() };
        var currentUser = new CurrentUser { Id = Guid.NewGuid(), Roles = new[] { "Admin" } };
        var article = new Article.Domain.Entities.Article
        {
            Id = command.ArticleId,
            AuthorId = currentUser.Id,
            IsPublished = false,
            IsDocumentation = false
        };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticleByIdAsync(command.ArticleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        article.IsPublished.Should().BeTrue();
        article.IsDocumentation.Should().BeTrue();
        article.DateOfPublication.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
        _unitOfWorkMock.Verify(x => x.ArticleRepository.UpdateArticleAsync(article, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenAuthorPublishes_ShouldSetPublishedFlagOnly()
    {
        // Arrange
        var command = new PublishArticleCommand { ArticleId = Guid.NewGuid() };
        var currentUser = new CurrentUser { Id = Guid.NewGuid(), Roles = new[] { "Author" } };
        var article = new Article.Domain.Entities.Article
        {
            Id = command.ArticleId,
            AuthorId = currentUser.Id,
            IsPublished = false,
            IsDocumentation = false
        };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticleByIdAsync(command.ArticleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        article.IsPublished.Should().BeTrue();
        article.IsDocumentation.Should().BeFalse();
        article.DateOfPublication.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
        _unitOfWorkMock.Verify(x => x.ArticleRepository.UpdateArticleAsync(article, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRegularUserPublishes_ShouldSetApprovalFlag()
    {
        // Arrange
        var command = new PublishArticleCommand { ArticleId = Guid.NewGuid() };
        var currentUser = new CurrentUser { Id = Guid.NewGuid(), Roles = Array.Empty<string>() };
        var article = new Article.Domain.Entities.Article
        {
            Id = command.ArticleId,
            AuthorId = currentUser.Id,
            IsPublished = false,
            IsShouldBeApproved = false
        };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticleByIdAsync(command.ArticleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        article.IsPublished.Should().BeFalse();
        article.IsShouldBeApproved.Should().BeTrue();
        article.DateOfPublication.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
        _unitOfWorkMock.Verify(x => x.ArticleRepository.UpdateArticleAsync(article, It.IsAny<CancellationToken>()), Times.Once);
    }
}