using System.ComponentModel.DataAnnotations.Schema;

namespace PmaApi.Models.Domain;

public class UserProject
{
    public long UserId { get; init; }
    public User User { get; init; }
    public long ProjectId { get; init; }
    public Project Project { get; init; }
    public required Role RoleOnProject { get; set; }
}