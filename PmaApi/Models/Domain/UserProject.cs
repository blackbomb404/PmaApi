using System.ComponentModel.DataAnnotations.Schema;

namespace PmaApi.Models.Domain;

public class UserProject
{
    public long UserId { get; init; }
    public User User { get; init; }
    public long ProjectId { get; init; }
    public Project Project { get; init; }
    public required AccessRole AccessRoleOnProject { get; set; }
}