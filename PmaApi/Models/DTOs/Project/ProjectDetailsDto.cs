using Pma.Models.DTOs.User;
using PmaApi.Models.Domain;

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
    public ICollection<Domain.Task> Tasks { get; set; } = new List<Domain.Task>();
}