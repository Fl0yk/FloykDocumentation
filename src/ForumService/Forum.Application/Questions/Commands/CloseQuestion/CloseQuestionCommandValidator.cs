using FluentValidation;

namespace Forum.Application.Questions.Commands.CloseQuestion;
public class CloseQuestionCommandValidator : AbstractValidator<CloseQuestionCommand>
{
    public CloseQuestionCommandValidator()
    {
        RuleFor(q => q.Id).NotEmpty();
    }
}
