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
using ArticleModel = Article.Domain.Entities.Article;

namespace Article.Application.Tests.UseCases.CommandHandler.Articles;

public class CreateArticleCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IBaseCurrentUserProvider> _currentUserProviderMock;
    private readonly CreateArticleCommandHandler _handler;

    public CreateArticleCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _currentUserProviderMock = new Mock<IBaseCurrentUserProvider>();
        _handler = new CreateArticleCommandHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _currentUserProviderMock.Object);
    }

    [Fact]
    public async Task Handle_WhenCurrentUserIsNull_ShouldThrowGuardForbiddenException()
    {
        // Arrange
        var command = new CreateArticleCommand();
        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns((CurrentUser)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<GuardForbiddenException>()
            .WithMessage("Current user is null");
    }

    [Fact]
    public async Task Handle_WhenAuthorNotFound_ShouldThrowGuardNotFoundException()
    {
        // Arrange
        var command = new CreateArticleCommand();
        var currentUser = new CurrentUser { Id = Guid.NewGuid() };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(currentUser.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<GuardNotFoundException>()
            .WithMessage($"User with id {currentUser.Id} was not found");
    }

    [Fact]
    public async Task Handle_WhenCategoryNotFound_ShouldThrowGuardNotFoundException()
    {
        // Arrange
        var command = new CreateArticleCommand { CategoryId = Guid.NewGuid() };
        var currentUser = new CurrentUser { Id = Guid.NewGuid() };
        var author = new User { Id = currentUser.Id };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(currentUser.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(author);
        _unitOfWorkMock.Setup(x => x.CatergoryRepository.GetCategoryByIdAsync(command.CategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<GuardNotFoundException>()
            .WithMessage($"Category whit id {command.CategoryId} was not found");
    }

    [Fact]
    public async Task Handle_WhenArticleAlreadyExists_ShouldThrowGuardArgumentException()
    {
        // Arrange
        var command = new CreateArticleCommand { Id = Guid.NewGuid() };
        var currentUser = new CurrentUser { Id = Guid.NewGuid() };
        var author = new User { Id = currentUser.Id };
        var category = new Category { Id = command.CategoryId };
        var existingArticle = new ArticleModel { Id = command.Id };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(currentUser.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(author);
        _unitOfWorkMock.Setup(x => x.CatergoryRepository.GetCategoryByIdAsync(command.CategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticleByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingArticle);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<GuardArgumentException>()
            .WithMessage("Article id alreafy exist");
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldCreateArticleAndReturnId()
    {
        // Arrange
        var command = new CreateArticleCommand
        {
            Id = Guid.NewGuid(),
            Title = "Test Article",
            ShortDescription = "Test Description",
            CategoryId = Guid.NewGuid()
        };

        var currentUser = new CurrentUser { Id = Guid.NewGuid() };
        var author = new User { Id = currentUser.Id };
        var category = new Category { Id = command.CategoryId };
        var expectedArticle = new ArticleModel
        {
            Id = command.Id,
            AuthorId = author.Id
        };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(currentUser.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(author);
        _unitOfWorkMock.Setup(x => x.CatergoryRepository.GetCategoryByIdAsync(command.CategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetArticleByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ArticleModel)null);

        _mapperMock.Setup(x => x.Map<ArticleModel>(command)).Returns(expectedArticle);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(expectedArticle.Id);
        _unitOfWorkMock.Verify(x => x.ArticleRepository.CreateArticleAsync(expectedArticle, It.IsAny<CancellationToken>()), Times.Once);
    }
}