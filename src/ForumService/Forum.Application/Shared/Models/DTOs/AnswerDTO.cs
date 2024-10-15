namespace Forum.Application.Shared.Models.DTOs;
public class AnswerDTO
{
    public Guid Id { get; set; }

    public Guid QuestionId { get; set; }

    public Guid? ParentId { get; set; }

    public required string Text { get; set; }

    public DateTime TimeOfCreation { get; set; }

    public int Level { get; set; }
}
