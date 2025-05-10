using Article.Application.Shared.Models.DTOs;
using MediatR;

namespace Article.Application.UseCases.Query.Articles;

public sealed class GetSavedArticlesByUserQuery : IRequest<IEnumerable<ShortArticleDTO>>
{
    
}
