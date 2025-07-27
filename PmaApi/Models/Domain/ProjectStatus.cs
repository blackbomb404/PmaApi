namespace PmaApi.Models.Domain;

public enum ProjectStatus : byte
{
    Proposed,
    Created,
    InProgress,
    OnHold,
    Cancelled,
    Completed
}