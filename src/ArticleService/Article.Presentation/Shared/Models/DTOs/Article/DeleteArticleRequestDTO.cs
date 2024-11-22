namespace Article.Presentation.Shared.Models.DTOs.Article;

public record class DeleteArticleRequestDTO(Guid Id, string CurrentUserName);
