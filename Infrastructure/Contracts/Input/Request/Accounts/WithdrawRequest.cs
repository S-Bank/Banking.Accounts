using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Banking.Accounts.Infrastructure.Contracts.Input.Request.Accounts;

/// <summary>
/// Запрос на снятие средств со счета.
/// </summary>
public class WithdrawRequest
{
    /// <summary>
    /// Сумма, которую необходимо снять со счета.
    /// </summary>
    [JsonPropertyName("amount")]
    public required Amount Amount { get; set; }
}
