using Forum.Domain.Entities;

namespace Forum.Infrastructure.Specifications.QuestionSpecifications;

public class GetQuestionByIdWithAnswersSpecification
    : Specification<Question>
{
    public GetQuestionByIdWithAnswersSpecification(Guid questionId)
        : base(question => question.Id == questionId)
    {
        AddInclude(question => question.Answers);
    }
}
