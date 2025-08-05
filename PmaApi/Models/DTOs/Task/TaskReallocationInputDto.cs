namespace PmaApi.Models.DTOs.Task;

public record TaskReallocationInputDto
{
    public long TaskId { get; set; }
    public long ProjectId { get; set; }
}