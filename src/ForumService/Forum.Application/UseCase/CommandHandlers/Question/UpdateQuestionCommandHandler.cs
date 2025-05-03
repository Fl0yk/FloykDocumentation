using AutoMapper;
using Core.Exceptions;
using Core.Providers.Interfaces;
using Forum.Application.UseCase.Command.Question;
using Forum.Domain.Abstractions.Repositories;
using MediatR;

namespace Forum.Application.UseCase.CommandHandlers.Question;

public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ITransactionProvider _transactionProvider;
    private readonly IBaseCurrentUserProvider _currentUserProvider;

    public UpdateQuestionCommandHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper,
        ITransactionProvider transactionProvider,
        IBaseCurrentUserProvider currentUserProvider)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _transactionProvider = transactionProvider;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<Guid> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
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

        if (dbQuestion.IsClosed)
        {
            throw new GuardArgumentException($"Question with id {request.Id} closed");
        }

        _mapper.Map(request, dbQuestion);

        Guid id = await _unitOfWork.QuestionRepository.UpdateQuestionAsync(dbQuestion, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _transactionProvider.Commit(cancellationToken);

        return id;
    }
}

