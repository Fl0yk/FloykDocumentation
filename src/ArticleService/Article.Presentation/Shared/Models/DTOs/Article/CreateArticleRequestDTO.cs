namespace Article.Presentation.Shared.Models.DTOs.Article;

public record class CreateArticleRequestDTO(Guid Id, string Title, string ShortDescription, Guid CategoryId);
