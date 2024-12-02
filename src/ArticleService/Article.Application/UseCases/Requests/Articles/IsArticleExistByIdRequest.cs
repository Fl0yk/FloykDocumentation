using MediatR;

namespace Article.Application.UseCases.Requests.Articles;

public record class IsArticleExistByIdRequest(Guid Id) : IRequest<bool>;
