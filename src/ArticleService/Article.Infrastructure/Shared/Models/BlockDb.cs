namespace Article.Infrastructure.Shared.Models;

public class BlockDb
{
    public Guid Id { get; set; }

    public string Text { get; set; } = string.Empty;

    public required string Type { get; set; }
}
