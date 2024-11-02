using Article.Presentation.Shared.Models.DTOs.Article;
using FluentValidation;

namespace Article.Presentation.Shared.Validators.Article;

public class GetPaginatedByDateArticlesRequestDTOValidator : AbstractValidator<GetPaginatedByDateArticlesRequestDTO>
{
    public GetPaginatedByDateArticlesRequestDTOValidator()
    {
        RuleFor(r => r.PageNo).GreaterThan(0);

        RuleFor(r => r.PageSize).GreaterThan(0);
    }
}
