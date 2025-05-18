using Article.Application.Shared.Models.DTOs;
using Article.Application.UseCases.Query.Categories;
using Article.Application.UseCases.QueryHandler.Categories;
using Article.Domain.Abstractions.Repositories;
using Article.Domain.Entities;
using AutoMapper;
using Core.Exceptions;
using FluentAssertions;
using Moq;

namespace Article.Application.Tests.UseCases.QueryHandler.Categories;

public class GetCategoryByIdWithoutArticlesQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetCategoryByIdWithoutArticlesQueryHandler _handler;

    public GetCategoryByIdWithoutArticlesQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetCategoryByIdWithoutArticlesQueryHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WhenCategoryNotFound_ShouldThrowGuardNotFoundException()
    {
        // Arrange
        var query = new GetCategoryByIdWithoutArticlesQuery { Id = Guid.NewGuid() };
        _unitOfWorkMock.Setup(x => x.CatergoryRepository.GetCategoryByIdAsync(query.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<GuardNotFoundException>()
            .WithMessage($"Category with id {query.Id} was not found");
    }

    [Fact]
    public async Task Handle_WhenCategoryExists_ShouldReturnMappedCategoryDTO()
    {
        // Arrange
        var query = new GetCategoryByIdWithoutArticlesQuery { Id = Guid.NewGuid() };
        var category = new Category
        {
            Id = query.Id,
            Name = "Test Category",
            ParentId = Guid.NewGuid(),
            Order = 1
        };
        var expectedDto = new CategoryDTO
        {
            Id = category.Id,
            Name = category.Name,
            ParentId = category.ParentId,
            Order = category.Order
        };

        _unitOfWorkMock.Setup(x => x.CatergoryRepository.GetCategoryByIdAsync(query.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);
        _mapperMock.Setup(x => x.Map<CategoryDTO>(category))
            .Returns(expectedDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedDto);
        _unitOfWorkMock.Verify(x => x.CatergoryRepository.GetCategoryByIdAsync(query.Id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationTokenToRepository()
    {
        // Arrange
        var query = new GetCategoryByIdWithoutArticlesQuery { Id = Guid.NewGuid() };
        var cancellationToken = new CancellationToken(true);
        var category = new Category { Id = query.Id };

        _unitOfWorkMock.Setup(x => x.CatergoryRepository.GetCategoryByIdAsync(query.Id, cancellationToken))
            .ReturnsAsync(category);
        _mapperMock.Setup(x => x.Map<CategoryDTO>(category))
            .Returns(new CategoryDTO());

        // Act
        await _handler.Handle(query, cancellationToken);

        // Assert
        _unitOfWorkMock.Verify(x => x.CatergoryRepository.GetCategoryByIdAsync(query.Id, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldMapCategoryToCategoryDTO()
    {
        // Arrange
        var query = new GetCategoryByIdWithoutArticlesQuery { Id = Guid.NewGuid() };
        var category = new Category { Id = query.Id };
        var expectedDto = new CategoryDTO { Id = category.Id };

        _unitOfWorkMock.Setup(x => x.CatergoryRepository.GetCategoryByIdAsync(query.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);
        _mapperMock.Setup(x => x.Map<CategoryDTO>(category))
            .Returns(expectedDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeSameAs(expectedDto);
        _mapperMock.Verify(x => x.Map<CategoryDTO>(category), Times.Once);
    }
}