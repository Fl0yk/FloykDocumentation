using Forum.Application.Shared.Exceptions;
using Forum.Application.UseCase.Command.Question;
using Forum.Domain.Abstractions.Repositories;
using MediatR;

namespace Forum.Application.UseCase.CommandHandlers.Question;

public class CloseQuestionCommandHandler : IRequestHandler<CloseQuestionCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public CloseQuestionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CloseQuestionCommand request, CancellationToken cancellationToken)
    {
        var dbQuestion = await _unitOfWork.QuestionRepository.FirstOrDefaultByIdAsync(request.Id);

        if (dbQuestion is null)
        {
            throw new NotFoundException($"Question with id {request.Id} not found");
        }

        if (dbQuestion.IsClosed)
        {
            throw new BadRequestException($"Question with id {request.Id} is already closed");
        }

        dbQuestion.IsClosed = true;

        Guid id = await _unitOfWork.QuestionRepository.UpdateQuestionAsync(dbQuestion, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return id;
    }
}
