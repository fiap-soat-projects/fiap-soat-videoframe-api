using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Entities.Page;

[ExcludeFromCodeCoverage]
public class PagedResult<T>
{
    public int Page { get; set; }
    public int Size { get; set; }
    public long TotalCount { get; set; }
    public int TotalPages { get; set; }
    public IEnumerable<T> Items { get; set; } = [];
}
