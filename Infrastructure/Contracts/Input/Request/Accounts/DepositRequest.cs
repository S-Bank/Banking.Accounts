using Banking.Accounts.Models.Account;
using System.Text.Json.Serialization;

namespace Banking.Accounts.Infrastructure.Contracts.Input.Request.Accounts;

/// <summary>
/// Запрос на пополнение банковского счета.
/// </summary>
public sealed class DepositRequest
{
    /// <summary>
    /// Сумма, которую необходимо зачислить на счет.
    /// </summary>
    [JsonPropertyName("amount")]
    public required Amount Amount { get; set; }
}