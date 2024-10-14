using Forum.Domain.Abstractions.Repositories;
using Forum.Domain.Entities;
using MediatR;

namespace Forum.Application.Questions.Commands.CloseQuestion;
public class CloseQuestionCommandHandler : IRequestHandler<CloseQuestionCommand, Guid>
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CloseQuestionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _questionRepository = unitOfWork.QuestionRepository;
    }

    public async Task<Guid> Handle(CloseQuestionCommand request, CancellationToken cancellationToken)
    {
        Question? dbQuestion = await _questionRepository.FirstOrDefaultAsync(q => q.Id == request.Id);

        if (dbQuestion is null)
            throw new KeyNotFoundException($"Question with id {request.Id} not found");

        if (dbQuestion.IsClosed)
            throw new InvalidOperationException($"Question with id {request.Id} is already closed");

        dbQuestion.IsClosed = true;

        Guid id = await _questionRepository.UpdateQuestionAsync(dbQuestion, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return id;
    }
}
