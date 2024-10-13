using Forum.Domain.Entities;

namespace Forum.Domain.Abstractions.Repositories;

public interface IQuestionRepository
{
    public Task<IQueryable<Question>> GetAllQuestionsAsync();

    public Task<Question> GetQuestionByIdAsync(Guid id);

    public Task<Guid> CreateQuestionAsync(Question question);

    public Task<Guid> UpdateQuestionAsync(Question question);

    public Task DeleteQuestionAsync(Guid id);
}

