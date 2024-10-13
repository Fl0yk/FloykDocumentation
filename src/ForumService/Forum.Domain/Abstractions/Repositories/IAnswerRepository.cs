using Forum.Domain.Entities;

namespace Forum.Domain.Abstractions.Repositories;
public interface IAnswerRepository
{
    public Task<IQueryable<Answer>> GetAllAnswersAsync();

    public Task<Answer> GetAnswerByIdAsyn(int id);

    public Task<int> CreateAswerAsync(Answer answer);

    public Task<int> UpdateAnswerAsync(Answer answer);

    public Task DeleteAnswerAsync(int id);
}
