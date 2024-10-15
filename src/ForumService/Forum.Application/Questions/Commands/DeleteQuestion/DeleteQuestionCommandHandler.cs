using Forum.Application.Shared.Exceptions;
using Forum.Domain.Abstractions.Repositories;
using Forum.Domain.Entities;
using MediatR;

namespace Forum.Application.Questions.Commands.DeleteQuestion;
public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand>
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteQuestionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _questionRepository = unitOfWork.QuestionRepository;
    }

    public async Task Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        Question? dbQuestion = await _questionRepository.FirstOrDefaultByIdAsync(request.Id, cancellationToken);

        if (dbQuestion is null)
            throw new NotFoundException($"Question with id {request.Id} not found");

        await _questionRepository.DeleteQuestionAsync(dbQuestion, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
