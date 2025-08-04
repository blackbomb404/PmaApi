using Pma.Models.DTOs.User;
using PmaApi.Models.Domain;
using Task = PmaApi.Models.Domain.Task;

namespace Pma.Models.DTOs.Project;

public record ProjectListDto
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public ProjectStatus Status { get; set; }
    public IEnumerable<UserOverviewDto> Members { get; set; } = new List<UserOverviewDto>();
}