using MediatR;

namespace Forum.Application.UseCase.Command.Question;

public sealed class CloseQuestionCommand : IRequest<Guid>
{
    public Guid Id { get; init; }
}