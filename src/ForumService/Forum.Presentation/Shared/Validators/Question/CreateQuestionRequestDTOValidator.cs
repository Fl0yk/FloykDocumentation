using FluentValidation;
using Forum.Presentation.Shared.Models.DTOs.Question;

namespace Forum.Presentation.Shared.Validators.Question;

public class CreateQuestionRequestDTOValidator : AbstractValidator<CreateQuestionRequestDTO>
{
    public CreateQuestionRequestDTOValidator()
    {
        RuleFor(q => q.Title).NotEmpty().MaximumLength(250);

        RuleFor(q => q.Description).NotNull();

        RuleFor(q => q.CurrentUserId).NotEmpty();
    }
}
