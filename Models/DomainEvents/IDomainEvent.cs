using MediatR;

namespace Banking.Accounts.Models.DomainEvents;

/// <summary>
/// Базовый интерфейс для доменных событий.
/// </summary>
public interface IDomainEvent : INotification { }