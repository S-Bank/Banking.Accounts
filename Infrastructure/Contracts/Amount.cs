using Banking.Accounts.Models.Account;
using System.Text.Json.Serialization;

namespace Banking.Accounts.Infrastructure.Contracts;

/// <summary>
/// Представляет сумму финансовой операции.
/// </summary>
public sealed class Amount
{
    /// <summary>
    /// Числовое значение суммы.
    /// </summary>
    [JsonPropertyName("value")]
    public required long Value { get; set; }

    /// <summary>
    /// Валюта.
    /// </summary>
    [JsonPropertyName("currency")]
    public required Currency Currency { get; set; }
}