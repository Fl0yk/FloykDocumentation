using AutoMapper;
using Core.Exceptions;
using Core.Providers.Interfaces;
using Forum.Application.Shared.Models.DTOs;
using Forum.Application.UseCase.Command.Answer;
using Forum.Domain.Abstractions.Repositories;
using MediatR;

namespace Forum.Application.UseCase.CommandHandlers.Answer;

public class UpdateAnswerCommandHandler : IRequestHandler<UpdateAnswerCommand, AnswerDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ITransactionProvider _transactionProvider;
    private readonly IBaseCurrentUserProvider _currentUserProvider;

    public UpdateAnswerCommandHandler(
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

    public async Task<AnswerDTO> Handle(UpdateAnswerCommand request, CancellationToken cancellationToken)
    {
        await _transactionProvider.OpenTransaction(cancellationToken);

        var currentUser = _currentUserProvider.GetCurrentUser();

        if (currentUser is null)
        {
            throw new GuardForbiddenException("Current user is null");
        }

        var dbAnswer = await _unitOfWork.AnswerRepository.FirstOrDefaultByIdAsync(request.Id, cancellationToken);

        if (dbAnswer is null)
        {
            throw new GuardNotFoundException($"Answer with id {request.Id} is not found");
        }

        if (dbAnswer.AuthorId != currentUser.Id)
        {
            throw new GuardForbiddenException($"User with id {currentUser.Id} is not the author of the answer");
        }

        if (dbAnswer.Question!.IsClosed)
        {
            throw new GuardArgumentException($"Question with id {dbAnswer.Question.Id} closed");
        }

        _mapper.Map(request, dbAnswer);

        Guid id = await _unitOfWork.AnswerRepository.UpdateAnswerAsync(dbAnswer, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _transactionProvider.Commit(cancellationToken);

        return _mapper.Map<AnswerDTO>(dbAnswer);
    }
}