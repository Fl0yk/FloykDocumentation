using Forum.Domain.Entities;

namespace Forum.Infrastructure.Specifications.AnswerSpecifications;

public class GetAnswerByIdSpecification
    : Specification<Answer>
{
    public GetAnswerByIdSpecification(Guid answerId)
        : base(answer => answer.Id == answerId)
    {
        AddInclude(answer => answer.Question!);
    }
}
