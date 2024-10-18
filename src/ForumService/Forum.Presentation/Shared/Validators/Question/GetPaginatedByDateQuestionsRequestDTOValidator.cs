using FluentValidation;
using Forum.Presentation.Shared.Models.DTOs.Question;

namespace Forum.Presentation.Shared.Validators.Question;

public class GetPaginatedByDateQuestionsRequestDTOValidator : AbstractValidator<GetPaginatedByDateQuestionsRequestDTO>
{
    public GetPaginatedByDateQuestionsRequestDTOValidator()
    {
        RuleFor(q => q.PageSize).GreaterThan(0);

        RuleFor(q => q.PageNumber).GreaterThan(0);
    }
}
