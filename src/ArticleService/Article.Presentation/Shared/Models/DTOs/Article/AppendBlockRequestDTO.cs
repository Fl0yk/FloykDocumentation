using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Article.Presentation.Shared.Models.DTOs.Article;

public class AppendBlockRequestDTO : IRequest
{
    public Guid ArticleId { get; set; }

    public string Blocks { get; set; } = null!;

    [FromForm(Name = "Files")]
    public List<IFormFile> Files { get; set; } = new List<IFormFile>();
}

