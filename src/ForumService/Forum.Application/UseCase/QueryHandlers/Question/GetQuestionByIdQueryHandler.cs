using AutoMapper;
using Core.Exceptions;
using Core.Providers.Interfaces;
using Forum.Application.Shared.Comparators;
using Forum.Application.Shared.Models.DTOs;
using Forum.Application.UseCase.Query.Question;
using Forum.Domain.Abstractions.Repositories;
using MediatR;

namespace Forum.Application.UseCase.QueryHandlers.Question;
public class GetQuestionByIdQueryHandler : IRequestHandler<GetQuestionByIdQuery, QuestionDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IBaseCurrentUserProvider _currentUserProvider;

    public GetQuestionByIdQueryHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper,
        IBaseCurrentUserProvider currentUserProvider)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<QuestionDTO> Handle(GetQuestionByIdQuery request,
                                    CancellationToken cancellationToken)
    {
        var currentUser = _currentUserProvider.GetCurrentUser();

        var dbQuestion = await _unitOfWork.QuestionRepository
                                            .FirstOrDefaultByIdWithAnswersAsync(
                                                    request.Id,
                                                    cancellationToken);

        if (dbQuestion is null)
        {
            throw new GuardNotFoundException($"Question with id {request.Id} not found");
        }

        var author = await _unitOfWork.UserRepository.GetByIdAsync(dbQuestion.AuthorId, cancellationToken);

        if (author is null)
        {
            throw new GuardNotFoundException("Author");
        }

        dbQuestion.Answers = [.. dbQuestion.Answers.Order(new AnswerComparator())];

        var res = _mapper.Map<QuestionDTO>(dbQuestion);
        res.AuthorUsername = author.Username;
        res.PublicAuthorUsername = author.PublicUsername;
        res.IsAuthor = currentUser is not null && currentUser.Id == author.Id;

        foreach (var answer in res.Answers)
        {
            var answAuthor = await _unitOfWork.UserRepository.GetByIdAsync(answer.AuthorId, cancellationToken);
            if (answAuthor is null) continue;

            answer.AuthorUsername = answAuthor.Username;
            answer.PublicAuthorUsername = answAuthor.PublicUsername;
            answer.IsAuthor = currentUser is not null && currentUser.Id == answAuthor.Id;
        }

        return res;
    }
}
