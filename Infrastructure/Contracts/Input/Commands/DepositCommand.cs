using System.Text.Json.Serialization;
using Banking.Accounts.Infrastructure.Contracts.Models;
using Banking.Accounts.Models.Transaction;

namespace Banking.Accounts.Infrastructure.Contracts.Input.Commands;

public class DepositCommand
{
    [JsonPropertyName("reference_id")]
    public required Guid ReferenceId { get; init; }

    /// <summary>
    /// Сумма, которую необходимо зачислить на счет.
    /// </summary>
    [JsonPropertyName("amount")]
    public required Amount Amount { get; init; }
}
