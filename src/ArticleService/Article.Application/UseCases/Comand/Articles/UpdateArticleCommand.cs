using MediatR;

namespace Article.Application.UseCases.Comand.Articles;

public sealed class UpdateArticleCommand : IRequest
{
    public Guid Id { get; init; }

    public string NewTitle { get; init; } = null!;
}
