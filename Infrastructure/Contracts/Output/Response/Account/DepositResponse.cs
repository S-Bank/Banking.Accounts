using System.Text.Json.Serialization;

namespace Banking.Accounts.Infrastructure.Contracts.Output.Response.Account;

/// <summary>
/// Ответ сервера на запрос пополнения счета.
/// </summary>
public sealed class DepositResponse
{
    /// <summary>
    /// Текущий баланс счета после выполнения операции.
    /// </summary>
    [JsonPropertyName("balance")]
    public long Balance { get; set; }
}