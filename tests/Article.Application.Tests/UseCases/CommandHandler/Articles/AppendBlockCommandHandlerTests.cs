using Article.Application.UseCases.Comand.Articles;
using Article.Application.UseCases.ComandHandler.Articles;
using Article.Domain.Abstractions.Repositories;
using Article.Domain.Entities;
using AutoMapper;
using Core.Exceptions;
using Core.Models;
using Core.Providers.Interfaces;
using FluentAssertions;
using Moq;

namespace Article.Application.Tests.UseCases.CommandHandler.Articles;

public class AppendBlockCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IBaseCurrentUserProvider> _currentUserProviderMock;
    private readonly AppendBlockCommandHandler _handler;

    public AppendBlockCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _currentUserProviderMock = new Mock<IBaseCurrentUserProvider>();
        _handler = new AppendBlockCommandHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _currentUserProviderMock.Object);
    }

    [Fact]
    public async Task Handle_WhenCurrentUserIsNull_ShouldThrowGuardForbiddenException()
    {
        // Arrange
        var command = new AppendBlockCommand { ArticleId = Guid.NewGuid() };
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
        var command = new AppendBlockCommand { ArticleId = Guid.NewGuid() };
        var currentUser = new CurrentUser { Id = Guid.NewGuid() };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticleByIdAsync(command.ArticleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Article)null);

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
        var command = new AppendBlockCommand { ArticleId = Guid.NewGuid() };
        var currentUser = new CurrentUser { Id = Guid.NewGuid() };
        var article = new Domain.Entities.Article { AuthorId = Guid.NewGuid() };
        var author = new User { Id = article.AuthorId };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticleByIdAsync(command.ArticleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);
        _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(article.AuthorId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(author);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<GuardForbiddenException>()
            .WithMessage($"The user {currentUser.Id} is not author of this article");
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldUpdateArticleWithBlocks()
    {
        // Arrange
        var command = new AppendBlockCommand
        {
            ArticleId = Guid.NewGuid(),
            Blocks = new List<BlockInfo>
            {
                new BlockInfo { Id = Guid.NewGuid(), BlockType = BlockType.Text, Data = "Test data" }
            }
        };

        var currentUser = new CurrentUser { Id = Guid.NewGuid() };
        var article = new Domain.Entities.Article { AuthorId = currentUser.Id };
        var author = new User { Id = currentUser.Id };
        var blocks = new List<Block> { new Block() };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticleByIdAsync(command.ArticleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);
        _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(article.AuthorId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(author);
        _mapperMock.Setup(x => x.Map<ICollection<Block>>(command.Blocks)).Returns(blocks);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        article.Blocks.Should().BeEquivalentTo(blocks);
        _unitOfWorkMock.Verify(x => x.ArticleRepository.UpdateArticleAsync(article, It.IsAny<CancellationToken>()), Times.Once);
    }
}