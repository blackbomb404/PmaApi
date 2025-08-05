using System.ComponentModel.DataAnnotations;

namespace Pma.Models.DTOs;

public class UserLogin
{
    [StringLength(256)]
    public required string Email { get; set; }
    [StringLength(256)]
    public required string Password { get; set; }
}