using Banking.Accounts.Models.DomainEvents;

namespace Banking.Accounts.Abstractions.Infrastructure.Transport.Serializer;

public interface ISystemEventSerializer
{
    string Serialize<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : IDomainEvent;
}
