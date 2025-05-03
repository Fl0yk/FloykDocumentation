using Article.Presentation.Shared.Models.DTOs.Article;
using FluentValidation;

namespace Article.Presentation.Shared.Validators.Article;

public class PublishArticleRequestDTOValidator : AbstractValidator<PublishArticleRequestDTO>
{
    public PublishArticleRequestDTOValidator()
    {
        RuleFor(r => r.ArticleId).NotEmpty();

        RuleFor(r => r.CurrentUserName).NotEmpty();
    }
}
