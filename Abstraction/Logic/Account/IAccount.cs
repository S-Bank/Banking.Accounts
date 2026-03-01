using Banking.Accounts.Models.Account;
using Banking.Accounts.Models.DomainEvents;
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
    /// Список домменных событий.
    /// </summary>
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    /// <summary>
    /// Отчистить доменные события.
    /// </summary>
    void ClearDomainEvents();

    /// <summary>
    /// Выполняет пополнение счета.
    /// </summary>
    /// <param name="amount">
    /// Объект значения суммы пополнения.
    /// </param>
    /// <param name="referenceId">
    /// Идентификатор транзакции.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если обязательные параметры равны null.
    /// </exception>
    void Deposit(Amount amount, ReferenceId referenceId);

    /// <summary>
    /// Выполняет списание средств со счета.
    /// </summary>
    /// <param name="amount">
    /// Объект значения суммы списания.
    /// </param>
    /// <param name="referenceId">
    /// Идентификатор транзакции.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если обязательные параметры равны null.
    /// </exception>
    void Withdraw(Amount amount, ReferenceId referenceId);
}