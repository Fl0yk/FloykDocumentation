using Core.Abstractions;

namespace Forum.Domain.Entities;

public class Question : BaseEntity
{
    public Guid AuthorId { get; set; }

    public User Author { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public bool IsClosed { get; set; } = false;

    public ICollection<Answer> Answers { get; set; } = null!;
}
