using System.ComponentModel.DataAnnotations;

namespace PmaApi.Models.Domain;

public class JobRole : BaseEntity
{
    [StringLength(30)]
    public required string Name { get; set; }
    [StringLength(100)]
    public string? Description { get; set; }
}