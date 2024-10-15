namespace Forum.Domain.Entities;

public class Question
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid AuthorId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsClosed { get; set; } = false;

    public DateTime DateOfCreation { get; set; } = DateTime.UtcNow.Date;

    public ICollection<Answer> Answers { get; set; } = [];
}
