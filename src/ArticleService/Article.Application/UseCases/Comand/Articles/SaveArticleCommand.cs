using MediatR;

namespace Article.Application.UseCases.Comand.Articles;

public sealed class SaveArticleCommand : IRequest
{
    public Guid ArticleId { get; init; }
}
