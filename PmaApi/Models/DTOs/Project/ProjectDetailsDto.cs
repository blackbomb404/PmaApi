using Pma.Models.DTOs.User;
using PmaApi.Models.Domain;
using PmaApi.Models.DTOs.Task;

namespace PmaApi.Models.DTOs.Project;

public record ProjectDetailsDto
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public ProjectStatus Status { get; set; }
    public IEnumerable<UserOverviewDto> Members { get; set; } = new List<UserOverviewDto>();
    public IEnumerable<TaskListDto> Tasks { get; set; } = new List<TaskListDto>();
}