using Banking.Accounts.Models.Account;

namespace Banking.Accounts.Models.Transaction;

/// <summary>
/// Модель, представляющая финансовую транзакцию по банковскому счету.
/// </summary>
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

    /// <summary>
    /// Инициализирует новый экземпляр транзакции.
    /// </summary>
    /// <param name="id">
    /// Внутренний идентификатор.
    /// </param>
    /// <param name="accountId">
    /// Идентификатор счета.
    /// </param>
    /// <param name="referenceId">
    /// Внешний идентификатор.
    /// </param>
    /// <param name="amount">
    /// Сумма операции.
    /// </param>
    /// <param name="type">
    /// Тип операции
    /// </param>
    /// <param name="createdAt">
    /// Время совершения.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если обязательные параметры равны null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Выбрасывается, если тип транзакции некорректен.
    /// </exception>
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