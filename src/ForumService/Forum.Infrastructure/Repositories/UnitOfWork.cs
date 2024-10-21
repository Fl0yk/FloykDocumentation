using Forum.Domain.Abstractions.Repositories;

namespace Forum.Infrastructure.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    private Lazy<IQuestionRepository> _questionRepository;

    private Lazy<IAnswerRepository> _answerRepository;

    public IQuestionRepository QuestionRepository => _questionRepository.Value;

    public IAnswerRepository AnswerRepository => _answerRepository.Value;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        _questionRepository = new(() => new QuestionRepository(context));
        _answerRepository = new(() => new  AnswerRepository(context));
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
