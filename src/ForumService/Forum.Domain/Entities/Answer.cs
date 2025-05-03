using Core.Abstractions;

namespace Forum.Domain.Entities;

public class Answer : BaseEntity
{
    public Guid AuthorId { get; set; }

    public User Author { get; set; } = null!;

    public Guid QuestionId { get; set; }

    public Question? Question { get; set; }

    public Guid? ParentId { get; set; }

    public Answer? Parent { get; set; }

    public ICollection<Answer> Childrens { get; set; } = [];

    public string Text { get; set; } = string.Empty;

    public int Level { get; set; }
}

