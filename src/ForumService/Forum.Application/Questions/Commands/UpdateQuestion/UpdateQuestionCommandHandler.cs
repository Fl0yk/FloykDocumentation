using AutoMapper;
using Forum.Application.Shared.Exceptions;
using Forum.Domain.Abstractions.Repositories;
using Forum.Domain.Entities;
using MediatR;

namespace Forum.Application.Questions.Commands.UpdateQuestion;
public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, Guid>
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateQuestionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _questionRepository = unitOfWork.QuestionRepository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        Question? dbQuestion = await _questionRepository.FirstOrDefaultByIdAsync(request.Id, cancellationToken);

        if (dbQuestion is null)
            throw new NotFoundException($"Question with id {request.Id} not found");

        if (dbQuestion.IsClosed)
            throw new BadRequestException($"Question with id {request.Id} closed");

        _mapper.Map(request, dbQuestion);

        Guid id = await _questionRepository.UpdateQuestionAsync(dbQuestion, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return id;
    }
}
