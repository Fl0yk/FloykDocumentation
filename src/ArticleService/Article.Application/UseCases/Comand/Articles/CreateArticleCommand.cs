using MediatR;

namespace Article.Application.UseCases.Comand.Articles;

public sealed class CreateArticleCommand : IRequest<Guid>
{
    public Guid Id { get; set; }

    public string Title { get; init; } = null!;

    public string ShortDescription { get; set; } = null!;

    public Guid CategoryId {  get; init; }
}
