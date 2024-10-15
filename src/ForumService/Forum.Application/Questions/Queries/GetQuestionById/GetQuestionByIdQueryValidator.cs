using FluentValidation;

namespace Forum.Application.Questions.Queries.GetQuestionById;
public class GetQuestionByIdQueryValidator : AbstractValidator<GetQuestionByIdQuery>
{
    public GetQuestionByIdQueryValidator()
    {
        RuleFor(q => q.Id).NotEmpty();
    }
}
