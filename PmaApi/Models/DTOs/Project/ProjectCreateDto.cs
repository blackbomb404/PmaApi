using System.ComponentModel.DataAnnotations;
using PmaApi.Models.Domain;
using Task = PmaApi.Models.Domain.Task;
using User = PmaApi.Models.Domain.User;

namespace Pma.Models.DTOs.Project;

public record ProjectCreateDto
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public DateOnly? StartDate { get; init; }
    public DateOnly? EndDate { get; init; }
    public ProjectStatus Status { get; init; }
    public HashSet<long> MemberIds { get; init; } = new();
}