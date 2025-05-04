using Forum.Domain.Abstractions.Repositories;
using Forum.Infrastructure.Database;

namespace Forum.Infrastructure.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    private readonly Lazy<IQuestionRepository> _questionRepository;

    private readonly Lazy<IAnswerRepository> _answerRepository;

    private readonly Lazy<IUserRepository> _userRepository;

    public IQuestionRepository QuestionRepository => _questionRepository.Value;

    public IAnswerRepository AnswerRepository => _answerRepository.Value;

    public IUserRepository UserRepository => _userRepository.Value;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        _questionRepository = new(() => new QuestionRepository(context));
        _answerRepository = new(() => new  AnswerRepository(context));
        _userRepository = new(() => new UserRepository(context));
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
