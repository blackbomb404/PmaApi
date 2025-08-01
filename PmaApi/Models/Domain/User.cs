using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PmaApi.Models.Domain;

public class User : BaseEntity
{
    [StringLength(25)]
    public required string FirstName { get; set; }
    [StringLength(25)]
    public required string LastName { get; set; }
    [StringLength(20)]
    public string? PhoneNumber { get; set; }
    [EmailAddress]
    [StringLength(256)]
    public required string Email { get; set; }
    [StringLength(256)]
    public string? PasswordHash { get; set; }
    [StringLength(256)]
    public string? PhotoUrl { get; set; }
    public DateTime LastLoginAt { get; set; }
    public long JobRoleId { get; set; }
    public JobRole JobRole { get; set; }
    
    public long AccessRoleId { get; set; }
    public AccessRole AccessRole { get; set; }
    
    public ICollection<Project> Projects = new List<Project>();
    
    public ICollection<Task> Tasks = new List<Task>();
}