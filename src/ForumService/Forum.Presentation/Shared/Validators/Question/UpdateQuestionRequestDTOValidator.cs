using FluentValidation;
using Forum.Presentation.Shared.Models.DTOs.Question;

namespace Forum.Presentation.Shared.Validators.Question;

public class UpdateQuestionRequestDTOValidator : AbstractValidator<UpdateQuestionRequestDTO>
{
    public UpdateQuestionRequestDTOValidator()
    {
        RuleFor(q => q.Id).NotEmpty();

        RuleFor(q => q.Title).NotEmpty().MaximumLength(250);

        RuleFor(q => q.Description).NotNull();
    }
}
