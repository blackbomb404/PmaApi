using System.ComponentModel.DataAnnotations;

namespace PmaApi.Models.Domain;

public class User : AuditableEntity
{
    public long Id { get; set; }
    [StringLength(25)]
    public required string FirstName { get; set; }
    [StringLength(25)]
    public required string LastName { get; set; }
    [EmailAddress]
    [StringLength(256)]
    public required string Email { get; set; }
    [StringLength(256)]
    public required string PasswordHash { get; set; }
    public string? PhotoUrl { get; set; }
    public long RoleId { get; set; }
    public Role Role { get; set; }
    
    public ICollection<Task> Tasks = new List<Task>();
    
    // Attachment (id, task_id?, project_id?, user_id, file_name, file_url, uploaded_at)
    // 
    // For files related to tasks or projects. task_id? and project_id? would imply an attachment could be linked to either.
    
    
    // User-Project Relationships
    // How do users relate to projects beyond just creating them or being assigned tasks?
    // 
    // User_Project (user_id, project_id, role_on_project_id)
    // 
    // This entity would define a user's specific role within a project (e.g., a user might be an "Admin" for the app but a "Team Member" on a specific project).
    // 
    // You might need a separate ProjectRole entity (e.g., ProjectRole (id, name) like "Project Manager", "Developer", "Client").
}