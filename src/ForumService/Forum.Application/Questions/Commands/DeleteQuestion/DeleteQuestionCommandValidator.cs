using FluentValidation;

namespace Forum.Application.Questions.Commands.DeleteQuestion;
public class DeleteQuestionCommandValidator : AbstractValidator<DeleteQuestionCommand>
{
    public DeleteQuestionCommandValidator()
    {
        RuleFor(q => q.Id).NotEmpty();
    }
}
