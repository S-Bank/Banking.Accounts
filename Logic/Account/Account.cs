using Banking.Accounts.Abstractions.Logic.Account;
using Banking.Accounts.Models.Account;
using Banking.Accounts.Models.DomainEvents;
using Banking.Accounts.Models.Exceptions;
using Banking.Accounts.Models.Transaction;

namespace Banking.Accounts.Logic.Account;

/// <summary>
/// Банковский счёт.
/// </summary>
public sealed class Account : IAccount
{
    /// </inheritdoc>
    public AccountId Id { get; private set; }

    /// </inheritdoc>
    public CustomerId CustomerId { get; private set; }

    /// </inheritdoc>
    public Balance Balance { get; private set; }

    /// </inheritdoc>
    public Currency Currency { get; private set; }

    /// </inheritdoc>
    public Status Status { get; private set; }

    /// </inheritdoc>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Создает экземпляр банковского счёта.
    /// Используется для восстановления агрегата из хранилища.
    /// </summary>
    /// <param name="id">
    /// Идентификатор счёта.
    /// </param>
    /// <param name="customerId">
    /// Идентификатор владельца счёта.
    /// </param>
    /// <param name="balance">
    /// Текущий баланс.
    /// </param>
    /// <param name="currency">
    /// Валюта счёта.
    /// </param>
    /// <param name="status">
    /// Статус счёта.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если обязательные параметры равны null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Выбрасывается, если переданы недопустимые значения перечислений.
    /// </exception>
    public Account(AccountId id, CustomerId customerId, Balance balance, Currency currency, Status status)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(customerId);
        ArgumentNullException.ThrowIfNull(balance);

        if (!Enum.IsDefined(typeof(Currency), currency))
        {
            throw new ArgumentOutOfRangeException(nameof(currency));
        }

        if (!Enum.IsDefined(typeof(Status), status))
        {
            throw new ArgumentOutOfRangeException(nameof(status));
        }

        Id = id;
        CustomerId = customerId;
        Balance = balance;
        Currency = currency;
        Status = status;
    }

    /// </inheritdoc>
    public void Deposit(Amount amount, ReferenceId referenceId)
    {
        ArgumentNullException.ThrowIfNull(amount);
        ArgumentNullException.ThrowIfNull(referenceId);

        EnsureAccountIsActive();

        EnsureSameCurrency(amount.Currency);

        Balance = Balance.Add(amount);

        AddDomainEvent(new AccountBalanceChangedEvent(Id, referenceId, amount, TransactionType.Deposit, Balance));
    }

    /// </inheritdoc>
    public void Withdraw(Amount amount, ReferenceId referenceId)
    {
        ArgumentNullException.ThrowIfNull(amount);
        ArgumentNullException.ThrowIfNull(referenceId);

        EnsureAccountIsActive();

        EnsureSameCurrency(amount.Currency);

        Balance = Balance.Subtract(amount);

        AddDomainEvent(new AccountBalanceChangedEvent(Id, referenceId, amount, TransactionType.Withdraw, Balance));
    }

    /// </inheritdoc>
    public void ClearDomainEvents() => _domainEvents.Clear();

    private void AddDomainEvent(IDomainEvent domainEvent)
        => _domainEvents.Add(domainEvent);


    private void EnsureAccountIsActive()
    {
        if (Status is not Status.Active)
        {
            throw new AccountDomainException($"Operation is not allowed. Account {Id} has status: {Status}");
        }
    }

    private void EnsureSameCurrency(Currency operationalCurrency)
    {
        if (operationalCurrency != Currency)
        {
            throw new AccountDomainException(
                $"Currency mismatch: Operation ({operationalCurrency}) vs Account ({Currency}).");
        }
    }

    private readonly List<IDomainEvent> _domainEvents = [];
}