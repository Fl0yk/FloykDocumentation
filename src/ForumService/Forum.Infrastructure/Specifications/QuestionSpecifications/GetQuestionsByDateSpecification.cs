using Forum.Domain.Entities;

namespace Forum.Infrastructure.Specifications.QuestionSpecifications;

public class GetQuestionsByDateSpecification
    : Specification<Question>
{
    public GetQuestionsByDateSpecification()
        : base(null)
    {
        AddOrderBy(question => question.DateOfCreation);
    }
}
