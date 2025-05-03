using Core.Exceptions;
using Core.Providers.Interfaces;
using Forum.Application.UseCase.Command.Question;
using Forum.Domain.Abstractions.Repositories;
using MediatR;

namespace Forum.Application.UseCase.CommandHandlers.Question;

public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITransactionProvider _transactionProvider;
    private readonly IBaseCurrentUserProvider _currentUserProvider;

    public DeleteQuestionCommandHandler(
        IUnitOfWork unitOfWork,
        ITransactionProvider transactionProvider,
        IBaseCurrentUserProvider currentUserProvider)
    {
        _unitOfWork = unitOfWork;
        _transactionProvider = transactionProvider;
        _currentUserProvider = currentUserProvider;
    }

    public async Task Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        await _transactionProvider.OpenTransaction(cancellationToken);

        var currentUser = _currentUserProvider.GetCurrentUser();

        if (currentUser is null)
        {
            throw new GuardForbiddenException("Current user is null");
        }

        var dbQuestion = await _unitOfWork.QuestionRepository.FirstOrDefaultByIdAsync(request.Id, cancellationToken);

        if (dbQuestion is null)
        {
            throw new GuardNotFoundException($"Question with id {request.Id} not found");
        }

        if (dbQuestion.AuthorId != currentUser.Id)
        {
            throw new GuardForbiddenException($"Current user with id {currentUser.Id} is not author of question with id {dbQuestion.Id}");
        }

        await _unitOfWork.QuestionRepository.DeleteQuestionAsync(dbQuestion, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _transactionProvider.Commit(cancellationToken);
    }
}
