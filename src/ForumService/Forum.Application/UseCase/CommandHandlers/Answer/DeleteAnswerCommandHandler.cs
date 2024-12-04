using Forum.Application.Shared.Exceptions;
using Forum.Application.Shared.Models.Responses;
using Forum.Application.UseCase.Command.Answer;
using Forum.Domain.Abstractions.Repositories;
using MediatR;

namespace Forum.Application.UseCase.CommandHandlers.Answer;

public class DeleteAnswerCommandHandler : IRequestHandler<DeleteAnswerCommand, DeleteAnswerResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAnswerCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<DeleteAnswerResponse> Handle(DeleteAnswerCommand request, CancellationToken cancellationToken)
    {
        var dbAnswer = await _unitOfWork.AnswerRepository.FirstOrDefaultByIdWithChildrenAsync(request.Id, cancellationToken);

        if (dbAnswer is null)
        {
            throw new NotFoundException($"Answer with id {request.Id} not found");
        }

        if (dbAnswer.AuthorId != request.AuthorId)
        {
            throw new ForbiddenException($"User with id {request.AuthorId} is not the author of the answer");
        }

        if (dbAnswer.Question!.IsClosed)
        {
            throw new BadRequestException($"Question with id {dbAnswer.Question.Id} closed");
        }

        await _unitOfWork.AnswerRepository.DeleteAnswerAsync(dbAnswer, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new DeleteAnswerResponse 
        {
            AnswerId = dbAnswer.Id,
            QuestionId = dbAnswer.Question.Id,
        };
    }
}
