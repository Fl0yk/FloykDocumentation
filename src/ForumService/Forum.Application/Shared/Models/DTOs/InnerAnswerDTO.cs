namespace Forum.Application.Shared.Models.DTOs;
public class InnerAnswerDTO
{
    public Guid Id { get; set; }

    public Guid AuthorId { get; set; }

    public string PublicAuthorUsername { get; set; } = null!;

    public string AuthorUsername { get; set; } = null!;

    public bool IsAuthor { get; set; }

    public Guid? ParentId { get; set; }

    public required string Text { get; set; }

    public DateTimeOffset TimeOfCreation { get; set; }
}
