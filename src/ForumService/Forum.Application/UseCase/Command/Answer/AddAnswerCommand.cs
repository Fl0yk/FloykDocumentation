using MediatR;

namespace Forum.Application.UseCase.Command.Answer;

public class AddAnswerCommand : IRequest<Guid>
{
    public required string Text { get; set; }

    public Guid AuthorId { get; set; }

    public Guid QuestionId { get; set; }

    public Guid? ParentId { get; set; }
}

