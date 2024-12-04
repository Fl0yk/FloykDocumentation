namespace Forum.Presentation.Shared.Models.DTOs.Answer;

public record class UpdateAnswerRequestDTO(Guid Id, Guid CurrentUserId, string Text);