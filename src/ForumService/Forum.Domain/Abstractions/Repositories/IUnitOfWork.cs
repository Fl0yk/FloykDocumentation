namespace Forum.Domain.Abstractions.Repositories;
public interface IUnitOfWork
{
    IQuestionRepository QuestionRepository { get; }

    IAnswerRepository AnswerRepository { get; }

    public Task SaveChangesAsync(CancellationToken cancellationToken);
}
