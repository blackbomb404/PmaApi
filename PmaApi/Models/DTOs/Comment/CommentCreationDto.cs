namespace PmaApi.Models.DTOs.Comment;

public record CommentCreationDto
{
    public required string Content { get; init; }
    public long TaskId { get; init; }
    public long UserId { get; init; }
}