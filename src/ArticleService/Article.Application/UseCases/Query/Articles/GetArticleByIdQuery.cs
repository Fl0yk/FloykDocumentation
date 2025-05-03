using Article.Application.Shared.Models.DTOs;
using MediatR;

namespace Article.Application.UseCases.Query.Articles;

public sealed class GetArticleByIdQuery : IRequest<ArticleDTO>
{
    public Guid Id { get; init; }
}
