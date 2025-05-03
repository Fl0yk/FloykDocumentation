using Forum.Domain.Entities;

namespace Forum.Domain.Abstractions.Repositories;

public interface IQuestionRepository
{
    public Task<IQueryable<Question>> GetQuestionsByDateAsync(CancellationToken cancellationToken = default);

    public Task<IEnumerable<Question>> GetOpenedQuestionsAsync(CancellationToken cancellationToken = default);

    public Task<Question?> FirstOrDefaultByIdAsync(Guid id, CancellationToken cancellationToken = default);

    public Task<Question?> FirstOrDefaultByIdWithAnswersAsync(Guid id, CancellationToken cancellationToken = default);

    public Task<Guid> CreateQuestionAsync(Question question, CancellationToken cancellationToken = default);

    public Task<Guid> UpdateQuestionAsync(Question question, CancellationToken cancellationToken = default);

    public Task DeleteQuestionAsync(Question question, CancellationToken cancellationToken = default);
}

