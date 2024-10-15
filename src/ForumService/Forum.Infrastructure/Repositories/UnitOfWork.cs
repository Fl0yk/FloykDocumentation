using Forum.Domain.Abstractions.Repositories;

namespace Forum.Infrastructure.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    private IQuestionRepository _questionRepository;

    private IAnswerRepository _answerRepository;

    public IQuestionRepository QuestionRepository => _questionRepository;

    public IAnswerRepository AnswerRepository => _answerRepository;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        _questionRepository = new QuestionRepository(context);
        _answerRepository = new AnswerRepository(context);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
