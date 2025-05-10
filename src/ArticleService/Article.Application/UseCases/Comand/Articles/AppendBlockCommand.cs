using Article.Domain.Entities;
using MediatR;

namespace Article.Application.UseCases.Comand.Articles;

public sealed class AppendBlockCommand : IRequest
{
    public string Text { get; init; } = null!;

    public BlockType BlockType { get; init; }

    public Guid ArticleId { get; init; }
}
