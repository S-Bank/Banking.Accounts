using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Banking.Accounts.Infrastructure.Contracts.Input.Request.Accounts;

public class WithdrawRequest
{
    [JsonPropertyName("amount")]
    public required Amount Amount { get; set; }
}
