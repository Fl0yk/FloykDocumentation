namespace Forum.Application.Shared.Models.DTOs;
public class InnerAnswerDTO
{
    public Guid Id { get; set; }

    public Guid AuthorId { get; set; }

    public Guid? ParentId { get; set; }

    public required string Text { get; set; }

    public DateTime TimeOfCreation { get; set; }
}
