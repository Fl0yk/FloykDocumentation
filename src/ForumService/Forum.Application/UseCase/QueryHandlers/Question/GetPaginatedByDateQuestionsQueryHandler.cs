using AutoMapper;
using Forum.Application.Shared.Exceptions;
using Forum.Application.Shared.Models.DTOs;
using Forum.Application.Shared.Models;
using Forum.Domain.Abstractions.Repositories;
using MediatR;
using Forum.Application.UseCase.Query.Question;

using QuestionModel = Forum.Domain.Entities.Question;

namespace Forum.Application.UseCase.QueryHandlers.Question;

public class GetPaginatedByDateQuestionQueryHandler
                : IRequestHandler<GetPaginatedByDateQuestionsQuery, PaginatedResult<QuestionDTO>>
{
    private IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPaginatedByDateQuestionQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<QuestionDTO>> Handle(GetPaginatedByDateQuestionsQuery request,
                                                        CancellationToken cancellationToken)
    {
        IQueryable<QuestionModel> questionQuery = await _unitOfWork.QuestionRepository.GetQuestionsByDateAsync(cancellationToken);

        var questions = questionQuery
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize).ToArray();

        if (questions.Length == 0)
        {
            throw new BadRequestException("Get an empty questions page");
        }

        int count = questionQuery.Count();

        return new()
        {
            Items = _mapper.Map<IEnumerable<QuestionDTO>>(questions),
            TotalPages = (int)Math.Ceiling((double)count / request.PageSize),
            CurrentPage = request.PageNumber,
            PageSize = request.PageSize
        };

    }
}
