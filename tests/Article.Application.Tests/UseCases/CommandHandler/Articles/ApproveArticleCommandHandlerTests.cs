using Article.Application.UseCases.Comand.Articles;
using Article.Application.UseCases.ComandHandler.Articles;
using Article.Domain.Abstractions.Repositories;
using Article.Domain.Entities;
using Core.Exceptions;
using Core.Models.Events;
using Core.Providers.Interfaces;
using FluentAssertions;
using MassTransit;
using Moq;

namespace Article.Application.Tests.UseCases.CommandHandler.Articles;

public class ApproveArticleCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IPublishEndpoint> _publishEndpointMock;
    private readonly Mock<ITransactionProvider> _transactionProviderMock;
    private readonly ApproveArticleCommandHandler _handler;

    public ApproveArticleCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _publishEndpointMock = new Mock<IPublishEndpoint>();
        _transactionProviderMock = new Mock<ITransactionProvider>();
        _handler = new ApproveArticleCommandHandler(
            _unitOfWorkMock.Object,
            _publishEndpointMock.Object,
            _transactionProviderMock.Object);
    }

    [Fact]
    public async Task Handle_WhenArticleNotFound_ShouldThrowGuardNotFoundException()
    {
        // Arrange
        var command = new ApproveArticleCommand { ArticleId = Guid.NewGuid() };
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticleByIdAsync(command.ArticleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Article)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<GuardNotFoundException>()
            .WithMessage($"Article by id '{command.ArticleId}' not found");

        _transactionProviderMock.Verify(x => x.OpenTransaction(It.IsAny<CancellationToken>()), Times.Once);
        _transactionProviderMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenAuthorNotFound_ShouldThrowGuardNotFoundException()
    {
        // Arrange
        var command = new ApproveArticleCommand { ArticleId = Guid.NewGuid() };
        var article = new Domain.Entities.Article { AuthorId = Guid.NewGuid(), IsShouldBeApproved = true };

        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticleByIdAsync(command.ArticleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);
        _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(article.AuthorId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<GuardNotFoundException>()
            .WithMessage($"User with id {article.AuthorId} not found");

        _transactionProviderMock.Verify(x => x.OpenTransaction(It.IsAny<CancellationToken>()), Times.Once);
        _transactionProviderMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenArticleShouldNotBeApproved_ShouldThrowGuardArgumentException()
    {
        // Arrange
        var command = new ApproveArticleCommand { ArticleId = Guid.NewGuid() };
        var article = new Domain.Entities.Article
        {
            AuthorId = Guid.NewGuid(),
            IsShouldBeApproved = false,
            Id = command.ArticleId
        };
        var author = new User { Id = article.AuthorId };

        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticleByIdAsync(command.ArticleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);
        _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(article.AuthorId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(author);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<GuardArgumentException>()
            .WithMessage($"Article with id {article.Id} should not be approved");

        _transactionProviderMock.Verify(x => x.OpenTransaction(It.IsAny<CancellationToken>()), Times.Once);
        _transactionProviderMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldApproveArticleAndPublishEvent()
    {
        // Arrange
        var command = new ApproveArticleCommand { ArticleId = Guid.NewGuid() };
        var article = new Domain.Entities.Article
        {
            AuthorId = Guid.NewGuid(),
            IsShouldBeApproved = true,
            Title = "Test Article"
        };
        var author = new User { Id = article.AuthorId };

        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticleByIdAsync(command.ArticleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);
        _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(article.AuthorId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(author);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        article.IsShouldBeApproved.Should().BeFalse();

        _unitOfWorkMock.Verify(x => x.ArticleRepository.UpdateArticleAsync(article, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        _publishEndpointMock.Verify(x => x.Publish(It.Is<ArticleApprovedEvent>(e =>
            e.UserId == author.Id &&
            e.Title == article.Title),
            It.IsAny<CancellationToken>()),
            Times.Once);

        _transactionProviderMock.Verify(x => x.OpenTransaction(It.IsAny<CancellationToken>()), Times.Once);
        _transactionProviderMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }
}