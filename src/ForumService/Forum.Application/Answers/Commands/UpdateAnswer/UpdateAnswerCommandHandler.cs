using AutoMapper;
using Forum.Application.Shared.Exceptions;
using Forum.Domain.Abstractions.Repositories;
using Forum.Domain.Entities;
using MediatR;

namespace Forum.Application.Answers.Commands.UpdateAnswer;

public class UpdateAnswerCommandHandler : IRequestHandler<UpdateAnswerCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAnswerRepository _answerRepository;
    private readonly IMapper _mapper;

    public UpdateAnswerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _answerRepository = unitOfWork.AnswerRepository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(UpdateAnswerCommand request, CancellationToken cancellationToken)
    {
        Answer? dbAnswer = await _answerRepository.FirstOrDefaultByIdAsync(request.Id, cancellationToken);

        if (dbAnswer is null)
            throw new NotFoundException($"Answer with id {request.Id} is not found");

        _mapper.Map(dbAnswer, request);

        Guid id = await _answerRepository.UpdateAnswerAsync(dbAnswer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return id;
    }
}
