using Banking.Accounts.Models.Account;
using Banking.Accounts.Models.Transaction;

namespace Banking.Accounts.Infrastructure.Storage.Models;

/// <summary>
/// Модель банковской операции.
/// </summary>
public sealed class Transaction
{
    /// <summary>
    /// Внутренний ID транзакции.
    /// </summary>
    public required TransactionId Id { get; init; }

    /// <summary>
    /// Ссылка на счет, к которому относится транзакция.
    /// </summary>
    public required AccountId AccountId { get; init; }

    /// <summary>
    /// Внешний ID.
    /// </summary>
    public required ReferenceId ReferenceId { get; init; }

    /// <summary>
    /// Сумма операции.
    /// </summary>
    public required Amount Amount { get; init; }

    /// <summary>
    /// Тип операции.
    /// </summary>
    public required TransactionType Type { get; init; }

    /// <summary>
    /// Время совершения операции.
    /// </summary>
    public required DateTimeOffset CreatedAt { get; init; }
}
