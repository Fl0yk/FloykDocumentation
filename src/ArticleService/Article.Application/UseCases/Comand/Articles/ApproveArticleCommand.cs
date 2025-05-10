using MediatR;

namespace Article.Application.UseCases.Comand.Articles;

public sealed class ApproveArticleCommand : IRequest
{
    public Guid ArticleId { get; set; }
}
