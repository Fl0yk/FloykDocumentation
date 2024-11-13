using MediatR;

namespace Article.Application.UseCases.Requests.Articles;

public record class DeleteArticleRequest(Guid Id, string AuthorName) : IRequest;
