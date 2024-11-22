using Article.Presentation.Shared.Models.DTOs.Article;
using FluentValidation;

namespace Article.Presentation.Shared.Validators.Article;

public class CreateArticleRequestDTOValidator : AbstractValidator<CreateArticleRequestDTO>
{
    public CreateArticleRequestDTOValidator()
    {
        RuleFor(r => r.Title).NotEmpty().MaximumLength(256);

        RuleFor(r => r.CurrentUserName).NotEmpty();

        RuleFor(r => r.CategoryId).NotEmpty();
    }
}
