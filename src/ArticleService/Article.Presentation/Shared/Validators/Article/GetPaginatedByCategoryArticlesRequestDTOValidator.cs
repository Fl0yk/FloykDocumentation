using Article.Presentation.Shared.Models.DTOs.Article;
using FluentValidation;

namespace Article.Presentation.Shared.Validators.Article;

public class GetPaginatedByCategoryArticlesRequestDTOValidator 
    : AbstractValidator<GetPaginatedByCategoryArticlesRequestDTO>
{
    public GetPaginatedByCategoryArticlesRequestDTOValidator()
    {
        RuleFor(r => r.PageNo).GreaterThan(0);

        RuleFor(r => r.PageSize).GreaterThan(0);

        RuleFor(r => r.CategoryId).NotEmpty();
    }
}
