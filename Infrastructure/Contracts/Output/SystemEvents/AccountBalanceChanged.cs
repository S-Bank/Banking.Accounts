using Banking.Accounts.Models.Transaction;
using System.Text.Json.Serialization;

namespace Banking.Accounts.Infrastructure.Contracts.Output.SystemEvents;

public sealed class AccountBalanceChanged
{
    [JsonPropertyName("account_id")]
    public required Guid AccountId { get; init; }

    [JsonPropertyName("reference_id")]
    public required Guid ReferenceId { get; init; }

    [JsonPropertyName("amount")]
    public required Amount Amount { get; init; }

    [JsonPropertyName("type")]
    public required TransactionType Type { get; init; }

    [JsonPropertyName("new_balance")]
    public required Balance NewBalance { get; init; }

}