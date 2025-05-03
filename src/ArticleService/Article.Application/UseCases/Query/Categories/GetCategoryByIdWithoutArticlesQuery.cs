using Article.Application.Shared.Models.DTOs;
using MediatR;

namespace Article.Application.UseCases.Query.Categories;

public sealed class GetCategoryByIdWithoutArticlesQuery : IRequest<CategoryDTO>
{
    public Guid Id { get; init; }
}
