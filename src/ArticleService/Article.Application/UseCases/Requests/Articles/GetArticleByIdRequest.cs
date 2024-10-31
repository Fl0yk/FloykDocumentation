using Article.Application.Shared.Models.DTOs;
using MediatR;

namespace Article.Application.UseCases.Requests.Articles;

public record class GetArticleByIdRequest(Guid Id) : IRequest<ArticleDTO>;
