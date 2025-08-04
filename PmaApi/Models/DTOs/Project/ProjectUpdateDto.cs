using PmaApi.Models.Domain;
using Task = System.Threading.Tasks.Task;

namespace Pma.Models.DTOs.Project;

public record ProjectUpdateDto
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public ProjectStatus Status { get; set; }
    public HashSet<long> MemberIds { get; set; } = new();
}