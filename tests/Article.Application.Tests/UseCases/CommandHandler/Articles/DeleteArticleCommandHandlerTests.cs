using Article.Application.UseCases.Comand.Articles;
using Article.Application.UseCases.ComandHandler.Articles;
using Article.Domain.Abstractions.Repositories;
using Core.Exceptions;
using Core.Models;
using Core.Providers.Interfaces;
using FluentAssertions;
using Moq;

namespace Article.Application.Tests.UseCases.CommandHandler.Articles;

public class DeleteArticleCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IBaseCurrentUserProvider> _currentUserProviderMock;
    private readonly Mock<ITransactionProvider> _transactionProviderMock;
    private readonly DeleteArticleCommandHandler _handler;

    public DeleteArticleCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _currentUserProviderMock = new Mock<IBaseCurrentUserProvider>();
        _transactionProviderMock = new Mock<ITransactionProvider>();
        _handler = new DeleteArticleCommandHandler(
            _unitOfWorkMock.Object,
            _currentUserProviderMock.Object,
            _transactionProviderMock.Object);
    }

    [Fact]
    public async Task Handle_WhenCurrentUserIsNull_ShouldThrowGuardForbiddenException()
    {
        // Arrange
        var command = new DeleteArticleCommand { Id = Guid.NewGuid() };
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
        var command = new DeleteArticleCommand { Id = Guid.NewGuid() };
        var currentUser = new CurrentUser { Id = Guid.NewGuid() };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticleByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Article.Domain.Entities.Article)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<GuardNotFoundException>()
            .WithMessage($"Article with id {command.Id} was not found");

        _transactionProviderMock.Verify(x => x.OpenTransaction(It.IsAny<CancellationToken>()), Times.Once);
        _transactionProviderMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenUserIsNotAuthor_ShouldThrowGuardForbiddenException()
    {
        // Arrange
        var command = new DeleteArticleCommand { Id = Guid.NewGuid() };
        var currentUser = new CurrentUser { Id = Guid.NewGuid() };
        var article = new Article.Domain.Entities.Article
        {
            Id = command.Id,
            AuthorId = Guid.NewGuid() // Different from current user
        };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticleByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<GuardForbiddenException>()
            .WithMessage($"Author with id {currentUser.Id} cannot delete this article");

        _transactionProviderMock.Verify(x => x.OpenTransaction(It.IsAny<CancellationToken>()), Times.Once);
        _transactionProviderMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldDeleteArticleAndCommitTransaction()
    {
        // Arrange
        var command = new DeleteArticleCommand { Id = Guid.NewGuid() };
        var currentUser = new CurrentUser { Id = Guid.NewGuid() };
        var article = new Article.Domain.Entities.Article
        {
            Id = command.Id,
            AuthorId = currentUser.Id
        };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticleByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);
        _unitOfWorkMock.Setup(x => x.UserRepository.RemoveSavedArticlesByArticleAsync(article.Id, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(x => x.UserRepository.RemoveSavedArticlesByArticleAsync(article.Id, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.ArticleRepository.DeleteArticleAsync(article, It.IsAny<CancellationToken>()), Times.Once);

        _transactionProviderMock.Verify(x => x.OpenTransaction(It.IsAny<CancellationToken>()), Times.Once);
        _transactionProviderMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenExceptionOccurs_ShouldNotCommitTransaction()
    {
        // Arrange
        var command = new DeleteArticleCommand { Id = Guid.NewGuid() };
        var currentUser = new CurrentUser { Id = Guid.NewGuid() };
        var article = new Article.Domain.Entities.Article
        {
            Id = command.Id,
            AuthorId = currentUser.Id
        };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticleByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);
        _unitOfWorkMock.Setup(x => x.UserRepository.RemoveSavedArticlesByArticleAsync(article.Id, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Test exception");
        _transactionProviderMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Never);
    }
}