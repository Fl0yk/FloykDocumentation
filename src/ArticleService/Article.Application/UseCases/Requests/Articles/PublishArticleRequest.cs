using MediatR;

namespace Article.Application.UseCases.Requests.Articles;

public record class PublishArticleRequest(Guid ArticleId, string AuthorName) : IRequest;
