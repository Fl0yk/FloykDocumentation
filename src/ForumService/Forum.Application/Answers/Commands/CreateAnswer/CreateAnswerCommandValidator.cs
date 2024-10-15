using FluentValidation;

namespace Forum.Application.Answers.Commands.CreateAnswer;
public class CreateAnswerCommandValidator : AbstractValidator<CreateAnswerCommand>
{
    public CreateAnswerCommandValidator()
    {
        RuleFor(a => a.Text).NotEmpty();

        RuleFor(a => a.AuthorId).NotEmpty();

        RuleFor(a => a.QuestionId).NotEmpty();
    }
}
