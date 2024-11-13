using MediatR;

namespace Article.Application.UseCases.Requests.Articles;

public record class DeleteBlockRequest(Guid ArticleId, Guid BlockId, string AuthorName) : IRequest;
