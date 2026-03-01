using Banking.Accounts.Abstractions.Infrastructure.Transport.Serializer;
using Banking.Accounts.Infrastructure.Contracts.Output.SystemEvents;
using Banking.Accounts.Models.DomainEvents;
using System.Text.Json;

namespace Banking.Accounts.Infrastructure.Transport.Serializer;

/// <summary>
/// Реализация сериализатора событий.
/// </summary>
public class SystemEventSerializer : ISystemEventSerializer
{
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        WriteIndented = false
    };

    /// <inheritdoc />
    public string Serialize<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : IDomainEvent
    {
        return domainEvent switch
        {
            AccountBalanceChangedEvent e => MapEvent(e),
            _ => throw new NotSupportedException($"Event type {typeof(TDomainEvent).Name} is not supported.")
        };
    }

    private string MapEvent(AccountBalanceChangedEvent domainEvent)
    {
        var contract = new AccountBalanceChanged
        {
            AccountId = domainEvent.AccountId.Value,
            ReferenceId = domainEvent.ReferenceId.Value,
            Amount = new()
            {
                Currency = domainEvent.Amount.Currency,
                Value = domainEvent.Amount.Units
            },
            Type = domainEvent.Type,
            NewBalance = new()
            {
                Value = domainEvent.NewBalance.Value,
                Currency = domainEvent.Amount.Currency
            }
        };

        return JsonSerializer.Serialize(contract, _options);
    }
}