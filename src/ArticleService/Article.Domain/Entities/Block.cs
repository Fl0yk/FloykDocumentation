using Core.Abstractions;

namespace Article.Domain.Entities;

public class Block : BaseEntity
{
    public string Text { get; set; } = string.Empty;

    public required string Type { get; set; }
}
