using Banking.Accounts.Abstractions.Logic.Account;
using Banking.Accounts.Models.Account;

namespace Banking.Accounts.Logic.Account;

/// <summary>
/// Фабрика для создания экземпляров банковского счёта.
/// </summary>
public sealed class AccountFactory : IAccountFactory
{
    /// </inheritdoc>
    public IAccount Create(AccountId id, CustomerId customerId, Balance balance, Currency currency, Status status)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(customerId);
        ArgumentNullException.ThrowIfNull(balance);

        if (!Enum.IsDefined(typeof(Currency), currency))
        {
            throw new ArgumentException("Некорректное значение валюты.", nameof(currency));
        }

        if (!Enum.IsDefined(typeof(Status), status))
        {
            throw new ArgumentException("Некорректное значение статуса.", nameof(status));
        }

        return new Account(
            id,
            customerId,
            balance,
            currency,
            status);
    }

    /// </inheritdoc>
    public IAccount CreateNew(CustomerId customerId, Currency currency)
    {
        ArgumentNullException.ThrowIfNull(customerId);

        if (!Enum.IsDefined(typeof(Currency), currency))
        {
            throw new ArgumentException("Некорректное значение валюты.", nameof(currency));
        }

        return new Account(
            new AccountId(Guid.NewGuid()),
            customerId,
            new Balance(0),
            currency,
            Status.Active);
    }
}