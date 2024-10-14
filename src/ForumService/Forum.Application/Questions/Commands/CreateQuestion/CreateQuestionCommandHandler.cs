using AutoMapper;
using Forum.Domain.Abstractions.Repositories;
using Forum.Domain.Entities;
using MediatR;

namespace Forum.Application.Questions.Commands.CreateQuestion;
public class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, Guid>
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateQuestionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _questionRepository = unitOfWork.QuestionRepository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
    {
        Question question = _mapper.Map<Question>(request);

        Guid id = await _questionRepository.CreateQuestionAsync(question, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return id;
    }
}
