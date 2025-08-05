using PmaApi.Models.Domain;
using TaskStatus = PmaApi.Models.Domain.TaskStatus;

namespace PmaApi.Models.DTOs.Task;

public record TaskUpdateDto
{
    public long Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public DateOnly? StartDate { get; init; }
    public DateOnly? EndDate { get; init; }
    public TaskStatus Status { get; init; }
    public Priority Priority { get; init; }
    public HashSet<long> MemberIds { get; init; } = new();
}