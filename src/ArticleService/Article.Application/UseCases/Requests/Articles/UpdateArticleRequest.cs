using MediatR;

namespace Article.Application.UseCases.Requests.Articles;

public record class UpdateArticleRequest(Guid Id, string AuthorName, string NewTitle) : IRequest;