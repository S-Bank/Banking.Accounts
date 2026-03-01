using System.Text.Json.Serialization;

namespace Banking.Accounts.Infrastructure.Contracts.Output.Response.Account;

public sealed class CreateAccountResponse
{

    [JsonPropertyName("accountId")]
    public Guid AccountId { get; set; }
}
