using Forum.Domain.Entities;

namespace Forum.Infrastructure.Specifications.QuestionSpecifications;

public class GetQuestionsByDateSpecification
    : Specification<Question>
{
    public GetQuestionsByDateSpecification()
        : base(null)
    {
        AddOrderByDescending(question => question.DateOfCreation);
    }
}
