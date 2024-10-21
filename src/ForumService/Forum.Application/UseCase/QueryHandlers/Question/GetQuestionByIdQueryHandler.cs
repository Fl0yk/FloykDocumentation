using AutoMapper;
using Forum.Application.Shared.Comparators;
using Forum.Application.Shared.Exceptions;
using Forum.Application.Shared.Models.DTOs;
using Forum.Application.UseCase.Query.Question;
using Forum.Domain.Abstractions.Repositories;
using MediatR;

namespace Forum.Application.UseCase.QueryHandlers.Question;
public class GetQuestionByIdQueryHandler : IRequestHandler<GetQuestionByIdQuery, QuestionDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetQuestionByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<QuestionDTO> Handle(GetQuestionByIdQuery request,
                                    CancellationToken cancellationToken)
    {
        var dbQuestion = await _unitOfWork.QuestionRepository
                                            .FirstOrDefaultByIdWithAnswersAsync(
                                                    request.Id,
                                                    cancellationToken);

        if (dbQuestion is null)
        {
            throw new NotFoundException($"Question with id {request.Id} not found");
        }

        dbQuestion.Answers = [.. dbQuestion.Answers.Order(new AnswerComparator())];

        return _mapper.Map<QuestionDTO>(dbQuestion);
    }
}
