using PmaApi.Models.Domain;

namespace PmaApi.Models.DTOs.Task;

public record TaskCreateDto
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public DateOnly? StartDate { get; init; }
    public DateOnly? EndDate { get; init; }
    public Priority Priority { get; init; }
    public int Order { get; init; }
    public long ProjectId { get; init; }
    public HashSet<long> MemberIds = new();
}