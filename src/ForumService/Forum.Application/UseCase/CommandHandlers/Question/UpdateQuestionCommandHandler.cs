using AutoMapper;
using Forum.Application.Shared.Exceptions;
using Forum.Application.UseCase.Command.Question;
using Forum.Domain.Abstractions.Repositories;
using MediatR;

namespace Forum.Application.UseCase.CommandHandlers.Question;

public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateQuestionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        var dbQuestion = await _unitOfWork.QuestionRepository.FirstOrDefaultByIdAsync(request.Id, cancellationToken);

        if (dbQuestion is null)
        {
            throw new NotFoundException($"Question with id {request.Id} not found");
        }

        if (dbQuestion.IsClosed)
        {
            throw new BadRequestException($"Question with id {request.Id} closed");
        }

        _mapper.Map(request, dbQuestion);

        Guid id = await _unitOfWork.QuestionRepository.UpdateQuestionAsync(dbQuestion, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return id;
    }
}

