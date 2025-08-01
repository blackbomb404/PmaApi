using System.ComponentModel.DataAnnotations;

namespace Pma.Models.DTOs.User;

public record UserUpdateDto
{
    [StringLength(25)]
    public required string FirstName { get; init; }
    [StringLength(25)]
    public required string LastName { get; init; }
    public string? PhoneNumber { get; init; }
    [EmailAddress]
    [StringLength(256)]
    public required string Email { get; init; }
    [StringLength(256)]
    public string? PhotoUrl { get; init; }
    public long JobRoleId { get; init; }
    public long AccessRoleId { get; init; }
}