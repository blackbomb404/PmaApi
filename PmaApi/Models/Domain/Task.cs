using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PmaApi.Models.Domain;

public class Task : AttachableEntity
{
    public TaskType Type { get; set; } = TaskType.Task;
    [StringLength(50)]
    public required string Name { get; set; }
    [StringLength(100)]
    public string? Description { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public TaskStatus Status { get; set; }
    public Priority Priority { get; set; }
    public int Order { get; set; }
    public long ProjectId { get; set; }
    public Project Project { get; set; }
    public ICollection<UserTask> UserTasks = new List<UserTask>();
    public ICollection<User> Members = new List<User>();
}