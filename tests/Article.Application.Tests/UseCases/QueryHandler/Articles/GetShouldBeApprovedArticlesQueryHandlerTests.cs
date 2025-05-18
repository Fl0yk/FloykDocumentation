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

public class GetShouldBeApprovedArticlesQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IBaseCurrentUserProvider> _currentUserProviderMock;
    private readonly GetShouldBeApprovedArticlesQueryHandler _handler;

    public GetShouldBeApprovedArticlesQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _currentUserProviderMock = new Mock<IBaseCurrentUserProvider>();
        _handler = new GetShouldBeApprovedArticlesQueryHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _currentUserProviderMock.Object);
    }

    [Fact]
    public async Task Handle_WhenCurrentUserIsNull_ShouldThrowGuardForbiddenException()
    {
        // Arrange
        var query = new GetShouldBeApprovedArticlesQuery();
        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns((CurrentUser)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<GuardForbiddenException>()
            .WithMessage("Current user is null");
    }

    [Fact]
    public async Task Handle_WhenCurrentUserIsNotAdmin_ShouldThrowGuardForbiddenException()
    {
        // Arrange
        var query = new GetShouldBeApprovedArticlesQuery();
        var currentUser = new CurrentUser { Id = Guid.NewGuid(), Roles = new[] { "Author" } }; // Not admin

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);

        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<GuardForbiddenException>()
            .WithMessage("Current user is null");
    }

    [Fact]
    public async Task Handle_WhenAdminUser_ShouldReturnArticlesNeedingApproval()
    {
        // Arrange
        var query = new GetShouldBeApprovedArticlesQuery();
        var currentUser = new CurrentUser { Id = Guid.NewGuid(), Roles = new[] { "Admin" } };
        var articles = new List<Article.Domain.Entities.Article>
        {
            new() { Id = Guid.NewGuid(), AuthorId = Guid.NewGuid(), IsShouldBeApproved = true },
            new() { Id = Guid.NewGuid(), AuthorId = Guid.NewGuid(), IsShouldBeApproved = true }
        };
        var author1 = new User { Id = articles[0].AuthorId, Username = "author1", PublicUsername = "pub_author1" };
        var author2 = new User { Id = articles[1].AuthorId, Username = "author2", PublicUsername = "pub_author2" };
        var mappedDtos = new List<ShortArticleDTO>
        {
            new() { Id = articles[0].Id, AuthorId = articles[0].AuthorId },
            new() { Id = articles[1].Id, AuthorId = articles[1].AuthorId }
        };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetShouldBeApprovedArticlesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(articles);
        _mapperMock.Setup(x => x.Map<IEnumerable<ShortArticleDTO>>(articles))
            .Returns(mappedDtos);
        _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(articles[0].AuthorId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(author1);
        _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(articles[1].AuthorId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(author2);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
        result.First().AuthorUsername.Should().Be(author1.Username);
        result.First().AuthorPublicUsername.Should().Be(author1.PublicUsername);
        result.Last().AuthorUsername.Should().Be(author2.Username);
        result.Last().AuthorPublicUsername.Should().Be(author2.PublicUsername);
    }

    [Fact]
    public async Task Handle_WhenAuthorNotFound_ShouldSkipSettingAuthorInfo()
    {
        // Arrange
        var query = new GetShouldBeApprovedArticlesQuery();
        var currentUser = new CurrentUser { Id = Guid.NewGuid(), Roles = new[] { "Admin" } };
        var articles = new List<Article.Domain.Entities.Article>
        {
            new() { Id = Guid.NewGuid(), AuthorId = Guid.NewGuid(), IsShouldBeApproved = true }
        };
        var mappedDto = new ShortArticleDTO { Id = articles[0].Id, AuthorId = articles[0].AuthorId };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetShouldBeApprovedArticlesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(articles);
        _mapperMock.Setup(x => x.Map<IEnumerable<ShortArticleDTO>>(articles))
            .Returns(new List<ShortArticleDTO> { mappedDto });
        _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(articles[0].AuthorId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        result.First().AuthorUsername.Should().BeNull();
        result.First().AuthorPublicUsername.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryWithCorrectParameters()
    {
        // Arrange
        var query = new GetShouldBeApprovedArticlesQuery();
        var currentUser = new CurrentUser { Id = Guid.NewGuid(), Roles = new[] { "Admin" } };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _unitOfWorkMock.Setup(x => x.ArticleRepository.GetShouldBeApprovedArticlesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Article.Domain.Entities.Article>());
        _mapperMock.Setup(x => x.Map<IEnumerable<ShortArticleDTO>>(It.IsAny<IEnumerable<Article.Domain.Entities.Article>>()))
            .Returns(new List<ShortArticleDTO>());

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(x => x.ArticleRepository.GetShouldBeApprovedArticlesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}