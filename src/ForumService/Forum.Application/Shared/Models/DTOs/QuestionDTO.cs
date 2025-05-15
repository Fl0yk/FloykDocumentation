namespace Forum.Application.Shared.Models.DTOs;
public class QuestionDTO
{
    public Guid Id { get; set; }

    public Guid AuthorId { get; set; }

    public string PublicAuthorUsername { get; set; } = null!;

    public string AuthorUsername { get; set; } = null!;

    public bool IsAuthor { get; set; }

    public required string Title { get; set; }

    public required string Description { get; set; }

    public bool IsClosed { get; set; }

    public DateTimeOffset DateOfCreation { get; set; }

    public IEnumerable<InnerAnswerDTO> Answers { get; set; } = [];
}
