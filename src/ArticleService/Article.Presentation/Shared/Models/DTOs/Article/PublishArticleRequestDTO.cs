namespace Article.Presentation.Shared.Models.DTOs.Article;

public record class PublishArticleRequestDTO(Guid ArticleId, string CurrentUserName);
