namespace Article.Presentation.Shared.Models.DTOs.Article;

public record class CreateArticleRequestDTO(string Title, string CurrentUserName, Guid CategoryId);
