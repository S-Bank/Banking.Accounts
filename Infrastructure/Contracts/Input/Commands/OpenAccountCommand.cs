using System.Text.Json.Serialization;
using Banking.Accounts.Models.Account;

namespace Banking.Accounts.Infrastructure.Contracts.Input.Commands;

public sealed class OpenAccountCommand
{
    /// <summary>
    /// Идентификатор счета.
    /// </summary>
    public required Guid AccountId { get; init; }

    /// <summary>
    /// Уникальный идентификатор клиента, для которого открывается счет.
    /// </summary>
    [JsonPropertyName("customerId")]
    public required Guid CustomerId { get; init; }

    /// <summary>
    /// Валюта открываемого счета.
    /// </summary>
    [JsonPropertyName("currency")]
    public required Currency Currency { get; init; }
}
