using MediatR;

namespace Article.Application.UseCases.Comand.Articles;

public sealed class CreateArticleCommand : IRequest
{
    public string Title { get; init; } = null!;

    public Guid CategoryId {  get; init; }
}
