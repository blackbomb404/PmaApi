namespace PmaApi.Models.DTOs.Comment;

public record CommentUpdateDto
{
    public long Id { get; init; }
    public required string Content { get; init; }
}