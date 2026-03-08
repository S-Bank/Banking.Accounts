using Banking.Accounts.Models.Account;
using System.Text.Json.Serialization;

namespace Banking.Accounts.Infrastructure.Contracts.Models;

/// <summary>
/// Представляет состояние баланса счета.
/// </summary>
public sealed class Balance
{
    /// <summary>
    /// Сумма баланса.
    /// </summary>
    [JsonPropertyName("value")]
    public long Value { get; init; }

    /// <summary>
    /// Валюта баланса.
    /// </summary>
    [JsonPropertyName("currency")]
    public required Currency Currency { get; set; }
}
