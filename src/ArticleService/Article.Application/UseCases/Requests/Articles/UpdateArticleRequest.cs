using MediatR;

namespace Article.Application.UseCases.Requests.Articles;

public class UpdateArticleRequest : IRequest
{
    public Guid Id { get; set; }

    public required string AuthorName { get; set; }

    public required string NewTitle { get; set; }
}
