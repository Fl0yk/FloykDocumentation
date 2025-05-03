namespace Forum.Presentation.Shared.Models.DTOs.Answer;

public record class AddAnswerRequestDTO(
                        string Text,
                        Guid QuestionId,
                        Guid? ParentId);
