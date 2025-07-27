using Microsoft.AspNetCore.Http.HttpResults;

namespace PmaApi.Models.Domain;

public enum TaskStatus
{
    Created,
    Assigned,
    InProgress,
    OnHold,
    Cancelled,
    WaitingForAproval,
    Completed
}