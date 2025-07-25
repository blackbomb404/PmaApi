namespace PmaApi.Models.Domain;

public class Project : AuditableEntity
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public long StatusId { get; set; }
    public ProjectStatus Status { get; set; }
    public ICollection<User> Members { get; set; } = new List<User>();
}