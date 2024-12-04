namespace Forum.Presentation.Shared.Models.DTOs.Answer;

public class DeleteAnswerRequestDTO
{
    public Guid AnswerId { get; init; }

    public Guid CurrentUserId { get; set; }
}
