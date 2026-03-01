using Banking.Accounts.Models.Account;
using Banking.Accounts.Models.Transaction;

namespace Banking.Accounts.Models.DomainEvents;

/// <summary>
/// Событие изменения баланса счета.
/// </summary>
/// <param name="AccountId">
/// Идентификатор счета.
/// </param>
/// <param name="ReferenceId">
/// Внешний идентификатор операции.
/// </param>
/// <param name="Amount">
/// Сумма операции.
/// </param>
/// <param name="Type">
/// Тип транзакции.
/// </param>
/// <param name="NewBalance">
/// Новое состояние баланса после изменения.
/// </param>
public record AccountBalanceChangedEvent(
    AccountId AccountId,
    ReferenceId ReferenceId,
    Amount Amount,
    TransactionType Type,
    Balance NewBalance) : IDomainEvent;