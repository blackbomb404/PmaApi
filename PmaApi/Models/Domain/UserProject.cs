using System.ComponentModel.DataAnnotations.Schema;

namespace PmaApi.Models.Domain;

public class UserProject
{
    public long UserId { get; set; }
    public long ProjectId { get; set; }
}