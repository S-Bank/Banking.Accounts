using Banking.Accounts.Models.Account;
using System.Text.Json.Serialization;

namespace Banking.Accounts.Infrastructure.Contracts.Input.Request.Accounts;

/// <summary>
/// Запрос на создание нового банковского счета для клиента.
/// </summary>
public sealed class CreateAccountRequest
{
    /// <summary>
    /// Уникальный идентификатор клиента, для которого открывается счет.
    /// </summary>
    [JsonPropertyName("customerId")]
    public required Guid CustomerId { get; set; }

    /// <summary>
    /// Валюта открываемого счета.
    /// </summary>
    [JsonPropertyName("currency")]
    public required Currency Currency { get; set; }
}
