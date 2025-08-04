namespace PmaApi.Models.DTOs;

public class QueryParameters
{
    public short PageNumber { get; init; } = 1;
    public short PageSize { get; init; } = 10;
    // public string? SortBy { get; set; }
    // public string? SortDirection { get; set; }
}