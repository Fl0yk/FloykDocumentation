using FluentValidation;

namespace Forum.Application.Questions.Commands.UpdateQuestion;
public class UpdateQuestionCommandValidator : AbstractValidator<UpdateQuestionCommand>
{
    public UpdateQuestionCommandValidator()
    {
        RuleFor(q => q.Id).NotEmpty();

        RuleFor(q => q.Title).NotEmpty().MaximumLength(250);

        RuleFor(q => q.Description).NotNull();
    }
}
