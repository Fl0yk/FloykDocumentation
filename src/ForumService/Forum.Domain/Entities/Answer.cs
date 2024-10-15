namespace Forum.Domain.Entities;

public class Answer
{
    public Guid Id { get; set; }

    public Guid QuestionId { get; set; }

    public Guid? ParentId { get; set; }

    public string Text { get; set; } = string.Empty;

    public DateTime TimeOfCreation { get; set; } = DateTime.UtcNow;

    public int Level { get; set; }
}

