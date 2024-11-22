using MediatR;

namespace Forum.Application.UseCase.Command.Answer;

//public record class AddAnswerCommand(
//                        string Text,
//                        Guid AuthorId,
//                        Guid QuestionId,
//                        Guid? ParentId) : IRequest<Guid>;

public class AddAnswerCommand : IRequest<Guid>
{
    public string Text { get; set; }

    public Guid AuthorId { get; set; }

    public Guid QuestionId { get; set; }

    public Guid? ParentId { get; set; }
}

