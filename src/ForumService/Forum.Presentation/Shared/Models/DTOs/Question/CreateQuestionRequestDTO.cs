namespace Forum.Presentation.Shared.Models.DTOs.Question;

public record CreateQuestionRequestDTO(
                            string Title,
                            string Description,
                            Guid AuthorId);