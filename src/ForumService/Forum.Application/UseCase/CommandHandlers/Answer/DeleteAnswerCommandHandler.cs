using Forum.Application.Shared.Exceptions;
using Forum.Application.UseCase.Command.Answer;
using Forum.Domain.Abstractions.Repositories;
using MediatR;

namespace Forum.Application.UseCase.CommandHandlers.Answer;

public class DeleteAnswerCommandHandler : IRequestHandler<DeleteAnswerCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAnswerCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteAnswerCommand request, CancellationToken cancellationToken)
    {
        var dbAnswer = await _unitOfWork.AnswerRepository.FirstOrDefaultByIdWithChildrenAsync(request.Id, cancellationToken);

        if (dbAnswer is null)
        {
            throw new NotFoundException($"Answer with id {request.Id} not found");
        }

        if (dbAnswer.Question!.IsClosed)
        {
            throw new BadRequestException($"Question with id {dbAnswer.Question.Id} closed");
        }

        await _unitOfWork.AnswerRepository.DeleteAnswerAsync(dbAnswer, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
