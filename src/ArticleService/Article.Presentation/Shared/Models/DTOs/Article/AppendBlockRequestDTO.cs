using MediatR;

namespace Article.Presentation.Shared.Models.DTOs.Article;

public record class AppendBlockRequestDTO(string Text, string BlockType, Guid ArticleId, string CurrentUserName);
