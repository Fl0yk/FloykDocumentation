using MediatR;

namespace Forum.Application.UseCase.Command.Question;

public class CreateQuestionCommand : IRequest<Guid>
{
    public Guid AuthorId { get; set; }

    public required string Title { get; set; }

    public required string Description { get; set; }
}

