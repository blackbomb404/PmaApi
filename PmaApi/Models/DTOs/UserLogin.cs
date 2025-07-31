using System.ComponentModel.DataAnnotations;

namespace Pma.Models.DTOs;

public class UserLogin
{
    [StringLength(256)]
    public required string Username { get; set; }
    [StringLength(256)]
    public required string Password { get; set; }
}