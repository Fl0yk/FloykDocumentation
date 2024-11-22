namespace Forum.Presentation.Shared.Models.DTOs.Answer;

public record class AddAnswerRequestDTO(
                        string Text,
                        Guid CurrentUserId,
                        Guid QuestionId,
                        Guid? ParentId);
