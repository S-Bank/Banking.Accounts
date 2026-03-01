using Banking.Accounts.Abstractions.Infrastructure.Storage.Repositories;
using Banking.Accounts.Abstractions.Logic.Account;
using Banking.Accounts.Infrastructure.Storage.Context;
using Banking.Accounts.Infrastructure.Storage.Models;
using Banking.Accounts.Models.Account;
using Banking.Accounts.Models.DomainEvents;
using MediatR;
using System.Net.WebSockets;

namespace Banking.Accounts.Infrastructure.Storage.Repositories;

/// <summary>
/// Репозиторий счетов.
/// </summary>
public sealed class AccountRepository :
    IAccountRepository
{
    /// <summary>
    /// Инициализирует новый экземпляр репозитория.
    /// </summary>
    /// <param name="context">
    /// Контекст репозитория для доступа к базе данных.
    /// </param>
    /// <param name="factory">
    /// Фабрика для создания доменных моделей счета.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если любой из обязательных параметров равен null.
    /// </exception>
    public AccountRepository(
        IRepositoryContext context,
        IAccountFactory factory)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(factory);

        _context = context;
        _factory = factory;
    }

    /// <inheritdoc />
    public void AddAccount(IAccount account)
    {
        ArgumentNullException.ThrowIfNull(account);

        _context.Accounts.Add(new Account
        {
            Id = account.Id,
            CustomerId = account.CustomerId,
            Balance = account.Balance,
            Currency = account.Currency,
            Status = account.Status,
        });
    }

    /// <inheritdoc />
    public async Task<IAccount?> FindAsync(AccountId id, CancellationToken token)
    {
        var model = await _context.Accounts.FindAsync(id, token);

        if (model is null)
        {
            return null;
        }

        return _factory.Create(
            model.Id,
            model.CustomerId,
            model.Balance,
            model.Currency,
            model.Status);
    }

    /// <inheritdoc />
    public async Task UpdateAsync(IAccount account, CancellationToken token)
    {
        var model = await _context.Accounts.FindAsync([account.Id], token);

        if (model is null)
        {
            throw new KeyNotFoundException($"Не нашелся счет с таким id {account.Id.Value}");
        }

        model.Balance = account.Balance;
        model.Status = account.Status;

        model.Version++;
    }

    private readonly IRepositoryContext _context;
    private readonly IAccountFactory _factory;
}
