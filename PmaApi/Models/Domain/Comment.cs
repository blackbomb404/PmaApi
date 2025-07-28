using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PmaApi.Models.Domain;

public class Comment : BaseEntity
{
    [StringLength(100)]
    public required string Content { get; set; }
    public long TaskId { get; set; }
    public Task Task { get; set; }
    public long UserId { get; set; }
    public User User { get; set; }
}