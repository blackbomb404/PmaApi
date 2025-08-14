namespace PmaApi.Models.DTOs.Comment;

public record CommentOutputDto
{
    public long Id { get; init; }
    public required string Content { get; init; }
    public long TaskId { get; init; }
    public required CommentAuthorDto Author { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}

public record CommentAuthorDto
{
    public long Id { get; init; }
    public required string Name { get; init; }
    public required string PhotoUrl { get; init; }
}