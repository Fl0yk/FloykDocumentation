using AutoMapper;
using Forum.Application.Shared.Exceptions;
using Forum.Domain.Abstractions.Repositories;
using Forum.Domain.Entities;
using MediatR;

namespace Forum.Application.Answers.Commands.CreateAnswer;
public class CreateAnswerCommandHandler : IRequestHandler<CreateAnswerCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAnswerRepository _answerRepository;
    private readonly IMapper _mapper;

    public CreateAnswerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _answerRepository = unitOfWork.AnswerRepository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateAnswerCommand request, CancellationToken cancellationToken)
    {
        Answer answer = _mapper.Map<Answer>(request);

        if (answer.ParentId is not null)
        {
            Answer? dbParent = await _answerRepository.FirstOrDefaultByIdAsync(answer.ParentId.Value, cancellationToken);

            if (dbParent is null)
                throw new BadRequestException($"Parent with id {answer.ParentId} si not found");

            answer.Level = dbParent.Level + 1;
        }

        Guid id = await _answerRepository.CreateAswerAsync(answer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return id;
    }
}
