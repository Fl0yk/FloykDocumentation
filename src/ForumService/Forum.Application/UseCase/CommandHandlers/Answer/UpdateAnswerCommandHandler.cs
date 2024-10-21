using AutoMapper;
using Forum.Application.Shared.Exceptions;
using Forum.Application.UseCase.Command.Answer;
using Forum.Domain.Abstractions.Repositories;
using MediatR;

namespace Forum.Application.UseCase.CommandHandlers.Answer;

public class UpdateAnswerCommandHandler : IRequestHandler<UpdateAnswerCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateAnswerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(UpdateAnswerCommand request, CancellationToken cancellationToken)
    {
        var dbAnswer = await _unitOfWork.AnswerRepository.FirstOrDefaultByIdAsync(request.Id, cancellationToken);

        if (dbAnswer is null)
        {
            throw new NotFoundException($"Answer with id {request.Id} is not found");
        }

        if (dbAnswer.Question!.IsClosed)
        {
            throw new BadRequestException($"Question with id {dbAnswer.Question.Id} closed");
        }

        _mapper.Map(request, dbAnswer);

        Guid id = await _unitOfWork.AnswerRepository.UpdateAnswerAsync(dbAnswer, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return id;
    }
}