using Article.Application.Shared.Models.DTOs;
using MediatR;

namespace Article.Application.UseCases.Requests.Categories;

public record class GetCategoryByIdWithoutArticlesRequest(Guid Id) : IRequest<CategoryDTO>;
