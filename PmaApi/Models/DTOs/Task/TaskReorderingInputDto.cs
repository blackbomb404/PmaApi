namespace PmaApi.Models.DTOs.Task;

public record TaskReorderingInputDto
{
    public long TaskId { get; init; }
    public int Order { get; init; }
}