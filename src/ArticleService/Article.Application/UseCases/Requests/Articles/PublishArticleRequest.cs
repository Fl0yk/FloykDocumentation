using MediatR;

namespace Article.Application.UseCases.Requests.Articles;

public class PublishArticleRequest : IRequest
{
    public Guid ArticleId { get; set; }

    public required string AuthorName { get; set; }
}