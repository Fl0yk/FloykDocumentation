using FluentValidation;

namespace Forum.Application.Answers.Commands.DeleteAnswer;

public class DeleteAnswerCommandValidator : AbstractValidator<DeleteAnswerCommand>
{
    public DeleteAnswerCommandValidator()
    {
        RuleFor(a => a.Id).NotEmpty();
    }
}

