using MediatR;

namespace Article.Application.UseCases.Requests.Articles;

public class CreateArticleRequest : IRequest
{
    public required string Title { get; set; }

    public required string AuthorName { get; set; }

    public Guid CategoryId { get; set; }
}
