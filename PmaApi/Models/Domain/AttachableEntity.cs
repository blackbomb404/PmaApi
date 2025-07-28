using System.ComponentModel.DataAnnotations.Schema;

namespace PmaApi.Models.Domain;

public abstract class AttachableEntity : BaseEntity
{
    // No specific properties here, it just serves as a common base
    // for entities that can have attachments.
    public ICollection<Attachment> Attachments { get; init; } = new List<Attachment>();
}