using Banking.Accounts.Infrastructure.Contracts.Models;
using Banking.Accounts.Models.Transaction;
using System.Text.Json.Serialization;

namespace Banking.Accounts.Infrastructure.Contracts.Output.SystemEvents;

/// <summary>
/// Событие, описывающее изменение баланса счета.
/// </summary>
public sealed class AccountBalanceChanged
{
    /// <summary>
    /// Уникальный идентификатор счета.
    /// </summary>
    [JsonPropertyName("account_id")]
    public required Guid AccountId { get; init; }

    /// <summary>
    /// Уникальный ключ идемпотентности операции.
    /// </summary>
    [JsonPropertyName("reference_id")]
    public required Guid ReferenceId { get; init; }

    /// <summary>
    /// Сумма и валюта операции.
    /// </summary>
    [JsonPropertyName("amount")]
    public required Amount Amount { get; init; }

    /// <summary>
    /// Тип транзакции.
    /// </summary>
    [JsonPropertyName("type")]
    public required TransactionType Type { get; init; }

    /// <summary>
    /// Новый баланс счета после операции.
    /// </summary>
    [JsonPropertyName("new_balance")]
    public required Balance NewBalance { get; init; }

}