using AutoMapper;
using Core.Exceptions;
using Core.Providers.Interfaces;
using Forum.Application.Shared.Models.DTOs;
using Forum.Application.UseCase.Command.Answer;
using Forum.Domain.Abstractions.Repositories;
using MediatR;
using AnswerModel = Forum.Domain.Entities.Answer;

namespace Forum.Application.UseCase.CommandHandlers.Answer;

public class AddAnswerCommandHandler : IRequestHandler<AddAnswerCommand, AnswerDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ITransactionProvider _transactionProvider;
    private readonly IBaseCurrentUserProvider _currentUserProvider;

    public AddAnswerCommandHandler(
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

    public async Task<AnswerDTO> Handle(AddAnswerCommand request, CancellationToken cancellationToken)
    {
        await _transactionProvider.OpenTransaction(cancellationToken);

        var currentUser = _currentUserProvider.GetCurrentUser();

        if (currentUser is null)
        {
            throw new GuardForbiddenException("Current user is null");
        }

        var dbQuestion = await _unitOfWork.QuestionRepository.FirstOrDefaultByIdAsync(request.QuestionId);

        if (dbQuestion is null)
        {
            throw new GuardNotFoundException($"To create an answer, question with id {request.QuestionId} was not found");
        }

        if (dbQuestion.IsClosed)
        {
            throw new GuardArgumentException($"Question with id {request.QuestionId} closed");
        }

        var author = await _unitOfWork.UserRepository.GetByIdAsync(currentUser.Id, cancellationToken);

        if (author is null)
        {
            throw new GuardNotFoundException($"User with id {currentUser.Id} was not found");
        }

        var answer = _mapper.Map<AnswerModel>(request);
        answer.AuthorId = author.Id;

        if (answer.ParentId is not null)
        {
            var dbParent = await _unitOfWork.AnswerRepository.FirstOrDefaultByIdAsync(answer.ParentId.Value, cancellationToken);

            if (dbParent is null)
            {
                throw new GuardArgumentException($"Parent with id {answer.ParentId} si not found");
            }

            answer.Level = dbParent.Level + 1;
        }

        await _unitOfWork.AnswerRepository.CreateAnswerAsync(answer, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _transactionProvider.Commit(cancellationToken);

        var res = _mapper.Map<AnswerDTO>(answer);
        res.AuthorUsername = author.Username;
        res.PublicAuthorUsername = author.PublicUsername;
        res.IsAuthor = true;

        return res;
    }
}
