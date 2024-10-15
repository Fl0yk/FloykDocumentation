using FluentValidation;

namespace Forum.Application.Answers.Commands.UpdateAnswer;
public class UpdateAnswerCommandValidator : AbstractValidator<UpdateAnswerCommand>
{
    public UpdateAnswerCommandValidator()
    {
        RuleFor(a => a.Id).NotEmpty();

        RuleFor(a => a.Text).NotEmpty();
    }
}
