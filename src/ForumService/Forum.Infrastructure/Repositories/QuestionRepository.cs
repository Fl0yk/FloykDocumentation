using Forum.Domain.Abstractions.Repositories;
using Forum.Domain.Entities;
using Forum.Infrastructure.Specifications;
using Forum.Infrastructure.Specifications.QuestionSpecifications;
using Microsoft.EntityFrameworkCore;

namespace Forum.Infrastructure.Repositories;

public class QuestionRepository : IQuestionRepository
{
    private readonly DbSet<Question> _questions;

    public QuestionRepository(ApplicationDbContext context)
    {
        _questions = context.Questions;
    }

    public Task<Guid> CreateQuestionAsync(Question question, CancellationToken cancellationToken = default)
    {
        _questions.Add(question);

        return Task.FromResult(question.Id);
    }

    public Task DeleteQuestionAsync(Question question, CancellationToken cancellationToken = default)
    {
        _questions.Remove(question);

        return Task.CompletedTask;
    }

    public Task<Question?> FirstOrDefaultByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
                                    ApplySpecification(new GetQuestionByIdSpecification(id))
                                    .FirstOrDefaultAsync(cancellationToken);

    public Task<Question?> FirstOrDefaultByIdWithAnswersAsync(Guid id, CancellationToken cancellationToken = default) =>
                                    ApplySpecification(new GetQuestionByIdWithAnswersSpecification(id))
                                    .FirstOrDefaultAsync(cancellationToken);

    public Task<IQueryable<Question>> GetQuestionsByDateAsync(CancellationToken cancellationToken = default) =>
                                    Task.FromResult(
                                        ApplySpecification(new GetQuestionsByDateSpecification()));

    public Task<Guid> UpdateQuestionAsync(Question question, CancellationToken cancellationToken = default)
    {
        _questions.Update(question);

        return Task.FromResult(question.Id);
    }

    private IQueryable<Question> ApplySpecification(Specification<Question> specification)
    {
        return SpecificationEvaluator.GetQuery(_questions, specification);
    }
}
