using Forum.Application.Shared.Exceptions;
using Forum.Domain.Abstractions.Repositories;
using Forum.Domain.Entities;
using MediatR;

namespace Forum.Application.Answers.Commands.DeleteAnswer;
public class DeleteAnswerCommandHandler : IRequestHandler<DeleteAnswerCommand>
{
    private readonly IUnitOfWork _uitOfWork;
    private readonly IAnswerRepository _answerRepository;

    public DeleteAnswerCommandHandler(IUnitOfWork unitOfWork)
    {
        _uitOfWork = unitOfWork;
        _answerRepository = unitOfWork.AnswerRepository;
    }

    public async Task Handle(DeleteAnswerCommand request, CancellationToken cancellationToken)
    {
        Answer? dbAnswer = await _answerRepository.FirstOrDefaultByIdWithChildrenAsync(request.Id, cancellationToken);

        if (dbAnswer is null)
            throw new NotFoundException($"Answer with id {request.Id} not found");

        await _answerRepository.DeleteAnswerAsync(dbAnswer, cancellationToken);
        await _uitOfWork.SaveChangesAsync(cancellationToken);
    }
}
