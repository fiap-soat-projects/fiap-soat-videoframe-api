using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities.Page;

[ExcludeFromCodeCoverage]
public class Pagination<T>
{
    public int Page { get; init; }
    public int Size { get; init; }
    public long TotalCount { get; init; }
    public int TotalPages { get; init; }
    public IEnumerable<T> Items { get; init; } = [];
}