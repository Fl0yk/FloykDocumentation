using MediatR;

namespace Article.Application.UseCases.Requests.Articles;

public record class AppendBlockRequest(string Text, string BlockType, Guid ArticleId, string AuthorName) : IRequest;
