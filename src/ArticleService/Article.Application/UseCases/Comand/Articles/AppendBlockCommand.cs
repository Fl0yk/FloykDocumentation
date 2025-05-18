using Article.Domain.Entities;
using MediatR;

namespace Article.Application.UseCases.Comand.Articles;

public class AppendBlockCommand : IRequest
{
    public Guid ArticleId { get; set; }

    public IEnumerable<BlockInfo> Blocks { get; set; } = null!;
}

public sealed class BlockInfo
{
    public Guid Id { get; set; }

    public BlockType BlockType { get; set; }

    public string Data { get; set; } = null!;
}