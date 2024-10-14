using AutoMapper;
using Forum.Application.Shared.Models.DTOs;
using Forum.Domain.Abstractions.Repositories;
using Forum.Domain.Entities;
using Forum.Domain.Models;
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

    public async Task<PaginatedResult<QuestionDTO>> Handle(GetPaginatedByDateQuestionsQuery request, CancellationToken cancellationToken)
    {
        PaginatedResult<Question> paginatedQuestion = await _questionRepository
                                                                .GetPaginetedByDateQuestionsAsync(
                                                                                request.PageSize, 
                                                                                request.PageNumber, 
                                                                                cancellationToken);

        if (!paginatedQuestion.Items.Any())
            throw new InvalidOperationException("Get an empty questions page");

        return _mapper.Map<PaginatedResult<QuestionDTO>>(paginatedQuestion);
    }
}
