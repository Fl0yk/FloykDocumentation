using Core.Abstractions;

namespace Article.Domain.Entities;

public class Block : BaseEntity
{
    public string Data { get; set; } = null!;

    public BlockType Type { get; set; }
}
