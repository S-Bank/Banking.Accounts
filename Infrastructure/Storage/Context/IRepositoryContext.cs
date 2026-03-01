using Banking.Accounts.Infrastructure.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace Banking.Accounts.Infrastructure.Storage.Context;

/// <summary>
/// Интерфейс контекста репозитория для доступа к данным.
/// </summary>
public interface IRepositoryContext
{
    /// <summary>
    /// Набор данных банковских счетов.
    /// </summary>
    DbSet<Account> Accounts { get; }

    /// <summary>
    /// Набор данных транзакций.
    /// </summary>
    DbSet<Transaction> Transactions { get; }

    DbSet<Outbox> Outbox { get; }

    /// <summary>
    /// Сохраняет все изменения, внесенные в контекст, в базу данных.
    /// </summary>
    /// <param name="cancellationToken">
    /// Токен отмены операции.
    /// </param>
    /// <returns>
    /// Задача, представляющая асинхронную операцию сохранения.
    /// </returns>
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
