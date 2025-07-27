using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PmaApi.Models.Domain;

[Table("permissions")]
public class Permission : BaseEntity<long>
{
    [StringLength(30)]
    public required string Name { get; set; }
    [StringLength(100)]
    public string? Description { get; set; }
    public ICollection<Role> Roles { get; set; } = new List<Role>();
}