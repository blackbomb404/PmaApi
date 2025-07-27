using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PmaApi.Models.Domain;

public class Attachment : BaseEntity<long>
{
    public long AttachableEntityId { get; set; }
    public AttachableEntity<long> AttachableEntity { get; set; }
    public long? UserId { get; set; }
    public User? User { get; set; }
    [StringLength(255)]
    public required string FileName { get; set; }
    [StringLength(100)]
    public required string FileMimeType { get; set; }
    [StringLength(2048)]
    public required string FileUrl { get; set; }
    public long FileSizeInBytes { get; set; }
    [StringLength(100)]
    public string? Note { get; set; }
    public DateTime UploadedAt { get; init; } = DateTime.UtcNow;
}