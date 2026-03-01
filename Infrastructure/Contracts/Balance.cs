using Banking.Accounts.Models.Account;
using System.Text.Json.Serialization;

namespace Banking.Accounts.Infrastructure.Contracts;

public sealed class Balance
{
    [JsonPropertyName("value")]
    public long Value { get; init; }

    [JsonPropertyName("currency")]
    public required Currency Currency { get; set; }
}
