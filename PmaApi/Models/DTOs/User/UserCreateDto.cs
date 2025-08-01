using System.ComponentModel.DataAnnotations;
using PmaApi.Models.Domain;
using Task = System.Threading.Tasks.Task;

namespace Pma.Models.DTOs.User;

public record UserCreateDto
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
    public required string Password { get; init; }
    [StringLength(256)]
    public string? PhotoUrl { get; init; }
    public long JobRoleId { get; init; }
    public long AccessRoleId { get; init; }
}