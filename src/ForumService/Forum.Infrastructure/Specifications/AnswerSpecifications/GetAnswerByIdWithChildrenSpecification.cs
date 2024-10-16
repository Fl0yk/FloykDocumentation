using Forum.Domain.Entities;

namespace Forum.Infrastructure.Specifications.AnswerSpecifications;

public class GetAnswerByIdWithChildrenSpecification
    : Specification<Answer>
{
    public GetAnswerByIdWithChildrenSpecification(Guid answerId)
        : base (answer => answer.Id == answerId)
    {
        AddInclude(answer => answer.Childrens);
    }
}
