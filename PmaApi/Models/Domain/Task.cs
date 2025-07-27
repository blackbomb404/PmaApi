using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PmaApi.Models.Domain;

[Table("tasks")]
public class Task : AttachableEntity<long>
{
    public int Order { get; set; }
    [StringLength(50)]
    public required string Name { get; set; }
    [StringLength(100)]
    public string? Description { get; set; }
    public DateOnly? DueDate { get; set; }
    public TaskStatus Status { get; set; }
    public Priority Priority { get; set; }
    public long ProjectId { get; set; }
    public Project Project { get; set; }
    public ICollection<User> Members = new List<User>();
}