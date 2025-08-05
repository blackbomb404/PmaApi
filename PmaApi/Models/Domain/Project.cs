using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PmaApi.Models.Domain;

public class Project : AttachableEntity
{
    [StringLength(30)]
    public required string Name { get; set; }
    [StringLength(50)]
    public string? Description { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public ProjectStatus Status { get; set; }
    public ICollection<UserProject> UserProjects { get; set; } = new List<UserProject>();
    public ICollection<User> Members { get; set; } = new List<User>();
    public ICollection<Task> Tasks { get; set; } = new List<Task>();
}