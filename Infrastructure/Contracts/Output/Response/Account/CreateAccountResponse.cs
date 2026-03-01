using System.Text.Json.Serialization;

namespace Banking.Accounts.Infrastructure.Contracts.Output.Response.Account;

/// <summary>
/// Ответ сервера на запрос создания нового банковского счета.
/// Содержит уникальный идентификатор созданного счета.
/// </summary>
public sealed class CreateAccountResponse
{
    /// <summary>
    /// Уникальный идентификатор нового счета.
    /// </summary>
    [JsonPropertyName("accountId")]
    public Guid AccountId { get; set; }
}
