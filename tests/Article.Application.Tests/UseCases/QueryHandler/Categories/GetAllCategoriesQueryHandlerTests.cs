using Article.Application.Shared.Models.DTOs;
using Article.Application.UseCases.Query.Categories;
using Article.Application.UseCases.QueryHandler.Categories;
using Article.Domain.Abstractions.Repositories;
using Article.Domain.Entities;
using AutoMapper;
using FluentAssertions;
using Moq;

namespace Article.Application.Tests.UseCases.QueryHandler.Categories;

public class GetAllCategoriesQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllCategoriesQueryHandler _handler;

    public GetAllCategoriesQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetAllCategoriesQueryHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAllCategories()
    {
        // Arrange
        var query = new GetAllCategoriesQuery();
        var categories = new List<Category>
        {
            new() { Id = Guid.NewGuid(), Name = "Category 1" },
            new() { Id = Guid.NewGuid(), Name = "Category 2" }
        };
        var expectedDtos = new List<CategoryDTO>
        {
            new() { Id = categories[0].Id, Name = categories[0].Name },
            new() { Id = categories[1].Id, Name = categories[1].Name }
        };

        _unitOfWorkMock.Setup(x => x.CatergoryRepository.GetAllCategoriesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(categories);
        _mapperMock.Setup(x => x.Map<IEnumerable<CategoryDTO>>(categories))
            .Returns(expectedDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedDtos);
        _unitOfWorkMock.Verify(x => x.CatergoryRepository.GetAllCategoriesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenNoCategoriesExist_ShouldReturnEmptyCollection()
    {
        // Arrange
        var query = new GetAllCategoriesQuery();
        var emptyCategories = Enumerable.Empty<Category>();

        _unitOfWorkMock.Setup(x => x.CatergoryRepository.GetAllCategoriesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyCategories);
        _mapperMock.Setup(x => x.Map<IEnumerable<CategoryDTO>>(emptyCategories))
            .Returns(Enumerable.Empty<CategoryDTO>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationTokenToRepository()
    {
        // Arrange
        var query = new GetAllCategoriesQuery();
        var cancellationToken = new CancellationToken(true);
        var categories = new List<Category> { new() };

        _unitOfWorkMock.Setup(x => x.CatergoryRepository.GetAllCategoriesAsync(cancellationToken))
            .ReturnsAsync(categories);
        _mapperMock.Setup(x => x.Map<IEnumerable<CategoryDTO>>(It.IsAny<IEnumerable<Category>>()))
            .Returns(new List<CategoryDTO> { new() });

        // Act
        await _handler.Handle(query, cancellationToken);

        // Assert
        _unitOfWorkMock.Verify(x => x.CatergoryRepository.GetAllCategoriesAsync(cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldMapCategoriesToCategoryDTOs()
    {
        // Arrange
        var query = new GetAllCategoriesQuery();
        var categories = new List<Category> { new() { Id = Guid.NewGuid() } };
        var expectedDtos = new List<CategoryDTO> { new() { Id = categories[0].Id } };

        _unitOfWorkMock.Setup(x => x.CatergoryRepository.GetAllCategoriesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(categories);
        _mapperMock.Setup(x => x.Map<IEnumerable<CategoryDTO>>(categories))
            .Returns(expectedDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeSameAs(expectedDtos);
        _mapperMock.Verify(x => x.Map<IEnumerable<CategoryDTO>>(categories), Times.Once);
    }
}