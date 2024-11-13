namespace Article.Presentation.Shared.Models.DTOs.Article;

public record class DeleteBlockRequestDTO(Guid ArticleId, Guid BlockId, string AuthorName);
