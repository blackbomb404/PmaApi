using System.ComponentModel.DataAnnotations.Schema;

namespace PmaApi.Models.Domain;

public  class BaseEntity<T>
{
    public T Id { get; init; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public long CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public long? UpdatedBy { get; set; }
    public bool IsActive { get; set; } = true;
}