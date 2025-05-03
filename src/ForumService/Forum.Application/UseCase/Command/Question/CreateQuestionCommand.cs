using MediatR;

namespace Forum.Application.UseCase.Command.Question;

public sealed class CreateQuestionCommand : IRequest<Guid>
{
    public string Title { get; init; } = null!;

    public string Description { get; init; } = null!;
}

