using Forum.Domain.Abstractions.Repositories;
using Forum.Domain.Entities;
using Forum.Infrastructure.Specifications;
using Forum.Infrastructure.Specifications.AnswerSpecifications;
using Microsoft.EntityFrameworkCore;

namespace Forum.Infrastructure.Repositories;

public class AnswerRepository : IAnswerRepository
{
    private readonly DbSet<Answer> _answers;

    public AnswerRepository(ApplicationDbContext context)
    {
        _answers = context.Answers;
    }

    public Task<Answer?> FirstOrDefaultByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
                            ApplySpecification(new GetAnswerByIdSpecification(id)).FirstOrDefaultAsync(cancellationToken);

    public Task<Answer?> FirstOrDefaultByIdWithChildrenAsync(Guid id, CancellationToken cancellationToken = default) =>
                            ApplySpecification(new GetAnswerByIdWithChildrenSpecification(id)).FirstOrDefaultAsync(cancellationToken);

    public Task<Guid> CreateAswerAsync(Answer answer, CancellationToken cancellationToken)
    {
        _answers.Add(answer);

        return Task.FromResult(answer.Id);
    }

    public Task DeleteAnswerAsync(Answer answer, CancellationToken cancellationToken)
    {
        DeleteNode(answer);

        return Task.CompletedTask;
    }

    public Task<Guid> UpdateAnswerAsync(Answer answer, CancellationToken cancellationToken)
    {
        _answers.Update(answer);

        return Task.FromResult(answer.Id);
    }

    private IQueryable<Answer> ApplySpecification(Specification<Answer> specification)
    {
        return SpecificationEvaluator.GetQuery(_answers, specification);
    }

    private void DeleteNode(Answer node)
    {
        if (node.Childrens.Any())
        {
            foreach (var child in node.Childrens) 
                DeleteNode(child);
        }

        _answers.Remove(node);
    }
}
