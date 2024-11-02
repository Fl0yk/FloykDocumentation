using Article.Presentation.Shared.Models.DTOs.Article;
using FluentValidation;

namespace Article.Presentation.Shared.Validators.Article;

public class DeleteBlockRequestDTOValidator : AbstractValidator<DeleteBlockRequestDTO>
{
    public DeleteBlockRequestDTOValidator()
    {
        RuleFor(r => r.ArticleId).NotEmpty();

        RuleFor(r => r.BlockId).NotEmpty();

        RuleFor(r => r.AuthorName).NotEmpty();
    }
}
