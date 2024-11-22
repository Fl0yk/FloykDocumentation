using MediatR;

namespace Forum.Application.UseCase.Command.Question;

//public record class CreateQuestionCommand(
//                string Title,
//                string Description,
//                Guid AuthorId) : IRequest<Guid>;

public class CreateQuestionCommand : IRequest<Guid>
{
    public Guid AuthorId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }
}

