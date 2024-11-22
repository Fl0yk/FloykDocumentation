using MediatR;

namespace Article.Application.UseCases.Requests.Articles;

public class AppendBlockRequest : IRequest
{
    public required string Text { get; set; }

    public required string BlockType { get; set; }

    public Guid ArticleId { get; set; }

    public required string AuthorName { get; set; }
}
