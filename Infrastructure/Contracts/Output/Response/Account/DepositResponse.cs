using System.Text.Json.Serialization;

namespace Banking.Accounts.Infrastructure.Contracts.Output.Response.Account;

public sealed class DepositResponse
{
    [JsonPropertyName("balance")]
    public long Balance { get; set; }
}
