namespace Forum.Application.Shared.Models.DTOs;
public class QuestionDTO
{
    public Guid Id { get; set; }

    public Guid AuthorId { get; set; }

    public required string Title { get; set; }

    public required string Description { get; set; }

    public bool IsClosed { get; set; }

    public DateTime DateOfCreation { get; set; }

    public IEnumerable<InnerAnswerDTO> Answers { get; set; } = [];
}
