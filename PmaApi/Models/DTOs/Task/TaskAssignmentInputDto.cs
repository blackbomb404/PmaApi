namespace PmaApi.Models.DTOs.Task;

public record TaskAssignmentInputDto
{
    public long TaskId { get; set; }
    public HashSet<long> MemberIds { get; set; } = new();
}