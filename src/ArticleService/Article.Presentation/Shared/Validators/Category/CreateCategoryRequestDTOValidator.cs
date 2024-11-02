using Article.Presentation.Shared.Models.DTOs.Category;
using FluentValidation;

namespace Article.Presentation.Shared.Validators.Category;

public class CreateCategoryRequestDTOValidator : AbstractValidator<CreateCategoryRequestDTO>
{
    public CreateCategoryRequestDTOValidator()
    {
        RuleFor(r => r.Name).NotEmpty();

        RuleFor(r => r.ParentId).NotEmpty();
    }
}
