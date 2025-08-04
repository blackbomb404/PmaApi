namespace PmaApi.Models.DTOs;

public class PagedResources<T>
{
    public required IEnumerable<T> Items { get; init; }
    public required Pagination Pagination { get; init; }
}

public class Pagination
{
    public short PageNumber { get; init; }
    public short PageSize { get; init; }
    public short TotalCount { get; init; }
    public short TotalPages => (short)(PageSize == 0 ? 0 : Math.Ceiling(TotalCount / (double)PageSize));
    
    public bool HasNext => PageNumber < TotalPages;
    public bool HasPrevious => PageNumber > 1;
}