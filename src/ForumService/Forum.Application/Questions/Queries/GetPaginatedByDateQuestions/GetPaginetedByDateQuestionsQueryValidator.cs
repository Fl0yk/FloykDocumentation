using FluentValidation;

namespace Forum.Application.Questions.Queries.GetPaginatedByDateQuestions;
public class GetPaginetedByDateQuestionsQueryValidator 
                    : AbstractValidator<GetPaginatedByDateQuestionsQuery>
{
    public GetPaginetedByDateQuestionsQueryValidator()
    {
        RuleFor(q => q.PageSize).GreaterThan(0);

        RuleFor(q => q.PageNumber).GreaterThan(0);
    }
}
