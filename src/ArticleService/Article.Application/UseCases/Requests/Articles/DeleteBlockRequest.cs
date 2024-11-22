using MediatR;

namespace Article.Application.UseCases.Requests.Articles;

public class DeleteBlockRequest : IRequest
{
    public Guid ArticleId { get; set; }

    public Guid BlockId { get; set; }

    public required string AuthorName { get; set; }
}