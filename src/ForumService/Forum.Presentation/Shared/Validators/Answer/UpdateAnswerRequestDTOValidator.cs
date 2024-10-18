using FluentValidation;
using Forum.Presentation.Shared.Models.DTOs.Answer;

namespace Forum.Presentation.Shared.Validators.Answer;

public class UpdateAnswerRequestDTOValidator : AbstractValidator<UpdateAnswerRequestDTO>
{
    public UpdateAnswerRequestDTOValidator()
    {
        RuleFor(a => a.Id).NotEmpty();

        RuleFor(a => a.Text).NotEmpty();
    }
}
