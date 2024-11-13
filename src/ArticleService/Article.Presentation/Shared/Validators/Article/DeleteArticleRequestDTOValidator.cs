using Article.Presentation.Shared.Models.DTOs.Article;
using FluentValidation;

namespace Article.Presentation.Shared.Validators.Article;

public class DeleteArticleRequestDTOValidator : AbstractValidator<DeleteArticleRequestDTO>
{
    public DeleteArticleRequestDTOValidator()
    {
        RuleFor(r => r.Id).NotEmpty();

        RuleFor(r => r.AuthorName).NotEmpty();
    }
}
