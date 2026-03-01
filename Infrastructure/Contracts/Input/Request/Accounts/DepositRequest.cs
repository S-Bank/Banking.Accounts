using Banking.Accounts.Models.Account;
using System.Text.Json.Serialization;

namespace Banking.Accounts.Infrastructure.Contracts.Input.Request.Accounts;

public sealed class DepositRequest
{
    [JsonPropertyName("amount")]
    public required Amount Amount { get; set; }
}