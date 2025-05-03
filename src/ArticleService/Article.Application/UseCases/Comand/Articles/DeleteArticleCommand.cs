using MediatR;

namespace Article.Application.UseCases.Comand.Articles;

public sealed class DeleteArticleCommand : IRequest
{
    public Guid Id { get; init; }
}
