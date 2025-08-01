using System.ComponentModel.DataAnnotations;
using PmaApi.Models.Domain;

namespace Pma.Models.DTOs.User;

public record UserOutputDto
{
    public required long Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public string? PhoneNumber { get; init; }
    public required string Email { get; init; }
    public required string? PhotoUrl { get; init; }
    public required string JobRoleName { get; set; }
    public required string AccessRoleName { get; set; }
}