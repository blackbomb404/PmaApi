namespace PmaApi.Models.DTOs.Task;

public record ProjectAssignmentInputDto
{
    public long ProjectId { get; set; }
    public HashSet<long> MemberIds { get; set; } = new();
}