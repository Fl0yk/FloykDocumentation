using AutoMapper;
using Forum.Application.Shared.Exceptions;
using Forum.Application.Shared.Models.DTOs;
using Forum.Application.UseCase.Query.Answer;
using Forum.Domain.Abstractions.Repositories;
using MediatR;

namespace Forum.Application.UseCase.QueryHandlers.Answer;

public class GetAnswerByIdQueryHandler : IRequestHandler<GetAnswerByIdQuery, AnswerDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAnswerByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AnswerDTO> Handle(GetAnswerByIdQuery request, CancellationToken cancellationToken)
    {
        var dbAnswer = await _unitOfWork.AnswerRepository.FirstOrDefaultByIdAsync(request.Id, cancellationToken);

        if (dbAnswer is null)
        {
            throw new NotFoundException($"Answer with id {request.Id} not found");
        }

        return _mapper.Map<AnswerDTO>(dbAnswer);
    }
}
