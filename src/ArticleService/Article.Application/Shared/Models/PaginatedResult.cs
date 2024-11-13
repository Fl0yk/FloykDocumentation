namespace Article.Application.Shared.Models;

public class PaginatedResult<T>
{
    public required IEnumerable<T> Items { get; init; }

    public required int CurrentPage { get; init; }

    public required int TotalPages { get; init; }

    public required int PageSize { get; init; }
}
