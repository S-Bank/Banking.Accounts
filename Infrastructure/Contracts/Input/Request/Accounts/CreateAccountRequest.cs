using Banking.Accounts.Models.Account;
using System.Text.Json.Serialization;

namespace Banking.Accounts.Infrastructure.Contracts.Input.Request.Accounts;

public sealed class CreateAccountRequest
{
    [JsonPropertyName("customerId")]
    public required Guid CustomerId { get; set; }

    [JsonPropertyName("currency")]
    public required Currency Currency { get; set; }
}
