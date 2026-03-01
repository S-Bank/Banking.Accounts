using Banking.Accounts.Models.Account;

namespace Banking.Accounts.Models.Transaction;

public sealed class Transaction
{
    /// <summary>
    /// Внутренний ID транзакции.
    /// </summary>
    public TransactionId Id { get; private set; }

    /// <summary>
    /// Ссылка на счет, к которому относится транзакция.
    /// </summary>
    public AccountId AccountId { get; private set; }

    /// <summary>
    /// Внешний ID.
    /// </summary>
    public ReferenceId ReferenceId { get; private set; }

    /// <summary>
    /// Сумма операции.
    /// </summary>
    public Amount Amount { get; private set; }

    /// <summary>
    /// Тип операции.
    /// </summary>
    public TransactionType Type { get; private set; }

    /// <summary>
    /// Время совершения операции.
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }

    public Transaction(
        TransactionId id,
        AccountId accountId,
        ReferenceId referenceId,
        Amount amount,
        TransactionType type,
        DateTimeOffset createdAt)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(accountId);
        ArgumentNullException.ThrowIfNull(referenceId);
        ArgumentNullException.ThrowIfNull(amount);

        if (!Enum.IsDefined(typeof(TransactionType), type))
        {
            throw new ArgumentOutOfRangeException(nameof(type));
        }

        Id = id;
        AccountId = accountId;
        ReferenceId = referenceId;
        Amount = amount;
        Type = type;
        CreatedAt = createdAt;
    }
}