using FluentValidation;
using Identity.Presentation.Shared.Models.DTOs.User;

namespace Identity.Presentation.Shared.Validators.User;

public class SaveArticleRequestDTOValidator : AbstractValidator<SaveArticleRequestDTO>
{
    public SaveArticleRequestDTOValidator()
    {
        RuleFor(r => r.Id).NotEmpty();

        RuleFor(r => r.ArticleName).NotEmpty();
    }
}
