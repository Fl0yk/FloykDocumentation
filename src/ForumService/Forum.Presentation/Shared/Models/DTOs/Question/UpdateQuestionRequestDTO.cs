namespace Forum.Presentation.Shared.Models.DTOs.Question;

public record class UpdateQuestionRequestDTO(
                        Guid Id,
                        string Title,
                        string Description);
