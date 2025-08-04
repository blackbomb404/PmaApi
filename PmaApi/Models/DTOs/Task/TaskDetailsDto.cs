using Pma.Models.DTOs.User;
using PmaApi.Models.Domain;
using TaskStatus = PmaApi.Models.Domain.TaskStatus;

namespace PmaApi.Models.DTOs.Task;

public record TaskDetailsDto
{
    public long Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public DateOnly? StartDate { get; init; }
    public DateOnly? EndDate { get; init; }
    public TaskStatus Status { get; init; }
    public Priority Priority { get; init; }
    public long ProjectId { get; init; }
    public IEnumerable<UserOverviewDto> Members = new List<UserOverviewDto>();
}