using Article.Application.UseCases.Comand.Articles;
using Article.Application.UseCases.ComandHandler.Articles;
using Article.Domain.Abstractions.Repositories;
using Article.Domain.Entities;
using Core.Exceptions;
using Core.Models;
using Core.Providers.Interfaces;
using FluentAssertions;
using Moq;

namespace Article.Application.Tests.UseCases.CommandHandler.Articles;

public class SaveArticleCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IBaseCurrentUserProvider> _currentUserProviderMock;
    private readonly Mock<ITransactionProvider> _transactionProviderMock;
    private readonly SaveArticleCommandHandler _handler;

    public SaveArticleCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _currentUserProviderMock = new Mock<IBaseCurrentUserProvider>();
        _transactionProviderMock = new Mock<ITransactionProvider>();
        _handler = new SaveArticleCommandHandler(
            _unitOfWorkMock.Object,
            _currentUserProviderMock.Object,
            _transactionProviderMock.Object);
    }

    [Fact]
    public async Task Handle_WhenCurrentUserIsNull_ShouldThrowGuardForbiddenException()
    {
        // Arrange
        var command = new SaveArticleCommand { ArticleId = Guid.NewGuid() };
        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns((CurrentUser)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<GuardForbiddenException>()
            .WithMessage("Current user is null");

        _transactionProviderMock.Verify(x => x.OpenTransaction(It.IsAny<CancellationToken>()), Times.Once);
        _transactionProviderMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenArticleNotFound_ShouldThrowGuardNotFoundException()
    {
        // Arrange
        var command = new SaveArticleCommand { ArticleId = Guid.NewGuid() };
        var currentUser = new CurrentUser { Id = Guid.NewGuid() };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticleByIdAsync(command.ArticleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Article.Domain.Entities.Article)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<GuardNotFoundException>()
            .WithMessage($"Article with id {command.ArticleId} was not found");

        _transactionProviderMock.Verify(x => x.OpenTransaction(It.IsAny<CancellationToken>()), Times.Once);
        _transactionProviderMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenUserNotFound_ShouldThrowGuardNotFoundException()
    {
        // Arrange
        var command = new SaveArticleCommand { ArticleId = Guid.NewGuid() };
        var currentUser = new CurrentUser { Id = Guid.NewGuid() };
        var article = new Article.Domain.Entities.Article { Id = command.ArticleId };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticleByIdAsync(command.ArticleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);
        _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(currentUser.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<GuardNotFoundException>()
            .WithMessage($"User with id {currentUser.Id} was not found");

        _transactionProviderMock.Verify(x => x.OpenTransaction(It.IsAny<CancellationToken>()), Times.Once);
        _transactionProviderMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenArticleAlreadySaved_ShouldThrowGuardArgumentException()
    {
        // Arrange
        var command = new SaveArticleCommand { ArticleId = Guid.NewGuid() };
        var currentUser = new CurrentUser { Id = Guid.NewGuid() };
        var article = new Article.Domain.Entities.Article { Id = command.ArticleId };
        var user = new User
        {
            Id = currentUser.Id,
            SavedArticles = new List<SavedArticle> { new SavedArticle { ArticleId = command.ArticleId } }
        };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticleByIdAsync(command.ArticleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);
        _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(currentUser.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _unitOfWorkMock.Setup(x => x.UserRepository.WithSavedArticles(user, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<GuardArgumentException>()
            .WithMessage($"Article with id {article.Id} already saved. User id: {user.Id}");

        _transactionProviderMock.Verify(x => x.OpenTransaction(It.IsAny<CancellationToken>()), Times.Once);
        _transactionProviderMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldSaveArticleAndCommitTransaction()
    {
        // Arrange
        var command = new SaveArticleCommand { ArticleId = Guid.NewGuid() };
        var currentUser = new CurrentUser { Id = Guid.NewGuid() };
        var article = new Article.Domain.Entities.Article { Id = command.ArticleId };
        var user = new User
        {
            Id = currentUser.Id,
            SavedArticles = new List<SavedArticle>()
        };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticleByIdAsync(command.ArticleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);
        _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(currentUser.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _unitOfWorkMock.Setup(x => x.UserRepository.WithSavedArticles(user, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(x => x.UserRepository.SaveArticle(
            It.Is<SavedArticle>(sa =>
                sa.UserId == user.Id &&
                sa.ArticleId == article.Id),
            It.IsAny<CancellationToken>()),
            Times.Once);

        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _transactionProviderMock.Verify(x => x.OpenTransaction(It.IsAny<CancellationToken>()), Times.Once);
        _transactionProviderMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenExceptionOccurs_ShouldNotCommitTransaction()
    {
        // Arrange
        var command = new SaveArticleCommand { ArticleId = Guid.NewGuid() };
        var currentUser = new CurrentUser { Id = Guid.NewGuid() };
        var article = new Article.Domain.Entities.Article { Id = command.ArticleId };
        var user = new User
        {
            Id = currentUser.Id,
            SavedArticles = new List<SavedArticle>()
        };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticleByIdAsync(command.ArticleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);
        _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(currentUser.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _unitOfWorkMock.Setup(x => x.UserRepository.WithSavedArticles(user, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _unitOfWorkMock.Setup(x => x.UserRepository.SaveArticle(It.IsAny<SavedArticle>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Test exception");
        _transactionProviderMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Never);
    }
}