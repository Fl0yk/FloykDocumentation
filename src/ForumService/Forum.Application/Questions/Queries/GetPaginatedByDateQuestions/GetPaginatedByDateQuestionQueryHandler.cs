using AutoMapper;
using Forum.Application.Shared.Exceptions;
using Forum.Application.Shared.Models;
using Forum.Application.Shared.Models.DTOs;
using Forum.Domain.Abstractions.Repositories;
using Forum.Domain.Entities;
using MediatR;

namespace Forum.Application.Questions.Queries.GetPaginatedByDateQuestions;
public class GetPaginatedByDateQuestionQueryHandler
                : IRequestHandler<GetPaginatedByDateQuestionsQuery, PaginatedResult<QuestionDTO>>
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IMapper _mapper;

    public GetPaginatedByDateQuestionQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _questionRepository = unitOfWork.QuestionRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<QuestionDTO>> Handle(GetPaginatedByDateQuestionsQuery request, 
                                                        CancellationToken cancellationToken)
    {
        IQueryable<Question> questionQuery = await _questionRepository
                                                        .GetQuestionsByDateAsync(cancellationToken);

        Question[] questions = questionQuery
            .Skip(request.PageNumber * request.PageSize)
            .Take(request.PageSize).ToArray();

        if (questions.Length == 0)
            throw new BadRequestException("Get an empty questions page");

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
