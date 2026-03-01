using Banking.Accounts.Models.DomainEvents;

namespace Banking.Accounts.Abstractions.Infrastructure.Transport.Serializer;

/// <summary>
/// Описывает сериализатор для преобразования доменных событий в строковый формат.
/// </summary>
public interface ISystemEventSerializer
{
    /// <summary>
    /// Сериализует доменное событие в строку.
    /// </summary>
    /// <typeparam name="TDomainEvent">
    /// Тип доменного события, реализующий <see cref="IDomainEvent"/>.
    /// </typeparam>
    /// <param name="domainEvent">
    /// Экземпляр события.
    /// </param>
    /// <returns>
    /// Сериализованная строка события.
    /// </returns>
    string Serialize<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : IDomainEvent;
}
