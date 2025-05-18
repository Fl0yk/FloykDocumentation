using Article.Application.UseCases.Comand.Articles;
using Article.Application.UseCases.ComandHandler.Articles;
using Article.Domain.Abstractions.Repositories;
using AutoMapper;
using Core.Exceptions;
using Core.Models;
using Core.Providers.Interfaces;
using FluentAssertions;
using Moq;

namespace Article.Application.Tests.UseCases.CommandHandler.Articles;

public class UpdateArticleCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IBaseCurrentUserProvider> _currentUserProviderMock;
    private readonly UpdateArticleCommandHandler _handler;

    public UpdateArticleCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _currentUserProviderMock = new Mock<IBaseCurrentUserProvider>();
        _handler = new UpdateArticleCommandHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _currentUserProviderMock.Object);
    }

    [Fact]
    public async Task Handle_WhenCurrentUserIsNull_ShouldThrowGuardForbiddenException()
    {
        // Arrange
        var command = new UpdateArticleCommand { Id = Guid.NewGuid() };
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
        var command = new UpdateArticleCommand { Id = Guid.NewGuid() };
        var currentUser = new CurrentUser { Id = Guid.NewGuid() };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticleByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Article.Domain.Entities.Article)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<GuardNotFoundException>()
            .WithMessage($"Article with id {command.Id} was not found");
    }

    [Fact]
    public async Task Handle_WhenUserIsNotAuthor_ShouldThrowGuardForbiddenException()
    {
        // Arrange
        var command = new UpdateArticleCommand { Id = Guid.NewGuid() };
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
            .WithMessage($"The user with id {currentUser.Id} is not author of this article");
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldUpdateArticle()
    {
        // Arrange
        var command = new UpdateArticleCommand
        {
            Id = Guid.NewGuid(),
            NewTitle = "Updated Title",
            NewShortDescription = "Updated Description"
        };

        var currentUser = new CurrentUser { Id = Guid.NewGuid() };
        var article = new Article.Domain.Entities.Article
        {
            Id = command.Id,
            AuthorId = currentUser.Id,
            Title = "Original Title",
            ShortDescription = "Original Description"
        };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticleByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);

        // Setup mapper to update the article
        _mapperMock.Setup(x => x.Map(command, article))
            .Callback<UpdateArticleCommand, Article.Domain.Entities.Article>((src, dest) =>
            {
                dest.Title = src.NewTitle;
                dest.ShortDescription = src.NewShortDescription;
            });

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        article.Title.Should().Be(command.NewTitle);
        article.ShortDescription.Should().Be(command.NewShortDescription);
        _unitOfWorkMock.Verify(x => x.ArticleRepository.UpdateArticleAsync(article, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldCallMapperWithCorrectParameters()
    {
        // Arrange
        var command = new UpdateArticleCommand
        {
            Id = Guid.NewGuid(),
            NewTitle = "New Title",
            NewShortDescription = "New Description"
        };

        var currentUser = new CurrentUser { Id = Guid.NewGuid() };
        var article = new Article.Domain.Entities.Article
        {
            Id = command.Id,
            AuthorId = currentUser.Id
        };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticleByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mapperMock.Verify(x => x.Map(command, article), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldNotChangeArticleId()
    {
        // Arrange
        var originalId = Guid.NewGuid();
        var command = new UpdateArticleCommand
        {
            Id = originalId,
            NewTitle = "Updated Title"
        };

        var currentUser = new CurrentUser { Id = Guid.NewGuid() };
        var article = new Article.Domain.Entities.Article
        {
            Id = originalId,
            AuthorId = currentUser.Id
        };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticleByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        article.Id.Should().Be(originalId);
    }
}