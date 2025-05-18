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

public class DeleteBlockCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IBaseCurrentUserProvider> _currentUserProviderMock;
    private readonly DeleteBlockCommandHandler _handler;

    public DeleteBlockCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _currentUserProviderMock = new Mock<IBaseCurrentUserProvider>();
        _handler = new DeleteBlockCommandHandler(
            _unitOfWorkMock.Object,
            _currentUserProviderMock.Object);
    }

    [Fact]
    public async Task Handle_WhenCurrentUserIsNull_ShouldThrowGuardForbiddenException()
    {
        // Arrange
        var command = new DeleteBlockCommand();
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
        var command = new DeleteBlockCommand { ArticleId = Guid.NewGuid() };
        var currentUser = new CurrentUser { Id = Guid.NewGuid() };

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
    public async Task Handle_WhenUserIsNotAuthor_ShouldThrowGuardForbiddenException()
    {
        // Arrange
        var command = new DeleteBlockCommand { ArticleId = Guid.NewGuid() };
        var currentUser = new CurrentUser { Id = Guid.NewGuid() };
        var article = new Article.Domain.Entities.Article
        {
            Id = command.ArticleId,
            AuthorId = Guid.NewGuid(), // Different from current user
            Title = "Test Article",
            Blocks = new List<Block>()
        };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticleByIdAsync(command.ArticleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<GuardForbiddenException>()
            .WithMessage($"Author with id {currentUser.Id} cannot delete block from this article");
    }

    [Fact]
    public async Task Handle_WhenBlockNotFound_ShouldThrowGuardNotFoundException()
    {
        // Arrange
        var command = new DeleteBlockCommand
        {
            ArticleId = Guid.NewGuid(),
            BlockId = Guid.NewGuid()
        };
        var currentUser = new CurrentUser { Id = Guid.NewGuid() };
        var article = new Article.Domain.Entities.Article
        {
            Id = command.ArticleId,
            AuthorId = currentUser.Id,
            Title = "Test Article",
            Blocks = new List<Block>() // Empty list
        };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticleByIdAsync(command.ArticleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<GuardNotFoundException>()
            .WithMessage($"Article \"{article.Title}\" does not have a block with id {command.BlockId}");
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldRemoveBlockAndUpdateArticle()
    {
        // Arrange
        var blockId = Guid.NewGuid();
        var command = new DeleteBlockCommand
        {
            ArticleId = Guid.NewGuid(),
            BlockId = blockId
        };
        var currentUser = new CurrentUser { Id = Guid.NewGuid() };
        var blockToRemove = new Block { Id = blockId };
        var article = new Article.Domain.Entities.Article
        {
            Id = command.ArticleId,
            AuthorId = currentUser.Id,
            Title = "Test Article",
            Blocks = new List<Block> { blockToRemove }
        };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticleByIdAsync(command.ArticleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        article.Blocks.Should().NotContain(blockToRemove);
        _unitOfWorkMock.Verify(x => x.ArticleRepository.UpdateArticleAsync(article, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenBlockRemoved_ShouldNotContainTheBlockAnymore()
    {
        // Arrange
        var blockId = Guid.NewGuid();
        var command = new DeleteBlockCommand
        {
            ArticleId = Guid.NewGuid(),
            BlockId = blockId
        };
        var currentUser = new CurrentUser { Id = Guid.NewGuid() };
        var blockToRemove = new Block { Id = blockId };
        var article = new Article.Domain.Entities.Article
        {
            Id = command.ArticleId,
            AuthorId = currentUser.Id,
            Blocks = new List<Block> { blockToRemove, new Block { Id = Guid.NewGuid() } }
        };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticleByIdAsync(command.ArticleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        article.Blocks.Should().HaveCount(1);
        article.Blocks.Should().NotContain(b => b.Id == blockId);
    }
}