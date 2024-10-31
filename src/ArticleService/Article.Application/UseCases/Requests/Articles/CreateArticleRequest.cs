using MediatR;

namespace Article.Application.UseCases.Requests.Articles;

public record class CreateArticleRequest(string Title, string AuthorName, Guid CategoryId) : IRequest;
