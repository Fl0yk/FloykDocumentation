using MediatR;

namespace Forum.Application.UseCase.Command.Question;

public sealed class UpdateQuestionCommand : IRequest<Guid>
{
    public Guid Id { get; init; }

    public string Title { get; init; } = null!;

    public string Description { get; init; } = null!;
}
