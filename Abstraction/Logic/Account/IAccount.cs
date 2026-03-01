using Banking.Accounts.Models.Account;
using Banking.Accounts.Models.DomainEvents;
using Banking.Accounts.Models.Exceptions;
using Banking.Accounts.Models.Transaction;

namespace Banking.Accounts.Abstractions.Logic.Account;

/// <summary>
/// Интерфейс агрегата банковского счёта.
/// </summary>
public interface IAccount
{
    /// <summary>
    /// Идентификатор банковского счёта.
    /// </summary>
    AccountId Id { get; }

    /// <summary>
    /// Идентификатор клиента.
    /// </summary>
    CustomerId CustomerId { get; }

    /// <summary>
    /// Текущий баланс счёта.
    /// </summary>
    Balance Balance { get; }

    /// <summary>
    /// Валюта счёта.
    /// </summary>
    Currency Currency { get; }

    /// <summary>
    /// Текущий статус счёта.
    /// </summary>
    Status Status { get; }

    /// <summary>
    /// Список доменных событий.
    /// </summary>
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    /// <summary>
    /// Очистить доменные события.
    /// </summary>
    void ClearDomainEvents();

    /// <summary>
    /// Выполняет пополнение счета.
    /// </summary>
    /// <param name="amount">
    /// Объект значения суммы пополнения.
    /// </param>
    /// <param name="referenceId">
    /// Внешний идентификатор операции.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если параметры равны null.
    /// </exception>
    /// <exception cref="AccountDomainException">
    /// Выбрасывается при нарушении бизнес-правил (неверный статус или валюта).
    /// </exception>
    void Deposit(Amount amount, ReferenceId referenceId);

    /// <summary>
    /// Выполняет списание средств со счета.
    /// </summary>
    /// <param name="amount">
    /// Объект значения суммы списания.
    /// </param>
    /// <param name="referenceId">
    /// Внешний идентификатор операции.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если параметры равны null.
    /// </exception>
    /// <exception cref="AccountDomainException">
    /// Выбрасывается при недостаточном балансе или неверном статусе.
    /// </exception>
    void Withdraw(Amount amount, ReferenceId referenceId);
}