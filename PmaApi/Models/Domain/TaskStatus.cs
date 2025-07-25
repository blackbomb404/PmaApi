using System.ComponentModel.DataAnnotations;

namespace PmaApi.Models.Domain;

public class TaskStatus
{
    public long Id { get; set; }
    [StringLength(30)]
    public required string Name { get; set; } // Not Started, In Progress, Completed
}