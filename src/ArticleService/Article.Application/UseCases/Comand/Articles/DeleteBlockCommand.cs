using MediatR;

namespace Article.Application.UseCases.Comand.Articles;

public sealed class DeleteBlockCommand : IRequest
{
    public Guid ArticleId { get; init; }

    public Guid BlockId { get; init; }
}
