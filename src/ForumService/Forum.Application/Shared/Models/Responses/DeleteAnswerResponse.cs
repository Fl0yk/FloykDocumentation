namespace Forum.Application.Shared.Models.Responses;

public class DeleteAnswerResponse
{
    public Guid AnswerId { get; init; }

    public Guid QuestionId { get; init; }
}
