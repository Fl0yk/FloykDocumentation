namespace Forum.Presentation.Shared.Models.DTOs.Question;

public record class CreateQuestionRequestDTO(
                            string Title,
                            string Description,
                            Guid CurrentUserId);