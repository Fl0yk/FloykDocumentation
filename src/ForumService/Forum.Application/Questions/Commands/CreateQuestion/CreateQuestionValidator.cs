using FluentValidation;

namespace Forum.Application.Questions.Commands.CreateQuestion;
public class CreateQuestionValidator : AbstractValidator<CreateQuestionCommand>
{
    public CreateQuestionValidator()
    {
        RuleFor(q => q.Title).NotEmpty().MaximumLength(250);

        RuleFor(q => q.Description).NotNull();

        RuleFor(q => q.AuthorId).NotEmpty();
    }
}
