using AutoMapper;
using Core.Exceptions;
using Core.Providers.Interfaces;
using Forum.Application.UseCase.Command.Question;
using Forum.Domain.Abstractions.Repositories;
using MediatR;

using QuestionModel = Forum.Domain.Entities.Question;

namespace Forum.Application.UseCase.CommandHandlers.Question;

public class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ITransactionProvider _transactionProvider;
    private readonly IBaseCurrentUserProvider _currentUserProvider;

    public CreateQuestionCommandHandler(
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

    public async Task<Guid> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
    {
        await _transactionProvider.OpenTransaction(cancellationToken);

        var currentUser = _currentUserProvider.GetCurrentUser();

        if (currentUser is null)
        {
            throw new GuardForbiddenException("Current user is null");
        }

        var author = await _unitOfWork.UserRepository.GetByIdAsync(currentUser.Id, cancellationToken);

        if (author is null)
        {
            throw new GuardNotFoundException($"User with id {currentUser.Id} was not found");
        }

        var question = _mapper.Map<QuestionModel>(request);
        question.AuthorId = author.Id;

        Guid id = await _unitOfWork.QuestionRepository.CreateQuestionAsync(question, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _transactionProvider.Commit(cancellationToken);

        return id;
    }
}
