using Core.Exceptions;
using Core.Providers.Interfaces;
using Forum.Application.Shared.Models.Responses;
using Forum.Application.UseCase.Command.Answer;
using Forum.Domain.Abstractions.Repositories;
using MediatR;

namespace Forum.Application.UseCase.CommandHandlers.Answer;

public class DeleteAnswerCommandHandler : IRequestHandler<DeleteAnswerCommand, DeleteAnswerResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITransactionProvider _transactionProvider;
    private readonly IBaseCurrentUserProvider _currentUserProvider;

    public DeleteAnswerCommandHandler(
        IUnitOfWork unitOfWork,
        ITransactionProvider transactionProvider,
        IBaseCurrentUserProvider currentUserProvider)
    {
        _unitOfWork = unitOfWork;
        _transactionProvider = transactionProvider;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<DeleteAnswerResponse> Handle(DeleteAnswerCommand request, CancellationToken cancellationToken)
    {
        await _transactionProvider.OpenTransaction(cancellationToken);

        var currentUser = _currentUserProvider.GetCurrentUser();

        if (currentUser is null)
        {
            throw new GuardForbiddenException("Current user is null");
        }

        var dbAnswer = await _unitOfWork.AnswerRepository.FirstOrDefaultByIdWithChildrenAsync(request.Id, cancellationToken);

        if (dbAnswer is null)
        {
            throw new GuardNotFoundException($"Answer with id {request.Id} not found");
        }

        if (dbAnswer.AuthorId != currentUser.Id)
        {
            throw new GuardForbiddenException($"User with id {currentUser.Id} is not the author of the answer");
        }

        if (dbAnswer.Question!.IsClosed)
        {
            throw new GuardArgumentException($"Question with id {dbAnswer.Question.Id} closed");
        }

        await _unitOfWork.AnswerRepository.DeleteAnswerAsync(dbAnswer, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _transactionProvider.Commit(cancellationToken);

        return new DeleteAnswerResponse 
        {
            AnswerId = dbAnswer.Id,
            QuestionId = dbAnswer.Question.Id,
        };
    }
}
