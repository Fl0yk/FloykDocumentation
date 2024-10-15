using FluentValidation;

namespace Forum.Application.Answers.Queries;
public class GetAnswerByIdQueryValidator : AbstractValidator<GetAnswerByIdQuery>
{
    public GetAnswerByIdQueryValidator()
    {
        RuleFor(a => a.Id).NotEmpty();
    }
}
