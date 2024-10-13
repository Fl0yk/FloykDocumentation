namespace Forum.Domain.Entities;

public class Answer
{
    public int Id { get; set; }

    public Guid QuestionId { get; set; }

    public int? ParentId { get; set; }

    public string Text { get; set; } = string.Empty;
}

