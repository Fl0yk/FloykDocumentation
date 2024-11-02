using MediatR;

namespace Article.Application.UseCases.Requests.Categories;

public record class DeleteCategoryRequest(Guid CategoryId) : IRequest;
