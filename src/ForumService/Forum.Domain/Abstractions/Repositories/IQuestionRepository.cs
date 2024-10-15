using Forum.Domain.Entities;
using System.Linq.Expressions;

namespace Forum.Domain.Abstractions.Repositories;

public interface IQuestionRepository
{
    public Task<IQueryable<Question>> GetQuestionsByDateAsync(CancellationToken cancellationToken = default);

    //public Task<IQueryable<Question>> GetQuestionsWithAnswersAsync(CancellationToken cancellationToken = default);

    public Task<Question> GetQuestionByIdWithAnswersAsync(Guid id, CancellationToken cancellationToken = default);

    public Task<Question> FirstOrDefaultAsync(Expression<Func<Question, bool>> filtres, CancellationToken cancellationToken = default);

    public Task<Guid> CreateQuestionAsync(Question question, CancellationToken cancellationToken = default);

    public Task<Guid> UpdateQuestionAsync(Question question, CancellationToken cancellationToken = default);

    public Task DeleteQuestionAsync(Question question, CancellationToken cancellationToken = default);
}

