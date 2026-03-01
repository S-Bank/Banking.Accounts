using Banking.Accounts.Models.Account;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Banking.Accounts.Infrastructure.Contracts;

public sealed class Amount
{
    [JsonPropertyName("value")]
    public required long Value { get; set; }

    [JsonPropertyName("currency")]
    public required Currency Currency { get; set; }
}