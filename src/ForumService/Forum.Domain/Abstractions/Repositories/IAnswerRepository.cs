using Forum.Domain.Entities;
using System.Linq.Expressions;

namespace Forum.Domain.Abstractions.Repositories;
public interface IAnswerRepository
{
    public Task<Answer?> FirstOrDefaultByIdAsync(Guid id, CancellationToken cancellationToken = default);

    public Task<Answer?> FirstOrDefaultByIdWithChildrenAsync(Guid id, CancellationToken cancellationToken = default);

    public Task<Guid> CreateAswerAsync(Answer answer, CancellationToken cancellationToken);

    public Task<Guid> UpdateAnswerAsync(Answer answer, CancellationToken cancellationToken);

    public Task DeleteAnswerAsync(Answer answer, CancellationToken cancellationToken);
}
