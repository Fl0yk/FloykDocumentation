using AutoMapper;
using Forum.Application.Shared.Exceptions;
using Forum.Application.UseCase.Command.Question;
using Forum.Domain.Abstractions.Repositories;
using Forum.Domain.Abstractions.Services;
using MediatR;

using QuestionModel = Forum.Domain.Entities.Question;

namespace Forum.Application.UseCase.CommandHandlers.Question;

public class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, Guid>
{
    private readonly IUserService _userService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateQuestionCommandHandler(IUserService userService, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _userService = userService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
    {
        bool isUserExist = await _userService.IsUserExist(request.AuthorId, cancellationToken);

        if (!isUserExist)
        {
            throw new NotFoundException($"User with id {request.AuthorId} was not found");
        }

        var question = _mapper.Map<QuestionModel>(request);

        Guid id = await _unitOfWork.QuestionRepository.CreateQuestionAsync(question, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return id;
    }
}
