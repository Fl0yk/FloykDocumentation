using AutoMapper;
using Forum.Application.Shared.Exceptions;
using Forum.Application.Shared.Models.DTOs;
using Forum.Domain.Abstractions.Repositories;
using Forum.Domain.Entities;
using MediatR;

namespace Forum.Application.Answers.Queries;
public class GetAnswerByIdQueryHandler : IRequestHandler<GetAnswerByIdQuery, AnswerDTO>
{
    private readonly IAnswerRepository _answerRepository;
    private readonly IMapper _mapper;

    public GetAnswerByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _answerRepository = unitOfWork.AnswerRepository;
        _mapper = mapper;
    }

    public async Task<AnswerDTO> Handle(GetAnswerByIdQuery request, CancellationToken cancellationToken)
    {
        Answer? dbAnswer = await _answerRepository.FirstOrDefaultByIdAsync(request.Id, cancellationToken);

        if (dbAnswer is null)
            throw new NotFoundException($"Answer with id {request.Id} not found");

        return _mapper.Map<AnswerDTO>(dbAnswer);
    }
}
