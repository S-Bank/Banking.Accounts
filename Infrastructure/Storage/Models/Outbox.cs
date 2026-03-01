using Banking.Accounts.Models.Outbox;

namespace Banking.Accounts.Infrastructure.Storage.Models;

public sealed class Outbox
{
    public required OutboxId Id { get; init; }

    public required string Type { get; init; }

    public required string Content { get; init; }

    public required DateTimeOffset OccurredOn { get; init; }

    public required DateTimeOffset? ProcessedOn { get; set; }

    public required string? Error { get; set; }

    public required int ErrorCount { get; set; }
}
