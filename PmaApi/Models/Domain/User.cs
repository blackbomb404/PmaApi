using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PmaApi.Models.Domain;

[Table("users")]
public class User : BaseEntity<long>
{
    [StringLength(25)]
    public required string FirstName { get; set; }
    [StringLength(25)]
    public required string LastName { get; set; }
    [EmailAddress]
    [StringLength(256)]
    public required string Email { get; set; }
    [StringLength(256)]
    public required string PasswordHash { get; set; }
    [StringLength(256)]
    public string? PhotoUrl { get; set; }
    
    public long RoleId { get; set; }
    public Role Role { get; set; }
    
    public ICollection<UserProject> UserProjects = new List<UserProject>();
    public ICollection<Project> Projects = new List<Project>();
    
    public ICollection<Task> Tasks = new List<Task>();
}