using Forum.Application.Shared.Exceptions;
using Forum.Application.UseCase.Command.Question;
using Forum.Domain.Abstractions.Repositories;
using MediatR;

namespace Forum.Application.UseCase.CommandHandlers.Question;

public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteQuestionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        var dbQuestion = await _unitOfWork.QuestionRepository.FirstOrDefaultByIdAsync(request.Id, cancellationToken);

        if (dbQuestion is null)
        {
            throw new NotFoundException($"Question with id {request.Id} not found");
        }

        await _unitOfWork.QuestionRepository.DeleteQuestionAsync(dbQuestion, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
