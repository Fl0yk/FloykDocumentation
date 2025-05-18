using Article.Application.Shared.Models.DTOs;
using MediatR;

namespace Article.Application.UseCases.Query.Articles;

public class GetShouldBeApprovedArticlesQuery : IRequest<IEnumerable<ShortArticleDTO>>
{
}
