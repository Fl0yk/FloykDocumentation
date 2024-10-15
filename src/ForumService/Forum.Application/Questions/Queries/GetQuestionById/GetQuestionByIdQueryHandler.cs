using AutoMapper;
using Forum.Application.Shared.Comparators;
using Forum.Application.Shared.Exceptions;
using Forum.Application.Shared.Models.DTOs;
using Forum.Domain.Abstractions.Repositories;
using Forum.Domain.Entities;
using MediatR;

namespace Forum.Application.Questions.Queries.GetQuestionById;
public class GetQuestionByIdQueryHandler : IRequestHandler<GetQuestionByIdQuery, QuestionDTO>
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IMapper _mapper;

    public GetQuestionByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _questionRepository = unitOfWork.QuestionRepository;
        _mapper = mapper;
    }

    public async Task<QuestionDTO> Handle(GetQuestionByIdQuery request, 
                                    CancellationToken cancellationToken)
    {
        Question? dbQuestion = await _questionRepository
                                        .FirstOrDefaultByIdWithAnswersAsync(
                                                request.Id, 
                                                cancellationToken);

        if (dbQuestion is null)
            throw new NotFoundException($"Question with id {request.Id} not found");

        dbQuestion.Answers.Order(new AnswerComparator());

        return _mapper.Map<QuestionDTO>(dbQuestion);
    }
}
