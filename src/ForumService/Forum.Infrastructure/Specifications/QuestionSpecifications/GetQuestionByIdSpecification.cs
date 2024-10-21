using Forum.Domain.Entities;

namespace Forum.Infrastructure.Specifications.QuestionSpecifications;

public class GetQuestionByIdSpecification
: Specification<Question>
{
    public GetQuestionByIdSpecification(Guid questionId)
        : base(question => question.Id == questionId)
    {

    }
}
