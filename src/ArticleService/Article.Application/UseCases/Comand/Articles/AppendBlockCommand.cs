using MediatR;

namespace Article.Application.UseCases.Comand.Articles;

public sealed class AppendBlockCommand : IRequest
{
    public string Text { get; init; } = null!;

    public string BlockType { get; init; } = null!;

    public Guid ArticleId { get; init; }
}
