using FluentValidation;
using Forum.Presentation.Shared.Models.DTOs.Answer;

namespace Forum.Presentation.Shared.Validators.Answer;

public class AddAnswerRequestDTOValidator : AbstractValidator<AddAnswerRequestDTO>
{
    public AddAnswerRequestDTOValidator()
    {
        RuleFor(a => a.Text).NotEmpty();

        RuleFor(a => a.AuthorId).NotEmpty();

        RuleFor(a => a.QuestionId).NotEmpty();
    }
}
