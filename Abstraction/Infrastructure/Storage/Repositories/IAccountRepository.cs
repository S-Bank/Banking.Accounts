using Banking.Accounts.Abstractions.Logic.Account;
using Banking.Accounts.Models.Account;

namespace Banking.Accounts.Abstractions.Infrastructure.Storage.Repositories;

/// <summary>
/// Интерфейс репозитория для управления банковскими счетами.
/// </summary>
public interface IAccountRepository
{

    /// <summary>
    /// Добавляет новый счет в репозиторий.
    /// </summary>
    /// <param name="account">
    /// Экземпляр счета для добавления.
    /// </param>
    void AddAccount(IAccount account);


    /// <summary>
    /// Выполняет асинхронный поиск счета по его идентификатору.
    /// </summary>
    /// <param name="id">
    /// Идентификатор искомого счета.
    /// </param>
    /// <param name="token">
    /// Токен отмены операции.
    /// </param>
    /// <returns>
    /// Экземпляр счета или null, если счет не найден.
    /// </returns>
    Task<IAccount?> FindAsync(AccountId id, CancellationToken token);

    /// <summary>
    /// Обновляет счет.
    /// </summary>
    /// <param name="account">
    /// Счет.
    /// </param>
    /// <param name="token">
    /// Токен отмены операции.
    /// </param>
    /// <returns></returns>
    Task UpdateAsync(IAccount account, CancellationToken token); 
}
