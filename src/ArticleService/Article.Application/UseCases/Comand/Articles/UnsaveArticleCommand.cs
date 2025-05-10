using MediatR;

namespace Article.Application.UseCases.Comand.Articles;

public sealed class UnsaveArticleCommand : IRequest
{
    public Guid ArticleId { get; init; }
}
