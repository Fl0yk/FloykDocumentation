using Article.Presentation.Shared.Models.DTOs.Article;
using FluentValidation;

namespace Article.Presentation.Shared.Validators.Article;

public class AppendBlockRequestDTOValidator : AbstractValidator<AppendBlockRequestDTO>
{
    public AppendBlockRequestDTOValidator()
    {
        RuleFor(r => r.CurrentUserName).NotEmpty();

        RuleFor(r => r.ArticleId).NotEmpty();

        RuleFor(r => r.Text).NotEmpty();

        RuleFor(r => r.BlockType).NotEmpty();
    }
}
