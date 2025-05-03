namespace Article.Application.Shared.Models.DTOs;

public class ShortArticleDTO
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    //TODO: мб инфо об авторе?
    public Guid AuthorId { get; set; }

    public bool IsPublished { get; set; }

    public DateTime DateOfPublication { get; set; }
}
