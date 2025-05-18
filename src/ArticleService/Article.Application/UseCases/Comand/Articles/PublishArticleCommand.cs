using MediatR;

namespace Article.Application.UseCases.Comand.Articles;

public sealed class PublishArticleCommand : IRequest
{
    public Guid ArticleId { get; init; }
}
