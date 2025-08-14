using System.ComponentModel.DataAnnotations.Schema;

namespace PmaApi.Models.Domain;

public  class BaseEntity
{
    public long Id { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public long CreatedBy { get; init; }
    public DateTime? UpdatedAt { get; set; }
    public long? UpdatedBy { get; set; }
    public bool IsActive { get; set; } = true;
}