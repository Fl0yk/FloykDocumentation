using Article.Domain.Entities;

namespace Article.Presentation.Shared.Models.DTOs.Article;

public record class AppendBlockRequestDTO(string Text, BlockType BlockType, Guid ArticleId);
