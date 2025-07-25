namespace PmaApi.Models.Domain;

public class Comment : AuditableEntity
{
    public long Id { get; set; }
    public required string Content { get; set; }
    public long TaskId { get; set; }
    public Task Task { get; set; }
    public long UserId { get; set; }
    public User User { get; set; }
}