namespace PmaApi.Models.Domain;

public class Task : AuditableEntity
{
    public long Id { get; set; }
    public int Order { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public DateOnly? DueDate { get; set; }
    public long StatusId { get; set; }
    public TaskStatus Status { get; set; }
    public long PriorityId { get; set; }
    public Priority Priority { get; set; }
    public long ProjectId { get; set; }
    public Project Project { get; set; }
    public ICollection<User> Members = new List<User>();
}