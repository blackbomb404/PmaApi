using System.ComponentModel.DataAnnotations;

namespace PmaApi.Models.Domain;

public class Priority
{
    public long Id { get; set; }
    [StringLength(15)]
    public required string Name { get; set; } // Low, Medium, High, Critical
}