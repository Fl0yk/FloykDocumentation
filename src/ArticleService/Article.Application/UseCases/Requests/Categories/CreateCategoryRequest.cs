using MediatR;

namespace Article.Application.UseCases.Requests.Categories;

public record class CreateCategoryRequest(string Name, Guid? ParentId) : IRequest;
