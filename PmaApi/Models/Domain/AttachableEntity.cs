using System.ComponentModel.DataAnnotations.Schema;

namespace PmaApi.Models.Domain;

[Table("attachable_entities")]
public abstract class AttachableEntity<T> : BaseEntity<T>
{
    // No specific properties here, it just serves as a common base
    // for entities that can have attachments.
    public ICollection<Attachment> Attachments { get; init; } = new List<Attachment>();
}