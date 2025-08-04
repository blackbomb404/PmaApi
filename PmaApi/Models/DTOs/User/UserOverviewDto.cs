namespace Pma.Models.DTOs.User;

public record UserOverviewDto
{
    public long Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public string? PhotoUrl { get; init; }
}