using AutoMapper;
using Forum.Application.Shared.Exceptions;
using Forum.Application.UseCase.Command.Answer;
using Forum.Domain.Abstractions.Repositories;
using MediatR;

using AnswerModel = Forum.Domain.Entities.Answer;

namespace Forum.Application.UseCase.CommandHandlers.Answer;

public class AddAnswerCommandHandler : IRequestHandler<AddAnswerCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddAnswerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(AddAnswerCommand request, CancellationToken cancellationToken)
    {
        var dbQuestion = await _unitOfWork.QuestionRepository.FirstOrDefaultByIdAsync(request.QuestionId);

        if (dbQuestion is null)
        {
            throw new NotFoundException($"To create an answer, question with id {request.QuestionId} was not found");
        }

        if (dbQuestion.IsClosed)
        {
            throw new BadRequestException($"Question with id {request.QuestionId} closed");
        }

        var answer = _mapper.Map<AnswerModel>(request);

        if (answer.ParentId is not null)
        {
            var dbParent = await _unitOfWork.AnswerRepository.FirstOrDefaultByIdAsync(answer.ParentId.Value, cancellationToken);

            if (dbParent is null)
            {
                throw new BadRequestException($"Parent with id {answer.ParentId} si not found");
            }

            answer.Level = dbParent.Level + 1;
        }

        Guid id = await _unitOfWork.AnswerRepository.CreateAswerAsync(answer, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return id;
    }
}
