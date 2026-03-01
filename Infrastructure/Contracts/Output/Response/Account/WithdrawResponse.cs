using System.Text.Json.Serialization;

namespace Banking.Accounts.Infrastructure.Contracts.Output.Response.Account;

// <summary>
/// Ответ сервера на запрос снятия средств со счета.
/// Содержит актуальный остаток после операции.
/// </summary>
public sealed class WithdrawResponse
{
    /// <summary>
    /// Текущий баланс счета после выполнения операции.
    /// </summary>
    [JsonPropertyName("balance")]
    public long Balance { get; set; }
}
