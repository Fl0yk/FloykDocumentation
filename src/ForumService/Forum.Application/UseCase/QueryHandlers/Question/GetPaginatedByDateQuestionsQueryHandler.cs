using AutoMapper;
using Core.Exceptions;
using Core.Models;
using Core.Providers.Interfaces;
using Forum.Application.Shared.Models.DTOs;
using Forum.Application.UseCase.Query.Question;
using Forum.Domain.Abstractions.Repositories;
using MediatR;
using QuestionModel = Forum.Domain.Entities.Question;

namespace Forum.Application.UseCase.QueryHandlers.Question;

public class GetPaginatedByDateQuestionQueryHandler
                : IRequestHandler<GetPaginatedByDateQuestionsQuery, PaginatedResult<QuestionDTO>>
{
    private IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IBaseCurrentUserProvider _currentUserProvider;

    public GetPaginatedByDateQuestionQueryHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper,
        IBaseCurrentUserProvider currentUserProvider)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<PaginatedResult<QuestionDTO>> Handle(GetPaginatedByDateQuestionsQuery request,
                                                        CancellationToken cancellationToken)
    {
        var currentUser = _currentUserProvider.GetCurrentUser();
        IQueryable<QuestionModel> questionQuery = await _unitOfWork.QuestionRepository.GetQuestionsByDateAsync(cancellationToken);

        var questions = questionQuery
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize).ToArray();

        if (questions.Length == 0)
        {
            throw new GuardArgumentException("Get an empty questions page");
        }

        int count = questionQuery.Count();

        var items = _mapper.Map<IEnumerable<QuestionDTO>>(questions);
        foreach (var item in items)
        {
            //TODO: rewrite to dictionary
            var answAuthor = await _unitOfWork.UserRepository.GetByIdAsync(item.AuthorId, cancellationToken);
            if (answAuthor is null) continue;

            item.AuthorUsername = answAuthor.Username;
            item.PublicAuthorUsername = answAuthor.PublicUsername;
            item.IsAuthor = currentUser is not null && currentUser.Id == answAuthor.Id;
        }

        return new()
        {
            Items = items,
            TotalPages = (int)Math.Ceiling((double)count / request.PageSize),
            CurrentPage = request.PageNumber,
            PageSize = request.PageSize
        };

    }
}
