using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PmaApi.Models.Domain;

public class AccessRole : BaseEntity<long>
{
    [StringLength(30)]
    public required string Name { get; set; }
    [StringLength(100)]
    public string? Description { get; set; }
    public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}