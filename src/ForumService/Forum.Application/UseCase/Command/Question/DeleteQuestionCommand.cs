using MediatR;

namespace Forum.Application.UseCase.Command.Question;

public sealed class DeleteQuestionCommand : IRequest
{
    public Guid Id { get; init; }
}
